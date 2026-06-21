# Changelog

## v1.0.0 - 2026-06-21

Release backend/platform inicial de LowCortisol.

### Incluye

- Bounded contexts backend: Workplace, Monitoring, Notification, DeviceControl y Shared.
- Persistencia EF Core PostgreSQL con fallback InMemory para demo/desarrollo.
- Modelo fisico de sedes, ambientes y grupos.
- Ciclo de monitoreo: lecturas, umbrales, anomalias y resumen operativo.
- Respuesta a riesgo: alertas, incidentes, canales y entregas in-app.
- Mitigacion operativa: comandos de dispositivo, operaciones de valvula y auditoria.
- Swagger, CORS, lowercase URLs, seeds de desarrollo y migraciones iniciales.

### Validacion

- `dotnet restore .\lowcortisol-platform.sln`
- `dotnet build .\lowcortisol-platform.sln --no-restore`
