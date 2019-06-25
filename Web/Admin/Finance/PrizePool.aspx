<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrizePool.aspx.cs" Inherits="Web.Admin.Finance.PrizePool" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>月分红</title>
    <style type="text/css">
        #poolTd tbody > tr > td {
            padding: 5px !important;
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
    <script type="text/javascript">
        $(function () {
        });

        function sendChange() {
            if (!confirm("将发送红包总金额至红包领取系统，是否继续发送！"))
                return;
            var value = $("#txt_allPrizeMoney").val();
            if (!$.isNumeric(value)) {
                layer.msg( '红包总金额：只能输入数字', { icon: 6 });
                return false;
            }
            var dateval = $("#txt_lingquDate").val();
            if (dateval == "") {
                layer.msg('红包领取日期：不能为空', { icon: 6 });
                return false;
            }
            


            var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            var rek = '<%=Request.RawUrl%>';
              rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
              $.ajax({
                  type: 'post',
                  url: 'Finance/' + rek + '?Action=MODIFY',
                  data: $('#form1').serializeArray(),
                  success: function (info) {
                      layer.close(index);
                      if (info == "0")
                          layer.alert("提交失败，请重试！");
                      else if (info == "1") {  //提交成功
                          layer.alert("操作成功！");
                      }
                      else
                          layer.alert(info);
                  }
              });
          }

          function checkChange() {
              if (!confirm("本月分红可能已经自动执行过了，确定要重复手动执行吗？执行后，奖金可能会重复增加，请谨慎执行！"))
                  return;
              var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
              var rek = '<%=Request.RawUrl%>';
            rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
            $.ajax({
                type: 'post',
                url: 'Finance/' + rek + '?Action=add',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    layer.close(index);
                    if (info == "0")
                        layer.alert("提交失败，请重试！");
                    else if (info == "1") {  //提交成功
                        layer.alert("操作成功！");
                    }
                    else
                        layer.alert(info);
                }
            });
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div id="finance" class="container">
            <form id="form1" style="padding: 30px">
                <div class="row">
                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_MID">&emsp;&emsp;&emsp;分红金额：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_fmMoney" require-type="decimal" require-msg="分红金额" />
                        </div>
                    </div>

                    <div class="col col-md-6 hidden">
                        <div class="form-group">
                            <label for="txt_Name">上月城市合伙人数量：</label>
                            <input runat="server" type="text" class="form-inline" id="txt_count" require-type="int" require-msg="上月城市合伙人数量" />
                        </div>
                    </div>

                    <div class="col col-md-6">
                        <div class="form-group">
                            <label for="txt_Name">历史分红会员：</label>
                            <select id="ddl_year">
                                <option value="2018">2018</option>
                                <option value="2019">2019</option>
                                <option value="2020">2020</option>
                                <option value="2021">2021</option>
                                <option value="2022">2022</option>
                            </select>年
                                <select id="ddl_month">
                                    <option value="01">01</option>
                                    <option value="02">02</option>
                                    <option value="03">03</option>
                                    <option value="04">04</option>
                                    <option value="05">05</option>
                                    <option value="06">06</option>
                                    <option value="07">07</option>
                                    <option value="08">08</option>
                                    <option value="09">09</option>
                                    <option value="10">10</option>
                                    <option value="11">11</option>
                                    <option value="12">12</option>
                                </select>月
                             &nbsp;<input type="button" onclick="setCheck()" value="查询" class="btn btn-sm btn-info" />

                        </div>
                    </div>

                    <div class="col col-md-6">
                        <div class="form-group">
                            <label>
                                选择城市合伙人：</label>
                            <input type="text" id="txtMID" class="form-inline" />&nbsp;<input type="button" onclick="setQuery()" value="查询" class="btn btn-sm btn-info" />
                        </div>
                    </div>
                    <div class="col col-md-6">
                        <div id="divChoicePerson" style="width: 500px">
                        </div>
                    </div>


                    <div class="col col-md-12">
                        <div class="form-group">
                            <input type="button" class="btn btn-danger" value="确认分红" id="btnSubmit" onclick="return checkChange()" />
                        </div>
                    </div>

                </div>


                <div class="row">
                    <div class="col col-md-12">
                        红包总金额：
                                   <input runat="server" type="text" class="form-inline" id="txt_allPrizeMoney" />元
                        红包领取日期：
                                   <input runat="server" type="text" class="form-inline" id="txt_lingquDate" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
                        <input type="button" class="btn btn-danger" value="发送到红包池" onclick="return sendChange()" />

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
            </form>
        </div>
    </div>
    <script type="text/javascript">

        function setCheck() {
            var userInfo = {
                type: 'GetMonthFHMemberInfo',
                year: $("#ddl_year").val(),
                month: $("#ddl_month").val()
            };
            var result = GetAjaxString(userInfo);
            if (result != "0") {
                //var data = JSON.parse(result);
                //console.log(JSON.stringify(data));

                var jsonArray = JSON.parse(result);
                $.each(jsonArray.Rows, function (index, val) {
                    var appendhtml = "<div id='person_" + 1 + "_" + val.UserId + "' class='inpersonlist'>账号：" + val.MID + "，姓名：" + val.MName + "<input type='checkbox' value='" + val.UserId + "' checked='checked' style='display:none' name='chkMemberCode' /><span class='inpersonspan' onclick='removeChoicedPerson(this)'>X</span> </div>";
                    if (typeof ($("#person_" + 1 + "_" + val.UserId).html()) == 'undefined' || $("#person_" + 1 + "_" + val.UserId).html() == '') {
                        $("#divChoicePerson").append(appendhtml);
                    }
                });

                //for (var index = 0; index < data.Row.length; index++) {
                //    var val = data[index];
                //    var appendhtml = "<div id='person_" + 1 + "_" + val.UserId + "' class='inpersonlist'>账号：" + val.MID + "，姓名：" + val.MName + "<input type='checkbox' value='" + id + "' checked='checked' style='display:none' name='chkMemberCode' /><span class='inpersonspan' onclick='removeChoicedPerson(this)'>X</span> </div>";
                //    if (typeof ($("#person_" + 1 + "_" + val.UserId).html()) == 'undefined' || $("#person_" + 1 + "_" + val.UserId).html() == '') {
                //        $("#divChoicePerson").append(appendhtml);
                //    }
                //}
            }
        }

        function setQuery() {
            var mid = $("#txtMID").val();
            if ($.trim(mid) != "") {
                $("#queryResult").show();
                var userInfo = {
                    type: 'GetNewMemberInfoByName',
                    pram: mid,
                };
                var result = GetAjaxString(userInfo);
                var trs = "<table class='table table-border'><tr><td>选择</td><td>会员账号</td><td>会员姓名</td><td>会员级别</td></tr>";
                if (result != "0") {
                    var data = eval(result);
                    for (var index = 0; index < data.length; index++) {
                        var val = data[index];
                        trs += "<tr><td><input type='radio' name='rd_mid'  value='" + val.Id + "' onclick=choicePerson(this,'" + val.MID + "',1,'" + val.Name + "')  /></td><td>" + val.MID + "</td><td>" + val.Name + "</td><td>" + val.RoleName + "</td></tr>";
                    }
                }
                trs += "</table>";

                $("#queryResultTd").html(trs);
                layer.open({
                    type: 1,
                    shade: true,
                    title: '选择分红城市合伙人', //显示标题
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


        function choicePerson(obj, mid, ind, mname) {
            var isChoice = $(obj).prop("checked");
            var id = $(obj).val();
            if (isChoice) { //选中了就加载到div中
                var appendhtml = "<div id='person_" + ind + "_" + id + "' class='inpersonlist'>账号：" + mid + "，姓名：" + mname + "<input type='checkbox' value='" + id + "' checked='checked' style='display:none' name='chkMemberCode' /><span class='inpersonspan' onclick='removeChoicedPerson(this)'>X</span> </div>";
                if (typeof ($("#person_" + ind + "_" + id).html()) == 'undefined' || $("#person_" + ind + "_" + id).html() == '') {
                    $("#divChoicePerson").append(appendhtml);
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
