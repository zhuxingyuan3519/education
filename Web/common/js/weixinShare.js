 (function () {
 var onBridgeReady = function () {
        // 发送给好友;
        WeixinJSBridge.on('menu:share:appmessage', function (argv) {
            WeixinJSBridge.invoke('sendAppMessage', {
                "appid": dataForWeixin.appId,
                "img_url": dataForWeixin.img,
                "img_width": "120",
                "img_height": "120",
                "link": dataForWeixin.url,
                "desc": dataForWeixin.desc,
                "title": dataForWeixin.title
            }, function (res) {
                alert(res);
            });
        });
        // 分享到朋友圈;
        WeixinJSBridge.on('menu:share:timeline', function (argv) {
            //$.post("view.aspx", { cmd: "share", txt: document.title + "(分享到朋友圈)" });
            WeixinJSBridge.invoke('shareTimeline', {
                "img_url": dataForWeixin.img,
                "img_width": "120",
                "img_height": "120",
                "link": dataForWeixin.url,
                "desc": dataForWeixin.desc,
                "title": dataForWeixin.title + ' ' + dataForWeixin.desc
            }, function (res) {
                //alert(res.err_msg);
            });
        });
        // 分享到微博;
        WeixinJSBridge.on('menu:share:weibo', function (argv) {
            WeixinJSBridge.invoke('shareWeibo', {
                "content": dataForWeixin.title + dataForWeixin.desc + ' ' + dataForWeixin.url,
                "url": dataForWeixin.url
            }, function (res) {
                
            });
        });
        // 分享到Facebook
        WeixinJSBridge.on('menu:share:facebook', function (argv) {
            WeixinJSBridge.invoke('shareFB', {
                "img_url": dataForWeixin.img,
                "img_width": "120",
                "img_height": "120",
                "link": dataForWeixin.url,
                "desc": dataForWeixin.desc,
                "title": dataForWeixin.title
            }, function (res) {
                
            });
        });
    };

    if (document.addEventListener) {
        document.addEventListener('WeixinJSBridgeReady', onBridgeReady, false);
        document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
            //WeixinJSBridge.call('hideToolbar');
        });
    } else if (document.attachEvent) {
        document.attachEvent('WeixinJSBridgeReady', onBridgeReady);
        document.attachEvent('onWeixinJSBridgeReady', onBridgeReady);
        document.attachEvent('onWeixinJSBridgeReady', function onBridgeReady() {
            //WeixinJSBridge.call('hideToolbar');
        });
    }    
})();