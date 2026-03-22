# ReceitasPlusSch (MyRecipeBook)

## Introduction 
ReceitasPlusSch (MyRecipeBook) is a comprehensive recipe management system backend API. It is designed using **Clean Architecture** principles and **Domain-Driven Design (DDD)** to ensure a scalable, maintainable, and highly testable codebase. The project manages user registration, authentication (JWT), and recipe creation/filtering.

---

## Architecture & Project Structure (DDD)

The project leverages Clean Architecture, dividing responsibilities clearly among layers:

- **MyRecipeBook.API (Presentation Layer):**
  - Contains API Controllers, Middleware, Filters, and configuration (`Program.cs`).
  - Handles HTTP requests and responses using DTOs.
- **MyRecipeBook.Application (Use Cases Layer):**
  - Contains application logic, Use Cases (e.g., RegisterUser, FilterRecipe), and Service interfaces.
- **MyRecipeBook.Domain (Domain Layer):**
  - Contains core domain Entities, Enums, and Repository Interfaces (e.g., `Tokens`, `Security`).
- **MyRecipeBook.Infrastructure (Infrastructure Layer):**
  - Handles external concerns: Data Access (Entity Framework), Migrations, Security (encryption/salting), and Services implementations.
- **Shared Projects:**
  - `MyRecipeBook.Communication`: DTOs (Requests/Responses) used across the system.
  - `MyRecipeBook.Exceptions`: Custom exception classes and resource strings.

### Folder Tree Overview
```text
src/
├── Backend/
│   ├── MyRecipeBook.API/            # Controllers, program startup, API configs
│   ├── MyRecipeBook.Application/    # Use case implementations
│   ├── MyRecipeBook.Domain/         # Core entities and interfaces
│   └── MyRecipeBook.Infrastructure/ # Database, DB migrations, Cryptography
└── Shared/
    ├── MyRecipeBook.Communication/  # Request and Response objects (DTOs)
    └── MyRecipeBook.Exceptions/     # Domain exception models
```

---

## API Endpoints

The system relies on RESTful endpoints. The main controllers interact with the application use cases to perform operations.
> Note: Many endpoints require a valid JWT token passed in the `Authorization` header (`Bearer {token}`).

### Users (`UserController`)
- `POST /user` : Registers a new user.
- `GET /user` : Retrieves the profile of the authenticated user.
- `PUT /user` : Updates the profile of the authenticated user.
- `PUT /user/change-password` : Changes the password of the authenticated user.

### Recipes (`RecipeController`)
- `POST /recipe` : Registers a new recipe.
- `POST /recipe/filter` : Returns a list of recipes filtered by the given criteria.

### Authentication (`LoginController`)
- `POST /login` *(inferred)* : Authenticates a user and returns a standard JWT token.

---

## Important Configuration Files

### `appsettings.Development.json` (and `appsettings.json`)
Defines essential settings for the application:
- **ConnectionStrings:**
  - `DatabaseType`: `0` for MySQL, `1` for SQL Server.
  - Contains local connection strings for SQL Server and MySQL.
- **Settings:**
  - `Password:salt`: Cryptographic salt for password hashing.
  - `Jwt`: Includes `SigningKey` and `ExpirationTimeMinutes` for generating bearer tokens.
  - `IdCryptographyAlphabet`: Specifically used alongside the **sqids** library to encrypt/obfuscate long IDs into secure alphanumeric strings.

---

## Build and Test

The project features a robust strategy leveraging unit tests and automated integration testing via `WebApplicationFactory` to spin up tests against real endpoints safely.

### Tests Folder Tree
```text
tests/
├── CommonTestUtilities / # Builders and shared mocks
├── UseCases.Test       / # Unit testing the core Application layer
├── Validators.Test     / # Tests over request validation logic
└── WebApi.Test         / # E2E & Integration tests targeting actual API Controllers
```
To run tests locally using the .NET CLI:
```bash
dotnet test
```

---

## Pull Requests and Version History

A complete list of the project's merged pull requests detailing its evolution:

- **PR 29:** Deploy 1st Millestone
- **PR 28:** feat: adding recipe filter unit tests and util classes
- **PR 27:** Filter post
- **PR 26:** feat: adding recipe unit tests
- **PR 25:** feat: adding sqids to encrypt long ids
- **PR 24:** feat: adding recipe register
- **PR 23:** feat: Adding recipe validation and unit tests, also removing ErrorMessages complexity
- **PR 22:** feat: adding recipes table and sub tables on migration, plus their Entities
- **PR 21:** feat: Change password unit tests
- **PR 20:** feat: moving encription to infrastructure and a lot of identations
- **PR 19:** feat: adding unit tests to put users
- **PR 18:** feat: adding put endpoint
- **PR 17:** feat: fix warnings and sonar issues
- **PR 16:** feat: add get user profile and tests
- **PR 15:** feat: adding token use on swagger
- **PR 14:** feat: adding jwt token generation and userIdentifier on db
- **PR 13:** feat: adding login controller and unit tests
- **PR 12:** feat: improving and fixing code according to sonar qube metrics
- **PR 11:** feat: adding unit tests on POST User
- **PR 10:** feat: setting versions fixed at 8.x.x and more
- **PR 9:** feat: adding unit tests, one common project and two unit test projects
- **PR 8:** feat: adding migrations
- **PR 7:** feat: adding user post, validation and mapping
- **PR 6:** feat: adding user repository
- **PR 5:** feat: adding support classes
- **PR 4:** feat: adding dependencies
- **PR 3:** feat: adding user entity
- **PR 2:** feat: adding exceptions and messages to registered users
- **PR 1:** feat: adding base project

---

## Getting Started

1. Set your designated `DatabaseType` in `appsettings.Development.json` (0 for MySQL).
2. Adjust your `ConnectionStrings` to point to a valid local DB instance.
3. Run `dotnet restore` to gather packages and `dotnet run` inside the `MyRecipeBook.API` folder.
4. Access `https://localhost:<port>/swagger` to view and interact with the endpoints via Swagger UI.