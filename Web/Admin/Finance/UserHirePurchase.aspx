<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserHirePurchase.aspx.cs" Inherits="Web.Admin.Finance.UserHirePurchase" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        tUrl = "Handler/HireList.ashx";
        doSearch();
        //查询，加入参数
        function doSearch() {
            searchPram = [];
            searchPram.push({ name: 'hiretype', value: '1' });
            searchPram.push({ name: $("#nMID").attr("id"), value: $("#nMID").val() });
            SearchByCondition(searchPram);
        }
        function seeDetail(id) {
            callhtml("/Admin/Finance/HirePurchaseDetail.aspx?id=" + id, "分期详情");
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div class="control">
            <div class="search"  >
                <input type="button" value="查询" class="ssubmit btn btn-success" onclick="doSearch()" />
                <input  id="nMID" name="txtKey" placeholder="会员账号"  type="text" class="sinput" />
            </div>
        </div>
        <div class="ui_table">
            <table cellpadding="0" cellspacing="0" class="tabcolor" id="Result">
                 <thead> <tr>
                    <th>
                        序号
                    </th>
                       <th>
                        会员账号
                    </th>
                     <th>
                        会员级别
                    </th>
                       <th>
                        分期时间
                    </th>
                       <th>
                        总期数
                    </th>
                     <th>
                        每期金额
                    </th>
                       <th>
                        剩余期数
                    </th>
                       <th>
                        状态
                    </th>
                       <th>
                        操作
                    </th>
                </tr> </thead>
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
         <tr> <td>{{ d[i].RowNumber }}</td>
              <td>{{ d[i].UserCode}}</td>
               <td>{{ d[i].RoleCode}}</td>
               <td>{{ d[i].CutTime}}</td>
              <td>{{ d[i].HireCount }}</td>
              <td>{{ d[i].EveryHireMoney }}</td>
                <td>{{ d[i].LeaveHireCount }}</td>
                 <td>{{ d[i].IsAllPay }}</td>
              <td>
                <input type='button' class='btn btn-info' value='查看' onclick="seeDetail('{{ d[i].Id }}')"/>&nbsp;
                <%-- <input type='button' class='btn btn-danger' value='删除' onclick="deleteArchive('{{ d[i].Id }}')"/>--%>
              </td>
          </tr>
        {{# } }}
         </script>
</body>
</html>
