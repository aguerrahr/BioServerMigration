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
    private static extern string _SendToServer(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string _ServerFind(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string _FindFinger(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string _FindPalm(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string _FindFace(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string _FindIris(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string _FindVoice(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string _ServerSave(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string _ServerFlush(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string _GetBioKey(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string _GetAppKey(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string _GetDataBioKey(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string _GetDataMapBioKey(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string _GetDataServer(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string _GetDataMapServer(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string _ServerDelete(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string _ServerFuse(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string _Special(string id, string secret, string payload);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    private static extern string _ServerCompare(string id, string secret, string payload);

    // ============================================================
    // MÉTODOS PÚBLICOS (Wrapper)
    // ============================================================

    public string SendToServer(string id, string secret, string payload)
        => _SendToServer(id, secret, payload);

    public string ServerFind(string id, string secret, string payload)
        => _ServerFind(id, secret, payload);

    public string FindFinger(string id, string secret, string payload)
        => _FindFinger(id, secret, payload);

    public string FindPalm(string id, string secret, string payload)
        => _FindPalm(id, secret, payload);

    public string FindFace(string id, string secret, string payload)
        => _FindFace(id, secret, payload);

    public string FindIris(string id, string secret, string payload)
        => _FindIris(id, secret, payload);

    public string FindVoice(string id, string secret, string payload)
        => _FindVoice(id, secret, payload);

    public string ServerSave(string id, string secret, string payload)
        => _ServerSave(id, secret, payload);

    public string ServerFlush(string id, string secret, string payload)
        => _ServerFlush(id, secret, payload);

    public string GetBioKey(string id, string secret, string payload)
        => _GetBioKey(id, secret, payload);

    public string GetAppKey(string id, string secret, string payload)
        => _GetAppKey(id, secret, payload);

    public string GetDataBioKey(string id, string secret, string payload)
        => _GetDataBioKey(id, secret, payload);

    public string GetDataMapBioKey(string id, string secret, string payload)
        => _GetDataMapBioKey(id, secret, payload);

    public string GetDataServer(string id, string secret, string payload)
        => _GetDataServer(id, secret, payload);

    public string GetDataMapServer(string id, string secret, string payload)
        => _GetDataMapServer(id, secret, payload);

    public string ServerDelete(string id, string secret, string payload)
        => _ServerDelete(id, secret, payload);

    public string ServerFuse(string id, string secret, string payload)
        => _ServerFuse(id, secret, payload);

    public string Special(string id, string secret, string payload)
        => _Special(id, secret, payload);

    public string ServerCompare(string id, string secret, string payload)
        => _ServerCompare(id, secret, payload);

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