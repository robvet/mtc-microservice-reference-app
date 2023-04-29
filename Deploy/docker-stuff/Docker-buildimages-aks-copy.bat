cd Basket.Service
dotnet publish
docker build . -f Dockerfile.basket -t VetContainerRegistry.azurecr.io/basket:aks
docker push VetContainerRegistry.azurecr.io/basket:aks

cd ..


cd Catalog.Service
dotnet publish
docker build . -f Dockerfile.catalog -t VetContainerRegistry.azurecr.io/catalog:aks
docker push VetContainerRegistry.azurecr.io/catalog:aks

cd ..
cd ApiGateway

dotnet publish
docker build . -f Dockerfile.gateway -t VetContainerRegistry.azurecr.io/gateway:aks
docker push VetContainerRegistry.azurecr.io/gateway:aks

cd ..

cd Ordering.Service
dotnet publish
docker build . -f Dockerfile.ordering -t VetContainerRegistry.azurecr.io/ordering:aks
docker push VetContainerRegistry.azurecr.io/ordering:aks
cd ..

cd UI

dotnet publish
docker build . -f Dockerfile.musicstore -t VetContainerRegistry.azurecr.io/musicstore:aks
docker push VetContainerRegistry.azurecr.io/musicstore:aks

cd ..
