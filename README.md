# Person API (.NET)

## Description

An example API and web application built with ASP.NET Core (.NET 8) and Entity Framework Core. It manages entities like Personas, Profesiones, Estudios, and Telefonos. The application is dockerized using Docker Compose for easy setup and execution, including automatic database migrations.

## Technology Stack

* .NET 8
* ASP.NET Core MVC
* Entity Framework Core 8
* SQL Server (via Docker)
* Docker / Docker Compose
* C#
* Repository Pattern
* Bootstrap 5
* jQuery & jQuery Validation

## Prerequisites

Before you begin, ensure you have the following installed:

* **Docker Desktop:** Make sure Docker Desktop is installed and running on your machine (Windows, macOS, or Linux). Download from [https://www.docker.com/products/docker-desktop/](https://www.docker.com/products/docker-desktop/).
* **Git:** Required for cloning the repository.

## Running with Docker Compose (Recommended Method)

This method starts both the web application and the SQL Server database within Docker containers. Database migrations are applied automatically when the application container starts.

1.  **Clone or Download:** Get the repository code onto your local machine.
    ```bash
    # Example using git clone
    # git clone <repository_url>
    # cd personapi-dotnet
    ```

2.  **Configure Database Password: [Optional]**
    * In the root directory of the project, create a file named `.env`.
    * Add the following line to the `.env` file, replacing `Your_Strong_Password123!` with a secure password of your choice:
        ```dotenv
        # .env
        DB_PASSWORD=Password123
        ```
    * **Important:** The `docker-compose.yml` file is already configured to read the password from this `.env` file. There's a hardcoded password directly in `docker-compose.yml`.

3.  **Open a terminal or command prompt** in the project's root directory (the folder containing `docker-compose.yml`).

4.  **Build and Run:** Execute the following command:
    ```bash
    docker-compose up --build -d
    ```
    * `--build`: Builds the application's Docker image the first time or if the code has changed.
    * `-d`: Runs the containers in the background (detached mode).
    * The first time you run this, it might take a few minutes to download the SQL Server image and build the application image.

5.  **Initialization:** Wait for the containers to start. The `docker-compose.yml` includes a health check for the database. The application container will wait for the database to be healthy before starting and then attempt to apply database migrations. You can monitor the progress using:
    ```bash
    docker-compose logs -f app
    ```
    *(Press `Ctrl+C` to stop viewing logs).* Look for messages indicating migrations are being applied or the database schema is up-to-date.

6.  **Access the Application:** Once the application container is running (check `docker ps`), open your web browser and navigate to:
    [http://localhost:5094](http://localhost:5094)
    *(Note: This uses port 5094 on your host machine, as configured in `docker-compose.yml`)*

## Stopping the Application

To stop and remove the containers:

1.  Open a terminal in the project's root directory.
2.  Run:
    ```bash
    docker-compose down
    ```
    * If you also want to delete the persisted database data volume, use `docker-compose down -v`.

## Project Structure Overview

* `personapi-dotnet/`: Contains the ASP.NET Core project source code.
    * `Controllers/`: MVC controllers.
    * `Models/`: Contains Entities, DbContext, Repository Interfaces, and Repository Implementations.
    * `Views/`: Razor MVC views.
    * `Migrations/`: EF Core database migration files.
    * `wwwroot/`: Static assets (CSS, JS, libraries).
    * `Program.cs`: Application startup and service configuration.
    * `appsettings.json`: Configuration files (DB connection string here is often overridden by Docker Compose environment variables).
* `Dockerfile`: Instructions to build the .NET application's Docker image.
* `docker-compose.yml`: Defines the application and database services for Docker Compose.
* `.dockerignore`: Specifies files/folders to exclude from the Docker build context.
* `.env`: Stores environment variables (like the DB password) used by Docker Compose (Not committed to Git).


![image](https://github.com/user-attachments/assets/c1908673-3d90-4c44-b399-4b7857cf8721)

---
