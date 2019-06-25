<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="rank_detail.aspx.cs" Inherits="Web.m.app.rank_detail" %>
<%--我的推荐列表--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .marg {
            margin-top: 0px;
        }
    </style>
    <script src="/common/laypage/laypage.js"></script>
    <script src="/common/laytpl/laytpl.js"></script>
         <script type="text/javascript">
             function setBangFuShow(obj,divId) {
                 if ($("#" + divId).hasClass("hidden")) {
                     $(obj).find("img").attr("src", "../images/bottom_arrow.png");
                     $("#" + divId).removeClass("hidden");
                 }
                 else {
                     $("#" + divId).addClass("hidden");
                     $(obj).find("img").attr("src", "../images/up_arrow.png");
                 }
             }

             $(function () {
                 getPageList(1);
             });
             function getPageList(curr) {
                 var pagesi = 10;
                 //分页参数，需要传到后台的
                 var param = { type: 'rank', pageIndex: curr, pageSize: pagesi };
                 //ajax获取
                 $.getJSON("/Handler/GetTDPageList.ashx?rand=" + Math.random(), param, function (res) {
                     var gettpl = document.getElementById('demo').innerHTML;
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="row bg-wh list-border">
        <h5><b>|</b>推荐人</h5>
    </div>
                <div   class="row marg list-border itemlist" >
                <div class="clickButton">
				    <div class="col-sm-12 col-xs-12 div_in_list">
				       <span class="col-sm-6 col-xs-6 text-left">姓名：<%=MTJName %></span>
                        <span class="col-sm-6 col-xs-6 text-left">电话：<%=MTJTel %></span>
				    </div>

                </div>
			</div>

    <div class="row bg-wh list-border">
        <h5><b>|</b>我的导师</h5>
    </div>
        <asp:Repeater ID="rep_my_teacher" runat="server">
            <ItemTemplate>
                <div   class="row marg list-border itemlist" >
                <div class="clickButton">
				    <div class="col-sm-12 col-xs-12 div_in_list">
				       <span class="col-sm-6 col-xs-6 text-left">姓名：<%#Eval("Name") %></span>
                        <span class="col-sm-6 col-xs-6 text-left">电话：<%#Eval("MID") %></span>
				    </div>

                </div>
			</div>
            </ItemTemplate>
        </asp:Repeater>

    <div class="row bg-wh list-border">
        <h5 onclick="setBangFuShow(this,'banfu_div')"><b>|</b>帮扶学员<img src="../images/up_arrow.png" class="img-responsive" style="width:25px;display:inline;float:right;margin-right: 20px;margin-top: 5px;" /></h5> 
    </div>
       <div class="hidden" id="banfu_div">
        <asp:Repeater ID="rep_td_member" runat="server">
            <ItemTemplate>
                <div   class="row marg list-border itemlist" >
                <div class="clickButton">
				    <div class="col-sm-12 col-xs-12 div_in_list">
				       <span class="col-sm-6 col-xs-6 text-left">姓名：<%#Eval("Name") %></span>
                        <span class="col-sm-6 col-xs-6 text-left">电话：<%#Eval("MID") %></span>
				    </div>
                </div>
			</div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

     <div class="row bg-wh list-border <%=TModel==null?"":(TModel.Learns=="service"?"":"hidden") %>">
        <h5 onclick="setBangFuShow(this,'tuandui_div')"><b>|</b>团队学员  <img src="../images/up_arrow.png" class="img-responsive" style="width:25px;display:inline;float:right;margin-right: 20px;margin-top: 5px;" /></h5> 
    </div>

       <div class="hidden" id="tuandui_div">
        <div id="appendView" ></div>
        <div class="clearfix"></div>
        <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent">
        </div>
   </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
        <script id="demo" type="text/html">
     {{# for (var i = 0; i<d.length; i++){ }}
            <div   class="row marg list-border itemlist" >
                <div class="clickButton">
				    <div class="col-sm-12 col-xs-12 div_in_list">
				       <span class="col-sm-6 col-xs-6 text-left">{{ d[i].RowNumber }}&nbsp;&nbsp;姓名：{{ d[i].NAME }}</span>
                        <span class="col-sm-6 col-xs-6 text-left">电话：{{ d[i].MID }}</span>
				    </div>
                </div>
			</div>
        {{# } }}
    </script>
</asp:Content>
