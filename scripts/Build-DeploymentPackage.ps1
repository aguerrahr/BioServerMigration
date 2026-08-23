# ========================================================================
# SCRIPT: Build-DeploymentPackage.ps1
# DESCRIPCION: Compila y empaqueta la aplicacion para su despliegue.
# ========================================================================

Write-Host "INICIANDO GENERACION DEL PAQUETE DE DESPLIEGUE" -ForegroundColor Cyan
$ErrorActionPreference = "Stop"

# --- 1. DEFINIR RUTAS (RUTAS ABSOLUTAS) ---
$repoRoot = "E:\SICREX\MCSISTEMAS\BioServer\BioServerMigration"
$projectPath = "$repoRoot\src\01-Phase1-API-REST\BioServerAPI\BioServerAPI.csproj"
$publishPath = Join-Path $env:TEMP "BioServerAPI-Publish"
$outputZip = "E:\SICREX\MCSISTEMAS\BioServer\BioServerMigration\deployment\BioServerAPI.zip"

# --- 2. VERIFICAR QUE EL PROYECTO EXISTE ---
if (-not (Test-Path $projectPath)) {
    Write-Host "ERROR: No se encontro el archivo de proyecto en: $projectPath" -ForegroundColor Red
    Write-Host "Verifica que la ruta sea correcta." -ForegroundColor Yellow
    exit 1
}
Write-Host "Proyecto encontrado: $projectPath" -ForegroundColor Green

# --- 3. LIMPIAR PUBLICACIONES ANTERIORES ---
if (Test-Path $publishPath) {
    Remove-Item -Path $publishPath -Recurse -Force
}
New-Item -ItemType Directory -Force -Path $publishPath | Out-Null

# --- 4. PUBLICAR LA APLICACION ---
Write-Host "Publicando la aplicacion..." -ForegroundColor Yellow
dotnet publish $projectPath -c Release -o $publishPath --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "Error al publicar la aplicacion." -ForegroundColor Red
    exit 1
}
Write-Host "Aplicacion publicada correctamente." -ForegroundColor Green

# --- 5. COPIAR DLLs DE VB6 (NO ES NECESARIO) ---
Write-Host "Las DLLs de VB6 NO se copian al paquete." -ForegroundColor Yellow
Write-Host "Se tomaran desde su ubicacion fija en el servidor." -ForegroundColor Cyan

# --- 6. CREAR ARCHIVO ZIP ---
Write-Host "Empaquetando archivos en $outputZip ..." -ForegroundColor Yellow
Compress-Archive -Path "$publishPath\*" -DestinationPath $outputZip -Force
Write-Host "Paquete creado correctamente." -ForegroundColor Green

# --- 7. LIMPIAR ---
Remove-Item -Path $publishPath -Recurse -Force

# --- 8. FINALIZACION ---
Write-Host ""
Write-Host "PAQUETE DE DESPLIEGUE GENERADO CON EXITO!" -ForegroundColor Green
Write-Host "Archivo: $outputZip" -ForegroundColor Cyan
Write-Host "Entregue este archivo al equipo de IT para el despliegue." -ForegroundColor Cyan
Write-Host ""