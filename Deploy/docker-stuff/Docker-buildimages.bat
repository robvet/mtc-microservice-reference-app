cd Catalog.Service
dotnet publish
docker build . -t catalog:1.0

cd ..
cd ApiGateway

dotnet publish
docker build . -t gateway:1.0

cd ..

cd Ordering.Service
dotnet publish
docker build . -t ordering:1.0

cd ..

cd UI

dotnet publish
docker build . -t musicstore:1.0

cd ..

