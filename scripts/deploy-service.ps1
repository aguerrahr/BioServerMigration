# ============================================================
# SCRIPT PARA DESPLEGAR BIOSERVER API COMO SERVICIO WINDOWS
# ============================================================
# Requisitos: PowerShell 5.1+, .NET 8 Runtime

param(
    [Parameter(Mandatory=$true)]
    [string]$ServiceName = "BioServerAPI",
    
    [Parameter(Mandatory=$true)]
    [string]$PhysicalPath,
    
    [string]$Environment = "Staging"
)

Write-Host "🚀 Instalando BioServer API como servicio Windows..." -ForegroundColor Cyan

# 1. Validar que el directorio existe
if (-not (Test-Path $PhysicalPath)) {
    Write-Host "❌ Error: La ruta '$PhysicalPath' no existe." -ForegroundColor Red
    exit 1
}

# 2. Publicar la aplicación
Write-Host "📦 Publicando aplicación..." -ForegroundColor Yellow
$publishPath = "$PhysicalPath\publish"
Remove-Item -Path $publishPath -Recurse -Force -ErrorAction SilentlyContinue

dotnet publish "$PhysicalPath\..\..\src\01-Phase1-API-REST\BioServerAPI\BioServerAPI.csproj" `
    -c Release `
    -o $publishPath `
    --no-restore

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Error al publicar la aplicación." -ForegroundColor Red
    exit 1
}

# 3. Copiar archivos de configuración según entorno
Write-Host "📋 Configurando entorno: $Environment" -ForegroundColor Yellow
$appSettings = "$publishPath\appsettings.json"
$appSettingsEnv = "$publishPath\appsettings.$Environment.json"

if (Test-Path $appSettingsEnv) {
    Copy-Item -Path $appSettingsEnv -Destination $appSettings -Force
    Write-Host "✅ Configuración de $Environment aplicada." -ForegroundColor Green
}

# 4. Instalar el servicio
Write-Host "🔄 Instalando servicio Windows..." -ForegroundColor Yellow
$service = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
if ($service) {
    Write-Host "⚠️ El servicio '$ServiceName' ya existe. Deteniendo y desinstalando..." -ForegroundColor Yellow
    Stop-Service -Name $ServiceName -Force -ErrorAction SilentlyContinue
    sc.exe delete $ServiceName
}

# Crear el servicio con sc.exe
$binPath = "$publishPath\BioServerAPI.exe"
$command = "sc.exe create $ServiceName binPath= `"$binPath`" start= auto DisplayName= `"$ServiceName`""

Write-Host "📝 Ejecutando: $command" -ForegroundColor Gray
Invoke-Expression -Command $command

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Error al crear el servicio." -ForegroundColor Red
    exit 1
}

# Configurar el servicio para que se inicie automáticamente
sc.exe config $ServiceName start= auto

# Iniciar el servicio
Write-Host "▶️ Iniciando servicio..." -ForegroundColor Yellow
Start-Service -Name $ServiceName -ErrorAction SilentlyContinue

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Error al iniciar el servicio." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Servicio instalado exitosamente." -ForegroundColor Green
Write-Host "📋 Para ver el estado del servicio, ejecute: Get-Service -Name $ServiceName" -ForegroundColor Cyan
Write-Host "📋 Para ver los logs, revise la carpeta: $publishPath\logs" -ForegroundColor Cyan