<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="main_index.aspx.cs" Inherits="Web.m.app.main_index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .indexIcon {
            width: 40% !important;
        }

        .indexIconp {
            padding-top: 4px !important;
        }

        .list_lh {
            height: 50px;
            overflow: hidden;
            padding-left: 0px;
            padding-right: 0px;
        }

            .list_lh li {
                padding-left: 5px;
                padding-right: 5px;
                list-style-type: none;
            }

                .list_lh li p {
                    border-bottom: 1px solid #d1d1d1;
                    padding-left: 10px;
                    padding-right: 5px;
                }

                    .list_lh li p a {
                        padding: 3px 8px;
                    }

                .list_lh li.lieven {
                    background: #F0F2F3;
                }

                .list_lh li p lable {
                    color: #999;
                    float: right;
                    font-size: 12px !important;
                }

        .border-r {
            height: 80px;
            display: block;
            border-right: 1px solid #d1d1d1;
            background-color: white;
            border-bottom: 1px solid #d1d1d1;
            border-top: 1px solid #d1d1d1;
        }

        .bg-wh {
            background-color: #ffffff;
            padding-left: 4px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            setNavIndexChecked("1");

            var isGuide1HasReaded = $.cookie(cookieKey + '_1');//卡片录入
            var isGuide4HasReaded = $.cookie(cookieKey + '_4');//卡片规划
            var isGuide5HasReaded = $.cookie(cookieKey + '_5');//每日计划
            if (isGuide1HasReaded == '1') {
                if (isGuide4HasReaded == '1') {
                    if (isGuide5HasReaded != '1')
                        setGuides('5', 'plan_everyday.aspx');
                }
                else {
                    setGuides('4', 'plan_list.aspx');
                }
            }
            else {
                setGuides('1', 'card_list.aspx');
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--轮播图***********************-->
    <div class="row">
        <div id="myCarousel" class="carousel slide" data-ride="carousel">
            <!-- 轮播（Carousel）指标 -->
            <%--	<ol class="carousel-indicators">
                        <%for(int i=1;i<=BannerListCount;i++){ %>
						<li data-target="#myCarousel" data-slide-to="<%=(i-1) %>" class="<%=(i==1)?"active":"" %>"></li>
                        <%} %>
					</ol>--%>
            <!-- 轮播（Carousel）项目 -->
            <div class="carousel-inner">
                <asp:Repeater ID="repBannerList" runat="server">
                    <ItemTemplate>
                        <div class="item <%#Container.ItemIndex==0?"active":"" %>">
                            <img src="/Attachment/<%#Eval("PicUrl") %>" class="img-responsive" width="100%" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

        </div>
    </div>
    <div id="remindListBox" runat="server">
        <div class="row bg-wh">
            <h5><b>|</b>重要提醒</h5>
        </div>
        <div class="row border-bot" style="height: initial">
            <div class="col-sm-12 col-xs-12 list_lh">
                <ul>
                    <asp:Repeater ID="repRemindList" runat="server">
                        <ItemTemplate>
                            <li>
                                <p>
                                    <label style="font-size: 12px; width: <%#Eval("Field1")!=null&&Eval("Field1")!=""?"80%":"100%" %>"><%#Eval("Remark") %></label>
                                    <a href="plan_edit?ac=<%#Eval("Field1") %>" class="btn btn-success btn-sm  pull-right <%#Eval("Field1")!=null&&Eval("Field1")!=""?"":"hidden" %>">去规划</a>
                                </p>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>

    </div>

    <div class="row">
        <!--卡片管理标题********************-->
        <div class="col-sm-12 col-xs-12 bg-wh">
            <h5><b>|</b>信用卡管理</h5>
        </div>
        <!--信用卡管理********************-->
        <div class="col-sm-4 col-xs-4 text-center border-r">
            <a href="card_list.aspx" class="btn-block ">
                <img src="../images/gl0_1.png" class="indexIcon" /><p class="indexIconp">卡片录入</p>
            </a>
        </div>
        <div class="col-sm-4 col-xs-4 text-center border-r">
            <a href="plan_list.aspx" class="btn-block ">
                <img src="../images/gl0_2.png" class="indexIcon" /><p class="indexIconp">卡片规划</p>
            </a>
        </div>
        <div class="col-sm-4 col-xs-4 text-center border-r">
            <a href="plan_everyday.aspx" class="btn-block ">
                <img src="../images/gl0_3.png" class="indexIcon" /><p class="indexIconp">每日计划</p>
            </a>
        </div>
    </div>

    <%if (Service.GlobleConfigService.GetWebConfig("IsShowCreditCenter").Value == "1")//显示信用卡中心
      {
    %>
    <div class="row">
        <!--信用卡中心菜单***********************-->
        <div class="col-sm-12 col-xs-12 bg-wh marg">
            <h5><b>|</b>信用卡中心</h5>
        </div>
        <!--信用卡中心菜单********************-->
        <%if (Service.GlobleConfigService.GetWebConfig("IsShowwoyaobanka").Value == "1")//
          {
        %>
        <div class="col-sm-4 col-xs-4 text-center border-r">
            <a href="javascript:void(0)" onclick="woyaobanka()" class="btn-block ">
                <img src="../images/gl0_4.png" class="indexIcon" /><p class="indexIconp">我要办卡</p>
            </a>
        </div>
        <%
          }%>

        <%if (Service.GlobleConfigService.GetWebConfig("IsShowwoyaodaikuan").Value == "1")//
          {
        %>
        <div class="col-sm-4 col-xs-4 text-center border-r">
            <a href="javascript:void(0)" onclick="woyaodaikuan()" class="btn-block ">
                <img src="../images/gl0_5.png" class="indexIcon" /><p class="indexIconp">我要贷款</p>
            </a>
        </div>
        <%
          }%>
        <%if (Service.GlobleConfigService.GetWebConfig("IsShowxuexijiqiao").Value == "1")//
          {
        %>
        <div class="col-sm-4 col-xs-4 text-center border-r">
            <a href="javascript:void(0)" onclick="xuexijiqiaoClick()" class="btn-block ">
                <img src="../images/gl0_6.png" class="indexIcon" /><p class="indexIconp">学习技巧</p>
            </a>
        </div>
        <%
          }%>
        <%if (Service.GlobleConfigService.GetWebConfig("IsShowwoyaotie").Value == "1")//
          {
        %>
        <div class="col-sm-4 col-xs-4 text-center border-r">
            <a href="javascript:windowopenloan('我要提额','http://xlf.mxsj888.com/a02.asp?mkid=3','IsShowwoyaotieOnlyVIP');" class="btn-block ">
                <img src="../images/gl0_9.png" class="indexIcon" /><p class="indexIconp">我要提额</p>
            </a>
        </div>
        <%
          }%>
        <%if (Service.GlobleConfigService.GetWebConfig("IsShowbankajindu").Value == "1")//
          {  %>
        <div class="col-sm-4 col-xs-4 text-center border-r">
            <a href="javascript:windowopenloan('办卡进度','http://xlf.mxsj888.com/a03.asp?mkid=4','IsShowbankajinduOnlyVIP');" class="btn-block ">
                <img src="../images/gl0_10.png" class="indexIcon" /><p class="indexIconp">办卡进度</p>
            </a>
        </div>
        <%}%>
        <%if (Service.GlobleConfigService.GetWebConfig("IsShowyinhangdianhua").Value == "1")//
          {
        %>
        <div class="col-sm-4 col-xs-4 text-center border-r">
            <a href="javascript:windowopenloan('银行电话','http://xlf.mxsj888.com/tel.asp','IsShowyinhangdianhuaOnlyVIP');" class="btn-block ">
                <img src="../images/gl0_11.png" class="indexIcon" /><p class="indexIconp">银行电话</p>
            </a>
        </div>
        <%
          }%>
    </div>
    <%
      }%>




    <%if (Service.GlobleConfigService.GetWebConfig("IsShowZuixinKouzi").Value == "1")//显示最新口子
      {
    %>
    <div class="row">
        <!--最新口子***********************-->
        <div class="col-sm-12 col-xs-12 bg-wh marg">
            <h5><b>|</b>最新口子</h5>
        </div>
        <%if (Service.GlobleConfigService.GetWebConfig("IsShowKuaikakouzi").Value == "1")//
          {
        %>
        <div class="col-sm-4 col-xs-4 text-center border-r">
            <a href="javascript:windowopenkouzi('快卡口子','guide_list.aspx?nid=7','IsShowKuaikakouziOnlyVIP');" class="btn-block ">
                <img src="../images/kkkz1.png" class="indexIcon" /><p class="indexIconp">快卡口子</p>
            </a>
        </div>
        <%}%>
        <%if (Service.GlobleConfigService.GetWebConfig("IsShowtiekouzi").Value == "1")//
          {
        %>
        <div class="col-sm-4 col-xs-4 text-center border-r">
            <a href="javascript:windowopenkouzi('提额口子','guide_list.aspx?nid=8','IsShowtiekouziOnlyVIP');" class="btn-block ">
                <img src="../images/tekz1.png" class="indexIcon" /><p class="indexIconp">提额口子</p>
            </a>
        </div>
        <%}%>
        <%if (Service.GlobleConfigService.GetWebConfig("IsShowdaikuankouzi").Value == "1")//
          {
        %>
        <div class="col-sm-4 col-xs-4 text-center border-r">
            <a href="javascript:windowopenkouzi('贷款口子','guide_list.aspx?nid=9','IsShowdaikuankouziOnlyVIP');" class="btn-block ">
                <img src="../images/dkkz1.png" class="indexIcon" /><p class="indexIconp">贷款口子</p>
            </a>
        </div>
        <%}%>
    </div>
    <%
      }%>


    <%if (Service.GlobleConfigService.GetWebConfig("IsShowjinrongchaoshi").Value == "1")//显示金融超市
      { 
    %>
    <div class="row">
        <!--金融超市***********************-->
        <div class="col-sm-12 col-xs-12 bg-wh marg">
            <h5><b>|</b>金融超市</h5>
        </div>
        <!--金融超市菜单********************-->
        <%if (Service.GlobleConfigService.GetWebConfig("IsShowposchaoshi").Value == "1")//
          { 
        %>
        <div class="col-sm-6 col-xs-6 text-center border-r">
            <a href="javascript:void(0)" onclick="poschaoshiClick()" class="btn-block ">
                <img src="../images/gl0_7.png" class="indexIcon" style="width: 25% !important;" /><p class="indexIconp">POS超市</p>
            </a>
        </div>
        <%
          }%>
        <%if (Service.GlobleConfigService.GetWebConfig("IsShowxindaichaoshi").Value == "1")//
          { 
        %>
        <div class="col-sm-6 col-xs-6 text-center border-r">
            <a href="javascript:void(0)" onclick="xindaicaoshiClick()" class="btn-block ">
                <img src="../images/gl0_8.png" class="indexIcon" style="width: 25% !important;" /><p class="indexIconp">信贷超市</p>
            </a>
        </div>
        <%
          }%>
    </div>
    <%
      }%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script src="/js/scroll.js"></script>
    <script type="text/javascript">
        $(function () {
            //$('.list_lh li:even').addClass('lieven');
            $("div.list_lh").myScroll({
                speed: 40, //数值越大，速度越慢
                rowHeight: 50 //li的高度
            });
        });
        //我要办卡
        var isnav = '<%=Service.GlobleConfigService.GetWebConfig("IsShowMainNav").Value%>';

        function woyaobanka() {
            var isOnlyVIP = '<%=Service.GlobleConfigService.GetWebConfig("IsShowwoyaobankaOnlyVIP").Value%>';
            if (isOnlyVIP == "1" && isLogin == '0') {
                //跳转到登录页面
                window.location = 'user_login.aspx';
                return false;
            }
            if (isOnlyVIP == "1" && isLogin == '1' && isVip == '0') {
                //跳转到登录页面
                upMember();
                return false;
            }

            if (systemId == 'kgj00') { //洛胜卡管家跳转到新的办卡页面
                window.location = 'credit_apply.aspx';
            }
            else {
                window.location = "credit_banklist.aspx?code=1";
            }
        }
        // 我要贷款
        function woyaodaikuan() {
            var isOnlyVIP = '<%=Service.GlobleConfigService.GetWebConfig("IsShowwoyaodaikuanOnlyVIP").Value%>';
            if (isOnlyVIP == "1" && isLogin == '0') {
                //跳转到登录页面
                window.location = 'user_login.aspx';
                return false;
            }
            if (isOnlyVIP == "1" && isLogin == '1' && isVip == '0') {
                //跳转到登录页面
                upMember();
                return false;
            }
            if (systemId == 'kgj00') { //洛胜卡管家跳转到新的
                window.location = 'credit_loanapply.aspx';
            }
            else {
                window.location = "credit_loan.aspx";
            }
        }
 
        function windowopenloan(webtitle, NContent, privageCode) {
            var isOnlyVIP = '0';
            if (privageCode == 'IsShowwoyaotieOnlyVIP')
                isOnlyVIP = '<%=Service.GlobleConfigService.GetWebConfig("IsShowwoyaotieOnlyVIP").Value%>';
            else if (privageCode == 'IsShowbankajinduOnlyVIP')
                isOnlyVIP = '<%=Service.GlobleConfigService.GetWebConfig("IsShowbankajinduOnlyVIP").Value%>';
            else if (privageCode == 'IsShowyinhangdianhuaOnlyVIP')
                isOnlyVIP = '<%=Service.GlobleConfigService.GetWebConfig("IsShowyinhangdianhuaOnlyVIP").Value%>';

            if (isOnlyVIP == "1" && isLogin == '0') {
                //跳转到登录页面
                window.location = 'user_login.aspx';
                return false;
            }
            if (isOnlyVIP == "1" && isLogin == '1' && isVip == '0') {
                //跳转到登录页面
                upMember();
                return false;
            }
            //if (isnav == "1")
            //    window.location.href = "out_link.aspx?returnTitle=" + webtitle + "&returnUrl=" + NContent;
            //else
            window.location.href = NContent;
      }
//最新口子的点击链接地址
function windowopenkouzi(webtitle, NContent, privageCode) {
    var isOnlyVIP = '0';
    var canSeeAfterRegist = 12 * 60;
    if (privageCode == 'IsShowKuaikakouziOnlyVIP') {
        isOnlyVIP = '<%=Service.GlobleConfigService.GetWebConfig("IsShowKuaikakouziOnlyVIP").Value%>';
        canSeeAfterRegist = '<%=Service.GlobleConfigService.GetWebConfig("MemberCanSeeKuaikakouziTime").Value%>';
    }
    else if (privageCode == 'IsShowtiekouziOnlyVIP') {
        isOnlyVIP = '<%=Service.GlobleConfigService.GetWebConfig("IsShowtiekouziOnlyVIP").Value%>';
        canSeeAfterRegist = '<%=Service.GlobleConfigService.GetWebConfig("MemberCanSeetiekouziTime").Value%>';
    }
    else if (privageCode == 'IsShowdaikuankouziOnlyVIP') {
        isOnlyVIP = '<%=Service.GlobleConfigService.GetWebConfig("IsShowdaikuankouziOnlyVIP").Value%>';
        canSeeAfterRegist = '<%=Service.GlobleConfigService.GetWebConfig("MemberCanSeedaikuankouziTime").Value%>';
    }

    if (isOnlyVIP == "1" && isLogin == '0') {
        //跳转到登录页面
        window.location = 'user_login.aspx';
        return false;
    }
    if (isOnlyVIP == "1" && isLogin == '1' && isVip == '0') {
        //跳转到登录页面
        //不是VIP，再看不是今天注册的，只有一天的体验期，按照00:00-24:00为限，当天注册的会员，到夜里24点就关闭，需要提示升级
        var mcreateDate = '<%=TModel!=null? TModel.MCreateDate.ToString("yyyy-MM-dd HH:mm:ss"):""%>';
        //注册之后多久可以查看(以分钟为单位)
         
        //mcreateDate='2017/05/27';
        var d2 = new Date(Date.parse(CurentTime().replace(/\-/g, "\/")));//取今天的日期
        var d1 = new Date(Date.parse(mcreateDate.replace(/\-/g, "\/")));
        var m = parseInt(Math.abs(d2 - d1) / 1000 / 60);//得到以分钟为单位的差值
        if (parseInt(canSeeAfterRegist) < m) {
            upMember();
            return false;
        }
        //if (d2 > d1) {
        //    upMember();
        //    return false;
        //}
    }
    window.location.href = NContent;
}
//学习技巧点击事件
function xuexijiqiaoClick() {
    var isOnlyVIP = '<%=Service.GlobleConfigService.GetWebConfig("IsShowxuexijiqiaoOnlyVIP").Value%>';
    if (isOnlyVIP == "1" && isLogin == '0') {
        //跳转到登录页面
        window.location = 'user_login.aspx';
        return false;
    }
    if (isOnlyVIP == "1" && isLogin == '1' && isVip == '0') {
        //跳转到登录页面
        upMember();
        return false;
    }
    window.location = "credit_banklist?code=3";
}

function xindaicaoshiClick() {
    var isOnlyVIP = '<%=Service.GlobleConfigService.GetWebConfig("IsShowxindaichaoshiOnlyVIP").Value%>';
    if (isOnlyVIP == "1" && isLogin == '0') {
        //跳转到登录页面
        window.location = 'user_login.aspx';
        return false;
    }
    if (isOnlyVIP == "1" && isLogin == '1' && isVip == '0') {
        //跳转到登录页面
        upMember();
        return false;
    }
    window.location = "finance_creditmarket.aspx";
}
function poschaoshiClick() {
    var isOnlyVIP = '<%=Service.GlobleConfigService.GetWebConfig("IsShowposchaoshiOnlyVIP").Value%>';
    if (isOnlyVIP == "1" && isLogin == '0') {
        //跳转到登录页面
        window.location = 'user_login.aspx';
        return false;
    }
    if (isOnlyVIP == "1" && isLogin == '1' && isVip == '0') {
        //跳转到登录页面
        upMember();
        return false;
    }
    window.location = "finance_posmarket.aspx";
}

    </script>
</asp:Content>
