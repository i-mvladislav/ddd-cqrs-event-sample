using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("YarpProxy"));

builder.Services.AddRateLimiter(option =>
{
    option.AddFixedWindowLimiter("fixed", config =>
    {
        config.PermitLimit = 3;
        config.Window = TimeSpan.FromSeconds(30);
    });
});

var app = builder.Build();

app.UseRateLimiter();
app.MapReverseProxy()
    .RequireRateLimiting("fixed");

await app.RunAsync();