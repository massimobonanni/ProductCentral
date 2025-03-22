@description('The location')
param location string

@description('The prefix for the resources')
param resourcesPrefix string

@description('The name of the application insight tied to the front end')
param applicationInsightName string

@description('The name of the Key Vault servuce used for the secret of the front end')
param keyVaultName string

@description('The name of the Function App hosted the back end')
param backEndName string

var appServiceName = toLower('${resourcesPrefix}-fe')
var appServicePlanName = toLower('${resourcesPrefix}-fe-plan')

resource applicationInsight 'Microsoft.Insights/components@2020-02-02' existing = {
  name: applicationInsightName
}

resource keyVault 'Microsoft.KeyVault/vaults@2021-11-01-preview' existing = {
  name: keyVaultName
}

resource functionApp 'Microsoft.Web/sites@2021-03-01' existing = {
  name: backEndName
}

resource frontEndAppServicePlan 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: appServicePlanName
  location: location
  properties: {
    reserved: false
  }
  sku: {
    name:'F1'
  }
  kind: 'windows'
}

resource frontEndAppService 'Microsoft.Web/sites@2021-02-01' = {
  name: appServiceName
  location: location
  kind: 'app'
  properties: {
    httpsOnly: true
    serverFarmId: frontEndAppServicePlan.id
    siteConfig: {
      netFrameworkVersion: 'v8.0'
    }
  }
  identity: {
    type: 'SystemAssigned'
  }
}

resource basicPublishingCredentials 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2023-12-01' = {
  parent: frontEndAppService
  name: 'scm'
  properties: {
    allow: true
  }
}

resource appSettings 'Microsoft.Web/sites/config@2022-03-01' = {
  name: 'appsettings'
  parent: frontEndAppService
  properties: {
    APPINSIGHTS_INSTRUMENTATIONKEY: applicationInsight.properties.InstrumentationKey
    APPINSIGHTS_CONNECTION_STRING: 'InstrumentationKey=${applicationInsight.properties.InstrumentationKey}'
    ProductApiUrl: functionApp.properties.defaultHostName
    ProductApiKey: '@Microsoft.KeyVault(SecretUri=${keyVault.properties.vaultUri}secrets/ProductApiKey)'
  }
}

resource appServiceKeyVaultAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid('Key Vault Secret User', appServiceName, subscription().subscriptionId)
  scope: keyVault
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '4633458b-17de-408a-b874-0445c86b69e6') // this is the role "Key Vault Secrets User"
    principalId: frontEndAppService.identity.principalId
    principalType: 'ServicePrincipal'
  }
}
