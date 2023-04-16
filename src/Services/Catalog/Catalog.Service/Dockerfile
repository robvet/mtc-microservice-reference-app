#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Catalog/Catalog.Service/catalog.service.csproj", "Services/Catalog/Catalog.Service/"]
COPY ["SharedServices/EventBus/EventBus.csproj", "SharedServices/EventBus/"]
COPY ["SharedServices/SharedUtilities/SharedUtilities.csproj", "SharedServices/SharedUtilities/"]
RUN dotnet restore "Services/Catalog/Catalog.Service/catalog.service.csproj"
COPY . .
WORKDIR "/src/Services/Catalog/Catalog.Service"
RUN dotnet build "catalog.service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "catalog.service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "catalog.service.dll"]