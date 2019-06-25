<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TXLog.aspx.cs" Inherits="Web.Admin.Finance.TXLog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .txImg {
            width: 220px;
        }
    </style>
    <script type="text/javascript">
        tState = '';
        tUrl = "Handler/TXLogList.ashx";
        doSearch();
        //查询，加入参数
        function doSearch() {
            searchPram = [];
            searchPram.push({ name: $("#nBank").attr("id"), value: $("#nBank").val() });
            searchPram.push({ name: $("#nMID").attr("id"), value: $("#nMID").val() });
            searchPram.push({ name: $("#nPayBeginTime").attr("id"), value: $("#nPayBeginTime").val() });
            searchPram.push({ name: $("#nPayEndTime").attr("id"), value: $("#nPayEndTime").val() });
            SearchByCondition(searchPram);
            countTotal();
        }


        function countTotal() {
            var searchTotalPram = [];
            searchTotalPram.push({ name: $("#nBank").attr("id"), value: $("#nBank").val() });
            searchTotalPram.push({ name: $("#nMID").attr("id"), value: $("#nMID").val() });
            searchTotalPram.push({ name: $("#nPayBeginTime").attr("id"), value: $("#nPayBeginTime").val() });
            searchTotalPram.push({ name: $("#nPayEndTime").attr("id"), value: $("#nPayEndTime").val() });
            var rek = '<%=Request.RawUrl%>';
            //获取最后一个/和?之间的内容，就是请求的页面
            rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
            $.ajax({
                type: 'post', url: 'Finance/' + rek + '?Action=OTHER', data: searchTotalPram,
                success: function (info) {
                    var array = info.split('*');
                    $("#spTotal").html(array[0]);
                    $("#spTotalFee").html(array[1]);
                    $("#spTotalReal").html(array[2]);
                }
            });
        }

        function changeTXStatus(status, code, obj) {
            if (!confirm("确定执行该操作吗？"))
                return;
            var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            var result = GetAjaxString('changeTXStatus', code + '&status=' + status);
            layer.close(index);
            if (result == "1") {
                layer.alert("操作成功");
                SearchByCondition();
            }
            else {
                layer.alert(result);
            }
        }
        function showTXInfo(i, mid) {
            layer.open({
                type: 1,
                shade: [0.8, '#393D49'],
                title: ['申请人：' + mid, 'font-size:18px;background:#5bc0de'],
                content: $('#divToTX' + i), //捕获的元素
                area: '500px',
                btn: ['关闭'],
                yes: function (index) {
                    $('#divToTX' + i).hide();
                    layer.close(index);
                },
                cancel: function (index) {
                    $('#divToTX' + i).hide();
                    layer.close(index);
                }
            });
        }

        function moreOperate(code, tbank, TXMoney, FeeMoney, RealMoney, Img, bankName, remark, TXMCode) {
            $("#img_ShouKuan").attr('src', Img);
            $("#txt_RealMoney").html(RealMoney);
            if (tbank == 1){
                $("#spQRimg").show();
                $("#lab_AliOrWeixin").html("支付宝");
            }
            else if (tbank == 2){
                $("#spQRimg").show();
                $("#lab_AliOrWeixin").html("微信");
            }
            else if (tbank == 3) {
                $("#spQRimg").hide();
                $("#lab_AliOrWeixin").html("提现银行：" + bankName + "<br/>收款人：" + remark + "。<br/>收款账号：" + Img);
            }
            else if (tbank == 4) {
                $("#spQRimg").hide();
                $("#lab_AliOrWeixin").html("TPay手机号后四位：" + remark + "<br/>账号（ID）：" + Img + "。<br/>昵称：" + TXMCode);
            }
            else if (tbank == 5) {
                $("#spQRimg").hide();
                $("#lab_AliOrWeixin").html("VPay手机号后四位：" + remark + "<br/>账号（ID）：" + Img + "。<br/>昵称：" + TXMCode);
            }
            $("#lab_TXMoney").html(TXMoney);
            $("#lab_FeeMoney").html(FeeMoney);

            layer.open({
                type: 1,
                shade: [0.8, '#393D49'],
                title: ['转账', 'font-size:18px;background:#5bc0de'],
                content: $('#layLoginUI'), //捕获的元素
                btn: ['转账', '取消'],
                area: ['45%', '450px'],
                yes: function (index) {
                    //ajax数据
                    var userInfo = {
                        type: 'UpdateTXLog',
                        valCode: code,
                    };
                    var result = GetAjaxString(userInfo);
                    if (result == "1") //保存成功
                    {
                        layer.alert("转账成功！", { icon: 6 });
                        callhtml('Finance/TXLog.aspx', '提现管理');
                        layer.close(index);
                    }
                    else
                        layer.alert(result);
                },
                cancel: function (index) {
                    layer.close(index);
                    //window.location = "index.aspx";
                }
            });
        }
    </script>
</head>
<body>
    <div id="mempay">
        <div class="control">
            <div class="search" id="DivSearch">
            <select id="nBank" >
                <option value="">--提现方式--</option>
                 <option value="1">支付宝</option>
                 <option value="2">微信</option>
            </select>
                <input id="nMID"  placeholder="申请人账号" type="text" class="sinput" />
                <input id="nPayBeginTime" placeholder="申请开始时间" type="text" class="sinput" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
                <input id="nPayEndTime"  placeholder="申请结束时间" type="text" class="sinput" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
                    <input type="button" value="申请提现" class=" btn btn-danger" onclick="callhtml('Finance/ApplyTX', '申请提现')" />
                <input type="button" value="查询" class=" btn btn-success" onclick="doSearch()" />
            </div>
        </div>
        <div class="ui_table">
            <table cellpadding="0" cellspacing="0" class="tabcolor" id="Result">
           <thead> <tr>
                    <th>序号
                    </th>
                    <th>申请人账号
                    </th>
                    <th>申请时间
                    </th>
                    <th>提现金额
                    </th>
                    <th>手续费
                    </th>
                    <th>到账金额
                    </th>
                    <th>收款信息
                    </th>
                    <th>进展状况
                    </th>
               <%if (TModel.Role.IsAdmin || TModel.RoleCode == "Admin")
                 {%>
                    <th>操作
                    </th>
               <%} %>
                </tr> </thead>
                  <tbody id="layerAppendView">
                </tbody>
            </table>
            <div class="ui_table_control">
                 <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent"></div>
            </div>
                <div style="float:left">
                 <div style="margin-top: -35px;font-size:14px">
                        提现金额合计：<span id="spTotal" style="color:red;font-size:14px"></span><br/>
                     手续费合计：<span id="spTotalFee" style="color:red;font-size:14px"></span><br/>
                     实际到账金额合计：<span id="spTotalReal" style="color:red;font-size:14px"></span>
                 </div>
            </div>
        </div>

        <div class="row" id="divToLogin" >
                <table class="layer_notice layui-layer-wrap" id="layLoginUI" style="display: none;margin-left:20px">
                    <tr>
                        <td class="tdlable" colspan="2"><label id="lab_AliOrWeixin"></label></td>
                        <td class="tdlable" id="spQRimg">&emsp;收款二维码：<img class="coverImg"  id="img_ShouKuan"  style="width:180px"/></td>
                    </tr>
                     <tr>
                        <td class="tdlable">申请提现金额：</td>
                        <td class="tdlable"><label id="lab_TXMoney"></label></td>
                    </tr>
                    <tr>
                        <td class="tdlable">手续费 ：</td>
                        <td class="tdlable"><label id="lab_FeeMoney"></label></td>
                    </tr>
                    <tr>
                      <td class="tdlable">实际到账金额 ：</td>
                        <td class="tdlable"><label id="txt_RealMoney" style="color:red;"></label></td>
                    </tr>
                </table>

            </div>
           <script id="demo" type="text/html">
         {{# for (var i = 0; i<d.length; i++){ }}
          <tr> <td>{{ d[i].RowNumber }}</td><td>{{ d[i].MID }}</td><td>{{ d[i].CutTime }}</td>
              <td>{{ d[i].TXMoney }}</td>
              <td>{{ d[i].FeeMoney }}</td>
              <td>{{ d[i].RealMoney }}</td>
              <td>{{ d[i].TXName }}</td>
              <td>{{Number( d[i].Status)>1?' 已转账':'提交申请' }}</td>
                 <%if (TModel.Role.IsAdmin || TModel.RoleCode == "Admin")
                   {%>
                 <td>
                        <input type='button' class="btn {{ Number(d[i].Status)>1?' btn-warning disabled':' btn-info' }}" value="{{Number( d[i].Status)>1?' 已转账/详情':' 转账/详情' }}" onclick="moreOperate('{{ d[i].Code }}', '{{ d[i].TXBank }}', '{{ d[i].TXMoney }}', '{{ d[i].FeeMoney }}', '{{ d[i].RealMoney }}', '{{ d[i].TXCard}}', '{{ d[i].TXName}}', '{{ d[i].Remark}}', '{{ d[i].TXMCode}}')"/>
                 </td>
                <%} %>
          </tr>
        {{# } }}
         </script>
    </div>
</body>
</html>
