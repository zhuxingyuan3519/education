<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="role_detail.aspx.cs" Inherits="Web.m.app.role_detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .marg {
            text-align: center !important;
        }

        img {
            width: 100% !important;
        }
    </style>
    <script type="text/javascript">
        var layerindex;
        function apply() {
            if (roleCode == "1F" || roleCode == "2F" || roleCode == "3F") {
                layerAlert("您已申请过，请勿重复申请");
                return false;
            }
            $("#hid_payType").val("");
            layeropenshow();
        }
        function layeropenshow() {
            layerindex = layer.open({
                type: 1
              , content: $("#layerShowHtml").html()
              , anim: 'up'
              , style: 'margin: 0 auto;top:20%; width:85%; border-radius:15px;outline:0; border: none; -webkit-animation-duration: .5s; animation-duration: .5s;'
            });
        }

        function signUp() {
            var applycode = '<%=course.Code%>';
            var payType = $("#hid_payType").val();
            var payInfoString = "";
            //alert(payType);

            if (payType == "")
            {
                layerAlert("请选择付款方式");
                return;
            }
            if (applycode == "3F") {
                if ($("#hid_payCity").val() == "") {
                    layerAlert("请选择申请城市");
                    return;
                }
            }

            if (payType == "TPay") {
                payInfoString = '手机号后四位：' + '<%=Service.GlobleConfigService.GetWebConfig("TPayName").Value%>';
                payInfoString += '<br/>TPay账号（ID）：' + '<%=Service.GlobleConfigService.GetWebConfig("TPayID").Value%>';
                payInfoString += '<br/>财务电话：' + '<%=Service.GlobleConfigService.GetWebConfig("TPayTel").Value%>';
                $("#div_payInfo").html(payInfoString);
                showPayInfo();

            }
            else if (payType == "VPay") {
                payInfoString = 'VPay账户名称：' + '<%=Service.GlobleConfigService.GetWebConfig("VPayName").Value%>';
                payInfoString += '<br/>VPay账号：' + '<%=Service.GlobleConfigService.GetWebConfig("VPayID").Value%>';
                payInfoString += '<br/>财务电话：' + '<%=Service.GlobleConfigService.GetWebConfig("VPayTel").Value%>';
                $("#div_payInfo").html(payInfoString);
                showPayInfo();
            }
            else if (payType == "WXPay") {
                //微信支付

            }

    }

    function showPayInfo() {
        layer.open({
            type: 1
         , content: $("#layerTPayShowHtml").html()
         , anim: 'up'
         , style: 'margin: 0 auto;top:20%; width:85%; border-radius:15px;outline:0; border: none; -webkit-animation-duration: .5s; animation-duration: .5s;'
        });

    }
    function payChange(obj) {
        $("#hid_payType").val($(obj).val());
    }
    function cityCheckChange(obj) {
        $("#hid_payCity").val($(obj).val());
    }

    function cityChange(obj, level) {
        if (level != 40)
            attrSelcet($(obj).val(), level);
        $("#hid_payCity").val($(obj).val());
        $("#hid_CityLeavel").val(level);
        //else
        //    $("#hidZone").val($(obj).val());
    }
    function attrSelcet(va, level) {
        var addInfo = {
            //type: 'GetAddressInfo',
            type: 'GetNewAddressInfo',
            code: va,
            condition: 'agent',
            rolecode: '<%=course.Code%>',
            level: level
        };
        var result = GetAjaxString(addInfo);
        if (result != "0") {
            var ddlcity = $(".layui-m-layercont #ddl_city");
            if (level == "30")
                ddlcity = $(".layui-m-layercont #ddl_zone");// $("#ddl_zone");
            else {
                $("#ddl_city").empty();
            }
            ddlcity.empty();
            //ddlcity.append("<option value=''>--请选择代理城市--</option>");
            //console.log(result);
            var data = eval(result);
            //console.log(typeof (ddlcity));
            var html = "<option value=''>--请选择代理城市--</option>";
            if (level == "30")
                html = "<option value=''>--请选择区县--</option>";
            for (var index = 0; index < data.length; index++) {
                var val = data[index];
                html += "<option value='" + val.Id + "'>" + val.Name + "</option>";
                //ddlcity.append(html);
            }
            ddlcity.html(html);
            //alert($("#div_city").html());
            //$(".layui-m-layercont #div_city").html("<select id='ddl_city' class='form-control'>" + html + "</select>");
            //layer.close(layerindex);
            //layeropenshow();
        }
    }
    function checkApply() {
        var applyCode = '<%=course.Code%>';
        var cityLeavel=$("#hid_CityLeavel").val();
        if (applyCode == "3F") {
            if (cityLeavel != "30") {
                layerAlert("请选择城市");
                return false;
            }
        }
        if (applyCode == "2F") {
            if (cityLeavel != "40") {
                layerAlert("请选择机构所在区县");
                return false;
            }
        }

        var addInfo = {
            type: 'ApplyRoles',
            code: '<%=course.Code%>',
            payType: $("#hid_payType").val(),
            applyCity: $("#hid_payCity").val()
        };
        var result = GetAjaxString(addInfo);
        if (result == "1") {

            layer.open({
                content: '您已申请过，请耐心等待审核结果'
              , btn: ['确定']
              , yes: function (index) {
                  layer.closeAll();
              }
            });
        }
        else if (result == "2") {
            layer.open({
                content: '申请成功，请耐心等待审核结果'
          , btn: ['确定']
          , yes: function (index) {
              layer.closeAll();
          }
            });
        }
        else {
            layerAlert("申请失败，请重试");
        }
    }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row ">
        <div class="col-sm-12 col-xs-12 padding-left-right-0 magin10">
            <img src="../images/<%=img_name %>.jpg" class="img-responsive" />
        </div>
    </div>
    <input type="hidden" id="hid_payType" />
    <input type="hidden" id="hid_payCity" />
    <input type="hidden" id="hid_CityLeavel" />
    <div class="row bg-wh">
        <h5><b>|</b>申请/加盟<%=course.Name %></h5>
    </div>
    <div class="row" style="margin-bottom: 40px">
        <div class="col-sm-12 col-xs-12  padding-left-right-0">
            <%=course.Introduce %>
        </div>
    </div>
    <div class="navbar navbar-fixed-bottom header text-center nav-head-hewhitewhitewhite" style="background-color: #00B5FF; border-radius: 8px;" onclick="apply()">
        <h5 id="h5_title" style="color: white; margin-top: 10px; margin-bottom: 10px;">立即申请
        </h5>
    </div>


    <div id="layerShowHtml" style="display: none">
        <div>
            <div style="font-size: 18px; font-family: serif; font-weight: bolder; padding: 20px 0px 10px 20px; text-align: center; border-bottom: 1px solid #ccc;">
                请您选择付款方式
            </div>
            <div style="padding: 30px 0px 30px 0px; text-align: center; font-weight: bolder">
                <select id="ddl_applyPayType" class="form-control" style="width: 80%; margin: 0 auto;" onchange="payChange(this)">
                    <option value="">--请选择付款方式--</option>
                    <option value="WXPay">线下付款</option>
                  <%--  <option value="TPay">TPay</option>--%>
                    <%--<option value="VPay" style="display: none">VPay</option>--%>
                </select>
            </div>
            <%--申请城市合伙人的时候，需要选择城市，直辖市不能申请--%>
            <div style="text-align: center; font-weight: bolder;" id="div_province" runat="server">
                <select id="ddl_province" class="form-control" runat="server" style="width: 80%; margin: 0 auto;" onchange="cityChange(this,20)">
                </select>
            </div>

            <div  style="padding: 30px 0px 30px 0px; text-align: center; font-weight: bolder; " id="div_city" runat="server">
                <select id="ddl_city" class="form-control" runat="server" style="width: 80%; margin: 0 auto;" onchange="cityChange(this,30)">
                    <option value="">--请选择城市--</option>
                </select>
            </div>

              <div  style=" text-align: center; font-weight: bolder; " id="div_zone" runat="server">
                <select id="ddl_zone" class="form-control" runat="server" style="width: 80%; margin: 0 auto;" onchange="cityChange(this,40)">
                    <option value="">--请选择区县--</option>
                </select>
            </div>


            <div style="text-align: center; font-size: 18px; padding: 15px; border-radius: 15px; outline: 0;">
                <input type="button" value="确定" class="btn btn-block btn-info" style="width: 90%" onclick="signUp()" />
            </div>
        </div>
    </div>


    <div id="layerTPayShowHtml" style="display: none">
        <div>
            <div style="font-size: 18px; font-family: serif; font-weight: bolder; padding: 20px 0px 10px 20px; text-align: center; border-bottom: 1px solid #ccc;">
                收款账号详情
            </div>
            <div style="padding: 30px 0px 30px 0px; text-align: center; font-weight: bolder" id="div_payInfo">
            </div>
            <div style="text-align: center; font-size: 18px; padding: 15px; border-radius: 15px; outline: 0;">
                <input type="button" value="提交申请" class="btn btn-block btn-info" style="width: 90%" onclick="checkApply()" />
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
