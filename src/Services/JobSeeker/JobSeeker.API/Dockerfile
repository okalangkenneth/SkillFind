# syntax=docker/dockerfile:1
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY NuGet.Config .
COPY src/Services/JobSeeker/JobSeeker.API/JobSeeker.API.csproj \
     src/Services/JobSeeker/JobSeeker.API/
COPY src/Services/JobSeeker/JobSeeker.Application/JobSeeker.Application.csproj \
     src/Services/JobSeeker/JobSeeker.Application/
COPY src/Services/JobSeeker/JobSeeker.Domain/JobSeeker.Domain.csproj \
     src/Services/JobSeeker/JobSeeker.Domain/
COPY src/Services/JobSeeker/JobSeeker.Infrastructure/JobSeeker.Infrastructure.csproj \
     src/Services/JobSeeker/JobSeeker.Infrastructure/
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore src/Services/JobSeeker/JobSeeker.API/JobSeeker.API.csproj

COPY src/Services/JobSeeker/ src/Services/JobSeeker/
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish src/Services/JobSeeker/JobSeeker.API/JobSeeker.API.csproj \
    -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "JobSeeker.API.dll"]
