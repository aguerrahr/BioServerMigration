# Diagrama de Flujo de Carriles - Migración BioServer

## Flujo de Migración por Fases

```mermaid
gantt
    title BioServer Migration Timeline
    dateFormat  YYYY-MM-DD
    axisFormat %d/%m
    
    section Fase 1: API REST
    Diseño Arquitectura        :a1, 2026-08-02, 3d
    Creación Proyecto ASP.NET  :a2, after a1, 4d
    Implementación Pool        :a3, after a2, 5d
    Wrapper C# para DLL        :a4, after a3, 3d
    Pruebas de Carga           :a5, after a4, 3d
    Despliegue en Staging      :a6, after a5, 2d

    section Fase 2: Módulos
    Módulo 1: GetBioKey        :b1, after a6, 5d
    Módulo 2: GetAppKey        :b2, after b1, 5d
    Módulo 3: ServerFlush      :b3, after b2, 5d
    Módulo 4: FindFace         :b4, after b3, 10d
    Módulo 5: FindVoice        :b5, after b4, 10d
    Módulo 6: FindIris         :b6, after b5, 10d
    Módulo 7: FindFinger       :b7, after b6, 10d
    Módulo 8: FindPalm         :b8, after b7, 10d
    Módulo 9: ServerCompare    :b9, after b8, 15d
    Módulo 10: ServerDelete    :b10, after b9, 10d
    Módulo 11: ServerFuse      :b11, after b10, 15d
    Módulo 12: ServerSave      :b12, after b11, 20d

    section Fase 3: Limpieza
    Pruebas de Integración     :c1, after b12, 15d
    Eliminación DLL VB6        :c2, after c1, 15d
    Despliegue Final           :c3, after c2, 10d
```
## Flujo de Carriles - Por Módulo
```mermaid
graph LR
    subgraph Semana["Semana X"]
        direction LR
        A[Análisis<br/>1-2 días] --> B[Desarrollo<br/>2-3 días]
        B --> C[Pruebas<br/>1-2 días]
        C --> D[Despliegue<br/>1 día]
    end

    subgraph Equipo["Equipo"]
        E1[Arquitecto]
        E2[Desarrollador]
        E3[QA]
        E4[DevOps]
    end

    E1 --> A
    E2 --> B
    E3 --> C
    E4 --> D
```

## Flujo de Carriles - Pruebas de Contraste

```mermaid
graph TD
    subgraph Input["Entrada"]
        I1[Payload JSON]
    end

    subgraph Ejecucion["Ejecución"]
        direction LR
        V1[VB6 DLL] --> R1[Resultado VB6]
        C1[C# Module] --> R2[Resultado C#]
    end

    subgraph Comparacion["Comparación"]
        R1 --> Comp{¿Son iguales?}
        R2 --> Comp
        Comp -->|Sí| OK[✅ Aprobado]
        Comp -->|No| FAIL[❌ Revisar]
    end

    I1 --> V1
    I1 --> C1

    FAIL --> Debug[Depurar y Ajustar]
    Debug --> C1
```
## Flujo de Carriles - Despliegue
```mermaid
graph LR
    subgraph Desarrollo["Desarrollo Local"]
        Dev["VS Code / Visual Studio"]
        Dev --> |"dotnet build"| Build["Compilación"]
        Build --> |"dotnet test"| Test["Pruebas Unitarias"]
    end

    subgraph Staging["Entorno Staging"]
        Test --> |"dotnet publish"| Pub["Publicación"]
        Pub --> |"PowerShell"| Deploy["Despliegue en Staging"]
        Deploy --> |"Postman"| Val["Pruebas de Contraste"]
    end

    subgraph Produccion["Entorno Producción"]
        Val --> |"Aprobado"| Prod["Despliegue en Producción"]
        Prod --> |"Monitoring"| Mon["Monitoreo"]
    end

    Val --> |"Fallido"| Dev
```    