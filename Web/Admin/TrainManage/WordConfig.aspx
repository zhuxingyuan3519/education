<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WordConfig.aspx.cs" Inherits="Web.Admin.TrainManage.WordConfig" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>混合词维护</title>
    <style type="text/css">
     
    </style>
    <script type="text/javascript">

    </script>
</head>
<body>
    <div id="mempay">
        <div id="finance" class="container">
            <form id="form1">
                <div class="row">
                        <div class="col col-md-12"  style="padding:10px;font-size:14px;">
                            请输入混合词，每个词之间用空格符间隔
                            <input type="hidden" runat="server" id="hidStatus" />
                        </div>
                    <div class="col col-md-12">
                        <div class="form-group">
                            <textarea id="txt_words" class="form-control" style="height:400px" runat="server">
                            </textarea>   
                        </div>
                    </div>
                </div>

                     <div class="row">
                    <div class="col col-md-12 text-center">
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="保存" id="btnSubmit" onclick="checkChange()" />
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
    <script type="text/javascript">
        function checkChange() {
            if (!checkForm())
                return;
            if (confirm("是否确认？")) {
                var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
                var rek = '<%=Request.RawUrl%>';
                //获取最后一个/和?之间的内容，就是请求的页面
                rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
                $.ajax({
                    type: 'post',
                    url: 'TrainManage/' + rek + '?Action=add',
                    data: $('#form1').serializeArray(),
                    success: function (info) {
                        layer.close(index);
                        var data = JSON.parse(info);
                        if (data.isSuccess == "false") {
                            layer.alert(data.msg);
                        }
                        else {
                            layer.alert("创建成功！");
                            //$("#hidId").val(data.msg);
                            setTimeout(function () {
                                callhtml('TrainManage/WordConfig?status=' + $("#hidStatus").val(), '混合词维护');
                            }, 2000);
                        }
                    }
                });
            }
        }
    </script>
</body>
</html>
