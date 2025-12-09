# NatureHelp

A comprehensive environmental monitoring and reporting platform built with ASP.NET Core and Angular.

## Overview

NatureHelp is a full-stack application designed for tracking and managing environmental data, including water quality monitoring, laboratory research, deficiency reporting, and organizational management. The platform provides real-time data collection, analysis, and visualization capabilities with robust logging and monitoring infrastructure.

## Features

- **Environmental Monitoring**: Track water quality and environmental parameters
- **Laboratory Management**: Manage research data and laboratory operations
- **Deficiency Reporting**: Report and track environmental deficiencies
- **Organization Management**: Handle organizational structures and user management
- **Real-time Analytics**: Monitor environmental trends and patterns
- **Audit System**: Complete audit trail for all system activities
- **Payment Processing**: Integrated payment system for services
- **Attachment Management**: Upload and manage documents and images with Azure Blob Storage
- **Multi-language Support**: Internationalization support for multiple languages

## Technology Stack

### Backend
- **Framework**: ASP.NET Core 8.0
- **Database**: PostgreSQL with Entity Framework Core
- **Authentication**: JWT Bearer tokens
- **API Documentation**: Swagger/OpenAPI
- **Caching**: Redis and In-Memory caching
- **Rate Limiting**: AspNetCoreRateLimit
- **Storage**: Azure Blob Storage
- **Email**: SMTP integration

### Frontend
- **Framework**: Angular 17+
- **State Management**: NgRx (Store, Effects, Entity)
- **UI Components**: PrimeNG with Angular Material
- **Styling**: TailwindCSS
- **Authentication**: @auth0/angular-jwt
- **Internationalization**: ngx-translate
- **Build Tools**: Angular CLI

### Observability & Monitoring
- **Structured Logging**: Serilog
- **Log Aggregation**: 
  - Seq (Development)
  - Grafana + Loki (Visualization)
  - Elasticsearch + Kibana (Search & Analytics)
- **Metrics**: Prometheus
- **Testing**: xUnit, Cypress

### DevOps
- **Containerization**: Docker & Docker Compose
- **CI/CD**: Jenkins
- **Deployment**: Firebase Hosting (Frontend)

## Architecture

```
NatureHelp/
├── src/
│   ├── NatureHelp/          # Web API Layer
│   ├── Application/         # Application Services & DTOs
│   ├── Domain/              # Domain Models & Interfaces
│   ├── Infrastructure/      # Data Access & External Services
│   ├── Shared/              # Shared DTOs & Utilities
│   ├── View/nature-help/    # Angular Frontend
│   └── Tests/               # Integration & Unit Tests
├── docker-compose.yml       # Multi-container orchestration
├── prometheus.yml           # Metrics configuration
└── Jenkinsfile             # CI/CD pipeline
```

## Quick Start

### Prerequisites
- .NET 8.0 SDK
- Node.js 18+ and npm
- Docker Desktop
- PostgreSQL (local) or Docker

### 1. Clone and Setup

```bash
git clone <repository-url>
cd NatureHelp
```

### 2. Start with Docker (Recommended)

```bash
docker-compose up -d
```

This starts:
- Backend API (port 5000)
- Frontend (port 5051)
- PostgreSQL database
- All logging services (Seq, Grafana, Kibana)
- Prometheus monitoring

### 3. Access the Application

| Service | URL | Credentials |
|---------|-----|-------------|
| Frontend | http://localhost:5051 | - |
| Backend API | http://localhost:5000 | - |
| Swagger UI | http://localhost:5000/swagger | - |
| Seq Logs | http://localhost:5341 | - |
| Grafana | http://localhost:3000 | admin/admin |
| Kibana | http://localhost:5601 | - |
| Prometheus | http://localhost:9090 | - |

### 4. Local Development Setup

#### Backend
```bash
cd src/NatureHelp
dotnet restore
dotnet run
```

#### Frontend
```bash
cd src/View/nature-help
npm install
npm start
```

## Configuration

### Environment Variables

Set these in `docker-compose.yml` or your deployment environment:

- `ASPNETCORE_ENVIRONMENT`: Development/Production
- `ConnectionStrings__DefaultConnection`: Database connection string
- `Serilog__WriteTo__*`: Logging destinations configuration

### Database Setup

The application uses Entity Framework Core migrations:

```bash
cd src/Infrastructure
dotnet ef database update
```

### Azure Blob Storage

Configure in `appsettings.json`:
```json
{
  "AzureStorage": {
    "ConnectionString": "your-connection-string",
    "ContainerName": "attachments"
  }
}
```

## Development

### Running Tests

```bash
cd src/Tests
dotnet test
```

### Code Quality

The project includes:
- ESLint configuration
- Prettier for code formatting
- Husky for pre-commit hooks

Frontend formatting:
```bash
cd src/View/nature-help
npm run prettier:fix
```

### API Documentation

Interactive API documentation is available at `/swagger` when running the backend.

Postman collection available at: `src/Postman/collection.json`

## Logging & Monitoring

Comprehensive observability setup with multiple platforms. See detailed guides:

- [LOGGING_SETUP.md](LOGGING_SETUP.md) - Complete logging configuration
- [QUICK_START.md](QUICK_START.md) - Quick start guide for viewing logs

### Log Platforms

1. **Seq** (http://localhost:5341) - Best for development and debugging
2. **Grafana + Loki** (http://localhost:3000) - Best for dashboards and visualization
3. **Kibana + Elasticsearch** (http://localhost:5601) - Best for advanced search

### Metrics

Prometheus metrics available at http://localhost:9090

Application metrics endpoint: http://localhost:5000/metrics

## Deployment

### Production Checklist

- [ ] Update connection strings in `docker-compose.yml`
- [ ] Configure Seq API keys
- [ ] Enable Elasticsearch security
- [ ] Set strong passwords for Grafana
- [ ] Configure CORS allowed origins
- [ ] Set up SSL/TLS certificates
- [ ] Review rate limiting policies
- [ ] Configure email SMTP settings
- [ ] Set up Azure Blob Storage
- [ ] Review database backup strategy

### Jenkins Pipeline

CI/CD pipeline defined in `Jenkinsfile` handles:
- Building and testing
- Docker image creation
- Deployment orchestration

## Security

See [SECURITY.md](SECURITY.md) for security policies and vulnerability reporting.

Key security features:
- JWT authentication
- Rate limiting
- CORS configuration
- SQL injection prevention (EF Core)
- Structured logging (no sensitive data)

## Live Demo

Visit the deployed application: [https://naturehelp-c4212.web.app/uk/water](https://naturehelp-c4212.web.app/uk/water)

## Project Structure

### Domain Layer
Core business entities and interfaces including:
- Nature models (Water, Air, Soil monitoring)
- Organization models
- Analytics models
- Audit models

### Application Layer
Business logic and services:
- Authentication & Authorization
- Payment processing
- Caching strategies
- Nature monitoring services
- Organization management

### Infrastructure Layer
Data access and external integrations:
- Entity Framework Core repositories
- Azure Blob Storage provider
- Email service
- Database migrations

### API Layer
RESTful endpoints organized by domain:
- Authentication
- Nature monitoring
- Organization management
- Analytics
- Audit logs
- Payments

## Contributing

1. Follow the existing code structure
2. Write tests for new features
3. Run code quality tools before committing
4. Update documentation for significant changes

## License

This project is licensed under a **Proprietary License**.  
All rights reserved © 2025 Valentyn Riabinchak.  
Use, modification, or distribution without explicit permission is strictly prohibited.

## Support

For issues, questions, or contributions, please contact the development team.

## Useful Commands

### Backend
```bash
dotnet restore              # Restore dependencies
dotnet build                # Build solution
dotnet test                 # Run tests
dotnet ef migrations add    # Create migration
dotnet ef database update   # Apply migrations
```

### Frontend
```bash
npm install                 # Install dependencies
npm start                   # Development server
npm run build              # Production build
npm test                    # Run tests
npm run prettier:fix       # Format code
ng extract-i18n            # Extract translations
```

### Docker
```bash
docker-compose up -d        # Start all services
docker-compose down         # Stop all services
docker-compose logs -f      # View logs
docker-compose ps           # List containers
```

### Cleanup
```powershell
.\clean_project.ps1        # Clean build artifacts
```
