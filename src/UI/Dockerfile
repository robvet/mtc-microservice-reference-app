#FROM microsoft/dotnet:2.0.0-sdk-2.0.2-nanoserver-1709
FROM microsoft/dotnet:2.0.0-sdk-2.0.2-nanoserver
#FROM microsoft/dotnet:2.0.0-sdk-2.0.2-jessie
WORKDIR dockerdemo
ADD . .
ENTRYPOINT ["dotnet","bin/Debug/netcoreapp2.0/publish/MusicStore.dll"]
EXPOSE 80
ENV ASPNETCORE_URLS http://0.0.0.0:80
