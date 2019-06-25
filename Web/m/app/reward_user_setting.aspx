<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="reward_user_setting.aspx.cs" Inherits="Web.m.app.reward_user_setting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .deletimg {
            width: 80%;
        }
    </style>
    <script type="text/javascript">
        $(function () {

        });
        function guidGenerator() {
            var S4 = function () {
                return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
            };
            return (S4() + S4());
        }
        function addNew(obj) {
            var appendHtml = $("#div_templete").html();
            var t = guidGenerator();

            var appendHtml = "<div id='append" + t + "' class=\"row\" style=\"margin-top:10px\">" + appendHtml + "</div>";;
            $("#div_append").append(appendHtml);
            $("#append" + t).find(".inputCode").attr("name", "Code_" + t).attr("require-type", "require").attr("require-msg", "人员账号");
            $("#append" + t).find(".inputRate").attr("name", "Rate_" + t).attr("require-type", "decimal").attr("require-msg", "分配比例");
        }

        function removeRow(obj) {
            $(obj).parent().parent().remove();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row bg-wh">
        <h5><b>|</b><%=typeName %> </h5>
        <input type="hidden" id="hid_type" runat="server" />
    </div>

    <div class="row ">
        <div class="col-sm-7 col-xs-7 text-center ">
         人员账号
        </div>
        <div class="col-sm-5 col-xs-5 text-center padding-left-right-0">
           分配比例(%)
        </div>
    </div>

    <div id="div_templete" class="row hidden">
        <div class="col-sm-7 col-xs-7 text-center ">
            <input type="text" class="inputCode form-control" placeholder="人员账号" />
        </div>
        <div class="col-sm-3 col-xs-3 text-center  padding-left-right-0">
            <input type="text" class="inputRate form-control" placeholder="分配比例" />
        </div>
        <div class="col-sm-2 col-xs-2 text-center ">
            <img src="../../img/delete.png" class="deletimg" onclick="removeRow(this)" />
        </div>
    </div>

    <div id="div_append">
        <asp:Repeater ID="rep_list" runat="server">
            <ItemTemplate>
                <div class="row" style="margin-top: 10px">
                    <div class="col-sm-7 col-xs-7 text-center ">
                        <input type="text" class="inputCode form-control" name="Code_<%# Container.ItemIndex + 1%>" placeholder="人员账号" require-type="require" require-msg="人员账号"  value="<%#Eval("UserCode")%>" />
                    </div>
                    <div class="col-sm-3 col-xs-3 text-center  padding-left-right-0">
                        <input type="text" class="inputRate form-control" name="Rate_<%# Container.ItemIndex + 1%>" placeholder="分配比例"  require-type="decimal" require-msg="分配比例" value="<%#Eval("HireMoney")%>" />
                    </div>
                    <div class="col-sm-2 col-xs-2 text-center ">
                        <img src="../../img/delete.png" class="deletimg" onclick="removeRow(this)" />
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="row " style="margin-left: 10px; margin-right: 10px; padding-bottom: 20px; padding-top: 30px;">
        <span class="col-sm-12 col-xs-12  bg-wh" onclick="addNew(this)" style="padding-top: 7px; padding-bottom: 7px; font-size: 10px; text-align: center; border: 1px solid #ccc; border-radius: 20px; color: #1c8cec">+添加新账号</span>
    </div>





    <div class="marg">
        <span class="col-sm-12 col-xs-12 marg">
            <a class="btn btn-block gree" href="javascript:void(0)" onclick="setupChange()">提交</a>
        </span>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
        function setupChange() {
            if (!checkForm())
                return false;

            var totalrate = 0;
            $(".inputRate").each(function (index, item) {
                if (index > 0) {
                    var thisval = parseFloat($(this).val());
                    console.log(thisval);
                    totalrate += thisval;
                }
            });
            console.log(totalrate);
            if (totalrate > 100) {
                layerAlert("股东和老师分红比例之和不能大于100");
                return false;
            }
            //查看两个比例加起来是不是大于100
            //var gudong = parseFloat($("#txt_gudong").val());
            //var laoshi = parseFloat($("#txt_laoshi").val());
            //if (gudong + laoshi > 100) {
            //    layerAlert("股东和老师分红比例之和不能大于100");
            //    return false;
            //}

            layerLoading();
            var rek = 'reward_user_setting';
            //获取最后一个/和?之间的内容，就是请求的页面
            $.ajax({
                type: 'post',
                url: rek + '?Action=ADD',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    closeLayerLoading();
                    if (info == "0")
                        layerAlert("提交失败");
                    else if (info == "1") {  //提交成功
                        layerMsg("提交成功");
                    }
                    else if (info == "-3") {  //提交成功
                        layerMsg("股东和老师分红比例之和不能大于100");
                    }
                    else
                        layerAlert(info);
                }
            });
        }

    </script>
</asp:Content>
