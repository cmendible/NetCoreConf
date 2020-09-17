param rgName string
param rgLocation string
param keyVaultName string
param objectId string

resource rg 'Microsoft.Resources/resourceGroups@2018-05-01' = {
    name: rgName
    location: rgLocation
}

resource deployment 'Microsoft.Resources/deployments@2018-05-01' = {
    name: 'kayVaultDeployment'
    location: rgLocation
    dependsOn: [
        rg
    ]
    properties: {
        mode: 'Incremental'
        template: {
            resources: [
                kv
            ]
        }
    }
}

resource kv 'Microsoft.KeyVault/vaults@2019-09-01' = {
  name: keyVaultName
  location: rgLocation
  properties: {
    tenantId: subscription().tenantId
    accessPolicies: [
        {
            objectId: objectId
            tenantId: subscription().tenantId
            permissions: {
                secrets: [ 
                    'list' 
                    'get'
                ]
            }
        }
    ]        
    sku: {
        name: 'Standard'
        family: 'A'
    }
    networkAcls: {
        defaultAction: 'Allow'
        bypass: 'AzureServices'
    }
  }
}