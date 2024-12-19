# EventHub

EventHub is an advanced event management platform built using modern software design principles and patterns. This README serves as a guide for understanding the architecture, folder structure, patterns, and deployment details of the application.

---

## ⭐ Give Us a Star

If this project is helpful to you, please consider giving it a star on GitHub. Your support means a lot!

---

## Solution Structure

The project is structured as follows:

```
EventHub
├── deploy
│   ├── .dockerignore
│   ├── docker-compose.yml
│   ├── docker-compose.development.yml
│   └── docker-compose.production.yml
├── Solution Items
│   ├── .editorconfig
│   ├── .gitignore
│   ├── .gitlab-ci.yml
│   ├── Directory.Build.props
│   ├── LICENSE.txt
│   └── README.md
└── source
    ├── EventHub.Application
    ├── EventHub.Domain
    ├── EventHub.Domain.Shared
    ├── EventHub.Infrastructure
    ├── EventHub.Infrastructure.Persistence
    ├── EventHub.Infrastructure.SignalR
    └── EventHub.Presentation
```

### Explanation of Folders

- **deploy/**: Contains Docker-related configuration files, including Docker Compose files for development and production environments.
- **Solution Items/**: Stores solution-level configuration files, CI/CD configuration (`.gitlab-ci.yml`), and other supporting documents such as the README and license.
- **source/**: Contains the main source code, organized by Clean Architecture principles:
    - **EventHub.Application**: Application layer with use cases and business rules.
    - **EventHub.Domain**: Domain layer with core business logic and entities.
    - **EventHub.Domain.Shared**: Shared utilities and functionality, enhancing reusability.
    - **EventHub.Infrastructure**: Infrastructure layer for external services.
    - **EventHub.Infrastructure.Persistence**: Handles database communications.
    - **EventHub.Infrastructure.SignalR**: Manages real-time communication using SignalR.
    - **EventHub.Presentation**: Presentation layer for API endpoints and UI components.

---

## Applied Architecture and Design Patterns

### Clean Architecture

Clean Architecture ensures a clear separation of concerns, fostering maintainable and scalable code:

- **Domain Layer** (`EventHub.Domain`): Encapsulates core business logic and entities.
- **Application Layer** (`EventHub.Application`): Contains use cases and application-specific business rules.
- **Infrastructure Layer** (`EventHub.Infrastructure`): Manages external services, including database interactions and third-party integrations.
- **Presentation Layer** (`EventHub.Presentation`): Handles user interface and API endpoints.
- **Persistence Module** (`EventHub.Infrastructure.Persistence`): A specialized component within Infrastructure to manage database communications.
- **Shared Module** (`EventHub.Domain.Shared`): Contains common utilities and shared functionality, improving reusability across layers without violating Clean Architecture principles.
- **SignalR Module** (`EventHub.Infrastructure.SignalR`): Manages real-time communication using SignalR for WebSocket-based interactions.

### Repository Pattern

Facilitates an abstraction layer for data access:

- Includes both standard and cached repositories, utilizing Redis for enhanced performance.

### Unit of Work Pattern

Ensures consistent and atomic database operations:

- Combines multiple repositories within a single `UnitOfWork` class for transaction management.

### Domain-Driven Design (DDD)

Focuses on modeling the core business domain:

- Aligns software design with business requirements to improve quality and maintainability.

### CQRS Pattern

Separates command (write) and query (read) responsibilities:

- A single database is used for both operations in this implementation.

### Transactional Outbox Pattern & Idempotency

Ensures reliable and consistent event sourcing:

- Prevents duplicate events and ensures successful message delivery using Hangfire for email notifications.

### Additional Design Patterns

- **Decorator Pattern**: Enhances object functionality dynamically.
- **Factory Pattern**: Centralizes object creation logic.
- **Singleton Pattern**: Managed through .NET Dependency Injection.

---

## Continuous Integration and Deployment (CI/CD)

The project includes a CI/CD pipeline configured for deployment on Azure App Service. Use it as a reference for deploying your applications to Azure.

---

## How to Run the Application (Development Environment)

### Step 1: Run Docker Compose

Run the following command to start the required services. Ensure the correct path for the Docker Compose file based on your current working directory:

```sh
docker-compose -f deploy/docker-compose.development.yml -p eventhub up -d --remove-orphans
```

### Step 2: Run Migrations (First-Time Setup)

Run migrations to set up the database schema. See the "Run Migrations" section for details.

### Step 3: Start the Application

- **HTTP (Development Environment):** Starts the application with the Development configuration.
- **HTTPS (Production Environment):** Starts the application with the Production configuration.

---

## Run Migrations

Ensure you are in the `/source` directory before running these commands.

### Add a New Migration

Replace `<your-migration-name>` with your desired migration name:

```sh
dotnet ef migrations add <your-migration-name> --startup-project EventHub.Presentation --project EventHub.Infrastructure.Persistence --output-dir ../EventHub.Infrastructure.Persistence/Migrations
```

### Apply Migrations

Run the following command to update the database schema after adding a new migration:

```sh
dotnet ef database update --startup-project EventHub.Presentation --project EventHub.Infrastructure.Persistence
```

---

By adhering to robust design principles and patterns, EventHub delivers a reliable and scalable solution for event management. Explore, contribute, and make the most of this project!

