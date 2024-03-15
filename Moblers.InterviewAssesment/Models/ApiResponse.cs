namespace Moblers.InterviewAssesment.Models;

public class ApiResponse<T>
{
    public ApiResponse(string message, T data)
    {
        Message = message;
        Data = data;
    }

    public ApiResponse()
    {
    }

    public string? Message { get; set; }

    public int Code { get; set; }
    public T? Data { get; set; }  
}