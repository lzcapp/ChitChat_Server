#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ChitChat_Server.csproj", "."]
RUN dotnet restore "./ChitChat_Server.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./ChitChat_Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ChitChat_Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChitChat_Server.dll"]

HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 CMD curl https://chitchat.seeleo.com/chitchat || exit 1
