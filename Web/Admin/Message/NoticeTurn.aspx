<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoticeTurn.aspx.cs" Inherits="Web.Admin.Message.NoticeTurn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript">
        function setUrlCookie() {
            document.cookie = 'lasturl=Message/NoticeTurn.aspx';
            document.cookie = 'lasturlname=最新口子';
        }
        function turnToAddKouzi(tid, title) {
            callhtml('Message/NoticeList.aspx?id=' + tid, title);
        }
    </script>
</head>
<body>
   <div id="mempay">
        <div id="finance" class="container"
            <form id="form1">
                
        <div class="row">
            <div class="col-sm-12" style="padding:10px">
                <input type="button" class="btn btn-success" value="分销商快卡口子" onclick="turnToAddKouzi(11, '分销商快卡口子')" />&emsp;
                <input type="button" class="btn btn-danger" value="前台快卡口子"  onclick="turnToAddKouzi(7, '前台快卡口子')"/>
            </div>

                <div class="col-sm-12" style="padding:10px">
                <input type="button" class="btn btn-success" value="分销商提额口子" onclick="turnToAddKouzi(12, '分销商提额口子')" />&emsp;
                <input type="button" class="btn btn-danger" value="前台提额口子"  onclick="turnToAddKouzi(8, '前台提额口子')"/>
            </div>

                <div class="col-sm-12" style="padding:10px">
                <input type="button" class="btn btn-success" value="分销商贷款口子" onclick="turnToAddKouzi(13, '分销商贷款口子')" />&emsp;
                <input type="button" class="btn btn-danger" value="前台贷款口子"  onclick="turnToAddKouzi(9, '前台贷款口子')"/>
            </div>
        </div>
    </form>
              </div>
        </div>
</body>
</html>
