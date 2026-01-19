# RetireTime - Project Guidelines

## Project Overview

RetireTime is a retirement planning tool designed to be user-friendly with a simple question/answer prompt interface similar to TurboTax. This application is targeted at customers who want to plan their retirement independently, without requiring a financial planner.

## Technology Stack

- **Framework**: Blazor Web App (C#)
- **Database**: PostgreSQL
- **Architecture Pattern**: Clean Architecture
- **Communication Pattern**: MediatR (CQRS)
- **Authentication**: Session-based with cookies

## Project Structure

The solution follows Clean Architecture principles and is divided into four distinct projects:

### 1. RetirementTime (Presentation Layer)
- **Purpose**: User interface and presentation logic
- **Responsibilities**:
  - Blazor components and pages
  - UI/UX implementation
  - Sending queries/commands to the Application layer via MediatR
  - Handling user input and displaying results
- **Dependencies**: Application layer
- **Location**: `/RetirementTime/`

### 2. RetirementTime.Application (Application Layer)
- **Purpose**: Application business logic and use cases
- **Responsibilities**:
  - MediatR handlers (Commands and Queries)
  - Orchestrating calls between Domain and Infrastructure
  - Application-specific DTOs and ViewModels
  - Converting between entities and view models
- **Dependencies**: Domain layer, Infrastructure layer (via interfaces)
- **Location**: `/RetirementTime.Application/`
- **Pattern**: All use cases follow the MediatR pattern with separate Query/Command and Handler files

### 3. RetirementTime.Domain (Domain Layer)
- **Purpose**: Core business logic and domain models
- **Responsibilities**:
  - Entity definitions
  - Business logic services
  - Domain interfaces (repository interfaces, service interfaces)
  - Domain-specific exceptions
- **Dependencies**: None (pure domain logic)
- **Location**: `/RetirementTime.Domain/`

### 4. RetirementTime.Infrastructure (Infrastructure Layer)
- **Purpose**: Data persistence and external concerns
- **Responsibilities**:
  - Database context (ApplicationDbContext)
  - Entity Framework Core migrations
  - Repository implementations
  - Database configuration (entity configurations using Fluent API)
  - All database calls and data access logic
- **Dependencies**: Domain layer (for entities and interfaces)
- **Database**: PostgreSQL with Entity Framework Core
- **Location**: `/RetirementTime.Infrastructure/`

## Architecture Flow

```
User Input → Presentation Layer (Blazor Components)
    ↓ (MediatR Query/Command)
Application Layer (Handlers)
    ↓ (Domain Services)      ↓ (Repository Interfaces)
Domain Layer (Business Logic) ← Infrastructure Layer (DB Access)
```

## Key Architectural Decisions

### Dependency Injection
- Each layer registers its dependencies via a `DependencyInjection.cs` file
- Infrastructure services are registered in `RetirementTime.Infrastructure.DependencyInjection`
- Application services are registered in `RetirementTime.Application.DependencyInjection`
- All registrations are called from `Program.cs` in the Presentation layer

### MediatR Pattern
- All communication from Presentation to Application layer uses MediatR
- Each use case has:
  - A Query/Command class (implements `IRequest<TResponse>`)
  - A Handler class (implements `IRequestHandler<TRequest, TResponse>`)
- Example structure: `Features/{Feature}/{Action}/{Action}Query.cs` and `{Action}Handler.cs`

### Database Access
- **All database operations** must go through the Infrastructure layer
- Repositories implement interfaces defined in the Domain layer
- The ApplicationDbContext is only accessed within the Infrastructure layer
- Entity configurations are done using Fluent API in `ApplicationDbContext.OnModelCreating`

### Entity Configuration Standards
- Table names: lowercase with underscores (e.g., `real_estate`, `buy_or_rent`)
- Primary keys: Use `Id` property (auto-generated) or specific names like `SessionId`
- Timestamps: Use `CreatedAt` with default `NOW() AT TIME ZONE 'UTC'`
- For expiration/validity: Use `ValidUntil` (e.g., Session entity: current timestamp + 30 minutes)
- Foreign keys: Named as `{Entity}Id` (e.g., `UserId`, `CountryId`)
- Navigation properties: Required entities use `required` keyword

### Data Flow and Conversion
- **Entities**: Defined in Domain layer, used throughout the application
- **ViewModels/DTOs**: Created in Application layer when needed
- **Conversion**: Entity to ViewModel conversion happens in the Application layer

### Authentication
- Session-based authentication using the `Session` entity
- Sessions stored in database with:
  - `SessionId` (primary key)
  - `UserId` (foreign key to User)
  - `CreatedAt` (timestamp)
  - `ValidUntil` (expires 30 minutes after creation)
- Session tokens stored in browser cookies (cookie name: `RetireTime_Session`)
- **AuthService** (`RetirementTime/Services/AuthService.cs`):
  - `IsAuthenticated()`: Validates session by calling Application layer
  - `SetSessionToken()`: Stores session token in cookie with 30-minute expiration
  - `GetSessionToken()`: Retrieves session token from cookie
  - `ClearSession()`: Removes session cookie
- **Protected Pages**:
  - Pages requiring authentication must inherit from `AuthenticatedComponentBase`
  - Automatically redirects to home page if session is invalid
  - Example: `@inherits RetirementTime.Components.AuthenticatedComponentBase`
- **Session Validation**:
  - ValidateSession feature in Application layer
  - Checks session validity against database
  - Returns `IsValid` and `UserId` on success

### Configuration
- `appsettings.json` in the RetirementTime project contains all configuration
- Infrastructure layer receives `IConfiguration` via dependency injection
- Connection strings accessed via `configuration.GetConnectionString("DefaultConnection")`

### Error Handling and Logging
- **All handlers** in the Application layer must implement `ILogger<THandler>`
- Handlers must be declared as `partial class` to support LoggerMessage attributes
- Use `[LoggerMessage]` attribute for all logging statements for better performance
- **Logging pattern**:
  - Log at `Information` level at the start of the handler
  - Log at `Information` level on successful completion
  - Log at `Error` level when exceptions occur
- **Exception handling**:
  - All handler logic must be wrapped in try-catch blocks
  - Catch generic `Exception` to handle all errors
  - Log the exception message using `ex.Message` parameter
  - Return user-friendly error messages (never expose technical details)
  - For query handlers returning lists, return empty list on error
  - For command handlers, return result with `Success = false` and generic error message

**Example Handler Pattern**:
```csharp
public partial class ExampleHandler(
    IRepository repository,
    ILogger<ExampleHandler> logger) : IRequestHandler<ExampleCommand, ExampleResult>
{
    public async Task<ExampleResult> Handle(ExampleCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            // Handler logic here
            var result = await repository.DoSomething(request.Id);
            
            LogSuccessfullyCompleted(logger, result.Id);
            
            return new ExampleResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new ExampleResult 
            { 
                Success = false, 
                ErrorMessage = "An error occurred. Please try again later." 
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<ExampleHandler> logger, int Id);

    [LoggerMessage(LogLevel.Information, "Successfully completed for ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<ExampleHandler> logger, int Id);

    [LoggerMessage(LogLevel.Error, "Error occurred for ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<ExampleHandler> logger, string Exception, int Id);
}
```

### Result Objects
- All command/query results should inherit from `BaseResult` (when applicable)
- `BaseResult` contains:
  - `Success` (bool): Indicates if the operation succeeded
  - `ErrorMessage` (string?): User-friendly error message when `Success = false`
- Extend `BaseResult` with operation-specific properties (e.g., `UserId`, `Data`)

## Domain Entities

### User Management
- **User**: User account information
- **Role**: User roles for authorization
- **Session**: Authentication sessions (30-minute validity)

### Location
- **Country**: Countries with subdivisions
- **Subdivision**: States/provinces within countries

### Real Estate
- **RealEstate**: Property information and costs
- **Mortgage**: Mortgage details and calculations
- **Rent**: Rental property information
- **BuyOrRent**: Comparison analysis between buying and renting

## Database Migrations

### Running Migrations
```bash
# Create a new migration
dotnet ef migrations add MigrationName --project RetirementTime.Infrastructure --startup-project RetirementTime

# Apply migrations to database
dotnet ef database update --project RetirementTime.Infrastructure --startup-project RetirementTime
```

### Seeding Data
- Data seeding is done in migrations using `migrationBuilder.InsertData()`
- Use SQL `NOW() AT TIME ZONE 'UTC'` for timestamp fields
- Reference foreign keys by their `Id` values
- Roles are seeded in `ApplicationDbContext.SeedRoleData()`:
  - Role ID 1: "User" (default role for new users)
  - Role ID 2: "Admin" (administrator role)

## Naming Conventions

### C# Entities
- **Singular** form (e.g., `User`, `Country`, `RealEstate`)
- PascalCase for class names and properties

### Database Tables
- Lowercase with underscores (e.g., `user`, `real_estate`)
- Typically singular form to match entity names

### DbSet Properties
- **Plural** form (e.g., `Users`, `Countries`, `RealEstates`)

### Repository Classes
- **Singular** form (e.g., `UserRepository`, `CountryRepository`)
- Implements interface from Domain layer (e.g., `IUserRepository`)

## Common Patterns

### Creating a New Feature
1. Define entity in `RetirementTime.Domain/Entities/`
2. Add entity configuration in `ApplicationDbContext.OnModelCreating()`
3. Add `DbSet<TEntity>` property in `ApplicationDbContext`
4. Create and run migration
5. Define repository interface in `RetirementTime.Domain/Interfaces/Repositories/`
6. Implement repository in `RetirementTime.Infrastructure/Repositories/`
7. Register repository in `RetirementTime.Infrastructure/DependencyInjection.cs`
8. Create Query/Command and Handler in `RetirementTime.Application/Features/`
9. Create Blazor component/page in `RetirementTime/Components/`

### Accessing Data in Blazor Components
1. Inject `IMediator` into the component
2. Create a Query/Command instance
3. Call `await Mediator.Send(query)`
4. Display results in UI

Example:
```csharp
[Inject] private IMediator Mediator { get; set; } = default!;

protected override async Task OnInitializedAsync()
{
    var countries = await Mediator.Send(new GetCountriesQuery());
    // Use countries in UI
}
```

## Development Guidelines

### Code Organization
- Keep concerns separated according to layer responsibilities
- Never reference Infrastructure directly from Presentation
- Use interfaces for all cross-layer dependencies
- Keep business logic in Domain services, not in repositories

### Error Handling
- Define custom exceptions in `RetirementTime.Application/Exceptions/`
- Handle exceptions at the appropriate layer
- Return meaningful error messages to users

### Testing Considerations
- Domain layer should be easily testable (no external dependencies)
- Use interfaces to enable mocking in tests
- Repository implementations can be tested against a test database

## User Experience Goals
- Simple, guided question/answer flow (TurboTax-style)
- Clear, non-technical language
- Step-by-step retirement planning process
- Empower users to make informed decisions without professional help

---

**Last Updated**: January 17, 2026

