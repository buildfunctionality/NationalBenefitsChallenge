# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["Products.Api/Products.Api.csproj", "Products.Api/"]
RUN dotnet restore "Products.Api/Products.Api.csproj"

# Copy the remaining source code and build the application
COPY . .
WORKDIR "/src/Products.Api"
RUN dotnet build "Products.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Products.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage: Build the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Build the test project
RUN dotnet build --no-restore -c Release

# Run the tests
CMD dotnet test --no-build -c Release

# Set the entry point for the application
ENTRYPOINT ["dotnet", "Products.Api.dll"]