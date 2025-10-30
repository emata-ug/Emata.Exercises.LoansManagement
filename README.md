# Emata.Exercises.LoansManagement

> ⚠️ **CAUTION - FICTITIOUS PROJECT** ⚠️
>
> This is a **fictional project** created exclusively for **interview purposes** and technical assessments.
> It does **NOT** represent any real-world application, actual business requirements, or production systems.
> The scenarios, business logic, and requirements presented here are purely hypothetical and should not be
> considered as reflecting real banking, financial services, or loan management practices.

## Project Overview

This project is a loan management system exercise designed to demonstrate software development skills and architectural decisions in a .NET environment.

## Project Structure

```text
├── src/
│   ├── Emata.Exercises.LoanManagement.API/          # Web API project
│   └── Emata.Exercises.LoanManagement.Notifications/ # Notifications service
├── tests/
│   └── Emata.Exercises.LoanManagement.IntegrationTests/ # Integration tests
├── Emata.Exercises.LoansManagement.sln              # Solution file
└── README.md                                         # This file
```

## Technology Stack

- **.NET 9.0** - Latest .NET framework
- **ASP.NET Core** - Web API framework
- **C#** - Primary programming language

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022 or VS Code
- Git

### Building the Project

```bash
# Clone the repository
git clone <repository-url>

# Navigate to project directory
cd Emata.Exercises.LoansManagement

# Restore dependencies
dotnet restore

# Build the solution
dotnet build
```

### Running the API

```bash
# Navigate to API project
cd src/Emata.Exercises.LoanManagement.API

# Run the application
dotnet run
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

**📋 For detailed testing information, see the [Testing Guide](docs/testing.md)**

## API Endpoints

The API will be available at `https://localhost:7xxx` when running locally. Check the `Properties/launchSettings.json` file for exact port numbers.

## Documentation

Comprehensive documentation is available in the `/docs` folder:

### 📚 Documentation Menu

- **[🏗️ Architecture Overview](docs/architecture.md)** - Learn about the modular monolith architecture, what it means, and how the system is organized into modules
- **[📁 Module Structure](docs/module-structure.md)** - Understand how each module is structured with Domain, Infrastructure, and Presentation layers
- **[🌐 API Endpoints](docs/api-endpoints.md)** - Complete reference of all available API endpoints and their usage
- **[🧪 Testing Guide](docs/testing.md)** - Comprehensive guide on testing requirements, tools, and how to run tests

### Quick Navigation

| Topic | Description | Link |
|-------|-------------|------|
| **System Architecture** | Modular monolith pattern, module overview | [architecture.md](docs/architecture.md) |
| **Code Organization** | Domain/Infrastructure/Presentation structure | [module-structure.md](docs/module-structure.md) |
| **API Reference** | Endpoints, request/response formats | [api-endpoints.md](docs/api-endpoints.md) |
| **Testing** | Testing tools, requirements, and how to run tests | [testing.md](docs/testing.md) |

## Development Notes

This project serves as a technical exercise and may include:

- RESTful API design patterns
- Clean architecture principles
- Unit and integration testing
- Dependency injection
- Configuration management
- Logging and monitoring concepts

## Disclaimer

**This project is created solely for educational and interview assessment purposes. Any resemblance to real business processes, actual companies, or production systems is purely coincidental.**
