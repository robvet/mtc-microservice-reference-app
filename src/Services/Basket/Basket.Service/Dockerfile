#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Basket/Basket.Service/basket.service.csproj", "Services/Basket/Basket.Service/"]
COPY ["SharedServices/EventBus/EventBus.csproj", "SharedServices/EventBus/"]
COPY ["SharedServices/RestCommunicator/RestCommunicator.csproj", "SharedServices/RestCommunicator/"]
COPY ["SharedServices/ServiceLocator/ServiceLocator.csproj", "SharedServices/ServiceLocator/"]
COPY ["SharedServices/SharedUtilities/SharedUtilities.csproj", "SharedServices/SharedUtilities/"]
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