# This script will generate a .YML file for both Docker Compose and Service Fabric Compose
# Set the local parameters to your values

####################################################
# Input information for the following three fields 
####################################################
# Initial used when creating resources:
# Initial used when creating resources:
$YourInitials = "your-initials"
#Resource group name: 
$ResourceGroupName = "your-resourcegroup-name"
#Set the Azure Subscription Id: 
$subscriptionId = "your-subscription-id" 

####################################################
# Modify the file name and location 
####################################################
$filePath="c:\aaworkshop\"

####################################################
# DO NOT CHANGE VALUES BENEATH THIS LINE
####################################################

#If user not logged into Azure account, redirect to login screen
if ([string]::IsNullOrEmpty($(Get-AzureRmContext).Account)) {Login-AzureRmAccount}


#Helpers
$quote='"'
$colon=':'
$comma=','
$equal='='
$dollarsign='$'
$space=' '
$https="https://"
$suffix="activateazure"

#CosmosDB database name
$DBName = -join($YourInitials,'-activateazure')

#If more than one under your account
Select-AzureRmSubscription -SubscriptionId $subscriptionId
#Verify Current Subscription
Get-AzureRmSubscription –SubscriptionId $subscriptionId

Write-Host "----------------------------------------------------------------------------------------------"
Write-Host " Processing, please be patient..."                                     
Write-Host "----------------------------------------------------------------------------------------------" 
Write-Host 

#Get server name for ACR
$AzureContainerRegistryName=$YourInitials+"acractivateazure.azurecr.io"
$fileName="Docker-compose.yml"

# Create new file
$File = New-Item $filePath$fileName -type file -force 

#Build storage acccount settings
$storageAccountHeader="StorageAccount"
$storageKeyHeader="StorageKey"
$storageAccount="$quote$storageAccountHeader$quote$colon$quote$YourInitials$suffix$quote$comma"

#Get the Storage account key - primary key 
$accountName = "$YourInitials$suffix"
$storagePrimKey = (Get-AzureRmStorageAccountKey -ResourceGroupName $ResourceGroupName -Name $YourInitials$suffix).Value[0] 
$storageAccountKey="$quote$storageKeyHeader$quote$colon$quote$storagePrimKey$quote$comma"

#Following code fetches storage account connection string -- not needed now, but great to have
##Add-Content -Path $File  "//Azure Storage Connection string" 
##$storageConnectionString= "DefaultEndpointsProtocol=https;AccountName=$accountName;AccountKey=$storagePrimKey;EndpointSuffix=core.windows.net"
##Add-Content -Path $File  $storageConnectionString 

#Get the database connection string for the Catalog database
$catalogConnectionStringHeader="CatalogConnectionString"
$sqlLoginSuffix = "sqllogin"
$sqlPasswordSuffix = "pass@word1$"
$userId = "$YourInitials$sqlLoginSuffix"
$password = "$YourInitials$sqlPasswordSuffix"
$ServerName = "$YourInitials-activateazure.database.windows.net"

$catalogDBString= "Server=tcp:$YourInitials-activateazure.database.windows.net,1433;Initial Catalog=ActivateAzure.Catalog;Persist Security Info=False;User ID=$userId;Password=$password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
$concatenatedCatalogString = "$quote$catalogConnectionStringHeader$quote$colon$quote$catalogDBString$quote$comma"

# Get the Topic connection string
$topicPrefix = "ServiceBusPublisherConnectionString"
$topic = (Get-AzureRmServiceBusKey -ResourceGroupName $ResourceGroupName -Namespace $YourInitials-ActivateAzure -Name RootManageSharedAccessKey).PrimaryConnectionString
$topicSuffix = ";EntityPath=eventbustopic"
$topicString="$quote$topicPrefix$quote$colon$quote$topic$topicSuffix$quote$comma"

#Get Cosmos connection information
# Get the list of keys for the CosmosDB database
$myKeys = Invoke-AzureRmResourceAction -Action listKeys `
    -ResourceType "Microsoft.DocumentDb/databaseAccounts" `
    -ApiVersion "2016-03-31" `
    -ResourceGroupName $ResourceGroupName `
    -Name $DBName -Force
  
# pull out the primary key
$primaryKey = $myKeys.primaryMasterKey;

# This method 'should' get the connection string but does not return anything 
#Invoke-AzureRmResourceAction -Action listConnectionStrings `
#    -ResourceType "Microsoft.DocumentDb/databaseAccounts" `
#    -ApiVersion "2016-03-31" `
#    -ResourceGroupName $ResourceGroupName `
#    -Name $DBName

# Get the CosmosDB connection URI
$cosmosUriHeader="CosmosEndpoint"
$cosmosUriString="$YourInitials-activateazure.documents.azure.com:443"
$cosmosUri="$quote$cosmosUriHeader$quote$colon$quote$https$cosmosUriString$quote$comma"

# Get the CosmosDB Primay Key
$cosmosKeyHeader="CosmosPrimaryKey"
$cosmosKey="$quote$cosmosKeyHeader$quote$colon$quote$primaryKey$quote$comma"

#Build database connection environmental variable...

#For training class, we must deal with quirky compose rules: Docker Compose requires you to delimit a $ with another $; ServiceFabric compose does not
$dollarsign="$"

#Get the database connection string for the Catalog database
$catalogConnectionStringHeader="      - CatalogConnectionString"
$sqlLoginSuffix = "sqllogin"
#Note that an addtional dollar sign is required when including a dollar sign when using compose 
$sqlPasswordSuffix = "pass@word1$"
$userId = "$YourInitials$sqlLoginSuffix"
$password = "$YourInitials$sqlPasswordSuffix"
$ServerName = "$YourInitials-activateazure.database.windows.net"
$catalogDBString= "Server=tcp:$YourInitials-activateazure.database.windows.net,1433;Initial Catalog=ActivateAzure.Catalog;Persist Security Info=False;User ID=$userId;Password=$password$dollarsign;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30"
$concatenatedCatalogString = "$catalogConnectionStringHeader$equal$catalogDBString"

#Build storage account environmental variables...
$storageAccount="      - $storageAccountHeader$equal$YourInitials$suffix"
$storageAccountKey ="      - $storageKeyHeader$equal$storagePrimKey"

#Build cosmos connection environmental variables...
$cosmosUri="      - $cosmosUriHeader$equal$https$cosmosUriString"
$cosmosKey="      - $cosmosKeyHeader$equal$primaryKey"

#Build service bus environmental variable...
$topicString="      - $topicPrefix$equal$topic$topicSuffix"

#Top header
Add-Content -Path $File  "version: '3'" 
Add-Content -Path $File  "" 
Add-Content -Path $File  "services:" 
Add-Content -Path $File  "" 

#Music Store
Add-Content -Path $File  "  musicstore:" 
$imageTag =  "    image: "
$musicStoreImage="musicstore:1.0"
$musicStoreImageString = "$imageTag$musicStoreImage"
Add-Content -Path $File  $musicStoreImageString 
Add-Content -Path $File  "    build:" 
Add-Content -Path $File  "      context: ./UI" 
Add-Content -Path $File  "      dockerfile: Dockerfile" 
Add-Content -Path $File  "    environment:" 
Add-Content -Path $File  '      - "ApiGateway=http://apigateway.api:8084"' 
Add-Content -Path $File  "    depends_on:" 
Add-Content -Path $File  "      - apigateway.api" 
Add-Content -Path $File  "" 

#ApiGateway
Add-Content -Path $File  "  apigateway.api:" 
$gatewayImage="gateway:1.0"
$gatewayImageString = "$imageTag$gatewayImage"
Add-Content -Path $File  $gatewayImageString 
Add-Content -Path $File  "    build:" 
Add-Content -Path $File  "      context: ./ApiGateway" 
Add-Content -Path $File  "      dockerfile: Dockerfile" 
Add-Content -Path $File  "    depends_on:" 
Add-Content -Path $File  "      - catalog.api" 
Add-Content -Path $File  "      - basket.api" 
Add-Content -Path $File  "      - ordering.api" 
Add-Content -Path $File  "    environment:" 
Add-Content -Path $File  '      - "Catalog=http://catalog.api:8082"' 
Add-Content -Path $File  '      - "Basket=http://basket.api:8083"' 
Add-Content -Path $File  '      - "Ordering=http://ordering.api:8085"' 
Add-Content -Path $File  "" 

#Catalog.API
Add-Content -Path $File  "  catalog.api:" 
$catalogImage="catalog:1.0"
$catalogImageString = "$imageTag$catalogImage"
Add-Content -Path $File  $catalogImageString 
Add-Content -Path $File  "    build:" 
Add-Content -Path $File  "      context: ./Catalog.Service" 
Add-Content -Path $File  "      dockerfile: Dockerfile" 
Add-Content -Path $File  "    environment:" 
Add-Content -Path $File  $concatenatedCatalogString 
Add-Content -Path $File  $topicString 
Add-Content -Path $File  "" 

#Basket.API
Add-Content -Path $File  "  basket.api:" 
$basketImage="basket:1.0"
$basketImageString = "$imageTag$basketImage"
Add-Content -Path $File  $basketImageString 
Add-Content -Path $File  "    build:" 
Add-Content -Path $File  "      context: ./Basket.Service" 
Add-Content -Path $File  "      dockerfile: Dockerfile" 
Add-Content -Path $File  "    depends_on:" 
Add-Content -Path $File  "      - catalog.api" 
Add-Content -Path $File  "    environment:" 
Add-Content -Path $File  '      - "Catalog=http://catalog.api:8082"' 
Add-Content -Path $File  $storageAccount 
Add-Content -Path $File  $storageAccountKey 
Add-Content -Path $File  $topicString 
Add-Content -Path $File  "" 

#Ordering.API
Add-Content -Path $File  "  ordering.api:" 
$orderingImage="ordering:1.0"
$orderingImageString = "$imageTag$orderingImage"
Add-Content -Path $File  $orderingImageString 
Add-Content -Path $File  "    build:" 
Add-Content -Path $File  "      context: ./Ordering.Service" 
Add-Content -Path $File  "      dockerfile: Dockerfile" 
Add-Content -Path $File  "    environment:" 
Add-Content -Path $File  $cosmosUri 
Add-Content -Path $File  $cosmosKey 
Add-Content -Path $File  $topicString 

Write-Host "**********************************************************************************************" 
Write-Host "* Done                                                                                       *" 
Write-Host "*                                                                                            *" 
Write-Host "* You have generated the Docker compose                                                      *"
Write-Host "* It can be found at c:\aaworkshop\docker-compose.yml                                        *"
Write-Host "*                                                                                            *" 
Write-Host "**********************************************************************************************"

