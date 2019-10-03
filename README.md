# CosmosDBAutoScale - Simple autoscaler Azure Function for Azure Cosmos DB

This is a simple Azure Function to scale up Azure Cosmos DB container throughput (Request Unit per second).

You can add an alert rule to monitor RU/s or number of 429 (throttled) requests and send a HTTP requeest to this Azure Function using "webhook" or "Azure Function" action type in your action group.

## Configurations

Add these configurations to "local.settings.json" file for local exeution in Visual Studio.

```
{
  "IsEncrypted": false,
  "Values": {
    ...
    "CosmosDB_Uri": "https://<account>.documents.azure.com:443/",
    "CosmosDB_appKey": "<key>",
    "CosmosDB_DatabaseId": "<database>",
    "CosmosDB_ContainerId": "<container>",
    "CosmosDB_RU": 100
  }
}
```

Add these configurations to the "Application settings" in the Function App.

## Other Cosmos DB Auto Scalers:

* https://github.com/NoOps-jp/azure-cosmosdb-scaler
* https://github.com/giorgited/CosmosScale
* https://github.com/GaryStrange/CosmosDBAutoScaling
