<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TempList.aspx.cs" Inherits="Web.Admin.Temp.TempList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        tState = '';
        tUrl = "/Admin/Handler/TempList.ashx";
        searchPram = [];
        SearchByCondition(searchPram);
        //查询，加入参数
        function doSearch() {
            searchPram = [];
            searchPram.push({ name: $("#txtKey").attr("id"), value: $("#txtKey").val() });
            SearchByCondition(searchPram);
        }
        function showDetail(obj, id) {
            callhtml("/Admin/Temp/TempEidt.aspx?id=" + id, "模板编辑");
        }
        function deleteArchive(obj, id) {
            if (confirm("确定要删除吗？")) {
                var result = GetAjaxString('deleteTemplate', id);
                if (result == "1") {
                    alert("删除成功");
                    $(obj).parent().parent().remove();
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
            <div class="pay" onclick="callhtml('/Admin/Temp/TempEidt','添加模板')">
                新增
            </div>
            <div class="search" id="DivSearch" runat="server">
                <input type="button" value="查询" class="ssubmit btn btn-success " onclick="doSearch()" />
               选择银行 <select id="txtKey" runat="server" class="sinput commonSearchKey"></select>
            </div>
        </div>
        <div class="ui_table">
            <table cellpadding="0" cellspacing="0" class="tabcolor" id="Result">
                   <thead><tr>
                    <th>序号
                    </th>
                    <th>银行
                    </th>
                    <th>额度区间
                    </th>
                    <th>刷卡笔数
                    </th>
                    <th>卡内预留额度百分比
                    </th>
                    <th>操作
                    </th>
               </tr>  </thead>
                      <tbody id="layerAppendView">
                </tbody>
            </table>
            <div class="ui_table_control">
                  <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent"></div>
            </div>
        </div>
    </div>
    <script id="demo" type="text/html">
         {{# for (var i = 0; i<d.length; i++){ }}
          <tr> <td>{{ d[i].RowNumber }}</td><td>{{ d[i].Bank }}</td><td>{{ d[i].MoneyToMoney }}</td>
              <td>{{ d[i].CostCount }}</td>
              <td>{{ d[i].LeavePencent }}</td>
              <td>
                  <input type='button' class='btn btn-info' value='查看' onclick="showDetail(this,'{{ d[i].Code }}')"/>&nbsp;
                <input type='button' class='btn btn-danger' value='删除' onclick="deleteArchive(this,'{{ d[i].Code }}', this)"/>
              </td>
          </tr>
        {{# } }}
         </script>
</body>
</html>
