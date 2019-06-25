<%@ Page Title="" Language="C#" MasterPageFile="~/pc/PCSite.Master" AutoEventWireup="true" Inherits="Web.m.app.main_mine" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .teacher-left img {
            width: 60%;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var loginUserInfo = '<%=loginUserModel==null?"":loginUserModel.MID%>';
            //layerAlert(loginUserInfo);
            if (loginUserInfo != '') {
                //跳转到个人信息页面
                //$("#img_to_click").click(function () {
                //    window.location = 'user_info.aspx'
                //});
                $("#lb_loginInfo").html("欢迎：" + loginUserInfo);
                $(".login-show").show();
                $(".no-login-show").hide();
                $("#sp_mname").html('<%=loginUserModel==null?"请登录":loginUserModel.MName%>');
                $("#sp_mid").html('<%=loginUserModel==null?"请登录":loginUserModel.MID%>');
                $("#sp_rolename").html('<%=loginUserModel==null?"请登录":loginUserModel.Role.Name%>');
                $("#sp_msh").html('<%=loginUserModel==null?"0.00":loginUserModel.MSH.ToString()%>');

            }
            else {
                $(".no-login-show").show();
                $(".login-show").hide();
                $("#div_loginOut").hide();
              
            }
        });

        function loginOut() {
            var alertInfo = '确定要退出吗？'
            layer.open({
                content: alertInfo
              , btn: ['确定', '取消']
              , yes: function (index, layero) {
                  window.location = '/loginout';
              }, btn2: function (index, layero) {
                  layer.close(index);
              }
            });
        }

        function memberUpgrade() {
            var alertInfo = '支付<%=Service.CacheService.GlobleConfig.Field1%>元/年可升级为VIP会员，可使用系统内所有功能！'
            layer.open({
                content: alertInfo
              , btn: ['立即支付', '我再逛逛']
              , yes: function (index, layero) {
                  //判断是否已交过费
                  var roleCode = '<%=loginUserModel!=null?loginUserModel.RoleCode:""%>';
                  if (roleCode == "VIP") {
                      layerMsg("您已是缴费会员，无需再次缴费！");
                      layer.close(index);
                      return false;
                  }

                  //按钮【立即支付】的回调，跳转到支付页面
                  window.location = '<%=Service.GlobleConfigService.GetWebConfig("WebSiteDomain").Value+ "/AliPay/alipay"%>';
              }, btn2: function (index, layero) {
                  layer.close(index);
              }
            });
      }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="services" style="padding-top: 0px">
        <div class="container">
            <div class="services-bottom">
                <div class="col-md-4">
                    <div class="row" style="padding-top: 10%">
                        <div class="col-md-4">
                            <img src="images/gl3.png" class="img-responsive" />
                        </div>
                        <div class="col-md-8">
                            <h4 id="sp_mname">----</h4>
                            <h4 id="sp_mid">----</h4>
                            <h4 id="sp_rolename">----</h4>
                        </div>
                        <div class="col-md-12">
                            <a href="javascript:void(0)" style="text-decoration: none">
                                <h3>
                                    <img src="images/cash.png" class="img-responsive" style="width: 10%; float: left; margin-right: 10px; margin-top: -5px;" />
                                    账户余额：<span id="sp_msh">0.00</span></h3>
                            </a>
                            <div class="contact-left" style="padding-top:10px">
                                <input type="button" onclick="turnLogin()"  value="立即登录" class="no-login-show"/>

                                <input type="button" onclick="turnTx()" value="申请提现" class="login-show"/>

                                <input type="button" onclick="loginOut()" class="login-show" value="安全退出"  style="    border: 2px solid #ec134f;    color: #ec134f;"/>

                            </div>
                        </div>
                    </div>

                    <div class=" personer text-center hidden" style="background-image: url(../images/pensoner_mine_bg.jpg); background-size: contain; cursor: pointer">
                        <img src="images/gl3.png" id="img_to_click" />
                        <h4 id="lb_loginInfo"></h4>
                    </div>
                </div>
                <div class="col-md-8 services-left">
                    <div id="myCarousel" class="carousel slide" data-ride="carousel">
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
            </div>
        </div>
    </div>
    <!--start-provide-->
    <div class="over">
        <div class="container">
            <div class="teacher-main">
                <div class="teacher-left">
                    <a href="ext_link">
                        <img src="images/gl1.png" alt="" />
                        <h3>分享推广</h3>
                    </a>
                </div>
                <div class="teacher-left">
                    <a href="ext_list">
                        <img src="images/gl2.png" alt="" />
                        <h3>业绩查询</h3>
                    </a>
                </div>
                <div class="teacher-left">
                    <a href="tx_apply">
                        <img src="images/yhxyd.png" alt="" />
                        <h3>我要提现</h3>
                    </a>
                </div>
                <div class="teacher-left">
                    <a href="ext_indetail">
                        <img src="images/wd.png" alt="" />
                        <h3>收入明细</h3>
                    </a>
                </div>
                     <div class="teacher-left">
                    <a href="http://www.d1peixun.com/">
                        <img src="images/index.png" alt="" />
                        <h3>返回首页</h3>
                    </a>
                </div>

                <div class="teacher-left">
                    <a href="javascript:layerAlert('更多功能，敬请期待')">
                        <img src="images/xsd.png" alt="" />
                        <h3>敬请期待</h3>
                    </a>
                </div>
            </div>

        </div>
        <div class="clearfix"></div>
    </div>
    <!--end-provide-->

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">


    <script type="text/javascript">
        function turnLogin() {
            window.location.href = "user_login";
        }
        function turnTx() {
            window.location.href = "tx_apply";
        }

        function applyAgent(agent) {
            if (isLogin != '1') {
                //跳转到登录页面
                window.location = 'user_login.aspx';
                return false;
            }
            var applyLeaveMoney = '<%=applyRoleMoney%>';
            var vipMoney = '<%=Service.CacheService.GlobleConfig.Field1%>';
            var ApplyAgent3Money = '<%=Service.GlobleConfigService.GetWebConfig("ApplyAgent3Money").Value%>';
            var ApplyAgent2Money = '<%=Service.GlobleConfigService.GetWebConfig("ApplyAgent2Money").Value%>';
            //if (applyLeaveMoney == "0")
            //    applyLeaveMoney = ApplyAgent3Money;
            if (isVip == '1') {
                applyLeaveMoney = ApplyAgent3Money - vipMoney;
            }
            else
                applyLeaveMoney = ApplyAgent3Money;
            var applyname = '分销商';
            if (agent == '2f') {
                applyname = '服务中心';
                //if (applyLeaveMoney == "0")
                //    applyLeaveMoney = ApplyAgent2Money;
                if (isVip == '1') {
                    applyLeaveMoney = ApplyAgent2Money - vipMoney;
                }
                else
                    applyLeaveMoney = ApplyAgent2Money;
            }

            var alertInfo = "申请成为" + applyname + "需要缴纳" + applyLeaveMoney + "元，是否继续申请？";
            if (isVip == '1') { //如果已经是VIP,，要查看需要补缴的金额
                //applyLeaveMoney = (applyAgentMoney- vipMoney).toFixed(2);
                alertInfo = "申请成为" + applyname + "需要补缴" + applyLeaveMoney + "元，是否继续申请？";
            }
            layer.open({
                content: alertInfo
            , btn: ['立即申请', '我再逛逛']
            , yes: function (index, layero) {
                layerLoading();
                var userInfo = {
                    type: 'CheckIsCanApplyAgent',
                    agentleavel: agent
                };
                var result = GetAjaxString(userInfo);
                closeLayerLoading();
                if (result == "1") {
                    layerAlert("您已是" + applyname + "，无需再次申请！");
                    layer.close(index);
                    return false;
                }
                else if (result == "2") {
                    layerAlert("对不起，您暂时不能缴费，详情咨询客服人员！");
                    layer.close(index);
                    return false;
                }
                else {
                    //按钮【立即支付】的回调，跳转到支付页面
                    window.location = '<%=Service.GlobleConfigService.GetWebConfig("WebSiteDomain").Value+ "/AliPay/alipay?type=applyagent"%>' + agent;
                }
            }, btn2: function (index, layero) {
                layer.close(index);
            }
            });
}
    </script>
</asp:Content>
