using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BioServerAPI.Services;

public class BioServerHealthCheck : IHealthCheck
{
    private readonly BioServerPool _pool;
    private readonly ILogger<BioServerHealthCheck> _logger;

    public BioServerHealthCheck(BioServerPool pool, ILogger<BioServerHealthCheck> logger)
    {
        _pool = pool;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Probar obtener una instancia
            var instance = await _pool.GetInstanceAsync(cancellationToken);
            if (instance != null)
            {
                _pool.ReturnInstance(instance);
                return HealthCheckResult.Healthy("BioServer pool está funcionando correctamente.");
            }
            
            return HealthCheckResult.Unhealthy("No se pudo obtener una instancia del pool.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check falló");
            return HealthCheckResult.Unhealthy("Error en el pool de BioServer", ex);
        }
    }
}