# Application Architecture

## Modular Monolith Pattern

Single deployable application organized into business domain modules:
- **Single Deployment**: One unit, simplified operations
- **Modular Organization**: Domain-based modules with clear boundaries  
- **Shared Infrastructure**: Common database access, logging, configuration

Provides better organization than traditional monoliths while avoiding microservices complexity.

## System Modules

```mermaid
graph LR
    subgraph API["🌐 API Project"]
        EP[Endpoints]
        SV[Services]
        MG[Migrations]
    end
    
    subgraph BM["👥 Borrowers Module"]
        BE[Borrower Entity]
        PA[Partner Entity] 
        AD[Address Entity]
        BDB[(Borrowers DB)]
    end
    
    subgraph LM["💰 Loans Module"]
        LE[Loan Entity]
        IR[InterestRate]
        DU[Duration]
        LDB[(Loans DB)]
    end
    
    subgraph SM["🔧 Shared Module"]
        IE[IEndpoints]
        EX[Extensions]
        CM[Common Utils]
    end
    
    API --> BM
    API --> LM
    API --> SM
    BM --> BDB
    LM --> LDB
```

### Borrowers Module
- **Entities**: `Borrower`, `Partner`, `Address`
- **Purpose**: Borrower and partner management

### Loans Module  
- **Entities**: `Loan` with `InterestRate`, `Duration`
- **Purpose**: Loan operations and tracking

### Shared Module
- **Components**: `IEndpoints`, common extensions
- **Purpose**: Cross-cutting infrastructure utilities

## API Integration Flow

```mermaid
sequenceDiagram
    participant C as Client
    participant A as API
    participant B as Borrowers Module
    participant L as Loans Module
    participant D as Database
    
    C->>A: HTTP Request
    A->>A: Route to Module
    alt Borrowers Endpoint
        A->>B: Handle Request
        B->>D: Query/Update
        D-->>B: Data
        B-->>A: Response
    else Loans Endpoint
        A->>L: Handle Request
        L->>D: Query/Update  
        D-->>L: Data
        L-->>A: Response
    end
    A-->>C: HTTP Response
```

Each module registers via extension methods:
- `AddBorrowersModule(configuration)`
- `AddLoansModule(configuration)` 
- Auto-migration at startup