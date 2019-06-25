<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditWord.aspx.cs" Inherits="Web.Admin.TrainManage.EditWord" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>单词维护</title>
    <style type="text/css">
        .width85 {
            width: 85%;
        }
    </style>
    <script type="text/javascript">

        function resetList() {
            callhtml('TrainManage/WordList', '词库列表');
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
                            <label for="txt_MID">英文单词：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_English" require-type="require" require-msg="英文单词" />
                        </div>
                    </div>

                    <div class="col col-md-4">
                        <div class="form-group">
                            <label for="txt_Name">音标：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_Phonetic" />
                        </div>
                    </div>


                    <div class="col col-md-4">
                        <div class="form-group">
                            <label for="txt_Name">中文释义：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_Chinese" require-type="require" require-msg="中文释义" />
                        </div>
                    </div>
                </div>



                <div class="row">
                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_MID">模块拆分：</label>
                            <input runat="server" type="text" class="form-inline width85" id="txt_Module1" />
                        </div>
                    </div>

                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_Name">情景联想：</label>
                            <input runat="server" type="text" class="form-inline width85" id="txt_Association1" />
                        </div>
                    </div>

                </div>




                <div class="row">
                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_MID">模块拆分：</label>
                            <input runat="server" type="text" class="form-inline width85" id="txt_Module2" />
                        </div>
                    </div>

                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_Name">情景联想：</label>
                            <input runat="server" type="text" class="form-inline width85" id="txt_Association2" />
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_MID">模块拆分：</label>
                            <input runat="server" type="text" class="form-inline width85" id="txt_Module3" />
                        </div>
                    </div>

                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_Name">情景联想：</label>
                            <input runat="server" type="text" class="form-inline width85" id="txt_Association3" />
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_MID">模块拆分：</label>
                            <input runat="server" type="text" class="form-inline width85" id="txt_Module4" />
                        </div>
                    </div>

                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_Name">情景联想：</label>
                            <input runat="server" type="text" class="form-inline width85" id="txt_Association4" />
                        </div>
                    </div>

                </div>



                <div class="row">
                    <div class="col col-md-12 text-center">
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="提交" id="btnSubmit" onclick="checkChange()" />&emsp;&emsp;
                             <input type="button" class="btn btn-info" value="返回" onclick="resetList()" />
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
            var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            var rek = '<%=Request.RawUrl%>';
            //获取最后一个/和?之间的内容，就是请求的页面
            rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
            $.ajax({
                type: 'post',
                cache: false,
                url: 'TrainManage/' + rek + '?Action=add',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    layer.close(index);
                    var data = JSON.parse(info);
                    if (data.isSuccess == "false") {
                        layer.alert(data.msg);
                    }
                    else {
                        layer.alert("操作成功");
                        $("#hidId").val(data.msg);

                    }
                }
            });
        }
    </script>
</body>
</html>
