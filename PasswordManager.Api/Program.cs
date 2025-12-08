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

var allowedOrigin = Environment.GetEnvironmentVariable("FRONTEND_URL")
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
    // Global limiter: 10 requests per minute per client IP
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(clientIp, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = permitLimit,                // max 10 requests
            Window = TimeSpan.FromHours(windowHour), // per 1 minute
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0                   // reject excess immediately
        });
    });

    // Response when limit is exceeded
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429; // Too Many Requests
        await context.HttpContext.Response.WriteAsync(
            "Too many requests. Try again 3600s later.", token);
    };
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
