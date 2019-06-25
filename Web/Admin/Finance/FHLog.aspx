<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FHLog.aspx.cs" Inherits="Web.Admin.Finance.FHLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        var tState = '<%=Request.QueryString["u"]%>';
        tUrl = "Handler/FHLogList.ashx";
        doSearch()
        //查询，加入参数
        function doSearch(isexport) {
            searchPram = [];
            searchPram.push({ name: tState, value: tState });
            searchPram.push({ name: $("#nMID").attr("id"), value: $("#nMID").val() });
            searchPram.push({ name: $("#nPayBeginTime").attr("id"), value: $("#nPayBeginTime").val() });
            searchPram.push({ name: $("#nPayEndTime").attr("id"), value: $("#nPayEndTime").val() });
            SearchByCondition(searchPram);
            countTotal();
        }

        function exportExcel() {
            searchPram = [];
            searchPram.push({ name: tState, value: tState });
            searchPram.push({ name: $("#nMID").attr("id"), value: $("#nMID").val() });
            searchPram.push({ name: $("#nPayBeginTime").attr("id"), value: $("#nPayBeginTime").val() });
            searchPram.push({ name: $("#nPayEndTime").attr("id"), value: $("#nPayEndTime").val() });
            var url = '/Admin/Handler/FHLogList.ashx?export=1';
            $.each(searchPram, function (n, value) {
                url += '&' + value.name + '=' + value.value;
            });
            window.open(url, "导出数据");
        }


        function getSearchPram() {
            var searchTotalPram = [];
            searchTotalPram.push({ name: tState, value: tState });
            searchTotalPram.push({ name: $("#nMID").attr("id"), value: $("#nMID").val() });
            searchTotalPram.push({ name: $("#nPayBeginTime").attr("id"), value: $("#nPayBeginTime").val() });
            searchTotalPram.push({ name: $("#nPayEndTime").attr("id"), value: $("#nPayEndTime").val() });
            return searchTotalPram;
        }


        function countTotal() {
            var searchTotalPram = getSearchPram();
            var rek = '<%=Request.RawUrl%>';
            //获取最后一个/和?之间的内容，就是请求的页面
           rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
           $.ajax({
               type: 'post', url: 'Finance/' + rek + '?Action=OTHER', data: searchTotalPram,
               success: function (info) {
                   $("#spTotal").html(info);
               }
           });
       }

    </script>
</head>
<body>
    <div id="mempay">
        <div class="control">
            <div class="search"  >
                <input type="button" value="查询" class="ssubmit btn btn-success" onclick="doSearch()" />
                   <input type="button" value="导出Excel" class="ssubmit btn btn-info"  onclick="exportExcel()" />
                <input  id="nMID" name="txtKey" placeholder="会员账号"  type="text" class="sinput" />
                <input  id="nPayBeginTime" name="txtKey" placeholder="分红开始时间"  type="text" class="sinput"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
                <input  id="nPayEndTime" name="txtKey" placeholder="分红结束时间"  type="text" class="sinput"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </div>
        </div>
        <div class="ui_table">
            <table cellpadding="0" cellspacing="0" class="tabcolor" id="Result">
                 <thead> <tr>
                    <th>
                        序号
                    </th>
                       <th>
                        账号
                    </th>
                       <th>
                        姓名
                    </th>
                       <th>
                        付款会员
                    </th>
                        <th>
                        奖励金额
                    </th>
                    <th>
                        奖励时间
                    </th>
                     <th>
                        支付方式
                    </th>
                     <th>
                        奖励事由
                    </th>
                </tr> </thead>
                  <tbody id="layerAppendView">
                </tbody>
            </table>
            <div class="ui_table_control">
                 <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent"></div>
            </div>
             <div style="float:left">
                 <div style="margin-top: -35px;font-size:14px">
                         合计：<span id="spTotal" style="color:red;font-size:14px"></span>
                 </div>
            </div>
        </div>
    </div>
     <script id="demo" type="text/html">
         {{# for (var i = 0; i<d.length; i++){ }}
          <tr> <td>{{ d[i].RowNumber }}</td>
                 <td>{{ d[i].FHMCode }}</td>
              <td>{{ d[i].MName }}</td>
              <td>{{ d[i].PayName }}</td>
              
              <td>{{ d[i].FHMoney }}</td>
              <td>{{ d[i].CutTime }}</td>
                 <td>{{ d[i].FHType }}</td>
              <td>{{ d[i].Remark }}</td>
          </tr>
        {{# } }}
         </script>
</body>
</html>
