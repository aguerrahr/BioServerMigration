# ============================================================
# SCRIPT PARA DESPLEGAR BIOSERVER API EN IIS
# ============================================================
# Requisitos: PowerShell 5.1+, IIS instalado, .NET 8 Runtime

param(
    [Parameter(Mandatory=$true)]
    [string]$SiteName = "BioServerAPI",
    
    [Parameter(Mandatory=$true)]
    [string]$AppPoolName = "BioServerAppPool",
    
    [Parameter(Mandatory=$true)]
    [string]$PhysicalPath,
    
    [string]$Environment = "Staging",
    
    [switch]$Force
)

Write-Host "🚀 Iniciando despliegue de BioServer API en IIS..." -ForegroundColor Cyan

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

# 4. Crear Application Pool
Write-Host "🔄 Configurando Application Pool..." -ForegroundColor Yellow
$appPool = Get-IISAppPool -Name $AppPoolName -ErrorAction SilentlyContinue
if (-not $appPool) {
    New-IISAppPool -Name $AppPoolName -ErrorAction SilentlyContinue
    Set-IISAppPool -Name $AppPoolName -ManagedRuntimeVersion "v8.0"
    Set-IISAppPool -Name $AppPoolName -ManagedPipelineMode "Integrated"
    Write-Host "✅ Application Pool '$AppPoolName' creado." -ForegroundColor Green
} else {
    Write-Host "ℹ️ Application Pool '$AppPoolName' ya existe." -ForegroundColor Gray
}

# 5. Crear o actualizar el sitio web
Write-Host "🌐 Configurando sitio web..." -ForegroundColor Yellow
$site = Get-IISSite -Name $SiteName -ErrorAction SilentlyContinue
if (-not $site) {
    New-IISSite -Name $SiteName -PhysicalPath $publishPath -BindingInformation "*:80:$SiteName" -Protocol http
    Write-Host "✅ Sitio '$SiteName' creado." -ForegroundColor Green
} else {
    if ($Force) {
        Remove-IISSite -Name $SiteName -Confirm:$false
        New-IISSite -Name $SiteName -PhysicalPath $publishPath -BindingInformation "*:80:$SiteName" -Protocol http
        Write-Host "✅ Sitio '$SiteName' actualizado." -ForegroundColor Green
    } else {
        Write-Host "⚠️ El sitio '$SiteName' ya existe. Use -Force para actualizar." -ForegroundColor Yellow
    }
}

# 6. Asignar Application Pool
Set-IISSite -Name $SiteName -ApplicationPool $AppPoolName

# 7. Verificar permisos de la carpeta
Write-Host "🔐 Configurando permisos..." -ForegroundColor Yellow
$identity = "IIS APPPOOL\$AppPoolName"
icacls $publishPath /grant "$identity:(OI)(CI)RX" /T

Write-Host "✅ Despliegue completado exitosamente." -ForegroundColor Green
Write-Host "🌐 El sitio está disponible en: http://localhost/$SiteName" -ForegroundColor Cyan