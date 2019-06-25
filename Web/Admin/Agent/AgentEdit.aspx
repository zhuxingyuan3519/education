<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgentEdit.aspx.cs" Inherits="Web.Admin.Agent.AgentEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        label{width:110px;text-align:right;}
    </style>
    <script type="text/javascript">
        function checkChange() {
            if (!checkForm())
                return;
            //if (confirm("要创建的分销商的推荐人为：" + $("#txt_MTJ").val() + "，是否确认？")) {
            var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            var rek = '<%=Request.RawUrl%>';
                rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
                $.ajax({
                    type: 'post',
                    url: 'Agent/' + rek + '?Action=add',
                    data: $('#form1').serializeArray(),
                    success: function (info) {
                        layer.close(index);
                        var data = JSON.parse(info);
                        if (data.isSuccess == "false") {
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
            //}
            }
            function resetList() {
                callhtml('Agent/AgentList', '培训机构列表');
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
            function cityChange(obj, level) {
                if (level != 40)
                    attrSelcet($(obj).val(), level);
                //else
                //    $("#hidZone").val($(obj).val());
            }
            function isCheckUseType(obj) {
                if ($(obj).val() == "Admin") {
                    $("#lbUseType").show();
                }
                else {
                    $("#lbUseType").hide();
                }

                if ($(obj).val() == "2F" || $(obj).val() == "1F") {
                    $("#divHirePhurse").show();
                }
                else {
                    $("#divHirePhurse").hide();
                }

            }
    </script>
</head>
<body>
    <div id="mempay">
        <div id="finance" class="container">
            <form id="form1" style="padding: 30px">
                <input type="hidden" runat="server" id="hidId" />
                <div class="row">
                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_Name">负责人姓名：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_Name" require-type="require" require-msg="姓名" />
                        </div>
                    </div>
                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_QQ">负责人电话：</label>
                            <input runat="server" type="text" class="form-inline" id="txtPhone" require-type="require" require-msg="手机号码" />
                        </div>
                    </div>
                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_MID">机构账号：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_MID" require-type="require" require-msg="机构账号" />
                        </div>
                    </div>
                <div class="col col-md-6">
                    <div class="form-group">
                        <label for="txt_Name">登录密码：</label>
                        <input runat="server" type="text" class="form-inline" id="txtPwd"  require-type="require" require-msg="登录密码"/>
                    </div>
                </div>
                <div class="col col-md-6">
                    <div class="form-group">
                        <label for="txt_MID">确认密码：</label>
                        <input runat="server" type="text" class="form-inline" id="txtPwd2"  require-type="require" require-msg="确认密码"/>
                    </div>
                </div>


                <div class="col col-md-6">
                    <div class="form-group">
                        <label for="txt_Phone">机构代码：</label>
                        <input runat="server" type="text" class="form-inline" id="txt_Branch"  require-type="require" require-msg="机构代码"/>
                    </div>
                </div>

                <div class="col col-md-6">
                    <div class="form-group">
                        <label for="txt_WeiXin">&emsp;&emsp;微信号码：</label>
                        <input runat="server" type="text" class="form-inline" id="txt_WeiXin" />
                    </div>
                </div>
                <div class="col col-md-6">
                    <div class="form-group">
                        <label for="txt_QQ">&nbsp;QQ号码：</label>
                        <input runat="server" type="text" class="form-inline" id="txt_QQ" />
                    </div>
                </div>
                <div class="col col-md-6">
                    <div class="form-group">
                        <label for="txt_WeiXin">缴费金额：</label>
                        <input runat="server" type="text" class="form-inline" id="txt_PointMoney" value="0" />
                    </div>
                </div>
                <div class="col col-md-6">
                    <div class="form-group">
                        <label for="txt_QQ">剩余学员账号数量：</label>
                        <input runat="server" type="text" class="form-inline" id="txt_LeaveTradePoints" require-type="int" require-msg="剩余学员账号数量" />
                    </div>
                </div>

                      <div class="col col-md-6">
                    <div class="form-group">
                        <label for="txt_WeiXin">用户级别：</label>
                           <select class="frm-field required" id="ddlRoles" runat="server" require-type="require" require-msg="级别">
                          </select>
                    </div>
                </div>


                          <div class="col col-md-6">
                    <div class="form-group">
                        <label for="txt_WeiXin">推荐人代码：</label>
                        <input runat="server" type="text" class="form-inline" id="txt_MTJ" require-type="require" require-msg="推荐人代码" />
                    </div>
                </div>
             
        </div>

        <div class="row">
            <div class="col col-md-12">
                <div class="form-group">
                    <label for="txt_MTJ">代理地区：</label>
                    <select class="frm-field required" id="ddl_Province" runat="server" onchange="cityChange(this,20)">
                    </select>

                    <select class="frm-field required" id="ddl_City" runat="server" onchange="cityChange(this,30)">
                    </select>

                    <select class="frm-field required" id="ddl_Zone" runat="server" onchange="cityChange(this,40)">
                    </select>
                  <input runat="server" type="text" class="form-inline" id="txt_address"  placeholder="具体地址" />
                               
                </div>
            </div>
        </div>



        <div class="row">
            <div class="col col-md-12 text-center">
                <div class="form-group">
                    <input type="button" class="btn btn-primary" value="提交" id="btnSubmit" onclick="checkChange()" />&emsp;&emsp;
                <input type="button" class="btn btn-info" value="返回" onclick="resetList()" />
                </div>
            </div>
        </div>

        </form>
    </div>
    </div>
</body>
</html>
