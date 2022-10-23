using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace InProc
{
    public class PingHttpTrigger
    {
        private readonly ILogger<PingHttpTrigger> _logger;

        public PingHttpTrigger(ILogger<PingHttpTrigger> logger)
        {
            this._logger = logger.ThrowIfNullOrDefault();
        }

        [FunctionName(nameof(PingHttpTrigger.Ping))]
        [OpenApiOperation(operationId: "Ping", tags: new[] { "ping" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The name to say hello to.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Ping(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "ping")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            var result = new OkObjectResult(responseMessage);

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
