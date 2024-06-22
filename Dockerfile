# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY api.csproj ./
RUN dotnet restore ./api.csproj

# Copy the remaining files and build the application
COPY . .
RUN dotnet build ./api.csproj -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish ./api.csproj -c Release -o /app/publish

# Use the official .NET ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy the published files from the publish stage
COPY --from=publish /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "api.dll"]
