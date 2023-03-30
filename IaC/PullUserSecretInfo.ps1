#####################################################################################
# 
# What does this script do?
# This script will output critical information from your Azure resources into the microservice projects
# User Secrets file. Items like connection strings, storage account keys etc will be put into Azure Key Vault secrets
# The Azure Key Vault secret URL will be put into your User Secrets file
# Although this script is normally executed from the DeployToAzure script, it can be executed separately if you
# supply the parameters at runtime. If you wish to run the script separately, change the directory where this file is 
# location (in the command window below) and then do :
# .\PullUserSecretInfo.ps1 -appName <appName> -ResourceGroupName <resourcegroupfromAzure> -subscriptionId <subscIdfrom Azure>
#
#####################################################################################

Param(
    [string] [Parameter(Mandatory=$true)] $appName,
    [string] $ResourceGroupName,
    [string] $subscriptionId
) 

# **********************************************************************************************
# Should we bounce this script execution?
# **********************************************************************************************
if (($appName -eq '') -or `
    ($ResourceGroupName -eq '') -or `
	($subscriptionId -eq ''))
{
	Write-Host 'You must provide your AppName, Resource Group Name and Subscription ID, before executing' -foregroundcolor Yellow
	exit
}

# Function checks to see if the user is logged in as administrator
function Test-Administrator
{
    $user = [Security.Principal.WindowsIdentity]::GetCurrent()

    (New-Object Security.Principal.WindowsPrincipal $user).IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)  
}

# Did you open Powershell as and admin?
if(!(Test-Administrator))
{
    Write-Host "Please close PowerShell ISE and open PowerShell ISE with Adminstrator access"
    exit;
}

Write-Host 
Write-Host "----------------------------------------------------------------------------------------------"
Write-Host "Your AppName are" $appName
Write-Host "Your ResourceGroup is" $ResourceGroupName
Write-Host "Your SubscriptioniId is" $subscriptionId

##########################################################################################
# Links
# Change directory
# https://docs.microsoft.com/en-us/powershell/scripting/samples/managing-current-location?view=powershell-6
## https://community.spiceworks.com/how_to/121063-using-try-catch-powershell-error-handling
# https://stackoverflow.com/questions/36512102/how-can-i-specify-a-path-relative-to-where-my-script-is-running
# https://docs.microsoft.com/en-us/powershell/scripting/samples/managing-current-location?view=powershell-6
#
# User Secrets
# https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.0&tabs=windows
#
#
##########################################################################################

####################################################
# Modify the file name and location 
####################################################
$filePath="c:\microservices_workshop\"
$fileName="usersecrets.txt"

####################################################
# Set project directory in which script will run
####################################################

# The following constucts set the executing directory
$currentdirectory = get-location

# Get executing directory
$MyDir = $PSScriptRoot
#Write-Host $MyDir

# Move 1 level up in hierarchy
$rootDirectory = $MyDir| Split-Path
#Write-Host "rootDirectory" $rootDirectory

$FullyQualifiedPath = $rootDirectory + "\src\services\"
#Write-Host "FullyQualifiedPath" $FullyQualifiedPath

$finalDirectory = Join-Path -Path $FullyQualifiedPath -ChildPath "Basket.Service"
Write-Host "Executing directory" $finalDirectory
Write-Host "----------------------------------------------------------------------------------------------"
Write-Host 

####################################################
# Switch to write configuration values to the screen.
# Set to False by default. 
# Set to 'true' to write values to screen.
####################################################
$WriteToScreen= "false"

####################################################
# Switch to write secrets to .NET UserSecrets file.
# Set to False by default. 
# Set to 'true' to write secrests to UserSecrets.
####################################################
$WriteSecretsToUserSecrets= "true"

####################################################
# DO NOT CHANGE VALUES BENEATH THIS LINE
####################################################

# If user not logged into Azure account, redirect to login screen
# Need to log in to both AzureRM and az login since we are 
# using az commands
if ([string]::IsNullOrEmpty($(Get-AzureRmContext).Account)) 
{
    try  
    {
        Login-AzureRmAccount 
        #If more than one under your account
        Select-AzureRmSubscription -SubscriptionId $subscriptionId
        #Verify Current Subscription
        Get-AzureRmSubscription –SubscriptionId $subscriptionId
        $VerbosePreference = "SilentlyContinue"

        # Do an az login since we are also using az commands
        az login
        az account set --subscription $subscriptionId
     }
     catch 
     {
        write-host "Exception capturing Authenticating to Azure:" -ForegroundColor Red
        write-host "Exception Type: $($_.Exception.GetType().FullName)" -ForegroundColor Red
        write-host "Exception Message: $($_.Exception.Message)" -ForegroundColor Red
        Exit
     }
}

#Helpers
$quote='"'
$colon=':'
$comma=','
$https="https://"
$kvName = $appName + "keyvault"

#CosmosDB database name
$DBName = -join($appName,'-cosmosdb')
#Config name
$ConfigName = -join($appName,'-appconfig')
#Function name
$FuncAppName = -join($appName,'-functapp')

# Create new file
$File = New-Item $filePath$fileName -type file -force

Write-Host "----------------------------------------------------------------------------------------------"
Write-Host " Processing, please be patient..."                                     
Write-Host "----------------------------------------------------------------------------------------------" 
Write-Host 

#**************************************
# Start Processing
#**************************************
Add-Content -Path $File  '{' 

#Get the Storage account name 
#This is your AppName plus activateazure  
#https://stackoverflow.com/questions/3896258/how-do-i-output-text-without-a-newline-in-powershell

# clear out existing content in user secrets file
dotnet user-secrets clear --project $finalDirectory

# Write default ServiceUrls 
# robvet, 6-28-2018, removed "ServiceUrl" JSON tag hierarchy
#Add-Content -Path $File  '"ServiceUrl": {' 
Add-Content -Path $File  '"ApiGateway": "http://localhost:8084",' 
dotnet user-secrets set "ApiGateway" "http://localhost:8084" --project $finalDirectory

Add-Content -Path $File  '"Catalog": "http://localhost:8082",' 
dotnet user-secrets set "Catalog" "http://localhost:8082" --project $finalDirectory

Add-Content -Path $File  '"Basket": "http://localhost:8083",' 
dotnet user-secrets set "Basket" "http://localhost:8083" --project $finalDirectory

Add-Content -Path $File  '"Ordering": "http://localhost:8085",' 
dotnet user-secrets set "Ordering" "http://localhost:8085" --project $finalDirectory

# robvet, 3/30/2020, added feature flag for gRPC 
#Add-Content -Path $File  '"gRPCFeatureFlag": "True",' 
#dotnet user-secrets set "gRPCFeatureFlag" "True" --project $finalDirectory

####################################################################################
# This function is used to get a jwt access token for the Pricing Engine
# Azure Function. We need to jwt token to get the default function key.
# The default function key is used to construct the URL to the PricingEngine function
# The URL is constructed and put into an Azure Key Vault secret
# All the function does is get the token
####################################################################################
#function Get-FunctionJwtToken
#{
#
#param (
#    [Parameter(Mandatory = $true)]
#    [string]
#    $ResourceGroupName,
#
#    [Parameter(Mandatory = $true)]
#    [string]
#    $FunctionAppName
#)
#
#
#    $publishingCredentials = Invoke-AzureRmResourceAction  `
#        -ResourceGroupName $ResourceGroupName `
#        -ResourceType 'Microsoft.Web/sites/config' `
#        -ResourceName ('{0}/publishingcredentials' -f $FunctionAppName) `
#        -Action list `
#        -ApiVersion 2019-08-01 `
#        -Force
#
#    $base64Credentials = [Convert]::ToBase64String(
#        [Text.Encoding]::ASCII.GetBytes(
#            ('{0}:{1}' -f $publishingCredentials.Properties.PublishingUserName, $publishingCredentials.Properties.PublishingPassword)))
#
#    return Invoke-RestMethod -Uri ('https://{0}.scm.azurewebsites.net/api/functions/admin/token' -f $FunctionAppName) `
#        -Headers @{ Authorization = ('Basic {0}' -f $base64Credentials) }
#
#}

#Build storage acccount settings
try  
    {
         $storageAccountHeader="StorageAccount"
         $storageAccount="$quote$storageAccountHeader$quote$colon$quote$appName" + "storage" + "$quote$comma"
         $storageAccountUserSecrets="$appName" + "storage" 

         Add-Content -Path $File  $storageAccount 
         #dotnet user-secrets set $quote$storageAccountHeader$quote $storageAccountUserSecrets --project $finalDirectory

         #store the storage account name in Azure Key Vault
         #Converts the name to a secure string
         $StorageAccountName = ConvertTo-SecureString -String $storageAccountUserSecrets -AsPlainText -Force

         #Creates a new secret in Azure Key Vault
         Write-Host "Setting storage account name in key vault..."
         Write-Host
         $StorageAccountNameSecret = Set-AzureKeyVaultSecret -VaultName $kvName -Name 'storageaccount' -SecretValue $StorageAccountName -Verbose
    }
catch 
    {
        write-host "Exception capturing Storage Account Name:" -ForegroundColor Red
        write-host "Exception Type: $($_.Exception.GetType().FullName)" -ForegroundColor Red
        write-host "Exception Message: $($_.Exception.Message)" -ForegroundColor Red
        Write-Host "Error with $storageAccount" -ForegroundColor Red
        Exit
    }

#Add-Content -Path $File  " " 

if ($WriteToScreen -eq "True") 
{
    Write-Host "//Azure Storage Account Name"
    Write-Host $storageAccount 
    #Write-Host 
}

# APPCONFIG - ADD GRPC FEATURE
# larrywa 5/3/2020, added feature flag for gRPC to Azure app config

#try
#{
#
#    #Create an environment variable to hold the primary connection string
#    # for Azure App Config
#    $appConfigConnString = az appconfig credential list -n $ConfigName --query "[?name == 'Primary']|[0].connectionString"
#    [System.Environment]::SetEnvironmentVariable('APPCONFIG_CONNSTRING',$appConfigConnString,[System.EnvironmentVariableTarget]::Machine)
#
#    # Use the connection string of App Config to set the feature flag, otherwise you'll
#    # have to use az login to log in first
#    az appconfig feature set --connection-string $appConfigConnString --feature gRPC --only-show-errors --yes
#    az appconfig feature enable --connection-string $appConfigConnString --feature gRPC --only-show-errors --yes
#
#
#    Add-Content -Path $File  "//----------------------------------------------"  
#    Add-Content -Path $File  "// Azure App Config connection string           " 
#    Add-Content -Path $File  "//----------------------------------------------"  
#    Add-Content -Path $File  $appConfigConnString 
#}
#catch
#{
#    write-host "Exception setting Azure App Config gRPC feature flag:" -ForegroundColor Red
#    write-host "Exception Type: $($_.Exception.GetType().FullName)" -ForegroundColor Red
#    write-host "Exception Message: $($_.Exception.Message)" -ForegroundColor Red
#}

# This code is used to build the URL for the PricingEngine function
# The function has to already exist in the function app
###try
###{
###
###  # build the URI to get to the key
###  $uriDefKey = [System.Uri]('https://' + $FuncAppName + '.azurewebsites.net/admin/functions/PricingEngine/keys/default')
###
###  # The function is setup for Function security, so we need to get the 'default' key for the function first
###  $funcDefKey = Invoke-RestMethod -Uri $uriDefKey -Headers @{Authorization = ("Bearer {0}" -f (Get-FunctionJwtToken -ResourceGroupName $ResourceGroupName -FunctionAppName $FuncAppName)) } -Method Post

###  #Build the function URL
###  $funcBaseURL = 'https://' + $FuncAppName + '.azurewebsites.net/api/PricingEngine?code='
###  $funcURL = $funcBaseURL + $funcDefKey.value
###  $SecretFuncURLValue = ConvertTo-SecureString -String $funcURL -AsPlainText -Force


###  #put the function URL into a key vault secret
###  $FuncURLSecret = Set-AzureKeyVaultSecret -VaultName $kvName -Name 'pricingenginefuncsecret' -SecretValue $SecretFuncURLValue -Verbose

###    Add-Content -Path $File  "//----------------------------------------------"  
###    Add-Content -Path $File  "// Azure PricingEnging Function URL             " 
###    Add-Content -Path $File  "//----------------------------------------------"  
###    Add-Content -Path $File  $funcURL 


###}
###catch
###{
###    write-host "Exception getting Azure function key to build URL:" -ForegroundColor Red
###    write-host "Exception Type: $($_.Exception.GetType().FullName)" -ForegroundColor Red
###    write-host "Exception Message: $($_.Exception.Message)" -ForegroundColor Red
###}

#Build storage key settings
try
{
    #Get the Storage account key - primary key 
    $accountName = "$appName" + "storage"
    $storageKeyHeader="StorageKey"
    $storagePrimKey = (Get-AzureRmStorageAccountKey -ResourceGroupName $ResourceGroupName -Name ($appName + "storage")).Value[0] 
    $storageAccountKey="$quote$storageKeyHeader$quote$colon$quote$storagePrimKey$quote$comma"
    $storageAccountKeyUserSecrets="$storagePrimKey"
    
    #****************************************************
    #VetMod-7-12-WriteToUserSecret
    #if ($WriteSecretsToUserSecrets -eq "true") {
        #dotnet user-secrets set "storagekeysecret" $storagePrimKey --project $finalDirectory
    #}
    #****************************************************
        
    #store the key in Azure Key Vault
    #Converts the key to a secure string
    $SecretValue = ConvertTo-SecureString -String $storagePrimKey -AsPlainText -Force

    #Creates a new secret in Azure Key Vault
    Write-Host "Setting secret in key vault..."
    Write-Host
    $StorageSecret = Set-AzureKeyVaultSecret -VaultName $kvName -Name 'storagekeysecret' -SecretValue $SecretValue -Verbose

    Add-Content -Path $File  "//----------------------------------------------"  
    Add-Content -Path $File  "//Azure Storage Account Key" 
    Add-Content -Path $File  "//----------------------------------------------"  
    Add-Content -Path $File  $storageAccountKey 
                
}
catch
{
    write-host "Exception capturing Storage Account Key secrets:" -ForegroundColor Red
    write-host "Exception Type: $($_.Exception.GetType().FullName)" -ForegroundColor Red
    write-host "Exception Message: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Error with $storageAccountKey" -ForegroundColor Red
    Exit
}

if ($WriteToScreen -eq "True") 
{
    Write-Host "//Azure Storage Account Key"
    Write-Host $storageAccountKey
    Write-Host 
}

#Build Sql DB - Catalog settings
try
{
    #Get the database connection string for the Catalog database
    $catalogConnectionStringHeader="CatalogConnectionString"
    $sqlLoginSuffix = "sqllogin"
    $sqlPasswordSuffix = "pass@word1$"
    $userId = "$appName$sqlLoginSuffix"
    $password = "$appName$sqlPasswordSuffix"

    #****************************************************
    #VetMod-7-12-WriteToUserSecret
    #if ($WriteSecretsToUserSecrets -eq "true") {
        #dotnet user-secrets set "catalogsqldbpwsecret" $password --project $finalDirectory
	#}
    #****************************************************

    #Converts the key to a secure string - put DB password in key vault
    $DBPwSecretValue = ConvertTo-SecureString -String $password -AsPlainText -Force
    $DBPasswordSecret = Set-AzureKeyVaultSecret -VaultName $kvName -Name 'catalogsqldbpwsecret' -SecretValue $DBPwSecretValue -Verbose

    # add database connecton string for Catalog SQLDbDatabase
    $ServerName = "$appName-dbsvr.database.windows.net"
    $catalogDBString= "Server=tcp:$appName-dbsvr.database.windows.net,1433;Initial Catalog=Services.Catalog;Persist Security Info=False;User ID=$userId;Password=$password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    $concatenatedCatalogString = "$quote$catalogConnectionStringHeader$quote$colon$quote$catalogDBString$quote$comma"
    $catalogDBStringUserSecrets = "$catalogDBString"

    #****************************************************
    #VetMod-7-12-WriteToUserSecret
    #if ($WriteSecretsToUserSecrets -eq "true") {
        #dotnet user-secrets set "catalogdbsecret" $catalogDBString --project $finalDirectory
	#}
    #****************************************************

    #Converts the key to a secure string - put connection string in key vault
    $DBSecretValue = ConvertTo-SecureString -String $catalogDBString -AsPlainText -Force
    $DBSecret = Set-AzureKeyVaultSecret -VaultName $kvName -Name 'catalogdbsecret' -SecretValue $DBSecretValue -Verbose

    # Add database credentials for troubleshooting
    dotnet user-secrets set "Catalog Sql Database Server Name" $ServerName --project $finalDirectory

    #Add-Content -Path $File  "Sql Database Login = $userId,"
    dotnet user-secrets set "Catalog Sql Database Login" $userId --project $finalDirectory
    dotnet user-secrets set "catalogsqldbpwsecret" $password --project $finalDirectory

    Add-Content -Path $File  $concatenatedCatalogString 

}
catch
{
    write-host "Exception capturing Catalog Sql DB secrets:" -ForegroundColor Red
    write-host "Exception Type: $($_.Exception.GetType().FullName)" -ForegroundColor Red
    write-host "Exception Message: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Error with $storageAccountKey" -ForegroundColor Red
    Exit
}

if ($WriteToScreen -eq "True") 
{
    Write-Host "//Catalog Azure Sql Database credentials" 
    Write-Host "SqlLogin=$userId"
    Write-Host "SqlPassword=$password" 
    Write-Host 
}
if ($WriteToScreen -eq "True") 
{
    Write-Host "//Catalog Database connection string" 
    Write-Host $concatenatedCatalogString 
    Write-Host 
}

#Build Sql DB - Orders settings

#try
#{
#    #Get the database connection string for the Catalog database
#    $ordersConnectionStringHeader="OrdersConnectionString"
#    $sqlLoginSuffix = "sqllogin"
#    $sqlPasswordSuffix = "pass@word1$"
#    $userId = "$appName$sqlLoginSuffix"
#    $password = "$appName$sqlPasswordSuffix"
#
#    #Converts the key to a secure string - put DB password in key vault
#    $DBPwSecretValue = ConvertTo-SecureString -String $password -AsPlainText -Force
#    $DBPasswordSecret = Set-AzureKeyVaultSecret -VaultName $kvName -Name 'orderssqldbpwsecret' -SecretValue $DBPwSecretValue -Verbose
#
#    $ServerName = "$appName-dbsvr.database.windows.net"
#    $ordersDBString= "Server=tcp:$appName-dbsvr.database.windows.net,1433;Initial Catalog=Services.Orders;Persist Security Info=False;User ID=$userId;Password=$password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
#    $concatenatedOrdersString = "$quote$ordersConnectionStringHeader$quote$colon$quote$ordersDBString$quote$comma"
#    $ordersDBStringUserSecrets = "$ordersDBString"
#
#    #****************************************************
#    #VetMod-7-12-WriteToUserSecret
#    #if ($WriteSecretsToUserSecrets -eq "true") {
#    #    dotnet user-secrets set "orderdbsecret" $ordersDBString --project $finalDirectory
#	#}
#    #****************************************************
#
#    #Converts the key to a secure string - put connection string in key vault
#    $DBSecretValue = ConvertTo-SecureString -String $ordersDBString -AsPlainText -Force
#    $DBSecret = Set-AzureKeyVaultSecret -VaultName $kvName -Name 'orderdbsecret' -SecretValue $DBSecretValue -Verbose
#
#    # Add database credentials for troubleshooting
#    dotnet user-secrets set "Orders Sql Database Server Name" $ServerName --project $finalDirectory
#
#
#    #Add-Content -Path $File  "Sql Database Login = $userId,"
#    dotnet user-secrets set "Orders Sql Database Login" $userId --project $finalDirectory
#
#    Add-Content -Path $File  $concatenatedOrdersString 
#
#}
#catch
#{
#    write-host "Exception capturing Orders Sql DB secrets:" -ForegroundColor Red
#    write-host "Exception Type: $($_.Exception.GetType().FullName)" -ForegroundColor Red
#    write-host "Exception Message: $($_.Exception.Message)" -ForegroundColor Red
#    Write-Host "Error with $storageAccountKey" -ForegroundColor Red
#}

#if ($WriteToScreen -eq "True") 
#{
#    Write-Host "//Orders Azure Sql Database credentials" 
#    Write-Host "SqlLogin=$userId"
#    Write-Host "SqlPassword=$password" 
#    Write-Host 
#}


#if ($WriteToScreen -eq "True") 
#{
#    Write-Host "//Orders Database connection string" 
#    Write-Host $concatenatedOrdersString 
#    Write-Host 
#}


#Build Redis Cache settings

try
{
 #Get the Storage account key - primary key 
    $redisKeyHeader = "RedisConnectionString="
    $redisCacheName = "$appName" + "-redis"
    $redisAll = (Get-AzureRmRedisCache -ResourceGroupName $ResourceGroupName -Name $redisCacheName)
    $redisPrimKey = (Get-AzureRmRedisCacheKey -ResourceGroupName $ResourceGroupName -Name $redisCacheName).PrimaryKey  
    $redisEndpoint = $redisAll.HostName + ":" + $redisAll.Port
    
    $redisConnString = ($redisEndpoint + ",password=" + $redisPrimKey + ",ssl=True,abortConnect=False")
    #$redisConnString = $redisEndpoint + ",password=" + $redisPrimKey + ",ssl=True,abortConnect=False"
    
    #****************************************************
    #VetMod-7-12-WriteToUserSecret
    #if ($WriteSecretsToUserSecrets -eq "true") {
    #    dotnet user-secrets set "redisconnstrsecret" $redisConnString --project $finalDirectory    
	#}
    #****************************************************
         
    #store the key in Azure Key Vault
    #Converts the key to a secure string
    $RedisSecretValue = ConvertTo-SecureString -String $redisConnString -AsPlainText -Force

    #Creates a new secret in Azure Key Vault
    Write-Host "Setting secret in key vault..."
    Write-Host
    $RedisSecret = Set-AzureKeyVaultSecret -VaultName $kvName -Name 'redisconnstrsecret' -SecretValue $RedisSecretValue -Verbose
    $kvRedisSecValue = $RedisSecret.Id


    Add-Content -Path $File  "//----------------------------------------------"  
    Add-Content -Path $File  "//Azure Redis Conn string                       "
    Add-Content -Path $File  "//----------------------------------------------"  
    Add-Content -Path $File  $redisConnString 
 
}
catch
{
    write-host "Exception capturing Redis Cache secrets:" -ForegroundColor Red
    write-host "Exception Type: $($_.Exception.GetType().FullName)" -ForegroundColor Red
    write-host "Exception Message: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Error with $topicString" -ForegroundColor Red
    Exit
}


#Build Service Bus settings
try
{
    # Get the Topic connection string
    $topicPrefix = "ServiceBusPublisherConnectionString"
    $topic = (Get-AzureRmServiceBusKey -ResourceGroupName $ResourceGroupName -Namespace $appName-sbns -Name RootManageSharedAccessKey).PrimaryConnectionString
    $topicSuffix = ";EntityPath=eventbustopic"
    $topicString="$quote$topicPrefix$quote$colon$quote$topic$topicSuffix$quote$comma"
    $topicStringUserSecrets="$topic$topicSuffix"

    #****************************************************
    #VetMod-7-12-WriteToUserSecret
    #if ($WriteSecretsToUserSecrets -eq "true") {
    #    dotnet user-secrets set "sbconnstrsecret" $topicStringUserSecrets --project $finalDirectory
	#}
    #****************************************************

    #store the key in Azure Key Vault
    #Converts the key to a secure string
    $SBSecretValue = ConvertTo-SecureString -String $topic$topicSuffix -AsPlainText -Force

    #Creates a new secret in Azure Key Vault
    Write-Host "Setting secret in key vault..."
    Write-Host
    $SBStorageSecret = Set-AzureKeyVaultSecret -VaultName $kvName -Name 'sbconnstrsecret' -SecretValue $SBSecretValue -Verbose
    $kvSBSecValue = $SBStorageSecret.Id

    Add-Content -Path $File  $topicString 
    #dotnet user-secrets set $quote$topicPrefix$quote $topic$topicSuffix --project $finalDirectory

}
catch
{
    write-host "Exception capturing Service Bus secrets:" -ForegroundColor Red
    write-host "Exception Type: $($_.Exception.GetType().FullName)" -ForegroundColor Red
    write-host "Exception Message: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Error with $topicString" -ForegroundColor Red
    Exit
}

#Add-Content -Path $File  " " 

if ($WriteToScreen -eq "True") 
{
    Write-Host "//Azure Service Bus Topic Connection String"
    Write-Host $topicString
    Write-Host 
}


#Build CosmosDB settings
try
{

    #Get Cosmos connection information
    # Get the list of keys for the CosmosDB database
    $myKeys = Invoke-AzureRmResourceAction -Action listKeys `
        -ResourceType "Microsoft.DocumentDb/databaseAccounts" `
        -ApiVersion "2016-03-31" `
        -ResourceGroupName $ResourceGroupName `
        -Name $DBName -Force
  
    # pull out the primary key
    $primaryKey = $myKeys.primaryMasterKey;

    # Get the CosmosDB connection URI
    $cosmosUriHeader="CosmosEndpoint"
    $cosmosUriString="$appName-cosmosdb.documents.azure.com:443"
    $cosmosUriWithHttps="$https$appName-cosmosdb.documents.azure.com:443"
    #$cosmosUri="$quote$cosmosUriHeader$quote$colon$quote$https$cosmosUriString$quote$comma"

    #Add-Content -Path $File  $cosmosUri 

    #store the Cosmos endpoint in Azure Key Vault
         #Converts the name to a secure string
    $CosmosEndpointAsSecret = ConvertTo-SecureString -String $cosmosUriWithHttps -AsPlainText -Force

         #Creates a new secret in Azure Key Vault
         Write-Host "Setting storage account name in key vault..."
         Write-Host
    $CosmosUri = Set-AzureKeyVaultSecret -VaultName $kvName -Name 'cosmosendpoint' -SecretValue $CosmosEndpointAsSecret -Verbose

    #fix
    ##dotnet user-secrets set $quote$cosmosUriHeader$quote $cosmosUriWithHttps --project $finalDirectory
}
catch
{
    write-host "Exception capturing CosmosDB keys:" -ForegroundColor Red
    write-host "Exception Type: $($_.Exception.GetType().FullName)" -ForegroundColor Red
    write-host "Exception Message: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Error with $cosmosUri" -ForegroundColor Red
    Exit
}
#Add-Content -Path $File  " " 

if ($WriteToScreen -eq "True") 
{
    Write-Host "//Azure Cosmos DB Connection URI"
    Write-Host $cosmosUri 
    Write-Host 
}

#Build CosmosDB key
try
{
    $cosmosEndpoint="Cosmos Endpoint: " + $cosmosUri
    # Get the CosmosDB Primay Key
    $cosmosKeyHeader="CosmosPrimaryKey"
    $cosmosKey="$quote$cosmosKeyHeader$quote$colon$quote$primaryKey$quote$comma"
    $cosmosKeyUserSecrets="$primaryKey"

    #****************************************************
    #VetMod-7-12-WriteToUserSecret
    #if ($WriteSecretsToUserSecrets -eq "true") {
    #    dotnet user-secrets set "cosmoskeysecret" $cosmosKeyUserSecrets --project $finalDirectory    
	#}
    #****************************************************

    #Converts the key to a secure string
    $CosmosSecretValue = ConvertTo-SecureString -String $primaryKey -AsPlainText -Force

    #Creates a new secret in Azure Key Vault
    Write-Host "Setting secret in key vault..."
    Write-Host
    $CosmosStorageSecret = Set-AzureKeyVaultSecret -VaultName $kvName -Name 'cosmoskeysecret' -SecretValue $CosmosSecretValue -Verbose
    $kvCosmosSecValue = $CosmosStorageSecret.Id

    Add-Content -Path $File  "//----------------------------------------------"  
    Add-Content -Path $File  "// CosmosDB Info                                "
    Add-Content -Path $File  "//----------------------------------------------"  
    Add-Content -Path $File  $cosmosEndpoint
    Add-Content -Path $File  $cosmosKey 
 
}
catch
{
    write-host "Exception capturing CosmosDB key:" -ForegroundColor Red
    write-host "Exception Type: $($_.Exception.GetType().FullName)" -ForegroundColor Red
    write-host "Exception Message: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Error with $cosmosKey" -ForegroundColor Red
    Exit
}


if ($WriteToScreen -eq "True") 
{
    Write-Host "//Azure Cosmos DB Primary Key"
    Write-Host $cosmosUricosmosKey 
    Write-Host 
}

#Build Application Insights
try
{
    $appInsightsInfo = Get-AzureRmApplicationInsights -ResourceGroupName $ResourceGroupName -Name ($appName + "-appinsights")
    $instrumKey = $appInsightsInfo.InstrumentationKey
    #****************************************************
    #VetMod-7-12-WriteToUserSecret
    #if ($WriteSecretsToUserSecrets -eq "true") {
    #    dotnet user-secrets set "aiinstrumkeysecret" $instrumKey --project $finalDirectory
	#}
    #****************************************************

    #****************************************************
    #VetMod-7-12-WriteToUserSecret
    #if ($WriteSecretsToUserSecrets -eq "true") {
    #    dotnet user-secrets set "aiinstrumkeysecret" $instrumKey --project $finalDirectory
	#}
    #****************************************************

    #store the instrumentation key in Azure Key Vault
    #Converts the key to a secure string
    $AISecretValue = ConvertTo-SecureString -String $instrumKey -AsPlainText -Force

    #Creates a new secret in Azure Key Vault
    Write-Host "Setting AI secret in key vault..."
    Write-Host
    $AIStorageSecret = Set-AzureKeyVaultSecret -VaultName $kvName -Name 'aiinstrumkeysecret' -SecretValue $AISecretValue -Verbose
    $kvAISecValue = $AIStorageSecret.Id

     Add-Content -Path $File  $AISecretValue

     Add-Content -Path $File  "//----------------------------------------------"  
     Add-Content -Path $File  "// App Insights instrumentation key             "
     Add-Content -Path $File  "//----------------------------------------------"  
     Add-Content -Path $File  $instrumKey 

}
catch
{
    write-host "Exception capturing AI instrumentation key:" -ForegroundColor Red
    write-host "Exception Type: $($_.Exception.GetType().FullName)" -ForegroundColor Red
    write-host "Exception Message: $($_.Exception.Message)" -ForegroundColor Red
    Exit
}

#Get ACR Resgisty Name
$acrRegistryPrefix="ContainerRegistryName"
$acrRegistryName=$appName+"acr"
$acrRegistry="$quote$acrRegistryPrefix$quote$colon$quote$acrRegistryName$quote$comma"
$acrRegistryNameForUserSecrets="$acrRegistryName"

Add-Content -Path $File  "//----------------------------------------------"  
Add-Content -Path $File  "// ACR Info                                     "
Add-Content -Path $File  "//----------------------------------------------"  

Add-Content -Path $File  $acrRegistry 
#dotnet user-secrets set $quote$acrRegistryPrefix$quote $acrRegistryNameForUserSecrets --project $finalDirectory
#Add-Content -Path $File  " " 

if ($WriteToScreen -eq "True") 
{
    Write-Host "//Azure Container Registry Name"
    Write-Host $acrRegistry 
    Write-Host 
}

#Get ACR Resgisty Login Server
$acrServerPrefix="ContainerLoginServer"
$acrServerName=$appName+"acr.azurecr.io"
$acrServer="$quote$acrServerPrefix$quote$colon$quote$acrServerName$quote"


Add-Content -Path $File  $acrServer 
#dotnet user-secrets set $quote$acrServerPrefix$quote $quote$acrServerName$quote --project $finalDirectory

if ($WriteToScreen -eq "True") 
{
    Write-Host "//Azure Container Registry Login Server"
    Write-Host $acrServer 
}

# Clean up
Write-Host "Logging out of Azure"
Logout-AzureRmAccount
#az logout

#**************************************
# End Processing
#**************************************
Add-Content -Path $File  '}' 

Write-Host  
Write-Host "**********************************************************************************************" 
Write-Host "Done processing " 
Write-Host "**********************************************************************************************" 
Write-Host  "Important Note:" 
Write-Host  "We have also written a backup in c:\microservices_workshop\usersecrets.txt" 
Write-Host "**********************************************************************************************" 

# Notes for modificaiton to optionally write secrets to .net user secrets
#Line 330- storagekeysecret
#Line 380- sqldbpwsecret
#Line 396- catalogdbsecret
#Line 458- orderdbsecret
#Line 515- redisconnstrsecret
#Line 559- sbconnstrsecret
#Line 650- cosmoskeysecret
#Line 702- aiinstrumkeysecret