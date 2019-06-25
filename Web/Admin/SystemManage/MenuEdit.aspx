<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuEdit.aspx.cs" Inherits="Web.Admin.SystemManage.MenuEdit" %>

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
        .txturl,txtname {
         width:15%;}
        .txturl {
       display:inline;}
          .txticon {
         width:100px;display:inline;}
            .txtindex {
         width:70px;display:inline;}
    </style>
    <script type="text/javascript">
        var mtype='<%=Request.QueryString["mtype"]%>';
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
        //cid:某一行的Id,isAdd:是否是新增。isDel:是否是删除。addPid:父Id
        function saveChange(obj, cid, isAdd, isDel, addPid) {
            var  privageid = $.trim($(obj).parent().find(".txtid").val());
            var cname = $.trim($(obj).parent().find(".txtname").val());
            var murl = $.trim($(obj).parent().find(".txturl").val());
            var micon = $.trim($(obj).parent().find(".txticon").val());
            var mindex = $.trim($(obj).parent().find(".txtindex").val());
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
                if (privageid == "") {
                    alert("权限代码不能为空"); return false;
                }
            }

            $.ajax({
                type: "Post",
                url: "/Admin/Handler/MenuEdit.ashx?mtype="+mtype+"&privageid="+privageid+"&cid=" + cid + "&cname=" + cname + "&type=" + type + "&pid=" + addPid+"&murl="+murl+"&micon="+micon+"&mindex="+mindex,
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
                                $(obj).parent().next().find("input[type='hidden']").val(data);
                            }
                            else {
                                $(obj).parent().append("<img class='imgop' src='../Admin/img/del.png' onclick=\"deleteSecondLeval(this,'" + data + "','" + $(obj).parent().parent().parent().find("input[type='hidden']").val() + "')\">");
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
                url: "/Admin/Handler/MenuEdit.ashx?cid=" + cid + "&type=5",
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
                    url: "/Admin/Handler/MenuEdit.ashx?cid=" + cid + "&pid=" + parentcid + "&type=6",
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
            html += "权限代码:<input type='text' value='' class='form-control subtitletxt txtid'/>";
            html += "名称:<input type='text' value='' class='form-control subtitletxt txtname'/>";
            html+=" 地址:<input type=\"text\" class=\"form-control subtitiletxt txturl\" />&nbsp;";
            html+=" 图标:<input type=\"text\" class=\"form-control subtitiletxt txticon\"  />&nbsp;";
            html+=" 排序:<input type=\"text\" class=\"form-control subtitiletxt txtindex\" />&nbsp;";
            html += "<input type='button' class='btn btn-xs btn-success' value='保存' onclick=\"saveChange(this,0,true,false,'" + pid + "')\" /></li>";
            $(obj).parent().find(".subuilist").append(html);
        }
        function addCategory(obj) {
            var html = "<li class='list-group-item'>";
            html += "<span class='mtype2hidden'>一级分类：<br /></span>";
            html += "权限代码:<input type='text' value='' class='form-control subtitletxt txtid'/>";
            html += "名称:<input type='text' value='' class='form-control titiletxt txtname' />";
            html+=" <span class='mtype2hidden'>地址:</span><input type=\"text\" class=\"form-control titiletxt txturl mtype2hidden\" />&nbsp;";
            html+=" <span class='mtype2hidden'>图标:</span><input type=\"text\" class=\"form-control titiletxt txticon mtype2hidden\"  />&nbsp;";
            html+=" <span class='mtype2hidden'>排序:</span><input type=\"text\" class=\"form-control titiletxt txtindex mtype2hidden\" />&nbsp;";
            html += "&nbsp;<input type='button' class='btn btn-xs btn-success' value='保存' onclick='saveChange(this,0,true,false,0)' /> <img class='imgop imgadd mtype2hidden' src='../Admin/img/plus.png' onclick='showSubCategory(this)' /><img class='imgop delone' src='../Admin/img/Delete.png' title='删除' onclick='deleteFirstCategory(this)' /></li>";
            html += "<li class='list-group-item hidden'><span class='subui'>  二级分类：</span><input type='hidden' value='' />";
            html += "<ul class='list-group subui subuilist'></ul><img class='imgop' src='../Admin/img/plus.png' onclick='addSubCategory(this)' /></li>";
            $(obj).parent().find(".width60").append(html);
            if(mtype=='2')
            {
                $(".mtype2hidden").hide();
            }
        }

        $(document).ready(function () {
            if(mtype=='2')
            {
                $(".mtype2hidden").hide();
            }
        });  

    </script>
</head>
<body>
     <form id="form1">
    <div id="mempay">
        <div class="control">
            <div class="search" id="DivSearch" runat="server">
                <input type="button" value="查询" class="ssubmit" onclick="search()" />
                <input name="txtcode" id="txtcode" placeholder="一级分类名称" type="text" class="sinput" />
                <%--<input name="txtname" id="txtname" placeholder="请输入一级分类名称" type="text" class="sinput" />--%>
            </div>
        </div>
        <div class="ui_table">
            <ul class="list-group width60">
                <%foreach (Model.Sys_Privage item in GetFirstLeavelDict("0"))
                  { %>
                <li class="list-group-item"><span class='mtype2hidden'>一级分类：<br /></span>
                       权限代码:<input type="text" class="form-control titiletxt txtid"    value="<%=item.Id %>" />&nbsp;
                    名称:<input type="text" class="form-control titiletxt txtname" value="<%=item.Name %>" />&nbsp;
                    <span class='mtype2hidden'>地址:</span><input type="text" class="form-control titiletxt txturl mtype2hidden" value="<%=item.URL %>" />&nbsp;
                    <span class='mtype2hidden'>图标:</span><input type="text" class="form-control titiletxt txticon mtype2hidden" value="<%=item.Icon %>" />&nbsp;
                    <span class='mtype2hidden'>排序:</span><input type="text" class="form-control titiletxt txtindex mtype2hidden" value="<%=item.MenuIndex %>" />&nbsp;
                    <input type="button" class="btn btn-xs btn-success" value="保存" onclick="saveChange(this,'<%=item.Id %>',false,false,0)" />
                    <img class="imgop imgadd mtype2hidden" src="../Admin/img/plus.png" onclick="showSubCategory(this)" />
                    <img class="imgop delone" src="../Admin/img/Delete.png" title="删除" onclick="deleteFirstCategory(this)" />
                </li>
                <li class="list-group-item hidden"><span class="subui">二级分类：</span>
                    <input type="hidden" value="<%=item.Id %>" />
                    <ul class="list-group subui subuilist">
                        <%foreach (Model.Sys_Privage subItem in GetSecondLeavelDict(item.Id))
                          { %>
                        <li class="list-group-item">
                            权限代码:<input type="text" class="form-control titiletxt txtid"    value="<%=subItem.Id %>" />&nbsp;
                            名称:<input type="text" value="<%=subItem.Name %>" class="form-control subtitletxt txtname" />
                          <abbr class="mtype2">   地址:<input type="text" class="form-control titiletxt txturl" value="<%=subItem.URL %>" />&nbsp;
                    图标:<input type="text" class="form-control titiletxt txticon" value="<%=subItem.Icon %>" />&nbsp;
                    排序:<input type="text" class="form-control titiletxt txtindex" value="<%=subItem.MenuIndex %>" />&nbsp;</abbr>
                            <input type="button" class="btn btn-xs btn-success" value="保存" onclick="saveChange(this,'<%=subItem.Id %>    ',false,false,'<%=subItem.ParentCode %>    ')" />
                            <img class="imgop" src="../Admin/img/del.png" onclick="deleteSecondLeval(this,'<%=subItem.Id %>','<%=subItem.ParentCode %>')" />
                        </li>
                        <%} %>
                    </ul>
                    <img class="imgop" src="../Admin/img/plus.png" onclick="addSubCategory(this)" />
                </li>
                <%} %>
            </ul>
            <img class="imgop" src="../Admin/img/plus.png" onclick="addCategory(this)" />
        </div>
    </div>
    </form>
    <script type="text/javascript">
        function search() {
            callhtml('SystemManage/MenuEdit.aspx?mtype='+mtype+'&txtcode=' + $("#txtcode").val() + "&txtname=" + $("#txtname").val(), '菜单管理');
        }
    </script>
</body>
</html>
