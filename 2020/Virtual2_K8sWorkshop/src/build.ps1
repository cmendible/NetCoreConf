$crName="cmendibl3"

docker build -f .\docker\webui.Dockerfile -t $crName/netcoreconfv2-webui .
docker build -f .\docker\webapi.Dockerfile -t $crName/netcoreconfv2-webapi .
docker build -f .\docker\secrets.Dockerfile -t $crName/netcoreconfv2-secrets .
docker push $crName/netcoreconfv2-webui
docker push $crName/netcoreconfv2-webapi
docker push $crName/netcoreconfv2-secrets
