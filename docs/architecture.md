# Application Architecture

## Overview

This loans management system is built using a **modular monolith** architectural pattern, which combines the simplicity of a monolithic deployment with the organizational benefits of modular design.

## What is a Modular Monolith?

A modular monolith is an architectural approach where:

- **Single Deployment Unit**: The entire application is deployed as one unit, simplifying deployment and operations
- **Modular Organization**: Code is organized into distinct, loosely-coupled modules based on business domains
- **Clear Module Boundaries**: Each module has well-defined responsibilities and interfaces
- **Shared Infrastructure**: Common concerns like database access, logging, and configuration are shared across modules

This approach provides a middle ground between traditional monoliths and microservices, offering better organization and maintainability while avoiding the complexity of distributed systems.

## Application Modules

The Loans Management system consists of three main modules:

### 1. Borrowers Module
**Purpose**: Manages borrower information and partner relationships

**Key Entities**:
- `Borrower`: Represents individuals or entities applying for loans
- `Partner`: Represents business partners in the loan process
- `Address`: Represents physical addresses for borrowers

**Responsibilities**:
- Borrower registration and management
- Partner management
- Address information handling

### 2. Loans Module
**Purpose**: Handles all loan-related operations and data

**Key Entities**:
- `Loan`: Represents individual loan records with terms, amounts, and status

**Responsibilities**:
- Loan creation and management
- Loan status tracking
- Loan data retrieval and reporting

### 3. Shared Module
**Purpose**: Provides common infrastructure and utilities used across all modules

**Key Components**:
- Common endpoint abstractions
- Shared infrastructure components
- Cross-cutting concerns

**Responsibilities**:
- Endpoint configuration utilities
- Shared data access patterns
- Common infrastructure services

## Module Integration

All modules are integrated into the main API project (`Emata.Exercise.LoansManagement.API`) which:
- Registers all module services
- Configures module endpoints
- Handles module database migrations
- Provides a unified API surface

The integration follows a plugin-like pattern where each module exposes extension methods to register its services and endpoints with the main application.