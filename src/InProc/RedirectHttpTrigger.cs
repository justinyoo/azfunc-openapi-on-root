using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;

namespace InProc
{
    public class RedirectHttpTrigger
    {
        private readonly ILogger<RedirectHttpTrigger> _logger;

        public RedirectHttpTrigger(ILogger<RedirectHttpTrigger> logger)
        {
            this._logger = logger.ThrowIfNullOrDefault();
        }

        [FunctionName(nameof(RedirectHttpTrigger.Redirect))]
        [OpenApiIgnore()]
        public async Task<IActionResult> Redirect(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "/")] HttpRequest req)
        {
            this._logger.LogInformation("C# HTTP trigger function processed a request.");

            var result = new RedirectResult("/swagger/ui", permanent: true);

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
