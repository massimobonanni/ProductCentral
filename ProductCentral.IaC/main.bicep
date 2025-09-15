targetScope = 'subscription'

@description('The prefix of the resource group name that contains all the resources')
param resourceGroupNamePrefix string = 'ProductManager'

@description('The primary location of the resources')
param location string = deployment().location

@description('The prefix for the resources')
param resourcesPrefix string = 'PM${uniqueString(subscription().id,resourceGroupNamePrefix)}'

var resourceGroupName = '${resourceGroupNamePrefix}-rg'

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: resourceGroupName
  location: location
}

module appInsight 'applicationInsight.bicep' = {
  scope: resourceGroup
  name: 'appInsight'
  params: {
    location: location
    resourcesPrefix: resourcesPrefix
  }
}

module keyVault 'keyVault.bicep' = {
  scope: resourceGroup
  name: 'keyVault'
  params: {
    location: location
    resourcesPrefix: resourcesPrefix
  }
}

module frontEnd 'frontEnd.bicep' = {
  scope: resourceGroup
  name: 'primaryFrontEnd'
  params: {
    location: location
    resourcesPrefix: resourceGroupName
    applicationInsightName: appInsight.outputs.appInsightName
    keyVaultName: keyVault.outputs.keyVaultName
    backEndName: backEnd.outputs.backEndName
  }
}

module backEnd 'backEnd.bicep' = {
  scope: resourceGroup
  name: 'backEnd'
  params: {
    location: location
    resourcesPrefix: resourcesPrefix
    applicationInsightName: appInsight.outputs.appInsightName
    keyVaultName: 'ProductCentralKeyVault'
  }
}

module database 'database.bicep' = {
  scope: resourceGroup
  name: 'database'
  params: {
    location: location
    resourcesPrefix: resourcesPrefix
    keyVaultName: keyVault.outputs.keyVaultName
  }
}
