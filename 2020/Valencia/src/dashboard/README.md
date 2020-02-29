az aks enable-addons --addons kube-dashboard -g atarraya-sample -n atarraya-sample
kubectl create clusterrolebinding kubernetes-dashboard --clusterrole=cluster-admin --serviceaccount=kube-system:kubernetes-dashboard
az aks browse -g kured -n kured
