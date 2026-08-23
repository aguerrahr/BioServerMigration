# Generación del Paquete de Despliegue

## Objetivo
Crear un paquete comprimido (`.zip`) con la aplicación BioServer API lista para ser desplegada en un servidor de producción. Este proceso debe ejecutarlo el equipo de desarrollo.

## Requisitos Previos
- .NET 10 SDK instalado en la máquina de desarrollo.
- Acceso a la carpeta con las DLLs de VB6 (`libBioServerWrapper.dll` y `libBioServer.dll`).
- Acceso al código fuente del proyecto.

## Script: `Build-DeploymentPackage.ps1`

### Instrucciones de Uso
1.  Copia el script `Build-DeploymentPackage.ps1` en la raíz de tu repositorio local.
2.  Abre PowerShell y navega hasta la raíz del repositorio.
3.  Ejecuta el script:
    ```powershell
        .\Build-DeploymentPackage.ps1
4. El script te pedirá la ruta de las DLLs de VB6. Proporciona la ruta completa.
5. El script generará un archivo BioServerAPI.zip en la carpeta C:\BioServer\Deployments\.