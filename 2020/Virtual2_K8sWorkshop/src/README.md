# The K8s Workshop

## Index

* [Build the applications](#build-the-applications)
* [Deploy Azure Kubernetes Service](#deploy-azure-kubernetes-service)
* [Deploy Bridge to Kubernetes Sample](#deploy-bridge-to-kubernetes-sample)
* [Using Bridge to Kubernetes without isolation](#using-bridge-to-kubernetes-without-isolation)
* [Using Bridge to Kubernetes with isolation](#using-bridge-to-kubernetes-with-isolation)
* [Deploy Ingress Controller](#deploy-ingress-controller)
* [Deploy Secrets with Dapr Sample](#deploy-secrets-with-dapr-sample) 

---

## Build the applications

You'll be working with 3 applications:

* **webui** an ASP.NET Core MVC application
* **webapi** an ASP.NET Core Web API application
* **secrets** an ASP.NET Core Web API application

### Prerequisites

* Docker
* Container Registry credentials.

### Steps

1. Login to your Container Registry
1. Open the build.ps1 file and replace the **crName** value with your docker user name or container registry uri.
1. Run build.ps1 

``` powershell
.\build.ps1
```

Your docker images are now built and pushed to your container registry.

---

## Deploy Azure Kubernetes Service

In the **deploy/aks** folder you'll find a Terrafrom script to deploy an Azure Kubernetes Cluster.

### Prerequisites

* Azure CLI
* Terraform
* An Azure Resource Group

> You can use Azure Cloud Shell!!!

### Steps

1. Run the following commands 

``` powershell
cd ./deploy/aks
az login
terraform init
terraform apply -var='resource_group_name=<resource group name>' -var='cluster_name=<aks cluster name>' -var='key_vault_name=<key vault name>'
az aks get-credentials -g <resource group name> -n <aks cluster name>
```

---

## Deploy Bridge to Kubernetes Sample

### Prerequisites

* kubectl

> You can use Azure Cloud Shell!!!

### Steps

1. From the **src** folder, run the following command to install the **webui** and **webapi** applications and k8s services.

``` powershell
kubectl apply -f ./deploy/bridge2kubernetes -n default
```

2. Check your pods and services running:

``` powershell
kubectl get po
kubectl get services
```

---

## Using Bridge to Kubernetes without isolation

### Prerequisites

* Visual Studio Code
* Bridge to Kubernetes plugin

### Steps

1. Open the **webapi** folder in Visual Studio Code
2. In the Command Pallete run **Bridge to Kubernetes: Configure**
3. Select **privacywebapi**
4. Write **80** as the local port
5. Select **.NET Core Launch (web)**
6. Select **No**
7. Add a breakpoint in the `PrivacyController`
8. Start Debugging. Be sure to select **.NET Core Launch (web) with Kubernetes**
9. To get the api of the **webui** application run:

``` powershell
kubectl get services --field-selector metadata.name=webui
```

10. Navigate to the service and click on the **Privacy** tab.

---

## Using Bridge to Kubernetes with isolation

### Prerequisites

* Visual Studio Code
* Bridge to Kubernetes plugin

### Steps

1. Open the **webapi** folder in Visual Studio Code
2. In the Command Pallete run **Bridge to Kubernetes: Configure**
3. Select **privacywebapi**
4. Write **80** as the local port
5. Select **.NET Core Launch (web)**
6. Select **Yes**
7. Add a breakpoint in the **PrivacyController**
8. Start Debugging. Be sure to select **.NET Core Launch (web) with Kubernetes**
9. To get the api of the **webui** application run:

``` powershell
kubectl get services --field-selector metadata.name=webui
```

10. Navigate to the service and click on the **Privacy** tab. Note: this time the breakpoint was not hit.
11. Open tasks.json and copy the **isolateAs** value from the **bridge-to-kubernetes.service** task.
12. Run the follwing command:

```powershell
curl -H "kubernetes-route-as: <isolateAs value>" http://<webui service ip>/Home/Privacy
```

---

## Deploy Ingress Controller

### Prerequisites

* Helm 3
* openssl 

### Steps

1. Run the following commands:

```powershell
helm repo add nginx-stable https://helm.nginx.com/stable
helm repo update
kubectl create namespace ingress
helm install basic-ingress --namespace ingress nginx-stable/nginx-ingress
```

2. Get the nginx service ip

``` powershell
kubectl get services --field-selector metadata.name=basic-ingress-nginx-ingress -n ingress
```

3. Update the Public IP of the nginx service with a DNS name 

``` powershell
$ipid=(az network public-ip list --query "[?ipAddress=='<nginx service public ip>'].id | [0]")
az network public-ip update --ids $ipid --dns-name <DNS name>
```

4. Update the host in the ingress/ingress.yaml file.

5. Create a certificate

``` powershell
openssl req -x509 -sha256 -nodes -days 365 -newkey rsa:2048 -keyout tls.key -out tls.crt -subj "/CN=nginxsvc/O=nginxsvc"
```

6. Deploy the certificate to your K8s cluster

``` powershell
kubectl create secret tls tls-secret --key tls.key --cert tls.crt -n default
```

7. Deploy the ingress rule for the webui application

``` powershell
kubectl apply -f ./deploy/ingress/ingress.yaml -n default
```

8. Browse to the application using the FQDN.

---

## Deploy Secrets with Dapr Sample

### Steps

1. Add a secret with name **netcoreconf** in the key vault created in the ... section
2. Add your Key Vault Name and the Managed Identity Client ID to the **./deploy/secrets/components/azure.keyvault.yaml** file.
3. From the **src** folder, run the following command to install the **secrets** application and required Dapr components:

``` powershell
kubectl create namespace dapr-test
kubectl apply -f ./deploy/secrets/components -n dapr-test
kubectl apply -f ./deploy/secrets/deployment.yaml -n dapr-test
```

4. Get the secrets app public ip

``` powershell
kubectl get services --field-selector metadata.name=secrets -n dapr-test
```

5. Retrieve a secret from the key vault

``` powershell
curl http://<public ip of the secrets service>/secrets
```

6. Retrieve a secret from kubernetes

``` powershell
curl http://<public ip of the secrets service>/secrets/k8s
```

---