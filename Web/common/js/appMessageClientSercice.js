var layMessageIndexof;
var layMessageIframe = '';

$(function () {
    if (isLogin == '1') {
        //登录之后开始轮询消息
        checkNewMessageNotice();
    }
    //var appendMessageNoticeDiv = "<div  id=\"divMessageNotice\" style=\"display: none; color: black; padding-top: 10px; padding-left: 10px; font-size: 16px\"><div class=\"divMessageNoticeSendNameSpan\"></div></div>";
    //$("body").append(appendMessageNoticeDiv);
});

function checkNewMessageNotice() {
    chechNotice();
    setInterval("chechNotice()", 30000);//1000为1秒钟
}

function chechNotice() {
    var msgCount = $("#spMessageCount").html();
    if (msgCount == '') {
        var checkInfo = {
            type: 'checkNoticeInfo'
        };
        var result = GetAjaxString(checkInfo);
        if (result != '') {  //有新消息
            showNotice(result);
        }
    }
}

function showNotice(mcode) {
    //$(".divMessageNoticeSendNameSpan").html('');
    var infoData = eval("(" + mcode + ")");
    $("#spMessageCount").html(infoData.noticeInfo.length);
    return false;

    $.each(infoData.noticeInfo, function (i, item) {
        var apphtml = "<a style='color:black;font-size:14px;' href='javascript:showNoticeDetail(\"" + item.Code + "\"," + item.MType + ")'><img src='/images/new918.gif'/>&nbsp;来自" + item.MID + "的新消息</a><br/>";
        $(".divMessageNoticeSendNameSpan").append(apphtml);
    });
    layMessageIndexof = layer.open({
        type: 1 //此处以iframe举例
      , title: ['通知信息', 'background:#1E9FFF']
      , area: ['200px', '180px']
      , shade: 0
      , offset: 'rb'
      , content: $("#divMessageNotice")
        //, btn: ['立即阅读', '稍后阅读']
        //, yes: function (index) {
        //    //弹出iframe，显示详细的
        //    showNoticeDetail(mcode.split('≌')[0]);
        //    layer.close(index);
        //}
        , cancel: function (index) {
            layer.close(index);
        }
      , zIndex: layer.zIndex //重点1
      , success: function (layero) {
          layer.setTop(layero); //重点2
      }
    });
}

function showNoticeDetail(args, mtype) {
    var offsetwidth = $(window).width();
    var offsetheight = $(window).height();
    if (isMobile()) {
        offsetwidth = offsetwidth - 50 + 'px';
        offsetheight = offsetheight - 120 + 'px';

    }
    else {
        offsetwidth = ($(window).width() - 200) + 'px';
        offsetheight = ($(window).height() - 100) + 'px'
    }
    layer.close(layMessageIndexof);
    layMessageIframe = layer.open({
        type: 2,
        title: '通知信息',
        shadeClose: true,
        shade: 0.8,
        maxmin: true, //开启最大化最小化按钮
        offset: '40px',
        area: [offsetwidth, offsetheight],
        content: 'Notice.aspx?mcode=' + args,
        btn: ['回复消息', '已阅']
     , yes: function (index) {
         //弹出iframe，显示详细的
         var responsemsg = parent.window['layui-layer-iframe' + index].updatenongjiinfo(); //调用iframe层方法 
         if ($.trim(responsemsg) == '') {
             layer.alert("回复内容不能为空！");
             return;
         }
         var responseInfo = {
             type: 'responseMsg',
             reMsg: responsemsg,
             mCode: args
         };
         var result = GetAjaxString(responseInfo);
         if (result == "1") {
             layer.alert("回复成功！");
             layer.close(index);
             layMessageIframe = '';
         }
         else
             layer.alert("回复失败！");
     }
        , btn2: function (index, layero) {
            //按钮【已阅】的回调
            var dealNoticeInfo = {
                type: 'dealNoticeInfo',
                mCode: args,
                MType: mtype
            };
            var result = GetAjaxString(dealNoticeInfo);
            layer.close(index);
            layMessageIframe = '';
        }
      , cancel: function (index) {
          layer.close(index);
          layMessageIframe = '';
      }
    });
}

