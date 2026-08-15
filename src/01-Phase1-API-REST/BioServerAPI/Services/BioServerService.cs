using System.Diagnostics;

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
    // MÉTODOS PRINCIPALES (Async)
    // ============================================================

    public async Task<string> SendToServerAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando SendToServer para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.SendToServer(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("SendToServer completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en SendToServer para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    public async Task<string> ServerFindAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando ServerFind para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.ServerFind(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("ServerFind completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en ServerFind para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    public async Task<string> FindFingerAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando FindFinger para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.FindFinger(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("FindFinger completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en FindFinger para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    public async Task<string> FindPalmAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando FindPalm para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.FindPalm(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("FindPalm completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en FindPalm para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    public async Task<string> FindFaceAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando FindFace para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.FindFace(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("FindFace completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en FindFace para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    public async Task<string> FindIrisAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando FindIris para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.FindIris(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("FindIris completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en FindIris para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    public async Task<string> FindVoiceAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando FindVoice para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.FindVoice(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("FindVoice completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en FindVoice para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    public async Task<string> ServerSaveAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando ServerSave para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.ServerSave(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("ServerSave completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en ServerSave para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    public async Task<string> ServerFlushAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando ServerFlush para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.ServerFlush(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("ServerFlush completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en ServerFlush para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    public async Task<string> GetBioKeyAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando GetBioKey para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.GetBioKey(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("GetBioKey completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en GetBioKey para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    public async Task<string> GetAppKeyAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando GetAppKey para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.GetAppKey(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("GetAppKey completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en GetAppKey para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    public async Task<string> GetDataBioKeyAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando GetDataBioKey para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.GetDataBioKey(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("GetDataBioKey completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en GetDataBioKey para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    public async Task<string> GetDataMapBioKeyAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando GetDataMapBioKey para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.GetDataMapBioKey(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("GetDataMapBioKey completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en GetDataMapBioKey para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    public async Task<string> GetDataServerAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando GetDataServer para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.GetDataServer(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("GetDataServer completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en GetDataServer para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    public async Task<string> GetDataMapServerAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando GetDataMapServer para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.GetDataMapServer(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("GetDataMapServer completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en GetDataMapServer para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    public async Task<string> ServerDeleteAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando ServerDelete para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.ServerDelete(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("ServerDelete completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en ServerDelete para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    public async Task<string> ServerFuseAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando ServerFuse para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.ServerFuse(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("ServerFuse completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en ServerFuse para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    public async Task<string> SpecialAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando Special para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.Special(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("Special completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en Special para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

    public async Task<string> ServerCompareAsync(string id, string secret, string payload)
    {
        var stopwatch = Stopwatch.StartNew();
        var instance = await _pool.GetInstanceAsync();
        try
        {
            if (_config.EnableDebug)
            {
                _logger?.LogDebug("Ejecutando ServerCompare para ID: {Id}, Payload: {PayloadLength} bytes",
                    id, payload?.Length ?? 0);
            }

            var result = instance.ServerCompare(id, secret, payload);
            
            stopwatch.Stop();
            _logger?.LogInformation("ServerCompare completado para ID: {Id} en {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger?.LogError(ex, "Error en ServerCompare para ID: {Id} después de {ElapsedMs}ms",
                id, stopwatch.ElapsedMilliseconds);
            throw;
        }
        finally
        {
            _pool.ReturnInstance(instance);
        }
    }

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
            // El pool se dispose en el contenedor DI
        }
        _disposed = true;
    }
}