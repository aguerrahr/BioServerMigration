# BioServer Migration - Master Document
**Versión:** 1.0  
**Fecha:** 2026-08-02  
**Estado:** En curso  

---

## 1. Resumen Ejecutivo

### Contexto
Sistema biométrico actual compuesto por:
- **DLL en VB6** (`libBioServer.dll`) - Núcleo de la lógica de negocio
- **Wrapper en VB6** (`libBioServerWrapper.dll`) - Fachada para consumidores
- **Web Service en VB.NET** (`BioServerWS.asmx`) - Exposición vía SOAP
- **Motor Biométrico HBIE** - Servicio REST para matching biométrico

### Problema Crítico
La DLL en VB6 está compilada con **ThreadingModel=1 (STA - Single-Threaded Apartment)**, lo que provoca:
- **Colisiones y bloqueos** cuando múltiples solicitudes concurrentes acceden a la DLL
- **Corrupción de memoria** en escenarios de alta carga
- **Rendimiento degradado** a medida que crece el número de usuarios

### Solución Propuesta
**Enfoque Híbrido en 3 Fases:**

1. **Fase 1 (Inmediata):** API REST en ASP.NET Core que envuelve la DLL VB6 con un pool de instancias.
2. **Fase 2 (Mediano Plazo):** Migración gradual de módulos de VB6 a C#.
3. **Fase 3 (Definitiva):** Eliminación completa de la dependencia de VB6.

---

## 2. Arquitectura

### Diagrama de Componentes
*(Ver documento separado: `docs/01-Architecture/ComponentDiagram.md`)*

### Diagrama de Clases
*(Ver documento separado: `docs/01-Architecture/ClassDiagram.md`)*

### Diagrama de Secuencia
*(Ver documento separado: `docs/01-Architecture/SequenceDiagram.md`)*

### Diagrama de Flujo de Carriles
*(Ver documento separado: `docs/01-Architecture/FlowDiagram.md`)*

---

## 3. Roadmap Detallado

### Fase 1: API REST Wrapper (Semanas 1-3)
| Semana | Actividad | Entregable |
|--------|-----------|------------|
| 1 | Diseño de arquitectura, creación del proyecto ASP.NET Core | Proyecto base |
| 2 | Implementación del pool de instancias, wrapper C# para DLL | Pool funcionando |
| 3 | Pruebas de carga, despliegue en staging | API REST en staging |

### Fase 2: Migración por Módulos (Semanas 4-24)
| Módulo | Complejidad | Semanas | Dependencias |
|--------|-------------|---------|--------------|
| GetBioKey | Baja | 1 | Ninguna |
| GetAppKey | Baja | 1 | Ninguna |
| ServerFlush | Baja | 1 | Ninguna |
| FindFace | Media | 2 | HBIE |
| FindVoice | Media | 2 | HBIE |
| FindIris | Media | 2 | HBIE |
| FindFinger | Media | 2 | HBIE |
| FindPalm | Media | 2 | HBIE |
| ServerCompare | Alta | 3 | HBIE, Find* |
| ServerDelete | Media | 2 | Find* |
| ServerFuse | Alta | 3 | Find*, ServerDelete |
| ServerFind | Media | 2 | Find* |
| SendToServer | Alta | 3 | ServerSave |
| GetDataBioKey | Media | 2 | Ninguna |
| GetDataMapBioKey | Media | 2 | Ninguna |
| GetDataServer | Media | 2 | Ninguna |
| GetDataMapServer | Media | 2 | Ninguna |
| Special | Baja | 1 | Ninguna |
| ServerSave | **Muy Alta** | 4 | **Todos los anteriores** |

### Fase 3: Limpieza y Eliminación (Semanas 25-28)
| Semana | Actividad | Entregable |
|--------|-----------|------------|
| 25-26 | Pruebas de integración completas | Sistema 100% en .NET |
| 27-28 | Eliminación de DLL VB6, despliegue final | Sistema en producción |

---

## 4. Riesgos y Mitigaciones

| Riesgo | Probabilidad | Impacto | Mitigación |
|--------|--------------|---------|------------|
| **STA y concurrencia** | Alta | Alto | Pool de instancias en Fase 1 |
| **Rendimiento de HBIE** | Media | Alto | Timeouts, retry policies, caché |
| **Errores en migración** | Media | Alto | Pruebas de contraste en cada módulo |
| **Base de datos** | Baja | Alto | Migrar a Dapper/EF Core con cuidado |
| **Criptografía** | Baja | Alto | Usar System.Security.Cryptography |

---

## 5. Decisiones Clave (ADRs)

### ADR-001: Elegir API REST en lugar de migrar directamente
**Decisión:** Crear un API REST en ASP.NET Core que envuelva la DLL VB6 con un pool de instancias.

### ADR-002: Migrar módulos de menor a mayor complejidad
**Decisión:** Migrar primero las funciones más simples (GetBioKey, GetAppKey, ServerFlush) y luego las más complejas.

### ADR-003: Usar Dapper en lugar de EF Core
**Decisión:** Usar Dapper para la migración a .NET, por su simplicidad y rendimiento.

### ADR-004: Usar System.Text.Json en lugar de Newtonsoft.Json
**Decisión:** Usar System.Text.Json (nativo en .NET Core) por su mejor rendimiento.

---

## 6. Métricas de Éxito

| Métrica | Objetivo | Medición |
|---------|----------|----------|
| **Tiempo de respuesta** | < 500ms | Promedio por solicitud |
| **Concurrencia** | 50 usuarios simultáneos | Pruebas de carga |
| **Errores** | < 0.1% | Logs de errores |
| **Cobertura de pruebas** | > 80% | Pruebas unitarias |

---

## 7. Próximos Pasos

1. ✅ Master Document creado
2. ✅ Estructura de carpetas creada
3. ✅ Código del API REST subido
4. ⏳ Pruebas de contraste (pendiente)
5. ⏳ Migración del Módulo 1 (GetBioKey) - pendiente

---

## 8. Glosario

| Término | Definición |
|---------|------------|
| **STA** | Single-Threaded Apartment - Modelo de concurrencia de COM |
| **ABIS** | Automated Biometric Identification System |
| **HBIE** | Motor biométrico utilizado (probablemente Neurotechnology) |
| **BioKey** | Identificador único de un sujeto biométrico |
| **ADR** | Architecture Decision Record |