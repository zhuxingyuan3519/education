<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePwd.aspx.cs" Inherits="Web.Admin.ChangePwd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript">
        function saveObjs() {
            if (!checkForm())
                return;
            //var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            $.ajax({
                type: 'post',
                url: '/Admin/Member/ChangePwd?Action=add',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    //layer.close(index);
                    if (info == "0")
                        layer.alert("提交失败，请重试！");
                    else if (info == "1") {  //提交成功
                        layer.alert("原始密码不正确！");
                    }
                    else if (info == "2") {  //提交成功
                        layer.alert("操作成功！");
                    }
                    else
                        layer.alert(info);
                }
            });
        }
       
    </script>
</head>
<body>
    <div id="mempay">
        <div id="finance" class="container">
            <form id="form1" style="margin-top:20px">
    <div class="row">
        <div class="col col-md-12">
            <div class="form-group">
                <label for="txtEmail">&emsp;原密码：</label>
               <input type="password" id="txtOrginPwd" runat="server"  require-type="require" require-msg="原密码" />
            </div>
                <div class="form-group">
                <label for="txtEmail">&emsp;新密码：</label>
               <input type="password" id="Password1" runat="server"    require-type="require" require-msg="新密码" />
            </div>
                <div class="form-group">
                <label for="txtEmail">确认密码：</label>
               <input type="password" id="Password2" runat="server"   require-type="require" require-msg="确认密码" />
            </div>
        </div>
    </div>
 <div class="row">
        <div class="col col-md-12 " style="margin-top: 20px">
            <div class="form-group">
                <input type="button" class="btn btn-info" value="确认修改" id="btnSubmit" onclick="return saveObjs()" />
            </div>
        </div>
    </div>
    </form>
        </div>
    </div>
</body>
</html>
