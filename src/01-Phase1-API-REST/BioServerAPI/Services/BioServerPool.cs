using System.Collections.Concurrent;

namespace BioServerAPI.Services;

public class BioServerPool : IDisposable
{
    private readonly ConcurrentBag<BioServerWrapper> _instances = new();
    private readonly SemaphoreSlim _semaphore;
    private readonly int _maxInstances;
    private int _currentCount = 0;
    private readonly ILogger<BioServerPool> _logger;
    private bool _disposed;

    public BioServerPool(int maxInstances = 10, ILogger<BioServerPool> logger = null)
    {
        _maxInstances = maxInstances;
        _semaphore = new SemaphoreSlim(maxInstances, maxInstances);
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        _logger?.LogInformation("Inicializando pool de BioServer con {MaxInstances} instancias", _maxInstances);
        
        // Crear instancias iniciales (calentar el pool)
        var initialInstances = Math.Min(2, _maxInstances);
        for (int i = 0; i < initialInstances; i++)
        {
            try
            {
                var instance = await CreateInstanceAsync();
                _instances.Add(instance);
                _logger?.LogDebug("Instancia inicial #{InstanceId} creada", i + 1);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error al crear instancia inicial #{InstanceId}", i + 1);
            }
        }
        
        _logger?.LogInformation("Pool inicializado con {InstanceCount} instancias", _instances.Count);
    }

    public async Task<BioServerWrapper> GetInstanceAsync(CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("Solicitando instancia de BioServer. Instancias activas: {CurrentCount}, Disponibles: {AvailableCount}", 
            _currentCount, _instances.Count);

        if (!await _semaphore.WaitAsync(TimeSpan.FromSeconds(30), cancellationToken))
        {
            _logger?.LogError("Timeout al esperar instancia de BioServer");
            throw new TimeoutException("No hay instancias disponibles de BioServer. " +
                $"Instancias activas: {_currentCount}, Máximo: {_maxInstances}");
        }

        // Intentar tomar una instancia disponible
        if (_instances.TryTake(out var instance))
        {
            _logger?.LogDebug("Instancia reutilizada. Instancias activas: {CurrentCount}, Disponibles: {AvailableCount}",
                _currentCount, _instances.Count);
            return instance;
        }

        // Crear nueva instancia si no se alcanzó el límite
        if (Interlocked.Increment(ref _currentCount) <= _maxInstances)
        {
            _logger?.LogInformation("Creando nueva instancia de BioServer. Total: {CurrentCount}/{MaxInstances}",
                _currentCount, _maxInstances);
            try
            {
                var newInstance = await CreateInstanceAsync();
                return newInstance;
            }
            catch
            {
                Interlocked.Decrement(ref _currentCount);
                _semaphore.Release();
                throw;
            }
        }

        // Fallback: esperar por una instancia disponible
        _logger?.LogWarning("Se alcanzó el límite de instancias. Esperando una disponible...");
        var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(10));
        
        while (_instances.IsEmpty)
        {
            await Task.Delay(100, timeout.Token);
        }

        return _instances.TryTake(out var waitingInstance) ? waitingInstance : null;
    }

    public void ReturnInstance(BioServerWrapper instance)
    {
        if (instance == null) return;
        _instances.Add(instance);
        _semaphore.Release();
        _logger?.LogDebug("Instancia devuelta al pool. Disponibles: {AvailableCount}/{MaxInstances}",
            _instances.Count, _maxInstances);
    }

    private async Task<BioServerWrapper> CreateInstanceAsync()
    {
        // Crear la instancia en un hilo STA (requerido para VB6)
        var tcs = new TaskCompletionSource<BioServerWrapper>();
        var thread = new Thread(() =>
        {
            try
            {
                var instance = new BioServerWrapper();
                tcs.SetResult(instance);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        return await tcs.Task;
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
            _semaphore.Dispose();
            foreach (var instance in _instances)
            {
                try
                {
                    instance.Dispose();
                }
                catch
                {
                    // Ignorar errores en dispose
                }
            }
            _instances.Clear();
        }
        _disposed = true;
    }
}