kubectl apply -f gql\service.yaml
kubectl apply -f gql\deployment.yaml
kubectl apply -f db\service.yaml
kubectl apply -f db\deployment.yaml
kubectl apply -f webapp\service.yaml
kubectl apply -f webapp\deployment.yaml