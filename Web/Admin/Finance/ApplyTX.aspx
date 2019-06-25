<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApplyTX.aspx.cs" Inherits="Web.Admin.Finance.ApplyTX" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
    <style type="text/css">
        .appendImg {
            width: 180px;
            float: right;
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

        .upload_control {
            /*font-size: 12px;
            background-color: #e1d9c1;
            padding: 5px;*/
            margin-top: 10px;
        }

        .list-card,.marg {
            padding-top: 10px;
        }

        ul > li {
            display: inline-flex;
            line-height: 40px;
        }

            ul > li > span {
                font-size: 14px;
            }
    </style>
    <link href="/js/uploadifive/uploadifive.css" rel="stylesheet" />
    <script src="/js/uploadifive/jquery.uploadifive.js"></script>
</head>
<body>
    <div id="mempay">
        <div id="finance" class="container">
            <form id="form1">
                <table>
                    <tr>
                        <td class="tdlable">现金总额: <%=TModel.MSH %>
                        </td>
                        <td class="tdlable">可提现总额:<%=GetCanTXMoney %>
                        </td>
                        <td class="tdvalue">提现金额：<input type="text" id="txt_TXMoney" runat="server" style="width: 80px;" require-type="require" require-msg="提现金额" />&nbsp;&nbsp;体现到
                            <select id="txbank" runat="server" onchange="checkShowTxWay(this)">
                                <option value="1">支付宝</option>
                                <option value="2">微信</option>
                                <option value="3">银行卡</option>
                            </select>
                            <span>提现手续费<%=GetTXFloat%>元/笔</span>

                            <input type="button" class="btn btn-success" value="申请提现" onclick="setupChange()" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdlable txways" colspan="4" id="tdTxToAlipay">
                            <div class="input_lable input_lable_w">支付宝提现信息</div>
                            <a href="javascript:void(0)" class="upload_control btn btn-info" title="btnUpload" onclick="showUpload(this,0)">上传支付宝收款二维码</a>
                            <div class='mainpiccontain'>
                                <input type='hidden' id="uploadImg" runat="server" class="uploadImg" />
                                <img class='appendImg' id="imgappendimg" runat="server" />
                            </div>
                        </td>
                        <td class="tdvalue txways" colspan="4" id="tdTxToweixin">
                            <div class="input_lable input_lable_w">微信提现信息：</div>
                            <a href="javascript:void(0)" class="upload_control  btn btn-info" title="btnUpload" onclick="showUpload(this,1)">上传微信收款二维码</a>
                            <div class=' mainpiccontain'>
                                <input type='hidden' id="uploadImgWeixin" runat="server" class="uploadImgWeixin" />
                                <img class='appendImg' id="imgappendimgWeixin" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr class="txways" id="tdTxTobank">
                        <td colspan="4">
                            <div class="col-sm-12 col-xs-12 marg">
                                <a href="javascript:void(0)" class="btn btn-success btn-sm" data-toggle="modal" data-target="#myModalAddTxBank">添加银行提现信息</a>
                            </div>
                            <div class="col-sm-12 col-xs-12 marg mainpiccontain"></div>
                            <div class="col-sm-12 col-xs-12 marg mainpiccontain" id="divBankInfo" style="display: none">
                                <input type="hidden" id="hidSelBank" name="hidSelBank" />
                                <input type="hidden" id="hidSelBankCode" name="hidSelBankCode" />
                                <input type="hidden" id="hidReceiveName"  name="hidReceiveName" />
                                <input type="hidden" id="hidBankNum" name="hidBankNum" />
                                <ul>
                                    <li><span id="spBankName"></span></li>
                                    <li><span id="spReceiveNum"></span></li>
                                    <li><span id="spBankNum"></span></li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div class="modal fade" id="myBankListModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-content">
                                    <div class="modal-body">
                                        <div class="row">
                                            <span class="col-sm-12 col-xs-12">历史收款信息
                                            </span>

                                            <asp:Repeater ID="repBankList" runat="server">
                                                <ItemTemplate>
                                                    <div class="marg">
                                                        <span class="col-sm-12 col-xs-12 marg">
                                                            <input type="radio" class="rdChkBank" value="<%#Eval("Code") %>" />
                                                            <span class="spBankName"><%#GetBankName(Eval("Bank")) %></span>
                                                            <span class="spReceiveName"><%#Eval("ReceiveName") %></span>
                                                            <span class="spBankNum"><%#Eval("BankNum") %></span>
                                                        </span>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>

                                            <div class="marg">
                                                <span class="col-sm-6 col-xs-6 marg">
                                                    <input type="button" value="选择" class="btn btn-info" onclick="choiceBankInfo()" />
                                                </span>
                                                <span class="col-sm-6 col-xs-6 marg" style="text-align: right">
                                                    <input type="button" value="取消" class="btn btn-danger" onclick="hideBankInfo()" />
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
                                            <span class="col-sm-12 col-xs-12">提现收款信息
                                            </span>
                                        </div>
                                        <div class="row list-card">
                                            <span class="col-sm-12 col-xs-12">
                                                <select id="ddl_Bank" class="form-control required" runat="server">
                                                </select>
                                            </span>
                                        </div>

                                        <div class="row list-card">
                                            <span class="col-sm-12 col-xs-12 marg">
                                                <input type="text" class="form-control" placeholder="收款人姓名" id="txt_ReceiveName" />
                                            </span>
                                        </div>

                                        <div class="row list-card">
                                            <span class="col-sm-12 col-xs-12 marg">
                                                <input type="text" class="form-control" placeholder="收款账号" id="txt_BankNum" />
                                            </span>
                                        </div>

                                        <div class="row list-card">
                                            <span class="col-sm-6 col-xs-6 marg">
                                                <input type="button" value="确定" class="btn btn-success" onclick="addTXBank()" />
                                            </span>

                                            <span class="col-sm-6 col-xs-6 text-right">
                                                <input type="button" value="历史收款信息" class="btn btn-info" data-toggle="modal" data-target="#myBankListModal" onclick="hideaddbank()" />
                                            </span>
                                        </div>

                                    </div>
                                </div>
        </div>
        </td>
                    </tr>
                </table>
                <div id="layUploadContent" style="display: none">
                    <div id="queue"></div>
                    <input id="file_upload" name="file_upload" type="file" multiple="false" />
                    <input type="button" onclick="uploadFiles()" class="btn btn-sm btn-success" style="width: 30%;" value="开始上传" />
                </div>
        </form>
    </div>

    </div>
    <script type="text/javascript">
        function choiceBankInfo() {
            var choiceChk = $('input:radio[class="rdChkBank"]:checked');
            if (choiceChk.val() == undefined) {
                layer.alert("请选择收款信息");
                return false;
            }
            else {
                $("#divBankInfo").show();
                var bankCode = choiceChk.val();
                var bankName = choiceChk.parent().find(".spBankName").text();
                var receiveName = choiceChk.parent().find(".spReceiveName").text();
                var bankNum = choiceChk.parent().find(".spBankNum").text();
                $("#hidSelBank").val(bankName);
                $("#hidSelBankCode").val(bankCode);
                $("#hidReceiveName").val(receiveName);
                $("#hidBankNum").val(bankNum);

                $("#spBankName").text("提现到：" + bankName);
                $("#spReceiveNum").text("。收款人：" + receiveName);
                $("#spBankNum").text("。收款账号：" + bankNum);
                hideBankInfo();
            }

        }
        function checkShowTxWay(obj) {
            var thisVal = $(obj).val();
            $(".txways").hide();
            if (thisVal == "1")
                $("#tdTxToAlipay").show();
            else if (thisVal == "2")
                $("#tdTxToweixin").show();
            else if (thisVal == "3")
                $("#tdTxTobank").show();
        }

        function setupChange() {
            if (!checkForm()) {
                return false;
            }
            if (!confirm("确定要提现吗？")) return false;
            if ($("#txbank").val() == "1" && $("#uploadImg").val() == "") {
                layer.alert("请上传您的支付宝收款信息！");
                return false;
            }
            if ($("#txbank").val() == "2" && $("#uploadImgWeixin").val() == "") {
                layer.alert("请上传您的微信收款信息！");
                return false;
            }
            if ($("#txbank").val() == "3" && $("#hidSelBank").val() == "") {
                layer.alert("请配置您的银行卡提现信息！");
                return false;
            }

            var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            var rek = '<%=Request.RawUrl%>';
            //获取最后一个/和?之间的内容，就是请求的页面
            rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
            $.ajax({
                type: 'post',
                url: 'Finance/' + rek + '?Action=ADD',
                data: $('#form1').serializeArray(),
                success: function (info) {
                    layer.close(index);
                    var res = info;
                    if (res == "0")
                        layer.alert("操作失败，请重试");
                    else if (res == "1") {  //提交成功
                        layer.alert("申请成功，我们将尽快为您提现！");
                        callhtml('Finance/ApplyTX', '申请提现')
                    }
                    else
                        layer.alert(info);
                }
            });
        }
        $(function () {
            if ($("#uploadImg").val() == "") {
                $("#imhDelimg").hide();
            }
            else {
                $("#imhDelimg").attr("onclick", "deletePic('" + $("#uploadImg").val() + "',this)");
            }
            $(".txways").hide();
            $("#tdTxToAlipay").show();
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
                },
                'onUploadError': function (file, data) {
                    alert(data);
                }
            });
        }
        var iyt = 0;
        function showUpload(obj, itype) {
            iyt = itype;
            var areaVal = "516px";
            var windowWidth = $(document.body).width()
            if (windowWidth < 500)
                areaVal = "300px";
            var layIndex = layer.open({
                type: 1,
                offset: '100px',
                area: areaVal,
                shade: [0.8, '#393D49'],
                title: ['上传文件', 'font-size:18px;background:#5bc0de'],
                content: $('#layUploadContent'), //捕获的元素
                btn: ['关闭'],
                yes: function (index) {
                    layer.close(layIndex);
                }
            });
        }
        function uploadFiles() {
            //var myid = 1234;
            //$('#file_upload').data('uploadifive').settings.formData = { 'ID': myid };   //动态更改formData的值 ,需要时可以这样用
            $('#file_upload').uploadifive('upload');
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
            $("#spReceiveNum").text("。收款人：" + receiveNum);
            $("#spBankNum").text("。收款账号：" + bankNum);
            hideaddbank();
        }
        function hideaddbank() {
            $('#myModalAddTxBank').modal('toggle');
        }
        function hideBankInfo() {
            $('#myBankListModal').modal('toggle');
        }

        function showTXToBank() {
            $("#tdTxTobank").show();
        }
    </script>
</body>
</html>
