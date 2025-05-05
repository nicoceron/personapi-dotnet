# Use the official .NET 8 SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the solution file and restore dependencies
COPY *.sln .
COPY personapi-dotnet/*.csproj ./personapi-dotnet/
RUN dotnet restore

# Copy the rest of the application code
COPY . .

# Build and publish the application
WORKDIR /app/personapi-dotnet
RUN dotnet publish -c Release -o out

# Use the official ASP.NET Core runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/personapi-dotnet/out .

# Expose port 8080 and 8081 for the application
EXPOSE 8080
EXPOSE 8081

# Set the entry point for the container
ENTRYPOINT ["dotnet", "personapi-dotnet.dll"] 