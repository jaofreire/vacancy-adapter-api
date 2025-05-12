using CurriculumAdapter.API.Services;
using CurriculumAdapter.API.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAdaptService, AdaptService>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
