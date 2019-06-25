<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrizeList.aspx.cs" Inherits="Web.Admin.Member.PrizeList" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        ul li {
            list-style: none;
        }

        .commonSearchKey {
            width: 100px;
            padding-left: 2px !important;
            padding-right: 2px !important;
        }
    </style>
    <script type="text/javascript">
        tState = '';
        tUrl = "Handler/PrizeList.ashx";
        //searchPram = [];
        //SearchByCondition(searchPram);

        $(function () {
            doSearch();
        })

        //查询，加入参数
        function doSearch() {
            searchPram = [];
            searchPram.push({ name: $("#nRegistBeginTime").attr("id"), value: $("#nRegistBeginTime").val() });
            searchPram.push({ name: $("#nRegistEndTime").attr("id"), value: $("#nRegistEndTime").val() });
            //searchPram.push({ name: $("#nMTJTitle").attr("id"), value: $("#nMTJTitle").val() });
            //searchPram.push({ name: $("#nMTJName").attr("id"), value: $("#nMTJName").val() });
            searchPram.push({ name: $("#nTitle").attr("id"), value: $("#nTitle").val() });
            searchPram.push({ name: $("#nName").attr("id"), value: $("#nName").val() });
            searchPram.push({ name: $("#ddlRoleCode").attr("id"), value: $("#ddlRoleCode").val() });
            searchPram.push({ name: $("#ddlIsGet").attr("id"), value: $("#ddlIsGet").val() });
            //searchPram.push({ name: 'hidType', value: $("#ddlRoleCode").val() });
            SearchByCondition(searchPram);
        }
        var hidType = $("#hidType").val();
        var addMsg = "学员";
        if (hidType == "Training") {
            addMsg = "机构";
        }
        else if (hidType == "Agent") {
            addMsg = "代理商";
        }
        function seeDetail(code) {
            callhtml('Member/MemberEdit?id=' + code + '&type=' + $("#hidType").val(), addMsg + '信息');
        }
        function addNew() {
            callhtml('Member/MemberEdit?type=' + $("#hidType").val(), '添加' + addMsg);
        }

        function deleteMember(code, obj) {
            if (!confirm("确定要删除吗？"))
                return;
            var userInfo = {
                type: 'deleteMember',
                code: code,
            };
            var result = GetAjaxString(userInfo);
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

        function seeRank()
        {
            var url = '/Admin/Member/StructNet.aspx';
            window.open(url);
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div class="control">
            <div class="search">
                    <input  id="nRegistBeginTime" placeholder="抽奖开始时间"  type="text" class="sinput commonSearchKey"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"/>
                      <input  id="nRegistEndTime" placeholder="抽奖结束时间"  type="text" class="sinput commonSearchKey"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"/>
               
                 <input  id="nTitle" placeholder="账号"  type="text" class="sinput commonSearchKey" />
                  <input  id="nName" placeholder="姓名"  type="text" class="sinput commonSearchKey" />

                <select id="ddlRoleCode" class="commonSearchKey" >
                    <option value="">红包类型</option>
                    <option value="1">游戏红包</option>
                    <option value="2">代言红包</option>
                </select>
                     <select id="ddlIsGet" class="commonSearchKey">
                    <option value="">全部</option>
                    <option value="1">中奖</option>
                    <option value="0">未中</option>
                </select>
                   <input type="button" value="查询" class="btn btn-success" onclick="doSearch()" />
                <input type="button" value="新增" class="btn btn-danger" onclick="addNew()" />
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
                        登录账号
                    </th>
                     <th>
                        姓名
                    </th>
                      <th>
                        抽奖时间
                    </th>
                       <th>
                        是否中奖
                    </th>
                       <th>
                        中奖金额
                    </th>
                    <th>
                        红包类型
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
          <tr> <td>{{ d[i].RowNumber }}</td><td>{{ d[i].MID }}</td><td>{{ d[i].MName }}</td>
              <td>{{ d[i].PrizeTime }}</td><td>{{ d[i].IsGet }}</td>
              <td>{{ d[i].PrizeMoney }}</td>  
               <td>{{ d[i].TypeName }}</td>
          </tr>
        {{# } }}
         </script>
</body>
</html>
