````markdown name=README.md
# MyCompany.TechnicalTest

## Overview

MyCompany.TechnicalTest is a .NET microservice template demonstrating Clean Architecture and Hexagonal (Ports & Adapters) principles. The repository is organized to keep business logic independent from infrastructure and framework concerns so use cases can be implemented, tested, and evolved in isolation.

The example domain includes customers, fleets and vehicles, and renting workflows (registering customers, renting and returning vehicles). It also contains infrastructure adapters, repository implementations, unit-of-work and tests (unit and integration) to illustrate an end-to-end implementation.

## Project Aim

- Provide a realistic microservice example that follows Clean Architecture and Hexagonal Architecture.
- Organize business behavior around explicit "use cases" (application services) that encapsulate domain workflows.
- Demonstrate ports (interfaces) and adapters (infrastructure implementations) so the core domain remains framework-agnostic.
- Include tests (unit and integration) for components in isolation and integrated scenarios.
- Demonstrate production-friendly capabilities: OpenAPI, health checks, telemetry, API versioning, configuration management, containerization, database persistence, and CI automation.

## Architectural Style

- Hexagonal Architecture (Ports & Adapters): core business logic is isolated and communicates with the outside world through well-defined ports and adapters.
- Clean Architecture: layers are separated into Domain, Application (Use Cases), Infrastructure (Adapters), and API (Controllers/Presenters).

## Technologies & Libraries

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core (DbContext) + SQLite (default/local/testing)
- Swashbuckle (Swagger / OpenAPI)
- Docker (Dockerfile included for the API)
- OpenTelemetry with Azure Monitor exporters
- ASP.NET Core Health Checks
- Asp.Versioning (API versioning)
- Microsoft.AspNetCore.Authentication.JwtBearer (JWT support; integrated into Swagger UI)
- FluentValidation (request/use-case validation)
- System.Text.Json (JSON serialization configuration)
- Built-in ASP.NET Core Dependency Injection
- NUnit (unit and integration tests)
- ProblemDetails middleware (standardized API errors)
- Common DDD & Clean Architecture patterns: Value Objects, Entities, Aggregate Roots, Repositories, Use Cases, Presenters, Factories, Unit of Work

## Key Features

- Clear separation of layers (Domain / Application / Infrastructure / API)
- Use-case centered design
- Testable components (unit + integration tests)
- Swagger UI with XML comments and JWT authentication support
- Health checks endpoint (`/health`)
- OpenTelemetry hooks for tracing and metrics
- Docker-ready API image
- Configuration helpers for User Secrets and Azure App Configuration
- Database persistence examples and test seeding helpers
- CI pipeline for automated build/test/image steps (see "CI flow" below)

## Database implementation flow

This project provides an end-to-end persistence flow, implemented with an EF Core DbContext and repository/unit-of-work patterns to keep persistence details outside the core use cases.

Highlights:
- DbContext: The infrastructure layer contains a DbContext implementation (e.g., `FleetContext`) which configures EF Core to use SQLite by default (see OnConfiguring -> `optionsBuilder.UseSqlite("Data Source={DbPath}")`). This makes the project lightweight and easy to run locally or in CI/tests.
- Repository pattern: Repositories implement persistence operations (Add, Get, Update, etc.) and translate between domain DTOs and persistence entities.
- Unit of Work: A Unit of Work abstraction (`IUnitOfWork`) centralizes transaction commits via a `Save()` method which calls `SaveChangesAsync()` on the DbContext. Use cases invoke the Unit of Work to ensure atomic persistence of changes across repositories.
- Test harness & data seeding: Tests include test DbContext helpers and seed data routines (used by repository and integration tests). Integration tests use a WebApplicationFixture and an in-process HTTP client to exercise controllers and the full stack.
- Local/CI configuration: The infrastructure and host setup load configuration from JSON/appsettings, environment variables and support user secrets / centralized configuration sources, allowing the DbPath or connection strings to be overridden for different environments.
- Extensibility: Although SQLite is used by default for ease of testing, the repository/DbContext wiring is contained in the infrastructure layer so replacing or configuring a different EF Core provider (e.g., SQL Server, PostgreSQL) is straightforward.

> Note: Database migrations are not required for the example local SQLite flow, but the code is structured so EF Core migrations can be added to support production DB providers.

## CI flow (implemented)

A Continuous Integration flow is implemented to automate build, test and packaging tasks. Typical CI stages supported by the repository layout and tests are:

1. Checkout & restore: fetch source and restore NuGet packages.
2. Build: `dotnet build` to compile projects.
3. Tests:
   - Unit tests: run repository/service unit tests.
   - Integration tests: run integration tests that exercise controllers and the full stack using the WebApplicationFixture and test DbContext (SQLite).
4. Static analysis / code quality checks: optional analyzers or linters can be run if configured.
5. Docker image build: build the container using `src/MyCompany.Microservice.Api/Dockerfile`.
6. Publish artifacts: collect test results, build outputs and (optionally) publish images to a registry.
7. Status reporting: CI status reports back to PRs/commits.

The repository is CI-ready: tests are deterministic (seeded test data), and the Dockerfile supports image build steps in the pipeline. Add or adapt a provider-specific workflow (GitHub Actions, Azure Pipelines, GitLab CI) to match your environment and to implement registry pushes and deployment steps.

## Startup & configuration

- Program & Host: `Program` contains the host bootstrap; `HostBuilderExtensions` provides helper methods to wire up user secrets and configuration sources (appsettings, env vars, Azure App Configuration integration hooks).
- Startup: `Startup.ConfigureServices` registers controllers, JSON options (camelCase, enum handling, number handling), custom API versioning, Swagger, ProblemDetails, health checks, application artifacts and OpenTelemetry.
- Swagger: Configured to include XML comments and to expose a JWT security definition (so secured endpoints can be exercised in the UI).
- Observability: OpenTelemetry is wired up with Azure Monitor exporters when an Application Insights connection string is present.
- Error handling: ProblemDetails middleware and centralized exception handling are used to present standardized error responses.

## How to build & run

Using the .NET CLI:
```bash
# from repository root
dotnet build
dotnet test
# run the API (from the API project)
cd src/MyCompany.Microservice.Api
dotnet run
```

Using Docker:
```bash
# build image
docker build -t mycompany-microservice -f src/MyCompany.Microservice.Api/Dockerfile .

# run container (example)
docker run -e ASPNETCORE_URLS=http://+:8080 -p 8080:8080 mycompany-microservice
```

After the API is running, open Swagger UI (for example: `http://localhost:8080/swagger`) to explore endpoints, versions and try secured endpoints (if configured with a token).

## Recommended Next Steps

- Provide or update User Secrets and centralized configuration values for local and cloud environments (e.g., connection strings, telemetry keys).
- Replace SQLite with a production database provider and add EF Core migrations if needed.
- Add or refine CI workflow files to push images to your container registry and to implement CD steps.
- Harden security (authentication/authorization), logging, and scaling for production readiness.

## Contributing

This repository is intended as a learning and starter template. Contributions are welcome—prefer changes that keep the architecture goals intact (isolated use cases, dependency inversion). Open a PR describing the motivation and changes.

## License

Intended for demonstration and educational use. Check the repository root for a license file.

## AI assistance disclosure

This README file (README.md) was generated with assistance from an AI tool. Only this README file was created using AI help; all other files and code in the repository were implemented by the project authors.
````