# ADR-001: Elegir API REST en lugar de migrar directamente

## Estado
✅ Aceptado (2026-08-02)

## Contexto
El sistema actual consiste en:
- DLL en VB6 con ThreadingModel=1 (STA)
- Web Service en VB.NET (SOAP)
- Problemas de concurrencia y bloqueos en producción

Se consideraron dos enfoques:
1. Migrar directamente la DLL VB6 a .NET
2. Crear un API REST que envuelva la DLL VB6 con un pool de instancias

## Decisión
Crear un API REST en ASP.NET Core que envuelva la DLL VB6 con un pool de instancias.

## Razones
- **Rápido**: Se puede implementar en semanas, no meses
- **Seguro**: No toca la lógica de negocio existente
- **Escalable**: Permite migrar gradualmente módulos
- **Resuelve el problema STA**: El pool de instancias controla la concurrencia

## Consecuencias
- ✅ Resuelve el problema de concurrencia inmediatamente
- ✅ Permite migrar gradualmente
- ❌ Sigue dependiendo de VB6
- ❌ Añade una capa adicional

## Alternativas Consideradas
1. **Migrar directamente a .NET**: Más tiempo, más riesgo
2. **Mantener el WS actual**: No resuelve el problema
3. **Reescribir desde cero**: Más costoso y riesgoso