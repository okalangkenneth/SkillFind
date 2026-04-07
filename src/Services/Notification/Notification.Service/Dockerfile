# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Services/Notification/Notification.Domain/Notification.Domain.csproj \
     src/Services/Notification/Notification.Domain/
COPY src/Services/Notification/Notification.Service/Notification.Service.csproj \
     src/Services/Notification/Notification.Service/
RUN dotnet restore src/Services/Notification/Notification.Service/Notification.Service.csproj

COPY src/Services/Notification/ src/Services/Notification/
RUN dotnet publish src/Services/Notification/Notification.Service/Notification.Service.csproj \
    -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Notification.Service.dll"]
