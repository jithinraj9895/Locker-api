using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

var connectionString = builder.Configuration.GetConnectionString("PgConnectionString");
// Read env variables with fallback defaults
int permitLimit = int.TryParse(Environment.GetEnvironmentVariable("RATE_LIMIT_PERMIT"), out var p) ? p : 4;
int windowHour = int.TryParse(Environment.GetEnvironmentVariable("RATE_LIMIT_WINDOW_HOURS"), out var w) ? w : 1;
// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddScoped<IVaultRepository, VaultRepository>();

var allowedOrigin = builder.Configuration["FRONTEND_URL"]
                    ?? "http://localhost:5173";

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigin)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add global rate limiti
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("vaultLimit", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 1,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));
});



var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRateLimiter();
app.UseCors("FrontendPolicy");

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
