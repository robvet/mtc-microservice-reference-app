#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Ordering.Service/Ordering.API.csproj", "Services/Ordering.Service/"]
COPY ["BuildingBlocks/EventBus/EventBus.csproj", "BuildingBlocks/EventBus/"]
COPY ["BuildingBlocks/Utilities/Utilities.csproj", "BuildingBlocks/Utilities/"]
COPY ["Services/Ordering.Domain/Ordering.Domain.csproj", "Services/Ordering.Domain/"]
COPY ["BuildingBlocks/Snowflake/Snowflake.csproj", "BuildingBlocks/Snowflake/"]
COPY ["Services/Ordering.Infrastructure.NoSql/Ordering.Infrastructure.NoSql.csproj", "Services/Ordering.Infrastructure.NoSql/"]
RUN dotnet restore "Services/Ordering.Service/Ordering.API.csproj"
COPY . .
WORKDIR "/src/Services/Ordering.Service"
RUN dotnet build "Ordering.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]