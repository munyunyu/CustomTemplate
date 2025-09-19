# Install and Use the Custom Solution Template

This repository is set up to be used as a reusable .NET template. It includes a multi-project solution with API, Blazor (Portal), Worker Service, Business, Library, Console, and Database projects.

## Prerequisites
- .NET 7 SDK (or newer)

## Install the template locally
From the repository root (`c:\Users\Percy\source\repos\CustomTemplate`):

# Optional: remove any previous installed copy
```powershell
dotnet new uninstall .

```powershell
# Install the template from the current directory
 dotnet new install .

# Verify it's installed
 dotnet new list | findstr custom-sln
```

If you ever need to uninstall the template:
```powershell
 dotnet new uninstall .
```

## Create a new solution from the template
Choose a destination folder where you want the new solution generated, then run:

```powershell
# Creates a new solution based on this template
 dotnet new custom-sln -n MyNewSolution
```

- The `-n` name will be used to rename:
  - The solution file from `Template.sln` to `MyNewSolution.sln`.
  - All project names and folders where `Template` appears (e.g., `Template.API` -> `MyNewSolution.API`).
- You can also pass optional parameters:

```powershell
 dotnet new custom-sln -n MyNewSolution --Company Contoso
```

### Database naming

By default, the API connection string in `Template.API/appsettings.json` uses `Database=TemplateDB`. Because the template uses `sourceName: "Template"`, the database name will automatically be renamed when you specify `-n`:

```text
TemplateDB  ->  MyNewSolutionDB
```

If you prefer a different database name that is not tied to `-n`, you can change it after generation in your `appsettings.json`, or I can add a dedicated `--DatabaseName` parameter to the template by request.

## What gets generated
- A full solution referencing these projects:
  - MyNewSolution.API
  - MyNewSolution.Portal (Blazor)
  - MyNewSolution.WorkerService
  - MyNewSolution.Business
  - MyNewSolution.Library
  - MyNewSolution.Console
  - MyNewSolution.Database (EF Core with migrations)

## Next steps after generation
1. Restore and build
   ```powershell
    dotnet restore
    dotnet build
   ```
2. Open the solution in Visual Studio or VS Code
3. Update appsettings as needed (connection strings, logging, etc.)
4. Run your preferred startup project(s)

## Notes
- Source renaming is driven by:
  - `sourceName: "Template"` — replaces occurrences in file/folder names and file contents.
  - `SolutionName` symbol — replaces occurrences of `CustomTemplate` (solution file name) inside files.
- Unwanted files/folders like `bin/`, `obj/`, `.vs/`, and `*.user` are excluded from the template.

- Case sensitivity: replacements are case-sensitive. For example, lowercase `template` (like `https://template.co.zw`) will not change to `mynewsolution` automatically. If you want case-insensitive or lowercase replacements, let me know and I can add the appropriate configuration.

## Troubleshooting
- If you see old names lingering, clear the template cache and re-install:
  ```powershell
   dotnet new uninstall .
   dotnet new install .
  ```
- Confirm your .NET SDK is the expected version:
  ```powershell
   dotnet --info
  ```
