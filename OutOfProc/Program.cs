using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var host = new HostBuilder()
           .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
           .ConfigureOpenApi()
            .ConfigureServices(services =>
            {
                services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
                        {
                            var options = new DefaultOpenApiConfigurationOptions()
                            {
                                Info = new OpenApiInfo()
                                {
                                    Version =  "1.0.0",
                                    Title = "Swagger UI on Root (out-of-proc)",
                                    Description = "This is a sample Azure Functions app showing the Swagger UI page on root.",
                                    TermsOfService = new Uri("https://github.com/Azure/azure-functions-openapi-extension"),
                                    Contact = new OpenApiContact()
                                    {
                                        Name = "Enquiry",
                                        Email = "azfunc-openapi@microsoft.com",
                                        Url = new Uri("https://github.com/Azure/azure-functions-openapi-extension/issues"),
                                    },
                                    License = new OpenApiLicense()
                                    {
                                        Name = "MIT",
                                        Url = new Uri("http://opensource.org/licenses/MIT"),
                                    }
                                },
                                OpenApiVersion = OpenApiVersionType.V3,
                            };

                            var codespaces = bool.TryParse(Environment.GetEnvironmentVariable("OpenApi__RunOnCodespaces"), out var isCodespaces) && isCodespaces;
                            if (codespaces)
                            {
                                options.IncludeRequestingHostName = false;
                            }

                            return options;
                        })
                        ;
            })
           .Build();

host.Run();
