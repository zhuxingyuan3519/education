<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HireEdit.aspx.cs" Inherits="Web.Admin.WebAdmin.HireEdit" %>

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
                <input type="hidden" runat="server" id="hidId" />
                <table cellpadding="0" cellspacing="0">
                    <tr id="trBank">
                        <td align="right">缴费角色
                        </td>
                        <td>
                            <select id="ddlRole" runat="server">
                            </select>
                        </td>
                        <td align="right">总分期期数
                        </td>
                        <td>
                            <input type="text" id="txt_HireCount" runat="server" />
                        </td>
                    </tr>

                    <tr>
                        <td align="right">付款日期（每月几号）
                        </td>
                        <td>
                            <input type="text" id="txt_PayDate" runat="server" />
                        </td>
                        <td align="right">每期金额
                        </td>
                        <td>
                            <input type="text" id="txt_EveryHireMoney" runat="server" />
                        </td>
                    </tr>

                    <tr>
                        <td align="right" style="width: 200px">每期配送收费端口数量
                        </td>
                        <td>
                            <input type="text" id="txt_TradePointCount" runat="server" />
                        </td>
                        <td align="right">每期配送体验端口数量
                        </td>
                        <td>
                            <input type="text" id="txt_LeaveTradePointCount" runat="server" />
                        </td>
                    </tr>

                    <tr style="height: 40px;">
                        <td colspan="4" align="left">
                            <input type="button" class="normal_btnok" value="提交" onclick="checkChange();" />

                            <input type="reset" class="btn btn-danger" value="返回" onclick="callhtml('WebAdmin/HireList.aspx', '办卡链接')" />&nbsp;&nbsp;
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
                        callhtml('WebAdmin/HireList.aspx', '分期配置');
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
