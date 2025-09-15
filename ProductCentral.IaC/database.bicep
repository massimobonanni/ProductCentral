@description('The location')
param location string

@description('The prefix for the resources')
param resourcesPrefix string

@description('The name of the Key Vault service used for the secret of the front end')
param keyVaultName string

var cosmosDbName  = toLower('${resourcesPrefix}-cosmosdb')

resource keyVault 'Microsoft.KeyVault/vaults@2021-11-01-preview' existing = {
  name: keyVaultName
}

//create cosmos db account
resource cosmosDbAccount 'Microsoft.DocumentDB/databaseAccounts@2021-04-15' = {
  name: cosmosDbName
  location: location
  kind: 'GlobalDocumentDB'
  properties: {
    databaseAccountOfferType: 'Standard'
    locations: [
      {
        locationName: location
        failoverPriority: 0
        isZoneRedundant: false
      }
    ]
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
    capabilities: [
      {
        name: 'EnableServerless'
      }
    ]
    enableFreeTier: true
  }
}

// create a sql database in the cosmos db account
resource cosmosDbSqlDatabase 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2021-04-15' = {
  parent: cosmosDbAccount
  name: 'productcentraldb'
  properties: {
    resource: {
      id: 'productcentraldb'
    }
    options: {}
  }
}

// create a container in the sql database
resource cosmosDbSqlContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2021-04-15' = {
  parent: cosmosDbSqlDatabase
  name: 'products'
  properties: {
    resource: {
      id: 'products'
      partitionKey: {
        paths: [
          '/id'
        ]
        kind: 'Hash'
      }
      defaultTtl: -1
    }
    options: {}
  }
}

// create a keyvault secret with the connection string of cosmosd db database
resource databaseConnectionStringKeyVaultKey 'Microsoft.KeyVault/vaults/secrets@2022-07-01' = {
  name: 'CosmosDbConnectionString'
  parent: keyVault
  properties: {
    attributes: {
      enabled: true
    }
    value: cosmosDbAccount.listConnectionStrings().connectionStrings[0].connectionString
  }
}

output cosmosDbName string = cosmosDbAccount.name
