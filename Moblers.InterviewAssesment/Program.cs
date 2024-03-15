using Moblers.InterviewAssesment;
using Moblers.InterviewAssesment.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Response Compression
builder.Services.RegisterResponseCompressor();

var app = builder.Build();

// Exception Handling Middleware
app.RegisterExceptionHandler(app.Services.GetService<ILogger<MoblersApp>>() ?? throw new InvalidOperationException());

// Request Logging Middleware
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ResponseCachingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();