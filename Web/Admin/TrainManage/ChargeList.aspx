<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChargeList.aspx.cs" Inherits="Web.Admin.TrainManage.ChargeList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .width50, .titiletxt {
            width: 20%;
        }

        .titiletxt, subtitiletxt {
            display: inline;
        }

        .imgop {
            cursor: pointer;
            width: 20px;
        }

        .subtitletxt {
            width: 20%;
            display: inline;
        }

        .errfail {
            color: Red;
        }

        .errsuccess {
            color: #86C440;
        }

        .subui {
            padding-left: 20px;
        }

        .width60 {
            width: 100%;
        }

        .delone {
            width: 20px;
        }

        .imgadd {
            float: right;
        }

        .txturl, txtname {
            width: 15%;
        }

        .txturl {
            display: inline;
        }

        .txticon {
            width: 100px;
            display: inline;
        }

        .txtindex {
            width: 70px;
            display: inline;
        }
    </style>
    <script type="text/javascript">
        function deleteFirstCategory(obj) {
            if (confirm("确定要删除？")) {
                $(obj).parent().remove();
            }
        }

        function addCategory(obj) {
            var html = "<li class='list-group-item Train-Item'>";
            html += "收费标准:<input type='text' value='' class='form-control subtitletxt txtid'  require-type=\"decimal\" require-msg=\"收费标准\" />&nbsp;";
            html += "<input type=\"button\" class=\"btn btn-xs btn-success\" value=\"选择训练项目\" onclick=\"showTrainList(this)\" />&nbsp;&nbsp;";
            html += " <span class=\"sp_trainItem\"></span>";
            html += "<img class='imgop delone' src='../Admin/img/Delete.png' title='删除'  style=\"float:right\" onclick='deleteFirstCategory(this)' /></li>";
            $(obj).parent().find(".width60").append(html);
        }

        $(document).ready(function () {
      
        });

    </script>
</head>
<body>
    <form id="form1">
        <div id="mempay">
            <div class="control">
            </div>
            <div class="ui_table">
                <ul class="list-group width60">
                    <asp:Repeater ID="rep_list" runat="server">
                        <ItemTemplate>
                            <li class="list-group-item Train-Item">收费标准:<input type="text" class="form-control titiletxt txtid" require-type="decimal" require-msg="收费标准"  value="<%#Eval("ChargeMoney") %>"/>&nbsp;
                   <input type="button" class="btn btn-xs btn-success" value="选择训练项目" onclick="showTrainList(this)" />&nbsp;&nbsp;
                <span class="sp_trainItem"><%#Eval("Remark") %></span>
                                <img class="imgop delone" src="../Admin/img/Delete.png" title="删除" style="float: right" onclick="deleteFirstCategory(this)" />
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <img class="imgop" src="../Admin/img/plus.png" onclick="addCategory(this)" />
            </div>
            <br />
            <br />
            <br />
            <input type="button" class="btn btn-success" value="保存" onclick="saveChange()" />
        </div>

        <ul class="layer_notice layui-layer-wrap" id="layLoginUI" style="display: none;">
            <li class="tdlable" style="margin-left: 20px">
                <input type="checkbox" value="1" onclick="checkTrainItem(this, 1, '混合词训练')" />
                初级混合词训练
            </li>
            <li class="tdlable" style="margin-left: 20px">
                <input type="checkbox" value="1" onclick="checkTrainItem(this, 5, '混合词训练')" />
                高级混合词训练
            </li>
            <li class="tdlable" style="margin-left: 20px">
                <input type="checkbox" value="2" onclick="checkTrainItem(this, 2, '数字训练')" />
                数字训练
            </li>
            <li class="tdlable" style="margin-left: 20px">
                <input type="checkbox" value="3" onclick="checkTrainItem(this, 3, '扑克牌训练')" />
                扑克牌训练
            </li>
            <li class="tdlable" style="margin-left: 20px">
                <input type="checkbox" value="4" onclick="checkTrainItem(this, 4, '字母训练')" />
                字母训练
            </li>
        </ul>
    </form>
    <script type="text/javascript">
        var checkItem = [];
        function checkIdExist(trainCode) {
            var isExist = false;
            $.each(checkItem, function (n, value) {
                if (value.trainCode == trainCode) {
                    isExist = true;
                    return false;
                }
            });
            return isExist;
        }

        function deleteObj(a) {
            $.each(checkItem, function (n, value) {
                if (value != null && typeof (value) != undefined) {
                    if (value.trainCode == a) {
                        checkItem.splice(n, 1);
                    }
                }
            })
        }

        function checkTrainItem(obj, trainCode, trainName) {
            if ($(obj).prop("checked")) {
                var item = { "trainCode": trainCode, "trainName": trainName };
                //查看是否存在
                if (!checkIdExist(trainCode))
                    checkItem.push(item);
            }
            else {
                //删除项目
                deleteObj(trainCode);
            }
        }

        function showTrainList(obj) {
            checkItem = [];
            $("#layLoginUI").find("input[type='checkbox']").prop("checked", false);
            layer.open({
                type: 1,
                shade: [0.8, '#393D49'],
                title: ['选择训练项目', 'font-size:18px;background:#5bc0de'],
                content: $('#layLoginUI'), //捕获的元素
                btn: ['确定', '取消'],
                area: ['350px', '195px'],
                yes: function (index) {
                    var appendHtml = "";
                    //console.log(JSON.stringify(checkItem));
                    $.each(checkItem, function (n, value) {
                        if (value != null && typeof (value) != undefined) {
                            appendHtml += "<input type='hidden' value='" + value.trainCode + "'/>" + value.trainName + "&emsp;";
                        }
                    });
                    $(obj).next().html(appendHtml);
                    layer.close(index);
                },
                cancel: function (index) {
                    layer.close(index);
                }
            });
        }

        function saveChange() {
            if (!checkForm())
                return;

            var saveArray = [];
            var sort = 0;
            $(".Train-Item").each(function () {
                var money = $(this).find(".txtid").val();
                sort++;
                var checkVal = '';
                $(this).find("input[type='hidden']").each(function () {
                    checkVal += $(this).val() + "|";
                });

                saveArray.push({ "ChargeMoney": money, "ChargeList": checkVal, "Sort": sort });
            });

            var userInfo = {
                type: 'SaveChargeList',
                pram: JSON.stringify(saveArray)
            };

            var result = GetAjaxString(userInfo);
            if (result == "1") {
                layer.alert("保存成功");
            }
            else {
                layer.alert("保存失败，请重试");
            }
        }

    </script>
</body>
</html>
