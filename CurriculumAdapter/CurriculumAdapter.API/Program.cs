using CurriculumAdapter.API.Middleware;
using CurriculumAdapter.API.Services;
using CurriculumAdapter.API.Services.Interface;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAdaptService, AdaptService>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CorsPolicy");

app.UseRateLimiter();

app.UseMiddleware<ExceptionHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers().RequireRateLimiting("RateLimitPolicy");

app.Run();
