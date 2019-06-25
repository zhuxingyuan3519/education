var layMessageIndexof;
var layMessageIframe = '';

$(function () {
    var appendMessageNoticeDiv = "<div  id=\"divMessageNotice\" style=\"display: none; color: black; padding-top: 10px; padding-left: 10px; font-size: 16px\"><div class=\"divMessageNoticeSendNameSpan\"></div></div>";
    $("body").append(appendMessageNoticeDiv);
});
function checkNewMessageNotice() {
    chechNotice();
    setInterval("chechNotice()", 30000);//1000为1秒钟
}

function chechNotice() {
    if ($("#divMessageNotice").css('display') == 'none') {
        if (layMessageIframe == '') {
            var checkInfo = {
                type: 'checkNoticeInfo'
            };
            var result = GetAjaxString(checkInfo);
            if (result != '') {  //有新消息
                showNotice(result);
            }
        }
    }
}
function showNotice(mcode) {
    $(".divMessageNoticeSendNameSpan").html('');
    //mcode是个消息列表的json
    var infoData = eval("(" + mcode + ")");
    $.each(infoData.noticeInfo, function (i, item) {
        var apphtml = "<a style='color:black;font-size:14px;' href='javascript:showNoticeDetail(\"" + item.Code + "\",\"" + item.MType + "\")'><img src='/images/new918.gif'/>&nbsp;来自" + item.MID + "的新消息</a><br/>";
        $(".divMessageNoticeSendNameSpan").append(apphtml);
    });
    layMessageIndexof = layer.open({
        type: 1 //此处以iframe举例
        , title: ['通知信息', 'background:#1E9FFF']
      , area: ['230px', '230px']
      , shade: 0
      , offset: [ //为了演示，随机坐标
            $(window).height() - 230, $(window).width() - 230
      ]
      , content: $("#divMessageNotice")
        //, btn: ['立即阅读', '稍后阅读']
        //, yes: function (index) {
        //    //弹出iframe，显示详细的
        //    //showNoticeDetail(mcode.split('≌')[0]);
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
    layer.close(layMessageIndexof);
    layMessageIframe = layer.open({
        type: 2,
        title: '通知信息',
        shadeClose: true,
        shade: 0.8,
        maxmin: true, //开启最大化最小化按钮
        area: [($(window).width() - 200) + 'px', ($(window).height() - 100) + 'px'],
        content: '/Admin/Message/MessageDetail.aspx?mcode=' + args,
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
            //按钮【按钮二】的回调
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