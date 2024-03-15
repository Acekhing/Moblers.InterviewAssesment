using System.Collections.Concurrent;
using Moblers.InterviewAssesment.Models;

namespace Moblers.InterviewAssesment.Middlewares;

public class ResponseCachingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ConcurrentDictionary<string, CacheEntry> _cache;

    public ResponseCachingMiddleware(RequestDelegate next)
    {
        _next = next;
        _cache = new ConcurrentDictionary<string, CacheEntry>();
    }
    
    public async Task Invoke(HttpContext context)
    {
        var cacheKey = $"{context.Request.Method}:{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";

        if (_cache.TryGetValue(cacheKey, out CacheEntry cachedEntry) && cachedEntry.ExpirationTime > DateTimeOffset.Now)
        {
            context.Response.ContentType = "application/json";
            await context.Response.Body.WriteAsync(cachedEntry.Content);
        }
        else
        {
            var streamBody = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await _next(context);

                // Cache response
                var content = responseBody.ToArray();
                var expirationTime = DateTimeOffset.Now.AddHours(24); // Expire after 24 hours
                _cache[cacheKey] = new CacheEntry { Url = cacheKey, Content = content, ExpirationTime = expirationTime };

                // Write response to the original stream
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(streamBody);
            }
        }
    }
}