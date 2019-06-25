<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Web.index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .quickimg {
            width: 88%;
        }
        /*提醒无缝滚动字幕*/
        ul, li, dl, ol {
            list-style: none;
        }

        .bcon h3 {
            border-bottom: 1px solid #eee;
            padding: 0 10px;
        }

            .bcon h3 b {
                font: bold 12px/40px 'microsoft yahei';
                padding: 0 8px;
                margin-top: -1px;
                display: inline-block;
            }

        .list_lh {
            height: 50px;
            overflow: hidden;
        }

            .list_lh li {
                padding: 10px;
            }

                .list_lh li.lieven {
                    background: #F0F2F3;
                }

                .list_lh li p lable {
                    color: #999;
                    float: right;
                    font-size: 12px;
                }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="banner">
        <div class="slider">
            <section class="slider">
                <div class="flexslider">

                    <div class="flex-viewport" style="overflow: hidden; position: relative;">
                        <ul class="slides" style="width: 100%; transition-duration: 0.6s; transform: translate3d(-2698px, 0px, 0px);">
                            <asp:Repeater ID="repBannerList" runat="server">
                                <ItemTemplate>
                                    <li class="clone" style="float: left; display: block;" onclick="showTel()">
                                        <img class="img-responsive" src="Attachment/<%#Eval("PicUrl") %>" />
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>

                </div>
            </section>
            <!--FlexSlider-->
            <script defer="" src="js/jquery.flexslider.js"></script>
            <script type="text/javascript">
                $(function () {
                    var hasCompleteAnswer = '<%=TModel==null?1: TModel.ReadNoticeId%>';
                    var IsChangePlan = '<%=IsChangePlan%>';
                    if (hasCompleteAnswer == 0) {
                        layer.open({
                            content: '您的账号安全问题还未设置，设置安全问题之后，可以自主提现、找回账号密码等操作。是否立即设置？也可点击右上角您的账号-选择"个人信息"-"安全问题"进行设置。'
                         , btn: ['立即设置', '我再逛逛']
                         , yes: function (index, layero) {
                             //按钮【立即支付】的回调，跳转到支付页面
                             window.location = "myregistanswer";
                         }, btn2: function (index, layero) {
                             //按钮【我再逛逛】的回调,跳转到首页
                             layer.close(index);
                         }
                              , cancel: function (index) {
                                  //右上角关闭回调
                                  layer.close(index);
                              }
                        });
                    }
                    if (IsChangePlan == '1') {
                        layer.open({
                            content: '尊敬的用户，由于您有未操作的还款/消费计划，系统已自动重新规划。为减小您的还款压力，请每日登陆系统执行。'
                         , btn: ['我知道了']
                         , yes: function (index, layero) {
                             //按钮【立即支付】的回调，跳转到支付页面
                             var addInfo = {
                                 type: 'HasKnowPlanChange'
                             };
                             GetAjaxString(addInfo);
                             layer.close(index);
                         }
                              , cancel: function (index) {
                                  //右上角关闭回调
                                  layer.close(index);
                              }
                        });
                    }
                });
                $(window).load(function () {
                    $('.flexslider').flexslider({
                        animation: "slide",
                        start: function (slider) {
                            $('body').removeClass('loading');
                        }
                    });
                });
            </script>
        </div>
    </div>

    <div class="box" id="remindListBox" runat="server">
        <div class="bcon">
            <h3><b>重要提醒</b></h3>
            <!-- 代码开始 -->
            <div class="list_lh">
                <ul>
                    <asp:Repeater ID="repRemindList" runat="server">
                        <ItemTemplate>
                            <li>
                                <p>
                                    <label style="width: <%#Eval("Field1")!=null&&Eval("Field1")!=""?"80%":"100%" %>"><%#Eval("Remark") %></label>
                                    <a href="addplan?ac=<%#Eval("Field1") %>" class="btn btn-success btn-sm  pull-right <%#Eval("Field1")!=null&&Eval("Field1")!=""?"":"hidden" %>">去规划</a>
                                </p>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
            <!-- 代码结束 -->
        </div>
    </div>

    <div class="content">
        <div class="container  container-fluid">
            <div class="content-top row text-center" style="padding-bottom: 5px; padding-left: 5px">
                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3 " style="width: 25%">
                    <a href="archivelist">
                        <img class="img-responsive quickimg" src="images/a1.jpg" alt="">
                    </a>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3" style="width: 25%">
                    <a href="planlist">
                        <img class="img-responsive quickimg" src="images/a2.jpg" alt="">
                    </a>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3" style="width: 25%">
                    <a href="everydayplan">
                        <img class="img-responsive quickimg" src="images/a3.jpg" alt="">
                    </a>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3" style="width: 25%">
                    <a href="posmarket">
                        <img class="img-responsive quickimg" src="images/a4.jpg" alt="">
                    </a>
                </div>
                <div class="clearfix"></div>
            </div>

            <div class="row text-center" style="padding-bottom: 5px; padding-left: 5px">
                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3 " style="width: 25%">
                    <a href="javascript:void(0)" onclick="tranToCredit()">
                        <img class="img-responsive quickimg" src="images/a5.jpg" alt="" />
                    </a>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3" style="width: 25%">
                    <a href="mylink">
                        <img class="img-responsive quickimg" src="images/a6.jpg" alt="" />
                    </a>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3" style="width: 25%">
                    <a href="mttj">
                        <img class="img-responsive quickimg" src="images/a7.jpg" alt="">
                    </a>
                </div>
                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3" style="width: 25%">
                    <a href="javascript:void(0)" onclick="upMember()">
                        <img class="img-responsive quickimg" src="images/a12.jpg" alt="">
                    </a>
                </div>
                <div class="clearfix"></div>
            </div>


            <div class="row text-center" style="padding-bottom: 5px; padding-left: 5px">
                <%--   <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3 " style="width: 25%">
                    <a href="javascript:void(0)" onclick="showCollection()">
                        <img class="img-responsive quickimg" src="images/a10.jpg" alt="">
                    </a>
                </div>--%>
                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3" style="width: 25%">
                    <a href="learning">
                        <img class="img-responsive quickimg" src="images/a11.jpg" alt="">
                    </a>
                </div>

                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3" style="width: 25%">
                    <a href="javascript:void(0)" onclick="setAdd()">
                        <img class="img-responsive quickimg" src="images/a13.jpg" alt="">
                    </a>
                </div>
                <%if (MethodHelper.ConfigHelper.GetAppSettings("SystemID") == "kgj00")
                  { %>
                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3" style="width: 25%">
                    <a href="javascript:void(0)" onclick="tranToxxfGood()">
                        <img class="img-responsive quickimg" src="images/viptegong.jpg" alt="">
                    </a>
                </div>

                <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3" style="width: 25%">
                    <a href="xindaikouzi">
                        <img class="img-responsive quickimg" src="images/xindaikouzi.jpg" alt="">
                    </a>
                </div>
                <%} %>
                <div class="clearfix"></div>
             
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script src="js/scroll.js"></script>
    <script type="text/javascript">
        $(function () {
            $('.list_lh li:even').addClass('lieven');
            $("div.list_lh").myScroll({
                speed: 40, //数值越大，速度越慢
                rowHeight: 50 //li的高度
            });
        });
        function tranToxxfGood() {
            //查看是否是vip会员
            if (isLogin == '1') {
                //登录之后开始轮询消息
                var isvip = '<%=TModel==null?"1": TModel.RoleCode%>';
                if (isvip == 'VIP') {
                    window.location = 'xxfGood';
                }
                else {
                    upMember();
                }
            }
            else {
                //跳转到登录页面
                window.location = 'login';
            }
        }
        function tranToCredit() {
            //查看是否是vip会员
            var systemId = '<%=MethodHelper.ConfigHelper.GetAppSettings("SystemID")%>';
            if (systemId == 'kgj01') {
                if (isLogin == '1') {
                    var isvip = '<%=TModel==null?"1": TModel.RoleCode%>';
                    if (isvip == 'VIP') {
                        window.location = 'credit';
                    }
                    else {
                        upMember();
                    }
                }
                else {
                    //跳转到登录页面
                    window.location = 'login';
                }
            }
            else {
                window.location = 'credit';
            }
        }
    </script>
</asp:Content>
