using System.Reflection.Metadata;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace OutOfProc
{
    public class RedirectHttpTrigger
    {
        private readonly ILogger _logger;

        public RedirectHttpTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RedirectHttpTrigger>();
        }

        [Function(nameof(RedirectHttpTrigger.Redirect))]
        [OpenApiIgnore()]
        public HttpResponseData Redirect([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "/")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.MovedPermanently);
            response.Headers.Add("Location", "/swagger/ui");

            return response;
        }
    }
}
