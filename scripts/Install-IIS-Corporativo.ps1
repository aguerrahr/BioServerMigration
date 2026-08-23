# ==========================================================
# INSTALACION CORPORATIVA IIS 10
# WINDOWS SERVER 2022
# ==========================================================
# 
# PROPOSITO:
# Instalar y configurar IIS 10 con todos los modulos necesarios
# para alojar aplicaciones ASP.NET Core (BioServer API, Quasar, etc.)
#
# REQUIERE:
# - Ejecucion como Administrador
# - Acceso a Internet (para descargar los modulos)
# - Windows Server 2022 (o 2019/2016)
#
# QUE INSTALA:
# - Modulos basicos de IIS (HTTP, seguridad, rendimiento)
# - Soporte para ASP.NET 4.5+ (Web-Net-Ext45, Web-Asp-Net45)
# - Herramientas de administracion (Web-Mgmt-Tools, Web-Mgmt-Console)
# - Soporte para WebSockets
# ==========================================================

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host " INSTALACION CORPORATIVA IIS 10 " -ForegroundColor Green
Write-Host " WINDOWS SERVER 2022 " -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Iniciando instalacion de IIS con todos los modulos..." -ForegroundColor Yellow
Write-Host ""

# --- INSTALAR TODOS LOS MODULOS DE IIS ---
Install-WindowsFeature `
    Web-Server, `                    # Servidor Web base
    Web-WebServer, `                 # Infraestructura del servidor web
    Web-Common-Http, `               # Caracteristicas HTTP comunes
    Web-Static-Content, `            # Contenido estatico (HTML, CSS, JS)
    Web-Default-Doc, `               # Documentos por defecto (default.htm, default.aspx)
    Web-Http-Errors, `               # Paginas de error personalizadas
    Web-Http-Redirect, `             # Redireccionamiento HTTP
    Web-Health, `                    # Health checks y diagnositco
    Web-Http-Logging, `              # Registro de logs HTTP
    Web-Request-Monitor, `           # Monitor de solicitudes en tiempo real
    Web-Http-Tracing, `              # Seguimiento de solicitudes HTTP
    Web-Performance, `               # Caracteristicas de rendimiento
    Web-Stat-Compression, `          # Compresion de contenido estatico
    Web-Dyn-Compression, `           # Compresion de contenido dinamico
    Web-Security, `                  # Caracteristicas de seguridad
    Web-Filtering, `                 # Filtrado de solicitudes
    Web-Windows-Auth, `              # Autenticacion Windows
    Web-App-Dev, `                   # Caracteristicas de desarrollo de aplicaciones
    Web-Net-Ext45, `                 # Extensiones de .NET Framework 4.5+
    Web-Asp-Net45, `                 # Soporte para ASP.NET 4.5+
    Web-ISAPI-Ext, `                 # Extensiones ISAPI
    Web-ISAPI-Filter, `              # Filtros ISAPI
    Web-WebSockets, `                # Soporte para WebSockets
    Web-Mgmt-Tools, `                # Herramientas de administracion de IIS
    Web-Mgmt-Console `               # Consola de administracion de IIS
    -IncludeManagementTools          # Incluye las herramientas de administracion

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host " INSTALACION COMPLETADA " -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# --- MOSTRAR MODULOS INSTALADOS ---
Write-Host "Modulos de IIS instalados:" -ForegroundColor Yellow
Get-WindowsFeature *web* | Where-Object {$_.InstallState -eq "Installed"} | Format-Table Name, InstallState -AutoSize

# --- REINICIAR IIS ---
Write-Host ""
Write-Host "Validando IIS..." -ForegroundColor Yellow
iisreset
Write-Host ""
Write-Host "✅ IIS instalado correctamente." -ForegroundColor Green
Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host " PROXIMO PASO " -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "1. Ejecute el script Setup-Server.ps1 para instalar .NET" -ForegroundColor White
Write-Host "2. Luego ejecute Deploy-BioServerAPI.ps1 para desplegar la App" -ForegroundColor White
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""