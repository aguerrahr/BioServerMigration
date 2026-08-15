# ADR-004: Usar System.Text.Json en lugar de Newtonsoft.Json

## Estado
✅ Aceptado (2026-08-02)

## Contexto
La DLL VB6 usa JSON para:
- Comunicación con HBIE (motor biométrico)
- Almacenamiento de datos en archivos .bio
- Respuestas del Web Service

## Decisión
Usar System.Text.Json (nativo en .NET Core) para serialización/deserialización.

## Razones
- **Rendimiento**: Es más rápido que Newtonsoft.Json
- **Nativo**: Viene incluido en .NET Core, sin dependencias externas
- **Estandarización**: Es el estándar de Microsoft para .NET Core
- **Menos overhead**: Menos asignaciones de memoria

## Consecuencias
- ✅ Mejor rendimiento
- ✅ Menos dependencias externas
- ✅ Soporte nativo en ASP.NET Core
- ❌ Menos flexibilidad que Newtonsoft.Json
- ❌ Requiere adaptar código existente (casos complejos)

## Alternativas Consideradas
1. **Newtonsoft.Json**: Más flexible, pero más lento
2. **Utf8Json**: Más rápido, pero menos conocido
3. **Jil**: Más rápido, pero menos mantenido