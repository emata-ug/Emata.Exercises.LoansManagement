# API Endpoints

This document describes all available API endpoints in the Loans Management system. The API follows RESTful principles and is built using .NET Minimal APIs.

## Base URL

When running locally, the API is available at:
- **HTTPS**: `https://localhost:7xxx` (port varies - check `Properties/launchSettings.json`)
- **HTTP**: `http://localhost:5xxx` (port varies - check `Properties/launchSettings.json`)

## API Documentation

The application provides interactive API documentation through:
- **OpenAPI/Swagger**: Available at `/openapi/v1.json`
- **Scalar UI**: Available at `/scalar/v1` (in development mode)

## Module Endpoints

### Borrowers Module

**Base Path**: `/borrowers`

#### Health Check
- **Endpoint**: `GET /borrowers/health`
- **Description**: Checks the health status of the Borrowers API
- **Response**: `200 OK` with message "Borrowers API is healthy"
- **Use Case**: Service health monitoring and diagnostics

**Data Models**:
- **Borrower Entity**: Contains borrower personal information
  - `Id`: Unique identifier
  - `Surname`: Last name
  - `GivenName`: First name
  - `DateOfBirth`: Birth date
  - `IdentificationNumber`: Government ID number
  - `PhoneNumber`: Contact phone
  - `Email`: Contact email
  - `Address`: Physical address (linked to Address entity)
  - `PartnerId`: Associated partner reference

### Partners Module

**Base Path**: `/partners`

#### Health Check
- **Endpoint**: `GET /partners/health`
- **Description**: Checks the health status of the Partners API
- **Response**: `200 OK` with message "Partners API is healthy"
- **Use Case**: Service health monitoring and diagnostics

**Data Models**:
- **Partner Entity**: Represents business partners
  - `Id`: Unique identifier
  - `Name`: Partner organization name
  - `Address`: Physical address (linked to Address entity)

**Available DTOs**:
- **PartnerDTO**: Data transfer object for partner information
  - `Id`: Partner identifier
  - `Name`: Partner name
  - `Town`: Partner location

### Loans Module

**Base Path**: `/loans`

#### Health Check
- **Endpoint**: `GET /loans/health`
- **Description**: Checks the health status of the Loans API
- **Response**: `200 OK` with message "Loans API is healthy"
- **Use Case**: Service health monitoring and diagnostics

#### Get All Loans
- **Endpoint**: `GET /loans`
- **Description**: Retrieves a list of all loans in the system
- **Response**: `200 OK` with array of loan objects
- **Use Case**: Loan portfolio overview and reporting

#### Get Loan by ID
- **Endpoint**: `GET /loans/{id}`
- **Parameters**:
  - `id` (path): Integer - The unique identifier of the loan
- **Description**: Retrieves a specific loan by its ID
- **Responses**:
  - `200 OK`: Loan found and returned
  - `404 Not Found`: Loan with specified ID does not exist
- **Use Case**: Loan detail view and individual loan management

**Data Models**:
- **Loan Entity**: Core loan information
  - `Id`: Unique identifier
  - `LoanAmount`: Principal loan amount (decimal)
  - `IssueDate`: Date loan was issued
  - `InterestRate`: Interest rate details (object)
    - `Rate`: Interest rate percentage
    - `Period`: Rate calculation period (Annual/Monthly/Weekly/Daily)
  - `Duration`: Loan term details (object)
    - `Length`: Duration value
    - `Period`: Duration unit (Annual/Monthly/Weekly/Daily)
  - `LoanApplicationId`: Reference to loan application
  - `CreatedOn`: Record creation timestamp

**Available DTOs**:
- **LoanItem**: Data transfer object for loan information (currently empty - placeholder for future development)

## Common Components

### Address Entity
Shared across Borrowers and Partners modules:
- `Town`: Required city/town name
- `District`: Optional district/area
- `Region`: Optional region/state
- `Country`: Optional country

### Period Enumeration
Used in loan calculations:
- `Annual` (0): Yearly periods
- `Monthly` (1): Monthly periods  
- `Weekly` (2): Weekly periods
- `Daily` (3): Daily periods

## Authentication & Authorization

Currently, the API does not implement authentication or authorization. All endpoints are publicly accessible.

## Error Handling

The API uses standard HTTP status codes:
- `200 OK`: Successful requests
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Server-side errors

## Development Notes

- API uses Entity Framework Core for data persistence
- Each module maintains its own database context
- Database migrations are applied automatically at application startup
- The API is configured for development with detailed error messages and API documentation