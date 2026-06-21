# LowCortisol Platform

Backend ASP.NET Core for the LowCortisol modular monolith.

## Run

```powershell
dotnet restore .\LowCortisol.Platform.API\LowCortisol.Platform.API.csproj
dotnet run --project .\LowCortisol.Platform.API\LowCortisol.Platform.API.csproj
```

Development URLs from `launchSettings.json`:

- HTTP: `http://localhost:5077`
- HTTPS: `https://localhost:7005`

## PostgreSQL

Development uses PostgreSQL by default. Configure the local connection string in
`LowCortisol.Platform.API/appsettings.Development.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=lowcortisol_platform;Username=postgres;Password=123456789"
}
```

Do not place production credentials in source. For production, provide
`ConnectionStrings__DefaultConnection` through environment variables or a secure
deployment secret store.

## Persistence Provider

Default development provider:

```json
"Persistence": {
  "Provider": "PostgreSQL",
  "SeedOnStartup": true
}
```

Set `Persistence:Provider` to `InMemory` to use the demo adapter without
PostgreSQL. The InMemory adapter is kept as fallback/demo and should not replace
the PostgreSQL adapter for persistence QA.

## Migrations

Install EF tooling if needed:

```powershell
dotnet tool install --global dotnet-ef
```

Create the operational model migration:

```powershell
dotnet ef migrations add InitialLowCortisolOperationalModel `
  --project .\LowCortisol.Platform.API\LowCortisol.Platform.API.csproj `
  --output-dir Shared\Infrastructure\Persistence\EntityFrameworkCore\Migrations
```

Apply migrations:

```powershell
dotnet ef database update `
  --project .\LowCortisol.Platform.API\LowCortisol.Platform.API.csproj
```

## Demo Seed

`DatabaseSeeder` runs only when `Persistence:SeedOnStartup` is `true` and the
database has no `Site` records. The demo seed prepares:

- Workplace physical model: site, rooms and device groups.
- DeviceControl: hub, water/gas sensors and valve.
- Monitoring: warning/critical thresholds, readings and anomalies.
- Notification: active `in_app` channel, critical alert, incident and delivery.

## Swagger / OpenAPI

In Development:

- OpenAPI JSON: `http://localhost:5077/openapi/v1.json`
- Swagger UI: `http://localhost:5077/swagger`

## Manual QA Flow

Use Swagger to validate:

1. `GET /api/v1/sites`
2. `GET /api/v1/sites/{siteId}/physical-model`
3. `GET /api/v1/monitoring/summary`
4. `GET /api/v1/thresholds/active`
5. `POST /api/v1/readings`
6. `GET /api/v1/anomalies/open`
7. `GET /api/v1/alerts/open`
8. `GET /api/v1/incidents/open`
9. `GET /api/v1/notifications/summary`
10. `POST /api/v1/anomalies/{id}/resolve`
11. `POST /api/v1/alerts/{id}/acknowledge`
12. `POST /api/v1/alerts/{id}/resolve`
13. `POST /api/v1/incidents/{id}/resolve`
14. `POST /api/v1/incidents/{id}/close`
