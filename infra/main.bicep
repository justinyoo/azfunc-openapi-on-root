targetScope = 'subscription'

param name string
param location string = 'koreacentral'

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
    name: 'rg-${name}'
    location: location
}

module fncapp_ip './provision-functionApp.bicep' = {
    name: 'FunctionApp_InProc'
    scope: rg
    params: {
        name: name
        suffix: 'in-proc'
        location: location
        workerRuntime: 'dotnet'
    }
}

module fncapp_oop './provision-functionApp.bicep' = {
    name: 'FunctionApp_OutPfProc'
    scope: rg
    params: {
        name: name
        suffix: 'out-of-proc'
        location: location
        workerRuntime: 'dotnet-isolated'
    }
}
