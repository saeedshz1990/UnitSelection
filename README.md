# UnitSelection

A sample university unit selection system built with ASP.NET Core and Entity Framework Core.

The project demonstrates a layered architecture with domain entities, services, repositories, migrations, REST API endpoints, dependency injection with Autofac, and automated tests.

## Features

- Manage students
- Manage teachers
- Manage terms
- Manage classes
- Manage courses
- Select units for students
- Retrieve selected units by term
- RESTful API endpoints
- SQL Server persistence
- Database migrations
- Unit and specification tests

## Technologies

- .NET 6
- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- Autofac
- Swagger / OpenAPI
- xUnit / test projects
- FluentMigrator

## Solution Structure

```text
src/
├── UnitSelection.Entities
├── UnitSelection.Services
├── UnitSelection.Infrastructure
├── UnitSelection.Persistence.EF
├── UnitSelection.Migrations
├── UnitSelection.RestApi
├── UnitSelection.Handlers.Specs
├── UnitSelection.Handlers.Tests.Unit
├── UnitSelection.Services.Test.Unit
├── UnitSelection.Specs
└── UnitSelection.TestTools
```

## API Areas

The REST API contains endpoints for:

```text
/api/students
/api/teachers
/api/terms
/api/classes
/api/courses
/api/chooseUnits
```

## Configuration

The project uses SQL Server.

Example local configuration:

```json
{
  "ConnectionString": "Server=.;Database=UnitSelection;Trusted_Connection=True;"
}
```

Test projects use a separate local database:

```text
UnitSelectionTest
```

## Build

From the `src` directory:

```bash
dotnet restore UnitSelection.sln
dotnet build UnitSelection.sln
```

## Run Tests

```bash
dotnet test UnitSelection.sln
```

## Run API

```bash
dotnet run --project UnitSelection.RestApi
```

Swagger is available in the Development environment.

## Database Migrations

The solution contains a dedicated migration project:

```text
UnitSelection.Migrations
```

This project creates the database when necessary and runs the configured migrations.

## Architecture

The solution separates responsibilities into:

```text
REST API
   ↓
Services
   ↓
Repositories / Unit of Work
   ↓
Entity Framework Core
   ↓
SQL Server
```

Autofac is used for dependency injection and service registration.

## Purpose

This repository is a learning and architecture sample focused on layered design, dependency injection, persistence, automated testing, and REST API development in .NET.