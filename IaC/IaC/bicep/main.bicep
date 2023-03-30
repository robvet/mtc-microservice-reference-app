param resourceGroupName string = resourceGroup().name
param applicationName string = '${replace((resourceGroupName), '-rg', '')}'
param location string = resourceGroup().location
param sqlServerName string = '${applicationName}-sqldb'
param sqlDatabaseName string = 'catalogservices'
param sqlAdminLogin string = 'micr0tunessqllogin'
param sqlAdminLoginPassword string = 'micr0tunespass@word1' //take(newGuid(), 16)


module sqlServerModule 'modules/sqlserver.bicep' = {
  name: '${deployment().name}--sqlserver'
  params: {
    sqlServerName: sqlServerName
    sqlDatabaseName: sqlDatabaseName
    sqlAdminLogin: sqlAdminLogin
    sqlAdminLoginPassword: sqlAdminLoginPassword
    location: location
  }
}

output resourceGroupName string = '${resourceGroupName}'
output applicationName string = '${applicationName}'
output location string = '${location}'
output sqlServerName string = '${sqlServerName}'

output sqlDatabaseName string = '${sqlDatabaseName}'
output sqlAdminLogin string = '${sqlAdminLogin}'
output sqlAdminLoginPassword string = '${sqlAdminLoginPassword}'




output output array = [
  '${resourceGroupName}'
  '${applicationName}'
  '${location}'
 ]




//param uniqueSeed string = '${subscription().subscriptionId}-${resourceGroup().name}'
//param uniqueSuffix string = 'reddog-${uniqueString(uniqueSeed)}'
//param containerAppsEnvName string = 'cae-${uniqueSuffix}'
//param logAnalyticsWorkspaceName string = 'log-${uniqueSuffix}'
//param appInsightsName string = 'appi-${uniqueSuffix}'
//param serviceBusNamespaceName string = 'sb-${uniqueSuffix}'
//param redisName string = 'redis-${uniqueSuffix}'
//param cosmosAccountName string = 'cosmos-${uniqueSuffix}'
//param cosmosDatabaseName string = 'reddog'
//param cosmosCollectionName string = 'loyalty'
//param storageAccountName string = 'st${replace(uniqueSuffix, '-', '')}'
//param blobContainerName string = 'receipts'


