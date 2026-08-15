# ADR-002: Migrar módulos de menor a mayor complejidad

## Estado
✅ Aceptado (2026-08-02)

## Contexto
La DLL VB6 tiene 19 funciones públicas que deben migrarse a C#. El orden de migración es crítico para:
- Minimizar riesgos
- Aprender el proceso con módulos simples
- Validar la estrategia de pruebas

## Decisión
Migrar primero las funciones más simples y luego las más complejas.

## Orden de Migración
| Orden | Módulo | Complejidad | Dependencias |
|-------|--------|-------------|--------------|
| 1 | GetBioKey | Baja | Ninguna |
| 2 | GetAppKey | Baja | Ninguna |
| 3 | ServerFlush | Baja | Ninguna |
| 4 | FindFace | Media | HBIE |
| 5 | FindVoice | Media | HBIE |
| 6 | FindIris | Media | HBIE |
| 7 | FindFinger | Media | HBIE |
| 8 | FindPalm | Media | HBIE |
| 9 | ServerCompare | Alta | HBIE |
| 10 | ServerDelete | Media | Find* |
| 11 | ServerFuse | Alta | Find*, ServerDelete |
| 12 | ServerFind | Media | Find* |
| 13 | SendToServer | Alta | ServerSave |
| 14 | GetDataBioKey | Media | Ninguna |
| 15 | GetDataMapBioKey | Media | Ninguna |
| 16 | GetDataServer | Media | Ninguna |
| 17 | GetDataMapServer | Media | Ninguna |
| 18 | Special | Baja | Ninguna |
| 19 | ServerSave | **Muy Alta** | **Todos los anteriores** |

## Razones
- **Aprendizaje**: Los módulos simples ayudan a entender el proceso
- **Validación**: Las pruebas de contraste se validan con módulos simples
- **Riesgo**: Los módulos complejos se abordan cuando ya hay experiencia

## Consecuencias
- ✅ Reduce el riesgo en las primeras etapas
- ✅ Permite validar la estrategia de pruebas
- ✅ El equipo gana confianza gradualmente
- ❌ El módulo más crítico (ServerSave) se migra al final

## Alternativas Consideradas
1. **Migrar ServerSave primero**: Demasiado riesgo
2. **Migrar todos los módulos en paralelo**: Difícil de coordinar
3. **Migrar por orden alfabético**: No considera complejidad