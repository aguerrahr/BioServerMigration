namespace BioServerAPI.Configuration;

public class BioServerConfig
{
    public int MaxInstances { get; set; } = 10;
    public int TimeoutSeconds { get; set; } = 30;
    public string DllPath { get; set; } = "libBioServerWrapper.dll";
    public bool EnableDebug { get; set; } = false;
}