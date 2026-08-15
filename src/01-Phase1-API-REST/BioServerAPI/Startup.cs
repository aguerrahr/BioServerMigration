using BioServerAPI.Configuration;
using BioServerAPI.Services;
using Microsoft.Extensions.Options;
using Serilog;

namespace BioServerAPI;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Configurar Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(_configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/bioserver-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        services.AddLogging(logging =>
        {
            logging.AddSerilog();
        });

        // Configurar BioServer
        services.Configure<BioServerConfig>(_configuration.GetSection("BioServer"));
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<BioServerConfig>>().Value);

        // Registrar el pool de BioServer
        services.AddSingleton<BioServerPool>(sp =>
        {
            var logger = sp.GetService<ILogger<BioServerPool>>();
            var config = sp.GetRequiredService<BioServerConfig>();
            return new BioServerPool(config.MaxInstances, logger);
        });

        // Registrar el servicio
        services.AddScoped<BioServerService>();

        // Configurar CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        // Configurar controladores
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.WriteIndented = false;
            });

        // Configurar Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Configurar Health Checks
        services.AddHealthChecks()
            .AddCheck<BioServerHealthCheck>("BioServerHealth");
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseAuthorization();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
        });
    }
}