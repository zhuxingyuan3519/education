<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberChange.aspx.cs" Inherits="Web.Admin.Member.MemberChange" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function checkChange() {
            if (!checkForm())
                return;
            var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            var rek = '<%=Request.RawUrl%>';
            //获取最后一个/和?之间的内容，就是请求的页面
            rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
            $.ajax({
                type: 'post',
                url: 'Member/' + rek + '?Action=add',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    layer.close(index);
                    if (isNaN(info)) {
                        layer.alert(info);
                    }
                    else {
                        layer.alert("修改失败，请重试！");
                        $("#hidId").val(info);
                        setTimeout(function () {
                            resetList()
                        }, 3000);
                    }
                }
            });
        }
        function checkMID() {
            var rek = '<%=Request.RawUrl%>';
            //获取最后一个/和?之间的内容，就是请求的页面
            rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
            $.ajax({
                type: 'post',
                url: 'Member/' + rek + '?Action=MODIFY',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    if (info.indexOf('~') > 0) {
                        var array = info.split('~');
                        $("#hidId").val(array[0]);
                        $("#spMTJ").html(array[2]);
                        $("#spAgent").html(array[6]);
                        $("#spAdmin").html(array[4]);
                        $("#spRoleCode").html(array[8]);
                    }
                    else {
                        alert(info);
                    }
                }
            });
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div id="finance" class="container">
            <form id="form1" style="padding: 30px">
                <input type="hidden" runat="server" id="hidId" />

                <div class="row">
                    <div class="col col-md-4">
                        <div class="form-group">
                            <label for="txt_Name">会员账号：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_MID" require-type="require" require-msg="会员账号" />
                            <input type="button" class="btn btn-success" value="查询" onclick="checkMID()" />
                        </div>
                    </div>
                    <div class="col col-md-8">
                        <div class="form-group">
                                    <label for="">用户级别：</label>
                                  <span id="spRoleCode"></span>&emsp;&emsp;
                            <label for="txt_MID">目前推荐人：</label>
                            <span id="spMTJ"></span>&emsp;&emsp;
                            <label for="txt_MID">目前归属代理商：</label>
                            <span id="spAgent"></span>&emsp;&emsp;
                            <label for="txt_MID">目前归属O单商：</label>
                            <span id="spAdmin"></span>
                        </div>
                </div>
                
                    <div class="row">
                    <div class="col col-md-4">
                    </div>
                    <div class="col col-md-4">
                        <div class="form-group">
                            <label for="txt_MID">新的推荐人：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_MTJ" require-type="require" require-msg="新的推荐人" />
                        </div>
                    </div>
                    <div class="col col-md-4">
                    </div>
                </div>
                <div class="row">
                    <div class="col col-md-12 text-center">
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="提交" id="btnSubmit" onclick="checkChange()" />&emsp;&emsp;
                        </div>
                    </div>
                </div>

            </form>
        </div>
    </div>
</body>
</html>
