# FineApp

This app is fine, not so good, just fine.

## Setup
Run with Azure CLI and git
```
az group create --name jitNetYarpRg --location "North Europe"
az appservice plan create --name yarpAppServicePlan --resource-group jitNetYarpRg --sku B2 --is-linux
git clone https://github.com/JakuSw/FineApp.git
cd FineApp
az webapp create --resource-group jitNetYarpRg --plan yarpAppServicePlan --name Yarp --multicontainer-config-type compose --multicontainer-config-file docker-compose.yml
```

## Clean up
```
az group delete --name jitNetYarpRg
```
