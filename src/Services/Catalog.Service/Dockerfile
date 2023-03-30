#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Catalog.Service/Catalog.API.csproj", "Services/Catalog.Service/"]
COPY ["BuildingBlocks/CommandBus/CommandBus.csproj", "BuildingBlocks/CommandBus/"]
COPY ["BuildingBlocks/EventBus/EventBus.csproj", "BuildingBlocks/EventBus/"]
COPY ["BuildingBlocks/RestCommunication/RestCommunication.csproj", "BuildingBlocks/RestCommunication/"]
COPY ["BuildingBlocks/ServiceDiscovery/ServiceDiscovery.csproj", "BuildingBlocks/ServiceDiscovery/"]
COPY ["BuildingBlocks/Utilities/Utilities.csproj", "BuildingBlocks/Utilities/"]
COPY ["BuildingBlocks/Snowflake/Snowflake.csproj", "BuildingBlocks/Snowflake/"]
RUN dotnet restore "Services/Catalog.Service/Catalog.API.csproj"
COPY . .
WORKDIR "/src/Services/Catalog.Service"
RUN dotnet build "Catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]