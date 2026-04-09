# Logging stack: Elasticsearch, Kibana, Grafana (and Loki, Seq)

This project can send application logs to several backends. The Docker setup includes a **logging** profile that runs Elasticsearch, Kibana, Grafana, Loki, Seq, and Prometheus.

## What each service is

| Service | What it is | Role in this project |
|--------|-------------|------------------------|
| **Elasticsearch** | Search/analytics engine that stores documents (e.g. logs) and lets you query them. | Stores Serilog logs from the NatureHelp backend. You query and analyze logs here (or via Kibana). |
| **Kibana** | Web UI for Elasticsearch. | Lets you search, filter, and visualize logs stored in Elasticsearch (e.g. by time, level, message). |
| **Grafana** | Dashboard and visualization platform. Connects to many data sources (Prometheus, Loki, Elasticsearch, etc.). | Used to build dashboards; in this setup it is preconfigured to use **Loki** for logs. You can also add Elasticsearch or Prometheus as data sources. |
| **Loki** | Log aggregation system (by Grafana Labs), optimized for storing and querying log streams. | Receives Serilog logs from the backend. Grafana queries Loki to show logs in dashboards. |
| **Seq** | Structured log server with a search UI. | Receives Serilog logs; useful for development and debugging with a simple UI. |
| **Prometheus** | Metrics database and scraping system. | Can scrape metrics (e.g. from the app or .NET); Grafana can use it for metric dashboards. |

In short: the **backend writes logs** to Elasticsearch, Loki, and Seq. You **inspect Elasticsearch** with **Kibana**, and **inspect Loki (and optionally others)** with **Grafana**.

---

## How to run the logging stack

All of these services are started with the Docker Compose **logging** profile. Start the full app (including backend) so that the API sends logs into the stack:

```bash
docker compose --profile logging up -d
```

This starts postgres, redis, backend, frontend, and the logging services: Seq, Loki, Grafana, Elasticsearch, Kibana, Prometheus. The backend is already configured (via env in `docker-compose.yml`) to send logs to `http://seq`, `http://elasticsearch:9200`, and `http://loki:3100`.

If you only want the logging stack (e.g. you run backend locally), start the logging profile and the dependencies that share the same network:

```bash
docker compose --profile logging up -d
```

Then ensure your backend can reach:

- Seq: `http://localhost:5341`
- Elasticsearch: `http://localhost:9200`
- Loki: `http://localhost:3100`

(`appsettings.Development.json` uses these localhost URLs when you run the backend on the host.)

---

## How to test each one

### 1. Elasticsearch

- **URL:** http://localhost:9200  
- **Quick check:** Open in browser or run:
  ```bash
  curl http://localhost:9200
  ```
  You should see JSON with `tagline: "You Know, for Search"` and cluster info.
- **Check indices (logs):** After the backend has run and sent logs:
  ```bash
  curl http://localhost:9200/_cat/indices?v
  ```
  Look for indices like `naturehelp-logs-*` (pattern is set in Serilog/Elasticsearch sink).

### 2. Kibana

- **URL:** http://localhost:5601  
- **Quick check:** Open in browser. Kibana may take a minute to start.
- **Test with your project logs:**
  1. Go to **Management** → **Stack Management** → **Index Patterns** (or **Data Views** in newer Kibana).
  2. Create an index pattern / data view, e.g. `naturehelp-logs-*`, with a time field (e.g. `@timestamp`).
  3. Go to **Analytics** → **Discover**. Select the `naturehelp-logs-*` data view and a time range. You should see log events from the backend.

### 3. Grafana

- **URL:** http://localhost:3000  
- **Login:** `admin` / `admin` (change when prompted).
- **Quick check:** Log in and open **Connections** → **Data sources**. Loki should be configured (e.g. `http://loki:3100` when running in Docker).
- **Test with your project logs (Loki):**
  1. Go to **Explore** (compass icon).
  2. Choose data source **Loki**.
  3. Run a query, e.g. `{application="NatureHelp"}` or `{job="naturehelp"}` (labels depend on Serilog Loki sink config). You should see log lines from the backend.
- **Optional – add Elasticsearch in Grafana:**  
  Add a data source: type **Elasticsearch**, URL `http://elasticsearch:9200` (when in Docker). Then you can build dashboards from Elasticsearch indices (e.g. `naturehelp-logs-*`) or keep using Loki for logs.

### 4. Seq (optional)

- **URL:** http://localhost:5341  
- No login by default (see `SEQ_FIRSTRUN_NOAUTHENTICATION=true` in compose).
- **Quick check:** Open in browser; you should see the Seq UI and events once the backend has sent logs.

---

## Summary: one command to run and test

```bash
docker compose --profile logging up -d
```

Wait ~1–2 minutes for Elasticsearch and Kibana to be healthy, then:

| What you want to test | Where to go |
|-----------------------|-------------|
| Elasticsearch is up   | http://localhost:9200 or `curl http://localhost:9200` |
| Kibana (search logs)  | http://localhost:5601 → create data view `naturehelp-logs-*` → Discover |
| Grafana (Loki logs)   | http://localhost:3000 → login admin/admin → Explore → Loki → query `{application="NatureHelp"}` |
| Seq (structured logs)| http://localhost:5341 |

Generate some traffic (e.g. open the app at http://localhost:5051/nature-help/ or call the API) so the backend writes logs; then refresh Kibana Discover, Grafana Explore, or Seq to see them.
