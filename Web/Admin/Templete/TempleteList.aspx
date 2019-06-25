<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TempleteList.aspx.cs" Inherits="Web.Admin.Templete.TempleteList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .coverImg {
         width:100px;}
    </style>
    <script type="text/javascript">
        tState = '';
        tUrl = "Handler/TemplateList.ashx";
        SearchByCondition();
        function seeDetail(code) {
            callhtml('Templete/TempleteEdit.aspx?id=' + code, '模板信息');
        }
        function deleteMember(code, obj) {
            if (!confirm("确定要删除吗？"))
                return;
            var result = GetAjaxString('deleteTemplete', code);
            if (result == "1") {
                alert("删除成功");
                $(obj).parent().parent().remove();
            }
            else if (result == "0") {
                alert("删除失败，请重试！");
            }
            else {
                alert(result);
            }
        }
      
    </script>
</head>
<body>
    <div id="mempay">
        <div class="control">
            <div class="search" id="DivSearch" runat="server">
                 <select id="ddlType"  runat="server" class="commonSearchKey"></select>
                 <input  id="nTitle" placeholder="请输入模板名称"  type="text" class="sinput commonSearchKey" />
                   <input type="button" value="查询" class="btn btn-success" onclick="SearchByCondition()" />
                   <input type="button" value="新增" class="btn btn-danger" onclick="callhtml('Templete/TempleteEdit.aspx', '模板信息');" />
            </div>
        </div>
        <div class="ui_table">
            <table class="tabcolor" id="Result">
                <tr>
                    <th>
                        序号
                    </th>
                         <th>
                        缩略图
                    </th>
                    <th>
                        模板名称
                    </th>
                     <th>
                        所属类型
                    </th>
                     <th>
                        可用行业
                    </th>
                       <th>
                        价格
                    </th>
                     <th>
                        操作
                    </th>
                </tr>
            </table>
            <div class="ui_table_control">
                <div class="pn">
                </div>
                <div class="pagebar">
                    <div id="Pagination">
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
