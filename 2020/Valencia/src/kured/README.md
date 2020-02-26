CLUSTER_RESOURCE_GROUP=$(az aks show -g kured -n kured --query nodeResourceGroup -o tsv)
SCALE_SET_NAME=$(az vmss list --resource-group $CLUSTER_RESOURCE_GROUP --query [0].name -o tsv)

az vmss extension set  \
    --resource-group $CLUSTER_RESOURCE_GROUP \
    --vmss-name $SCALE_SET_NAME \
    --name VMAccessForLinux \
    --publisher Microsoft.OSTCExtensions \
    --version 1.4 \
    --protected-settings "{\"username\":\"cmendibl3\", \"ssh_key\":\"$(cat ~/.ssh/id_rsa.pub)\"}"

az vmss update-instances --instance-ids '*' \
	--resource-group $CLUSTER_RESOURCE_GROUP \
	--name $SCALE_SET_NAME

---

kubectl run --generator=run-pod/v1 -it --rm aks-ssh --image=debian

apt-get update && apt-get install openssh-client -y

kubectl cp ~/.ssh/id_rsa $(kubectl get pod -l run=aks-ssh -o jsonpath='{.items[0].metadata.name}'):/id_rsa

chmod 0600 id_rsa

ssh -i id_rsa cmendibl3@10.240.0.4

---

kubectl apply -f kured-1.3.0-dockerhub.yaml
