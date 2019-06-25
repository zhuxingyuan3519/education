<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HirePurchaseDetail.aspx.cs" Inherits="Web.Admin.Finance.HirePurchaseDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        var id = '<%=Request.QueryString["id"]%>';
        tUrl = "Handler/HirePurchaseDetail.ashx";
        doSearch();
        //查询，加入参数
        function doSearch() {
            searchPram = [];
            //searchPram.push({ name: 'id', value: id });
            SearchByCondition(searchPram);
        }
        function moreOperate(code) {
            if (!confirm("确定要审核通过吗？"))
                return;
            var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            var userInfo = {
                type: 'getHireMoney',
                code: code,
            };
            var result = GetAjaxString(userInfo);
            layer.close(index);
            if (result == "1") {
                layer.alert("审核成功");
                doSearch();
            }
            else if (result == "2") {
                layer.alert("该条记录已审核过");
            } else if (result == "3") {
                layer.alert("您的端口数量不足，无法完成缴费");
            }
            else {
                layer.alert("审核失败，请重试");
            }
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div class="control hidden">
            <div class="search"  >
                <input type="button" value="返回" class="ssubmit btn btn-success" onclick="callhtml('/Admin/Finance/UserHirePurchase.aspx', '分期一览');" />
            </div>
        </div>
        <div class="ui_table">
            <table cellpadding="0" cellspacing="0" class="tabcolor" id="Result">
                 <thead> <tr>
                    <th>
                        序号
                    </th>
                       <th>
                        申请人
                    </th>
                     <th>
                        支付方式
                    </th>
                       <th>
                        申请级别
                    </th>
                     <th>
                        申请时间
                    </th>
                       <th>
                        申请地区
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
              <td>{{ d[i].HireId}}</td>
               <td>{{ d[i].RoleCode}}</td>
               <td>{{ d[i].CutTime}}</td>
              <td>{{ d[i].Remark }}</td>
               <td>
                     <input type='button' class="btn {{ d[i].HireType=='1'?' btn-danger ':' btn-info disabled' }}" value="{{d[i].HireType=='1'?'通过审核':' 已处理' }}" onclick="moreOperate('{{ d[i].Id }}')"/>
               </td>
          </tr>
        {{# } }}
         </script>
</body>
</html>
