<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoticeModify.aspx.cs" Inherits="Web.Admin.Message.NoticeModify"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>修改公告</title>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
    <script type="text/javascript" charset="utf-8" src="shdksiwh/editor_config.js"></script>
    <script type="text/javascript" charset="utf-8" src="shdksiwh/editor_all.js"></script>
</head>
<body>
    <div id="mempay">
        <div id="finance">
            <form id="form1">
            <table cellpadding="0" cellspacing="0">
               <tr id="trTitel">
                    <td  align="right">
                        标题<input runat="server" id="lbID" type="hidden" />
                    </td>
                    <td style="height: 40px;">
                        <input id="txtNTitle" class="normal_input" runat="server" style="width: 50%;" />
                        <input type="checkbox" id="chkFixed" runat="server" checked="checked"  style="display:none"/>
                        <input name="hdchkFixed" id="hdchkFixed" type="hidden" />
                           <input type="button" class="btn btn-danger" value="返回" onclick="toUpperLeavel();" />
                    </td>
                </tr>
                 <tr id="trBank" runat="server">
                    <td  align="right">
                        发布银行
                    </td>
                    <td >
                       <select id="ddlBank" runat="server">
                       </select>
                    </td>
                </tr>
                <tr id="trContent">
                    <td align="right">
                        内容
                    </td>
                    <td style="padding: 15px;">
                        <script id="editor" type="text/plain"><%=txtNContent %></script>
                        <input name="hdContent" id="hdContent" type="hidden" />
                    </td>
                </tr>
                   <tr id="trLink" style="display:none">
                    <td align="right">
                        链接地址
                    </td>
                    <td style="padding: 15px;">
                     <textarea id="txt_content" runat="server"  style="width:600px;height:100px"><%=txtNContent %></textarea>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                   
                    </td>
                    <td  align="left">
                         <input type="reset" class="normal_btnok" value="重 置"   />&nbsp;
                        <input type="button" class="normal_btnok" value="发 布"   onclick="checkChange();" />
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
                alert('标题不能为空', '1', 'ture');
                return false;
            }
           else {
                $('#hdchkFixed').val(document.getElementById('chkFixed').checked);
                $('#hdContent').val(encodeURI(ue.getContent()));
                var rek = '<%=Request.RawUrl%>';
                rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
                var url = '/Admin/Message/' + rek + '?Action=Modify';
                $.ajax({
                    type: 'post',
                    url: url,
                    data: $('#form1').serialize(),
                    success: function (info) {
                        alert('修改成功');
                    }
                });
            }
        }
    </script>
</body>
</html>
