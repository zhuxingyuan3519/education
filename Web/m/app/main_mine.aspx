<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="main_mine.aspx.cs" Inherits="Web.m.app.main_mine" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .indexIcon {
            width: 60% !important;
        }

        .indexIconleft {
            width: 100%;
            margin: 5px auto;
        }


        .indexIconp {
            padding-top: 4px !important;
        }

        .p-right {
            /*padding-top: 5% !important;*/
            font-weight: bolder;
        }

        .row h4, .row h3 {
            color: white;
        }

        .magin10 {
            margin-top: 0px;
        }

        h4 {
            margin-bottom: 0px;
            font-size: 14px;
        }

        .list-content {
            padding: 6px 5px;
            background-color: #f5f5f5;
        }

            .list-content a {
                background-color: #fff;
                padding-top: 8px;
                padding-bottom: 1px;
                padding-left: 15px;
                border-radius: 10px;
            }

        .border-r a {
            padding-top: 5px;
            padding-bottom: 0px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            //setNavIndexChecked(2);

            var isShowBindWX = '<%=isShowBindWX%>';
            if (isShowBindWX == '1') {
                var alertInfo = '您还未绑定手机号，绑定手机号后可享受更多优质服务';
                layer.open({
                    content: alertInfo
                    , btn: ['立即绑定', '稍后绑定']
                    , yes: function (index, layero) {
                        //立即绑定按钮
                        window.location.href = "user_bind_wx";
                    }, btn2: function (index, layero) {
                        layer.close(index);
                    }
                });

            }




            //登录之后判断
            if (isLogin == "1") {
                var thisTeacher = '<%=loginUserModel!=null?loginUserModel.ParentTrade:""%>';
                if (thisTeacher == "") {
                    if (roleCode == "VIP" || roleCode == "Member") {
                        var teachercode = window.localStorage.getItem("teachercode");
                        if (teachercode != null && teachercode != '') {
                            var alertInfo = '您还未绑定老师，是否绑定老师账号：' + teachercode;
                            layer.open({
                                content: alertInfo
                                , btn: ['立即绑定', '稍后绑定']
                                , yes: function (index, layero) {
                                    //立即绑定按钮
                                    layerLoading();
                                    var userInfo = {
                                        type: 'bindTeacher',
                                        teacherCode: teachercode
                                    };
                                    var result = GetAjaxString(userInfo);
                                    closeLayerLoading();
                                    if (result == "1") {
                                        layerAlert("绑定成功");
                                        layer.close(index);
                                    }
                                    else if (result == "2") {
                                        layerAlert("不存在该老师");
                                        layer.close(index);
                                    }
                                    else {
                                        layerAlert("绑定失败，请重试");
                                    }

                                }, btn2: function (index, layero) {
                                    layer.close(index);
                                }
                            });
                        }
                    }
                }
            }
            //判断信息是否都已完善
            if (roleCode == "1F" || roleCode == "2F" || roleCode == "3F")
                checkIsCompleteInfo();
        });

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


        function memberUpgradeToStudent() {
            var alertInfo = '支付<%=Service.CacheService.RoleList.FirstOrDefault(c => c.Code == "Student").Remark%>元成为学员，享受更优级的服务和更多奖励！'
            layer.open({
                content: alertInfo
                , btn: ['立即支付', '我再逛逛']
                , yes: function (index, layero) {
                    //判断是否已交过费
                    var roleCodeIndex = '<%=loginUserModel!=null?loginUserModel.Role.RIndex:1%>';
                    if (parseInt(roleCodeIndex) >= 2) {
                        layerMsg("您已是学员以上级别，无需再次缴费！");
                        layer.close(index);
                        return false;
                    }

                    //按钮【立即支付】的回调，跳转到支付页面
                    window.location = '/WXPay/JsApiPayPage?applyrole=Student';
                }, btn2: function (index, layero) {
                    layer.close(index);
                }
            });
        }

        function turnPoint() {
            var roleCode = '<%=loginUserModel!=null?loginUserModel.RoleCode:""%>';
            if (roleCode == "2F" || roleCode == "1F" || roleCode == "3F") {
                window.location = "turn_point";
            }
            else {
                if (isLogin == "1") {
                    layerAlert("您没有权限");
                }
                else {
                    window.location = 'user_login';
                }
            }
        }
        //跳转到红包抽取页面
        function turnToPrize() {
            if (isLogin == "1") {
                var thisUrl = window.location.href;
                var ucode = '<%=loginUserModel!=null?loginUserModel.ID:""%>';
                window.location = 'http://jwy.u1200.com/v1/user/sign?UCode=' + ucode + '&reference=' + thisUrl;
            }
            else {
                window.location = 'user_login';
            }
        }
        function turnToPrize2() {
            if (isLogin == "1") {
                var thisUrl = window.location.href;
                var ucode = '<%=loginUserModel!=null?loginUserModel.ID:""%>';
                window.location = 'http://jwy.u1200.com/v1/user/prizeAnswer?UCode=' + ucode + '&reference=' + thisUrl;
            }
            else {
                window.location = 'user_login';
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row ">
        <div class="col-sm-12 col-xs-12 padding-left-right-0 magin10">
            <img src="../images/main_index.png" class="img-responsive" />
        </div>
    </div>

    <div class="row">
        <div class="col-sm-4 col-xs-4 text-center border-r border-none">
            <a href="ext_list" class="btn-block ">
                <img src="../images/gl1.png" class="indexIcon" /><p class="indexIconp">客户查询</p>
            </a>
        </div>

        <div class="col-sm-4 col-xs-4 text-center border-r border-none">
            <a href="role_list" class="btn-block ">
                <img src="../images/gl2.png" class="indexIcon" /><p class="indexIconp">加盟合作</p>
            </a>
        </div>

        <div class="col-sm-4 col-xs-4 text-center border-r border-none">
            <a href="javascript:turnPoint()" class="btn-block ">
                <img src="../images/gl3.png" class="indexIcon" /><p class="indexIconp">名额转让</p>
            </a>
        </div>
    </div>

    <div class="row ">
        <div class="col-sm-6 col-xs-6 text-center border-r  border-none list-content ">
            <a href="javascript:turnToPrize()" class="btn-block ">
                <div class="row">
                    <div class="col-sm-4 col-xs-4 ">
                        <img src="../images/gl6.png" class="indexIconleft" />
                    </div>
                    <div class="col-sm-8 col-xs-8  text-left ">
                        <p class="p-right">签到送红包</p>
                    </div>
                </div>
            </a>
        </div>
        <div class="col-sm-6 col-xs-6 text-center border-r  border-none list-content ">
            <a href="javascript:turnToPrize2()" class="btn-block ">
                <div class="row">
                    <div class="col-sm-4 col-xs-4 ">
                        <img src="../images/hbmx.png" class="indexIconleft" />
                    </div>
                    <div class="col-sm-8 col-xs-8  text-left ">
                        <p class="p-right">红包密令</p>
                    </div>
                </div>
            </a>
        </div>

    </div>

    <div class="row">

        <div class="col-sm-6 col-xs-6 text-center border-r  border-none list-content ">
            <a href="javascript:memberUpgradeToStudent()" class="btn-block ">
                <div class="row">
                    <div class="col-sm-4 col-xs-4 ">
                        <img src="../images/hysj.png" class="indexIconleft" />
                    </div>
                    <div class="col-sm-8 col-xs-8  text-left ">
                        <p class="p-right">会员升级</p>
                    </div>
                </div>
            </a>
        </div>



        <div class="col-sm-6 col-xs-6 text-center border-r  border-none list-content">
            <a href="words_query" class="btn-block ">
                <div class="row">
                    <div class="col-sm-4 col-xs-4 ">
                        <img src="../images/gl10.png" class="indexIconleft" />
                    </div>
                    <div class="col-sm-8 col-xs-8  text-left ">
                        <p class="p-right">速记大辞典</p>
                    </div>
                </div>
            </a>
        </div>


        <div class="col-sm-6 col-xs-6 text-center border-r  border-none list-content">
            <a href="ext_center" class="btn-block ">
                <div class="row">
                    <div class="col-sm-4 col-xs-4 ">
                        <img src="../images/gl5.png" class="indexIconleft" />
                    </div>
                    <div class="col-sm-8 col-xs-8   text-left">
                        <p class="p-right">分享推广</p>
                    </div>
                </div>
            </a>
        </div>

        <div class="col-sm-6 col-xs-6 text-center border-r  border-none list-content">

            <a href="javascript:checkExam();" class="btn-block ">
                <div class="row">
                    <div class="col-sm-4 col-xs-4 ">
                        <img src="../images/dccp.png" class="indexIconleft" />
                    </div>
                    <div class="col-sm-8 col-xs-8   text-left">
                        <p class="p-right">单词测评</p>
                    </div>
                </div>
            </a>
        </div>

        <div class="col-sm-6 col-xs-6 text-center border-r  border-none list-content">
            <a href="main_application" class="btn-block ">
                <div class="row">
                    <div class="col-sm-4 col-xs-4 ">
                        <img src="../images/grzx.png" class="indexIconleft" />
                    </div>
                    <div class="col-sm-8 col-xs-8  text-left">
                        <p class="p-right">个人中心</p>
                    </div>
                </div>
            </a>
        </div>

        <div class="col-sm-6 col-xs-6 text-center border-r  border-none list-content">
            <a href="video_center" class="btn-block ">
                <div class="row">
                    <div class="col-sm-4 col-xs-4 ">
                        <img src="../images/gl8.png" class="indexIconleft" />
                    </div>
                    <div class="col-sm-8 col-xs-8  text-left ">
                        <p class="p-right">视频播放</p>
                    </div>
                </div>
            </a>
        </div>


        <div class="col-sm-6 col-xs-6 text-center border-r  border-none list-content">
            <a href="javascript:turnToCodeLearn()" class="btn-block ">
                <div class="row">
                    <div class="col-sm-4 col-xs-4 ">
                        <img src="../images/gl9.png" class="indexIconleft" />
                    </div>
                    <div class="col-sm-8 col-xs-8  text-left ">
                        <p class="p-right">在线训练</p>
                    </div>
                </div>
            </a>
        </div>

        <%--  <div class="col-sm-6 col-xs-6 text-center border-r  border-none list-content">
            <a href="student_list" class="btn-block ">
                <div class="row">
                    <div class="col-sm-4 col-xs-4 ">
                        <img src="../images/gl9.png" class="indexIconleft" />
                    </div>
                    <div class="col-sm-8 col-xs-8  text-left ">
                        <p class="p-right">我的学员</p>
                    </div>
                </div>
            </a>
        </div>--%>
    </div>

    <%if (TModel != null && TModel.RoleCode == "Teacher")
        { %>
    <div class="row bg-wh" style="display: none">
        <h5><b>|</b>单词测评</h5>
    </div>

    <div class="row">
        <div class="marg">
            <span class="col-sm-12 col-xs-12  bg-wh" onclick="addNewExamVersion()" style="padding-top: 10px; padding-bottom: 10px; font-size: 14px; text-align: center; color: #000; font-weight: bold;">+添加试卷版本</span>
        </div>
    </div>
    <%} %>
    <div id="layerShowHtml" style="display: none">
        <div>
            <div style="font-size: 18px; font-family: serif; font-weight: bolder; padding: 20px 0px 10px 20px; text-align: center; border-bottom: 1px solid #ccc;">
                提&emsp;示
            </div>
            <div style="padding: 30px 0px 30px 0px; text-align: center; font-weight: bolder" id="div_signUpTip">
                签到赚积分！兑换大礼包！
            </div>
            <div style="text-align: center; font-size: 18px; padding: 15px; border-radius: 15px; outline: 0;">
                <input type="button" value="立即签到" id="btn_signup" class="btn btn-block btn-success" style="width: 90%" onclick="signUp()" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12 col-xs-12 text-center " style="padding-top: 20px">
            Copyright © 2018.<%=Service.CacheService.GlobleConfig.Contacter %>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
    <script type="text/javascript">

        function turnToCodeLearn() {
            var roleCode = '<%=loginUserModel!=null?loginUserModel.RoleCode:""%>';
            if (roleCode == "Member") {
                layerAlert("对不起，您没有权限，详情请咨询客服");
            }
            else {
                window.location = "code_learn";
            }
        }

        function checkIsCompleteInfo() {
            if (isLogin == "1") {
                var isComplete = '<%=(TModel != null ?TModel.Mather:"") %>';
                if (isComplete != '1') {
                    var alertInfo = '请先完善个人信息';
                    layer.open({
                        content: alertInfo
                        , btn: ['立即完善', '稍后完善']
                        , yes: function (index, layero) {
                            //立即绑定按钮
                            window.location = 'user_addinfo';
                        }, btn2: function (index, layero) {
                            layer.close(index);
                        }
                    });
                }
            }
        }


        function checkExam() {
            if (isLogin == "1") {
                var isComplete = '<%=(TModel != null ?TModel.Mather:"") %>';
                if (isComplete != '1') {
                    var alertInfo = '您需完善信息后才能进行测评，是否立即完善？';
                    layer.open({
                        content: alertInfo
                        , btn: ['立即完善', '稍后完善']
                        , yes: function (index, layero) {
                            //立即绑定按钮
                            window.location = 'user_addinfo';
                        }, btn2: function (index, layero) {
                            layer.close(index);
                        }
                    });
                }
                else {
                    window.location = 'exam_choice';
                }
            }
            else {
                window.location = 'user_login';
            }
        }


        function addNewExamVersion() {
            window.location = "exam_version";
        }

        function signUp() {
            layerLoading();
            var userInfo = {
                type: 'signup'
            };
            var result = GetAjaxString(userInfo);
            closeLayerLoading();
            if (result == "-1")
                layerAlert("今天您已签到过！");
            else if (result == "-2")
                layerAlert("签到失败，请重试！");
            else {
                issing = "1";
                layer.close(layer.index);
                layerAlert("签到成功，恭喜您，获得积分<span style='color:orange'>+" + result + "</span>");
                //$("#div_signUpTip").html("恭喜您，获得积分<span style='color:orange'>+" + result + "</span>");
                //console.log($("#div_signUpTip").html());
                //$("#btn_signup").val("签到成功").css("background-color", "orange").removeAttr("onclick");
            }
        }
        //签到
        var issing = '<%=isSignUp%>';
        function showSignIn() {

            var loginUserInfo = '<%=loginUserModel==null?"":loginUserModel.MID%>';
            //layerAlert(loginUserInfo);
            if (loginUserInfo == '') {
                window.location = 'user_login.aspx'
            }

            if (issing == "1") {
                layerAlert("您今天已签到过");
                return;
            }
            layer.open({
                type: 1
                , content: $("#layerShowHtml").html()
                , anim: 'up'
                , style: 'margin: 0 auto;top:20%; width:85%; border-radius:15px;outline:0; border: none; -webkit-animation-duration: .5s; animation-duration: .5s;'
            });
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
