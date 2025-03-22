@description('The location')
param location string

@description('The prefix for the resources')
param resourcesPrefix string

@description('The name of the application insight tied to the front end')
param applicationInsightName string

@description('The name of the Key Vault servuce used for the secret of the front end')
param keyVaultName string

var functionAppName = toLower('${resourcesPrefix}-be')
var functionAppPlanName = toLower('${resourcesPrefix}-be-plan')
var functionAppStorageAccountName = toLower('${resourcesPrefix}bestore')

resource applicationInsight 'Microsoft.Insights/components@2020-02-02' existing = {
  name: applicationInsightName
}

resource keyVault 'Microsoft.KeyVault/vaults@2021-11-01-preview' existing = {
  name: keyVaultName
}

resource functionAppStorageAccount 'Microsoft.Storage/storageAccounts@2021-08-01' = {
  name: functionAppStorageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
}

resource backEndAppServicePlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: functionAppPlanName
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  properties: {}
}

resource functionApp 'Microsoft.Web/sites@2021-03-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: backEndAppServicePlan.id
    siteConfig: {
      ftpsState: 'FtpsOnly'
      minTlsVersion: '1.2'
    }
    httpsOnly: true
  }
}

resource funcAppKeyVaultAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid('Key Vault Secret User', functionAppName, subscription().subscriptionId)
  scope: keyVault
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '4633458b-17de-408a-b874-0445c86b69e6') // this is the role "Key Vault Secrets User"
    principalId: functionApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource appSettings 'Microsoft.Web/sites/config@2022-03-01' = {
  name: 'appsettings'
  parent: functionApp
  properties: {
    APPINSIGHTS_INSTRUMENTATIONKEY: applicationInsight.properties.InstrumentationKey
    AzureWebJobsStorage: 'DefaultEndpointsProtocol=https;AccountName=${functionAppStorageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${functionAppStorageAccount.listKeys().keys[0].value}'
    WEBSITE_CONTENTAZUREFILECONNECTIONSTRING: 'DefaultEndpointsProtocol=https;AccountName=${functionAppStorageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${functionAppStorageAccount.listKeys().keys[0].value}'
    WEBSITE_CONTENTSHARE: toLower(functionAppName)
    FUNCTIONS_EXTENSION_VERSION: '~4'
    WEBSITE_NODE_DEFAULT_VERSION: '~10'
    FUNCTIONS_WORKER_RUNTIME: 'dotnet-isolated'
    ServiceBusConnectionString: ''
    ServiceBusTopicName: 'logistictopic'
    ServiceBusSubscriptionName: 'BackEndSub'
  }
}

resource functionAppKeyVayultKey 'Microsoft.KeyVault/vaults/secrets@2022-07-01' = {
  name: 'ProductApiKey'
  parent: keyVault
  properties: {
    attributes: {
      enabled: true
    }
    value: listKeys('${functionApp.id}/host/default', functionApp.apiVersion).functionKeys.default
  }
}

output backEndName string = functionApp.name
