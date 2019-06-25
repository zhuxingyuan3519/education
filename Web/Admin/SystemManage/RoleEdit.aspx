<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleEdit.aspx.cs" Inherits="Web.Admin.SystemManage.RoleEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .width50, .titiletxt
        {
            width: 20%;
        }
        .titiletxt,subtitiletxt
        {
            display: inline;
        }
        .imgop
        {
            cursor: pointer;
            width: 20px;
        }
        .subtitletxt
        {
            width: 20%;
            display: inline;
        }
        .errfail
        {
            color: Red;
        }
        .errsuccess
        {
            color: #86C440;
        }
        .subui
        {
            padding-left: 20px;
        }
        .width60
        {
            width: 100%;
        }
        .delone
        {
            width: 20px;
        }
        .imgadd
        {
            float: right;
        }
        .txturl {
         width:30%;display:inline;}
          .txticon {
         width:100px;display:inline;}
            .txtindex {
         width:70px;display:inline;}
    </style>
    <script type="text/javascript">
 
        //cid:某一行的Id,isAdd:是否是新增。isDel:是否是删除。addPid:父Id
        function saveChange(obj, cid, isAdd, isDel, addPid) {
            var cname = $.trim($(obj).parent().find(".txtname").val());
            var murl = $.trim($(obj).parent().find(".txturl").val());
            var type = isAdd ? 3 : 1;
            if (isDel) {
                if (confirm("确定要删除吗？")) {
                    type = 2;
                } else {
                    return false;
                }
            }
            else {
                if (cname == "") {
                    alert("名称不能为空"); return false;
                }
            }

            $.ajax({
                type: "Post",
                url: "/Admin/Handler/RoleEdit.ashx?cid=" + cid + "&cname=" + cname + "&type=" + type + "&pid=" + addPid+"&murl="+murl,
                success: function (data) {
                    if (data == "0") {
                        $(".errfail").remove();
                        $(obj).parent().append("<span class='errfail'>失败</span>");
                        $(obj).parent().find(".imgadd").removeAttr("onclick");
                    }
                    else if (data == "-1") {
                        $(".errfail").remove();
                        $(obj).parent().append("<span class='errfail'>失败,该编号已存在</span>");
                        $(obj).parent().find(".imgadd").removeAttr("onclick");
                    }
                    else {
                        if (isDel) {
                            $(obj).parent().remove();
                        }
                        else {
                            if (addPid == 0) {
                                $(obj).parent().find("input[type='hidden']").val(data);
                            }
                            $(".errsuccess").remove();
                            $(".errfail").remove();
                            $(obj).parent().append("<span class='errsuccess'>保存成功</span>");
                        }
                    }
                },
                error: function (err) {
                    alert(err);
                }
            });
        }

        //删除一级分类
        function deleteFirstCategory(obj) {
            //            alert("暂不支持删除");
            //            return false;
            if (confirm("确认要删除？")) {
                var cid = $(obj).parent().find("input[type='hidden']").val();
                if (cid != "") {
                    deleteFirstLeval(cid);
                }
                $(obj).parent().next().remove();
                $(obj).parent().remove();
            }
        }
        function deleteFirstLeval(cid) {
            $.ajax({
                type: "Post",
                url: "/Admin/Handler/RoleEdit.ashx?cid=" + cid + "&type=5",
                success: function (data) {

                },
                error: function (err) {
                    alert(err);
                }
            });
        }
        
        function addCategory(obj) {
            var html = "<li class='list-group-item'>";
            html += " 角色编号:<input type='text' value='' class='form-control titiletxt txtname' />&nbsp;";
            html+=" 角色名称:<input type=\"text\" class=\"form-control titiletxt txturl\" />&nbsp;";
            html += "&nbsp;<input type='button' class='btn btn-xs btn-success' value='保存' onclick='saveChange(this,0,true,false,0)' /><img class='imgop delone' src='../Admin/img/Delete.png' title='删除' onclick='deleteFirstCategory(this)' />";
            html+="  <input type='hidden'/></li>";
            $(obj).parent().find(".width60").append(html);
        }


        $(document).ready(function () {

        });  

    </script>
</head>
<body>
     <form id="form1">
    <div id="mempay">
        <div class="control">
        </div>
        <div class="ui_table">
            <ul class="list-group width60">
                <%foreach (Model.Sys_Role item in list)
                  { %>
                <li class="list-group-item">
                    角色编号:<input type="text" class="form-control titiletxt txtname" readonly="readonly" value="<%=item.Id %>" />&nbsp;
                    角色名称:<input type="text" class="form-control titiletxt txturl" value="<%=item.Name %>" />&nbsp;
                    <input type="button" class="btn btn-xs btn-success" value="保存" onclick="saveChange(this,'<%=item.Id %>    ',false,false,0)" />
                    <img class="imgop delone" src="../Admin/img/Delete.png" title="删除" onclick="deleteFirstCategory(this)" />
                       <input type="hidden" value="<%=item.Id %>" />
                </li>
                <%} %>
            </ul>
            <img class="imgop" src="../Admin/img/plus.png" onclick="addCategory(this)" />
        </div>
    </div>
    </form>
    <script type="text/javascript">
        function search() {
            callhtml('SystemManage/RoleEdit.aspx?txtcode=' + $("#txtcode").val() + "&txtname=" + $("#txtname").val(), '菜单管理');
        }
    </script>
</body>
</html>
