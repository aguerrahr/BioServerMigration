namespace BioServerAPI.Models;

public class BioServerRequest
{
    public string Id { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
}