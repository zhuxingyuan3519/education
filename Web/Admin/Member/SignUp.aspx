<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="Web.Admin.Member.SignUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>学员缴费升级</title>
    <style type="text/css">
        .divnames {
            float: left;
            margin-left: 10px;
            width: 200px;
        }

        .spnames {
            width: 80px;
        }

        input[type='checkbox'] {
            cursor: pointer;
        }

        #queryResult {
            display: none;
        }

        .inpersonlist {
            float: left;
            margin-left: 10px;
            padding-top: 10px;
        }

        .inpersonspan {
            float: right;
            margin-top: -7px;
            cursor: pointer;
            color: red;
        }

        #queryResultTd ul li {
            padding: 5px;
        }

        .operator {
            display: none;
        }
    </style>
</head>
<body>
    <div id="mempay">
        <div id="finance" class="container">
            <form id="form1" style="margin-top: 20px">
                <div class="row">
                    <div class="col col-md-6">
                        <div class="form-group">
                            <label>
                                缴费会员：
                                <input type="text" id="txtMID" class="form-inline" style="width: 50%" />&nbsp;<input type="button" onclick="setQuery(1)" value="查询" class="btn btn-sm btn-info" /></label>
                        </div>
                    </div>
                    <div class=" col-md-6">
                        <div id="divChoicePerson" style="width: 500px">
                        </div>
                    </div>

                </div>
                <div class="row">

                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_Phone">操作类型：</label>
                            <select id="ddl_operatorType" runat="server" onchange="operatorChange()">
                                <option value="">选择操作类型</option>
                                <option value="0">用户缴费升级</option>
                                <option value="1">用户购买名额</option>
                            </select>
                        </div>
                    </div>


                    <div class="col col-md-6 operator operator_0">
                        <div class="form-group">
                            <label for="txt_Phone">缴费级别：</label>
                            <select id="ddlCourse" runat="server" onchange="leavelChange()">
                            </select>
                        </div>
                    </div>

                    <div class="col col-md-6 operator operator_0  operator_1">
                        <div class="form-group">
                            <label for="txt_Phone">
                                配送数量：
                                 <input type="text" id="txtPoint" class="form-inline" runat="server" style="width: 50%" /></label>
                        </div>
                    </div>

                    <div class="col col-md-6 operator operator_1">
                        <div class="form-group">
                            <label for="txt_Phone">
                                缴费金额：
                                 <input type="text" id="txt_money" class="form-inline" runat="server" style="width: 50%" /></label>
                        </div>
                    </div>

                    <div class="col col-md-6 operator operator_1">
                        <div class="form-group">
                            <label for="txt_Phone">
                                推荐人分红比例：
                                 <input type="text" id="txt_bili" class="form-inline" runat="server" style="width: 30%" />%</label>
                        </div>
                    </div>


                    <div class="col col-md-6 operator  operator_0  operator_1">
                        <div class="form-group">
                            <label>&emsp;支付方式</label>
                            <select id="ddlPayType" runat="server">
                                <option value="WXpay">微信支付</option>
                                <option value="Cash">现金支付</option>
                                <option value="Tpay">Tpay支付</option>
                                <option value="Vpay">Vpay支付</option>
                            </select>
                        </div>
                    </div>

                    <div class="col col-md-6 operator operator_0 operator_1">
                        <div class="form-group">
                            <label for="txt_Phone">
                                推荐人为游客也分红：
                              <input type="checkbox" name="chk_memberFH" value="1" />
                            </label>
                        </div>
                    </div>


                    <div class="col col-md-6 hidden">
                        <div class="form-group">
                            <label for="txt_Phone">代理商：</label>
                            <select id="ddlTraing" runat="server">
                            </select>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col col-md-12">
                        <div id="queryResult">
                            <div id="queryResultTd">
                            </div>
                        </div>
                        <div style="max-height: 100px; overflow-y: auto">
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col col-md-12 " style="margin-top: 20px">
                        <div class="form-group">
                            &emsp;&emsp;&emsp;&emsp;    
                            <input type="button" class="btn btn-danger" value="确认提交" id="btnSubmit" onclick="return checkChange()" />&emsp;    
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
    <script type="text/javascript">

        function operatorChange() {
            var optype = $("#ddl_operatorType").val();
            if (optype == "")
                return;
            $(".operator").hide();
            $(".operator_" + optype).show();
          
        }

        function checkFo(optype) {
            if (optype == "1") {
                var value = $.trim($("#txtPoint").val());
                if (!(/(^\d+$)/.test(value)) || value < 0) {
                    layer.alert('配送数量：只能输入整数', { icon: 6 });
                    return false
                }

                value = $.trim($("#txt_money").val());
                if (!$.isNumeric(value)) {
                    layer.alert('缴费金额：只能输入正整数', { icon: 6 });
                    return false
                }

                value = $.trim($("#txt_bili").val());
                if (!(/(^\d+$)/.test(value)) || value < 0) {
                    layer.alert('推荐人分红比例：只能输入正整数', { icon: 6 });
                    return false
                }
            }
            else {
                var value = $.trim($("#txtPoint").val());
                if (!(/(^\d+$)/.test(value)) || value < 0) {
                    layer.alert('配送数量：只能输入整数', { icon: 6 });
                    return false
                }
            }
            return true;
        }

        function leavelChange() {
            var pointer = $("#ddlCourse option:selected").attr("point");
            $("#txtPoint").val(pointer);
        }

        function checkChange() {
            var optype = $("#ddl_operatorType").val();
            if (optype == "")
                return;
            if (!checkFo(optype))
                return false;
            var checkMember = $("input[type='checkbox'][name='chkMemberCode']").val();
            if (checkMember == undefined) {
                layer.alert("请选择缴费会员");
                return false;
            }

            if (confirm("即将要为【" + $('.inpersonlist').text() + "】缴费升级，是否确认？")) {
                var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
                var rek = '<%=Request.RawUrl%>';
                //获取最后一个/和?之间的内容，就是请求的页面
                rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
                $.ajax({
                    type: 'post',
                    url: 'Member/' + rek + '?Action=add&addtype=' + optype,
                    data: $('#form1').serializeArray(),
                    success: function (info) {
                        layer.close(index);
                        var data = JSON.parse(info);
                        //alert(data.isSuccess);
                        if (data.isSuccess == "false") {
                            layer.msg(data.msg);
                        }
                        else {
                            layer.msg("缴费成功！");
                            //layer.alert("报名成功！");
                            //setTimeout(function () {
                            //    resetList()
                            //}, 2000);
                        }
                    }
                });
            }
        }

        function resetList() {
            callhtml('Member/SignUpList.aspx', '报名列表');
        }
        function setQuery(ind) {
            var mid = $("#txtMID").val();
            if (ind == 2) {
                mid = $("#txtAgentMID").val();
            }
            if (ind == 3) {
                mid = $("#txtSendAgentMID").val();
            }
            if ($.trim(mid) != "") {
                $("#queryResult").show();
                var userInfo;
                if (ind == 2) {
                    userInfo = {
                        type: 'GetAgentInfoByName',
                        pram: mid,
                    };
                }
                else if (ind == 1) {
                    userInfo = {
                        type: 'GetMemberInfoByName',
                        pram: mid,
                    };
                }
                else if (ind == 3) {
                    userInfo = {
                        type: 'GetAgentInfoByName',
                        pram: mid,
                    };
                }
                var result = GetAjaxString(userInfo);
                if (result != "0") {
                    var arr = result.split('*');
                    var trs = "<ul><li>会员账号&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;会员姓名</li>";
                    $.each(arr, function (n, value) {
                        if (value != null && value != "") {
                            if (ind == 1)
                                trs += "<li><input type='radio' name='rd_mid'  value='" + value.split('≌')[0] + "' onclick=choicePerson(this,'" + value.split('≌')[1] + "',1,'" + value.split('≌')[2] + "')  />&nbsp;" + value.split('≌')[1] + "&emsp;&emsp;" + value.split('≌')[2] + "</li>";
                            else if (ind == 2)
                                trs += "<li><input type='radio'  name='rd_mid'  value='" + value.split('≌')[0] + "' onclick=choicePerson(this,'" + value.split('≌')[1] + "',2,'" + value.split('≌')[2] + "')  />&nbsp;" + value.split('≌')[1] + "&emsp;&emsp;" + value.split('≌')[2] + "</li>";
                            else if (ind == 3)
                                trs += "<li><input type='radio'  name='rd_mid'  value='" + value.split('≌')[0] + "' onclick=choicePerson(this,'" + value.split('≌')[1] + "',3,'" + value.split('≌')[2] + "')  />&nbsp;" + value.split('≌')[1] + "&emsp;&emsp;" + value.split('≌')[2] + "</li>";
                        }
                    });
                    trs += "</ul>";
                    //if (ind == 1)
                    $("#queryResultTd").html(trs);
                    //else
                    //    $("#queryAgentResultTd").html(trs);
                    layer.open({
                        type: 1,
                        shade: true,
                        title: '选择缴费会员', //显示标题
                        area: ['500px', '350px'],
                        content: $('#queryResultTd'), //捕获的元素，注意：最好该指定的元素要存放在body最外层，否则可能被其它的相对元素所影响
                        btn: ['确定'],
                        yes: function (index) {
                            var checkMember = $("input[type='checkbox'][name='chkMemberCode']").val();
                            if (checkMember == undefined) {
                                layer.alert("请选择缴费会员");
                                return false;
                            }
                            else
                                layer.close(index);
                        },
                        cancel: function (index) {
                            layer.close(index);
                        }
                    });
                }
                else {
                    $("#queryResultTd").html('');
                }
            }
        }
        function choicePerson(obj, mid, ind, mname) {
            var isChoice = $(obj).prop("checked");
            var id = $(obj).val();
            if (isChoice) { //选中了就加载到div中
                var appendhtml = "<div id='person_" + ind + "_" + id + "' class='inpersonlist'>缴费会员：账号：" + mid + "，姓名：" + mname + "<input type='checkbox' value='" + id + "' checked='checked' style='display:none' name='chkMemberCode' /><span class='inpersonspan' onclick='removeChoicedPerson(this)'>X</span> </div>";
                if (ind == 2)
                    appendhtml = "<div id='person_" + ind + "_" + id + "' class='inpersonlist'>缴费会员：账号：" + mid + "，姓名：" + mname + "<input type='checkbox' value='" + id + "' checked='checked' style='display:none' name='chkAgentCode' /><span class='inpersonspan' onclick='removeChoicedPerson(this)'>X</span> </div>";
                if (ind == 3)
                    appendhtml = "<div id='person_" + ind + "_" + id + "' class='inpersonlist'>缴费会员：账号：" + mid + "，姓名：" + mname + "<input type='checkbox' value='" + id + "' checked='checked' style='display:none' name='chkSendMemberCode' /><span class='inpersonspan' onclick='removeChoicedPerson(this)'>X</span> </div>";
                if (typeof ($("#person_" + ind + "_" + id).html()) == 'undefined' || $("#person_" + ind + "_" + id).html() == '') {
                    if (ind == 1)
                        $("#divChoicePerson").html(appendhtml);
                    else if (ind == 2)
                        $("#queryAgentResultTd").html(appendhtml);
                    else if (ind == 3)
                        $("#querySendAgentResultTd").html(appendhtml);
                }
                layer.closeAll();
            }
            else {
                $("#person_" + ind + "_" + id).remove();
            }
        }
        function removeChoicedPerson(obj) {
            $(obj).parent().remove();
        }
    </script>
</body>
</html>
