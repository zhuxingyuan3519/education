<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="tx_apply.aspx.cs" Inherits="Web.m.app.tx_apply" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/js/uploadifive/uploadifive.css" rel="stylesheet" />
    <script src="/js/uploadifive/jquery.uploadifive.js"></script>
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
             
        ul > li {
            display: inline-flex;
            line-height: 40px;
        }
        .a-addbtn{width:100%;}
    </style>
    <script type="text/javascript">
        $(function () {
            var errormsg = '<%=ErrorMsg%>';
            if ($.trim(errormsg) != "") {
                layerAlert(errormsg);
            }

            if ($("#uploadImg").val() == "") {
                $("#imhDelimg").hide();
            }
            else {
                $("#imhDelimg").attr("onclick", "deletePic('" + $("#uploadImg").val() + "',this)");
            }
            //if (roleCode=="Member") 
            //        layerAlert("对不起，您还未购买过任何课程，购买过课程才能提现");
            loadUploadify();
        });
        function deletePic(code, obj) {
            layer.confirm('确定要删除吗?', function (index) {
                $.ajax({
                    type: 'get',
                    url: '/Handler/DeleteUPFile.ashx?picName=' + code,
                    success: function (info) {
                        layer.close(index);
                        if (info == "0")
                            layer.alert("删除失败，请重试！");
                        else if (info == "1") {
                            if (code == $("#hidMainPic").val())
                                $("#hidMainPic").val("");
                            $(obj).parent().remove();
                        }
                        else
                            layer.alert(info);
                    }
                });
                layer.close(index);
            });
        }
        var ftype = 0;
        function loadUploadify() {
            var filetypes = '*';
            $('#file_upload').uploadifive({
                'auto': false,
                //'fileObjName': 'fileData', //传递到服务器的file取到的name。默认是fileData,可以不填
                'formData': { 'uptype': 'asset' },  //需要传递到服务器的数据，不用时注掉
                'buttonText': '选择文件',
                'queueID': 'queue',
                'multi': false,
                //'uploadLimit': 1,
                'queueSizeLimit': 1,
                'uploadScript': '/Handler/UploadExcel.ashx',
                'fileType': filetypes,
                'onUploadComplete': function (file, data) {
                    if (iyt == 0) {
                        $("#uploadImg").val("/Attachment/" + data);
                        $("#imgappendimg").attr("src", "/Attachment/" + data);
                    }
                    else if (iyt == 1) {
                        $("#uploadImgWeixin").val("/Attachment/" + data);
                        $("#imgappendimgWeixin").attr("src", "/Attachment/" + data);
                    }
                    $('#myModal').modal('toggle');
                },
                'onUploadError': function (file, data) {
                    alert(data);
                }
                //,'onAddQueueItem': function (file) {
                //    alert('The file ' + file.name + ' was added to the queue!');
                //}
            });
        }
        var iyt = 0;
        function showUpload(obj, itype) {
            iyt = itype;
            $('#file_upload').uploadifive('clearQueue');
            //$("#file_upload").uploadifyCancel(queueID);
        }
        function uploadFiles() {
            //var myid = 1234;
            //$('#file_upload').data('uploadifive').settings.formData = { 'ID': myid };   //动态更改formData的值 ,需要时可以这样用
            $('#file_upload').uploadifive('upload');
        }

        function showAddTXPay(paytype,puy) {
            $("#div_pay").html(paytype + "<br />收款信息");
            $(".sp_pay").html(paytype);
            $(".tx1").hide();
            $(".tx2").hide();
            $(".txbank").hide();
            $(".txpay").show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row bg-wh">
        <h5><b>|</b>申请提现  <a href="tx_list.aspx"  style="padding: 6px 8px; color:#000; float: right; margin-right: 5px">提现记录</a></h5>
    </div>
    <div class="row marg list-border itemlist">
        <div class="col-sm-12 col-xs-12 padding-left-right-0" style="padding-left:5px !important;color:red">
         温馨提示：提现金额需为10或100的倍数；提现手续费为5元/笔（T+1到账）。
        </div>
    </div>
    <div class="row list-card txcardscreen">
        <div class="marg">
            <div class="col-sm-3 col-xs-3 margs text-left">申请金额</div>
            <div class=" col-xs-9  marg">
                <input type="text" class="form-control" id="txtMoney" require-msg="申请金额" require-type="decimal" placeholder="<%=Service.CacheService.GlobleConfig.MinTXMoney %>元以上可以提现" />
            </div>
        </div>

        <div class="marg">
            <div class="col-sm-3 col-xs-3 margs text-left" style="margin-top: 15px">提现到</div>
            <div class="col-sm-9 col-xs-9 marg" style="padding-bottom: 15px">
                <input type="radio" name="rdTx" value="1" checked="checked" style="margin-top: 13px;" onclick="showTXUpload('1', '2')" />支付宝
                <input type="radio" name="rdTx" value="2" style="margin-top: 13px; margin-left: 10px" onclick="showTXUpload('2', '1')" />微信
                <input type="radio" name="rdTx" value="3" style="margin-top: 13px; margin-left: 10px" onclick="showAddTXBank('3', '1')" />银行卡<br />
                <input type="radio" name="rdTx" value="4" style="margin-top: 13px; " onclick="showAddTXPay('TPay', '1')" />TPay&nbsp;&nbsp;
              <%--  <input type="radio" name="rdTx" value="5" style="margin-top: 13px; margin-left: 10px;display:none" onclick="showAddTXPay('VPay', '1')" /> --%>
                  <%--VPay--%>
            </div>
        </div>


        <div class="marg tx1">
            <div class="col-sm-3 col-xs-3 margs text-left">
                支付宝<br />
                提现信息
            </div>
            <div class="col-sm-9 col-xs-9 marg ">
                <a href="javascript:void(0)" class="btn  gree a-addbtn" title="btnUpload" data-toggle="modal" data-target="#myModal" onclick="showUpload(this,0)">上传支付宝收款二维码</a>
            </div>
            <div class="col-sm-12 col-xs-12 marg mainpiccontain">
                <input type='hidden' id="uploadImg" runat="server" class="uploadImg" />
                <img class='appendImg' id="imgappendimg" runat="server" />
            </div>
        </div>
        <div class="marg tx2" style="display: none">
            <div class="col-sm-3 col-xs-3 margs text-left">
                微信<br />
                提现信息
            </div>
            <div class="col-sm-9 col-xs-9 marg text-right">
                <a href="javascript:void(0)" class="btn   gree a-addbtn" title="btnUpload" data-toggle="modal" data-target="#myModal" onclick="showUpload(this,1)">上传微信收款二维码</a>
            </div>
            <div class="col-sm-12 col-xs-12 marg mainpiccontain">
                <input type='hidden' id="uploadImgWeixin" runat="server" class="uploadImgWeixin" />
                <img class='appendImg' id="imgappendimgWeixin" runat="server" />
            </div>
        </div>

        <div class="marg txbank" style="display: none">
            <div class="col-sm-3 col-xs-3 margs text-left">
                银行<br />
                提现信息
            </div>
            <div class="col-sm-9 col-xs-9 marg  text-right">
                <a href="javascript:void(0)" class="btn   gree a-addbtn" data-toggle="modal" data-target="#myModalAddTxBank">添加银行提现信息</a>
            </div>
                <div class="col-sm-12 col-xs-12 marg mainpiccontain"></div>
            <div class="col-sm-12 col-xs-12 marg mainpiccontain" id="divBankInfo" style="display:none">
                <input type="hidden" id="hidSelBank" />
                <input type="hidden" id="hidSelBankCode" />
                <input type="hidden" id="hidReceiveName" />
                <input type="hidden" id="hidBankNum" />
                <ul>
                    <li><span id="spBankName"></span></li>
                    <li><span id="spReceiveNum"></span></li>
                    <li><span id="spBankNum"></span></li>
                </ul>
            </div>
        </div>

        <div class="marg txpay" style="display: none">
            <div class="col-sm-3 col-xs-3 margs text-left" id="div_pay">
            </div>
            <div class="col-sm-9 col-xs-9 marg">
              <span class="sp_pay"></span>手机号后四位:<input type="text" class="form-control" id="txt_payname" /><br />
             <span class="sp_pay"></span>账号（ID）:<input type="text" class="form-control" id="txt_paynum" />
                <br />
             <span class="sp_pay"></span>昵称:<input type="text" class="form-control" id="txt_nickname" />
            </div>
        </div>

        <div class="marg">
            <span class="col-sm-6 col-xs-6 marg">
                <input type="text" class="form-control" placeholder="请填写验证码" id="VerificationCode" require-type="require" require-msg="验证码" />
            </span>
            <span class="col-sm-6 col-xs-6 marg">
                <input type="button" class="form-control  gree"  value="获取验证码" id="btnGetCode" onclick="sendCode(this)" style="padding-left: 5px;line-height: 1.42857143;
    padding: 6px 12px !important;" />
            </span>
        </div>



        <div class="marg">
            <div class="col-sm-12 col-xs-12 marg">
                <a class="btn btn-block gree" onclick="setupChange()" href="javascript:void(0)">提&emsp;交</a>
            </div>
        </div>
    </div>


    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-content">
            <div class="modal-body">
                <div class="row list-card">
                    <div class="marg addmarg" id="queue"></div>
                    <div class="marg addmarg" style="padding-left:20px">
                        <input id="file_upload" name="file_upload" type="file" multiple="false" />
                    </div>
                    <div class="marg addmarg" style="padding-left:20px">
                        <input type="button" onclick="uploadFiles()" class="btn btn-sm btn-success" style="width: 30%;" value="开始上传" />
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

    <div class="modal fade" id="myModalAddTxBank" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">

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
            else if (result == "-2") {
                layerAlert("对不起，您推荐的人数不足三人，暂不能提现。");
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

        function showAddTXBank() {
            $(".tx1").hide();
            $(".tx2").hide();
            $(".txpay").hide();
            $(".txbank").show();
        }


        function showTXUpload(showindex, hideindex) {
            $(".tx" + hideindex).hide();
            $(".tx" + showindex).show();
            $(".txbank").hide();
            $(".txpay").hide();
        }

        function setupChange() {
            if (!checkForm())
                return false;
            var canApplayMoney = parseFloat($.trim($("#spCanApplyMoney").text()));
            var applyMoney = parseFloat($.trim($("#txtMoney").val()));
            if (applyMoney > canApplayMoney) {
                alertTip();
                return false;
            }
            var txWay = $("input[name='rdTx']:checked").val();
            if (txWay == "1") {
                if ($(".uploadImg").val() == '') {
                    layerMsg("请上传支付宝收款二维码！", { icon: 6 });
                    return false;
                }
            }
            else if (txWay == "2") {
                if ($(".uploadImgWeixin").val() == '') {
                    layerMsg("请上传微信收款二维码！", { icon: 6 });
                    return false;
                }
            }
            else if (txWay == "3") {
                if ($("#spBankName").text() == '' || $("#spBankNum").text() == '') {
                    layerMsg("请填写提现银行信息！");
                    return false;
                }
            }
            else if (txWay == "4") {
                if ($("#txt_payname").val() == '' || $("#txt_paynum").val() == '') {
                    layerMsg("请填写完整的TPay收款信息！");
                    return false;
                }
            }
            else if (txWay == "5") {
                if ($("#txt_payname").val() == '' || $("#txt_paynum").val() == '') {
                    layerMsg("请填写完整的VPay收款信息！");
                    return false;
                }
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
                txway: txWay, //体现方式
                alipay: $(".uploadImg").val(),
                weixinpay: $(".uploadImgWeixin").val(),
                sendcode: $("#VerificationCode").val(),
                txbank: $("#hidSelBank").val(),//提现银行
                txbankcode: $("#hidSelBankCode").val(),//提现银行Code
                txbanknum: $("#hidBankNum").val(),//提现银行账号
                txreceivenum: $("#hidReceiveName").val(),//提现收款人
                txpaynum: $("#txt_paynum").val(),//提现vpay账号
                txpayname: $("#txt_payname").val(),//提现vpay账户
                nickname: $("#txt_nickname").val()//昵称
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
                layerAlert("对不起，您还未购买过任何课程，购买过课程才能提现！");
            else if (result == "-5")
                layerAlert("对不起，您需要先发2个红包才能提现。");
            else if (result == "7")
                layerAlert("对不起，您的账户余额不足。");
            else if (result == "8")
                layerAlert("对不起，您的账户余额不足。");
            else if (result == "9")
                layerAlert("对不起，您推荐的人数不足三人，暂不能提现。");
            else
                layerAlert(result);
        }
        function alertTip() {
            layerAlert("抱歉，您的账户余额不足，请重新填写金额。");
            return false;
        }


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
                hideBankInfo();
            }

        }
        function hideaddbank() {
            $('#myModalAddTxBank').modal('toggle');
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
                layer.msg("提现银行或账号不能为空");
                return false;
            }
            $("#divBankInfo").show();
            //显示到页面上
            $("#hidSelBank").val(bank);
            $("#hidBankNum").val(bankNum);
            $("#hidReceiveName").val(receiveNum);
            $("#hidSelBankCode").val("");
            $("#spBankName").text("提现到：" + bankName);
            $("#spReceiveNum").text("收款人：" + receiveNum);
            $("#spBankNum").text("收款账号：" + bankNum);
            hideaddbank();
        }

    </script>
</asp:Content>
