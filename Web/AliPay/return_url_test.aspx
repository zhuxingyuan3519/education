<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="return_url_test.aspx.cs" Inherits="Web.AliPay.return_url_test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="/js/jquery.min.js"></script>
    <script type="text/javascript">
        function getCookie(name) {
            var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
            if (arr = document.cookie.match(reg))
                return unescape(arr[2]);
            else
                return null;
        }
        $(function () {
            var ac = getCookie('<%=MethodHelper.ConfigHelper.GetAppSettings("kgjpayurlInCookie")%>');
            setTimeout(function () {
                //自动跳转到“每日计划”的页面并弹出提醒
                if (ac != null&&ac != '')
                    window.location = '<%=Service.GlobleConfigService.GetWebConfig("WebSiteDomain").Value + "/addplan?ac="%>'+ac;
                else
                    window.location = '<%=Service.GlobleConfigService.GetWebConfig("WebSiteDomain").Value + "/everydayplan"%>';
            }, 2000);
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
