param name string
param suffix string
param location string = resourceGroup().location
@allowed([
    'dotnet'
    'dotnet-isolated'
])
param workerRuntime string

var shortname = '${name}${suffix}'
var longname = '${name}-${suffix}'

module st './storageAccount.bicep' = {
    name: 'StorageAccount_${suffix}'
    params: {
        name: shortname
        location: location
    }
}

module wrkspc './logAnalyticsWorkspace.bicep' = {
    name: 'LogAnalyticsWorkspace_${suffix}'
    params: {
        name: longname
        location: location
    }
}

module appins './appInsights.bicep' = {
    name: 'ApplicationInsights_${suffix}'
    params: {
        name: longname
        location: location
        workspaceId: wrkspc.outputs.id
    }
}

module csplan './consumptionPlan.bicep' = {
    name: 'ConsumptionPlan_${suffix}'
    params: {
        name: longname
        location: location
    }
}

module fncapp './functionApp.bicep' = {
    name: 'FunctionApp_${suffix}'
    params: {
        name: longname
        location: location
        storageAccountConnectionString: st.outputs.connectionString
        appInsightsInstrumentationKey: appins.outputs.instrumentationKey
        appInsightsConnectionString: appins.outputs.connectionString
        consumptionPlanId: csplan.outputs.id
        workerRuntime: workerRuntime
    }
}
