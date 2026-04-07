# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Services/ApiGateway/ApiGateway.csproj src/Services/ApiGateway/
RUN dotnet restore src/Services/ApiGateway/ApiGateway.csproj

COPY src/Services/ApiGateway/ src/Services/ApiGateway/
RUN dotnet publish src/Services/ApiGateway/ApiGateway.csproj \
    -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
