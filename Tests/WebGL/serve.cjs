// Local smoke-test server. Never include this file in the uploaded game archive.
// node Tests/WebGL/serve.cjs <build directory> [port]
const http = require('node:http');
const fs = require('node:fs');
const path = require('node:path');
const root = fs.realpathSync(process.argv[2]);
const port = Number(process.argv[3] || 8089);
const mime = { '.html': 'text/html; charset=utf-8', '.js': 'application/javascript',
  '.wasm': 'application/wasm', '.json': 'application/json', '.png': 'image/png' };
http.createServer((req, res) => {
  let filename;
  try {
    const pathname = decodeURIComponent(new URL(req.url, 'http://localhost').pathname);
    filename = path.resolve(root, '.' + (pathname === '/' ? '/index.html' : pathname));
    if (!filename.startsWith(root + path.sep)) throw Error('Outside root');
    filename = fs.realpathSync(filename);
    if (!filename.startsWith(root + path.sep) || !fs.statSync(filename).isFile()) throw Error('Not a file');
  } catch {
    res.writeHead(404); res.end('Not found. SDK integration requires the Yandex draft.'); return;
  }
  res.writeHead(200, { 'Content-Type': mime[path.extname(filename)] || 'application/octet-stream',
    'Cache-Control': 'no-store' });
  fs.createReadStream(filename).pipe(res);
}).listen(port, '127.0.0.1', () => console.log(`Serving ${root} at http://127.0.0.1:${port}`));
