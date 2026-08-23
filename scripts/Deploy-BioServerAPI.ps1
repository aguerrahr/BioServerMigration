# ========================================================================
# SCRIPT: Deploy-BioServerAPI.ps1
# DESCRIPCION: Despliega la aplicacion BioServer API en IIS usando appcmd.
# REQUIERE: Ejecucion como Administrador.
# ========================================================================

Write-Host "🚀 INICIANDO DESPLIEGUE DE BIOSERVER API" -ForegroundColor Cyan
$ErrorActionPreference = "Stop"

# --- 1. DEFINIR RUTA DE APPCMD ---
$appcmd = "C:\Windows\System32\inetsrv\appcmd.exe"

# --- 2. SOLICITAR RUTA DEL PAQUETE ---
Write-Host "📂 Solicitando ruta del paquete de despliegue..." -ForegroundColor Yellow
$zipPath = Read-Host -Prompt "Ingresa la ruta completa del archivo BioServerAPI.zip (ej: C:\temp\BioServerAPI.zip)"
if (-not (Test-Path $zipPath)) {
    Write-Host "❌ Error: El archivo '$zipPath' no existe." -ForegroundColor Red
    exit 1
}
Write-Host "✅ Paquete aceptado: $zipPath" -ForegroundColor Green

# --- 3. DEFINIR RUTAS ---
$appPath = "C:\inetpub\BioServerApp\Deployments\BioServerAPI"
$logPath = "C:\inetpub\BioServerApp\Logs"
Write-Host "📂 La aplicacion se desplegara en: $appPath" -ForegroundColor Cyan

# --- 4. VERIFICAR QUE APPCMD.EXE EXISTE ---
Write-Host "🔍 Verificando disponibilidad de appcmd.exe..." -ForegroundColor Yellow
if (-not (Test-Path $appcmd)) {
    Write-Host "❌ Error: No se encontro appcmd.exe en $appcmd" -ForegroundColor Red
    Write-Host "⚠️ Asegurate de que IIS este instalado con las herramientas de administracion." -ForegroundColor Yellow
    exit 1
}
Write-Host "✅ appcmd.exe disponible en: $appcmd" -ForegroundColor Green

# --- 5. DETENER SITIO EN IIS (SI EXISTE) ---
Write-Host "🔄 Deteniendo sitio en IIS (si existe)..." -ForegroundColor Yellow
$siteName = "BioServerAPI"
try {
    $siteExists = & $appcmd list site /name:$siteName 2>$null
    if ($siteExists -match $siteName) {
        & $appcmd stop site $siteName
        Write-Host "✅ Sitio detenido." -ForegroundColor Green
    } else {
        Write-Host "ℹ️ El sitio no existe, continuando..." -ForegroundColor Gray
    }
} catch {
    Write-Host "⚠️ No se pudo interactuar con IIS. Asegurate de ejecutar el script como Administrador." -ForegroundColor Yellow
}

# --- 6. ELIMINAR DESPLIEGUE ANTERIOR ---
Write-Host "📂 Preparando carpeta de despliegue..." -ForegroundColor Yellow
if (Test-Path $appPath) {
    Write-Host "🗑️ Eliminando despliegue anterior en $appPath..." -ForegroundColor Gray
    Remove-Item -Path $appPath -Recurse -Force -ErrorAction SilentlyContinue
}
New-Item -ItemType Directory -Force -Path $appPath | Out-Null
Write-Host "✅ Carpeta de despliegue preparada." -ForegroundColor Green

# --- 7. DESCOMPRIMIR PAQUETE ---
Write-Host "📦 Descomprimiendo paquete..." -ForegroundColor Yellow
try {
    Expand-Archive -Path $zipPath -DestinationPath $appPath -Force
    Write-Host "✅ Paquete descomprimido correctamente." -ForegroundColor Green
} catch {
    Write-Host "❌ Error al descomprimir el paquete: $_" -ForegroundColor Red
    exit 1
}

# --- 8. CONFIGURAR appsettings.json ---
Write-Host "📝 Configurando archivo de entorno..." -ForegroundColor Yellow
$appSettingsPath = "$appPath\appsettings.json"
$appSettingsStaging = "$appPath\appsettings.Staging.json"
if (Test-Path $appSettingsStaging) {
    Copy-Item -Path $appSettingsStaging -Destination $appSettingsPath -Force
    Write-Host "✅ Configuracion de staging aplicada." -ForegroundColor Green
} else {
    Write-Host "⚠️ No se encontro appsettings.Staging.json. Usando la configuracion por defecto." -ForegroundColor Yellow
}

# --- 9. CONFIGURAR appsettings.Production.json ---
Write-Host "📝 Creando configuracion de produccion..." -ForegroundColor Yellow
$appSettingsProd = @"
{
  "Logging": {
    "LogLevel": {
      "Default": "Error",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "BioServer": {
    "MaxInstances": 5,
    "TimeoutSeconds": 30,
    "DllPath": "C:\\BioServer\\bin\\libBioServerWrapper.dll",
    "EnableDebug": false
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Error",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "$logPath\\bioserver-.txt",
          "rollingInterval": "Day",
          "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      }
    ]
  },
  "AllowedHosts": "*"
}
"@
$appSettingsProd | Out-File -FilePath "$appPath\appsettings.Production.json" -Encoding utf8
Write-Host "✅ Configuracion de produccion creada." -ForegroundColor Green

# --- 10. CREAR APPLICATION POOL ---
Write-Host "🔄 Configurando Application Pool..." -ForegroundColor Yellow
$appPoolName = "BioServerAppPool"
try {
    $poolExists = & $appcmd list apppool /name:$appPoolName 2>$null
    if ($poolExists -match $appPoolName) {
        Write-Host "ℹ️ Application Pool '$appPoolName' ya existe." -ForegroundColor Gray
    } else {
        & $appcmd add apppool /name:$appPoolName
        Write-Host "✅ Application Pool '$appPoolName' creado." -ForegroundColor Green
    }
    & $appcmd set apppool $appPoolName /managedRuntimeVersion:"v8.0"
    & $appcmd set apppool $appPoolName /managedPipelineMode:"Integrated"
    Write-Host "✅ Application Pool '$appPoolName' configurado." -ForegroundColor Green
} catch {
    Write-Host "❌ Error al configurar Application Pool: $_" -ForegroundColor Red
    Write-Host "⚠️ Asegurate de que IIS este instalado y que appcmd.exe este disponible." -ForegroundColor Yellow
    exit 1
}

# --- 11. CREAR SITIO WEB ---
Write-Host "🌐 Configurando Sitio Web..." -ForegroundColor Yellow
try {
    $siteExists = & $appcmd list site /name:$siteName 2>$null
    if ($siteExists -match $siteName) {
        Write-Host "ℹ️ Sitio Web '$siteName' ya existe." -ForegroundColor Gray
    } else {
        & $appcmd add site /name:$siteName /physicalPath:$appPath /bindings:"http/*:80:$siteName"
        Write-Host "✅ Sitio Web '$siteName' creado." -ForegroundColor Green
    }
    & $appcmd set site $siteName /applicationDefaults.applicationPool:$appPoolName
    Write-Host "✅ Sitio Web '$siteName' configurado." -ForegroundColor Green
} catch {
    Write-Host "❌ Error al configurar Sitio Web: $_" -ForegroundColor Red
    exit 1
}

# --- 12. ASIGNAR PERMISOS ---
Write-Host "🔐 Asignando permisos..." -ForegroundColor Yellow
$identity = "IIS APPPOOL\$appPoolName"
try {
    icacls $appPath /grant "$($identity):(OI)(CI)RX" /T | Out-Null
    Write-Host "✅ Permisos asignados correctamente." -ForegroundColor Green
} catch {
    Write-Host "❌ Error al asignar permisos: $_" -ForegroundColor Red
    Write-Host "⚠️ Puedes asignar manualmente permisos a la carpeta $appPath para el usuario $identity." -ForegroundColor Yellow
}

# --- 13. INICIAR SITIO ---
Write-Host "▶️ Iniciando sitio en IIS..." -ForegroundColor Yellow
try {
    & $appcmd start site $siteName
    Write-Host "✅ Sitio iniciado." -ForegroundColor Green
} catch {
    Write-Host "❌ Error al iniciar el sitio: $_" -ForegroundColor Red
    Write-Host "⚠️ Puedes iniciar el sitio manualmente desde el Administrador de IIS." -ForegroundColor Yellow
}

# --- 14. FINALIZACION ---
Write-Host ""
Write-Host "🎉 ¡DESPLIEGUE COMPLETADO CON EXITO!" -ForegroundColor Green
Write-Host ""
Write-Host "📋 RESUMEN DE LA INSTALACION:" -ForegroundColor Cyan
Write-Host "🌐 URL del API REST: http://$($env:COMPUTERNAME)/$siteName" -ForegroundColor White
Write-Host "📂 Carpeta de la aplicacion: $appPath" -ForegroundColor White
Write-Host "📂 Logs: $logPath" -ForegroundColor White
Write-Host "📂 DLLs de VB6: C:\BioServer\bin" -ForegroundColor White
Write-Host ""
Write-Host "📋 PARA VERIFICAR EL FUNCIONAMIENTO:" -ForegroundColor Cyan
Write-Host "1. Health Check: http://$($env:COMPUTERNAME)/$siteName/health" -ForegroundColor White
Write-Host "2. Swagger UI: http://$($env:COMPUTERNAME)/$siteName/swagger" -ForegroundColor White
Write-Host "3. Probar un endpoint: http://$($env:COMPUTERNAME)/$siteName/api/bioserver/get-bio-key" -ForegroundColor White
Write-Host ""