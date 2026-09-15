mergeInto(LibraryManager.library, {
  $mtPlatform: { sdk: null, initializing: false },
  MT_Initialize__deps: ['$mtPlatform'],
  MT_Initialize: function () {
    if (mtPlatform.initializing) return;
    mtPlatform.initializing = true;
    var notify = function (method, value) { SendMessage('YandexPlatform', method, value); };
    var visibility = function () {
      notify('OnVisibility', document.hidden ? '1' : '0');
    };
    document.addEventListener('visibilitychange', visibility);
    window.addEventListener('blur', function () { notify('OnVisibility', '1'); });
    window.addEventListener('focus', visibility);
    visibility();
    var script = document.createElement('script');
    script.src = '/sdk.js';
    script.onload = function () {
      YaGames.init().then(function (sdk) {
        mtPlatform.sdk = sdk;
        sdk.on('game_api_pause', function () { notify('OnPlatformPause', '1'); });
        sdk.on('game_api_resume', function () { notify('OnPlatformPause', '0'); });
        var canvas = document.getElementById('unity-canvas');
        if (canvas && sdk.deviceInfo && (sdk.deviceInfo.isMobile() || sdk.deviceInfo.isTablet())) {
          canvas.addEventListener('pointerdown', function () {
            if (sdk.screen.fullscreen.status !== 'on') {
              try {
                Promise.resolve(sdk.screen.fullscreen.request()).catch(function (error) {
                  console.warn('Fullscreen unavailable:', error);
                });
              } catch (error) { console.warn('Fullscreen unavailable:', error); }
            }
          }, { once: true });
        }
        notify('OnSdkInitialized', sdk.environment.i18n.lang);
      }).catch(function (error) { notify('OnSdkError', String(error)); });
    };
    script.onerror = function () { notify('OnSdkError', 'Cannot load /sdk.js. Use the Yandex draft or SDK proxy for testing.'); };
    document.head.appendChild(script);
  },
  MT_Ready__deps: ['$mtPlatform'],
  MT_Ready: function () { mtPlatform.sdk.features.LoadingAPI.ready(); },
  MT_Rewarded__deps: ['$mtPlatform'],
  MT_Rewarded: function () {
    var rewarded = false;
    var finished = false;
    var finish = function () {
      if (finished) return;
      finished = true;
      SendMessage('YandexPlatform', 'OnAdFinished', rewarded ? '1' : '0');
    };
    try {
      mtPlatform.sdk.adv.showRewardedVideo({ callbacks: {
        onOpen: function () {},
        onRewarded: function () { if (!finished) rewarded = true; },
        onClose: finish,
        onError: function (error) { console.warn('Rewarded video:', error); finish(); }
      } });
    } catch (error) { console.warn('Rewarded video:', error); finish(); }
  }
});
