# Use the official .NET 6 SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy the project file and restore dependencies
COPY eShopLegacyMVC_Core/*.csproj ./eShopLegacyMVC_Core/
RUN dotnet restore eShopLegacyMVC_Core/eShopLegacyMVC_Core.csproj

# Copy the rest of the application code
COPY eShopLegacyMVC_Core/ ./eShopLegacyMVC_Core/
WORKDIR /app/eShopLegacyMVC_Core
RUN dotnet publish -c Release -o out

# Use the runtime image for final execution
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/eShopLegacyMVC_Core/out ./

# Expose ports
EXPOSE 80
EXPOSE 5000

# Set the entry point
ENTRYPOINT ["dotnet", "eShopLegacyMVC_Core.dll"]
