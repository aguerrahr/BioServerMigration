# ========================================================================
# SCRIPT: Setup-Server.ps1
# DESCRIPCION: Prepara el servidor Windows con todos los prerrequisitos.
# REQUIERE: Ejecucion como Administrador.
# ========================================================================

Write-Host "INICIANDO CONFIGURACION DEL SERVIDOR PARA BIOSERVER API" -ForegroundColor Cyan
$ErrorActionPreference = "Stop"

# --- 1. DEFINIR VERSIONES ---
$DotNetVersion = "10.0.0"
$RuntimeVersion = "10.0.0"

# --- 2. FUNCION PARA VERIFICAR .NET RUNTIME ---
function Test-DotNetInstalled {
    $dotnetPath = Get-ChildItem -Path "C:\Program Files\dotnet\shared\Microsoft.NETCore.App" -ErrorAction SilentlyContinue | Sort-Object -Property Name -Descending | Select-Object -First 1
    if ($dotnetPath) {
        return $true
    }
    return $false
}

# --- 3. INSTALAR .NET 10 RUNTIME (USANDO SCRIPT OFICIAL) ---
Write-Host "Verificando e instalando .NET 10 Runtime..." -ForegroundColor Yellow
if (Test-DotNetInstalled) {
    Write-Host ".NET Runtime ya esta instalado." -ForegroundColor Green
} else {
    Write-Host ".NET 10 Runtime no encontrado. Descargando e instalando..." -ForegroundColor Yellow
    
    # Descargar el script oficial de instalacion de .NET
    $installScriptUrl = "https://builds.dotnet.microsoft.com/dotnet/scripts/v1/dotnet-install.ps1"
    $installScriptPath = "$env:TEMP\dotnet-install.ps1"
    
    try {
        Write-Host "Descargando script de instalacion de .NET..." -ForegroundColor Yellow
        Invoke-WebRequest -Uri $installScriptUrl -OutFile $installScriptPath -UseBasicParsing
        
        Write-Host "Ejecutando instalacion de .NET Runtime ${DotNetVersion}..." -ForegroundColor Yellow
        & $installScriptPath -Version $DotNetVersion -Runtime dotnet -InstallDir "C:\Program Files\dotnet" -NoPath
        
        Remove-Item -Path $installScriptPath -Force
        Write-Host ".NET Runtime instalado correctamente." -ForegroundColor Green
    } catch {
        Write-Host "Error al descargar o instalar .NET Runtime: $_" -ForegroundColor Red
        Write-Host "Verifica la conexion a internet y que la URL sea accesible." -ForegroundColor Yellow
        exit 1
    }
}

# --- 4. INSTALAR ASP.NET CORE HOSTING BUNDLE ---
Write-Host "Instalando ASP.NET Core Hosting Bundle..." -ForegroundColor Yellow
# Usamos la URL directa que encontraste
$url = "https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/${RuntimeVersion}/aspnetcore-runtime-${RuntimeVersion}-win-x64.exe"
$installer = "$env:TEMP\aspnetcore-runtime-${RuntimeVersion}-win-x64.exe"
try {
    Write-Host "Descargando ASP.NET Core Hosting Bundle desde:" -ForegroundColor Yellow
    Write-Host $url -ForegroundColor Cyan
    
    Invoke-WebRequest -Uri $url -OutFile $installer -UseBasicParsing
    Write-Host "Instalando ASP.NET Core Hosting Bundle..." -ForegroundColor Yellow
    Start-Process -FilePath $installer -ArgumentList "/quiet /norestart" -Wait
    Remove-Item -Path $installer -Force
    Write-Host "ASP.NET Core Hosting Bundle instalado correctamente." -ForegroundColor Green
} catch {
    Write-Host "Error al descargar o instalar ASP.NET Core Hosting Bundle: $_" -ForegroundColor Red
    Write-Host "Verifica la conexion a internet y que la URL sea accesible." -ForegroundColor Yellow
    Write-Host "Puedes descargar manualmente el instalador desde: https://dotnet.microsoft.com/en-us/download/dotnet/10.0" -ForegroundColor Cyan
    exit 1
}

# --- 5. VERIFICAR IIS Y MODULOS ---
Write-Host "Verificando instalacion de IIS..." -ForegroundColor Yellow
try {
    Import-Module ServerManager -ErrorAction SilentlyContinue
    if (-not (Get-WindowsFeature -Name Web-Server).Installed) {
        Write-Host "IIS no esta instalado. Instalando..." -ForegroundColor Yellow
        Install-WindowsFeature -Name Web-Server -IncludeAllSubFeature -IncludeManagementTools
        Write-Host "IIS instalado correctamente." -ForegroundColor Green
    } else {
        Write-Host "IIS ya esta instalado." -ForegroundColor Green
    }
} catch {
    Write-Host "Error al verificar o instalar IIS: $_" -ForegroundColor Red
    Write-Host "Asegurate de ejecutar el script como Administrador." -ForegroundColor Yellow
    exit 1
}

# --- 6. CREAR ESTRUCTURA DE CARPETAS ---
Write-Host "Creando estructura de carpetas en C:\inetpub\BioServerApp\" -ForegroundColor Yellow
$folders = @(
    "C:\inetpub\BioServerApp\Deployments",
    "C:\inetpub\BioServerApp\Logs",
    "C:\inetpub\BioServerApp\DLLs",
    "C:\inetpub\BioServerApp\Backups"
)
foreach ($folder in $folders) {
    try {
        New-Item -ItemType Directory -Force -Path $folder -ErrorAction SilentlyContinue | Out-Null
    } catch {
        Write-Host "Error al crear la carpeta ${folder}: $_" -ForegroundColor Red
    }
}
Write-Host "Estructura de carpetas creada." -ForegroundColor Green

# --- 7. AGREGAR .NET AL PATH (SI ES NECESARIO) ---
Write-Host "Verificando que .NET este en el PATH..." -ForegroundColor Yellow
$dotnetPath = "C:\Program Files\dotnet"
if ($env:Path -notlike "*$dotnetPath*") {
    Write-Host "Agregando .NET al PATH..." -ForegroundColor Yellow
    [Environment]::SetEnvironmentVariable("Path", $env:Path + ";$dotnetPath", [EnvironmentVariableTarget]::Machine)
    # Actualizar el PATH para la sesion actual
    $env:Path = [System.Environment]::GetEnvironmentVariable("Path", "Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path", "User")
    Write-Host ".NET agregado al PATH." -ForegroundColor Green
} else {
    Write-Host ".NET ya esta en el PATH." -ForegroundColor Green
}

# --- 8. REINICIAR IIS (SI ES NECESARIO) ---
Write-Host "Reiniciando IIS para aplicar cambios..." -ForegroundColor Yellow
iisreset /noforce | Out-Null
Write-Host "IIS reiniciado correctamente." -ForegroundColor Green

# --- 9. FINALIZACION ---
Write-Host ""
Write-Host "CONFIGURACION DEL SERVIDOR COMPLETADA CON EXITO!" -ForegroundColor Green
Write-Host "Los prerequisitos estan listos y la estructura de carpetas ha sido creada." -ForegroundColor Cyan
Write-Host "La carpeta 'C:\inetpub\BioServerApp\DLLs' esta lista para que copies las DLLs de VB6." -ForegroundColor Cyan
Write-Host ""
Write-Host "Para verificar la instalacion de .NET, ejecuta: dotnet --info" -ForegroundColor Cyan
Write-Host ""