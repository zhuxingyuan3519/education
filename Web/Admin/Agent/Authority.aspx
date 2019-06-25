<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Authority.aspx.cs" Inherits="Web.Admin.Agent.Authority" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .width50, .titiletxt {
            width: 20%;
        }

        .titiletxt, subtitiletxt {
            display: inline;
        }

        .imgop {
            cursor: pointer;
            width: 20px;
        }

        .subtitletxt {
            width: 20%;
            display: inline;
        }

        .errfail {
            color: Red;
        }

        .errsuccess {
            color: #86C440;
        }

        .subui {
            padding-left: 20px;
        }

        .width60 {
            width: 100%;
        }

        .delone {
            width: 20px;
        }

        .imgadd {
            float: right;
        }

        .txturl {
            width: 30%;
            display: inline;
        }

        .txticon {
            width: 100px;
            display: inline;
        }

        .txtindex {
            width: 70px;
            display: inline;
        }
    </style>
    <script type="text/javascript">
        function showSubCategory(obj) {
            var subCate = $(obj).parent().next();
            if (subCate.hasClass("hidden")) {
                $(obj).attr("src", "../Admin/img/del.png");
                subCate.removeClass("hidden");
            }
            else {
                $(obj).attr("src", "../Admin/img/plus.png");
                subCate.addClass("hidden");
            }
        }
        $(document).ready(function () {
            //加载权限

            roleChange();
        });
        function choiceSubAll(obj) {
            if ($(obj).prop("checked")) {
                $(obj).parent().next().find("input:checkbox").prop("checked", true);
            }
            else {
                $(obj).parent().next().find("input:checkbox").prop("checked", false);
            }
        }
        function subChoice(obj) {
            if ($(obj).prop("checked")) {
                $(obj).parent().parent().parent().prev().find("input:checkbox").prop("checked", true);
            }
        }
        function saveAll() {
            var roleId = $("#ddl_Roles").val();
            if (roleId == '') {
                layer.alert("请选择角色！");
                return false;
            }
            var rek = '<%=Request.RawUrl%>';
            rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
            $.ajax({
                type: 'post',
                url: 'Agent/' + rek + '?Action=ADD',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    if (info == "1") {  //提交成功
                        layer.alert("保存成功！");
                    }
                    else
                        layer.alert("保存失败");
                }
            });
        }
        function roleChange() {
            var roleId = $("#hidCode").val();
            var rek = '<%=Request.RawUrl%>';
            rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
            $.ajax({
                type: 'post',
                url: 'Agent/' + rek + '?Action=MODIFY',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    $("input:checkbox").prop("checked", false);
                    if (info != '') {
                        var infoarr = info.split('*');
                        for (var i = 0; i < infoarr.length; i++) {
                            if (infoarr[i] != '') {
                                $("input[value='" + infoarr[i] + "']").prop("checked", true);
                            }
                        }
                    }
                }
            });
        }
        function choiceAllInTitle(obj)
        {
            if ($(obj).prop("checked")) {
                $(obj).parent().parent().parent().find("input:checkbox").prop("checked", true);
            }
            else{
                $(obj).parent().parent().parent().find("input:checkbox").prop("checked", false);
            }
        }
    </script>
</head>
<body>
    <form id="form1">
        <div id="mempay">
            <input type="hidden" id="hidCode" runat="server" />
             <div class="control">
                账号：<span id="spMID" runat="server"></span>&emsp;
                 姓名：<span id="spName" runat="server"></span>&emsp;
                 代理商级别：<span id="spAgent" runat="server"></span>&emsp;
                 代理地区：<span id="spAgentAddress" runat="server"></span>
                  <input type="button" value="保存" class="btn btn-success" onclick="saveAll()" />
            </div>
             
            <div class="ui_table" style="width: 250px; float: left; margin-right: 50px">
                <ul class="list-group width60">
                    <li class="list-group-item"  style="background-color:#3498DB;color:white">网站后台菜单：
                            <span style="float:right">全选：  <input type="checkbox"  onclick="choiceAllInTitle(this)" /></span>
                    </li>
                    <%foreach (Model.Sys_Privage item in listAdmin)
                      { %>
                    <li class="list-group-item">
                        <input type="checkbox" name="menuCode" value="<%=item.Id %>" onclick="choiceSubAll(this)" />
                        <span><%=item.Name %></span>&nbsp;
                    <img class="imgop imgadd" src="../Admin/img/plus.png" onclick="showSubCategory(this)" />
                    </li>
                    <li class="list-group-item hidden">
                        <input type="hidden" value="<%=item.Id %>" />
                        <ul class="list-group subui subuilist">
                            <%foreach (Model.Sys_Privage subItem in GetSecondLeavelDict(item.Id))
                              { %>
                            <li class="list-group-item">
                                <input type="checkbox" name="menuCode" value="<%=subItem.Id %>" onclick="subChoice(this)" />
                                <span><%=subItem.Name %></span>&nbsp;
                            </li>
                            <%} %>
                        </ul>
                    </li>
                    <%} %>
                </ul>
            </div>
            <div class="ui_table" style="width: 400px; float: left">
                   <ul class="list-group width60">
                    <li class="list-group-item"  style="background-color:#DB3489;color:white">操作权限：
                       <span style="float:right">全选：  <input type="checkbox"  onclick="choiceAllInTitle(this)" /></span>
                    </li>
                    <li class="list-group-item">
                         <%foreach (Model.Sys_Privage item in listOperator)
                      { %>
                        <input type="checkbox" name="menuCode" value="^<%=item.Id %>" onclick="choiceSubAll(this)" />
                        <span><%=item.Name %></span>&nbsp;
                          <%} %>
                    </li>
                </ul>
               </div>
            <div>
             </div>
        </div>
    </form>

</body>
</html>

