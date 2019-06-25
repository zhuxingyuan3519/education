<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CardEdit.aspx.cs" Inherits="Web.Admin.WebAdmin.CardEdit" %>

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
                <table cellpadding="0" cellspacing="0">
                    <tr id="trBank">
                        <td width="5%" align="right">发布银行
                        </td>
                        <td width="75%">
                            <select id="ddlBank" runat="server">
                            </select>
                        </td>
                    </tr>

                    <tr >
                        <td align="right">办卡链接地址
                        </td>
                        <td style="padding: 15px;">
                            <textarea id="txt_content" runat="server" style="width: 600px; height: 100px"></textarea>
                        </td>
                    </tr>
                    <tr style="height: 40px;">
                        <td width="5%" align="right"></td>
                        <td width="75%" align="left">
                            <input type="button" class="normal_btnok" value="提交" onclick="checkChange();" />

                            <input type="reset" class="btn btn-danger" value="返回" onclick="callhtml('WebAdmin/CardList.aspx', '办卡链接')" />&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </form>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
        })


        function checkChange() {
            var rek = '<%=Request.RawUrl%>';
            rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
            var index = layer.load(2, { shade: false }); //0代表加载的风格，支持0-2
            $.ajax({
                type: 'post',
                url: '/Admin/WebAdmin/' + rek + '?Action=Add',
                data: $('#form1').serialize(),
                success: function (info) {
                    layer.close(index);
                    if (info == "1") {
                        alert('操作成功');
                        callhtml('WebAdmin/CardList.aspx', '办卡链接');
                    }
                    else {
                        alert('操作失败，请重试！');
                    }
                }
            });
        }
    </script>
</body>
</html>
