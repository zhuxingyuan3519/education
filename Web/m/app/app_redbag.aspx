<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="app_redbag.aspx.cs" Inherits="Web.m.app.app_redbag" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--络胜红包--%>
  <style type="text/css">
    </style>
    <script src="/common/laypage/laypage.js"></script>
    <script src="/common/laytpl/laytpl.js"></script>
         <script type="text/javascript">
             $(function () {
                 getPageList(1);
             });
             function getPageList(curr) {
                 var pagesi = 30;
                 //分页参数，需要传到后台的
                 var param = { cid: '', pageIndex: curr, pageSize: pagesi, type: type };
                 //ajax获取
                 $.getJSON("/Handler/GetRedBagPageList.ashx", param, function (res) {
                     var gettpl = document.getElementById('demo').innerHTML;
                     if (type == 2)
                         gettpl = document.getElementById('demo2').innerHTML;
                     laytpl(gettpl).render(res.Rows, function (html) {
                         $('#appendView').html(html);
                     });
                     var pages = Math.ceil(res.Total / pagesi); //得到总页数
                     //显示分页
                     laypage({
                         cont: 'pageContent', //容器。值支持id名、原生dom对象，jquery对象。【如该容器为】：<div id="page1"></div>
                         pages: pages, //通过后台拿到的总页数
                         groups: 0,
                         first: false,
                         last: false,
                         curr: curr || 1, //当前页
                         jump: function (obj, first) { //触发分页后的回调
                             if (!first) { //点击跳页触发函数自身，并传递当前页：obj.curr
                                 getPageList(obj.curr);
                             }
                         }
                     });
                 });
             }
             function showRedBagList() {
                 type = 1;
                 $("#spTitle").html("收到红包记录");
                 getPageList(1);
             }

             function showHasRedBagList() {
                 type = 2;
                 $("#spTitle").html("发出红包记录");
                 getPageList(1);
             }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div class="row bg-wh">
        <h5><b>|</b>总计收益</h5>
    </div>
    			<div class="row">
				<div class="col-sm-12 col-xs-12 text-center">
					<span>总资产：<%=totalMoney%></span>&emsp;&emsp;
                    <span>未激活红包金额：<%=noActiveRedBagMoney%></span>
				</div>
	<div class="col-sm-12 col-xs-12 text-center">
         <a href="javascript:void(0)"  class="btn btn-success btn-sm" style="margin-top:-5px;"  onclick="showRedBagList()">收到的红包</a>
             <a href="javascript:void(0)"  class="btn btn-info btn-sm" style="margin-top:-5px;margin-left:10px"  onclick="showHasRedBagList()">发出的红包</a>
             <a href="javascript:void(0)"  class="btn btn-danger btn-sm" style="margin-top:-5px;margin-left:10px"  data-toggle="modal" data-target="#myModal"  >发红包</a>
          
	</div>
			</div>


    <div class="row bg-wh">
        <h5><b>|</b><span id="spTitle">红包收发记录</span></h5>
    </div>
    <div id="appendView" ></div>
    <div class="clearfix"></div>

    <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent">
    </div>
   
      <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
		<div class="modal-dialog modal-content">
<div class="modal-body">
      <div class="row list-card">
            <div class="marg addmarg">
                <span class="col-sm-5 col-xs-5  " style="text-align:right">红包个数：</span>
                <span class="col-sm- col-xs-7 addvalue">
                    <input type="text" class="form-control" placeholder="红包个数" id="txt_RedBagCount" value="2"  />
                </span>
            </div>
          </div>

</div>
             <div class="modal-footer">
				<button type="button" class="btn btn-foot btn-default" data-dismiss="modal">取消
				</button>
				<button type="button" class="btn btn-foot btn-success" data-dismiss="modal" onclick="addRedBag()" >
					发红包
				</button>
			</div>
            </div></div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">

     <script type="text/javascript">
         $(function () {

         });
         function operatorRedBag(code, obj) {
             //var loadIndex = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
             layerLoadingIndex();
             var userInfo = {
                 type: 'operatorRedBag',
                 code: code
             };
             var result = GetAjaxString(userInfo);
             //layer.close(loadIndex);
             closeLayerLoading();
             if (result == '1') {
                 layerAlert("领取成功");
                 $(obj).val("已领取").attr("disabled", "disabled").removeAttr("onclick");
             }
         }
         function getRedBagStatus(status) {
             var result = '冻结中';
             switch (status) {
                 case '4': result = '已领取'; break;
                 case '3': result = '可领取'; break;
             }
             return result;
         }


         function addRedBag() {
             var count = $.trim($("#txt_RedBagCount").val());
             if (!(/(^\d+$)/.test(count)) || count < 0) { layerMsg("红包数量只能填正整数"); return false; }
             if (parseInt(count) <= 1)
             {
                 layerAlert("红包数量至少要发2个。");
                 return false;
             }
             var userInfo = {
                 type: 'addRedBag',
                 count: count
             };
             var result = GetAjaxString(userInfo);
             if (result == "0") {
                 layerMsg("红包数量至少要发2个");
                 $('#myModal').modal('toggle');
                 return false;
             }
             else if (result == "1") {
                 layerMsg("账户金额不足");
                 var alertInfo = "账户金额不足，是否现在去充值？";
                 layer.open({
                     content: alertInfo
                   , btn: ['立即充值', '我再逛逛']
                   , yes: function (index) {
                       //跳转到充值页面
                       window.location = "finance_recharge";
                   }
                 });
             }
             else if (result == "3") {
                 layerAlert("发放成功");
                 setTimeout("window.location.href='app_redbag.aspx'", 1200)
             }
             else {
                 layerAlert(result);
                 return false;
             }
         }

    </script>


     <script id="demo" type="text/html">
     {{# for (var i = 0; i<d.length; i++){ }}
			<div   class="row marg list-border itemlist" >
                <div class="clickButton">
				    <div class="col-sm-12 col-xs-12 div_in_list">
				       <span class="col-sm-12 col-xs-12 text-left">    {{ d[i].CutTime }} 来自
                                                             {{ d[i].FromUserCode }}的
                                                         {{ d[i].RedBagMoney }}元红包</span>
                       <input type="button" class="btn btn-{{ d[i].Status=="3"?"success":"danger" }} btn-sm hidden" value="{{ getRedBagStatus(d[i].Status) }}"  {{ d[i].Status=="3"?"":"disabled='disabled'" }}  onclick="operatorRedBag('{{ d[i].Code }}', this)" />
				    </div>
                </div>
			</div>
        {{# } }}
    </script>

    <script id="demo2" type="text/html">
     {{# for (var i = 0; i<d.length; i++){ }}
			<div   class="row marg list-border itemlist" >
                <div class="clickButton">
				    <div class="col-sm-12 col-xs-12 div_in_list">
				       <span class="col-sm-12 col-xs-12 text-left">  
                              {{ d[i].CutTime }} 发出{{ d[i].RedBagCount }}个{{ d[i].RedBagMoney }}元的红包。
				       </span>
				    </div>
                </div>
			</div>
        {{# } }}
    </script>
</asp:Content>
