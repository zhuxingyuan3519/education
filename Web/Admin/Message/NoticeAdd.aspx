<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoticeAdd.aspx.cs" Inherits="Web.Admin.Message.NoticeAdd"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
    <script type="text/javascript" charset="utf-8" src="shdksiwh/editor_config.js"></script>
    <script type="text/javascript" charset="utf-8" src="shdksiwh/editor_all.js"></script>
</head>
<body>
    <div id="mempay">
        <div id="finance">
            <form id="form1">
                <input type="hidden" id="hidType" runat="server" />
                <input type="hidden" id="hidCType" runat="server" />
                <table cellpadding="0" cellspacing="0">
                    <tr id="trTitel">
                        <td width="5%" align="right">标题
                        </td>
                        <td width="75%">
                            <input id="txtNTitle" name="txtNTitle" type="text" class="normal_input" maxlength="200" style="width: 50%;" />
                            <input type="checkbox" name="isFaxed" value="true" style="display: none" />
                            <input type="button" class="btn btn-danger" value="返回" onclick="toUpperLeavel();" />
                        </td>
                    </tr>

                    <tr id="trBank" runat="server">
                        <td width="5%" align="right">发布银行
                        </td>
                        <td width="75%">
                            <select id="ddlBank" runat="server">
                            </select>
                        </td>
                    </tr>

                    <tr id="trContent">
                        <td align="right">内容
                        </td>
                        <td style="padding: 15px;">
                            <script id="editor" type="text/plain"></script>
                            <input name="hdContent" id="hdContent" type="hidden" />
                        </td>
                    </tr>
                    <tr id="trLink" style="display: none">
                        <td align="right">链接地址
                        </td>
                        <td style="padding: 15px;">
                            <textarea id="txt_content" runat="server" style="width: 600px; height: 100px"></textarea>
                        </td>
                    </tr>
                    <tr style="height: 40px;">
                        <td width="5%" align="right"></td>
                        <td width="75%" align="left">
                            <input type="reset" class="normal_btnok" value="重 置" />&nbsp;&nbsp;
                        <input type="button" class="normal_btnok" value="发 布" onclick="checkChange();" />
                        </td>
                    </tr>
                </table>
            </form>
        </div>
    </div>
    <script type="text/javascript">
        var typeId = '<%=NType%>';
        $(function () {
            if (typeId === '4') {
                //$("#trTitel").hide();
                $("#trContent").hide();
                $("#trLink").show();
                $("#trBank").show();
            }
        });
        function toUpperLeavel() {
            var title = $(".alert-danger").find("strong").html();
            callhtml('Message/NoticeList.aspx?id=' + typeId, title)
        }

        var ue = UE.getEditor('editor');
        function checkChange() {
            if ($('#txtNTitle').val() == '') {
                layer.alert('标题不能为空'); return false;
            }
            else {
                $('#hdContent').val(encodeURI(ue.getContent()));
                var rek = '<%=Request.RawUrl%>';
                rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
                var typeId = $("#hidType").val();
                var index = layer.load(2, { shade: false }); //0代表加载的风格，支持0-2
                $.ajax({
                    type: 'post',
                    url: '/Admin/Message/' + rek + '?Action=Add',
                    data: $('#form1').serialize(),
                    success: function (info) {
                        layer.close(index);
                        if (info == "1") {
                            alert('发布成功');
                            if (typeId == "1")
                                callhtml('Message/NoticeList.aspx?id=' + typeId, '公告管理');
                            else if (typeId == "2") {
                                var ctype = $("#hidCType").val();
                                if (ctype == "0") {
                                    callhtml('Message/NoticeList.aspx?id=' + typeId + '&bank=' + $("#ddlBank").val() + '&type=0', '信用卡常识—' + $('#ddlBank  option:selected').text());
                                }
                                else if (ctype == "1") {
                                    callhtml('Message/NoticeList.aspx?id=' + typeId + '&bank=' + $("#ddlBank").val() + '&type=1', '信用卡提额技术—' + $('#ddlBank  option:selected').text());
                                }
                            }
                            var title = $(".alert-danger").find("strong").html();
                            //if (typeId == "3")
                                callhtml('Message/NoticeList.aspx?id=' + typeId, title);
                            //if (typeId == "4")
                            //    callhtml('Message/NoticeList.aspx?id=' + typeId, '信用贷款');
                            //if (typeId == "5")
                            //    callhtml('Message/NoticeList.aspx?id=' + typeId, '新手指南');
                        }
                        else {
                            alert('操作失败，请重试！');
                        }
                    }
                });
            }
        }
    </script>
</body>
</html>
