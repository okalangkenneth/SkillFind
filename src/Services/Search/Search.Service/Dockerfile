# syntax=docker/dockerfile:1
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY NuGet.Config .
COPY src/Services/Search/Search.Domain/Search.Domain.csproj \
     src/Services/Search/Search.Domain/
COPY src/Services/Search/Search.Service/Search.Service.csproj \
     src/Services/Search/Search.Service/
COPY src/Services/Notification/Notification.Domain/Notification.Domain.csproj \
     src/Services/Notification/Notification.Domain/
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore src/Services/Search/Search.Service/Search.Service.csproj

COPY src/Services/Search/ src/Services/Search/
COPY src/Services/Notification/Notification.Domain/ src/Services/Notification/Notification.Domain/
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish src/Services/Search/Search.Service/Search.Service.csproj \
    -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Search.Service.dll"]
