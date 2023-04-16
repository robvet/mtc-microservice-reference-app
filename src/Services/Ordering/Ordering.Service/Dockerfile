#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Ordering/Ordering.Service/order.service.csproj", "Services/Ordering/Ordering.Service/"]
COPY ["SharedServices/EventBus/EventBus.csproj", "SharedServices/EventBus/"]
COPY ["SharedServices/SharedUtilities/SharedUtilities.csproj", "SharedServices/SharedUtilities/"]
COPY ["Services/Ordering/order.domian/order.domian.csproj", "Services/Ordering/order.domian/"]
COPY ["Services/Ordering/Ordering.Infrastructure.NoSql/ordering.infrastructure.nosql.csproj", "Services/Ordering/Ordering.Infrastructure.NoSql/"]
RUN dotnet restore "Services/Ordering/Ordering.Service/order.service.csproj"
COPY . .
WORKDIR "/src/Services/Ordering/Ordering.Service"
RUN dotnet build "order.service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "order.service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "order.service.dll"]