<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CardList.aspx.cs" Inherits="Web.Admin.WebAdmin.CardList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        tUrl = "/Admin/Handler/CardList.ashx";
        searchPram = [];
        SearchByCondition(searchPram);
        //查询，加入参数
        function doSearch() {
            searchPram = [];
            searchPram.push({ name: $("#txtKey").attr("id"), value: $("#txtKey").val() });
            SearchByCondition(searchPram);
        }
        function seeDetail(id) {
            callhtml("/Admin/WebAdmin/CardEdit.aspx?id=" + id, "办卡地址");
        }
        function deleteArchive(id,obj) {
            if (confirm("确定要删除吗？")) {
                var userInfo = {
                    type: 'deleteCardLink',
                    pram: id,
                };
                var result = GetAjaxString(userInfo);
                if (result == "1") {
                    alert("删除成功");
                    doSearch();
                }
                else {
                    alert("删除失败，请重试！");
                }
            }
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div class="control">
            <div class="pay" onclick="callhtml('/Admin/WebAdmin/CardEdit','新增办卡地址')">
                新增
            </div>
            <div class="search" >
                   <input type="button" value="查询" class="ssubmit btn btn-success " onclick="doSearch()" />
               选择银行 <select id="txtKey" runat="server" class="sinput commonSearchKey"></select>
            </div>
        </div>
        <div class="ui_table">
            <table cellpadding="0" cellspacing="0" class="tabcolor" id="Result">
                <thead><tr> <th>序号
                    </th>
                    <th>银行
                    </th>
                    <th>办卡地址
                    </th>
                    <th>操作
                    </th>
                </tr></thead>
                 <tbody id="layerAppendView">
                </tbody>
            </table>
            <div class="ui_table_control">
                    <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent"></div>
            </div>
        </div>
           <script id="demo" type="text/html">
         {{# for (var i = 0; i<d.length; i++){ }}
          <tr> <td>{{ d[i].RowNumber }}</td>
               <td>{{ d[i].Remark}}</td>
              <td>{{ d[i].LinkUrl }}</td>
              <td>
                    <input type='button' class='btn btn-info' value='查看' onclick="seeDetail('{{ d[i].Id }}')"/>&nbsp;
                 <input type='button' class='btn btn-danger' value='删除' onclick="deleteArchive('{{ d[i].Id }}')"/>
              </td>
          </tr>
        {{# } }}
         </script>
    </div>
</body>
</html>
