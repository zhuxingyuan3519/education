<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUpList.aspx.cs" Inherits="Web.Admin.Member.SignUpList" %>
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
        tUrl = "Handler/SignUpList.ashx";
        searchPram = [];
        SearchByCondition(searchPram);
        //查询，加入参数
        function doSearch() {
            searchPram = [];
            searchPram.push({ name: $("#nRegistBeginTime").attr("id"), value: $("#nRegistBeginTime").val() });
            searchPram.push({ name: $("#nRegistEndTime").attr("id"), value: $("#nRegistEndTime").val() });
            searchPram.push({ name: $("#nName").attr("id"), value: $("#nName").val() });
            searchPram.push({ name: $("#ddlTraing").attr("id"), value: $("#ddlTraing").val() });
            searchPram.push({ name: $("#ddlCourse").attr("id"), value: $("#ddlCourse").val() });

            SearchByCondition(searchPram);
        }
        function seeDetail(code) {
            callhtml('Member/SignUp?id=' + code, '会员信息');
        }
        function addNew() {
            callhtml('Member/SignUp.aspx', '学员报名');
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
    </script>
</head>
<body>
    <div id="mempay">
        <div class="control">
            <div class="search">
                    <input  id="nRegistBeginTime" placeholder="报名开始时间"  type="text" class="sinput commonSearchKey"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"/>
                      <input  id="nRegistEndTime" placeholder="报名结束时间"  type="text" class="sinput commonSearchKey"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"/>
                      <input  id="nName" placeholder="报名学员"  type="text" class="sinput commonSearchKey" />

                <select id="ddlTraing" class="commonSearchKey" runat="server">
                </select>
                   <select id="ddlCourse" class="commonSearchKey" runat="server">
                </select>
                   <input type="button" value="查询" class="btn btn-success" onclick="doSearch()" />
                <input type="button" value="新增报名" class="btn btn-danger" onclick="addNew()" />
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
                        学员账号
                    </th>
                     <th>
                        学员姓名
                    </th>
                      <th>
                        报名时间
                    </th>
                       <th>
                        报名课程
                    </th>
                        <th>
                        报名费用
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
              <td>{{ formatDate(d[i].SignDate,'yyyy-MM-dd HH:mm') }}</td>
              <td>{{ d[i].CourseName }}</td>
              <td>{{ d[i].Fee }}</td> 
          </tr>
        {{# } }}
         </script>
</body>
</html>
