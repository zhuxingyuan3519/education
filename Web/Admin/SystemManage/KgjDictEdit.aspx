<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KgjDictEdit.aspx.cs" Inherits="Web.Admin.SystemManage.KgjDictEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .width50, .titiletxt
        {
            width: 20%;
        }
        .titiletxt
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
            width: 60%;
        }
        .delone
        {
            width: 20px;
        }
        .imgadd
        {
            float: right;
        }
    </style>
    <script type="text/javascript">
        function showSubCategory(obj) {
            var subCate = $(obj).parent().next();
            if (subCate.hasClass("hidden")) {
                $(obj).attr("src", "../img/del.png");
                subCate.removeClass("hidden");
            }
            else {
                $(obj).attr("src", "../img/plus.png");
                subCate.addClass("hidden");
            }
        }
        //cid:类型的Id,isAdd:是否是新增。isDel:是否是删除。addPid:是否是一级类型
        function saveChange(obj, cid, isAdd, isDel, addPid) {
            var cname = escape($.trim($(obj).parent().find(".txtname").val()));
            var type = isAdd ? 3 : 1;
            if (isDel) {
                if (confirm("确定要删除吗？")) {
                    type = 2;
                } else {
                    return false;
                }
            }
            else {
                var ccode = escape($.trim($(obj).parent().find(".txtcode").val()));
                if (ccode == "") {
                    alert("编号不能为空"); return false;
                }
                if (cname == "") {
                    alert("名称不能为空"); return false;
                }
            }

            $.ajax({
                type: "Post",
                url: "/Admin/SystemManage/Handler/DictEdit.ashx?cid=" + cid + "&cname=" + cname + "&type=" + type + "&pid=" + addPid + "&ccode=" + ccode,
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
                                $(obj).parent().next().find("input[type='hidden']").val(ccode);
                            }
                            else {
                                $(obj).parent().append("<img class='imgop' src='../Admin/img/del.png' onclick=\"deleteSecondLeval(this,'" + ccode + "','" + $(obj).parent().parent().parent().find("input[type='hidden']").val() + "')\">");
                            }
                            $(".errsuccess").remove();
                            $(obj).parent().append("<span class='errsuccess'>保存成功</span>");
                            $(obj).parent().find(".imgadd").attr("onclick", "showSubCategory(this)");
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
            if (confirm("删除一级分类之后将删除该分类下的所有分类，是否要继续删除？")) {
                var cid = $(obj).parent().next().find("input[type='hidden']").val();
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
                url: "/Admin/SystemManage/Handler/DictEdit.ashx?cid=" + cid + "&type=5",
                success: function (data) {

                },
                error: function (err) {
                    alert(err);
                }
            });
        }
        function deleteSecondLeval(obj, cid, parentcid) {
            if (confirm("确定要删除吗？")) {
                $.ajax({
                    type: "Post",
                    url: "/Admin/SystemManage/Handler/DictEdit.ashx?cid=" + cid + "&pid=" + parentcid + "&type=6",
                    success: function (data) {
                        if (data != "0") {
                            $(obj).parent().remove();
                        }
                    },
                    error: function (err) {
                        alert(err);
                    }
                });
            }
        }
        //新增二级分类
        function addSubCategory(obj) {
            var pid = $(obj).parent().find("input[type='hidden']").val();
            //            alert(pid);
            //            return false;
            var html = "<li class='list-group-item'>";
            html += " 编号:<input type='text'  class='form-control subtitletxt txtcode' /> 名称:<input type='text' value='' class='form-control subtitletxt txtname'/>&nbsp; <input type='button' class='btn btn-xs btn-success' value='保存' onclick=\"saveChange(this,0,true,false,'" + pid + "')\" /></li>";
            $(obj).parent().find(".subuilist").append(html);
        }
        function addCategory(obj) {
            var html = "<li class='list-group-item'>";
            html += " 一级分类： 编号:<input type='text' value='' class='form-control titiletxt txtcode'/> 名称:<input type='text' value='' class='form-control titiletxt txtname' />";
            html += "&nbsp;<input type='button' class='btn btn-xs btn-success' value='保存' onclick='saveChange(this,0,true,false,0)' /> <img class='imgop imgadd' src='../Admin//img/plus.png' onclick='showSubCategory(this)' /><img class='imgop delone' src='../Admin//img/Delete.png' title='删除' onclick='deleteFirstCategory(this)' /></li>";
            html += "<li class='list-group-item hidden'><span class='subui'>  二级分类：</span><input type='hidden' value='' />";
            html += "<ul class='list-group subui subuilist'></ul><img class='imgop' src='../img/plus.png' onclick='addSubCategory(this)' /></li>";
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
            <div class="search" id="DivSearch" runat="server">
                <input type="button" value="查询" class="ssubmit" onclick="search()" />
                <input name="txtcode" id="txtcode" placeholder="请输入一级分类编号" type="text" class="sinput" />
                <input name="txtname" id="txtname" placeholder="请输入一级分类名称" type="text" class="sinput" />
            </div>
        </div>
        <div class="ui_table">
            <ul class="list-group width60">
                <%foreach (Model.CM_Dict item in GetFirstLeavelDict("0"))
                  { %>
                <li class="list-group-item">一级分类：编号:
                    <input type="text" class="form-control titiletxt txtcode" value="<%=item.Code %>" />
                    名称:<input type="text" class="form-control titiletxt txtname" value="<%=item.Name %>" />&nbsp;
                    <input type="button" class="btn btn-xs btn-success" value="保存" onclick="saveChange(this,<%=item.Id %>,false,false,0)" />
                    <img class="imgop imgadd" src="../img/plus.png" onclick="showSubCategory(this)" />
                    <img class="imgop delone" src="../img/Delete.png" title="删除" onclick="deleteFirstCategory(this)" />
                </li>
                <li class="list-group-item hidden"><span class="subui">二级分类：</span>
                    <input type="hidden" value="<%=item.Code %>" />
                    <ul class="list-group subui subuilist">
                        <%foreach (Model.CM_Dict subItem in GetSecondLeavelDict(item.Code))
                          { %>
                        <li class="list-group-item">编号:<input type="text" value="<%=subItem.Code %>" class="form-control subtitletxt txtcode" />
                            名称:<input type="text" value="<%=subItem.Name %>" class="form-control subtitletxt txtname" />
                            <input type="button" class="btn btn-xs btn-success" value="保存" onclick="saveChange(this,<%=subItem.Id %>,false,false,0)" />
                            <img class="imgop" src="../img/del.png" onclick="saveChange(this,<%=subItem.Id %>,false,true,0)" />
                        </li>
                        <%} %>
                    </ul>
                    <img class="imgop" src="../img/plus.png" onclick="addSubCategory(this)" />
                </li>
                <%} %>
            </ul>
            <img class="imgop" src="/Admin//img/plus.png" onclick="addCategory(this)" />
        </div>
    </div>
    </form>
    <script type="text/javascript">
        function search() {
            callhtml('/Admin/SystemManage/DictEdit?txtcode=' + $("#txtcode").val() + "&txtname=" + $("#txtname").val(), '数据字典维护');
        }
    </script>
</body>
</html>
