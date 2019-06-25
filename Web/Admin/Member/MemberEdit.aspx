<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberEdit.aspx.cs" Inherits="Web.Admin.Member.MemberEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        label{width: 110px;
    text-align: right;}
    </style>
    <script type="text/javascript">
        function checkChange() {
            if (!checkForm())
                return;
            if (confirm("要创建的会员的推荐人为：" + $("#txt_MTJ").val() + "，是否确认？")) {
                var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
                var rek = '<%=Request.RawUrl%>';
                //获取最后一个/和?之间的内容，就是请求的页面
                rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
                $.ajax({
                    type: 'post',
                    url: 'Member/' + rek + '?Action=add',
                    data: $('#form1').serializeArray(),
                    success: function (info) {
                        layer.close(index);
                        var data = JSON.parse(info);
                        if (data.isSuccess == "false"){
                            layer.alert(data.msg);
                        }
                        else {
                            layer.alert("注册成功！");
                            $("#hidId").val(data.msg);
                            //setTimeout(function () {
                            //    resetList()
                            //}, 3000);
                        }
                    }
                });
            }
        }
        var hidType = $("#hidType").val();
        var addMsg = "学员";
        if (hidType == "Training") {
            addMsg = "机构";
        }
        else if (hidType == "Agent") {
            addMsg = "代理商";
        }
        $("#spType").html('');
        function resetList() {
            callhtml('Member/MemberList.aspx?type='+hidType, addMsg+'列表');
        }
        function cityChange(obj, level) {
            if (level != 40)
                attrSelcet($(obj).val(), level);
            //else
            //    $("#hidZone").val($(obj).val());
        }
        function attrSelcet(va, level) {
            var addInfo = {
                //type: 'GetAddressInfo',
                type: 'GetNewAddressInfo',
                pram: va,
                level: level
            };
            var result = GetAjaxString(addInfo);
            if (result != "0") {
                var ddlcity = $("#ddl_City");
                if (level == "30")
                    ddlcity = $("#ddl_Zone");
                else {
                    $("#ddl_City").empty();
                }
                ddlcity.empty();
                ddlcity.append("<option value=''>--请选择--</option>");
                var data = eval(result);
                for (var index = 0; index < data.length; index++) {
                    var val = data[index];
                    var html = "<option value='" + val.Id + "'>" + val.Name + "</option>";
                    ddlcity.append(html);
                }
            }
        }

        function leavelChange() {
            var pointer = $("#ddlType option:selected").attr("point");
            $("#txtPoint").val(pointer);
        }

        function getRoleChecked() {
            var currentRoleCode = $("#hid_userRoleCode").val();
            var currentRoleCodeArray = currentRoleCode.split(',');
            //console.log(currentRoleCodeArray.length);
            for (var i = 0; i < currentRoleCodeArray.length; i++) {
                //console.log(currentRoleCodeArray[i]);
                if (currentRoleCodeArray[i] != '') {
                    $("input[type='checkbox'][value='" + currentRoleCodeArray[i] + "']").prop("checked", true);
                }
            }
        }
        $(function () {
            getRoleChecked();
        });


    </script>
</head>
<body>
    <div id="mempay">
        <div id="finance" class="container">
            <form id="form1" style="padding: 30px">
                <input type="hidden" runat="server" id="hidId" />
                <input type="hidden" runat="server" id="hidType" />
                <div class="row hidden">
                    <div class="col col-md-12">
                        <div class="form-group">
                            <span>我的体验端口数量：<%=TModel.LeaveTradePoints %></span>&emsp;
                  <span>我的收费端口数量：<%=TModel.TradePoints %></span>
                        </div>
                    </div>
                </div>

                <div class="row">

                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_MID">登录账号：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_MID" require-type="require" require-msg="登录账号" />
                        </div>
                    </div>


                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_Name">密&emsp;&emsp;码：</label>
                            <input runat="server" type="text" class="form-inline" id="txtPwd" />
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_Name">姓&emsp;&emsp;名：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_Name" require-type="require" require-msg="姓名" />
                        </div>
                    </div>
                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_Tel">手机号码：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_Tel" require-type="phone" require-msg="手机号码" />
                        </div>
                    </div>
                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_Phone">邮&emsp;&emsp;箱：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_Email" />
                        </div>
                    </div>
                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_WeiXin">微信号码：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_WeiXin" />
                        </div>
                    </div>

                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_QQ">&nbsp;QQ号码：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_QQ" />
                        </div>
                    </div>

                    <div class="col col-md-12">
                        <div class="form-group">
                            <label for="txt_Phone"><span id="spType"></span>级&emsp;&emsp;别：</label>
                            <input type="hidden" id="hid_userRoleCode" runat="server" />
                            <select id="ddlType" runat="server"  onchange="leavelChange()">
                            </select>
                           <%-- <asp:Repeater ID="repRoleList" runat="server">
                                <ItemTemplate>
                                    <input type="checkbox"  value="<%#Eval("Code") %>" <%#GetRoleCheckStatus(Eval("Code")) %> name="chk_roles"/><%#Eval("Name") %>
                                </ItemTemplate>
                            </asp:Repeater>--%>
                        </div>
                    </div>

                       <div class="col col-md-6">
                        <div class="form-group">
                            <label>VIP会员配送数量：</label>
                           <input type="text" id="txtPoint" class="form-inline" runat="server" />
                        </div>
                    </div>

                         <div class="col col-md-6">
                        <div class="form-group">
                            <label>支付方式：</label>
                          <select id="ddlPayType" runat="server">
                                <option value="WXpay">微信支付</option>
                              <option value="Cash">现金支付</option>
                              <option value="Tpay">Tpay支付</option>
                              <option value="Vpay">Vpay支付</option>
                            </select>
                        </div>
                    </div>

                </div>

                <div class="row">




                    <div class="col col-md-6 hidden">
                        <div class="form-group">
                            <label for="txt_WeiXin">使用时限：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_UseBeginTime" style="width: 110px" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" require-msg="使用时限" />~
                <input runat="server" type="text" class="form-inline" id="txt_UseEndTime" style="width: 110px" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" require-msg="使用时限" value="2016-12-30" />
                        </div>
                    </div>

                </div>


                <div class="row">



                    <div class="col col-md-6 hidden">
                        <div class="form-group">
                            <label for="txt_MTJ">登录状态：</label>
                            <select id="ddlStatus" runat="server">
                                <option value="1">开放登录</option>
                                <option value="0">限制登录</option>
                            </select>
                        </div>
                    </div>

                    <div class="col col-md-12">
                        <div class="form-group">
                            <label for="txt_area">地区：</label>
                            <select id="ddl_Province" onchange="cityChange(this,20)" runat="server"></select>
                            <select id="ddl_City" onchange="cityChange(this,30)" runat="server"></select>
                            <select id="ddl_Zone" onchange="cityChange(this,40)" runat="server"></select>
                            <label for="txt_Address">详细地址：</label>
                            <input runat="server" type="text" class="form-inline"  id="txt_Address" />
                        </div>
                    </div>


                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_MTJ">推荐人账号：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_MTJ" require-type="require" require-msg="推荐人代码" />
                        </div>
                    </div>

                         <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_MTJ">归属老师账号：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_TeacherMID"/>
                        </div>
                    </div>


                      <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_MTJ">归属机构代码：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_Company" require-type="require" require-msg="归属机构代码" />
                        </div>
                    </div>

                    <%if(!string.IsNullOrEmpty(Request.QueryString["id"]))
                        { %>
                    <div class="col col-md-6 hidden">
                        <div class="form-group">
                            <label for="txt_MTJ">&nbsp;是否服务中心：</label>
                            <input type="checkbox" name="chk_service" <%=isService?"checked='checked'":"" %>  value="service"/>
                            服务中心名称：<input runat="server" type="text" class="form-inline" id="txt_WelfareID" />
                        </div>
                    </div>
                    <%} %>

                     <div class="col col-md-6">
                        <div class="form-group">
                            <label>发放推荐奖：</label>
                            <select id="ddl_tjStatus" runat="server">
                                <option value="0">限制</option>
                                <option value="1">开放</option>
                            </select>
                        </div>
                    </div>

                     <div class="col col-md-6">
                        <div class="form-group">
                            <label>游客身份提现：</label>
                            <select id="ddl_ReadNoticeId" runat="server">
                                <option value="0">限制</option>
                                <option value="1">开放</option>
                            </select>
                        </div>
                    </div>


                        <div class="col col-md-6">
                        <div class="form-group">
                            <label>记忆训练项目：</label>
                            <input type="checkbox" value="1" name="chkTrainCode" <%=GetTrainCheckStatus("1") %> />初级混合词&nbsp;
                          <input type="checkbox" value="5" name="chkTrainCode" <%=GetTrainCheckStatus("5") %> />高级混合词&nbsp;
                            <input type="checkbox" value="2" name="chkTrainCode" <%=GetTrainCheckStatus("2") %>/>数字&nbsp;
                            <input type="checkbox" value="3" name="chkTrainCode" <%=GetTrainCheckStatus("3") %>/>扑克牌&nbsp;
                            <input type="checkbox" value="4" name="chkTrainCode" <%=GetTrainCheckStatus("4") %>/>字母
                        </div>
                    </div>


                </div>

                <div class="row">
                    <div class="col col-md-12 text-center">
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="提交" id="btnSubmit" onclick="checkChange()" />&emsp;
                <input type="button" class="btn btn-info" value="返回" onclick="resetList()" />
                        </div>
                    </div>
                </div>

            </form>
        </div>
    </div>
</body>
</html>
