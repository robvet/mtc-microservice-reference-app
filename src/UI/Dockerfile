#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["UI/MusicStore.csproj", "UI/"]
COPY ["Plumbing/SharedUtilities/SharedUtilities.csproj", "Plumbing/SharedUtilities/"]
RUN dotnet restore "UI/MusicStore.csproj"
COPY . .
WORKDIR "/src/UI"
RUN dotnet build "MusicStore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MusicStore.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MusicStore.dll"]