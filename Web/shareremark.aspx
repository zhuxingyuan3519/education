<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="shareremark.aspx.cs" EnableViewState="false" Inherits="Web.shareremark" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <style type="text/css">
       
    </style>
    <script type="text/javascript">
        function toHaiBao() {
            var branch = '<%=MethodHelper.CommonHelper.DESEncrypt(TModel.ID.ToString()) %>';
            window.location.href = "share?registcode="+branch;
        }
        document.onclick = toHaiBao;
    </script>
    <title><%=Service.CacheService.GlobleConfig.Contacter %></title>
</head>
<body>
    <form id="form1" runat="server">
           <div id="content">
               <img src="images/tjshow.jpg" style="width:100%" />
           </div>
    </form>
</body>
</html>
