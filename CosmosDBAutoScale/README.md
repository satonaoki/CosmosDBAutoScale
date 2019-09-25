
# local.settings.json

Add these configurations to "local.settings.json" file for local exeution in Visual Studio.

'''
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
'''
