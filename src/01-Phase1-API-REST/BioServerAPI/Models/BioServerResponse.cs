namespace BioServerAPI.Models;

public class BioServerResponse
{
    public bool Success { get; set; }
    public string Data { get; set; }
    public string Error { get; set; }
    public string ErrorDetail { get; set; }
    public long ElapsedMilliseconds { get; set; }
}