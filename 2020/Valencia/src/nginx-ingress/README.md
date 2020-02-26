kubectl create namespace ingress
helm repo add stable https://kubernetes-charts.storage.googleapis.com/
helm install nginx-ingress stable/nginx-ingress --namespace ingress -f ./ingress-properties.yaml
helm uninstall nginx-ingress

