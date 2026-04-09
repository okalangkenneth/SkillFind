# LinkedIn Post — SkillFind

---

Rebuilt a job portal from the ground up — and finally understood why Kubernetes exists.

I inherited a half-finished microservices project: one service, targeting an EOL framework, with hardcoded credentials, no Docker, no tests, and five services that were supposed to exist but didn't.

Eight phases later it runs on a local Kubernetes cluster with a CI/CD pipeline shipping to GitHub Container Registry.

Here's what the rehabilitation involved:

**The starting point:** A single JobPosting service on .NET 5 (end-of-life), AutoMapper with a known CVE, SQL Server instead of the planned PostgreSQL, and hardcoded passwords in appsettings.json.

**What got built from scratch:** Five additional services — JobSeeker with OpenIddict auth, JobCategory, a Notification worker consuming RabbitMQ events, a Search service backed by Elasticsearch, and an Ocelot API gateway. 13 projects total, 0 errors.

**The part I wanted to understand deeply — Kubernetes:**

Docker Compose tells your machine what to run. Kubernetes tells a cluster what state to maintain — forever. If a pod crashes, it restarts. If a node goes down, pods reschedule. You declare intent and the control loop reconciles reality toward it.

The concrete objects I worked with:
→ Deployments declare replicas and container spec
→ Services give pods a stable DNS name inside the cluster
→ Secrets hold base64-encoded credentials injected as env vars
→ PersistentVolumeClaims survive pod restarts (critical for Postgres and Elasticsearch)
→ Ingress routes external traffic by URL path via NGINX

Two things that only become clear when you actually run it:

First — Ocelot intercepts every HTTP path including /healthz. Kubernetes liveness probes hit /healthz, get a 404, and restart the pod in a loop. The fix: switch gateway probes to tcpSocket, which checks if the port accepts connections without touching HTTP routing.

Second — Docker Desktop's Kubernetes uses containerd internally, which can't see images built with docker build. A local registry container bridges the two runtimes. In production this is replaced by GHCR images from the CD pipeline.

The pipeline runs on GitHub Actions: CI builds and tests all 13 projects on every push, CD builds 6 Docker images and pushes to GitHub Container Registry tagged with both latest and the git SHA.

The interactive demo (Leaflet.js, dark map, job pins across Swedish cities) is on GitHub Pages.

My background is in agriculture and economics — I came to software through data, then infrastructure, then distributed systems. Each project adds a layer the previous one didn't have.

GitHub: https://github.com/okalangkenneth/SkillFind
Live demo: https://okalangkenneth.github.io/SkillFind/

#dotnet #kubernetes #microservices #devops #csharp #rabbitmq #elasticsearch #githubactions #softwaredevelopment
