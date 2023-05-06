#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Order/Order.Service/order.service.csproj", "Services/Order/Order.Service/"]
COPY ["Plumbing/EventBus/EventBus.csproj", "Plumbing/EventBus/"]
COPY ["Plumbing/SharedUtilities/SharedUtilities.csproj", "Plumbing/SharedUtilities/"]
COPY ["Services/Order/Order.Domain/order.domain.csproj", "Services/Order/Order.Domain/"]
COPY ["Services/Order/Order.Infrastructure.NoSql/order.infrastructure.nosql.csproj", "Services/Order/Order.Infrastructure.NoSql/"]
RUN dotnet restore "Services/Order/Order.Service/order.service.csproj"
COPY . .
WORKDIR "/src/Services/Order/Order.Service"
RUN dotnet build "order.service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "order.service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "order.service.dll"]