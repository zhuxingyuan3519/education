<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebConfig.aspx.cs" Inherits="Web.Admin.WebAdmin.WebConfig"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
    <script type="text/javascript">
        $(function () {
            $("#hidRemark").val(unique_callhtml_title);
        });
    </script>
   <script type="text/javascript">
        window.UEDITOR_HOME_URL = "/ueditor/";
    </script>
    <script type="text/javascript" charset="utf-8" src="/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="/ueditor/ueditor.all.min.js"> </script>
    <!--建议手动加在语言，避免在ie下有时因为加载语言失败导致编辑器加载失败-->
    <!--这里加载的语言文件会覆盖你在配置项目里添加的语言类型，比如你在配置项目里配置的是英文，这里加载的中文，那最后就是中文-->
    <script type="text/javascript" charset="utf-8" src="/ueditor/lang/zh-cn/zh-cn.js"></script>
 
     
</head>
<body>
    <div id="mempay">
        <div id="finance">
            <form id="form1">
                <input type="hidden" id="hidCode" runat="server" />
                           <input type="hidden" id="hidRemark" runat="server" />
                <input type="hidden" id="hidType" runat="server" />
                <table cellpadding="0" cellspacing="0">

                    <tr id="trContent">
                        <td align="right">内容
                        </td>
                        <td style="padding: 15px;">

                             <textarea id="editor1" style="width: 100%; height: 400px;" runat="server"></textarea>
                        </td>
                    </tr>
                    <tr style="height: 40px;">
                        <td width="5%" align="right"></td>
                        <td width="75%" align="left">
                            <input type="reset" class="normal_btnok" value="重 置" />&nbsp;&nbsp;
                        <input type="button" class="normal_btnok" value="发 布" onclick="checkChange();" />
                            <input type="button" class="btn btn-danger hidden" value="返回" onclick="returnLast();" />
                        </td>
                    </tr>
                </table>
            </form>
        </div>
    </div>
    <script type="text/javascript">
           var ue = UE.getEditor('editor1');

        function returnLast() {
            var lasturl = getCookie('lasturl');
            var lasturltitle = getCookie('lasturlname');
            callhtml(lasturl, lasturltitle);
        }
        function getCookie(name) {
            var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
            if (arr = document.cookie.match(reg))
                return unescape(arr[2]);
            else
                return null;
        }

        //var ue = UE.getEditor('editor');
        function checkChange() {
            //$('#hdContent').val(encodeURI(ue.getContent()));
            var rek = '<%=Request.RawUrl%>';
                rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
                var typeId = $("#hidType").val();
                var index = layer.load(2, { shade: false }); //0代表加载的风格，支持0-2
                $.ajax({
                    type: 'post',
                    url: '/Admin/WebAdmin/' + rek + '?Action=Add',
                    data: $('#form1').serializeArray(),
                    success: function (info) {
                        layer.close(index);
                        if (info == "1") {
                            alert('发布成功');
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
