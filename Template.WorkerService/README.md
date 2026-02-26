# Template.WorkerService

A .NET Worker Service that runs scheduled background jobs using **Cronos** cron expressions.  
Every job automatically logs its schedule and full execution history to the database.

---

## Table of Contents

1. [Architecture overview](#architecture-overview)
2. [Prerequisites](#prerequisites)
3. [Configuration](#configuration)
4. [Database setup (migrations)](#database-setup-migrations)
5. [Running locally](#running-locally)
6. [Installing as a Windows Service](#installing-as-a-windows-service)
7. [Managing the Windows Service](#managing-the-windows-service)
8. [Viewing logs](#viewing-logs)
9. [Viewing job history in the database](#viewing-job-history-in-the-database)
10. [Adding a new job](#adding-a-new-job)
11. [Updating the cron schedule of an existing job](#updating-the-cron-schedule-of-an-existing-job)
12. [Disabling a job without redeploying](#disabling-a-job-without-redeploying)
13. [Deploying an update](#deploying-an-update)
14. [Troubleshooting](#troubleshooting)

---

## Architecture overview

```
Template.WorkerService
│
├── Job/
│   ├── Base/
│   │   └── CronJobService.cs        ← abstract base (scheduling + DB logging)
│   └── SystemJob/
│       ├── EmailServiceJob.cs       ← processes TblEmailQueue (every 5 min)
│       └── DataIntegrityServiceJob.cs  ← integrity checks (daily at 00:30)
│
├── Extensions/
│   └── ScheduledServiceExtensions.cs  ← AddCronJob<T>() DI helper
│
├── Worker.cs                        ← RabbitMQ consumer (runs alongside jobs)
└── Program.cs                       ← host builder / DI registration
```

### Database tables written by the service

| Table | Description |
|---|---|
| `TblJobSchedule` | One row per job class. Stores cron expression, `IsEnabled`, `NextRunTime`, `LastRunTime`, `LastRunStatus`. Auto-created or updated on every service start. |
| `TblJobHistory` | One row per execution. Stores `StartedAt`, `FinishedAt`, `Status`, `AffectedRecords`, `ErrorMessage`, `Notes`. |

---

## Prerequisites

- .NET 10 SDK — [download](https://aka.ms/dotnet/download)
- SQL Server (any edition, including LocalDB for local dev)
- *(Optional)* RabbitMQ if using the `Worker` consumer

---

## Configuration

All settings live in `appsettings.json` (or `appsettings.Development.json` for local overrides).

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=TemplateDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "RabbitMQSettings": {
    "RabbitMQUrl": "localhost",
    "Username": "guest",
    "Password": "guest",
    "VirtualHost": "/",
    "Port": 5672
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  }
}
```

> For production, keep secrets out of `appsettings.json`. Use environment variables or a secrets manager:
> ```powershell
> $env:ConnectionStrings__DefaultConnection = "Server=prod-sql;..."
> ```

### Available cron expressions (`CronExpressions` constants)

| Constant | Schedule |
|---|---|
| `EveryMinute` | Every minute |
| `Every5Minutes` | Every 5 minutes |
| `Every30Minutes` | Every 30 minutes |
| `EveryHour` | Every hour |
| `DailyAt0030` | Daily at 00:30 |
| `DailyAt2330` | Daily at 23:30 |
| `EveryDayAtMidnight` | Daily at 00:00 |
| `WeekdaysAt9AM` | Mon–Fri at 09:00 |
| `WeekendsAtNoon` | Sat–Sun at 12:00 |
| `SaturdayFirstHour` | Saturday at 00:00 |

Custom expressions use **6-field Cronos format** (seconds included):
```
0 */15 * * * *   ← every 15 minutes
0 0 2 * * *      ← daily at 02:00
0 0 9 * * MON    ← every Monday at 09:00
```

---

## Database setup (migrations)

Run these once from the **solution root** after the first checkout or after adding new jobs:

```powershell
# Create the migration
dotnet ef migrations add AddJobScheduleAndHistory `
  --project Template.Database `
  --startup-project Template.API

# Apply to the database
dotnet ef database update `
  --project Template.Database `
  --startup-project Template.API
```

> The `TblJobSchedule` and `TblJobHistory` tables will be created automatically.

---

## Running locally

```powershell
# From the solution root
dotnet run --project Template.WorkerService

# Or from the project folder
cd Template.WorkerService
dotnet run
```

Console output example:
```
info: EmailServiceJob[0]
      EmailServiceJob: started with expression [0 */5 * * * *].
info: EmailServiceJob[0]
      EmailServiceJob: next run at 2026-02-26T15:25:00
info: DataIntegrityServiceJob[0]
      DataIntegrityServiceJob: started with expression [0 30 0 * * *].
```

---

## Installing as a Windows Service

### 1. Publish a self-contained executable

```powershell
dotnet publish Template.WorkerService `
  --configuration Release `
  --runtime win-x64 `
  --self-contained true `
  --output C:\Services\TemplateWorker
```

### 2. Add the Windows Service hosting support

The `.csproj` already references `Microsoft.Extensions.Hosting`. For Windows Service mode, add this to `Program.cs` if not already present:

```csharp
IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()          // ← enables Windows Service mode
    .ConfigureServices(...)
    .Build();
```

### 3. Register the service with `sc.exe`

Open **PowerShell as Administrator**:

```powershell
sc.exe create "TemplateWorkerService" `
  binPath= "C:\Services\TemplateWorker\Template.WorkerService.exe" `
  DisplayName= "Template Worker Service" `
  start= auto

sc.exe description "TemplateWorkerService" "Runs scheduled background jobs (email queue, data integrity, etc.)"
```

### 4. Start the service

```powershell
sc.exe start "TemplateWorkerService"
```

---

## Managing the Windows Service

```powershell
# Start
sc.exe start "TemplateWorkerService"

# Stop (graceful — waits for running jobs to finish)
sc.exe stop "TemplateWorkerService"

# Restart
sc.exe stop "TemplateWorkerService"
sc.exe start "TemplateWorkerService"

# Check current status
sc.exe query "TemplateWorkerService"

# Remove the service (stop first)
sc.exe stop  "TemplateWorkerService"
sc.exe delete "TemplateWorkerService"
```

You can also manage it from **Services** (`services.msc`) or **Task Manager → Services** tab.

---

## Viewing logs

### Console / local run
Logs print directly to the terminal.

### Windows Service logs — Event Viewer
1. Open **Event Viewer** (`eventvwr.msc`)
2. Navigate to **Windows Logs → Application**
3. Filter by **Source = `TemplateWorkerService`**

### Windows Service logs — PowerShell

```powershell
# Last 50 entries for this service
Get-EventLog -LogName Application -Source "TemplateWorkerService" -Newest 50

# Only errors
Get-EventLog -LogName Application -Source "TemplateWorkerService" -EntryType Error -Newest 20
```

### Structured logging (recommended for production)

The service uses `Microsoft.Extensions.Logging`. Add a Serilog sink to `appsettings.json` to write to a file:

```json
"Serilog": {
  "WriteTo": [
    { "Name": "Console" },
    {
      "Name": "File",
      "Args": {
        "path": "C:\\Logs\\TemplateWorker\\log-.txt",
        "rollingInterval": "Day",
        "retainedFileCountLimit": 30
      }
    }
  ]
}
```

And add the NuGet package:
```powershell
dotnet add Template.WorkerService package Serilog.Sinks.File
```

---

## Viewing job history in the database

### Current schedule status (next / last run per job)

```sql
SELECT
    JobName,
    CronExpression,
    IsEnabled,
    NextRunTime,
    LastRunTime,
    LastRunStatus,
    CreatedDate
FROM TblJobSchedule
ORDER BY JobName;
```

### Full execution history

```sql
SELECT
    h.JobName,
    h.StartedAt,
    h.FinishedAt,
    DATEDIFF(SECOND, h.StartedAt, h.FinishedAt) AS DurationSeconds,
    h.Status,
    h.AffectedRecords,
    h.Notes,
    h.ErrorMessage
FROM TblJobHistory h
ORDER BY h.StartedAt DESC;
```

### Failures only

```sql
SELECT
    JobName,
    StartedAt,
    FinishedAt,
    ErrorMessage,
    Notes
FROM TblJobHistory
WHERE Status = 7   -- Status.Failed = 7
ORDER BY StartedAt DESC;
```

### Last 7 days summary per job

```sql
SELECT
    JobName,
    COUNT(*)                                          AS TotalRuns,
    SUM(CASE WHEN Status = 0 THEN 1 ELSE 0 END)      AS Succeeded,
    SUM(CASE WHEN Status = 7 THEN 1 ELSE 0 END)      AS Failed,
    SUM(AffectedRecords)                              AS TotalAffectedRecords,
    AVG(DATEDIFF(SECOND, StartedAt, FinishedAt))      AS AvgDurationSeconds
FROM TblJobHistory
WHERE StartedAt >= DATEADD(DAY, -7, GETUTCDATE())
GROUP BY JobName
ORDER BY JobName;
```

### Status codes reference

| Value | Name | Meaning |
|---|---|---|
| 0 | `Success` | Completed without errors |
| 3 | `InProgress` | Currently running |
| 6 | `Cancelled` | Cancelled via shutdown signal |
| 7 | `Failed` | Threw an unhandled exception |

---

## Adding a new job

### 1. Create the job class

Add a new file under `Job/SystemJob/` (or your own sub-folder):

```csharp
using Microsoft.Extensions.DependencyInjection;
using Template.Business.Interfaces.System;
using Template.Library.Models.Shedular;
using Template.WorkerService.Job.Base;

namespace Template.WorkerService.Job.SystemJob
{
    public class MyNewJob : CronJobService
    {
        private readonly ILogger<MyNewJob> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public MyNewJob(
            IScheduleConfig<MyNewJob> config,
            ILogger<MyNewJob> logger,
            IServiceScopeFactory scopeFactory)
            : base(config.CronExpression, config.TimeZoneInfo, logger, scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public override async Task<JobResult> DoWork(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<IDatabaseService>();

            // ── your logic here ───────────────────────────────────────────────
            var processed = 0;

            // ...

            return JobResult.WithRecords(processed, $"Processed {processed} record(s).");
        }
    }
}
```

### 2. Register it in `Program.cs`

```csharp
services.AddCronJob<MyNewJob>(c =>
{
    c.TimeZoneInfo = TimeZoneInfo.Local;
    c.CronExpression = CronExpressions.EveryHour;   // or any custom expression
});
```

That's it. On next startup the job will:
- Write a row to `TblJobSchedule`
- Log its next run time to the console
- Write a `TblJobHistory` row on every execution

---

## Updating the cron schedule of an existing job

1. Open `Program.cs`
2. Change the `CronExpression` value for the target job:

```csharp
services.AddCronJob<EmailServiceJob>(c =>
{
    c.TimeZoneInfo = TimeZoneInfo.Local;
    c.CronExpression = CronExpressions.EveryHour;   // was Every5Minutes
});
```

3. Redeploy / restart the service. The `TblJobSchedule` row is updated automatically on startup.

---

## Disabling a job without redeploying

Set `IsEnabled = 0` directly in the database:

```sql
UPDATE TblJobSchedule
SET IsEnabled = 0, LastUpdatedDate = GETUTCDATE()
WHERE JobName = 'EmailServiceJob';
```

> **Note:** The `IsEnabled` flag is stored for reporting and admin-UI purposes.  
> To fully prevent the job from running you must also remove or comment out its `AddCronJob<T>()` registration and redeploy. A future enhancement can make the scheduler check this flag at runtime before firing.

---

## Deploying an update

### Replacing the Windows Service binaries

```powershell
# 1. Stop the service
sc.exe stop "TemplateWorkerService"

# 2. Publish the new build
dotnet publish Template.WorkerService `
  --configuration Release `
  --runtime win-x64 `
  --self-contained true `
  --output C:\Services\TemplateWorker

# 3. Restart the service
sc.exe start "TemplateWorkerService"
```

> All job history is preserved in the database — the `TblJobSchedule` row is simply updated with the new cron expression on startup.

### Run any new migrations before starting

```powershell
dotnet ef database update `
  --project Template.Database `
  --startup-project Template.API
```

---

## Troubleshooting

| Symptom | Check |
|---|---|
| Service fails to start | Run `sc.exe query "TemplateWorkerService"` and check Event Viewer → Application |
| `Login failed for user` error | Verify `ConnectionStrings:DefaultConnection` in `appsettings.json` and that the service account has DB access |
| Job never runs | Confirm the cron expression is 6-field (seconds included), e.g. `0 */5 * * * *` not `*/5 * * * *` |
| Job row missing from `TblJobSchedule` | DB migration may not have been applied — run `dotnet ef database update` |
| `TblJobHistory` shows `Status = 7` (Failed) | Query `ErrorMessage` column: `SELECT TOP 10 JobName, ErrorMessage FROM TblJobHistory WHERE Status = 7 ORDER BY StartedAt DESC` |
| RabbitMQ connection errors on startup | RabbitMQ is optional — set `Worker` as not started or point to a running instance in `appsettings.json` |
| Old cron expression still in use after deploy | Check `TblJobSchedule.CronExpression` — it is updated on startup; confirm the service actually restarted |

### Useful diagnostic queries

```sql
-- Is the service running? (InProgress rows older than 10 min indicate a hung job)
SELECT JobName, StartedAt, Status
FROM TblJobHistory
WHERE Status = 3  -- InProgress
  AND StartedAt < DATEADD(MINUTE, -10, GETUTCDATE());

-- Recent errors with detail
SELECT TOP 20 JobName, StartedAt, ErrorMessage
FROM TblJobHistory
WHERE ErrorMessage IS NOT NULL
ORDER BY StartedAt DESC;
```
