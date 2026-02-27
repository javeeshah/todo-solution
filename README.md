# Todo Solution

## Project overview
A Simple To‑Do app with API and frontend SPA app for managing tasks (create, read, update, delete). Backend is in .NET 8 Web API with EF Core in‑memory database used for development and demo. Frontend is an Angular 21.2.0 - SPA that calls the .NET API.

## Tech stack
- Backend: .NET 8+ (ASP.NET Core Web API), Entity Framework Core (InMemory provider)
- Mapping/Validation: AutoMapper, FluentValidation (Added to demonstrate common patterns, not strictly required for a simple API)
- Logging: Microsoft.Extensions.Logging
- Frontend: Angular 21.2.0
- Tests: NUnit (Unit and Integration Tests)

## How to run backend (API)
1. Open a terminal in the repository root:
   - dotnet restore
   - dotnet build
2. Run:
   - Open Todo.Api in Visual Studio and run it.  
   - Note the port assigned by Visual Studio (e.g., https://localhost:7079). 
   - dotnet run --project backend/Todo.Api
3. Development notes:
   - The API uses an in‑memory EF provider by default (`UseInMemoryDatabase`).
   - Swagger UI available in Development: `https://localhost:{port}/swagger`
   - To run with Development environment:
     - PowerShell: `$env:ASPNETCORE_ENVIRONMENT='Development'; dotnet run --project backend/Todo.Api`

## How to run frontend (ToDo UI)
- frontend/todo-ui folder (e.g. `frontend/todo-ui`):
  - cd frontend/todo-ui
  - npm install
  - npm start
- Update the API Base URL in proxy.conf.json
- apiUrl: 'https://localhost:<your-api-port>'

## API endpoints
Base route: `/api/todo`

- GET `/api/todo`  
  Response: 200 OK — list of TodoItemDto

- GET `/api/todo/{id}`  
  Response: 200 OK — TodoItemDto, 404 Not Found if missing

- POST `/api/todo`  
  Body: TodoItemCreateDto  
  Response: 201 Created — created TodoItemDto, 400 ValidationProblemDetails on invalid input

- PUT `/api/todo/{id}`  
  Body: TodoItemUpdateDto  
  Response: 200 OK — updated TodoItemDto, 400 ProblemDetails on id mismatch, 404 Not Found

- DELETE `/api/todo/{id}`  
  Response: 204 No Content, 404 Not Found if missing

Validation errors and unexpected errors are reported using RFC 7807 ProblemDetails JSON.

## Architecture overview
- Presentation: Controllers expose REST endpoints.
- Application: Services contain business logic (ITodoService / TodoService).
- Persistence: EF Core DbContext (TodoContext) and repositories (if present).
- Cross-cutting: AutoMapper profiles, FluentValidation validators, global exception middleware producing ProblemDetails.
- Tests: unit and integration tests target service and controller layers.

## Folder structure (key folders)
- backend/Todo.Api/  
  - Controllers/ (API controllers)  
  - Services/ (business logic)  
  - Repositories/ (data access)  
  - Models/ (domain entities)  
  - Dtos/ (request/response DTOs)  
  - MappingProfiles/ (AutoMapper)  
  - Validators/ (FluentValidation)  
  - Middleware/ (global exception handling)  
  - Program.cs
- frontend/ (Angular SPA)
- tests/ (unit/integration tests)

## Testing instructions
- Unit tests:
  - dotnet test
  - Run from repo root or `tests` folder: `dotnet test ./tests`
- Integration tests:
  - Use WebApplicationFactory or TestServer to run the API in-memory.
  - Ensure ASPNETCORE_ENVIRONMENT=Development if tests rely on dev settings.
- Manual API testing:
  - Use Swagger UI or tools like curl / Postman.