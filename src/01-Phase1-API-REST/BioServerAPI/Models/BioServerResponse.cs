namespace BioServerAPI.Models;

public class BioServerResponse
{
    public bool Success { get; set; }
    public string Data { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
    public string ErrorDetail { get; set; } = string.Empty;
    public long ElapsedMilliseconds { get; set; }
}