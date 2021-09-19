#!/usr/bin/env sh
COMMIT_ID=$(git rev-parse --short HEAD)

#REGISTRY=https://docker.pkg.github.com/danvic712/ingos-abp-api-template
IMAGE=ingos/ingos-template

VERSION=$(cat version) 
TAG=$(echo "$VERSION" | awk -F. -v OFS=. '{$NF++;print}')
echo "$TAG" > version

if [ "$COMMIT_ID" == "" ]; then
  IMAGE_TAG=$IMAGE:$TAG
else
  IMAGE_TAG=$IMAGE:$TAG-$COMMIT_ID
fi 

docker build -t "$IMAGE_TAG" --build-arg commit-id="$COMMIT_ID" . 
# docker push "$IMAGE_TAG"

