```markdown
# RetirementTime Project - Copilot Instructions

## Project Overview
RetirementTime is a retirement planning application built with C# .NET Core backend and Blazor frontend. The primary goal is to help users plan their retirement through easy-to-understand instructions and calculations.

## Technology Stack
- **Backend**: ASP.NET Core (.NET Core)
- **Frontend**: Blazor Server-Side Rendering
- **IDE**: JetBrains Rider
- **Architecture**: Server-side HTML rendering with API controllers

## Project Structure
```
/Users/samuelhamann/workspace/RetireTime/RetirementTime/
├── Program.cs                          # Application entry point, DI configuration
├── Controllers/                        # API endpoints
│   └── HtmlRenderController.cs        # Handles HTML rendering requests
├── Services/                          # Business logic and utilities
│   ├── IRazorViewToStringRenderer.cs  # Interface for Razor rendering
│   └── RazorViewToStringRenderer.cs   # Implementation of server-side rendering
├── Models/                            # DTOs and data models
│   └── RenderedHtmlDto.cs            # Data transfer objects
├── Views/                            # Razor templates
│   └── Shared/                       # Shared views/layouts
│       └── ExampleTemplate.cshtml    # Sample template
└── Client/                           # Blazor frontend
    └── Pages/                        # Blazor pages/components
        └── RenderAndSend.razor       # Example client interaction
```

## Architecture Principles

### Server-Side Rendering Flow
1. Blazor frontend requests rendered HTML from backend API
2. Backend uses Razor view engine to render templates with data
3. Rendered HTML is returned to frontend as JSON
4. Frontend can display or send the HTML back to backend for storage/processing

### Key Components
- **RazorViewToStringRenderer**: Core service that converts Razor views to HTML strings
- **HtmlRenderController**: Exposes REST endpoints for rendering and receiving HTML
- **Blazor Pages**: Client-side UI that interacts with backend APIs

## Domain Context: Retirement Planning

### Core Concepts
- **Retirement Age**: Target age when user plans to retire
- **Savings Goals**: Financial targets for retirement
- **Income Planning**: Estimating retirement income needs
- **Investment Strategy**: Asset allocation and growth projections
- **Timeline**: Years until retirement and years in retirement

### User Experience Goals
- **Simplicity**: Easy-to-understand language, avoiding financial jargon
- **Step-by-Step Guidance**: Break complex retirement planning into manageable steps
- **Visual Feedback**: Charts, progress bars, and visual indicators
- **Personalization**: Adapt advice based on user's specific situation
- **Actionable Instructions**: Clear next steps for users to take

## Coding Guidelines

### Naming Conventions
- Controllers: `[Feature]Controller.cs`
- Services: `I[Service].cs` (interface), `[Service].cs` (implementation)
- Models: `[Entity]Dto.cs` for data transfer, `[Entity].cs` for domain models
- Views: `[Feature]Template.cshtml` or `[Feature].cshtml`
- Blazor Pages: `[Feature].razor`

### Patterns to Follow
- **Dependency Injection**: Register all services in Program.cs
- **Interface Segregation**: Create interfaces for all services
- **Single Responsibility**: Each class/method should have one clear purpose
- **Async/Await**: Use async methods for I/O operations
- **Logging**: Use ILogger for diagnostic information

### Code Style
- Use C# nullable reference types
- Prefer explicit types over `var` for clarity in retirement calculations
- Add XML comments for public APIs
- Keep methods focused and concise (prefer < 30 lines)
- Use meaningful variable names (e.g., `retirementAge` not `ra`)

## Common Scenarios

### Adding a New Retirement Calculator
1. Create model in `Models/[Calculator]Input.cs` and `Models/[Calculator]Result.cs`
2. Create service interface in `Services/I[Calculator]Service.cs`
3. Implement service in `Services/[Calculator]Service.cs`
4. Register service in `Program.cs`
5. Create API endpoint in `Controllers/[Calculator]Controller.cs`
6. Create Blazor page in `Client/Pages/[Calculator].razor`
7. Create Razor template if server-side rendering needed

### Adding a New View Template
1. Create `.cshtml` file in `Views/Shared/` or feature-specific folder
2. Define strongly-typed model using `@model` directive
3. Use Bootstrap classes for styling (if applicable)
4. Test rendering via `IRazorViewToStringRenderer`

### Adding New API Endpoint
1. Create controller inheriting from `ControllerBase`
2. Add `[ApiController]` attribute
3. Use route pattern: `api/[controller]/[action]`
4. Return `IActionResult` types
5. Use async methods for database/external calls

## Financial Calculation Guidelines
- Always use `decimal` for monetary values (never `float` or `double`)
- Round currency to 2 decimal places
- Handle edge cases: negative values, zero income, extreme ages
- Validate user inputs (age ranges, positive amounts)
- Include inflation calculations where appropriate
- Provide explanations alongside numbers

## Error Handling
- Use try-catch for I/O and external service calls
- Log exceptions with context using ILogger
- Return user-friendly error messages (avoid technical details)
- Validate inputs early and return 400 Bad Request for invalid data
- Use ProblemDetails for structured error responses

## Testing Priorities
- Unit tests for calculation services (retirement projections, savings goals)
- Integration tests for API endpoints
- Test edge cases in financial calculations
- Verify rendered HTML output matches expected templates

## Security Considerations
- Validate all user inputs
- Sanitize HTML if displaying user-generated content
- Use HTTPS in production
- Don't log sensitive financial information
- Follow OWASP guidelines for web applications

## Future Considerations
- Multi-user support with authentication
- Data persistence (database integration)
- PDF report generation
- Email notifications for milestones
- Integration with financial data providers
- Mobile-responsive design enhancements

## Questions to Ask When Unclear
1. Is this calculation based on today's dollars or future dollars?
2. Should this display be simplified for non-financial users?
3. Does this feature need server-side rendering or can it be client-only?
4. What validation rules apply to this input?
5. Should this data be persisted or calculated on-demand?

## Helpful Context for AI Assistants
- Prioritize clarity and user-friendliness over technical complexity
- When suggesting financial formulas, explain the reasoning
- Consider both pre-retirement and post-retirement scenarios
- Remember this is for everyday people, not financial professionals
- Balance comprehensive planning with simplicity
```
```

