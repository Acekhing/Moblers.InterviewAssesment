using Microsoft.AspNetCore.ResponseCompression;
using Moblers.InterviewAssesment.Providers;

namespace Moblers.InterviewAssesment.Middlewares;

public static class ConfigureResponseCompression
{
    public static IServiceCollection RegisterResponseCompressor(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.Providers.Add<LowLatencyCompressorProvider>();
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" });
            //options.EnableForHttps = true;
        });

        return services;
    }
}