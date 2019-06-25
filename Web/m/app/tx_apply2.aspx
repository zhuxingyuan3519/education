<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="tx_apply2.aspx.cs" Inherits="Web.m.app.tx_apply2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--新的一种提现方式，只填写银行账号--%> 
    <style type="text/css">
        .list-card input {
            height: initial !important;
        }
          .appendImg {
            width: 180px;
        }

        #divImgContainer div {
            margin-top: 20px;
        }

        .imgdel {
            float: left;
            cursor: pointer;
        }

        .uploadifive-button {
            float: left;
            margin-right: 10px;
        }

        #queue {
            border: 1px solid #E5E5E5;
            overflow: auto;
            margin-bottom: 10px;
            padding: 0 3px 3px;
        }

        .filename, .fileinfo {
            color: black;
            font-size: 12px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            if (systemId == "kgj01") {
                if (roleCode != "VIP")
                    layerAlert("对不起，您不是VIP用户，VIP用户才能提现，请先升级");
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row bg-wh">
        <h5><b>|</b>申请提现  <a href="tx_list.aspx" class="btn btn-success btn-sm" style="padding: 3px 8px;float:right;margin-right:5px">进度查询</a></h5>
    </div>
    <div class="row marg list-border itemlist">
        <div class="col-sm-12 col-xs-12">
            现金总额：<%=TotalMoney %>&nbsp;&nbsp;可提现总额：<span id="spCanApplyMoney"><%=GetCanTXMoney %></span>&nbsp;&nbsp;
             手续费：<%=GetTXFloat %><br />
             <%=Service.CacheService.GlobleConfig.MinTXMoney %>元以上可以提现
        </div>
    </div>
    <div class="row list-card">

          <div class="marg">
            <div class="col-sm-12 col-xs-12 marg">
                申请金额：
                  </div>
            </div>

        <div class="marg">
            <div class="col-sm-12 col-xs-12 marg">
                <input type="text" class="form-control" style="width:45%;float:left" id="txtMoney" require-msg="申请金额" require-type="decimal" placeholder=" <%=Service.CacheService.GlobleConfig.MinTXMoney %>元以上可以提现" />
                     <input type="button" value="添加提现信息" class="form-control   btn-info"   data-toggle="modal" data-target="#myModal"  style="margin-left: 5px;width:50%;float:left"  />
               </div>
            </div>

          <div class="marg" id="divBankInfo" style="display:none">
            <div class="col-sm-12 col-xs-12 margs" >
                <input type="hidden" id="hidSelBank" />
                <input type="hidden" id="hidSelBankCode" />
                    <input type="hidden" id="hidReceiveName" />
                    <input type="hidden" id="hidBankNum" />
                <span id="spBankName"></span><br />
                 <span  id="spReceiveNum"></span><br />
                <span  id="spBankNum"></span>
            </div>
        </div>
        

          <div class="marg">
                    <span class="col-sm-12 col-xs-12 marg ">
                        <input type="text" class="form-control" style="width:45%;float:left" placeholder="请填写验证码" id="VerificationCode" require-type="require" require-msg="验证码" />
                        <input type="button" class="form-control btn-success" value="获取验证码" id="btnGetCode" onclick="sendCode(this)" style="margin-left: 5px;width:50%;float:left" />
                    </span>

                </div>



        <div class="marg">
    <div class="col-sm-6 col-xs-6 col-sm-offset-3 col-xs-offset-3">
        <a class="btn btn-block gree" onclick="setupChange()" href="javascript:void(0)">提    交</a>
    </div>
         </div>
    </div>
      <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-body">
                <div class="row list-card">
            <span class="col-sm-12 col-xs-12">
              提现收款信息
            </span>

             <div class="marg">
            <span class="col-sm-12 col-xs-12">
                <select id="ddl_Bank" class="form-control required" runat="server">
                </select>
            </span>
        </div>

                       <div class="marg">
            <span class="col-sm-12 col-xs-12 marg">
                <input type="text" class="form-control" placeholder="收款人姓名" id="txt_ReceiveName"  />
            </span>
        </div>
                    
        <div class="marg">
            <span class="col-sm-12 col-xs-12 marg">
                <input type="text" class="form-control" placeholder="收款账号" id="txt_BankNum"  />
            </span>
        </div>

                    <div class="marg">
            <span class="col-sm-6 col-xs-6 marg">
             <input type="button" value="确定" class="btn btn-success"  onclick="addTXBank()" />
            </span>

        <span class="col-sm-6 col-xs-6 marg">
             <input type="button" value="历史收款信息" class="btn btn-info"  data-toggle="modal" data-target="#myBankListModal"  onclick="hideaddbank()"/>
            </span>
        </div>

                </div>
            </div>
        </div>
    </div>
    
    <div class="modal fade" id="myBankListModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-body">
                <div class="row">
                    <span class="col-sm-12 col-xs-12">
                      历史收款信息
                    </span>

                    <asp:Repeater ID="repBankList" runat="server">
                        <ItemTemplate>
                                 <div class="marg">
                                <span class="col-sm-12 col-xs-12 marg">
                                    <input type="radio" class="rdChkBank" value="<%#Eval("Code") %>" /> 
                                    <span class="spBankName"> <%#GetBankName(Eval("Bank")) %></span>
                                     <span class="spReceiveName"> <%#Eval("ReceiveName") %></span>
                                     <span  class="spBankNum"><%#Eval("BankNum") %></span>
                                </span>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                     <div class="marg">
            <span class="col-sm-6 col-xs-6 marg">
              <input type="button" value="选择" class="btn btn-info"  onclick="choiceBankInfo()"/>
            </span>
                          <span class="col-sm-6 col-xs-6 marg" style="text-align:right">
              <input type="button" value="取消" class="btn btn-danger"  onclick="hideBankInfo()"/>
            </span>
        </div>

                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">
        function choiceBankInfo() {
            var choiceChk = $('input:radio[class="rdChkBank"]:checked');
            if (choiceChk.val() == undefined) {
                layerMsg("请选择收款信息");
                return false;
            }
            else {
                $("#divBankInfo").show();
                var bankCode = choiceChk.val();
                var bankName = choiceChk.parent().find(".spBankName").text();
                var receiveName = choiceChk.parent().find(".spReceiveName").text();
                var bankNum = choiceChk.parent().find(".spBankNum").text();
                //$("#hidSelBank").val(bank);
                $("#hidSelBankCode").val(bankCode);
             
                $("#spBankName").text("提现到：" + bankName);
                $("#spReceiveNum").text("收款人：" + receiveName);
                $("#spBankNum").text("收款账号：" + bankNum);
                $('#myBankListModal').modal('toggle');
            }

        }
        function hideaddbank() {
            $('#myModal').modal('toggle');
        }
        function hideBankInfo() {
            $('#myBankListModal').modal('toggle');
        }
        function addTXBank() {
            var bank = $("#ddl_Bank option:selected").val();
            var bankName = $("#ddl_Bank option:selected").text();
            var bankNum = $("#txt_BankNum").val();
            var receiveNum = $("#txt_ReceiveName").val();
            if (bank == '' || $.trim(bankNum) == '') {
                layerMsg("提现银行或账号不能为空");
                return false;
            }
            $("#divBankInfo").show();
            //显示到页面上
            $("#hidSelBank").val(bank);
            $("#hidBankNum").val(bankNum);
            $("#hidReceiveName").val(receiveNum);
            $("#hidSelBankCode").val("");
            $("#spBankName").text("提现到：" + bankName);
            $("#spReceiveNum").text("收款人："+receiveNum);
            $("#spBankNum").text("收款账号："+bankNum);
            $('#myModal').modal('toggle');
        }

        var clock = '';
        var nums = 120;
        var btn;
        function sendCode(thisBtn) {
            //校验手机号
            var info = {
                type: 'sendTelCode',
                sendtype: '3' //提现
            };
            var result = GetAjaxString(info);
            if (result == "1") {
                btn = thisBtn;
                btn.disabled = true; //将按钮置为不可点击
                btn.value = nums + '秒后可重新获取';
                //发送验证码
                layerMsg("验证码发送成功");
                clock = setInterval(doLoop, 1000); //一秒执行一次
            }
            else if (result == "-1") {
                layerAlert("对不起，您不是VIP用户，VIP用户才能提现，请先升级！");
            }
            else {
                layerAlert("验证码发送失败，请重试");
            }
        }
        function doLoop() {
            nums--;
            if (nums > 0) {
                btn.value = nums + '秒后可重新获取';
            } else {
                clearInterval(clock); //清除js定时器
                btn.disabled = false;
                btn.value = '点击获取验证码';
                nums = 10; //重置时间
            }
        }
        var itemType = "1";
   
        function setupChange() {
            if (!checkForm())
                return false;
            var canApplayMoney = parseFloat($.trim($("#spCanApplyMoney").text()));
            var applyMoney = parseFloat($.trim($("#txtMoney").val()));
            if (applyMoney > canApplayMoney) {
                alertTip();
                return false;
            }
            if ($("#hidSelBank").val() == "" || $("#hidBankNum").val() == "") {
                layerMsg("请填写或选择提现信息！", { icon: 6 });
                return false;
            }
            //查看提现金额是不是100的整数倍
            var txBase = '<%=Service.CacheService.GlobleConfig.BaseJifen%>';
            if (applyMoney % parseFloat(txBase) != 0) {
                layerMsg("提现金额应为" + txBase + "的倍数！", { icon: 6 });
                return false;
            }
            layerLoading();

            var userInfo = {
                type: 'TX',
                txmoney: applyMoney,
                txway: '3', //体现方式
                alipay: $("#uploadImg").val(),
                weixinpay: $("#uploadImgWeixin").val(),
                sendcode: $("#VerificationCode").val(),
                txbank: $("#hidSelBank").val(),//提现银行
                txbankcode: $("#hidSelBankCode").val(),//提现银行Code
                txbanknum: $("#hidBankNum").val(),//提现银行账号
                txreceivenum: $("#hidReceiveName").val()//提现收款人
            };
            var result = GetAjaxString(userInfo);
            closeLayerLoading();
            if (result == "0") //登陆成功
            {
                layerAlert("申请成功，我们会尽快为您提现！", { icon: 6 });
                setTimeout(function () { window.location.reload(); }, 2000);
            }
            else if (result == "-1")
                layerMsg("操作失败，请重试！", { icon: 6 });
            else if (result == "1")
                layerMsg("提现金额大于您的可提现金额！", { icon: 6 });
            else if (result == "2") {
                alertTip();
            }
            else if (result == "3")
                layerMsg("提现金额小于最低提现金额！", { icon: 6 });
            else if (result == "4")
                layerMsg("提现金额应为" + txBase + "的倍数！", { icon: 6 });
            else if (result == "5")
                layerMsg("请输入安全问题答案！", { icon: 6 });
            else if (result == "-3")
                layerAlert("请输入正确的验证码！");
            else if (result == "-4")
                layerAlert("对不起，您不是VIP用户，VIP用户才能提现，请先升级！");
            else
                layerAlert(result);
        }
        function alertTip() {
            layerAlert("抱歉，您的账户余额不足，请重新填写金额。");
            return false;
        }
    
    </script>

</asp:Content>
