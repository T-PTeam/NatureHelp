# Development Seed Data

This document describes demo data inserted by `DevelopmentDatabaseSeeder` when the API runs with `ASPNETCORE_ENVIRONMENT=Development` (local `dotnet run`, Docker Compose, or IDE Development profile).

Source files:

- [`DevelopmentDatabaseSeeder.cs`](../src/Infrastructure/Data/DevelopmentDatabaseSeeder.cs)
- [`DevelopmentSeedData.cs`](../src/Infrastructure/Data/DevelopmentSeedData.cs)

## When seed data is applied

| Scenario | Behavior |
|----------|----------|
| First startup on empty database | Core organizations, laboratories, users, reports, and 6 soil + 6 water deficiencies are inserted. Bulk records are added up to configured counts. |
| Later startups | Bulk methods top up missing records only. Core seed does **not** re-run if the primary organization already exists. |
| Production | Seeder is **not** executed. |

To see the updated core seed (new titles, users, coordinates), reset the development database:

```bash
docker compose down -v
docker compose up -d
```

Or drop and recreate `NatureHelpDB` in PostgreSQL, then restart the API.

## Configurable bulk counts

Set in `docker-compose.yml` or app configuration:

| Key | Default | Description |
|-----|---------|-------------|
| `Seed:UsersCount` | `15` | Total users (8 core + generated researchers) |
| `Seed:LaboratoriesCount` | `30` | Total laboratories (6 core + generated stations) |
| `Seed:SoilDeficienciesCount` | `50` | Total soil deficiencies |
| `Seed:WaterDeficienciesCount` | `50` | Total water deficiencies |

Roughly **85%** of generated laboratories and deficiencies are placed at distinct Ukrainian coordinates. The last three bulk laboratories use international reference sites (Berlin, Warsaw, Prague).

## Login credentials

| Email | Password | Role | Notes |
|-------|----------|------|-------|
| `valentyn@example.com` | `12341234` | Owner | Primary demo owner; used by Cypress and Postman |
| `igorzayets@example.com` | `DemoPass1!` | Owner | Black Sea & river basin organization owner |
| `kateryna.melnyk@example.com` | `DemoPass1!` | Supervisor | Dnipro basin supervisor |
| `andrii.bondarenko@example.com` | `DemoPass1!` | Researcher | Kharkiv industrial ecology |
| `olena.shevchenko@example.com` | `DemoPass1!` | Researcher | Southern Bug and coastal water |
| `mykola.koval@example.com` | `DemoPass1!` | Supervisor | Carpathian freshwater monitoring |
| `natalia.petrenko@example.com` | `DemoPass1!` | Researcher | Vinnytsia agroecology |
| `dmytro.ivanov@example.com` | `DemoPass1!` | Manager | Odesa coastal operations |
| Bulk users (`iryna.lysenko@example.com`, etc.) | `DemoPass1!` | Researcher / Supervisor | See bulk user table below |

All demo passwords satisfy the application password policy (uppercase, lowercase, digit, special character).

## Organizations

| ID | Title | Focus |
|----|-------|-------|
| `bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb` | Ukrainian Environmental Research Institute | National soil, water, and biodiversity monitoring across central and eastern Ukraine |
| `cccccccc-cccc-cccc-cccc-cccccccccccc` | Black Sea & River Basin Monitoring Alliance | Dnipro, Southern Bug, and Black Sea coastal water quality |
| `dd111111-dddd-dddd-dddd-dddddddddd11` | Carpathian Nature Protection Cooperative | Western Ukrainian rivers, forests, and mountain catchments |

## Core laboratories

| ID | Title | Location |
|----|-------|----------|
| `dddddddd-dddd-dddd-dddd-dddddddddddd` | Kyiv Soil & Agrochemistry Laboratory | 14 Hlybochytska St, Kyiv |
| `eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee` | Dnipro River Hydrochemistry Laboratory | 27 Naberezhna Peremohy, Dnipro |
| `ffffffff-ffff-ffff-ffff-ffffffffffff` | Odesa Coastal & Marine Laboratory | 4 Lanzheron Beach Rd, Odesa |
| `aa111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa` | Kharkiv Industrial Ecology Laboratory | 8 Nauky Ave, Kharkiv |
| `bb222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb` | Lviv Freshwater Monitoring Laboratory | 11 Kopernyka St, Lviv |
| `cc333333-cccc-cccc-cccc-cccccccccccc` | Vinnytsia Agroecology Field Laboratory | 45 Soborna St, Vinnytsia |

Bulk laboratories use titles such as *Poltava Steppe Ecology Station*, *Chernihiv Floodplain Research Unit*, and *Zaporizhzhia Industrial Impact Lab*, each at a separate Ukrainian map coordinate.

## Core users

| ID | Name | Email | Role | Organization | Laboratory |
|----|------|-------|------|--------------|------------|
| `11112222-3333-4444-5555-666677778888` | Valentyn Riabinchak | `valentyn@example.com` | Owner | Ukrainian Environmental Research Institute | Kyiv Soil & Agrochemistry |
| `99990000-aaaa-bbbb-cccc-ddddeeeeffff` | Ihor Zaiets | `igorzayets@example.com` | Owner | Black Sea & River Basin Monitoring Alliance | Odesa Coastal & Marine |
| `11223344-5566-7788-99aa-bbccddeeff00` | Kateryna Melnyk | `kateryna.melnyk@example.com` | Supervisor | Ukrainian Environmental Research Institute | Dnipro River Hydrochemistry |
| `22334455-6677-8899-aabb-ccddeeff0011` | Andrii Bondarenko | `andrii.bondarenko@example.com` | Researcher | Ukrainian Environmental Research Institute | Kharkiv Industrial Ecology |
| `33445566-7788-99aa-bbcc-ddeeff001122` | Olena Shevchenko | `olena.shevchenko@example.com` | Researcher | Black Sea & River Basin Monitoring Alliance | Dnipro River Hydrochemistry |
| `44556677-8899-aabb-ccdd-eeff00112233` | Mykola Koval | `mykola.koval@example.com` | Supervisor | Carpathian Nature Protection Cooperative | Lviv Freshwater Monitoring |
| `55667788-99aa-bbcc-cdd0-0eff11223344` | Natalia Petrenko | `natalia.petrenko@example.com` | Researcher | Ukrainian Environmental Research Institute | Vinnytsia Agroecology |
| `66778899-aabb-ccdd-ee00-ff1122334455` | Dmytro Ivanov | `dmytro.ivanov@example.com` | Manager | Black Sea & River Basin Monitoring Alliance | Odesa Coastal & Marine |

### Bulk users (generated when `Seed:UsersCount` > 8)

| Name | Email |
|------|-------|
| Iryna Lysenko | `iryna.lysenko@example.com` |
| Vasyl Tkachenko | `vasyl.tkachenko@example.com` |
| Hanna Moroz | `hanna.moroz@example.com` |
| Taras Savchuk | `taras.savchuk@example.com` |
| Yuliia Kravets | `yuliia.kravets@example.com` |
| Petro Hryhoriev | `petro.hryhoriev@example.com` |
| Sofiia Romaniuk | `sofiia.romaniuk@example.com` |
| Roman Danylenko | `roman.danylenko@example.com` |
| Viktoriia Ponomarenko | `viktoriia.ponomarenko@example.com` |
| Bohdan Kuzmenko | `bohdan.kuzmenko@example.com` |
| Lesia Holub | `lesia.holub@example.com` |
| Oleksandr Marchenko | `oleksandr.marchenko@example.com` |

## Core soil deficiencies

Each record has a human-readable title, field description, address, and non-overlapping coordinates (radius 2.2–4.0 km).

| ID | Title | Danger | Location |
|----|-------|--------|----------|
| `d1111111-1111-1111-1111-111111111111` | Heavy metals near Troieshchyna industrial zone | Moderate | Troieshchyna, Kyiv |
| `d2222222-2222-2222-2222-222222222222` | Nitrate buildup in Kharkiv Lisovy Park buffer soils | Critical | Lisovy Park, Kharkiv |
| `d3333333-3333-3333-3333-333333333333` | Legacy contamination at Prydniprovskyi Chemical Plant | Critical | Prydniprovskyi area, Dnipro |
| `d4444444-4444-4444-4444-444444444444` | Pesticide residues in Bila Tserkva cropland | Dangerous | Ros river meadows, Bila Tserkva |
| `d5555555-5555-5555-5555-555555555555` | Oil hydrocarbons near Konotop railway depot | Moderate | Konotop railway depot, Sumy Oblast |
| `d6666666-6666-6666-6666-666666666666` | Copper accumulation on Uzhhorod vineyard terraces | Moderate | Uzhhorod vineyard terraces |

## Core water deficiencies

| ID | Title | Danger | Location |
|----|-------|--------|----------|
| `c1111111-1111-1111-1111-111111111111` | Urban runoff at Kyiv Hydropark embankment | Moderate | Hydropark, Kyiv |
| `c2222222-2222-2222-2222-222222222222` | Agricultural nitrate pulse on Southern Bug near Voznesensk | Dangerous | Southern Bug, Voznesensk |
| `c3333333-3333-3333-3333-333333333333` | Post-storm bacterial load at Lanzheron beach | Critical | Lanzheron beach, Odesa |
| `c4444444-4444-4444-4444-444444444444` | Industrial runoff in Siverskyi Donets near Sloviansk | Dangerous | Siverskyi Donets, Sloviansk |
| `c5555555-5555-5555-5555-555555555555` | Phosphate enrichment at Danube delta boundary | Critical | Danube delta, Izmail |
| `c6666666-6666-6666-6666-666666666666` | Municipal discharge impact on Ros river near Bila Tserkva | Moderate | Ros river, Bila Tserkva |

## Bulk deficiencies

Generated soil and water deficiencies cycle through **25 Ukrainian sampling sites** (Kyiv, Kharkiv, Dnipro, Odesa, Lviv, Vinnytsia, Poltava, Chernihiv, Zaporizhzhia, Mykolaiv, Uzhhorod, Chernivtsi, and oblast-level field sites). Each bulk record:

- Uses a descriptive title template plus region name
- Includes a narrative description and `Address`
- Applies a small coordinate offset so map markers do not stack
- Uses a smaller affected radius (2–6 km) than the old seed

Example bulk soil title: *Compaction and salinity on irrigated plots — Poltava*.

Example bulk water title: *Algae bloom risk in sheltered bay — Lviv*.

## Map placement strategy

1. **Core deficiencies** — fixed coordinates at real Ukrainian landmarks, spaced across oblasts.
2. **Bulk deficiencies** — rotate through the `UkrainianLocations` list with index-based lat/lng offsets.
3. **Laboratories** — six anchor labs in major cities; bulk labs spread across Ukraine with only the last three entries abroad.

## Reports (core seed)

| Title | Topic | Reporter |
|-------|-------|----------|
| Chernozem health review for central Ukraine | Soil | Valentyn Riabinchak |
| Black Sea bathing water quality summary | Water | Ihor Zaiets |
| Dnipro basin nutrient transport model | Water | Kateryna Melnyk |

## Related docs

- [Postman collection](../src/Postman/README.md) — uses `valentyn@example.com` / `12341234` for local login
- [README](../README.md) — project quick start
