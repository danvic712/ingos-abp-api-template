kubectl apply -f ./yaml/ingress-nginx.yaml
kubectl apply -f ./yaml/kubernetes-dashboard.yaml
kubectl apply -f ./yaml/kubernetes-dashboard-ingress.yaml

$TOKEN = ((kubectl -n kube-system describe secret default | Select-String "token:") -split " +")[1]
kubectl config set-credentials docker-for-desktop --token="${TOKEN}"
echo $TOKEN

"Any key to exit"  ;
Read-Host | Out-Null ;
Exit