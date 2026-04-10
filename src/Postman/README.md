# Postman

## What “local” means

Postman and Newman only send HTTP to whatever URL `baseUrl` is. They do not start the API.

- **On your machine:** run the API first (`dotnet run` in `src/NatureHelp`, IDE “Development” profile, or Docker). Set `baseUrl` to that process (e.g. `http://localhost:5077` from `Properties/launchSettings.json`). Then run the collection in the Postman app or with Newman.

- **In GitHub Actions:** the runner is another machine. “Local” there means **loopback on the runner**: the workflow must **build and start** the API in a step, wait until `GET /health` succeeds, then run Newman with `baseUrl` pointing at that same runner (`http://127.0.0.1:5077`). This repo’s [`.github/workflows/postman-tests.yml`](../../.github/workflows/postman-tests.yml) does that using Postgres and Redis service containers and `ASPNETCORE_ENVIRONMENT=CI` (see `appsettings.CI.json` in the API project). The workflow uses `dotnet run --no-launch-profile` so `Properties/launchSettings.json` does not override the environment (otherwise the app would use Development and the wrong database/password).

- **Alternative CI:** skip starting the API and point Newman at a deployed URL using a secret (e.g. `newman run ... --env-var baseUrl=${{ secrets.POSTMAN_BASE_URL }}`) and a matching environment file.

## Import

- Collection: `collection.json`
- Pick an environment:
  - `environment.json` / `environment.local.json` — `baseUrl` `http://localhost:5077` (matches `NatureHelp (Development)` in `Properties/launchSettings.json`). If you use Docker Compose, set `baseUrl` to `http://localhost:5001` when the compose file maps `5001:5000`.
  - `environment.ci.json` — same variables as local; `baseUrl` `http://127.0.0.1:5077` for the GitHub Actions job that starts the API on the runner.
  - `environment.digitalocean.json` — `baseUrl` is set for the current droplet API (`http://164.92.129.51:5000`, no path). Change it if the host, port, or scheme (HTTPS) differs.

## Order

1. Start the API (`dotnet run` from `src/NatureHelp`, or your IDE profile, or Docker) so `{{baseUrl}}` responds (e.g. `GET {{baseUrl}}/health`).
2. Run **Login user** (`POST /api/user/login`) so Tests store `accessToken` and `refreshToken` in the active environment.
3. Other requests inherit **Bearer** auth from the collection using `{{accessToken}}`.
4. **Get organization users** requires Bearer auth (same as most endpoints). **Refresh access token** uses **no auth** in the header and a body with `{{refreshToken}}`.

## Identifiers and login

For **local Development**, run the API with `ASPNETCORE_ENVIRONMENT=Development` so migrations apply and [DevelopmentDatabaseSeeder.cs](../Infrastructure/Data/DevelopmentDatabaseSeeder.cs) inserts demo rows. Postman IDs then match that seed (e.g. `valentyn@example.com` / `12341234`). **Production** does not run that seeder; use real IDs and a real user, or create a SuperAdmin via `SuperAdmin:Email` / `SuperAdmin:Password` on first deploy (see deploy docs).
