<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoticeDetail.aspx.cs" Inherits="Web.Admin.Message.NoticeDetail"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
</head>
<body>
    <div id="mempay">
        <div id="finance">
            <form id="form1">
                <div class="row">
                    <div class="col-sm-12" style="text-align:center;font-size:16px;padding:10px"><%=NoticeModel.NTitle%> &emsp;<span style="font-size:12px"><%=NoticeModel.NCreateTime%></span></div>
                      <div class="col-sm-12" style="padding:10px"><%=NoticeModel.NContent%></div>
                       <div class="col-sm-12" style="padding:20px;text-align:center">
                             <input type="button" value="返回" class=" btn btn-danger" onclick=" toUpperLeavel();" />
                       </div>
                </div>
            </form>
        </div>
    </div>
    <script type="text/javascript">
        var typeId = '<%=NoticeModel.NType%>';
        function toUpperLeavel() {
            var title = $(".alert-danger").find("strong").html();
            callhtml('Message/NoticeList.aspx?id=' + typeId, title)
        }
    </script>
</body>
</html>
