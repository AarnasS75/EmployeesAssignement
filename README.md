#  Employees Assignment

A .NET 6.0 console application to get and set employee data using Entity Framework Core with a PostgreSQL database.

## Requirements

- [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Docker](https://www.docker.com/) or a local PostgreSQL instance
- PowerShell

## Setup

**Set up the database**

```powershell
./setUpDatabase.ps1
```

## Usage

```powershell
dotnet run set-employee --employeeId 1 --employeeName John --employeeSalary 500
dotnet run get-employee --employeeId 1
```

For a full list of available commands and options run `dotnet run -- --help`.

## Verifying the submission

```powershell
./verifySubmission.ps1
```

> **Note:** The verification script assumes a clean database on each run. If you have run it before, tear down and recreate the database first, otherwise `get-employee` may return stale data from a previous run.

## Architecture

The application is structured in the following layers:

- **Commands** — `IEmployeeCommandHandler` wires up the CLI and delegates to the factory
- **Factories** — `IEmployeeCommandFactory` builds the `RootCommand` and subcommands using `System.CommandLine`
- **Services** — `IEmployeeService` contains the business logic for getting and setting employees
- **Persistence** — `TaskDbContext` configures EF Core and the database connection
- **Models** — `Employee` entity
- **Constants** — command name constants

## Known Issues

### Missing PRIMARY KEY constraint in database schema

The provided `dbSchema.sql` defines the `employees` table without a `PRIMARY KEY` constraint:

```sql
CREATE TABLE employees (
    employeeid INT NOT NULL,
    employeename VARCHAR(128) NOT NULL,
    employeesalary INT NOT NULL
);
```

Without this constraint the database does not enforce constraint on `employeeid`, which allows duplicate records to be inserted.
A better approach would be to add Constraint
```sql
CREATE TABLE employees (
    employeeid INT NOT NULL,
    employeename VARCHAR(128) NOT NULL,
    employeesalary INT NOT NULL,
    CONSTRAINT pk_employees PRIMARY KEY (employeeid)
);
```