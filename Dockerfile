# Use o ASP.NET base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5001

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    clang zlib1g-dev
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./protocol-rabbitqm.api/protocol-rabbitqm.api.csproj", "protocol-rabbitqm.api/"]
RUN dotnet restore "./protocol-rabbitqm.api/protocol-rabbitqm.api.csproj"
COPY . .
WORKDIR "/src/protocol-rabbitqm.api"
RUN dotnet build "./protocol-rabbitqm.api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./protocol-rabbitqm.api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 5001
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "shop.manoel.api.dll"]