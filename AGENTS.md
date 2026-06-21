# LowCortisol Platform Rules

Scope: backend only. Do not use this file for frontend rules.

- Stack: ASP.NET Core on .NET, modular monolith.
- Use DDD and CQRS when adding backend behavior.
- Organize bounded contexts with `Domain`, `Application`, `Infrastructure` and
  `Interfaces`.
- Commands represent state changes; Queries represent reads.
- Application services expose `Handle(...)` methods and propagate
  `CancellationToken`.
- Expected business failures return `Result` or `Result<T>`.
- Use `Resources`, not DTOs, for REST contracts.
- Keep repository ports in `Domain`.
- Put EF Core repository adapters and persistence configuration in
  `Infrastructure`.
- Use Unit of Work to commit each use case once.
- Controllers must stay thin: receive Resource, assemble Command/Query, call the
  service, return assembled HTTP results.
- Use Assemblers for Resource-to-Command, Entity-to-Resource and
  Result-to-ActionResult mapping.
- Use RFC 7807 `ProblemDetails` for API errors.
- Use `.resx` resources and `IStringLocalizer` for backend i18n.
- Document REST endpoints with Swagger/OpenAPI response metadata.
- Do not expose aggregates directly through HTTP.
- Do not create migrations or persistence changes without explicit scope.
- Verify backend changes with:

```powershell
dotnet build .\lowcortisol-platform.sln
```

For detailed backend architecture, read `../docs/architecture-guidelines.md`.
