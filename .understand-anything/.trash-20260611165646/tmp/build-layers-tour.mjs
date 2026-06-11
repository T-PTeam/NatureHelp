import { readFileSync, writeFileSync } from 'node:fs';
import { join } from 'node:path';

const PROJECT_ROOT = 'd:/MyProjects';
const graph = JSON.parse(
  readFileSync(join(PROJECT_ROOT, '.understand-anything/intermediate/assembled-graph.json'), 'utf8'),
);
const scan = JSON.parse(
  readFileSync(join(PROJECT_ROOT, '.understand-anything/intermediate/scan-result.json'), 'utf8'),
);

const fileLevelTypes = new Set([
  'file',
  'config',
  'document',
  'service',
  'pipeline',
  'table',
  'schema',
  'resource',
  'endpoint',
]);

const fileNodes = graph.nodes.filter((n) => fileLevelTypes.has(n.type));
const nodeIds = new Set(graph.nodes.map((n) => n.id));

function assignLayer(n) {
  const p = (n.filePath || n.id.split(':').slice(1).join(':')).replace(/\\/g, '/');
  if (p.startsWith('src/View/nature-help/')) return 'layer:frontend';
  if (p.startsWith('src/NatureHelp/Controllers/')) return 'layer:api-controllers';
  if (p.startsWith('src/NatureHelp/')) return 'layer:api-host';
  if (p.startsWith('src/Application/')) return 'layer:application';
  if (p.startsWith('src/Domain/')) return 'layer:domain';
  if (p.startsWith('src/Infrastructure/')) return 'layer:infrastructure';
  if (p.startsWith('src/Shared/')) return 'layer:shared';
  if (p.startsWith('src/Tests/')) return 'layer:tests';
  if (p.startsWith('src/Postman/')) return 'layer:api-testing';
  if (p.startsWith('.github/') || p.includes('docker-compose') || p === 'Dockerfile' || p.startsWith('grafana/') || p.startsWith('prometheus'))
    return 'layer:devops';
  if (p.startsWith('docs/') || p.endsWith('.md')) return 'layer:documentation';
  return 'layer:root-config';
}

const layerMap = new Map([
  ['layer:frontend', { id: 'layer:frontend', name: 'Frontend (Angular)', description: 'Angular SPA UI, modules, services, and components.', nodeIds: [] }],
  ['layer:api-controllers', { id: 'layer:api-controllers', name: 'API Controllers', description: 'ASP.NET Core HTTP endpoints exposing REST APIs.', nodeIds: [] }],
  ['layer:api-host', { id: 'layer:api-host', name: 'API Host', description: 'Program startup, DI registration, middleware, and host configuration.', nodeIds: [] }],
  ['layer:application', { id: 'layer:application', name: 'Application Services', description: 'Business logic, DTOs, and service interfaces.', nodeIds: [] }],
  ['layer:domain', { id: 'layer:domain', name: 'Domain Models', description: 'Core entities, enums, and domain contracts.', nodeIds: [] }],
  ['layer:infrastructure', { id: 'layer:infrastructure', name: 'Infrastructure', description: 'EF Core, repositories, migrations, and external integrations.', nodeIds: [] }],
  ['layer:shared', { id: 'layer:shared', name: 'Shared DTOs', description: 'Cross-cutting DTOs shared between layers.', nodeIds: [] }],
  ['layer:tests', { id: 'layer:tests', name: 'Tests', description: 'Unit and integration tests.', nodeIds: [] }],
  ['layer:api-testing', { id: 'layer:api-testing', name: 'API Testing (Postman)', description: 'Postman collections and environments.', nodeIds: [] }],
  ['layer:devops', { id: 'layer:devops', name: 'DevOps & Infrastructure', description: 'Docker, CI/CD workflows, and observability configs.', nodeIds: [] }],
  ['layer:documentation', { id: 'layer:documentation', name: 'Documentation', description: 'Project and operational documentation.', nodeIds: [] }],
  ['layer:root-config', { id: 'layer:root-config', name: 'Root Configuration', description: 'Top-level configs and tooling.', nodeIds: [] }],
]);

for (const n of fileNodes) {
  const layerId = assignLayer(n);
  layerMap.get(layerId).nodeIds.push(n.id);
}

const layers = [...layerMap.values()].filter((l) => l.nodeIds.length > 0);

const pick = (predicate) => fileNodes.find(predicate)?.id;

const tour = [
  {
    order: 1,
    title: 'Project Overview',
    description: 'Start with the README to understand NatureHelp purpose, stack, and architecture.',
    nodeIds: [pick((n) => n.id === 'document:README.md') || 'document:README.md'].filter((id) => nodeIds.has(id)),
  },
  {
    order: 2,
    title: 'API Entry Point',
    description: 'Program.cs bootstraps the ASP.NET Core host, configures middleware, and wires dependency injection.',
    nodeIds: ['file:src/NatureHelp/Program.cs'].filter((id) => nodeIds.has(id)),
  },
  {
    order: 3,
    title: 'Dependency Injection',
    description: 'DependencyInjection.cs registers application, infrastructure, and cross-cutting services.',
    nodeIds: ['file:src/NatureHelp/DependencyInjection.cs'].filter((id) => nodeIds.has(id)),
  },
  {
    order: 4,
    title: 'User & Auth API',
    description: 'UserController handles login, registration, organization users, and profile endpoints.',
    nodeIds: ['file:src/NatureHelp/Controllers/Organization/UserController.cs'].filter((id) => nodeIds.has(id)),
  },
  {
    order: 5,
    title: 'Environmental Deficiencies',
    description: 'Water and soil deficiency controllers and services manage environmental monitoring data.',
    nodeIds: [
      'file:src/NatureHelp/Controllers/Nature/WaterDeficiencyController.cs',
      'file:src/NatureHelp/Controllers/Nature/SoilDeficiencyController.cs',
    ].filter((id) => nodeIds.has(id)),
  },
  {
    order: 6,
    title: 'Domain Models',
    description: 'Domain layer defines users, organizations, laboratories, and deficiency entities.',
    nodeIds: [
      'file:src/Domain/Models/Organization/User.cs',
      'file:src/Domain/Models/Nature/WaterDeficiency.cs',
    ].filter((id) => nodeIds.has(id)),
  },
  {
    order: 7,
    title: 'Data Access',
    description: 'Infrastructure repositories and ApplicationContext provide PostgreSQL persistence via EF Core.',
    nodeIds: [
      'file:src/Infrastructure/Data/ApplicationContext.cs',
      'file:src/Infrastructure/Repositories/BaseRepository.cs',
    ].filter((id) => nodeIds.has(id)),
  },
  {
    order: 8,
    title: 'Angular Bootstrap',
    description: 'The frontend Angular app boots from app.component and routes into feature modules.',
    nodeIds: [
      'file:src/View/nature-help/src/app/app.component.ts',
      'file:src/View/nature-help/src/app/app-routing.module.ts',
    ].filter((id) => nodeIds.has(id)),
  },
  {
    order: 9,
    title: 'Deficiency UI',
    description: 'Water and soil deficiency tables and detail forms are core user-facing monitoring screens.',
    nodeIds: [
      'file:src/View/nature-help/src/app/modules/water-deficiency/components/water-deficiency-table/water-deficiency-table.component.ts',
      'file:src/View/nature-help/src/app/modules/soil-deficiency/components/soil-deficiency-table/soil-deficiency-table.component.ts',
    ].filter((id) => nodeIds.has(id)),
  },
  {
    order: 10,
    title: 'Docker & Deployment',
    description: 'docker-compose.yml orchestrates API, frontend, database, and observability stack for local and production.',
    nodeIds: ['service:docker-compose.yml', 'pipeline:.github/workflows/deploy-production-droplet.yml'].filter((id) =>
      nodeIds.has(id),
    ),
  },
].filter((s) => s.nodeIds.length > 0);

const knowledgeGraph = {
  version: '1.0.0',
  project: {
    name: scan.name || 'NatureHelp',
    languages: scan.languages || [],
    frameworks: scan.frameworks || [],
    description: scan.description || '',
    analyzedAt: new Date().toISOString(),
    gitCommitHash: '5e5671426daec0c743b9a8834cc6e844a450ccbe',
  },
  nodes: graph.nodes,
  edges: graph.edges,
  layers,
  tour,
};

writeFileSync(
  join(PROJECT_ROOT, '.understand-anything/intermediate/assembled-graph.json'),
  JSON.stringify(knowledgeGraph, null, 2),
);
writeFileSync(join(PROJECT_ROOT, '.understand-anything/intermediate/layers.json'), JSON.stringify(layers, null, 2));
writeFileSync(join(PROJECT_ROOT, '.understand-anything/intermediate/tour.json'), JSON.stringify(tour, null, 2));
console.log('layers', layers.length, 'tour steps', tour.length);
