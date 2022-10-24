# Azure Functions OpenAPI on Root #

This provides sample codes to show Swagger UI on root (`/`), instead of `/api/swagger/ui`.


## Getting Started ##

There are two function apps available &ndash; one for the in-proc worker and the other for the out-of-proc worker. Take a look at your preferred function app depending on your situation.

To use the root page as Swagger UI, follow the steps below. Note that there's no way to use HTTP URL rewrite feature as of today.


### `host.json` ###

Update your `host.json` like below:

```jsonc
{
  "version": "2.0",
  ...
  "extensions": {
    "http": {
      // Remove the default route prefix from 'api' to ''
      "routePrefix": ""
    }
  }
}
```


### `local.settings.json` or app settings on Azure ###

Add the following app settings key:

```jsonc
{
  "IsEncrypted": false,
  "Values": {
    ...
    // Disable the homepage
    "AzureWebJobsDisableHomepage": true,
    ...
  }
}
```


## Redirect HTTP Trigger ###

Add a new HTTP trigger for URL redirection.

```csharp
// In-Proc function
public class RedirectHttpTrigger
{
    // Add this HTTP trigger for URL redirection
    [FunctionName(nameof(RedirectHttpTrigger.Redirect))]
    [OpenApiIgnore()]
    public async Task<IActionResult> Redirect(
        // Make sure the route MUST be "/"
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "/")] HttpRequest req)
    {
        this._logger.LogInformation("C# HTTP trigger function processed a request.");

        // Make sure the redirection URL MUST be "/swagger/ui"
        var result = new RedirectResult("/swagger/ui", permanent: true);

        return await Task.FromResult(result).ConfigureAwait(false);
    }
}
```

```csharp
// Out-of-Proc function
public class RedirectHttpTrigger
{
    // Add this HTTP trigger for URL redirection
    [Function(nameof(RedirectHttpTrigger.Redirect))]
    [OpenApiIgnore()]
    public HttpResponseData Redirect(
        // Make sure the route MUST be "/"
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "/")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        // Make sure to put the HTTP status code of 301 (move permanently)
        var response = req.CreateResponse(HttpStatusCode.MovedPermanently);
    
        // Make sure to put the "Location" header with the redirection URL of "/swagger/ui"
        response.Headers.Add("Location", "/swagger/ui");

        return response;
    }
}
```


## Running on GitHub Codespaces ##

If you want to run this sample app on your GitHub Codespaces, please run the script first:

```bash
# On bash shell
pwsh -c "Invoke-RestMethod https://aka.ms/azfunc-openapi/add-codespaces.ps1 | Invoke-Expression"
```

```powershell
# On PowerShell
Invoke-RestMethod https://aka.ms/azfunc-openapi/add-codespaces.ps1 | Invoke-Expression
```

This script will update your `local.settings.json` file to accommodate your GitHub Codespaces to work as a local running environment.


## Deploy to Azure ##

To deploy both Azure Functions app, you can use [Azure Developer CLI](https://learn.microsoft.com/azure/developer/azure-developer-cli/overview?WT.mc_id=dotnet-79949-juyoo) by running those two commands:

```bash
# Initialise environment
azd init

# Provision and deploy apps
azd up
```
