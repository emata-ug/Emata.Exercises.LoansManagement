# Module Structure

Each module in the Loans Management system follows a consistent organizational pattern based on Domain-Driven Design (DDD) principles and Clean Architecture concepts.

## Standard Module Layout

Every module is organized into three main folders that separate concerns:

```
ModuleName/
├── Domain/              # Business entities and domain logic
├── Infrastructure/      # Technical implementation details
│   ├── Data/           # Entity Framework related code
│   │   ├── DbContext           # Database context
│   │   ├── EntityConfigurations/  # EF entity configurations
│   │   └── Migrations/         # Database migrations
│   └── Configuration/  # Module-specific configuration (if needed)
└── Presentation/       # API endpoints and controllers
```

## Folder Responsibilities

### Domain Folder
**Purpose**: Contains the core business entities and domain logic

**Contents**:
- **Entities**: Plain C# classes representing business concepts (e.g., `Borrower.cs`, `Loan.cs`, `Address.cs`)
- **Value Objects**: Immutable objects that describe characteristics of entities
- **Domain Services**: Business logic that doesn't naturally fit within entities
- **Domain Events**: Events that represent important business occurrences

**Key Principles**:
- No dependencies on external frameworks
- Contains pure business logic
- Framework-agnostic code

### Infrastructure Folder
**Purpose**: Handles technical implementation details and external integrations

**Structure**:

#### Data Subfolder
Contains Entity Framework Core related components:

- **DbContext**: Database context class that manages entity tracking and database operations
  - Example: `BorrowersDbContext.cs`, `LoansDbContext.cs`
- **EntityConfigurations/**: EF Core entity type configurations
  - Fluent API configurations for each entity
  - Database table mappings, constraints, and relationships
- **Migrations/**: EF Core database migration files
  - Version-controlled database schema changes
  - Auto-generated migration code

#### Configuration Subfolder (when present)
- Module-specific configuration classes
- Dependency injection setup
- Infrastructure service registrations

### Presentation Folder
**Purpose**: Exposes module functionality through HTTP endpoints

**Contents**:
- **Endpoint Classes**: Classes implementing `IEndpoints` interface
- **API Contracts**: Request/Response DTOs specific to the module
- **Endpoint Registration**: Methods to configure routing and HTTP verbs

**Key Features**:
- RESTful endpoint design
- Minimal API approach using .NET's minimal APIs
- Clear separation between HTTP concerns and business logic

## Module Examples

### Borrowers Module Structure
```
Emata.Exercise.LoansManagement.Borrowers/
├── Domain/
│   ├── Borrower.cs         # Main borrower entity
│   ├── Partner.cs          # Partner entity
│   └── Address.cs          # Address value object
├── Infrastructure/
│   └── Data/
│       ├── BorrowersDbContext.cs
│       ├── EntityConfigurations/
│       └── Migrations/
└── Presentation/
    ├── BorrowersEndpoints.cs
    └── PartnersEndpoints.cs
```

### Loans Module Structure
```
Emata.Exercise.LoansManagement.Loans/
├── Domain/
│   └── Loan.cs             # Main loan entity
├── Infrastructure/
│   └── Data/
│       ├── LoansDbContext.cs
│       ├── EntityConfigurations/
│       └── Migrations/
└── LoansEndpoints.cs       # Endpoints at module root
```

### Shared Module Structure
```
Emata.Exercise.LoansManagement.Shared/
├── Infrastructure/
│   └── EntityCoreExtensions.cs  # Common EF extensions
└── Endpoints/
    ├── IEndpoints.cs             # Endpoint interface
    └── EndpointConfigurationExtensions.cs  # Registration utilities
```

## Benefits of This Structure

1. **Separation of Concerns**: Each folder has a single, well-defined responsibility
2. **Testability**: Domain logic is isolated from infrastructure concerns
3. **Maintainability**: Consistent organization across all modules
4. **Scalability**: Easy to add new modules following the same pattern
5. **Database Independence**: Domain entities are not coupled to specific database implementations