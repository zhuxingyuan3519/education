<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogList.aspx.cs" Inherits="Web.Admin.Log.LogList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        tUrl = "Handler/LogList.ashx";
        $(function () {
            doSearch();
        })
        //查询，加入参数
        function doSearch() {
            searchPram = [];
            //searchPram.push({ name: tState, value: tState });
            searchPram.push({ name: $("#ddlLogType").attr("id"), value: $("#ddlLogType").val() });
            searchPram.push({ name: $("#nMID").attr("id"), value: $("#nMID").val() });
            searchPram.push({ name: $("#nPayBeginTime").attr("id"), value: $("#nPayBeginTime").val() });
            searchPram.push({ name: $("#nPayEndTime").attr("id"), value: $("#nPayEndTime").val() });
            SearchByCondition(searchPram);
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div class="control">
            <div class="search"  >
                <input type="button" value="查询" class="ssubmit btn btn-success" onclick="doSearch()" />
                日志类型：
                <select id="ddlLogType">
                    <option value="">--请选择--</option>
                       <option value="1">登录系统</option>
                       <option value="2">创建会员</option>
                       <option value="3">删除会员</option>
                       <option value="4">创建管理员</option>
                       <option value="5">删除管理员</option>
                       <option value="6">开通或禁用系统</option>
                       <option value="7">会员自主注册</option>
                     <option value="8">支付相关</option>
                      <option value="10">端口消费记录</option>
                </select>
                <input  id="nMID" name="txtKey" runat="server" placeholder="操作人账号"  type="text" class="sinput" />
                <input  id="nPayBeginTime" name="txtKey" placeholder="开始时间"  type="text" class="sinput"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
                <input  id="nPayEndTime" name="txtKey" placeholder="结束时间"  type="text" class="sinput"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
      
            </div>
        </div>
        <div class="ui_table">
            <table cellpadding="0" cellspacing="0" class="tabcolor" id="Result">
                 <thead> <tr>
                    <th>
                        序号
                    </th>
                     <%--  <th>
                        日志类型
                    </th>--%>
                       <th>
                        操作人账号
                    </th>
                      <th>
                        操作人级别
                    </th>
                    <th>
                        操作时间
                    </th>
                       <th>
                        操作说明
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
              <%--<td>{{ d[i].LType }}</td>--%>
                <td>{{ d[i].MCode }}</td>
                <td>{{ d[i].OperatorRole }}</td>
              <td>{{ d[i].CutTime }}</td>
              <td>{{ d[i].Remark }}</td>
          </tr>
        {{# } }}
         </script>
</body>
</html>
