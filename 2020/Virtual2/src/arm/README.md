az deployment sub create --name iacwar --location "West Europe" --template-file azuredeploy.json --parameters azuredeploy.parameters.json

az deployment sub what-if --name iacwar --location "West Europe" --template-file azuredeploy.json --parameters azuredeploy.parameters.json

az deployment sub delete --name iacwwar   