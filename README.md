# CustomTemplate

A .NET 10 solution template with API, Blazor Portal, Worker Service, Business, Library, Console, and Database projects.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)
- (Optional) EF Core CLI tools: `dotnet tool install --global dotnet-ef`

## Getting Started

### 1. Clone

```bash
git clone https://github.com/your-org/CustomTemplate.git
cd CustomTemplate
```

### 2. Configure

Update the connection string in both:

- `Template.API/appsettings.json`
- `Template.WorkerService/appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=CustomTemplateDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Configure SMTP settings in `TblEmailConfig` (seeded via migrations) or update `appsettings.json` as needed.

### 3. Apply Migrations

```bash
dotnet ef database update --project Template.Database --startup-project Template.API
```

### 4. Run

```bash
# API (default: https://localhost:5001)
dotnet run --project Template.API

# Worker Service (background jobs)
dotnet run --project Template.WorkerService

# Blazor Portal (admin UI)
dotnet run --project Template.Portal
```

### 5. Build All

```bash
dotnet build
```

## Projects

| Project | Description |
|---|---|
| `Template.API` | REST API with JWT auth, Swagger, health checks |
| `Template.Portal` | Blazor Server admin dashboard |
| `Template.WorkerService` | Coravel-based scheduled jobs (email, notifications, stats, integrity) |
| `Template.Business` | Business logic and service layer |
| `Template.Database` | EF Core DbContext, migrations, seed data |
| `Template.Library` | Shared models, entities, constants, extensions |
| `Template.Console` | Console utility app |

## Scheduled Jobs (Worker Service)

| Job | Schedule | Description |
|---|---|---|
| `EmailServiceJob` | Every 5 min | Sends queued emails via MailKit SMTP, retries up to 3 times |
| `NotificationJob` | Every 5 min | Processes pending notifications |
| `DataIntegrityJob` | Daily 00:00 | Runs data consistency checks |
| `SystemUsageStatsJob` | Daily 23:00 | Snapshots system usage metrics to `TblSystemUsageStats` |