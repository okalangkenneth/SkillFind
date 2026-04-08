# syntax=docker/dockerfile:1
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY NuGet.Config .
COPY src/Services/JobPosting/JobPosting.API/JobPosting.API.csproj \
     src/Services/JobPosting/JobPosting.API/
COPY src/Services/JobPosting/JobPosting.Application/JobPosting.Application.csproj \
     src/Services/JobPosting/JobPosting.Application/
COPY src/Services/JobPosting/JobPosting.Domain/JobPosting.Domain.csproj \
     src/Services/JobPosting/JobPosting.Domain/
COPY src/Services/JobPosting/JobPosting.Infrastructure/JobPosting.Infrastructure.csproj \
     src/Services/JobPosting/JobPosting.Infrastructure/
COPY src/Services/Notification/Notification.Domain/Notification.Domain.csproj \
     src/Services/Notification/Notification.Domain/
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore src/Services/JobPosting/JobPosting.API/JobPosting.API.csproj

COPY src/Services/JobPosting/ src/Services/JobPosting/
COPY src/Services/Notification/Notification.Domain/ src/Services/Notification/Notification.Domain/
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish src/Services/JobPosting/JobPosting.API/JobPosting.API.csproj \
    -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "JobPosting.API.dll"]
