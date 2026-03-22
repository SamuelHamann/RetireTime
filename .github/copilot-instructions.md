# RetireTime - Project Guidelines

## Project Overview

RetireTime is a retirement planning tool designed to be user-friendly with a simple question/answer prompt interface similar to TurboTax. This application is targeted at customers who want to plan their retirement independently, without requiring a financial planner.

### Brand Name
- **English**: NestEgg
- **French (fr-CA)**: Cagnotte
- The project/solution name remains `RetirementTime` — only the UI-facing brand name changes per language.
- Always use `NestEgg` in English UI text and resource files, and `Cagnotte` in French (`fr-CA`) resource files.

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
- **Shared Components**: `/RetirementTime/Components/Shared/`
  - **IMPORTANT**: Always check the Shared folder for reusable components before creating new ones
  - Available shared components:
    - `YesNoToggle` - Reusable Yes/No toggle buttons with customizable labels
  - Usage: `<YesNoToggle @bind-Value="@_myBoolValue" YesLabel="@Localizer["Yes"]" NoLabel="@Localizer["No"]" />`

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
- `BaseResult` is a **record type**, so all derived results must also be **record types**
- `BaseResult` contains:
  - `Success` (bool): Indicates if the operation succeeded
  - `ErrorMessage` (string?): User-friendly error message when `Success = false`
- Extend `BaseResult` with operation-specific properties (e.g., `UserId`, `Data`)
- Use `init` accessors for properties in record types

## Development Workflow

### Feature Planning and Implementation
**CRITICAL**: Before implementing any new feature (entities, repositories, handlers, etc.), ALWAYS:
1. **Create a detailed plan** including:
   - Database table names and structure
   - File names and locations for all layers
   - Data flow diagram
   - Repository methods needed
2. **Present the plan to the user for validation**
3. **Wait for explicit approval** before proceeding with implementation
4. This ensures alignment and prevents rework

**Example Plan Structure**:
- Tables: List all table names with column details
- Domain Layer: Entity files and interfaces
- Infrastructure Layer: Repository implementations, DbContext changes
- Application Layer: Commands/Queries, Handlers, Results
- Presentation Layer: Component changes
- Ask specific questions about naming, patterns, and approaches

## Component Development Best Practices

### CRITICAL: Always Check for Reusable Components First
Before creating any new UI component, **ALWAYS** check the `/RetirementTime/Components/Shared/` folder for existing reusable components.

**Available Shared Components:**
- `YesNoToggle` - Reusable Yes/No toggle buttons with customizable labels
  - Parameters: `Value` (bool), `YesLabel` (string), `NoLabel` (string)
  - Usage: `<YesNoToggle @bind-Value="@_myBoolValue" YesLabel="@Localizer["Yes"]" NoLabel="@Localizer["No"]" />`
  
**Component Development Checklist:**
1. ✅ Check `/Components/Shared/` for existing components
2. ✅ If a similar component exists, use it instead of creating a new one
3. ✅ If creating a new reusable component, place it in `/Components/Shared/`
4. ✅ Separate code into `.razor`, `.razor.cs`, and `.razor.css` files
5. ✅ Use localized strings from resource files (no hardcoded text)
6. ✅ Follow naming conventions for components and parameters

### Component File Structure
- **Markup**: `ComponentName.razor` - Contains only HTML/Razor markup
- **Logic**: `ComponentName.razor.cs` - Contains C# logic, parameters, and methods
- **Styles**: `ComponentName.razor.css` - Contains component-specific CSS (scoped)

**Example:**
```
/Components/Pages/BeginnerGuide/Assets/
  Step3OtherAssets.razor
  Step3OtherAssets.razor.cs
  Step3OtherAssets.razor.css
```

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

### Model Classes for UI Components
- **Location**: `/RetirementTime/Models/`
- **Structure**: Mirror the component folder structure
- **Pattern**: If a component is at `Components/Pages/BeginnerGuide/Components/Assets.razor`, its model should be at `Models/BeginnerGuide/Assets/`
- **Purpose**: UI-specific data models (not domain entities)
- **Example**:
  - Component: `RetirementTime/Components/Pages/BeginnerGuide/Components/Assets.razor`
  - Model: `RetirementTime/Models/BeginnerGuide/Assets/PropertyData.cs`
- **Guidelines**:
  - Create separate model files instead of nested classes in components
  - Use fully qualified type names (e.g., `System.DateTime`) or add using statements
  - Models are specific to UI needs and may differ from domain entities
  - Use for form data binding, view models, and component-specific data structures

### Form Models with Validation
- **CRITICAL**: When creating any form (login, signup, data entry, etc.), ALWAYS create a dedicated model class in the Models folder
- **Location Pattern**: 
  - Page: `Components/Pages/Auth/Login.razor`
  - Model: `Models/Auth/LoginModel.cs`
- **Required Components**:
  1. Model class in separate file (never nested in component)
  2. Data annotation validation attributes on all properties
  3. Mirror the component folder structure
  
**Validation Attributes to Use**:
- `[Required(ErrorMessage = "...")]` - For required fields
- `[EmailAddress(ErrorMessage = "...")]` - For email fields
- `[MinLength(n, ErrorMessage = "...")]` - For minimum length
- `[MaxLength(n, ErrorMessage = "...")]` - For maximum length
- `[Range(min, max, ErrorMessage = "...")]` - For numeric ranges
- `[RegularExpression(pattern, ErrorMessage = "...")]` - For custom patterns
- `[Compare("PropertyName", ErrorMessage = "...")]` - For password confirmation
- `[StringLength(max, MinimumLength = min, ErrorMessage = "...")]` - For string length
- `[Phone(ErrorMessage = "...")]` - For phone numbers
- `[Url(ErrorMessage = "...")]` - For URLs

**Example Form Model Structure**:
```csharp
using System.ComponentModel.DataAnnotations;

namespace RetirementTime.Models.Auth;

public class LoginModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}
```

**Usage in Blazor Component**:
```razor
@using RetirementTime.Models.Auth

<EditForm Model="@loginModel" OnValidSubmit="@HandleSubmit">
    <DataAnnotationsValidator />
    
    <div class="form-group">
        <label for="email">Email</label>
        <InputText id="email" @bind-Value="loginModel.Email" />
        <ValidationMessage For="@(() => loginModel.Email)" />
    </div>
    
    <button type="submit">Submit</button>
</EditForm>

@code {
    private LoginModel loginModel = new();
    
    private async Task HandleSubmit()
    {
        // Form is validated, process submission
    }
}
```

**Best Practices**:
- ✅ Always create models in separate files (not nested classes)
- ✅ Use descriptive, user-friendly error messages
- ✅ Initialize string properties to `string.Empty` to avoid null reference warnings
- ✅ Use `<DataAnnotationsValidator />` in EditForm
- ✅ Display validation messages with `<ValidationMessage For="..." />`
- ✅ Localize error messages when possible
- ✅ Keep validation logic in model, not in component code

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

## Introduction Pages

### Shared Stylesheet
All introduction step pages load `wwwroot/css/introduction.css` via `<HeadContent>` in `Introduction.razor`. This file contains all reusable primitives for the introduction flow:
- Step shell (`.intro-step`, `.step-title`, `.step-subtitle`, `.step-divider`, `.welcome-step`, `.welcome-body`, `.btn-start`)
- Form layout (`.intro-form`)
- Field group (`.field-group`, `.field-question`, `.field-sublabel`, `.field-row`, `.field-row-item`)
- Inputs (`.field-input`, `.field-input--date`, `.field-select`, `.field-error`)
- Animations (`.step-header-animate`, `.step-body-animate`)

**Do NOT** put step-specific or form styles in `Introduction.razor.css` — that file is reserved for page layout, the topbar breadcrumb, and nav action buttons only.

### Animation Pattern — REQUIRED for all intro steps
**EVERY** introduction step component must use the two-phase fade-in animation. This is mandatory:

1. Wrap the title (and subtitle if present) in `<div class="step-header-animate">` — fades in immediately.
2. Wrap everything below (divider, form, body text, buttons) in `<div class="step-body-animate">` — fades in after a `0.45s` delay.

**Standard structure for a step with a form:**
```razor
<div class="intro-step">
    <div class="step-header-animate">
        <h2 class="step-title">@Localizer["Intro_StepN_Title"]</h2>
        <p class="step-subtitle">@Localizer["Intro_StepN_Subtitle"]</p>
    </div>

    <div class="step-body-animate">
        <hr class="step-divider" />
        <EditForm Model="@Model" class="intro-form">
            <!-- fields -->
        </EditForm>
    </div>
</div>
```

**Standard structure for the welcome step (no divider/form):**
```razor
<div class="intro-step welcome-step">
    <div class="step-header-animate">
        <h2 class="step-title">@Localizer["Intro_Step1_Title"]</h2>
    </div>

    <div class="step-body-animate">
        <p class="welcome-body">...</p>
        <button class="btn-start" @onclick="OnStart">...</button>
    </div>
</div>
```

### Navigation Buttons
Back/Next/Finish buttons in `Introduction.razor` use `<span>` inside the button tag so the arrow hover effect works correctly:
```razor
<button class="btn-back" @onclick="PreviousStep"><span>@Localizer["Intro_Back"]</span></button>
<button class="btn-next" @onclick="NextStep"><span>@Localizer["Intro_Next"]</span></button>
```
On hover, the text slides out and an arrow (`←` / `→`) slides in via `::after` pseudo-element.

## Localization and Resource Files

### Resource File Organization
- **Location**: `/RetirementTime/Resources/`
- **Structure**: Organize resource files by feature/page area
- **Supported Languages**: English (Canadian - en-CA), French (Canadian - fr-CA)
- **Resource Types**:
  - `.resx` - Default/neutral culture (serves as English Canadian)
  - `.fr-CA.resx` - French Canadian translations
  - `.Designer.cs` - Auto-generated strongly-typed resource accessor classes

**IMPORTANT**: Do NOT create `.en-CA.resx` files. The default `.resx` file automatically serves as both the neutral culture and English Canadian culture.

### Resource File Naming Convention
- **Pattern**: `{FeatureArea}Resources.{culture}.resx`
- **Examples**:
  - `LandingResources.resx` (default - serves as English Canadian)
  - `LandingResources.fr-CA.resx` (French Canadian)
  - `AuthResources.resx`
  - `BeginnerGuideResources.resx`
  - `CommonResources.resx`

**Note**: Only create culture-specific files (like `.fr-CA.resx`) for non-English languages. The default `.resx` file serves as the English Canadian version.

### Resource File Structure
```
/Resources/
  /Landing/
    LandingResources.resx
    LandingResources.fr.resx
    LandingResources.Designer.cs
  /Auth/
    AuthResources.resx
    AuthResources.fr.resx
    AuthResources.Designer.cs
  /BeginnerGuide/
    BeginnerGuideResources.resx
    BeginnerGuideResources.fr.resx
    BeginnerGuideResources.Designer.cs
  /Common/
    CommonResources.resx
    CommonResources.fr.resx
    CommonResources.Designer.cs
```

### Creating New Resource Files

**IMPORTANT**: When creating new resource files, follow these steps:

1. **Create the resource files** in the appropriate folder under `/RetirementTime/Resources/`
   - Create `.resx` (default - serves as English Canadian)
   - Create `.fr-CA.resx` (French Canadian)
   - **DO NOT** create `.en-CA.resx` - it's not needed and causes conflicts

2. **Configure in .csproj file** - Add entries for each resource file:

```xml
<ItemGroup>
  <!-- Main resource file with PUBLIC generator -->
  <EmbeddedResource Update="Resources\{FeatureArea}\{Name}Resources.resx">
    <Generator>PublicResXFileCodeGenerator</Generator>
    <LastGenOutput>{Name}Resources.Designer.cs</LastGenOutput>
  </EmbeddedResource>
  
  <!-- French Canadian translation with PUBLIC generator -->
  <EmbeddedResource Update="Resources\{FeatureArea}\{Name}Resources.fr-CA.resx">
    <Generator>PublicResXFileCodeGenerator</Generator>
    <LastGenOutput>{Name}Resources.fr-CA.Designer.cs</LastGenOutput>
  </EmbeddedResource>
</ItemGroup>

<ItemGroup>
  <!-- Designer file compilation entries -->
  <Compile Update="Resources\{FeatureArea}\{Name}Resources.Designer.cs">
    <DesignTime>True</DesignTime>
    <AutoGen>True</AutoGen>
    <DependentUpon>{Name}Resources.resx</DependentUpon>
  </Compile>
  
  <Compile Update="Resources\{FeatureArea}\{Name}Resources.fr-CA.Designer.cs">
    <DesignTime>True</DesignTime>
    <AutoGen>True</AutoGen>
    <DependentUpon>{Name}Resources.fr-CA.resx</DependentUpon>
  </Compile>
</ItemGroup>
```

3. **CRITICAL**: Use `PublicResXFileCodeGenerator` instead of `ResXFileCodeGenerator`
   - This ensures the generated Designer class is `public` instead of `internal`
   - Required for `IStringLocalizer<T>` to access the resources
   - Without this, localization will not work properly

4. **Generate Designer files**: Build the project or use Visual Studio/Rider to auto-generate Designer.cs files

5. **Verify Designer class visibility**: Ensure the generated Designer class is marked as `public`:
```csharp
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[System.Diagnostics.DebuggerNonUserCodeAttribute()]
[System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
public class {Name}Resources {  // Must be public, not internal
    // ...
}
```

### Resource Key Naming Convention
- **Pattern**: `{PageArea}_{Element}`
- **Examples**:
  - `Home_Login` (Home page, Login button)
  - `Home_HeroTitle` (Home page, Hero title)
  - `SignUp_Email` (SignUp page, Email label)
  - `Login_WelcomeBack` (Login page, Welcome message)

### Using Resources in Blazor Components

**Step 1**: Add necessary using statements:
```csharp
@using RetirementTime.Resources.{FeatureArea}
@using System.Globalization
```

**Step 2**: Inject the localizer:
```csharp
@inject IStringLocalizer<{FeatureArea}Resources> Localizer
```

**Step 3**: Use in markup:
```html
<h1>@Localizer["Home_HeroTitle"]</h1>
<button>@Localizer["Home_Login"]</button>
```

**Complete Example**:
```csharp
@page "/"
@using RetirementTime.Resources.Landing
@inject IStringLocalizer<LandingResources> Localizer

<h1>@Localizer["Home_HeroTitle"]</h1>
<p>@Localizer["Home_HeroSubtitle"]</p>
<button>@Localizer["Home_StartPlanning"]</button>
```

### Localization Best Practices

**CRITICAL RULE: ALL user-facing text MUST be localized. Never hardcode English or French text directly in components.**

1. **One Resource File Per Page/Feature Area**
   - Landing page → `LandingResources`
   - Authentication pages → `AuthResources`
   - Beginner guide → `BeginnerGuideResources`
   - Common/shared elements → `CommonResources`
   - **IMPORTANT**: When creating a new page or major feature, ALWAYS create a dedicated resource file for it
   - **IMPORTANT**: When creating new resource files, ALWAYS use `PublicResXFileCodeGenerator` in the .csproj file (see "Creating New Resource Files" section above)

2. **Resource File Scope**
   - Create a new resource file for each major page or feature
   - Use `CommonResources` for truly shared elements (error messages, buttons, etc.)
   - Don't mix unrelated page content in one resource file
   - **Every label, button, heading, message, and placeholder must have a resource key**

3. **Maintain Consistency**
   - Keep the same keys across all language files
   - Default `.resx` contains English values and serves as both neutral and English Canadian culture
   - French Canadian translations (`.fr-CA.resx`) should match keys exactly
   - **Never create `.en-CA.resx` files** - they cause resource lookup conflicts

4. **Translation Quality**
   - Use proper French translations with correct accents and grammar
   - Consider cultural differences (e.g., date formats, currency)
   - Test with both cultures to ensure UI layout accommodates text length

5. **Designer Class Accessibility**
   - **ALWAYS** use `PublicResXFileCodeGenerator` in .csproj
   - Verify Designer classes are public after generation
   - If resources don't load, check Designer class accessibility first

6. **Implementation Checklist**
   When creating or modifying components:
   - ✅ Create a new resource file for new pages/features (don't reuse existing ones)
   - ✅ Configure new resource files with `PublicResXFileCodeGenerator` in .csproj
   - ✅ Add all text to appropriate .resx file
   - ✅ Add matching French translation to .fr-CA.resx
   - ✅ Use `@Localizer["ResourceKey"]` in component
   - ✅ Never hardcode text strings in the UI
   - ✅ Include button labels, form labels, headings, messages, placeholders
   - ✅ Test in both English and French

### Troubleshooting Localization Issues

**Problem**: Resources showing as `[ResourceKey]` instead of actual text
- **Solution**: Verify Designer class is `public`, not `internal`
- Check .csproj uses `PublicResXFileCodeGenerator`
- Rebuild project to regenerate Designer files

**Problem**: Culture not changing
- **Solution**: Ensure culture is set in middleware/Program.cs
- Check culture cookie/session management
- Verify resource files exist for the target culture

**Problem**: Missing translations
- **Solution**: Ensure all resource keys exist in all language files
- Check for typos in resource keys
- Verify .resx files are marked as EmbeddedResource

---

## Design System

**Reference file**: `RetirementTime/ThemesAndUI/DESIGN (1).md` — *"Editorial Earth & Refined Utility"*

### Creative North Star
"The Modern Tactile" — editorial warmth meets precise utility. Pairs **Newsreader** (serif, headlines) with **Inter** (sans-serif, body/labels) on a warm cream canvas. Feels like a high-end architectural journal, not a generic fintech app.

### CSS Variables
All design tokens are defined in `:root` inside `RetirementTime/wwwroot/app.css`. **Always use these variables — never hardcode palette colours.**

| Variable | Value | Role |
|---|---|---|
| `--ne-primary` | `#9A3412` | Terracotta — CTAs, active states, checked items |
| `--ne-primary-dark` | `#781f00` | Hover / gradient start for primary buttons |
| `--ne-primary-tint` | `#fef2ee` | Checked checklist background |
| `--ne-secondary` | `#334155` | Charcoal Slate — nav links, sub-labels, inactive items |
| `--ne-tertiary` | `#334155` | Same as secondary |
| `--ne-accent-bar` | `#334155` | Active sidebar right border |
| `--ne-surface` | `#fdf9e9` | Warm Cream — page background (never use pure white) |
| `--ne-surface-low` | `#f8f4e4` | Sidebar / surface_container_low |
| `--ne-surface-container` | `#f0ead6` | Hover states, card groupings |
| `--ne-surface-highest` | `#dedacb` | Input backgrounds (surface_dim) |
| `--ne-surface-hover` | `#d1ccbc` | Input/checklist hover |
| `--ne-surface-white` | `#ffffff` | Cards on surface_low (natural lift) |
| `--ne-border-tonal` | `#dec0b7` | Tonal separators (sidebar shadow, topbar) |
| `--ne-outline-variant` | `#dec0b7` | Section divider lines |
| `--ne-on-surface` | `#1c1c13` | Primary text (warm near-black) |
| `--ne-on-surface-variant` | `#4a4035` | Body text, labels |
| `--ne-on-surface-muted` | `#7a6e65` | Hints, placeholders |
| `--ne-focus-bg` | `#ede8d5` | Input focus background |
| `--ne-shadow-ambient` | `rgba(28,28,19,0.06)` | Ambient card shadow |

### Typography Rules
- **Headlines / Display**: `Newsreader` serif — `font-headline`, tight letter-spacing `-0.02em`
- **Body / Labels / UI**: `Inter` (primary), `Manrope` (fallback) — `font-body` / `font-label`
- **Pairing rule**: Always follow a Newsreader headline with an Inter subtitle

### The "No-Line" Rule
Never use `1px solid` borders to section content. Use **tonal background shifts** instead:
- Sidebar (`--ne-surface-low`) against page (`--ne-surface`)
- Cards (`--ne-surface-white`) on a `--ne-surface-low` background

### Introduction Pages — Animation
**EVERY** intro step must use the two-phase fade-in:
```razor
<div class="intro-step step-header-animate">
    <header class="mb-4">
        <h1 class="font-headline text-5xl text-ne-primary ...">Title</h1>
        <p class="font-body text-ne-on-surface-variant ...">Subtitle</p>
    </header>
    <div class="step-body-animate">
        <div class="bg-ne-surface-low rounded-xl px-8 pb-8 pt-5 ...">
            <!-- form content -->
        </div>
    </div>
</div>
```

---

**Last Updated**: March 21, 2026

