
# Stage 1: Build and publish the API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy project file and restore dependencies
COPY ./Captain/Captain.csproj ./
RUN dotnet restore Captain.csproj

# Copy the remaining source code
COPY ./Captain ./

# Publish in Release mode
RUN dotnet publish Captain.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/publish ./

# ASP.NET Core listens on port 8080
ENV ASPNETCORE_HTTP_PORTS=8080

EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "Captain.dll"]
