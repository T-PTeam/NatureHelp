using Microsoft.AspNetCore.Http;

namespace NatureHelp.Security;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _environment;

    public SecurityHeadersMiddleware(RequestDelegate next, IWebHostEnvironment environment)
    {
        _next = next;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

        context.Response.Headers.Append("X-Frame-Options", "DENY");

        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");

        context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");

        if (!_environment.IsDevelopment())
        {
            context.Response.Headers.Append("Content-Security-Policy",
                "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval'; style-src 'self' 'unsafe-inline'; img-src 'self' data: https:; font-src 'self' data:;");
        }

        context.Response.Headers.Append("Permissions-Policy",
            "geolocation=(), microphone=(), camera=()");

        await _next(context);
    }
}

