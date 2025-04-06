# ContactApp
A simple ASP.NET Core web application with a clean architecture that includes:

- A contact form (name, last name, email)
- SQL Server integration
- Email notifications (via MailHog for development)
- External API integration for pulling user details
- Rate limiting to prevent spam submissions

## Architecture
The application follows Clean Architecture principles with separate projects for each layer:
- ContactApp.Web: Presentation layer with Razor Pages
- ContactApp.Application: Business logic and use cases
- ContactApp.Domain: Core entities and business rules
- ContactApp.Infrastructure: External services implementation (email, API, database)

## Fetaures
- Razor Pages frontend
- Clean architecture (separated projects per layer)
- SQL Server database
- Rate limiting: One submission per minute per terminal
- Email sending after successful form submission
- API integration to fetch additional user data

## Technologies
- .NET 8 / ASP.NET Core
- Razor Pages
- SQL Server
- Serilog
- MailHog (for local SMTP)
- Bootstrap (for styling)

## Setup instructions
1. Clone the repository
2. Navigate to solution directory
3. Restore Nuget Packages: `dotnet restore`
4. Build the solution `dotnet build`

### Database Setup

#### Option 1: Execute SQL Script Manually
1. Locate the SQL script file at `ContactApp.Infrastructuure/Scripts/initDatabase2.sql` in the project
2. Execute the script:Right-click the script file in Solution Explorer and select "Execute"
#### Option 2: Update Connection String
Ensure the connection string in `appsettings.json` matches your SQL Server instance(there is appsettings.example.json for your reference):
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ContactAppDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}

### Email Setup via MailHog
For development purposes, this application uses MailHog as a local SMTP server to catch and view emails without actually sending them.
Install MailHog:
Download from MailHog GitHub releases
1. Run MailHog
2. Access MailHog Web Interface:
3. Open http://localhost:8025 in your browser to view captured emails
Verify Configuration:
The application is pre-configured to use MailHog, all configs are defefined in appsettings.example.json

### Rate limit configuration
The application is configured to allow only one submission per minute from the same IP address. 
This setting can be adjusted in appsettings.json

### API Integration adjustments
The application is configured to pull additional user data from API defined in the appsettings.json

### Logging
The application uses Serilog for structured logging:
Console output for development
Daily rolling log files stored in the Logs directory
Configurable log levels via appsettings.json

## Testing
The application includes unit tests for the application service layer, implemented using xUnit testing framework.
### Running Tests
To run the unit tests:
1. Run: `dotnet test`
Test Coverage
The unit tests cover the following areas of the application service layer:
- Contact form submission validation
- Rate limiting functionality
- API integration service
- Email notification service

