# This script will generate a .YAML Manifest file for Azure Kubernetes Service.
# Set the local parameters to your values

####################################################
# Input information for the following three fields 
####################################################
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

$AzureContainerRegistryName=$YourInitials+"acractivateazure.azurecr.io"
$fileName="k8deployment.yaml"

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
$dollarsign=""

#Get the database connection string for the Catalog database
$catalogConnectionStringHeader="CatalogConnectionString"
$sqlLoginSuffix = "sqllogin"
#Note that an addtional dollar sign is required when including a dollar sign when using compose 
$sqlPasswordSuffix = "pass@word1$"
$userId = "$YourInitials$sqlLoginSuffix"
$password = "$YourInitials$sqlPasswordSuffix"
$ServerName = "$YourInitials-activateazure.database.windows.net"
$catalogDBString= "Server=tcp:$YourInitials-activateazure.database.windows.net,1433;Initial Catalog=ActivateAzure.Catalog;Persist Security Info=False;User ID=$userId;Password=$password$dollarsign;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30"
$concatenatedCatalogString = "$catalogConnectionStringHeader$equal$catalogDBString"

#Build storage account environmental variables...
$storageAccount="$storageAccountHeader$equal$YourInitials$suffix"
$storageAccountKey ="$storageKeyHeader$equal$storagePrimKey"

#Build cosmos connection environmental variables...
$cosmosUri="      - $cosmosUriHeader$equal$https$cosmosUriString"
$cosmosKey="      - $cosmosKeyHeader$equal$primaryKey"

#Build service bus environmental variable...
$topicString="$topicPrefix$equal$topic$topicSuffix"

$catalogImage="catalog:aks"
$basketImage="basket:aks"
$gatewayImage="gateway:aks" 
$orderingImage="ordering:aks"
$uiImage="musicstore:aks"

############################################
#Metadata section for Catalog
############################################
Add-Content -Path $File  "apiVersion: extensions/v1beta1" 
Add-Content -Path $File  "kind: Deployment" 
Add-Content -Path $File  "metadata:" 
Add-Content -Path $File  "  name: catalog" 
Add-Content -Path $File  "spec:" 
Add-Content -Path $File  "  replicas: 1" 
Add-Content -Path $File  "  template:" 
Add-Content -Path $File  "    metadata:" 
Add-Content -Path $File  "      labels:" 
Add-Content -Path $File  "        app: musicstore" 
Add-Content -Path $File  "        component: catalog" 
Add-Content -Path $File  "    spec:" 
Add-Content -Path $File  "      containers:" 
Add-Content -Path $File  "      - name: catalog" 
Add-Content -Path $File  "        image: mswsaksdocker/akslinuximages:catalog" 
Add-Content -Path $File  "        env:" 
Add-Content -Path $File  "        - name: GET_HOSTS_FROM" 
Add-Content -Path $File  "          value: dns" 
Add-Content -Path $File  "        - name: CatalogConnectionString" 
Add-Content -Path $File  "          value: $catalogDBString" 
Add-Content -Path $File  "        - name: ServiceBusPublisherConnectionString" 
Add-Content -Path $File  "          value: $topic$topicSuffix" 
Add-Content -Path $File  "        ports:" 
Add-Content -Path $File  "        - containerPort: 80" 
Add-Content -Path $File  "---" 
############################################
#Metadata section for Basket
############################################
Add-Content -Path $File  "apiVersion: extensions/v1beta1" 
Add-Content -Path $File  "kind: Deployment" 
Add-Content -Path $File  "metadata:" 
Add-Content -Path $File  "  name: basket" 
Add-Content -Path $File  "spec:" 
Add-Content -Path $File  "  replicas: 1" 
Add-Content -Path $File  "  template:" 
Add-Content -Path $File  "    metadata:" 
Add-Content -Path $File  "      labels:" 
Add-Content -Path $File  "        app: musicstore" 
Add-Content -Path $File  "        component: basket" 
Add-Content -Path $File  "    spec:" 
Add-Content -Path $File  "      containers:" 
Add-Content -Path $File  "      - name: basket" 
Add-Content -Path $File  "        image: mswsaksdocker/akslinuximages:basket"
Add-Content -Path $File  "        env:" 
Add-Content -Path $File  "        - name: GET_HOSTS_FROM" 
Add-Content -Path $File  "          value: dns" 
Add-Content -Path $File  "        - name: StorageAccount" 
Add-Content -Path $File  "          value: $YourInitials$suffix" 
Add-Content -Path $File  "        - name: StorageKey" 
Add-Content -Path $File  "          value: $storagePrimKey" 
Add-Content -Path $File  "        - name: catalog" 
Add-Content -Path $File  "          value: http://catalog" 
Add-Content -Path $File  "        - name: ServiceBusPublisherConnectionString" 
Add-Content -Path $File  "          value: $topic$topicSuffix" 
Add-Content -Path $File  "        ports:" 
Add-Content -Path $File  "        - containerPort: 80" 
Add-Content -Path $File  "---"
############################################
#Metadata section for Gateway
############################################
Add-Content -Path $File  "apiVersion: extensions/v1beta1" 
Add-Content -Path $File  "kind: Deployment" 
Add-Content -Path $File  "metadata:" 
Add-Content -Path $File  "  name: gateway" 
Add-Content -Path $File  "spec:" 
Add-Content -Path $File  "  replicas: 1" 
Add-Content -Path $File  "  template:" 
Add-Content -Path $File  "    metadata:" 
Add-Content -Path $File  "      labels:" 
Add-Content -Path $File  "        app: musicstore" 
Add-Content -Path $File  "        component: gateway" 
Add-Content -Path $File  "    spec:" 
Add-Content -Path $File  "      containers:" 
Add-Content -Path $File  "      - name: gateway" 
Add-Content -Path $File  "        image: mswsaksdocker/akslinuximages:gateway"
Add-Content -Path $File  "        env:" 
Add-Content -Path $File  "        - name: GET_HOSTS_FROM" 
Add-Content -Path $File  "          value: dns" 
Add-Content -Path $File  "        - name: Catalog" 
Add-Content -Path $File  "          value: http://catalog" 
Add-Content -Path $File  "        - name: Basket" 
Add-Content -Path $File  "          value: http://basket" 
Add-Content -Path $File  "        - name: Ordering" 
Add-Content -Path $File  "          value: http://ordering" 
Add-Content -Path $File  "        ports:" 
Add-Content -Path $File  "        - containerPort: 80" 
Add-Content -Path $File  "---"
############################################
#Metadata section for Ordering
############################################
Add-Content -Path $File  "apiVersion: extensions/v1beta1" 
Add-Content -Path $File  "kind: Deployment" 
Add-Content -Path $File  "metadata:" 
Add-Content -Path $File  "  name: ordering" 
Add-Content -Path $File  "spec:" 
Add-Content -Path $File  "  replicas: 1" 
Add-Content -Path $File  "  template:" 
Add-Content -Path $File  "    metadata:" 
Add-Content -Path $File  "      labels:" 
Add-Content -Path $File  "        app: musicstore" 
Add-Content -Path $File  "        component: ordering" 
Add-Content -Path $File  "    spec:" 
Add-Content -Path $File  "      containers:" 
Add-Content -Path $File  "      - name: ordering" 
Add-Content -Path $File  "        image: mswsaksdocker/akslinuximages:ordering"
Add-Content -Path $File  "        env:" 
Add-Content -Path $File  "        - name: GET_HOSTS_FROM" 
Add-Content -Path $File  "          value: dns" 
Add-Content -Path $File  "        - name: CosmosEndpoint" 
Add-Content -Path $File  "          value: $https$cosmosUriString" 
Add-Content -Path $File  "        - name: CosmosPrimaryKey" 
Add-Content -Path $File  "          value: $primaryKey" 
Add-Content -Path $File  "        - name: ServiceBusPublisherConnectionString" 
Add-Content -Path $File  "          value: $topic$topicSuffix" 
Add-Content -Path $File  "        ports:" 
Add-Content -Path $File  "        - containerPort: 80" 
Add-Content -Path $File  "---"
############################################
#Metadata section for UI
############################################
Add-Content -Path $File  "apiVersion: extensions/v1beta1" 
Add-Content -Path $File  "kind: Deployment" 
Add-Content -Path $File  "metadata:" 
Add-Content -Path $File  "  name: ui" 
Add-Content -Path $File  "spec:" 
Add-Content -Path $File  "  replicas: 1" 
Add-Content -Path $File  "  template:" 
Add-Content -Path $File  "    metadata:" 
Add-Content -Path $File  "      labels:" 
Add-Content -Path $File  "        app: musicstore" 
Add-Content -Path $File  "        component: ui" 
Add-Content -Path $File  "    spec:" 
Add-Content -Path $File  "      containers:" 
Add-Content -Path $File  "      - name: ui" 
Add-Content -Path $File  "        image: mswsaksdocker/akslinuximages:musicstore"
Add-Content -Path $File  "        env:" 
Add-Content -Path $File  "        - name: GET_HOSTS_FROM" 
Add-Content -Path $File  "          value: dns" 
Add-Content -Path $File  "        - name: ApiGateway" 
Add-Content -Path $File  "          value: http://gateway"
Add-Content -Path $File  "        ports:" 
Add-Content -Path $File  "        - containerPort: 80" 
Add-Content -Path $File  "---"
############################################
#Service Metadata section for Basket
############################################
Add-Content -Path $File  "apiVersion: v1" 
Add-Content -Path $File  "kind: Service" 
Add-Content -Path $File  "metadata:" 
Add-Content -Path $File  "  labels:" 
Add-Content -Path $File  "    app: musicstore" 
Add-Content -Path $File  "  name: basket" 
Add-Content -Path $File  "spec:" 
Add-Content -Path $File  "  ports:" 
Add-Content -Path $File  "  - port: 80" 
Add-Content -Path $File  "  selector:" 
Add-Content -Path $File  "    app: musicstore" 
Add-Content -Path $File  "    component: basket" 
Add-Content -Path $File  "---"
############################################
#Service Metadata section for Catalog
############################################
Add-Content -Path $File  "apiVersion: v1" 
Add-Content -Path $File  "kind: Service" 
Add-Content -Path $File  "metadata:" 
Add-Content -Path $File  "  labels:" 
Add-Content -Path $File  "    app: musicstore" 
Add-Content -Path $File  "  name: catalog" 
Add-Content -Path $File  "spec:" 
Add-Content -Path $File  "  ports:" 
Add-Content -Path $File  "  - port: 80" 
Add-Content -Path $File  "  selector:" 
Add-Content -Path $File  "    app: musicstore" 
Add-Content -Path $File  "    component: catalog" 
Add-Content -Path $File  "  type: LoadBalancer  " 
Add-Content -Path $File  "---"
############################################
#Service Metadata section for Gateway
############################################
Add-Content -Path $File  "apiVersion: v1" 
Add-Content -Path $File  "kind: Service" 
Add-Content -Path $File  "metadata:" 
Add-Content -Path $File  "  labels:" 
Add-Content -Path $File  "    app: musicstore" 
Add-Content -Path $File  "  name: gateway" 
Add-Content -Path $File  "spec:" 
Add-Content -Path $File  "  ports:" 
Add-Content -Path $File  "  - port: 80" 
Add-Content -Path $File  "  selector:" 
Add-Content -Path $File  "    app: musicstore" 
Add-Content -Path $File  "    component: gateway" 
Add-Content -Path $File  "  type: LoadBalancer" 
Add-Content -Path $File  "---"
############################################
#Service Metadata section for Ordering
############################################
Add-Content -Path $File  "apiVersion: v1" 
Add-Content -Path $File  "kind: Service" 
Add-Content -Path $File  "metadata:" 
Add-Content -Path $File  "  labels:" 
Add-Content -Path $File  "    app: musicstore" 
Add-Content -Path $File  "  name: ordering" 
Add-Content -Path $File  "spec:" 
Add-Content -Path $File  "  ports:" 
Add-Content -Path $File  "  - port: 80" 
Add-Content -Path $File  "  selector:" 
Add-Content -Path $File  "    app: musicstore" 
Add-Content -Path $File  "    component: ordering" 
Add-Content -Path $File  "---"
############################################
#Service Metadata section for Music Store
############################################
Add-Content -Path $File  "apiVersion: v1" 
Add-Content -Path $File  "kind: Service" 
Add-Content -Path $File  "metadata:" 
Add-Content -Path $File  "  labels:" 
Add-Content -Path $File  "    app: musicstore" 
Add-Content -Path $File  "  name: ui" 
Add-Content -Path $File  "spec:" 
Add-Content -Path $File  "  ports:" 
Add-Content -Path $File  "  - port: 80" 
Add-Content -Path $File  "  selector:" 
Add-Content -Path $File  "    app: musicstore" 
Add-Content -Path $File  "    component: ui" 
Add-Content -Path $File  "  type: LoadBalancer" 
Add-Content -Path $File  "---"

Write-Host "**********************************************************************************************" 
Write-Host "* Done                                                                                       *" 
Write-Host "*                                                                                            *" 
Write-Host "* You have generated the YAML compose file for Azure Kubenetes Service                       *"
Write-Host "* It can be found at c:\aaworkshop\k8deployment.yml                                          *"
Write-Host "*                                                                                            *" 
Write-Host "**********************************************************************************************"

