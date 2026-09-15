// node Tests/WebGL/verify-build.cjs <fresh build directory>
const fs = require('node:fs');
const path = require('node:path');
const assert = require('node:assert/strict');
const root = path.resolve(process.argv[2]);
const files = [];
function walk(directory) {
  for (const entry of fs.readdirSync(directory, { withFileTypes: true })) {
    const full = path.join(directory, entry.name);
    assert(!entry.isSymbolicLink(), 'Release must not contain symlinks');
    if (entry.isDirectory()) walk(full);
    else files.push({ name: path.relative(root, full).replaceAll('\\', '/'), bytes: fs.statSync(full).size });
  }
}
walk(root);
assert(files.some(f => f.name === 'index.html'), 'Missing root index.html');
assert(files.every(f => !/[\s\u0400-\u04ff]/u.test(f.name)), 'Space or Cyrillic in a filename');
assert(files.every(f => !/\.(zip|cs|csproj|meta|log)$/i.test(f.name)), 'Unexpected development/archive file');
const bytes = files.reduce((sum, file) => sum + file.bytes, 0);
assert(bytes <= 100000000, 'Unarchived files exceed 100 MB');
const html = fs.readFileSync(path.join(root, 'index.html'), 'utf8');
const referenced = [...html.matchAll(/["'](Build\/[^"']+)["']/g)].map(m => m[1]);
assert(referenced.length >= 4, 'Missing Unity loader/data/framework/wasm URLs');
for (const file of referenced) assert(files.some(f => f.name === file), `Missing ${file}`);
const loaders = files.filter(f => f.name.endsWith('.loader.js'));
assert.equal(loaders.length, 1, 'Build directory contains old loaders');
console.log(JSON.stringify({ result: 'PASS', bytes, files, referenced }, null, 2));
