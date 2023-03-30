cd Basket.Service
dotnet publish
docker build . -f Dockerfile.basket -t <your-acr-name>.azurecr.io/basket:aks
docker push <your-acr-name>.azurecr.io/basket:aks

cd ..


cd Catalog.Service
dotnet publish
docker build . -f Dockerfile.catalog -t <your-acr-name>.azurecr.io/catalog:aks
docker push <your-acr-name>.azurecr.io/catalog:aks

cd ..
cd ApiGateway

dotnet publish
docker build . -f Dockerfile.gateway -t <your-acr-name>.azurecr.io/gateway:aks
docker push <your-acr-name>.azurecr.io/gateway:aks

cd ..

cd Ordering.Service
dotnet publish
docker build . -f Dockerfile.ordering -t <your-acr-name>.azurecr.io/ordering:aks
docker push <your-acr-name>.azurecr.io/ordering:aks
cd ..

cd UI

dotnet publish
docker build . -f Dockerfile.musicstore -t <your-acr-name>.azurecr.io/musicstore:aks
docker push <your-acr-name>.azurecr.io/musicstore:aks

cd ..
