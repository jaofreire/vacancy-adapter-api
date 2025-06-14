using CurriculumAdapter.API.Data.Context;
using CurriculumAdapter.API.Data.Repositories;
using CurriculumAdapter.API.Data.Repositories.Interfaces;
using CurriculumAdapter.API.Middleware;
using CurriculumAdapter.API.Services;
using CurriculumAdapter.API.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira 'Bearer' [espaço] e o seu token JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

string connectionStrings = Environment.GetEnvironmentVariable("DATABASE_CONNECTION") ?? builder.Configuration["DatabaseConnection:ConnectionStrings"]!;

builder.Services.AddDbContextPool<DatabaseContext>(o => o.UseNpgsql(connectionStrings));

string qdrantHost = Environment.GetEnvironmentVariable("QDRANT_HOST") ?? builder.Configuration["Qdrant:Host"]!;

builder.Services.AddSingleton<QdrantContext>(new QdrantContext(qdrantHost));

builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IJobsCollectionRepository, JobsCollectionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFeatureUsageLogRepository, FeatureUsageLogRepository>();

builder.Services.AddScoped<IAdaptService, AdaptService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IAdvisorService, AdvisorService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

string secret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? builder.Configuration["JWT:Secret"]!;

var key = Encoding.ASCII.GetBytes(secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SubscriberPolicy", policy => policy.RequireRole("Subscriber"));

    options.AddPolicy("EveryoneHasAccessPolicy", policy => policy.RequireAssertion(context => context.User.HasClaim(c =>
    (c.Type == ClaimTypes.Role && c.Value == "Default") ||
    (c.Type == ClaimTypes.Role && c.Value == "Subscriber")
    )));

});


builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("RateLimitPolicy", context =>
    RateLimitPartition.GetFixedWindowLimiter(partitionKey: context.Connection.RemoteIpAddress.ToString(), factory: key => new FixedWindowRateLimiterOptions
    {
        AutoReplenishment = true,
        PermitLimit = 5,
        Window = TimeSpan.FromSeconds(10),
        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
        QueueLimit = 5,
    }));
});

builder.Services.AddCors(c =>
{
    c.AddPolicy("CorsPolicy", p =>
    {
        p.WithOrigins
        (
            builder.Configuration["CORS:ClientUrl"] ?? Environment.GetEnvironmentVariable("CORS_CLIENT_URL")!,
            builder.Configuration["CORS:LocalhostUrl"] ?? Environment.GetEnvironmentVariable("CORS_LOCALHOST_URL")!
        );
        p.WithMethods("GET", "POST");
        p.AllowAnyHeader();
        p.AllowCredentials();
    });
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CorsPolicy");

app.UseRateLimiter();

app.UseMiddleware<ExceptionHandler>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers().RequireRateLimiting("RateLimitPolicy");

app.Run();
