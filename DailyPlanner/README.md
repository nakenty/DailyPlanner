# DailyPlanner

This repository contains the initial scaffolding for the **Final Boss Planner**—a productivity web application built with a . NET (ASP.NET Core) back‑end and an Angular front‑end.  The goal of this scaffolding is to lay down a clean architecture foundation that separates concerns into distinct layers and provide basic functionality for tasks, notes, favorite links, KPI reporting and search.  The Angular client is not yet included, but the back‑end demonstrates how features will be exposed via REST endpoints.

- **Domain** – Contains the core business entities and basic enums, with no dependencies on other projects.  Here you’ll find models like `TaskItem`, `Note`, and `FavoriteLink`, along with the `TaskStatus` enumeration.
- **Application** – Defines interfaces and application‑level logic.  This layer depends on the Domain but not on Infrastructure or Web UI.  For example, the `ITaskService` interface lives here.
- **Infrastructure** – Implements the application interfaces with concrete services.  To keep the example lightweight and free of external dependencies, an in‑memory implementation of the `ITaskService` is provided.  In a future iteration this layer can contain services that read and write a planner data file to OneDrive via Microsoft Graph or persist data to a database.
- **Web UI** – Hosts the ASP.NET Core Web API.  It registers services using dependency injection and exposes endpoints under `api/`.  A basic `TasksController` demonstrates CRUD‑style endpoints for tasks.  The Angular client is not yet included; this folder strictly hosts the back‑end API.

## Available API endpoints

The following controllers are defined in the Web UI project:

| Controller         | Route Prefix     | Operations                                                                      |
|--------------------|------------------|---------------------------------------------------------------------------------|
| `TasksController`  | `/api/tasks`     | `GET /api/tasks/{date}` – list tasks by due date; `POST /api/tasks` – create a task; `PUT /api/tasks/{id}/complete` – mark a task complete |
| `NotesController`  | `/api/notes`     | `GET /api/notes` – list all notes; `GET /api/notes/task/{taskId}` – list notes for a task; `POST /api/notes` – create a note; `PUT /api/notes/{id}` – update a note; `DELETE /api/notes/{id}` – delete a note |
| `LinksController`  | `/api/links`     | `GET /api/links` – list favorite links; `POST /api/links` – add a link; `DELETE /api/links/{id}` – remove a link |
| `KpiController`    | `/api/kpi`       | `GET /api/kpi?startDate=yyyy-MM-dd&endDate=yyyy-MM-dd` – return a summary of tasks completed, overdue counts and average completion time for the specified range |
| `SearchController` | `/api/search`    | `GET /api/search?q=keyword` – search tasks, notes and links for the given keyword |

These endpoints operate on simple in‑memory collections by default.  You can replace the in‑memory services in `Program.cs` with real implementations (e.g. connecting to OneDrive or a database) when you’re ready.

With this structure in place you can begin adding business logic, integrate authentication, and scaffold the Angular front‑end.  See the `Program.cs` and `TasksController` for examples of how to wire up services and expose endpoints.
## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Your favorite IDE (Visual Studio, VS Code, Rider)

### Running the API

1. Build the solution:
   ```bash
   dotnet build
   ```

2. Run tests:
   ```bash
   dotnet test
   ```

3. Start the API:
   ```bash
   cd WebUI
   dotnet run
   ```

4. Access Swagger UI (in Development mode):
   - Navigate to `http://localhost:5000/swagger`
   - Explore and test all API endpoints interactively

### What's Next?

This project has completed its initial scaffolding phase with all core features implemented and tested. For the complete roadmap and next development steps, see **[ROADMAP.md](ROADMAP.md)**.

**Immediate next steps:**
1. Implement persistent storage (replace in-memory services)
2. Add authentication and authorization
3. Build the Angular front-end
4. Deploy to production environment

See [ROADMAP.md](ROADMAP.md) for detailed implementation plans, priorities, and technical decisions.
