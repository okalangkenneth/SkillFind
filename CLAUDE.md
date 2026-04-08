# CLAUDE.md — SkillFind (Inherited)
# Job portal microservices platform — rehabilitation project
# Location: E:\Projects\inherited\SkillFind
# Repo: https://github.com/okalangkenneth/SkillFind
# Build state last updated: 2026-04-08 (Phase 3 complete — EF migrations applied, MassTransit wired, ES indexed)

---

## PROJECT OVERVIEW

SkillFind is an abandoned job-portal microservices solution originally designed with:
- .NET microservices (DDD + CQRS pattern)
- IdentityServer4 for auth
- Ocelot API Gateway
- RabbitMQ + MassTransit for messaging
- Elasticsearch for job search
- ELK stack for observability
- **Kubernetes + Docker** for orchestration (KEY DIFFERENTIATOR)
- GitHub Actions CI/CD pipeline

The Kubernetes layer is what makes this project unique in the portfolio. Every other project uses
docker-compose only. SkillFind adds real k8s manifests (Deployments, Services, Ingress, ConfigMaps,
Secrets, PVCs) so the portfolio demonstrates production-grade orchestration knowledge.

---

## DISCOVERY PHASE — COMPLETE BEFORE WRITING ANY CODE

### Step 1 — Clone and catalogue
```
git clone https://github.com/okalangkenneth/SkillFind.git E:\Projects\inherited\SkillFind
```
Then run:
```
read_multiple_files src/**/*.csproj
```
List every .csproj found and add them to the BUILD STATE TRACKER below.

### Step 2 — Assess each service
For each service, document:
- Does it build? (`dotnet build src/<service>/<service>.csproj`)
- What NuGet packages does it use? (check .csproj)
- Does it have any DB migrations?
- Does it have any existing Dockerfile?
- Does it have any existing k8s manifests?

### Step 3 — Identify the gaps
Expected services (from README architecture):
- [x] JobSeeker.API — **MISSING** (solution folder placeholder only, no code)
- [x] JobPosting.API — **EXISTS** (builds with warnings — see BUILD STATE TRACKER)
- [x] JobCategory.API — **MISSING** (solution folder placeholder only, no code)
- [x] Notification.Service — **MISSING** (solution folder placeholder only, no code)
- [x] ApiGateway — **MISSING** (not referenced in solution at all)
- [x] Search.Service — **MISSING** (not referenced in solution at all)

Cross-cutting concerns:
- [x] Any existing Dockerfiles? **NO** — none anywhere
- [x] Any existing docker-compose.yml? **NO** — none anywhere
- [x] Any existing k8s/ or kubernetes/ folder? **NO** — none anywhere
- [x] Any existing GitHub Actions workflows (.github/workflows/)? **NO** — none anywhere
- [x] Any existing README beyond the top-level one? **NO** — only root README.md

---

## BUILD STATE TRACKER

### Services
| Service | Builds? | DB? | Dockerfile? | k8s manifest? | Notes |
|---------|---------|-----|-------------|---------------|-------|
| JobSeeker.API | CLEAN | Npgsql+OpenIddict | YES | NO | Phase 2: Dockerfile added; InitialCreate migration |
| JobPosting.API | CLEAN | Npgsql migrations | YES | NO | Phase 2: Dockerfile added; InitialCreate migration |
| JobPosting.Application | CLEAN | - | - | NO | Fixed Phase 1A |
| JobPosting.Domain | CLEAN | - | - | NO | Fixed Phase 1A |
| JobPosting.Infrastructure | CLEAN | - | - | NO | Phase 2: Design-time factory added |
| JobCategory.API | CLEAN | Npgsql | YES | NO | Phase 2: Dockerfile added; InitialCreate migration |
| Notification.Service | CLEAN | - | YES | NO | Phase 2: Dockerfile added |
| ApiGateway | CLEAN | - | YES | NO | Phase 2: Dockerfile added; ocelot.json publish fix |
| Search.Service | CLEAN | - | YES | NO | Phase 2: Dockerfile added |

**dotnet build result (2026-04-02):** `Build succeeded. 4 Warning(s), 0 Error(s)`
SDK on machine: .NET 9.0.201 (but projects target net5.0)

### Infrastructure
| Component | Status |
|-----------|--------|
| PostgreSQL | docker-compose — postgres:16-alpine, port 5432, 3 databases |
| RabbitMQ | docker-compose — rabbitmq:3.13-management-alpine, ports 5672/15672 |
| Elasticsearch 7.17 | docker-compose — elasticsearch:7.17.21, port 9200 |
| Kibana 7.17 | docker-compose — kibana:7.17.21, port 5601 |
| Redis (session cache) | NOT ADDED |
| NGINX Ingress | NOT ADDED (Phase 4) |

---

## TECHNICAL DEBT LOG

### Phase 0 findings (2026-04-02)

**Framework / Package issues**
- [ALL PROJECTS] [HIGH] net5.0 EOL since May 2022 — upgrade all to net8.0 in Phase 1
- [JobPosting.Application] [HIGH] AutoMapper 12.0.1 — known HIGH severity vulnerability (GHSA-rvv3-g6hj-g44x) — upgrade to 13.x+
- [JobPosting.Infrastructure] [HIGH] Uses `Microsoft.EntityFrameworkCore.SqlServer` — plan calls for PostgreSQL; swap to `Npgsql.EntityFrameworkCore.PostgreSQL`
- [JobPosting.Application] [MED] `services.AddMediatR(Assembly.GetExecutingAssembly())` — MediatR 11 uses this API but MediatR 12 changed it to `AddMediatR(cfg => cfg.RegisterServicesFromAssembly(...))` — will break on upgrade

**Security / Config**
- [JobPosting.API] [HIGH] Hardcoded credentials in appsettings.json: `User Id=ok;Password=Ok12345678;` — must move to env vars / secrets
- [JobPosting.API] [MED] SendGrid API key field present in appsettings.json but value is empty string `""` — not using env var
- [JobPosting.Infrastructure] [MED] `services.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"))` — incorrect binding syntax (missing `.Bind`), email config will be empty at runtime

**Missing capabilities**
- [JobPosting.API] [MED] No health check endpoint (`/healthz`) — required for k8s liveness/readiness probes
- [ALL PROJECTS] [MED] No Serilog — using default ASP.NET logging; need console + Elasticsearch sinks for ELK stack
- [JobPosting.API] [LOW] No CORS policy configured

**Code bugs**
- [JobPosting.API] [MED] Route/parameter mismatch in `JobPostingController.GetJobPostingsTitle`: route template is `{userName}` but action parameter is `string title` — the route value will never bind
- [JobPostingContext] [MED] `SaveChangesAsync` hardcodes `JobCategory = "Programming"` and `JobCategoryDescription = "Python,C#, JavaScript"` for every Add/Update — this is leftover placeholder logic that must be removed

**Missing services (5 of 6 need to be created from scratch)**
- JobSeeker.API (with OpenIddict auth — IdentityServer4 is EOL)
- JobCategory.API
- Notification.Service (RabbitMQ + MassTransit)
- ApiGateway (Ocelot)
- Search.Service (Elasticsearch)

---

## REHABILITATION PHASES

### Phase 0 — Discovery & Triage (do this first, before any code)
- Clone repo
- Run `dotnet build` on every service
- Catalogue all errors into TECHNICAL DEBT LOG
- Update BUILD STATE TRACKER
- Mark Phase 0 COMPLETED when every service's build status is known

### Phase 1 — Make Everything Build Clean
- Fix all compile errors service by service
- Upgrade deprecated packages (especially IdentityServer4 → OpenIddict or Duende)
- Add missing appsettings.Development.json stubs so nothing crashes on startup
- Verify: `dotnet build` returns 0 errors, 0 warnings for every service

### Phase 2 — Dockerize All Services
- Add Dockerfile to each service (use multi-stage build, .NET 8 SDK → runtime)
- Add docker-compose.yml with all services + infrastructure
- Add health checks to each API (/healthz endpoint using app.MapHealthChecks)
- Add Serilog with console + Elasticsearch sinks to each service
- Verify: `docker-compose up -d --build` and all containers stay green

### Phase 3 — Wire Services Together
- Configure RabbitMQ + MassTransit consumers in Notification.Service
- Configure Ocelot routes in ApiGateway to proxy all downstream services
- Configure Elasticsearch index for job search in Search.Service
- Seed test data via a SQL migration or a seed endpoint
- Verify: end-to-end flow works through API Gateway

### Phase 4 — Kubernetes Manifests (KEY PHASE)
See KUBERNETES GUIDE section below for full explanation.

Deliverables for this phase:
```
k8s/
  namespace.yaml
  configmaps/
    app-config.yaml
  secrets/
    app-secrets.yaml
  deployments/
    jobseeker-deployment.yaml
    jobposting-deployment.yaml
    jobcategory-deployment.yaml
    notification-deployment.yaml
    apigateway-deployment.yaml
    search-deployment.yaml
    postgres-deployment.yaml
    rabbitmq-deployment.yaml
    elasticsearch-deployment.yaml
    kibana-deployment.yaml
  services/
    jobseeker-service.yaml
    jobposting-service.yaml
    jobcategory-service.yaml
    notification-service.yaml
    apigateway-service.yaml
    search-service.yaml
    postgres-service.yaml
    rabbitmq-service.yaml
    elasticsearch-service.yaml
    kibana-service.yaml
  ingress/
    ingress.yaml
  pvcs/
    postgres-pvc.yaml
    elasticsearch-pvc.yaml
```

Test with: Docker Desktop Kubernetes (enable in Docker Desktop settings → Kubernetes → Enable Kubernetes)
Deploy with: `kubectl apply -f k8s/`
Verify with: `kubectl get pods -n skillfind` — all pods Running

### Phase 5 — GitHub Actions CI/CD
- Add `.github/workflows/ci.yml` — build + test on push to main
- Add `.github/workflows/cd.yml` — build Docker images, push to GHCR, apply k8s manifests
- Use `GITHUB_TOKEN` for GHCR (no extra secrets needed for public repos)

### Phase 6 — GitHub Pages Demo
- Add `docs/` folder with Leaflet.js job map demo
- Dark "field instrument" aesthetic (CartoDB Dark Matter tiles)
- Show job pins on map, filter by category
- Wire to a mock JSON data file (no backend needed for demo)

### Phase 7 — README + LinkedIn Post
- Update README.md with:
  - Architecture diagram (Mermaid)
  - Kubernetes architecture diagram (Mermaid)
  - Setup instructions (docker-compose AND kubectl paths)
  - GitHub Pages demo link
- Write LINKEDIN_POST.md following established pattern

---

## KUBERNETES GUIDE — HOW IT WORKS

This section explains the Kubernetes concepts used in this project so you understand
what you're building, not just how to copy it.

### The Mental Model

Docker Compose = "run these containers on MY machine"
Kubernetes    = "run these containers ACROSS a cluster of machines, keep them healthy,
                 scale them, and self-heal if they crash"

For local dev, Docker Desktop provides a single-node Kubernetes cluster that behaves
exactly like a production cluster — perfect for portfolio demonstration.

### Core Kubernetes Objects

#### Namespace (namespace.yaml)
A logical partition inside the cluster. All SkillFind resources live in the `skillfind`
namespace so they don't collide with anything else on the cluster.
```yaml
apiVersion: v1
kind: Namespace
metadata:
  name: skillfind
```

#### Deployment (deployments/jobposting-deployment.yaml)
Declares "I want 2 replicas of the JobPosting API container running at all times."
If one crashes, Kubernetes restarts it automatically.
```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: jobposting-api
  namespace: skillfind
spec:
  replicas: 2                        # run 2 copies
  selector:
    matchLabels:
      app: jobposting-api
  template:
    metadata:
      labels:
        app: jobposting-api
    spec:
      containers:
      - name: jobposting-api
        image: ghcr.io/okalangkenneth/skillfind-jobposting:latest
        ports:
        - containerPort: 8080
        env:
        - name: ConnectionStrings__DefaultConnection
          valueFrom:
            secretKeyRef:
              name: app-secrets
              key: postgres-connection
        resources:
          requests:
            memory: "128Mi"
            cpu: "100m"
          limits:
            memory: "256Mi"
            cpu: "200m"
        livenessProbe:
          httpGet:
            path: /healthz
            port: 8080
          initialDelaySeconds: 10
          periodSeconds: 30
```

#### Service (services/jobposting-service.yaml)
A stable network address for a Deployment. Pods get random IPs that change on restart;
a Service gives them a fixed DNS name inside the cluster.
- **ClusterIP** (default) — only reachable inside the cluster. Use for inter-service calls.
- **NodePort** — exposes on a port on every node. Useful for local testing.
- **LoadBalancer** — provisions a cloud load balancer. Use in production cloud.
```yaml
apiVersion: v1
kind: Service
metadata:
  name: jobposting-api
  namespace: skillfind
spec:
  selector:
    app: jobposting-api   # routes to pods with this label
  ports:
  - port: 80
    targetPort: 8080
  type: ClusterIP
```

#### Ingress (ingress/ingress.yaml)
The single entry point for external traffic — exactly like Ocelot but at the
infrastructure level. NGINX Ingress Controller routes requests by path or hostname.
```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: skillfind-ingress
  namespace: skillfind
  annotations:
    nginx.ingress.kubernetes.io/rewrite-target: /$2
spec:
  ingressClassName: nginx
  rules:
  - http:
      paths:
      - path: /jobs(/|$)(.*)
        pathType: Prefix
        backend:
          service:
            name: jobposting-api
            port:
              number: 80
      - path: /categories(/|$)(.*)
        pathType: Prefix
        backend:
          service:
            name: jobcategory-api
            port:
              number: 80
      - path: /api(/|$)(.*)
        pathType: Prefix
        backend:
          service:
            name: apigateway
            port:
              number: 80
```

#### ConfigMap (configmaps/app-config.yaml)
Non-secret configuration injected into pods as environment variables or mounted files.
```yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: app-config
  namespace: skillfind
data:
  RabbitMQ__Host: "rabbitmq"
  Elasticsearch__Uri: "http://elasticsearch:9200"
  ASPNETCORE_ENVIRONMENT: "Production"
```

#### Secret (secrets/app-secrets.yaml)
Base64-encoded sensitive values. Never commit real secrets — use sealed-secrets or
external secret managers in production.
```yaml
apiVersion: v1
kind: Secret
metadata:
  name: app-secrets
  namespace: skillfind
type: Opaque
data:
  postgres-connection: <base64-encoded-connection-string>
  rabbitmq-password: <base64-encoded-password>
```
Encode: `echo -n "mypassword" | base64`

#### PersistentVolumeClaim (pvcs/postgres-pvc.yaml)
A request for durable storage. Without this, a Pod's filesystem is wiped on restart —
databases would lose all data. The PVC survives Pod restarts.
```yaml
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: postgres-pvc
  namespace: skillfind
spec:
  accessModes:
  - ReadWriteOnce
  resources:
    requests:
      storage: 5Gi
```

### Key kubectl Commands to Know

```bash
# Apply all manifests
kubectl apply -f k8s/

# Check pod status
kubectl get pods -n skillfind

# See logs from a pod
kubectl logs -n skillfind <pod-name> -f

# Describe a pod (see events, errors)
kubectl describe pod -n skillfind <pod-name>

# Get services
kubectl get services -n skillfind

# Shell into a running pod
kubectl exec -it -n skillfind <pod-name> -- /bin/sh

# Delete everything and start fresh
kubectl delete namespace skillfind
kubectl apply -f k8s/namespace.yaml
kubectl apply -f k8s/

# Scale a deployment
kubectl scale deployment jobposting-api --replicas=3 -n skillfind

# Watch pods restart in real time
kubectl get pods -n skillfind -w
```

### Docker Desktop Kubernetes Setup
1. Open Docker Desktop → Settings → Kubernetes → Enable Kubernetes → Apply & Restart
2. Wait ~2 minutes for the cluster to start
3. Install NGINX Ingress Controller (one-time):
   ```
   kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.10.1/deploy/static/provider/cloud/deploy.yaml
   ```
4. Verify: `kubectl get nodes` should show one node in Ready state

---

## WORKFLOW RULES (same as other projects)

- ALWAYS use `docker-compose up -d --build` — never `docker-compose restart`
- Git operations go through Claude Code (Code tab), not Desktop Commander
- Desktop Commander: file reading, short commands, dotnet build per service
- Claude Code: build-fix loops, docker commands, git push
- ALWAYS end every Claude Code prompt with: `git push origin main`
- Update this CLAUDE.md after every phase (mark COMPLETED, update BUILD STATE TRACKER)

---

## COMPLETED PHASES
- [x] Phase 0 — Discovery & Triage (2026-04-02)
- [x] Phase 1 — Make Everything Build Clean (2026-04-06)
  - Phase 1A: Upgraded JobPosting to net8, Mapster, Npgsql, Serilog, MediatR v12
  - Phase 1B: Scaffolded JobCategory, JobSeeker (OpenIddict), Notification, ApiGateway, Search
  - Full solution builds: 0 errors, 0 warnings (13 projects)
- [x] Phase 2 — Dockerize All Services (2026-04-07)
  - 6 multi-stage Dockerfiles (sdk:8.0 → aspnet:8.0)
  - docker-compose.yml: postgres, rabbitmq, elasticsearch, kibana + all 6 app services
  - infrastructure/postgres/init.sql: creates skillfind_jobcategory + skillfind_jobseeker DBs
  - EF InitialCreate migrations for JobPosting, JobCategory, JobSeeker
  - Design-time DbContext factories for all 3 DB services
  - Serilog config section added to all 6 appsettings.json files
  - ApiGateway.csproj: ocelot.json marked as publish content
- [x] Phase 3 — Wire Services Together (2026-04-08)
  - Added CompanyName to Job_Posting entity + CreateJobPostingCommand + migration AddCompanyName
  - EF migrations applied to all 3 PostgreSQL databases (localhost:5432)
  - MassTransit + Notification.Domain added to JobPosting.Application
  - MassTransit RabbitMQ publisher registered in JobPosting.API Program.cs
  - CreateJobPostingCommandHandler now publishes JobPostCreatedEvent on every job creation
  - JobPostCreatedConsumer in Notification.Service logs new postings ✅
  - JobPostCreatedIndexConsumer in Search.Service indexes to Elasticsearch ✅
  - Elasticsearch index 'skillfind-jobs' created on Search.Service startup
  - JobPosting Dockerfile updated to include Notification.Domain source
  - Ocelot routes verified — all 5 routes match docker-compose service names/ports
  - docker-compose.yml: removed deprecated `version: '3.8'` header
  - Smoke tests available: POST /api/v1/jobposting → event → Notification logs + ES index

## REMAINING PHASES
- [ ] Phase 4 — Kubernetes Manifests
- [ ] Phase 5 — GitHub Actions CI/CD
- [ ] Phase 6 — GitHub Pages Demo
- [ ] Phase 7 — README + LinkedIn Post
