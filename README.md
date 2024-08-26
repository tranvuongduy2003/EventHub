# EventHub

### Give me 1 star if it is useful for you.

===========================================================

# APPLIED ARCHITECTURE & DESIGN PATTERN:
  - Clean Architecture
    - Clean Architecture is a software architecture that emphasizes the separation of concerns, the independence of components, and the use of well-defined boundaries.
    - EventHub.Domain represents for Domain layer, EventHub.Application for Application (Usecases) layer, EventHub.Infrastructure for Infrastructure layer, EventHub.Presentation for Presentation layer.
    - EventHub.Persistence is actually Infrastructure layer, but it is seperated from the layer in order to easily communicate with the database.
    - Instead of defining common class in (normally) Domain, I put all of them in Shared to easily reference to, it actually does not break the Clean Architecture principles.
    - EventHub.SignalR is the same as EventHub.Persistence, it belongs to Infrastructure layer but is seperated from the layer. It handles all of realtime work with using SignalR for Websocket.
  - Repository Pattern:
    - The repository design pattern allows you to create an accessible data layer. I also have cached repositories in case of caching data. The cache database I used is Redis.
  - Unit of Work Pattern:
    - A unit of work encapsulates one or more code repositories and a list of actions to be performed which are necessary for the successful implementation of self-contained and consistent data change. In this solution, I put all of repositories (also cached repositories) into just only 1 UnitOfWork class.
  - Domain-Driven Design (DDD):
    - Domain-Driven Design (DDD) is a software development philosophy that emphasizes the importance of understanding and modeling the business domain. It is a strategy aimed at improving the quality of software by aligning it more closely with the business needs it serves.
  - CQRS Pattern:
    - CQRS stands for Command and Query Responsibility Segregation, a pattern that separates read and update operations for a data store. It is perfect pattern to collaborate with DDD. In this solution, I just use 1 database for both commands and queries.
  - Transactional Outbox Pattern & Idempotency:
    - The transactional outbox pattern resolves the dual write operations issue that occurs in distributed systems when a single operation involves both a database write operation and a message or event notification. Idempotency is a crucial property of certain operations or API requests that guarantees consistent outcomes, regardless of the number of times an operation is performed. I use the couple of these patterns to ensure Event Sourcing of DDD work well. The event will be recalled if there are any errors, and if an event was called, it will not be called again (duplicate message). It is also used for message broker, in this solution I just apply to sending email using Hangfire, the result must be similar with the Event Sourcing.
  - Some another Design Patterns: Decorator Pattern, Factory Pattern, Singleton Pattern (just is C# Dependency Injection), ...

# CI/CD
  - The project has set up CI/CD to deploy to Azure App Service. If anyone wants to deploy to Azure, you can use it for reference.

# HOW TO RUN THE APPLICATION (Development Environment)
  - **Step 1:** Run docker compose (pay attention to the **path** to the docker-compose file, if you have **cd src** then you don't need to add **src/** to the command)
  ```
  docker-compose -f src/docker-compose.development.yml -p eventhub up -d --remove-orphans 
  ```
  - **Step 2:** Run migrations (for the first time)
    - Last section
  - **Step 3:** Run app with **http** options
    - **http** will run with the environment **Development** while **https** will run with the environment **Production**

# RUN MIGRATIONS (ensure you are in **/src**):
  - If you want to add a new migration, use this command:
  ```
  dotnet ef migrations add "InitialMigration" --startup-project EventHub.Presentation --project EventHub.Persistence --output-dir ../EventHub.Persistence/Migrations 
  ```
  - Run this command after step 2 of **# HOW TO RUN THE APPLICATION** section, or after adding a new migration to update your database:
  ```
  dotnet ef database update --startup-project EventHub.Presentation --project EventHub.Persistence
  ```
