param name string
param location string = resourceGroup().location

@secure()
param storageAccountConnectionString string
param consumptionPlanId string
@secure()
param appInsightsInstrumentationKey string
@secure()
param appInsightsConnectionString string

@allowed([
    'dotnet'
    'dotnet-isolated'
])
param workerRuntime string

var storage = {
    connectionString: storageAccountConnectionString
}

var consumption = {
    id: consumptionPlanId
}

var appInsights = {
    instrumentationKey: appInsightsInstrumentationKey
    connectionString: appInsightsConnectionString
}

var functionApp = {
    name: 'fncapp-${name}'
    location: location
    workerRuntime: workerRuntime
}

resource fncapp 'Microsoft.Web/sites@2022-03-01' = {
    name: functionApp.name
    location: functionApp.location
    kind: 'functionapp'
    properties: {
        serverFarmId: consumption.id
        httpsOnly: true
        siteConfig: {
            appSettings: [
                // Common Settings
                {
                    name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
                    value: appInsights.instrumentationKey
                }
                {
                    name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
                    value: appInsights.connectionString
                }
                {
                    name: 'AzureWebJobsStorage'
                    value: storage.connectionString
                }
                {
                    name: 'FUNCTION_APP_EDIT_MODE'
                    value: 'readonly'
                }
                {
                    name: 'FUNCTIONS_EXTENSION_VERSION'
                    value: '~4'
                }
                {
                    name: 'FUNCTIONS_WORKER_RUNTIME'
                    value: functionApp.workerRuntime
                }
                {
                    name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
                    value: storage.connectionString
                }
                {
                    name: 'WEBSITE_CONTENTSHARE'
                    value: functionApp.name
                }
                {
                    name: 'AzureWebJobsDisableHomepage'
                    value: 'true'
                }
            ]
        }
    }
}

output id string = fncapp.id
output name string = fncapp.name
