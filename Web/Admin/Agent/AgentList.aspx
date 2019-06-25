<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgentList.aspx.cs" Inherits="Web.Admin.Member.AgentList" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>机构列表</title>
    <script type="text/javascript">
        tUrl = "Handler/AgentList.ashx";
        doSearch();
        var layIndex;
        //查询，加入参数
        function doSearch() {
            searchPram = [];
            searchPram.push({ name: $("#nMID").attr("id"), value: $("#nMID").val() });
            searchPram.push({ name: $("#nMName").attr("id"), value: $("#nMName").val() });
            //searchPram.push({ name: $("#txtKey").attr("id"), value: $("#txtKey").val() });
            //searchPram.push({ name: $("#ddl_City").attr("id"), value: $("#ddl_City").val() });
            //searchPram.push({ name: $("#ddl_Zone").attr("id"), value: $("#ddl_Zone").val() });
            searchPram.push({ name: $("#ddlRoleCode").attr("id"), value: $("#ddlRoleCode").val() });
            searchPram.push({ name: $("#nTitle").attr("id"), value: $("#nTitle").val() });
            //searchPram.push({ name: $("#nName").attr("id"), value: $("#nName").val() });
            SearchByCondition(searchPram);
        }
        function seeDetail(code)
        {
            callhtml('Agent/AgentEdit?id=' + code,'机构信息');
        }
        function seeNextList(code) {
            $("#agent").val(code);
            $("#onlyAgent").val('');
            doSearch();
        }
        function seeOnlyAgent() {
            $("#agent").val('');
            $("#onlyAgent").val('1');
            doSearch();
        }
       
        function deleteMember(code,obj)
        {
            if (!confirm("确定要删除吗？"))
                return;
            var userInfo = {
                type: 'deleteMember',
                code: code,
            };
            var result = GetAjaxString(userInfo);
            if (result == "1") {
                alert("删除成功");
                layer.close(layIndex);
                $("#divShow_" + code).parent().parent().remove();
                //doSearch();
                //$(obj).parent().parent().remove();
            }
            else if (result == "0") {
                alert("删除失败，请重试！");
            }
            else {
                alert(result);
            }
        }
        function reload(){
            callhtml('Agent/AgentList','代理商列表');
        }
        function resetFH(code,isFH, obj) {
            var objVal = $(obj).attr("value");
            if (!confirm("确定要"+objVal+"吗？"))
                return;
            var userInfo = {
                type: 'resetFH',
                code: code,
                flag:isFH
            };
            var result = GetAjaxString(userInfo);
            if (result == "1") {
                alert("操作成功");
                layer.close(layIndex);
                reload();
            }
            else if (result == "0") {
                alert("操作失败，请重试！");
            }
            else {
                alert(result);
            }
        }
        function setAuthority(code) {
            layer.close(layIndex);
            callhtml('Agent/Authority.aspx?id=' + code, '代理商权限设置');
        }
        function moreOperator(id, obj) {
            layIndex= layer.open({
                type: 1,
                shade: [0.8, '#393D49'],
                title: ['更多操作', 'font-size:14px;background:#2aaacb'],
                content: $('#divShow_'+id), //捕获的元素
                cancel: function (index) {
                    layer.close(index);
                }
            });
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div class="control">
            <div class="search" >
                <input type="button" value="查询" class="ssubmit btn btn-success" onclick="doSearch()" />
                  <input type="button" value="新增" class="ssubmit btn btn-info" onclick="callhtml('Agent/AgentEdit','添加机构')" />
                <input  id="nTitle"  placeholder="请输入机构代码"  type="text" class="sinput commonSearchKey" />
                <input  id="nMID"  placeholder="请输入机构账号"  type="text" class="sinput commonSearchKey" />
                <input  id="nMName"  placeholder="请输入机构名称"  type="text" class="sinput commonSearchKey" />
                   <select id="ddlRoleCode" class="commonSearchKey" runat="server">
                </select>
            </div>
        </div>
        <div class="ui_table">
            <table cellpadding="0" cellspacing="0" class="tabcolor" id="Result">
                  <thead>
                <tr>
                    <th>
                        序号
                    </th>
                    <th>
                        机构代码
                    </th>
                     <th>
                        登录账号
                    </th>
                     <th>
                        机构名称
                    </th>
                       <th>
                        所在地址
                    </th>
                      <th>
                        账户级别
                    </th>
                      <th>
                        学员数量
                    </th>
                     <th>
                        剩余学员名额
                    </th>
                     <th>
                        操作
                    </th>
                </tr>  </thead>
                  <tbody id="layerAppendView">
                </tbody>
            </table>
            <div class="ui_table_control">
                 <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent"></div>
            </div>
        </div>
          <script id="demo" type="text/html">
         {{# for (var i = 0; i<d.length; i++){ }}
          <tr> <td>{{ d[i].RowNumber }}</td><td>{{ d[i].Branch }}</td><td>{{ d[i].MID }}</td>
              <td>{{ d[i].MName }}</td>
              <td>{{ d[i].Province }}</td><td>{{ d[i].RoleCode }}</td>
              <td>{{ d[i].TradePoints }}</td><td>{{ d[i].LeaveTradePoints }}</td>
              <td>
                  <input type='button' class='btn btn-info' value='查看' onclick="seeDetail('{{ d[i].ID }}')"/>
              </td>
          </tr>
        {{# } }}
         </script>
    </div>
</body>
</html>
