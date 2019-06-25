<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberList.aspx.cs" Inherits="Web.Admin.Member.MemberList" %>
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
        tUrl = "Handler/MemberList.ashx";
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
            searchPram.push({ name: $("#nMTJTitle").attr("id"), value: $("#nMTJTitle").val() });
            searchPram.push({ name: $("#nMTJName").attr("id"), value: $("#nMTJName").val() });
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
        function moreOperate(code, IsShowIndex, mid, obj, isCanSeePlan) {
            //js绑定Radio是否在首页展示
            var radios = document.getElementsByName("txt_IsShowIndex");
            if (IsShowIndex == 'True') {
                radios[0].checked = true;
            } else {
                radios[1].checked = true;
            }
            var radiosCanSee = document.getElementsByName("txt_IsCanSeePlanDetail");
            if (isCanSeePlan == '1') {
                if (typeof (radiosCanSee[0]) != 'undefined')
                    radiosCanSee[0].checked = true;
            } else {
                if (typeof (radiosCanSee[1]) != 'undefined')
                    radiosCanSee[1].checked = true;
            }
            //$("input:radio[value='" + $("#HidRadio").val() + "']").attr('checked', 'true');
            $("#btnSeeLog").data("mid", mid);

            layer.open({
                type: 1,
                shade: [0.8, '#393D49'],
                title: ['操作', 'font-size:18px;background:#5bc0de'],
                content: $('#layLoginUI'), //捕获的元素
                btn: ['保存', '取消'],
                area: ['320px', '195px'],
                yes: function (index) {
                    //ajax数据
                    var userInfo = {
                        type: 'UpdateMemberStatus',
                        valCode: code,
                        valMID: mid,
                        valShowIndex: $('input:radio[name="txt_IsShowIndex"]:checked').val(),
                        valSeePlan: $('input:radio[name="txt_IsCanSeePlanDetail"]:checked').val()
                    };
                    var result = GetAjaxString(userInfo);
                    if (result == "1") //保存成功
                    {
                        layer.alert("保存成功！", { icon: 6 });
                        doSearch();
                        layer.close(index);
                    }
                    else
                        layer.alert('操作失败，请重试');
                },
                cancel: function (index) {
                    layer.close(index);
                }
            });
        }
        function trunLogPage() {
            layer.closeAll();
            var mid = $("#btnSeeLog").data("mid");
            callhtml('Log/LogList.aspx?mid=' + mid, '操作日志');
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
                    <input  id="nRegistBeginTime" placeholder="注册开始时间"  type="text" class="sinput commonSearchKey"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"/>
                      <input  id="nRegistEndTime" placeholder="注册结束时间"  type="text" class="sinput commonSearchKey"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"/>
                <input  id="nServiceMID" placeholder="归属服务中心"  type="text" class="sinput commonSearchKey hidden" /> 
                  <input  id="nMTJTitle" placeholder="推荐人账号"  type="text" class="sinput commonSearchKey" />
                      <input  id="nMTJName" placeholder="推荐人姓名"  type="text" class="sinput commonSearchKey" />
                <input  id="nTitle" placeholder="账号"  type="text" class="sinput commonSearchKey" />
                      <input  id="nName" placeholder="姓名"  type="text" class="sinput commonSearchKey" />
                <input type="hidden" id="hidType" runat="server" class="sinput commonSearchKey"  />
                <select id="ddlRoleCode" class="commonSearchKey" runat="server">
                </select>
                   <input type="button" value="查询" class="btn btn-success" onclick="doSearch()" />
                <input type="button" value="新增" class="btn btn-danger" onclick="addNew()" />
                   <input type="button" value="查看排位" class="btn btn-info" onclick="seeRank()" />
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
                        注册时间
                    </th>
                       <th>
                        联系电话
                    </th>
                       <th>
                        会员级别
                    </th>
                    <th>
                        推荐人账号
                    </th>
                        <th>
                        推荐人姓名
                    </th>
                 
                       <th>
                        账户余额
                    </th>
           
                       <%if (IsHasPower("CKHYMM"))
                         { %>
                 <th>密码</th>
                 <%} %>
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

            <ul class="layer_notice layui-layer-wrap" id="layLoginUI" style="display: none;">
                    <li style="height:10px;width:100%"></li>
                    <li class="tdlable" style="margin-left: 20px" >端口开通或禁用：
                        <input id="txt_IsShowIndexYes" name="txt_IsShowIndex" value="True" type="radio" />开通 &emsp;&emsp;
                        <input id="txt_IsShowIndexNo" name="txt_IsShowIndex"  value="False" type="radio" />禁用
                    </li>
                <%if (TModel.Role.IsAdmin)
                  { %>
                  <li class="tdlable hidden" style="margin-left: 20px" >是否能查看规划表：
                        <input id="IsCanSeePlanDetail1" name="txt_IsCanSeePlanDetail" value="1" type="radio" />是 &emsp;&emsp;
                        <input id="IsCanSeePlanDetail0" name="txt_IsCanSeePlanDetail"  value="0" type="radio" />否
                    </li>
                    <li class="tdlable" style="margin-left: 20px" ><input type="button" id="btnSeeLog" data-mid="" value="查看日志" class="btn btn-info btn-sm" onclick="trunLogPage()" />
                    </li>
                <%} %>
                </ul>
    </div>
       <script id="demo" type="text/html">
         {{# for (var i = 0; i<d.length; i++){ }}
          <tr> <td>{{ d[i].RowNumber }}</td><td>{{ d[i].MID }}</td><td>{{ d[i].MName }}</td>
              <td>{{ d[i].CutTime }}</td><td>{{ d[i].Tel }}</td>
              <td>{{ d[i].RoleCode }}</td>  
               <td>{{ d[i].MTJ }}</td>
                    <td>{{ d[i].MTJName }}</td>
              <td>{{ d[i].MSH }}</td>


                <%if (IsHasPower("CKHYMM"))
                  { %>
                 <td>{{ d[i].Password }}</td>
                 <%} %>
              <td>
                  <input type='button' class='btn btn-info' value='查看' onclick="seeDetail('{{ d[i].ID }}')"/>&nbsp;
            <%if (IsHasPower("SCHYXX"))
              { %>
                <input type='button' class='btn btn-danger' value='删除' onclick="deleteMember('{{ d[i].ID }}', this)"/>
                    <%} %>

                  <input type='button' class="btn btn-success hidden" value="更多操作" onclick="moreOperate('{{ d[i].ID }}', '{{ d[i].MState }}', '{{ d[i].MID }}', this, '{{ d[i].Salt }}')"/>
              </td>
          </tr>
        {{# } }}
         </script>
</body>
</html>
