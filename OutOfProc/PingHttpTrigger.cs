using System.Reflection.Metadata;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace OutOfProc
{
    public class PingHttpTrigger
    {
        private readonly ILogger _logger;

        public PingHttpTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PingHttpTrigger>();
        }

        [Function(nameof(PingHttpTrigger.Ping))]
        [OpenApiOperation(operationId: "Ping", tags: new[] { "ping" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public HttpResponseData Ping([HttpTrigger(AuthorizationLevel.Anonymous, "GET")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
