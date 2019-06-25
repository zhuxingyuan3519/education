<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="plan_list.aspx.cs" Inherits="Web.m.app.plan_list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <style type="text/css">
           .xxheight {
               line-height: 94px;
               height: 94px;
           }
       </style>
    <script src="/common/js/jquery.touchSwipe.min.js"></script>
    <script src="/common/laypage/laypage.js"></script>
    <script src="/common/laytpl/laytpl.js"></script>
         <script type="text/javascript">
             $(function () {
                 getPageList(1);
             });

             function addSwipe() {
                 var toLeft = '90px'; //向左移动的距离'
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
                         deletePlan(thisObj, code);
                     },
                 });
             }
             function getPageList(curr) {
                 var pagesi = 10;
                 //分页参数，需要传到后台的
                 var param = { cid: '', pageIndex: curr, pageSize: pagesi };
                 //ajax获取
                 $.getJSON("/Handler/GetPlanPageList.ashx?rand=" + Math.random(), param, function (res) {
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
             };
             function seeDetaildjhd(code) {
                 window.location.href = "plan_edit?id=" + code;
             }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <!--卡片规划标题********************-->
    <div class="row bg-wh">
        <h5><b>|</b>规划列表</h5>
    </div>
    <div id="appendView"></div>
    
    <div class="clearfix"></div>
    <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent">
    </div>

    <%--添加新规划--%>
     <div  class="row marg list-border text-center list_add_new"   data-toggle="modal" data-target="#myModal"  >
           <div class="col-sm-8 col-xs-8" >
				 <a href="javascript:void(0)"  >
        <img  src="/m/images/Add.png"  />&nbsp;&nbsp;添加新规划</a>
				    </div>
       </div>


    <%--模态框--%>
      <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
		<div class="modal-dialog modal-content">
<div class="modal-body">
     <div class="row list-card" style="padding-bottom:20px">
            <div class="marg addmarg">
                <span class="col-sm-4 col-xs-4  ">选择银行</span>
                <span class="col-sm-8 col-xs-8 ">
                    <select id="ddlArchiveBank" runat="server"></select>
                </span>
            </div>
         </div>

     <div class="modal-footer">
				<button type="button" class="btn btn-foot btn-default" data-dismiss="modal">取消
				</button>
				<button type="button" class="btn btn-foot btn-success" data-dismiss="modal" onclick="addFenqi()"  >
					去规划
				</button>
			</div>
  </div>
</div>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
        <script type="text/javascript">
            function addFenqi() {
                var archCode = $("#ddlArchiveBank").val();
                if (archCode == '') {
                    layerMsg("请选择需要规划的信用卡信息");
                    return false;
                }
                window.location = "plan_edit?ac=" + archCode;

            }
            function deletePlan(obj, co) {
                layer.open({
                    content: '确定要删除吗？'
                    , btn: ['确定', '取消']
                    , yes: function (index, layero) {
                        var info = {
                            type: 'deletePlan',
                            code: co
                        };
                        var result = GetAjaxString(info);
                        if (result == "0") {
                            layerMsg("删除成功");
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
    </script>
     <script id="demo" type="text/html">
     {{# for (var i = 0; i<d.length; i++){ }}

         	<div   class="row marg list-border itemlist" >
                <div class="clickButton" data-code="{{ d[i].Code}}">
                    <div class="col-sm-4 col-xs-4 div_in_list">
					    <img src="/{{ d[i].Bank }}"/>
				    </div>
				    <div class="colsm-8 col-xs-8 div_in_list">
				        <span class="col-sm-12 col-xs-12 text-left">卡号后四位：{{ d[i].CardID }}</span>
				       <span class="col-sm-12 col-xs-12 text-left">规划日期：{{ d[i].PlanBeginDateString}}至{{ d[i].PlanEndDateString }}</span>
				    </div>
                </div>
                 <div class="itemlist_delete delButton" style="display:none"  data-code="{{ d[i].Code}}">
                        <span class="itemlist_delete_btn"  >删除</span>
                 </div>
			</div>

        {{# } }}
    </script>
</asp:Content>
