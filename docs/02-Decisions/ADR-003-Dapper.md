# ADR-003: Usar Dapper en lugar de EF Core

## Estado
✅ Aceptado (2026-08-02)

## Contexto
La DLL VB6 usa ADODB con consultas SQL dinámicas. La base de datos contiene:
- Tablas como BIO_INDEX, BIO_APP_KEY, BIO_USERS
- Consultas SQL complejas con joins y subconsultas
- Procedimientos almacenados (posiblemente)

## Decisión
Usar Dapper para el acceso a datos en la migración a .NET.

## Razones
- **Control total**: Permite escribir SQL exacto como en VB6
- **Rendimiento**: Más rápido que EF Core
- **Simplicidad**: Menos abstracciones, más fácil de depurar
- **Compatibilidad**: Facilita la migración de consultas existentes

## Consecuencias
- ✅ Control total sobre las consultas SQL
- ✅ Alto rendimiento
- ✅ Migración más directa desde ADODB
- ❌ Más código manual que EF Core
- ❌ Menos características ORM (tracking, migrations)

## Alternativas Consideradas
1. **Entity Framework Core**: Más abstracción, más lento
2. **ADO.NET puro**: Más código boilerplate
3. **NHibernate**: Complejo, curva de aprendizaje