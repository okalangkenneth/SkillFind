#!/bin/bash
# load-images.sh — Import locally-built Docker images into the Docker Desktop kind cluster.
#
# Docker Desktop's k8s (kind-based) uses containerd inside the Linux VM.
# Images built with `docker-compose build` exist in the Docker daemon image store
# (accessible via the `default` context on Windows), NOT in containerd.
# This script saves each image and loads it into the docker-desktop context
# so that `imagePullPolicy: Never` deployments can find them.
#
# Prerequisites:
#   - Docker Desktop running with Kubernetes enabled
#   - docker-compose build already completed (all 6 images present)
#   - kubectl context set to docker-desktop
#
# Usage:  bash k8s/load-images.sh

set -euo pipefail

IMAGES=(
  "skillfind-jobposting-api:latest"
  "skillfind-jobcategory-api:latest"
  "skillfind-jobseeker-api:latest"
  "skillfind-notification-service:latest"
  "skillfind-search-service:latest"
  "skillfind-apigateway:latest"
)

echo "==> Checking source images in default Docker context..."
for img in "${IMAGES[@]}"; do
  if docker --context default inspect "$img" > /dev/null 2>&1; then
    echo "  [OK] $img"
  else
    echo "  [MISSING] $img — run: docker-compose build"
    exit 1
  fi
done

echo ""
echo "==> Loading images into Docker Desktop Linux engine (desktop-linux context)..."
for img in "${IMAGES[@]}"; do
  echo "  Loading $img ..."
  docker --context default save "$img" | docker --context desktop-linux load
done

echo ""
echo "==> Done. All images loaded into the Docker Desktop k8s node."
echo "    Pods using imagePullPolicy: Never will now find these images."
