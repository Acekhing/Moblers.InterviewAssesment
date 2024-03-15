using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Moblers.InterviewAssesment.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Moblers.InterviewAssesment.Middlewares;

public static class GlobalExceptionHandlerMiddleware
{
    public static void RegisterExceptionHandler(this IApplicationBuilder app, ILogger<MoblersApp> logger)
    {
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    logger.LogError(contextFeature.Error, "Something went wrong: {Message}", contextFeature.Error?.Message);

                    var respJson = JsonConvert.SerializeObject(new ApiResponse<object>()
                    {
                        Code = (int)HttpStatusCode.InternalServerError,
                        Message = "Ooops, something really bad happened. Please try again later.",
                    }, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

                    context.Response.ContentLength = respJson.Length;

                    await context.Response.WriteAsync(respJson);
                }
            });
        });
    }
}