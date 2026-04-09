# Kibana, Grafana & Elasticsearch – What They Are & How to Test

## What Each Service Is

| Service | Role |
|--------|------|
| **Elasticsearch** | Search and analytics engine. Your .NET backend (Serilog) sends application logs here. It stores and indexes log entries so you can search and aggregate them. |
| **Kibana** | Web UI for Elasticsearch. You use it to explore logs, run searches, build dashboards, and inspect indices. It talks only to Elasticsearch. |
| **Grafana** | Visualization and monitoring UI. In this project it is used with **Loki** (log store). The backend also sends logs to Loki; Grafana shows those logs and can build dashboards. It can also use Elasticsearch as a data source. |

In this project:

- **Backend** writes logs to: Seq, **Elasticsearch**, and **Grafana Loki** (see `Serilog` in `appsettings.json` and Docker env).
- **Elasticsearch** holds those logs in daily indices like `naturehelp-logs-2025.03.18`.
- **Kibana** connects to Elasticsearch to search and visualize those logs.
- **Grafana** connects to Loki (and optionally Elasticsearch) to visualize logs and metrics.

---

## Prerequisites

- Docker and Docker Compose installed.
- Project root: `d:\MyProjects` (or your clone).

---

## 1. Start the Stack Including Elasticsearch, Kibana & Grafana

Logging services (Elasticsearch, Kibana, Grafana, Loki, Seq, Prometheus) use the **`logging`** profile. Start the full app with logging:

```powershell
cd d:\MyProjects
docker compose --profile logging up -d
```

Wait until containers are healthy (Elasticsearch can take 30–60 seconds). Check:

```powershell
docker compose --profile logging ps
```

You should see `elasticsearch`, `kibana`, `grafana`, `loki`, `seq`, `backend`, etc. running.

---

## 2. Test Elasticsearch

Elasticsearch listens on **port 9200**.

**From host (PowerShell):**

```powershell
curl http://localhost:9200
```

Or in a browser: **http://localhost:9200**

Expected: JSON with `cluster_name`, `version`, etc.

**Cluster health:**

```powershell
curl "http://localhost:9200/_cluster/health?pretty"
```

Expected: `"status" : "yellow"` or `"green"` (yellow is normal for a single node).

**List indices (your log indices):**

```powershell
curl "http://localhost:9200/_cat/indices?v"
```

After the backend has been running and logging, you should see indices like `naturehelp-logs-2025.03.18`.

---

## 3. Test Kibana

Kibana runs on **http://localhost:5601**.

1. Open **http://localhost:5601** in a browser.
2. Wait for Kibana to finish loading (it waits for Elasticsearch).
3. Go to **Stack Management** (gear icon) → **Index Management** (or **Data** → **Index Management** depending on version). You should see `naturehelp-logs-*` indices.
4. Go to **Stack Management** → **Index Patterns** (or **Data Views** in 8.x). Create an index pattern:
   - Name: e.g. `naturehelp-logs-*`
   - Timestamp: select the time field (e.g. `@timestamp`).
5. Go to **Discover** (compass icon). Select the data view `naturehelp-logs-*`. You should see log events from your backend.

**Quick check:** If Discover shows no data, generate some traffic (e.g. open the app at http://localhost:5051/nature-help/, call API, then refresh Discover and expand the time range).

---

## 4. Test Grafana

Grafana runs on **http://localhost:3000**.

1. Open **http://localhost:3000**.
2. Log in: **admin** / **admin** (change password if prompted).
3. **Loki (logs from backend):**
   - A Loki data source is **provisioned automatically** (see `grafana/provisioning/datasources/loki.yml`) with URL **http://loki:3100** so Grafana (in Docker) can reach Loki on the same network.
   - Go to **Explore**, select the **Loki** data source, and run a query like `{app="NatureHelp"}` to see backend logs.
   - If you add Loki manually, the URL **must** be **http://loki:3100** (not `http://localhost:3100`), because from inside the Grafana container `localhost` is the container itself.
4. **Elasticsearch (optional):**
   - Add data source **Elasticsearch**: URL **http://elasticsearch:9200** (from Grafana container). If Grafana runs in the same Docker network, no auth is needed (security is off in your compose).
   - In Explore, select Elasticsearch and query your `naturehelp-logs-*` index to confirm you see the same logs as in Kibana.

---

## 5. End-to-End Log Test

1. Start stack: `docker compose --profile logging up -d`.
2. Wait for Elasticsearch to be healthy: `curl http://localhost:9200/_cluster/health?pretty`.
3. Open the app: http://localhost:5051/nature-help/ and use it (login, navigate, trigger API calls).
4. **Kibana:** Open http://localhost:5601 → Discover → select `naturehelp-logs-*` → set time range to “Last 15 minutes” → you should see log entries.
5. **Grafana:** Open http://localhost:3000 → Explore → Loki (or Elasticsearch if configured) → run query → you should see the same kind of logs.

---

## 6. URLs Summary

| Service | URL | Notes |
|--------|-----|--------|
| Elasticsearch | http://localhost:9200 | API only; use Kibana or Grafana for UI. |
| Kibana | http://localhost:5601 | No default login (security off in compose). |
| Grafana | http://localhost:3000 | admin / admin. |
| Loki | http://localhost:3100 | Used by Grafana; backend sends logs here. |
| Seq | http://localhost:5341 | Alternative log UI (also receives backend logs). |

---

## 7. Stop Logging Stack

```powershell
docker compose --profile logging down
```

To remove volumes as well (deletes Elasticsearch/Kibana/Grafana data):

```powershell
docker compose --profile logging down -v
```

---

## Troubleshooting

- **“Unable to connect with Loki” in Grafana:** Grafana runs inside Docker, so it must use the Loki **service name**, not `localhost`. Use URL **http://loki:3100** for the Loki data source. The project now provisions Loki automatically; restart the stack so Grafana loads it: `docker compose --profile logging up -d --force-recreate grafana`. If you previously added Loki with `http://localhost:3100`, edit the data source (Connections → Data sources → Loki) and set URL to `http://loki:3100`, then Save & test.
- **Kibana “Unable to connect to Elasticsearch”:** Wait 1–2 minutes after `docker compose up`; Elasticsearch starts slowly. Check `docker compose logs elasticsearch`.
- **No logs in Kibana Discover:** Ensure the backend is running and the index pattern/data view uses `naturehelp-logs-*` and a correct time field; expand the time range.
- **Grafana cannot reach Loki/Elasticsearch:** From inside Docker always use `http://loki:3100` and `http://elasticsearch:9200`. Never use `localhost` in datasource URLs when Grafana runs in a container.
