# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Services/JobCategory/JobCategory.API/JobCategory.API.csproj \
     src/Services/JobCategory/JobCategory.API/
COPY src/Services/JobCategory/JobCategory.Application/JobCategory.Application.csproj \
     src/Services/JobCategory/JobCategory.Application/
COPY src/Services/JobCategory/JobCategory.Domain/JobCategory.Domain.csproj \
     src/Services/JobCategory/JobCategory.Domain/
COPY src/Services/JobCategory/JobCategory.Infrastructure/JobCategory.Infrastructure.csproj \
     src/Services/JobCategory/JobCategory.Infrastructure/
RUN dotnet restore src/Services/JobCategory/JobCategory.API/JobCategory.API.csproj

COPY src/Services/JobCategory/ src/Services/JobCategory/
RUN dotnet publish src/Services/JobCategory/JobCategory.API/JobCategory.API.csproj \
    -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "JobCategory.API.dll"]
