import { readFileSync, writeFileSync, existsSync } from 'node:fs';
import { join, basename, dirname } from 'node:path';
import { spawnSync } from 'node:child_process';
import { fileURLToPath } from 'node:url';

const PROJECT_ROOT = 'd:/MyProjects';
const SKILL_DIR =
  'C:/Users/rabin/.cursor/plugins/cache/understand-anything/understand-anything/09ede1917ffd043e6d5bbc8a80b45760814c2d7f/skills/understand';
const EXTRACT_SCRIPT = join(SKILL_DIR, 'extract-structure.mjs');
const batches = JSON.parse(
  readFileSync(join(PROJECT_ROOT, '.understand-anything/intermediate/batches.json'), 'utf8'),
);

function nodeTypeForFile(file) {
  const p = file.path.replace(/\\/g, '/');
  if (file.fileCategory === 'docs') return 'document';
  if (file.fileCategory === 'config') return 'config';
  if (file.fileCategory === 'data') {
    if (p.endsWith('.sql')) return 'table';
    return 'schema';
  }
  if (file.fileCategory === 'infra') {
    if (p.includes('.github/workflows') || p.endsWith('Jenkinsfile')) return 'pipeline';
    if (p.endsWith('.tf') || p.endsWith('.tfvars')) return 'resource';
    if (p.includes('Dockerfile') || p.includes('docker-compose')) return 'service';
    return 'service';
  }
  return 'file';
}

function nodeId(type, path) {
  const prefix =
    type === 'file'
      ? 'file'
      : type === 'document'
        ? 'document'
        : type === 'config'
          ? 'config'
          : type === 'service'
            ? 'service'
            : type === 'pipeline'
              ? 'pipeline'
              : type === 'resource'
                ? 'resource'
                : type === 'table'
                  ? 'table'
                  : type === 'schema'
                    ? 'schema'
                    : 'file';
  return `${prefix}:${path}`;
}

function inferTags(file, result) {
  const p = file.path.replace(/\\/g, '/').toLowerCase();
  const tags = new Set();
  if (p.includes('test') || p.includes('.spec.') || p.includes('.cy.')) tags.add('test');
  if (p.includes('controller')) tags.add('api-handler');
  if (p.includes('service')) tags.add('service');
  if (p.includes('component')) tags.add('component');
  if (p.includes('repository')) tags.add('data-model');
  if (p.includes('migration')) tags.add('migration');
  if (p.includes('dockerfile') || p.includes('docker-compose')) tags.add('infrastructure');
  if (p.includes('.github/workflows')) tags.add('ci-cd');
  if (p.endsWith('program.cs') || p.endsWith('main.ts') || p.endsWith('app.component.ts'))
    tags.add('entry-point');
  if (file.fileCategory === 'docs') tags.add('documentation');
  if (file.fileCategory === 'config') tags.add('configuration');
  if ((result?.functions?.length || 0) > 5) tags.add('utility');
  if (tags.size === 0) tags.add(file.fileCategory === 'code' ? 'module' : file.fileCategory);
  return [...tags].slice(0, 5);
}

function inferComplexity(nonEmptyLines = 0) {
  if (nonEmptyLines < 50) return 'simple';
  if (nonEmptyLines < 200) return 'moderate';
  return 'complex';
}

function buildSummary(file, result) {
  const name = basename(file.path);
  const fn = result?.functions?.length || 0;
  const cls = result?.classes?.length || 0;
  if (file.fileCategory === 'docs')
    return `Documentation file (${name}) describing project or feature behavior.`;
  if (file.fileCategory === 'config')
    return `Configuration for ${dirname(file.path).replace(/\\/g, '/')} build or runtime settings.`;
  if (file.fileCategory === 'infra')
    return `Infrastructure definition for deployment, CI/CD, or container orchestration (${name}).`;
  if (cls > 0 && fn > 0)
    return `${name} defines ${cls} class(es) and ${fn} function(s) in the ${file.language} layer.`;
  if (cls > 0) return `${name} defines ${cls} class(es) for the application domain.`;
  if (fn > 0) return `${name} provides ${fn} function(s) used by surrounding modules.`;
  return `${name} is a ${file.language} ${file.fileCategory} module in NatureHelp.`;
}

function resultToGraph(batch, extractOutput) {
  const nodes = [];
  const edges = [];
  const batchImportData = batch.batchImportData || {};

  for (const file of batch.files) {
    const result = extractOutput.results.find((r) => r.path === file.path);
    const type = nodeTypeForFile(file);
    const id = nodeId(type, file.path);
    nodes.push({
      id,
      type,
      name: basename(file.path),
      filePath: file.path,
      summary: buildSummary(file, result),
      tags: inferTags(file, result),
      complexity: inferComplexity(result?.nonEmptyLines || file.sizeLines),
    });

    if (file.fileCategory === 'code' || file.fileCategory === 'script') {
      for (const imp of batchImportData[file.path] || []) {
        edges.push({
          source: `file:${file.path}`,
          target: `file:${imp}`,
          type: 'imports',
          direction: 'forward',
          weight: 0.7,
        });
      }
    }

    for (const fn of result?.functions || []) {
      const lineSpan = (fn.endLine || fn.startLine) - fn.startLine + 1;
      const exported = (result.exports || []).some((e) => e.name === fn.name);
      if (!exported && lineSpan < 10) continue;
      const fnId = `function:${file.path}:${fn.name}`;
      nodes.push({
        id: fnId,
        type: 'function',
        name: fn.name,
        filePath: file.path,
        summary: `Function ${fn.name} in ${basename(file.path)}.`,
        tags: ['function'],
        complexity: lineSpan < 30 ? 'simple' : lineSpan < 80 ? 'moderate' : 'complex',
      });
      edges.push({
        source: id,
        target: fnId,
        type: 'contains',
        direction: 'forward',
        weight: 1.0,
      });
      if (exported) {
        edges.push({
          source: id,
          target: fnId,
          type: 'exports',
          direction: 'forward',
          weight: 0.8,
        });
      }
    }

    for (const cls of result?.classes || []) {
      const lineSpan = (cls.endLine || cls.startLine) - cls.startLine + 1;
      const methodCount = cls.methods?.length || 0;
      const exported = (result.exports || []).some((e) => e.name === cls.name);
      if (!exported && methodCount < 2 && lineSpan < 20) continue;
      const clsId = `class:${file.path}:${cls.name}`;
      nodes.push({
        id: clsId,
        type: 'class',
        name: cls.name,
        filePath: file.path,
        summary: `Class ${cls.name} with ${methodCount} method(s) in ${basename(file.path)}.`,
        tags: ['class', cls.name.toLowerCase().includes('service') ? 'service' : 'data-model'],
        complexity: lineSpan < 50 ? 'simple' : lineSpan < 150 ? 'moderate' : 'complex',
      });
      edges.push({
        source: id,
        target: clsId,
        type: 'contains',
        direction: 'forward',
        weight: 1.0,
      });
      if (exported) {
        edges.push({
          source: id,
          target: clsId,
          type: 'exports',
          direction: 'forward',
          weight: 0.8,
        });
      }
    }

    for (const step of result?.steps || []) {
      const stepId = `step:${file.path}:${step.name}`;
      nodes.push({
        id: stepId,
        type: 'pipeline',
        name: step.name,
        filePath: file.path,
        summary: `CI/CD step ${step.name} in ${basename(file.path)}.`,
        tags: ['ci-cd', 'deployment'],
        complexity: 'simple',
      });
      edges.push({
        source: id,
        target: stepId,
        type: 'contains',
        direction: 'forward',
        weight: 1.0,
      });
    }

    for (const svc of result?.services || []) {
      const svcId = `service:${file.path}:${svc.name}`;
      nodes.push({
        id: svcId,
        type: 'service',
        name: svc.name,
        filePath: file.path,
        summary: `Service ${svc.name} defined in ${basename(file.path)}.`,
        tags: ['infrastructure', 'containerization'],
        complexity: 'moderate',
      });
      edges.push({
        source: id,
        target: svcId,
        type: 'contains',
        direction: 'forward',
        weight: 1.0,
      });
    }
  }

  return { nodes, edges };
}

for (const batch of batches.batches) {
  const idx = batch.batchIndex;
  const inputPath = join(PROJECT_ROOT, `.understand-anything/tmp/ua-file-analyzer-input-${idx}.json`);
  const extractPath = join(PROJECT_ROOT, `.understand-anything/tmp/ua-file-extract-results-${idx}.json`);
  const outputPath = join(PROJECT_ROOT, `.understand-anything/intermediate/batch-${idx}.json`);

  writeFileSync(
    inputPath,
    JSON.stringify({
      projectRoot: PROJECT_ROOT,
      batchFiles: batch.files,
      batchImportData: batch.batchImportData || {},
    }),
  );

  const proc = spawnSync('node', [EXTRACT_SCRIPT, inputPath, extractPath], {
    encoding: 'utf8',
    maxBuffer: 50 * 1024 * 1024,
  });
  if (proc.status !== 0) {
    console.error(`Batch ${idx} extract failed:`, proc.stderr);
    continue;
  }
  if (!existsSync(extractPath)) {
    console.error(`Batch ${idx} missing extract output`);
    continue;
  }

  const extractOutput = JSON.parse(readFileSync(extractPath, 'utf8'));
  const graph = resultToGraph(batch, extractOutput);
  writeFileSync(outputPath, JSON.stringify(graph, null, 2));
  console.error(
    `Batch ${idx}/${batches.totalBatches}: ${graph.nodes.length} nodes, ${graph.edges.length} edges`,
  );
}

console.error('All batches processed');
