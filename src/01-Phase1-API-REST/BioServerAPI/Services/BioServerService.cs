using System.Diagnostics;
using BioServerAPI.Configuration;
using Microsoft.Extensions.Logging;

namespace BioServerAPI.Services;

public class BioServerService : IDisposable
{
    private readonly BioServerPool _pool;
    private readonly ILogger<BioServerService> _logger;
    private readonly BioServerConfig _config;
    private bool _disposed;

    public BioServerService(BioServerPool pool, ILogger<BioServerService> logger, BioServerConfig config)
    {
        _pool = pool;
        _logger = logger;
        _config = config;
    }

    // ============================================================
    // MÉTODO AUXILIAR (Elimina la duplicación y las advertencias)
    // ============================================================

    private async Task<string> ExecuteWrapperAsync(
    string id,
    string secret,
    string payload,
    Func<BioServerWrapper, string, string, string, string> wrapperMethod,
    string methodName)
    {
        var stopwatch = Stopwatch.StartNew();

        // Validación de payload (LANZA EXCEPCIÓN si es null o vacío)
        if (string.IsNullOrWhiteSpace(payload))
        {
            _logger?.LogWarning("Payload vacío o nulo para {MethodName}, ID: {Id}", methodName, id);
            throw new ArgumentException("El payload no puede estar vacío.", nameof(payload));
        }

        // Validación de instancia (LANZA EXCEPCIÓN si es null)
        var instance = await _pool.GetInstanceAsync();
        if (instance == null)
        {
            _logger?.LogError("No se pudo obtener instancia del pool para {MethodName}, ID: {Id}", methodName, id);
            throw new InvalidOperationException("El pool de BioServer no tiene instancias disponibles.");
        }

        // A partir de aquí, 'payload' y 'instance' NO SON NULL
        // Guardamos los valores en variables locales para que el análisis de nullable lo entienda
        string nonNullPayload = payload; // El análisis de nullable sabe que payload no es null
        BioServerWrapper nonNullInstance = instance; // El análisis de nullable sabe que instance no es null

        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando {MethodName} para ID: {Id}, Payload: {PayloadLength} bytes",
                    methodName, id, payload.Length);
            }

            // Llamada al método del wrapper con valores no nulos
            var result = wrapperMethod(nonNullInstance, id, secret, nonNullPayload);

            stopwatch.Stop();
            _logger?.LogInformation("{MethodName} completado para ID: {Id} en {ElapsedMs}ms",
                methodName, id, stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en {MethodName} para ID: {Id} después de {ElapsedMs}ms",
                methodName, id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    // ============================================================
    // MÉTODOS PRINCIPALES (Async)
    // ============================================================

    public async Task<string> SendToServerAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.SendToServer(idParam, secretParam, payloadParam),
            nameof(SendToServerAsync));

    public async Task<string> ServerFindAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.ServerFind(idParam, secretParam, payloadParam),
            nameof(ServerFindAsync));

    public async Task<string> FindFingerAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.FindFinger(idParam, secretParam, payloadParam),
            nameof(FindFingerAsync));

    public async Task<string> FindPalmAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.FindPalm(idParam, secretParam, payloadParam),
            nameof(FindPalmAsync));

    public async Task<string> FindFaceAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.FindFace(idParam, secretParam, payloadParam),
            nameof(FindFaceAsync));

    public async Task<string> FindIrisAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.FindIris(idParam, secretParam, payloadParam),
            nameof(FindIrisAsync));

    public async Task<string> FindVoiceAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.FindVoice(idParam, secretParam, payloadParam),
            nameof(FindVoiceAsync));

    public async Task<string> ServerSaveAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.ServerSave(idParam, secretParam, payloadParam),
            nameof(ServerSaveAsync));

    public async Task<string> ServerFlushAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.ServerFlush(idParam, secretParam, payloadParam),
            nameof(ServerFlushAsync));

    public async Task<string> GetBioKeyAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.GetBioKey(idParam, secretParam, payloadParam),
            nameof(GetBioKeyAsync));

    public async Task<string> GetAppKeyAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.GetAppKey(idParam, secretParam, payloadParam),
            nameof(GetAppKeyAsync));

    public async Task<string> GetDataBioKeyAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.GetDataBioKey(idParam, secretParam, payloadParam),
            nameof(GetDataBioKeyAsync));

    public async Task<string> GetDataMapBioKeyAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.GetDataMapBioKey(idParam, secretParam, payloadParam),
            nameof(GetDataMapBioKeyAsync));

    public async Task<string> GetDataServerAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.GetDataServer(idParam, secretParam, payloadParam),
            nameof(GetDataServerAsync));

    public async Task<string> GetDataMapServerAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.GetDataMapServer(idParam, secretParam, payloadParam),
            nameof(GetDataMapServerAsync));

    public async Task<string> ServerDeleteAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.ServerDelete(idParam, secretParam, payloadParam),
            nameof(ServerDeleteAsync));

    public async Task<string> ServerFuseAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.ServerFuse(idParam, secretParam, payloadParam),
            nameof(ServerFuseAsync));

    public async Task<string> SpecialAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.Special(idParam, secretParam, payloadParam),
            nameof(SpecialAsync));

    public async Task<string> ServerCompareAsync(string id, string secret, string payload)
        => await ExecuteWrapperAsync(id, secret, payload,
            (wrapper, idParam, secretParam, payloadParam) =>
                wrapper.ServerCompare(idParam, secretParam, payloadParam),
            nameof(ServerCompareAsync));

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
            // El pool se disposa en el contenedor DI
        }
        _disposed = true;
    }
}