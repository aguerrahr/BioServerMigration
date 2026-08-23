# Despliegue de la Aplicación BioServer API en IIS

## Objetivo
Desplegar la aplicación BioServer API en el servidor de producción (o staging) utilizando el paquete `.zip` generado por el equipo de desarrollo.

## Requisitos Previos
- El servidor debe haber sido preparado con el script `Setup-Server.ps1`.
- Tener acceso al archivo `BioServerAPI.zip`.
- Tener las DLLs de VB6 copiadas en la carpeta `C:\BioServer\DLLs` (o la ruta que se haya configurado en el script).
- Ejecutar el script con permisos de **Administrador**.

## Script: `Deploy-BioServerAPI.ps1`

### Instrucciones de Uso
1.  Copia el script `Deploy-BioServerAPI.ps1` en el servidor.
2.  Abre PowerShell como Administrador.
3.  Navega hasta la carpeta donde se encuentra el script.
4.  Ejecuta el script:
    ```powershell
    .\Deploy-BioServerAPI.ps1
5. El script te pedirá la ruta del archivo BioServerAPI.zip. Proporciona la ruta completa.
6. El script descomprimirá los archivos, configurará IIS y dejará la aplicación en funcionamiento.