<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="card_list.aspx.cs" Inherits="Web.m.app.card_list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
          .bankimg{width:100%; height:auto !important;margin-top:5px;}
    </style>
    <script src="/common/js/jquery.touchSwipe.min.js"></script>
    <script src="/common/laypage/laypage.js"></script>
    <script src="/common/laytpl/laytpl.js"></script>
         <script type="text/javascript">
             $(function () {
                  setGuides('2', 'card_edit.aspx');
                 getPageList(1);
             });

             function addSwipe() {
                 var toLeft = '90px'; //向左移动的距离'
                 //滑动方法
                 $(".itemlist").swipe({
                     swipe: function (event, direction, distance, duration, fingerCount) {
                         $(".itemlist_delete").hide();
                         $(".itemlist").css("left", "0px");
                         var thisObj = $(this);
                         if (direction == 'left') {
                             thisObj.animate({ left: '-' + toLeft }, "fast", function () {
                                 thisObj.children(".itemlist_delete").show().css({ "right": "-" + toLeft, "width": toLeft });
                             });
                         }
                         else if (direction == 'right') {
                             thisObj.children(".itemlist_delete").hide();
                             thisObj.css("left", "0px");
                         }
                     },
					 allowPageScroll:"auto"
                 });
                 //单击方法-查看/编辑
                 $(".clickButton").swipe({
                     click: function (event, direction, distance, duration, fingerCount) {
                         var thisObj = $(this);
                         var code = thisObj.data('code');
                         seeDetaildjhd(code);
                     },
                 });
                 //单击方法-删除
                 $(".delButton").swipe({
                     click: function (event, direction, distance, duration, fingerCount) {
                         var thisObj = $(this);
                         var code = thisObj.data('code');
                         deleteArchive(thisObj, code);
                     },
                 });
             }

             function getPageList(curr) {
                 var pagesi = 100000;
                 //分页参数，需要传到后台的
                 var param = { cid: '', pageIndex: curr, pageSize: pagesi };
                 //ajax获取
                 $.getJSON("/Handler/GetArchivePageList.ashx?rand=" + Math.random(), param, function (res) {
                     if (res.Total > 0) {
                         setGuides('3', '');
                     }

                     var gettpl = document.getElementById('demo').innerHTML;
                     laytpl(gettpl).render(res.Rows, function (html) {
                         $('#appendView').html(html);
                         addSwipe();
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

             function deleteArchive(obj, co) {
                 layer.open({
                     content: '确定要删除吗？'
                    , btn: ['确定', '取消']
                    , yes: function (index, layero) {
                        var info = {
                            type: 'deleteArchive',
                            code: co
                        };
                        var result = GetAjaxString(info);
                        if (result == "0") {
                            layerAlert("删除成功");
                            $(obj).parent().remove();
                        }
                        else {
                            layerAlert("删除失败，请重试");
                        }
                        layer.close(index);
                    }, no: function (index, layero) {
                     
                        layer.close(index);
                    }
                 });
             }

             function seeDetaildjhd(code) {
                 window.location = "card_edit?id=" + code;
             }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--卡片管理标题********************-->
   
    <div class="row bg-wh">
        <h5><b>|</b>卡片列表</h5>
    </div>
    <div id="appendView" ></div>
    <div class="clearfix"></div>

    <div  class="row marg list-border text-center list_add_new" onclick="window.location.href='card_edit.aspx'" >
           <div class="col-sm-8 col-xs-8">
				 <a href="javascript:void(0)" >
        <img  src="/m/images/Add.png"  />&nbsp;&nbsp;添加新卡</a>
				    </div>
       </div>
              
    <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent">
    </div>
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
     <script id="demo" type="text/html">
     {{# for (var i = 0; i<d.length; i++){ }}
			<div   class="row marg list-border itemlist" >
                <div class="clickButton" data-code="{{ d[i].Code}}">
                    <div class="col-sm-4 col-xs-4 div_in_list">
					    <img src="/{{ d[i].Bank }}" class="img-responsive bankimg"/>
				    </div>
				    <div class="col-sm-8 col-xs-8 div_in_list">
				       <span class="col-sm-12 col-xs-12 text-left">账单日：{{ d[i].BillDate }}号&nbsp;&nbsp;&nbsp;&nbsp;还款日：{{ d[i].RepayDate }}号</span>
				        <span class="col-sm-12 col-xs-12 text-left">卡号后四位：{{ d[i].CardID }}</span>
				    </div>
                </div>
                 <div class="itemlist_delete delButton" style="display:none"  data-code="{{ d[i].Code}}">
                        <span class="itemlist_delete_btn"  >删除</span>
                 </div>
			</div>
        {{# } }}
    </script>
</asp:Content>
