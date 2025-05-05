# Person API (.NET)

## Description

An example API and web application built with ASP.NET Core (.NET 8) and Entity Framework Core. It manages entities like Personas, Profesiones, Estudios, and Telefonos. The application is dockerized using Docker Compose for easy setup and execution, including automatic database migrations.

<br/>

![image](https://github.com/user-attachments/assets/bbb2bf6b-f9e1-4e15-a2d2-33c2046da091)

<br/>

## Technology Stack

- .NET 8
- ASP.NET Core MVC
- Entity Framework Core 8
- SQL Server (via Docker)
- Docker / Docker Compose
- C#
- jQuery & jQuery Validation

## Prerequisites

Before you begin, ensure you have the following installed:

- **Docker Desktop:** Make sure Docker Desktop is installed and running on your machine (Windows, macOS, or Linux). Download from [https://www.docker.com/products/docker-desktop/](https://www.docker.com/products/docker-desktop/).
- **Git:** Required for cloning the repository.

## Running with Docker Compose

This method starts both the web application and the SQL Server database within Docker containers. Database migrations are applied automatically when the application container starts.

1.  **Clone or Download:** Get the repository code onto your local machine.

    ```bash
    # Example using git clone
    # git clone <repository_url>
    # cd personapi-dotnet
    ```

2.  **Configure Database Password: [Optional]**

    - In the root directory of the project, create a file named `.env`.
    - Add the following line to the `.env` file, replacing `Password123` with a secure password of your choice:
      ```dotenv
      # .env
      DB_PASSWORD=Password123
      ```
    - **Important:** The `docker-compose.yml` file is already configured to read the password from this `.env` file. There's a hardcoded password directly in `docker-compose.yml`.

3.  **Open a terminal or command prompt** in the project's root directory (the folder containing `docker-compose.yml`).

4.  **Build and Run:** Execute the following command:

    ```bash
    docker-compose up --build -d
    ```

    - `--build`: Builds the application's Docker image the first time or if the code has changed.
    - `-d`: Runs the containers in the background (detached mode).
    - The first time you run this, it might take a few minutes to download the SQL Server image and build the application image.

5.  **Initialization:** Wait for the containers to start. The application container includes a short delay (`sleep 10` in the entrypoint) before starting the application to allow the database time to initialize.

6.  **Access the Application:** Once the application container is running (check `docker ps`), open your web browser and navigate to:
    `http://localhost:8080`
    _(Note: This uses port 8080 on your host machine, as configured in `docker-compose.yml`)_

## Stopping the Application

To stop and remove the containers:

1.  Open a terminal in the project's root directory.
2.  Run:
    ```bash
    docker-compose down
    ```
    - If you also want to delete the persisted database data volume, use `docker-compose down -v`.

## Project Structure Overview

- `personapi-dotnet/`: Contains the ASP.NET Core project source code.
  - `Controllers/`: MVC controllers.
  - `Models/`: Contains Entities, DbContext, Repository Interfaces, and Repository Implementations.
  - `Views/`: Razor MVC views.
  - `Migrations/`: EF Core database migration files.
  - `wwwroot/`: Static assets (CSS, JS, libraries).
  - `Program.cs`: Application startup and service configuration.
  - `appsettings.json`: Configuration files (DB connection string here is often overridden by Docker Compose environment variables).
- `Dockerfile`: Instructions to build the .NET application's Docker image.
- `docker-compose.yml`: Defines the application and database services for Docker Compose.
- `.dockerignore`: Specifies files/folders to exclude from the Docker build context.
- `.env`: Stores environment variables (like the DB password) used by Docker Compose (Not committed to Git).

## Database Configuration and Migrations

The application uses Entity Framework Core (EF Core) to manage database schema and data. Here's how it works:

1. **Connection String**: The application reads the connection string named "DefaultConnection" from `appsettings.json`. This connection string specifies how to connect to the SQL Server database.

2. **DbContext Registration**: The `PersonaDbContext` is registered to be used with SQL Server, using the connection string read from the configuration.

3. **Applying Migrations**: When the application starts, it creates a scope and retrieves the `PersonaDbContext` service. It then calls `context.Database.Migrate()` to apply any pending migrations to the database. This ensures that the database schema is up-to-date with the application's data model.

4. **Data Seeding**: After applying migrations, the `DataSeeder.SeedData(context)` method is called to populate the database with initial data.

5. **Error Handling**: If an error occurs during migration or seeding, it is logged using the application's logging framework.

This setup ensures that your database is automatically configured and populated with the necessary tables and initial data when the application starts.

---
