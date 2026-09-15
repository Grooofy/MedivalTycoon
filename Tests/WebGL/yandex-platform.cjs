// Run from the repository root: node Tests/WebGL/yandex-platform.cjs
const fs = require('node:fs');
const vm = require('node:vm');
const assert = require('node:assert/strict');
const messages = [];
const events = {};
let script, callbacks, ready = 0;
const sdk = {
  environment: { i18n: { lang: 'ru' } },
  features: { LoadingAPI: { ready() { ready++; } } },
  on(event, callback) { events[event] = callback; },
  adv: { showRewardedVideo(value) { callbacks = value.callbacks; } }
};
const library = {};
const context = {
  LibraryManager: { library }, mergeInto: Object.assign,
  SendMessage: (...args) => messages.push(args), console,
  document: { hidden: false, hasFocus: () => true, addEventListener() {},
    getElementById: () => null,
    createElement: () => ({}), head: { appendChild(value) { script = value; } } },
  window: { addEventListener() {} }, YaGames: { init: async () => sdk }
};
vm.createContext(context);
vm.runInContext(fs.readFileSync('Assets/Plugins/YandexPlatform.jslib', 'utf8'), context);
context.mtPlatform = library.$mtPlatform;
(async () => {
  library.MT_Initialize();
  assert.equal(script.src, '/sdk.js');
  script.onload();
  await new Promise(resolve => setImmediate(resolve));
  assert(messages.some(m => m[1] === 'OnSdkInitialized' && m[2] === 'ru'));
  library.MT_Ready(); assert.equal(ready, 1);
  events.game_api_pause(); events.game_api_resume();
  assert.deepEqual(messages.slice(-2).map(m => m[2]), ['1', '0']);
  messages.length = 0;
  library.MT_Rewarded(); callbacks.onRewarded();
  assert.equal(messages.length, 0, 'Do not resume while ad is open');
  callbacks.onClose(); callbacks.onClose(); callbacks.onRewarded();
  assert.deepEqual(messages.map(m => m[2]), ['1'], 'Reward exactly once');
  messages.length = 0;
  library.MT_Rewarded(); callbacks.onClose(); callbacks.onRewarded();
  assert.deepEqual(messages.map(m => m[2]), ['0'], 'No reward after early close');
  messages.length = 0;
  library.MT_Rewarded(); callbacks.onError('test failure'); callbacks.onClose();
  assert.deepEqual(messages.map(m => m[2]), ['0'], 'Error completes only once');
  messages.length = 0;
  sdk.adv.showRewardedVideo = () => { throw Error('test exception'); };
  library.MT_Rewarded();
  assert.deepEqual(messages.map(m => m[2]), ['0']);
  let fullscreenRequests = 0, firstTouch;
  context.mtPlatform.initializing = false;
  context.document.getElementById = () => ({ addEventListener(event, handler, options) {
    assert.equal(event, 'pointerdown'); assert.equal(options.once, true); firstTouch = handler;
  } });
  sdk.deviceInfo = { isMobile: () => true, isTablet: () => false };
  sdk.screen = { fullscreen: { status: 'off', request() {
    fullscreenRequests++; return Promise.reject(Error('browser denied fullscreen'));
  } } };
  library.MT_Initialize(); script.onload();
  await new Promise(resolve => setImmediate(resolve));
  assert.equal(fullscreenRequests, 0, 'Fullscreen requires user interaction');
  firstTouch();
  await new Promise(resolve => setImmediate(resolve));
  assert.equal(fullscreenRequests, 1, 'A mobile touch requests fullscreen');
  console.log('PASS: initialization, ready, platform events, reward/close/error/duplicate callbacks');
  console.log('PASS: mobile fullscreen waits for touch and tolerates browser denial');
})().catch(error => { console.error(error); process.exitCode = 1; });
