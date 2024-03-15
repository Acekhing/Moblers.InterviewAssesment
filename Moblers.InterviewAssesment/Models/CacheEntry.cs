namespace Moblers.InterviewAssesment.Models;

public class CacheEntry
{
    public string Url { get; set; }
    public byte[] Content { get; set; }
    public DateTimeOffset ExpirationTime { get; set; }
}