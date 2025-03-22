# Product Central - IaC

This project contains the bicep templates you can use to create the environment to host the Product Central project.

To create the resource group and to deploy the resources you need for the project, simply run the following command:

```
az deployment sub create --location <your region> --template-file main.bicep
```

where 
- `<your region>` is the location where you want to create the deployment


You can also set these parameters:

- `location` : the location you want to deploy (by default the location is the same of your deployment)
- `resourceGroupNamePrefix` : the prefix of the resource group name that contains all the resources. The Resource Group name will be generated composing this prefix with the postfix "-rg". Default value is `ProductCentral`
- `resourcesPrefix` : the prefix used to generate the name of all the resources. The default value is `PM`

```
az deployment sub create --location <your region> --template-file main.bicep --parameters location=<location to deploy> resourceGroupNamePrefix=<rg prefix> resourceGroupNamePrefix=<respurces prefix>
```