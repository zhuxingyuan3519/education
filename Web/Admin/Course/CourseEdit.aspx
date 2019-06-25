<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseEdit.aspx.cs" Inherits="Web.Admin.Course.CourseEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>课程维护</title>
      <script type="text/javascript" charset="utf-8" src="shdksiwh/editor_config.js"></script>
    <script type="text/javascript" charset="utf-8" src="shdksiwh/editor_all.js"></script>
    <script type="text/javascript">
        var ue = UE.getEditor('editor');
        function checkChange() {
            if (!checkForm())
                return;
            if (confirm("要创建的名称为：" + $("#txt_Name").val() + "，是否确认？")) {
                var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
                var rek = '<%=Request.RawUrl%>';
                //获取最后一个/和?之间的内容，就是请求的页面
                rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
                $('#hdContent').val(encodeURI(ue.getContent()));
                $.ajax({
                    type: 'post',
                    url: 'Course/' + rek + '?Action=add',
                    data: $('#form1').serializeArray(),
                    success: function (info) {
                        layer.close(index);
                        var data = JSON.parse(info);
                        if (!data.isSuccess) {
                            layer.alert(data.msg);
                        }
                        else {
                            layer.alert("创建成功！");
                            $("#hidId").val(data.msg);
                            //setTimeout(function () {
                            //    resetList()
                            //}, 3000);
                        }
                    }
                });
            }
        }
        function resetList() {
            callhtml('Course/CourseList.aspx', '名人大咖');
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div id="finance" class="container">
            <form id="form1" style="padding: 30px">
                <input type="hidden" runat="server" id="hidId" />
              
                <div class="row">
                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_Name">名称：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_Name" require-type="require" require-msg="名称"  style="width:80%"/>
                        </div>
                    </div>
                    <div class="col col-md-6 hidden">
                        <div class="form-group">
                            <label for="txt_Tel">课程费用：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_Fee" value="0" require-type="decimal" require-msg="课程费用" />
                        </div>
                    </div>

                       <div class="col col-md-6 hidden">
                      <div class="form-group hidden">
             <label>是否自动获得代理商资格：</label>
                <select id="ddl_Leavel" runat="server">
                    <option value="0">否</option>
                    <option value="1">是</option>
                </select>
            </div>
                    </div>
                  </div>
                   <div class="row hidden">
                    <div class="col col-md-12">
                        <div class="form-group">
                            <label for="txt_Name">课程标题：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_Title"   style="width:70%"/>
                        </div>
                    </div>
                  </div>
                   <div class="row">
                          <div class="col col-md-12">
                        <div class="form-group">
                            <label for="txt_Name">详情：</label>
                            <script id="editor" type="text/plain"><%=txtNContent %></script>
                            <input name="hdContent" id="hdContent" type="hidden" />
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
    
</body>
</html>
