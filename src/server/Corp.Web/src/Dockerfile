# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG BUILD_CONFIGURATION=Release

# Restore NuGet packages and cache the results.
WORKDIR /src

# removeme-syntax=docker/dockerfile:1.7-labs
# COPY --parents server/*/*.csproj .
COPY ["./server/Corp.Data/src/Corp.Data.csproj", "./server/Corp.Data/src/"]
COPY ["./server/Corp.Data.Npgsql/src/Corp.Data.Npgsql.csproj", "./server/Corp.Data.Npgsql/src/"]
COPY ["./server/Corp.Data.Sqlite/src/Corp.Data.Sqlite.csproj", "./server/Corp.Data.Sqlite/src/"]
COPY ["./server/Corp.Sdk/src/Corp.Sdk.csproj", "./server/Corp.Sdk/src/"]
COPY ["./server/Corp.Services/src/Corp.Services.csproj", "./server/Corp.Services/src/"]
COPY ["./server/Corp.Web/src/Corp.Web.csproj", "./server/Corp.Web/src/"]

RUN dotnet restore /src/server/Corp.Web/src/Corp.Web.csproj

COPY . .
WORKDIR /src
RUN dotnet build server/Corp.Web/src/Corp.Web.csproj -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish server/Corp.Web/src/Corp.Web.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
ARG ASPNETCORE_ENVIRONMENT
ENV ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Corp.Web.dll"]