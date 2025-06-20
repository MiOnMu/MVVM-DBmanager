# MVVM-DBmanager

> 🚧 **This project is currently under active development. Features, structure, and implementation details are subject to change.**

A sample .NET solution implementing the MVVM (Model-View-ViewModel) architectural pattern for database management. This project provides a modular structure separating core business logic, data access, application services, and UI layers, and demonstrates CRUD operations on an underlying database.

---

## Table of Contents

* [Features](#features)
* [Architecture Overview](#architecture-overview)
* [Project Structure](#project-structure)
* [Requirements](#requirements)
* [Installation](#installation)
* [Configuration](#configuration)
* [Usage](#usage)

---

## Features

* MVVM-based UI layer with data binding and commands
* Clean separation of concerns across multiple projects:

  * Core business entities and interfaces
  * Data access layer (Dapper/EF Core ready)
  * Application service layer for use-case orchestration
  * Cross-cutting utilities (logging, validation)
* CRUD operations on database tables
* Swappable storage providers via dependency injection
* Sample UI to view, add, edit, and delete records

## Architecture Overview

This solution follows the MVVM pattern:

* **Model**: `DataBaseManager.Core` contains domain entities (e.g., `User`, `Product`) and repository interfaces.
* **ViewModel**: In `DataBaseManagerUi`, each view (page/window) has a corresponding ViewModel that implements `INotifyPropertyChanged` and commands for actions.
* **View**: XAML UI definitions in `DataBaseManagerUi` bind to ViewModel properties and commands.

Application services (`DataBaseManager.AppService`) encapsulate business use cases, and data access implementations (`DataBaseManager.DataAccess`) handle database operations. Cross-cutting concerns (logging, validation) are centralized in `DataBaseManager.CrossCutting`.

Dependency Injection is configured in the UI project to wire up services and repositories at startup.

## Project Structure

```
protoDbManager.sln
│
├── DataBaseManager.Core             # Core domain models and interfaces
├── DataBaseManager.DataAccess       # Data access implementations
├── DataBaseManager.DataAccess.Contracts # Interfaces for data repositories
├── DataBaseManager.AppService       # Application services orchestrating use cases
├── DataBaseManager.AppService.Contracts # Service layer interfaces
├── DataBaseManager.CrossCutting     # Shared utilities (logging, validation, DI)
├── DataBaseManager                  # Console or host project
└── DataBaseManagerUi                # WPF/WinUI UI project implementing MVVM
```

## Requirements

* [.NET 6 SDK](https://dotnet.microsoft.com/download)
* A SQL-compatible database (SQL Server, SQLite, PostgreSQL, etc.)

## Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/MiOnMu/MVVM-DBmanager.git
   cd MVVM-DBmanager
   ```

2. **Restore NuGet packages**

   ```bash
   dotnet restore
   ```

3. **Build the solution**

   ```bash
   dotnet build
   ```

## Configuration

1. In `DataBaseManagerUi/appsettings.json` (or `appsettings.Development.json`), set your database connection string:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=MyDb;Trusted_Connection=True;"
     }
   }
   ```

2. (Optional) Adjust logging levels or other settings in the same file.

## Usage

  ```bash
  cd DataBaseManager
  dotnet run
  ```

Use to connect, browse tables, and perform CRUD operations.
