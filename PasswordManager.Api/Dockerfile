# ----------- Build Stage -----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution
COPY PasswordManager.sln ./

# Copy all project files
COPY PasswordManager.Api/PasswordManager.Api.csproj PasswordManager.Api/
COPY PasswordManager.Core/PasswordManager.Core.csproj PasswordManager.Core/
COPY PasswordManager.Infrastructure/PasswordManager.Infrastructure.csproj PasswordManager.Infrastructure/
COPY PasswordManager.Tests/PasswordManager.Tests.csproj PasswordManager.Tests/

# Restore all projects
RUN dotnet restore PasswordManager.sln

# Copy everything
COPY . .

# Build API project in Release
RUN dotnet build PasswordManager.Api/PasswordManager.Api.csproj -c Release -o /app/build

# ----------- Publish Stage -----------
FROM build AS publish
RUN dotnet publish PasswordManager.Api/PasswordManager.Api.csproj -c Release -o /app/publish

# ----------- Final Runtime Image -----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .

# Expose port (Render uses its own internal port anyway)
EXPOSE 8080

# Start the API
ENTRYPOINT ["dotnet", "PasswordManager.Api.dll"]
