# GrillMaster

GrillMaster is a .NET Web API that generates optimized grill session plans from menu data using a bin-packing algorithm.

## Overview

This solution follows a layered, use-case driven architecture (Clean/Onion-like). Important: the repository enforces a strict dependency order between projects to guarantee connections and boundaries according to the design pattern — higher-level projects depend on lower-level projects only (API -> Application -> Domain -> Infrastructure).

The core packing algorithm is a Guillotine bin-packer (GuillotinePacker) used to group grill items into sessions efficiently.

## Folder structure (key folders)

- GrillMaster.API/ — ASP.NET Core Web API, controllers, program startup.
- GrillMaster.Application/ — Use cases and DTOs.
- GrillMaster.Domain/ — Domain entities, value objects, and interfaces.
- GrillMaster.Infraestructure/ — Infrastructure implementations (external API client, packers).
- GrillMaster.Test/ — Unit tests.

## Project structure (detailed)

```
GrillMaster.Application
├── DTOs
│   ├── GrillItemDto.cs
│   ├── GrillPlanDto.cs
│   └── MenuRequestDto.cs
└── UseCases
	└── PlanGrillSessionsUseCase.cs

GrillMaster.Domain
├── Abstractions
│   └── IBinPackingService.cs
├── Constants
│   └── GrillConstants.cs
├── Entities
│   ├── GrillItem.cs
│   └── GrillSession.cs
└── ValueObjects
	└── Placement.cs

GrillMaster.Infraestructure
├── DependencyInjection.cs
└── Packaging
	├── FreeRectangle.cs
	└── GuillotinePacker.cs

GrillMaster.API
├── Controller
│   └── GrillController.cs
├── Mappers
│   └── MenuRequestMapper.cs
├── Properties
│   └── launchSettings.json
├── appsettings.json
├── appsettings.Development.json
└── GrillMaster.API.http

Projects in the solution
- GrillMaster.Domain\GrillMaster.Domain.csproj
- GrillMaster.Application\GrillMaster.Application.csproj
- GrillMaster.Infraestructure\GrillMaster.Infraestructure.csproj
- GrillMaster.API\GrillMaster.API.csproj
```

## Requirements

- .NET 10 SDK (target framework: net10.0)
- Optional: IDE such as Visual Studio or VS Code

## How to run (development)

1. From the solution root restore and build:

```bash
dotnet restore
dotnet build
```

2. Run the API project:

```bash
dotnet run --project GrillMaster.API
```

3. Open the API endpoint in your browser or HTTP client:

- HTTPS: https://localhost:7044/api/grill/plan
- HTTP:  http://localhost:5024/api/grill/plan

The API will fetch menus from the configured external menu client and return an optimized grill plan.

## Testing

Run unit tests:

```bash
dotnet test
```

## API

- GET /api/grill/plan
  - Description: Fetches menus from the external API and returns optimized grill sessions.
  - Response: JSON with grill plan DTOs (GrillPlanDto / GrillSummaryDto).

## Known issues and troubleshooting

- CS0311 error when registering HTTP client:

If you see an error like:

```text
The type 'MenuApiClient' cannot be used as type parameter 'TImplementation' ... no implicit reference conversion to IMenuApiClient
```

- Ensure `MenuApiClient` implements `IMenuApiClient` and both types belong to the same interface definition and assembly.
- Or register the concrete client if you do not need the interface:

```csharp
services.AddHttpClient<MenuApiClient>();
```

## Notes

- The packing algorithm sorts items by area (descending) to improve packing efficiency.
- The API controller uses dependency-injected UseCase and IMenuApiClient to separate concerns and keep testability.
- Replace the external menu client with a mock implementation for local testing or CI.

