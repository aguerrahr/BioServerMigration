using BioServerAPI.Configuration;
using BioServerAPI.Services;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/bioserver-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Configurar servicios
builder.Services.Configure<BioServerConfig>(builder.Configuration.GetSection("BioServer"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<BioServerConfig>>().Value);

// Registrar el pool de BioServer
builder.Services.AddSingleton<BioServerPool>(sp =>
{
    var logger = sp.GetService<ILogger<BioServerPool>>();
    var config = sp.GetRequiredService<BioServerConfig>();
    return new BioServerPool(config.MaxInstances, logger);
});

// Registrar el servicio
builder.Services.AddScoped<BioServerService>();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configurar controladores
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.WriteIndented = false;
    });

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<BioServerHealthCheck>("BioServerHealth");

var app = builder.Build();

// Configurar pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

// Inicializar el pool
var pool = app.Services.GetRequiredService<BioServerPool>();
await pool.InitializeAsync();

app.Run();