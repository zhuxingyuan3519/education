<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberTrainList.aspx.cs" Inherits="Web.Admin.TrainManage.MemberTrainList" %>
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
        tUrl = "Handler/MemberTrainList.ashx";
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
            searchPram.push({ name: $("#nTitle").attr("id"), value: $("#nTitle").val() });
            searchPram.push({ name: $("#nName").attr("id"), value: $("#nName").val() });
            searchPram.push({ name: $("#ddlRoleCode").attr("id"), value: $("#ddlRoleCode").val() });
            searchPram.push({ name: $("#nServiceMID").attr("id"), value: $("#nServiceMID").val() });
            searchPram.push({ name: 'hidType', value: $("#ddlRoleCode").val() });
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
                type: 'deleteMemberTrain',
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

        function trunLogPage() {
            layer.closeAll();
            var mid = $("#btnSeeLog").data("mid");
            callhtml('Log/LogList.aspx?mid=' + mid, '操作日志');
        }

        function seeRank() {
            var url = '/Admin/Member/StructNet.aspx';
            window.open(url);
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div class="control">
               <div class="search" style="float:left;color:red">
                   "闪现频率"为0就是全部显示，不闪现。
               </div>
            <div class="search">
                    <input  id="nRegistBeginTime" placeholder="训练开始时间"  type="text" class="sinput commonSearchKey"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"/>
                      <input  id="nRegistEndTime" placeholder="训练结束时间"  type="text" class="sinput commonSearchKey"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"/>
                <input  id="nTitle" placeholder="账号"  type="text" class="sinput commonSearchKey" />
                      <input  id="nName" placeholder="姓名"  type="text" class="sinput commonSearchKey" />
                <select id="ddlRoleCode" class="commonSearchKey" runat="server">
                  <option value="">--训练项目--</option>
                    <option value="1">初级混合词训练</option>
                      <option value="5">高级混合词训练</option>
                    <option value="2">数字训练</option>
                    <option value="3">扑克牌训练</option>
                    <option value="4">字母训练</option>
                </select>
                   <input type="button" value="查询" class="btn btn-success" onclick="doSearch()" />
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
                        账号
                    </th>
                      <th>
                        姓名
                    </th>
                     <th>
                        训练项目
                    </th>
                     <th>
                        训练数量
                    </th>
                      <th>
                        闪现频率
                    </th>
                    <th>
                        训练开始时间
                    </th>
                     <th>
                        记忆时间
                    </th>
                     <th>
                        答题时间
                    </th>
                      <th>
                        复习时间
                    </th>
                       <th>
                        正确数量
                    </th>
                       <th>
                        错误数量
                    </th>
                       <th>
                        正确率
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

    </div>
       <script id="demo" type="text/html">
         {{# for (var i = 0; i<d.length; i++){ }}
          <tr> <td>{{ d[i].RowNumber }}</td><td>{{ d[i].MID }}</td><td>{{ d[i].MName }}</td>
              <td>{{ d[i].CodeType }}</td><td>{{ d[i].TrainCount }}</td>
              <td>{{ d[i].ShowTime }}</td>  
               <td>{{ d[i].CutTime }}</td>
                    <td>{{ d[i].MemoryTimeString }}</td>
              <td>{{ d[i].AnswerTimeString }}</td>
               <td>{{ d[i].ReviewTimeString }}</td>
               <td>{{ d[i].CorrectCount }}</td>
                <td>{{ d[i].ErrorCount }}</td>
               <td>{{ d[i].Remark }}</td>
              <td>
                <input type='button' class='btn btn-danger' value='删除' onclick="deleteMember('{{ d[i].Code }}', this)"/>
</td>
          </tr>
        {{# } }}
         </script>
</body>
</html>
