<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="out_link.aspx.cs" Inherits="Web.m.app.out_link" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1;" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
    <title></title>
    <script src="/common/js/jquery.min.js" type="text/javascript" charset="utf-8"></script>
       <link rel="stylesheet" type="text/css" href="/common/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/m/css/m.css" />
    <style type="text/css">
        body{margin:0; padding:0;}
        .scroll-wrapper {   
  position: fixed;    
  right: 0;    
  bottom: 0;    
  left: 0;   
  top: 0;   
  -webkit-overflow-scrolling: touch;   
  overflow-y: scroll;   
}   
   
.scroll-wrapper iframe {   
  height: 100%;   
  width: 100%;   
}
    </style>
    <script type="text/javascript">
        //获取url中的参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var l = decodeURI(window.location.search);
            var r = l.substr(1).match(reg);
            if (r != null) return unescape(r[2]);
            return null;
        }
        var isShowNav = '<%=Service.GlobleConfigService.GetWebConfig("IsShowMainNav").Value == "1"%>';
        $(function () {
            $("#urlIframe").css("height", document.body.scrollHeight);
            var url = getQueryString("returnUrl");
            var title = getQueryString("returnTitle");
            //$("title").html(title);
            //$("#titleH").html(title);
            $("#urlIframe").attr("src", url);
            //alert(document.body.scrollHeight);
            if (isShowNav == "1") { //显示导航栏
                $("#urlIframe").css("margin-top","30px");
                //隐藏导航栏
                //$(".nav-head-height").hide().css("height", "0px").css("min-height", "0px").css("padding-top","0px");
            }
        })

        function reinitIframe() {
            var iframe = document.getElementById("urlIframe");
            try {
                var bHeight = iframe.contentWindow.document.body.scrollHeight;
                /*       
                                   var dHeight = iframe.contentWindow.document.documentElement.scrollHeight;              
                                   var height = Math.max(bHeight, dHeight);                          
                                    iframe.height = height;                */
                iframe.height = bHeight;
            }
            catch (ex) { }
        }

    </script>
</head>
<body>
     <div class="container-fluid" style="padding-right:0px;padding-left:0px;">

            <%if (MethodHelper.ConfigHelper.GetAppSettings("SystemID") != "yykgj")
                  { %>
        <div class="navbar navbar-fixed-top header text-center nav-head-height">
            <a href="javascript:window.history.go(-1);" class="history"></a>
            <h5 id="titleH"><%=Service.CacheService.GlobleConfig.Contacter %> </h5>
        </div>
         </div>
       <%}%>
    <div class="scroll-wrapper">
    <iframe marginwidth="0" src="" marginheight="0" style="width:100%;height:100%;" name="i" id="urlIframe" frameborder="0" scrolling="yes" onload="reinitIframe()" >
</iframe>
        </div>

</body>
</html>
