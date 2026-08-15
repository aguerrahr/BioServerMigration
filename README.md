# BioServer Migration

## 📋 Descripción del Proyecto
Migración del sistema biométrico BioServer de VB6 a .NET 8, resolviendo problemas de concurrencia (STA) y mejorando la mantenibilidad.

## 🎯 Objetivos
- ✅ Resolver problemas de concurrencia en la DLL VB6 (ThreadingModel=1)
- ✅ Migrar gradualmente la lógica de negocio a C#
- ✅ Mantener compatibilidad con el motor biométrico HBIE
- ✅ Mejorar la escalabilidad y mantenibilidad

## 🏗️ Arquitectura

### Fase 1: API REST Wrapper (Actual)
- API REST en ASP.NET Core 8
- Pool de instancias para la DLL VB6
- Manejo de concurrencia con SemaphoreSlim
- Health Checks y logging con Serilog

### Fase 2: Migración de Módulos (En progreso)
- Migración función por función
- Feature flags para despliegue gradual
- Pruebas de contraste entre VB6 y C#

### Fase 3: Limpieza (Futuro)
- Eliminación de dependencias VB6
- Código 100% en .NET

## 🚀 Cómo Empezar

### Requisitos
- .NET 8 SDK
- Visual Studio 2022 o VS Code
- DLLs de VB6 (libBioServerWrapper.dll, libBioServer.dll)

### Configuración
1. Copiar las DLLs de VB6 a la carpeta de salida
2. Configurar `appsettings.Staging.json` con:
   - Cadena de conexión a la base de datos
   - Rutas de archivos
   - Configuración del motor HBIE

### Ejecución
```bash
cd src/01-Phase1-API-REST/BioServerAPI
dotnet restore
dotnet build
dotnet run --environment Staging