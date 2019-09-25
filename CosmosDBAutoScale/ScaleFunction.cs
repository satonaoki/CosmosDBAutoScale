using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Cosmos;

namespace CosmosDBAutoScale
{
    public static class ScaleFunction
    {
        [FunctionName("ScaleFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function 'ScaleFunction' processed a request.");

            try
            {
                var config = new ConfigurationBuilder().SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                string endpoint = config["CosmosDB_Uri"];
                if (string.IsNullOrEmpty(endpoint))
                {
                    throw new ArgumentNullException("Please specify a valid endpoint in the local.settings.json");
                }

                string authKey = config["CosmosDB_appKey"];
                if (string.IsNullOrEmpty(authKey))
                {
                    throw new ArgumentException("Please specify a valid key in the local.settings.json");
                }

                string databaseId = config["CosmosDB_DatabaseId"];
                if (string.IsNullOrEmpty(databaseId))
                {
                    throw new ArgumentException("Please specify a database id in the local.settings.json");
                }

                string containerId = config["CosmosDB_ContainerId"];
                if (string.IsNullOrEmpty(containerId))
                {
                    throw new ArgumentException("Please specify a container id in the local.settings.json");
                }

                int incrementalRU = 0;
                try
                {
                    incrementalRU = int.Parse(config["CosmosDB_RU"]);                  
                }
                catch (Exception)
                {
                    throw new ArgumentException("Please specify a valid RU in the local.settings.json");
                }

                using (CosmosClient client = new CosmosClient(endpoint, authKey))
                {
                    var container = client.GetContainer(databaseId, containerId);
                    var currentRU = await container.ReadThroughputAsync();
                    log.LogInformation(string.Format("Current provisioned throughput is: {0} RU/s", 
                        currentRU.ToString()));

                    int newRU = currentRU.GetValueOrDefault() + incrementalRU;
                    var response = await container.ReplaceThroughputAsync(newRU);
                    log.LogInformation(string.Format("New provisioned througput: {0} RU/s", newRU.ToString()));

                    return new OkObjectResult("The collection's throughput was changed...");
                }
            }
            catch (Exception e)
            {
                log.LogInformation(e.Message);
                return new BadRequestObjectResult("ERROR: The container's throughput was not changed...");
            }
        }
    }
}
