# Preparación del Servidor Windows para BioServer API

## Objetivo
Este documento describe el proceso para preparar un servidor Windows Server (con IIS) para alojar el API REST de BioServer. El proceso está automatizado mediante un script de PowerShell.

## Requisitos Previos
- Servidor con Windows Server 2016 o superior.
- Acceso a Internet para la descarga de los instaladores de .NET.
- Ejecutar el script con permisos de **Administrador**.

## Script: `Setup-Server.ps1`

### Instrucciones de Uso
1.  Copia el script `Setup-Server.ps1` en el servidor.
2.  Abre PowerShell como Administrador.
3.  Navega hasta la carpeta donde se encuentra el script.
4.  Ejecuta el script:
    ```powershell
    .\Setup-Server.ps1
5. Espera a que el script finalice. Este instalará todos los componentes necesarios y verificará las dependencias.    