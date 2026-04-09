# SkillFind — Job Portal Microservices Platform

[![CI](https://github.com/okalangkenneth/SkillFind/actions/workflows/ci.yml/badge.svg)](https://github.com/okalangkenneth/SkillFind/actions/workflows/ci.yml)
[![CD](https://github.com/okalangkenneth/SkillFind/actions/workflows/cd.yml/badge.svg)](https://github.com/okalangkenneth/SkillFind/actions/workflows/cd.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

**[Live Demo](https://okalangkenneth.github.io/SkillFind/)** · **[Architecture](#architecture)** · **[Kubernetes](#kubernetes)** · **[Getting Started](#getting-started)**

An end-to-end job portal platform built with a production-grade microservices architecture.
Inherited as a single half-finished service; rehabilitated, extended, and deployed with
full Kubernetes orchestration, a CI/CD pipeline, and an ELK observability stack.

---

## Architecture

```
                          ┌─────────────────────────────────────────────┐
                          │              Kubernetes Cluster              │
                          │                 (skillfind ns)               │
                          │                                              │
 Browser / Client         │   ┌──────────┐     ┌──────────────────────┐ │
       │                  │   │  NGINX   │     │     API Gateway      │ │
       │ :80 / :30000 ────┼──▶│ Ingress  │────▶│  (Ocelot — routes   │ │
                          │   └──────────┘     │  by path prefix)     │ │
                          │                    └──────────────────────┘ │
                          │                              │               │
                          │          ┌───────────────────┼──────────┐    │
                          │          ▼           ▼        ▼          ▼    │
                          │   ┌──────────┐ ┌────────┐ ┌────────┐ ┌──────┐│
                          │   │JobPosting│ │  Job   │ │  Job   │ │Search││
                          │   │   API    │ │Category│ │Seeker  │ │  Svc ││
                          │   └────┬─────┘ └────────┘ └────────┘ └──┬───┘│
                          │        │                                  │    │
                          │        ▼ publish event                    │    │
                          │   ┌─────────┐   ┌──────────────────┐     │    │
                          │   │RabbitMQ │──▶│  Notification    │     │    │
                          │   └─────────┘   │    Service       │     │    │
                          │        │        └──────────────────┘     │    │
                          │        └──────────────────────────────────┘    │
                          │                         ▼ index documents       │
                          │                  ┌─────────────┐               │
                          │                  │Elasticsearch│               │
                          │                  │(skillfind-  │               │
                          │                  │  jobs idx)  │               │
                          │                  └─────────────┘               │
                          │                         │                       │
                          │                  ┌─────────────┐               │
                          │                  │   Kibana    │               │
                          │                  │  (logs UI)  │               │
                          │                  └─────────────┘               │
                          │                                                │
                          │   ┌───────────────────────────────┐           │
                          │   │         PostgreSQL             │           │
                          │   │  jobposting / jobcategory /   │           │
                          │   │        jobseeker DBs           │           │
                          │   └───────────────────────────────┘           │
                          └─────────────────────────────────────────────────┘
```

### Services

| Service | Responsibility | Tech |
|---------|---------------|------|
| **JobPosting.API** | Job post CRUD, publishes `JobPostCreatedEvent` | .NET 8, MediatR, MassTransit, EF Core, PostgreSQL |
| **JobCategory.API** | Category management | .NET 8, MediatR, EF Core, PostgreSQL |
| **JobSeeker.API** | User registration, profiles, auth — issues JWT tokens for employer authentication | .NET 8, OpenIddict, ASP.NET Identity, PostgreSQL |
| **Notification.Service** | Consumes events, sends alerts | .NET 8 Worker, MassTransit, RabbitMQ |
| **Search.Service** | Full-text job search, indexes to `skillfind-jobs` | .NET 8, NEST 7.17, Elasticsearch, MassTransit |
| **ApiGateway** | Single entry point, path-based routing | .NET 8, Ocelot |

### Authentication Flow

```
Employer registers → POST /api/jobseekers (JobSeeker.API)
Employer logs in   → POST /connect/token  (OpenIddict password flow)
                     Returns JWT access token

Employer posts job → POST /api/v1/jobposting
                     Authorization: Bearer <token>
                     JobPosting.API validates token against JobSeeker.API discovery endpoint
                     EmployerId extracted from token sub claim
                     Stored on Job_Posting entity

Only owner can modify → PUT/DELETE check jobPost.IsOwnedBy(currentUserId)
                        Returns 403 if token belongs to a different employer
GET endpoints are public — job seekers browse without logging in
```

### Ocelot Route Map

| Upstream path (client) | Downstream service | Port |
|------------------------|-------------------|------|
| `/api/jobs/{everything}` | `jobposting-api` | 8080 |
| `/api/categories/{everything}` | `jobcategory-api` | 8080 |
| `/api/jobseekers/{everything}` | `jobseeker-api` | 8080 |
| `/api/search/{everything}` | `search-service` | 8080 |
| `/connect/{everything}` | `jobseeker-api` (OpenIddict) | 8080 |

---

## Kubernetes

The Kubernetes layer is what differentiates this project from a standard docker-compose
deployment. Every service runs in the `skillfind` namespace with production-grade configuration.

### Object breakdown

| Object | Purpose |
|--------|---------|
| `Namespace` | Logical isolation — all resources scoped to `skillfind` |
| `Secret` | Base64-encoded credentials (postgres, rabbitmq, connection strings) |
| `ConfigMap` | Non-sensitive shared config injected as env vars |
| `PersistentVolumeClaim` | Durable storage for PostgreSQL and Elasticsearch |
| `Deployment` | Desired-state declaration — Kubernetes self-heals toward this |
| `Service (ClusterIP)` | Stable internal DNS for inter-service communication |
| `Service (NodePort 30000)` | External access on port 30000 for local testing (apigateway) |
| `Ingress (NGINX)` | Path-based routing to services from a single entry point |

### Key decisions

**`imagePullPolicy: Always` with GHCR** — deployments pull `ghcr.io/okalangkenneth/skillfind-*:latest`
on every pod start, so the cluster always runs the image built by the latest CD run.
During local development a `registry:2` container on port 5555 bridges Docker Desktop's
build daemon and containerd runtime; that phase used `imagePullPolicy: IfNotPresent`.

**TCP socket probes for the Ocelot gateway** — Ocelot intercepts every HTTP path including
`/healthz` and returns 404 when no downstream route matches, which causes HTTP liveness
probes to restart the pod in a loop. TCP socket probes bypass HTTP routing entirely and
only verify the port accepts connections.

**`replicas: 1` on a single-node cluster** — Docker Desktop provides one node with a
shared CPU budget. Running `replicas: 2` across all services exhausts the quota.
In a multi-node production cluster, `replicas: 2-3` with pod anti-affinity rules
would be the correct configuration.

### Deploy to local cluster

```bash
# Prerequisites: Docker Desktop with Kubernetes enabled, NGINX Ingress Controller installed
kubectl apply -f k8s/namespace.yaml
kubectl apply -f k8s/secrets/
kubectl apply -f k8s/configmaps/
kubectl apply -f k8s/pvcs/

# Infrastructure first
kubectl apply -f k8s/deployments/postgres-deployment.yaml
kubectl apply -f k8s/deployments/rabbitmq-deployment.yaml
kubectl apply -f k8s/deployments/elasticsearch-deployment.yaml

# Wait for infrastructure to be ready
kubectl wait --namespace skillfind --for=condition=ready pod \
  --selector=app=postgres --timeout=120s
kubectl wait --namespace skillfind --for=condition=ready pod \
  --selector=app=rabbitmq --timeout=120s

# Application services
kubectl apply -f k8s/deployments/
kubectl apply -f k8s/services/
kubectl apply -f k8s/ingress/

# Verify
kubectl get pods -n skillfind
curl http://localhost:30000/api/categories
```

---

## Getting Started

### Option A — Docker Compose (local dev)

```bash
git clone https://github.com/okalangkenneth/SkillFind.git
cd SkillFind

docker compose up -d --build
```

Wait ~60 seconds for Elasticsearch to become healthy, then apply migrations:

```bash
dotnet ef database update \
  --project src/Services/JobPosting/JobPosting.Infrastructure \
  --startup-project src/Services/JobPosting/JobPosting.API

dotnet ef database update \
  --project src/Services/JobCategory/JobCategory.Infrastructure \
  --startup-project src/Services/JobCategory/JobCategory.API

dotnet ef database update \
  --project src/Services/JobSeeker/JobSeeker.Infrastructure \
  --startup-project src/Services/JobSeeker/JobSeeker.API
```

Services available at:

| Service | URL |
|---------|-----|
| API Gateway | http://localhost:5000 |
| JobPosting API | http://localhost:5001 |
| JobCategory API | http://localhost:5002 |
| JobSeeker API | http://localhost:5003 |
| Search Service | http://localhost:5004 |
| RabbitMQ Management | http://localhost:15672 |
| Kibana | http://localhost:5601 |

### Option B — Kubernetes (see [Deploy to local cluster](#deploy-to-local-cluster))

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Services | .NET 8 Minimal APIs |
| ORM | EF Core 8 + Npgsql |
| CQRS | MediatR 12 |
| Mapping | Mapster 7 |
| Auth | OpenIddict 5.4 |
| Messaging | MassTransit 8 + RabbitMQ 3.13 |
| Gateway | Ocelot 23 |
| Search | NEST 7.17 + Elasticsearch 7.17.21 |
| Database | PostgreSQL 16 |
| Logging | Serilog + ELK Stack |
| Containers | Docker + docker-compose |
| Orchestration | Kubernetes (10 Deployments, NGINX Ingress) |
| Registry | GitHub Container Registry (GHCR) |
| CI/CD | GitHub Actions |
| Demo | GitHub Pages + Leaflet.js |

---

## CI/CD Pipeline

```
push to main
     │
     ├── CI workflow (ubuntu-latest, .NET 8.0.x)
     │     dotnet restore → dotnet build --configuration Release
     │     → dotnet test (continue-on-error until test projects added)
     │
     └── CD workflow (ubuntu-latest, docker/build-push-action@v5)
           Build 6 Docker images with GitHub Actions layer cache
           Push to ghcr.io/okalangkenneth/skillfind-*:<8-char-sha>
           Push to ghcr.io/okalangkenneth/skillfind-*:latest
```

Images are tagged with both `latest` and the short git SHA for full traceability.
The CD workflow uses `GITHUB_TOKEN` — no extra secrets needed.

### GHCR images

| Image | Repository |
|-------|-----------|
| jobposting-api | `ghcr.io/okalangkenneth/skillfind-jobposting-api` |
| jobcategory-api | `ghcr.io/okalangkenneth/skillfind-jobcategory-api` |
| jobseeker-api | `ghcr.io/okalangkenneth/skillfind-jobseeker-api` |
| notification-service | `ghcr.io/okalangkenneth/skillfind-notification-service` |
| search-service | `ghcr.io/okalangkenneth/skillfind-search-service` |
| apigateway | `ghcr.io/okalangkenneth/skillfind-apigateway` |

---

## Observability

All services write structured JSON logs via Serilog to Elasticsearch.

| Service | Index pattern |
|---------|--------------|
| JobPosting.API | `jobposting-logs-{yyyy.MM.dd}` |
| JobSeeker.API | `jobseeker-logs-{yyyy.MM.dd}` |
| Notification.Service | `notification-logs-{yyyy.MM.dd}` |
| Search.Service | `search-logs-{yyyy.MM.dd}` |

Kibana: open http://localhost:5601 → Index Management → use pattern `*-logs-*` with `@timestamp`.
Recommended columns: `level`, `fields.SourceContext`, `message`.

Job documents are indexed to Elasticsearch index `skillfind-jobs` by `Search.Service`
via a `JobPostCreatedIndexConsumer` that listens on the MassTransit/RabbitMQ bus.

---

## What I'd Add for Production

- **Audience validation** — register JobPosting.API as a resource server in OpenIddict's database so the `aud` claim can be validated in addition to the issuer
- **Horizontal Pod Autoscaler (HPA)** on JobPosting and Search — scale on CPU/RPS
- **Sealed Secrets or External Secrets Operator** — replace base64 secrets with vault-backed ones
- **Istio or Linkerd service mesh** — mTLS between services, distributed tracing
- **Multi-node cluster** — enable `replicas: 2-3` with pod anti-affinity rules
- **Helm chart** — package all k8s manifests as a versioned, parameterised chart
- **Integration test project** — end-to-end API tests in the CI pipeline
- **Rate limiting in Ocelot** — protect the gateway from abuse

---

## License

MIT © [Kenneth Okalang](https://github.com/okalangkenneth)
