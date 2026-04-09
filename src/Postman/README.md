# Postman

## Import

- Collection: `collection.json`
- Pick an environment:
  - `environment.json` / `environment.local.json` — `baseUrl` `http://localhost:5000`
  - `environment.digitalocean.json` — `baseUrl` is set for the current droplet API (`http://164.92.129.51:5000`, no path). Change it if the host, port, or scheme (HTTPS) differs.

## Order

1. Run **Login user** (`POST /api/user/login`) so Tests store `accessToken` and `refreshToken` in the active environment.
2. Other requests inherit **Bearer** auth from the collection using `{{accessToken}}`.
3. **Get organization users** uses **no auth** (anonymous API); **Refresh access token** uses **no auth** and a body with `{{refreshToken}}`.

## Identifiers and login

For **local Development**, run the API with `ASPNETCORE_ENVIRONMENT=Development` so migrations apply and [DevelopmentDatabaseSeeder.cs](../Infrastructure/Data/DevelopmentDatabaseSeeder.cs) inserts demo rows. Postman IDs then match that seed (e.g. `valentyn@example.com` / `12341234`). **Production** does not run that seeder; use real IDs and a real user, or create a SuperAdmin via `SuperAdmin:Email` / `SuperAdmin:Password` on first deploy (see deploy docs).
