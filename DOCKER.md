# Run NatureHelp with Docker only

Use Docker Compose to run the app without installing .NET or Node locally.

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (Windows/Mac) or Docker Engine + Docker Compose
- Copy `.env.example` to `.env` and set variables (see [Env configuration](#env-configuration) below). Required for prod: `DATABASE_URL`. Optional for OAuth: `GOOGLE_CLIENT_ID`, `GOOGLE_CLIENT_SECRET`, `FACEBOOK_APP_ID`, `FACEBOOK_APP_SECRET`.

## Env configuration

**Docker:** Create a `.env` file in the project root (copy from `.env.example`). Docker Compose reads it and passes variables to containers.

**Local run (dotnet run):** Copy `src/NatureHelp/appsettings.Local.json.example` to `src/NatureHelp/appsettings.Local.json` and fill in connection strings, Redis, OAuth, Payment, EmailSettings, etc. The backend loads `appsettings.Local.json` after other appsettings (optional file, gitignored).

| Variable | Used by | Required for |
|----------|---------|--------------|
| `DATABASE_URL` | Prod backend (DigitalOcean managed Postgres) | Production only |
| `GOOGLE_CLIENT_ID`, `GOOGLE_CLIENT_SECRET` | Backend OAuth | If you use Google login |
| `FACEBOOK_APP_ID`, `FACEBOOK_APP_SECRET` | Backend OAuth | If you use Facebook login |

Any appsettings key can be overridden by environment variables (e.g. `ConnectionStrings__LocalConnection`, `Redis__ConnectionString`). See commented entries in `.env.example`.

## Hosting: DigitalOcean vs AWS

For this app (one server + managed DB, optional Redis), **DigitalOcean is usually the easiest and predictably cheap**:

- **DigitalOcean:** One dashboard, droplets + managed PostgreSQL. Typical small setup: ~\$12/mo droplet + ~\$15/mo managed DB (or free Postgres in container for stage). No IAM/VPC complexity. Clear pricing.
- **AWS:** More powerful long-term but more to learn (EC2, RDS, VPC, IAM). Can be cheap with 12‑month free tier; after that small RDS + EC2 often runs \$30–60+/mo. Easy to overprovision or misconfigure and get surprises.

Recommendation: use **DigitalOcean** for simplest and low cost. Consider AWS if you already use it, need other AWS services, or want to grow into a larger AWS-based setup later.

## DigitalOcean droplet: prod vs stage

Both frontend and backend run on the same droplet. Use one of:

**Production (DigitalOcean managed PostgreSQL):** Backend uses a DigitalOcean managed database; no postgres container. Put the DO connection string in `.env` as `DATABASE_URL` (see [Env configuration](#env-configuration)).

```bash
docker compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```

Frontend is built with production API URL (`environment.ts` → https://naturehelp.store/api). Point your domain (e.g. naturehelp.store) to the droplet and proxy HTTPS to the frontend container (port 80) and `/api` to the backend (port 5001 or internal).

**Staging (local PostgreSQL on droplet):** Backend uses the postgres container on the same droplet. Frontend is built with staging API URL (`environment.stage.ts` → https://stage.naturehelp.store/api).

```bash
docker compose -f docker-compose.yml -f docker-compose.stage.yml up -d
```

Point your staging domain (e.g. stage.naturehelp.store) to the same droplet. Ensure CORS and nginx (or your reverse proxy) allow the staging origin and route `/api` to the backend.

To change the staging API URL or domain, edit `src/View/nature-help/src/environments/environment.stage.ts` and rebuild the frontend.

## Updating prod and stage correctly

Use the same droplet (or two droplets) and the same repo; switch by compose files and `.env`.

### One-time setup on the droplet

1. Install Docker and Docker Compose.
2. Clone the repo and `cd` into the project root.
3. Create `.env` from `.env.example` and set at least:
   - **Prod:** `DATABASE_URL` (DigitalOcean Postgres), `GOOGLE_CLIENT_ID`, `GOOGLE_CLIENT_SECRET`, `FACEBOOK_APP_ID`, `FACEBOOK_APP_SECRET` if you use OAuth.
   - **Stage:** optional (postgres runs in Docker; OAuth vars if needed).
4. Point DNS: prod domain (e.g. naturehelp.store) and stage domain (e.g. stage.naturehelp.store) to the droplet IP.
5. Run a reverse proxy (nginx/Caddy) on the host: HTTPS → frontend container (port 80), `/api` → backend (port 5001).

### Deploy / update production

```bash
cd /path/to/project
git pull
docker compose -f docker-compose.yml -f docker-compose.prod.yml build --no-cache backend frontend
docker compose -f docker-compose.yml -f docker-compose.prod.yml up -d backend frontend
```

- Ensure `.env` has `DATABASE_URL` and OAuth vars.
- If you added new EF migrations, apply them to the prod DB (migrations do not run automatically in Production). From your machine: `dotnet ef database update --project src/Infrastructure --startup-project src/NatureHelp` with prod connection string, or run a one-off container with the same `DATABASE_URL`.

### Deploy / update staging

```bash
cd /path/to/project
git pull
docker compose -f docker-compose.yml -f docker-compose.stage.yml build --no-cache backend frontend
docker compose -f docker-compose.yml -f docker-compose.stage.yml up -d backend frontend
```

- Staging uses the postgres container; migrations run automatically only when `ASPNETCORE_ENVIRONMENT` is Development. For Staging, apply new migrations manually if needed (e.g. one-off run with connection string to the stage postgres).

### Checklist

| Step | Prod | Stage |
|------|------|--------|
| `.env` with `DATABASE_URL` | Yes (DO managed DB) | No (postgres in Docker) |
| Compose files | `docker-compose.yml` + `docker-compose.prod.yml` | `docker-compose.yml` + `docker-compose.stage.yml` |
| New EF migrations | Run manually vs prod DB | Run manually vs stage DB if not Development |
| After deploy | Check https://naturehelp.store and /api/health | Check https://stage.naturehelp.store and /api/health |

### Prod and stage on the same droplet

Run one stack at a time (prod **or** stage) by using the corresponding compose files. To run both: use two project directories (or two compose projects with `-p prod` / `-p stage`) and give stage different ports in an override (e.g. frontend 5051, backend 5002); then point the reverse proxy so `naturehelp.store` goes to prod and `stage.naturehelp.store` to stage.

## Start the app (minimal stack)

**Default (postgres in Docker):** Backend uses the **postgres container** (its own DB). Mock data is loaded automatically from `src/Infrastructure/Migrations/SQL/Autogenerating_Data.sql` by the backend right after it applies migrations (Development only).

```bash
docker compose up -d
```

**Recommended for local dev (use your existing DB):** Backend uses your **host PostgreSQL** so you see the same entities as when running `dotnet run`. No postgres container started.

```bash
docker compose -f docker-compose.yml -f docker-compose.local-db.yml up -d
```

Requirements for local DB: PostgreSQL on the host with database `NatureHelpDB`, user `postgres`, password `10101010` (same as `appsettings.Development.json`).

- **Frontend:** http://localhost:5051/nature-help/
- **Backend API:** http://localhost:5001
- **Swagger:** http://localhost:5001/swagger

The frontend is built with `environment.docker.ts`, so it calls `http://localhost:5001/api` from the browser. CORS allows `http://localhost:5051`. Port 5001 is used so NatureHelp does not conflict with another app on port 5000.

## Development with watchers (no rebuild)

Use this when you want code changes to apply without rebuilding images. Backend runs `dotnet watch run` and frontend runs `ng serve` with source mounted; both restart or reload on file changes.

**Start postgres and redis, then backend and frontend with watchers:**

```bash
docker compose -f docker-compose.yml -f docker-compose.dev.yml up
```

This stack runs **two** UI containers: the base **nginx** `frontend` (built Angular on port **5051**) and **`frontend-dev`** (`ng serve` on **4200** with live reload). Use **http://localhost:4200** while you change UI code; use **http://localhost:5051/nature-help/** for a production-like static build (rebuild the `frontend` image after code changes).

- **Frontend (live reload):** http://localhost:4200  
- **Frontend (nginx build):** http://localhost:5051/nature-help/  
- **Backend API:** http://localhost:5077 (mapped from container port 5000; matches `environment.dev.ts`)  
- **Swagger:** http://localhost:5077/swagger  

The dev watcher runs `ng serve` with `-c development`; the app imports `environment.dev.ts`, which points the API to `http://localhost:5077/api`. Backend and frontend containers use the same postgres and redis as the default compose. Do not combine with `docker-compose.local-db.yml` for the backend (dev backend expects postgres in Docker). For host DB, run only postgres/redis with the base compose, then run backend with `dotnet watch run` and frontend with `ng serve` on the host.

**Local watchers (no Docker for app):** Run postgres (and optionally redis) in Docker, then on the host:

- Backend: `dotnet watch run --project src/NatureHelp/NatureHelp.csproj` (from repo root)
- Frontend: `cd src/View/nature-help && npm run start` (or `npm run watch` for build-only watch)

Use `docker-compose.local-db.yml` so the backend connects to your host DB if needed.

## Start with postgres container (optional)

If you want the postgres container as well when using the local-db override (e.g. a second environment):

```bash
docker compose -f docker-compose.yml -f docker-compose.local-db.yml --profile with-postgres up -d
```

## Start with logging stack

To also run Seq, Elasticsearch, Loki, Grafana, Kibana, and Prometheus:

```bash
docker compose --profile logging up -d
```

| Service   | URL                    |
|----------|------------------------|
| Seq      | http://localhost:5341  |
| Grafana  | http://localhost:3000 (admin/admin) |
| Kibana   | http://localhost:5601  |
| Prometheus | http://localhost:9090 |

## Rebuild after code changes

When using the default compose (no dev override), after changing frontend or backend code you must rebuild so the running containers use the new code. To avoid rebuilds, use [Development with watchers](#development-with-watchers-no-rebuild) instead. If you use the local DB setup, add the same compose files: `-f docker-compose.yml -f docker-compose.local-db.yml`. For prod use `-f docker-compose.yml -f docker-compose.prod.yml`; for stage use `-f docker-compose.yml -f docker-compose.stage.yml`. Otherwise you may see CORS errors, 404s, or old API URLs (e.g. `/api/WaterDeficiency` instead of `/api/waterdeficiency`).

```bash
docker compose build backend frontend
docker compose up -d backend frontend
```

Or rebuild and start in one step:

```bash
docker compose up -d --build
```

If the frontend still uses old URLs or behavior, force a clean build (no cache):

```bash
docker compose build --no-cache frontend
docker compose up -d frontend
```

If you get **404** for `/api/waterdeficiency`, `/api/soildeficiency`, or `/api/laboratory`, rebuild the **backend** so it uses the updated controller routes:

```bash
docker compose build --no-cache backend
docker compose up -d backend
```

## Stop

```bash
docker compose down
```

With logging profile:

```bash
docker compose --profile logging down
```

## Volumes

Data is stored in Docker volumes: `postgres-data`, `redis` (no named volume; data in container), `dataprotection-keys`, and optionally `loki-data`, `grafana-data`, `elasticsearch-data`, `prometheus-data`. Use `docker compose down -v` to remove volumes (deletes DB and other data).
