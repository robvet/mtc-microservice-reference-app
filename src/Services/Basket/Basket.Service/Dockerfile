#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Basket/Basket.Service/basket.service.csproj", "Services/Basket/Basket.Service/"]
COPY ["Plumbing/EventBus/EventBus.csproj", "Plumbing/EventBus/"]
COPY ["Plumbing/RestCommunicator/RestCommunicator.csproj", "Plumbing/RestCommunicator/"]
COPY ["Plumbing/ServiceLocator/ServiceLocator.csproj", "Plumbing/ServiceLocator/"]
COPY ["Plumbing/SharedUtilities/SharedUtilities.csproj", "Plumbing/SharedUtilities/"]
RUN dotnet restore "Services/Basket/Basket.Service/basket.service.csproj"
COPY . .
WORKDIR "/src/Services/Basket/Basket.Service"
RUN dotnet build "basket.service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "basket.service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "basket.service.dll"]