<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HireList.aspx.cs" Inherits="Web.Admin.WebAdmin.HireList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        tUrl = "/Admin/Handler/HireList.ashx";
        doSearch()
        //查询，加入参数
        function doSearch() {
            searchPram = [];
            searchPram.push({ name:'hiretype', value: '0'});
            searchPram.push({ name: $("#txtKey").attr("id"), value: $("#txtKey").val() });
            SearchByCondition(searchPram);
        }
        function seeDetail(id) {
            callhtml("/Admin/WebAdmin/HireEdit.aspx?id=" + id, "分期配置");
        }
        function deleteArchive(id,obj) {
            if (confirm("确定要删除吗？")) {
                var userInfo = {
                    type: 'deleteHire',
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
            <div class="pay" onclick="callhtml('/Admin/WebAdmin/HireEdit','新增')">
                新增
            </div>
            <div class="search" >
                   <input type="button" value="查询" class="ssubmit btn btn-success " onclick="doSearch()" />
               选择角色 <select id="txtKey" runat="server" class="sinput commonSearchKey"></select>
            </div>
        </div>
        <div class="ui_table">
            <table cellpadding="0" cellspacing="0" class="tabcolor" id="Result">
                <thead><tr> <th>序号
                    </th>
                    <th>角色
                    </th>
                    <th>分期期数
                    </th>
                     <th>每期金额
                    </th>
                     <th>付款日期
                    </th>
                     <th>每期配送收费端口数量
                    </th>
                      <th>每期配送体验端口数量
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
               <td>{{ d[i].RoleCode}}</td>
              <td>{{ d[i].HireCount }}</td>
              <td>{{ d[i].EveryHireMoney }}</td>
              <td>{{ d[i].PayDate }}</td>
              <td>{{ d[i].TradePointCount }}</td>
              <td>{{ d[i].LeaveTradePointCount }}</td>
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
