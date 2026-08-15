using System.Runtime.InteropServices;

namespace BioServerAPI.Services;

public class BioServerWrapper : IDisposable
{
    private const string DllName = "libBioServerWrapper.dll";
    private bool _disposed;

    // ============================================================
    // IMPORTACIÓN DE TODAS LAS FUNCIONES DE LA DLL VB6
    // ============================================================

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string SendToServer(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string ServerFind(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string FindFinger(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string FindPalm(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string FindFace(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string FindIris(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string FindVoice(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string ServerSave(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string ServerFlush(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string GetBioKey(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string GetAppKey(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string GetDataBioKey(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string GetDataMapBioKey(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string GetDataServer(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string GetDataMapServer(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string ServerDelete(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string ServerFuse(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string Special(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string ServerCompare(string id, string secret, string payload);

    // ============================================================
    // MÉTODOS PÚBLICOS (Wrapper)
    // ============================================================

    public string SendToServer(string id, string secret, string payload)
        => SendToServer(id, secret, payload);

    public string ServerFind(string id, string secret, string payload)
        => ServerFind(id, secret, payload);

    public string FindFinger(string id, string secret, string payload)
        => FindFinger(id, secret, payload);

    public string FindPalm(string id, string secret, string payload)
        => FindPalm(id, secret, payload);

    public string FindFace(string id, string secret, string payload)
        => FindFace(id, secret, payload);

    public string FindIris(string id, string secret, string payload)
        => FindIris(id, secret, payload);

    public string FindVoice(string id, string secret, string payload)
        => FindVoice(id, secret, payload);

    public string ServerSave(string id, string secret, string payload)
        => ServerSave(id, secret, payload);

    public string ServerFlush(string id, string secret, string payload)
        => ServerFlush(id, secret, payload);

    public string GetBioKey(string id, string secret, string payload)
        => GetBioKey(id, secret, payload);

    public string GetAppKey(string id, string secret, string payload)
        => GetAppKey(id, secret, payload);

    public string GetDataBioKey(string id, string secret, string payload)
        => GetDataBioKey(id, secret, payload);

    public string GetDataMapBioKey(string id, string secret, string payload)
        => GetDataMapBioKey(id, secret, payload);

    public string GetDataServer(string id, string secret, string payload)
        => GetDataServer(id, secret, payload);

    public string GetDataMapServer(string id, string secret, string payload)
        => GetDataMapServer(id, secret, payload);

    public string ServerDelete(string id, string secret, string payload)
        => ServerDelete(id, secret, payload);

    public string ServerFuse(string id, string secret, string payload)
        => ServerFuse(id, secret, payload);

    public string Special(string id, string secret, string payload)
        => Special(id, secret, payload);

    public string ServerCompare(string id, string secret, string payload)
        => ServerCompare(id, secret, payload);

    // ============================================================
    // IDisposable Implementation
    // ============================================================

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            // Liberar recursos manejados (si los hay)
        }
        _disposed = true;
    }
}