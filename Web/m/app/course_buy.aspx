<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/mSiteV1.Master" AutoEventWireup="true" CodeBehind="course_buy.aspx.cs" Inherits="Web.m.app.course_buy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .li-name {
            font-weight: bolder;
            color: black;
            padding: 6px;
        }

        .li-title {
            color: black;
            font-size: 10px;
            padding-bottom: 5px;
        }

        .sp-buycount {
            float: left;
            font-size: 8px;
            color: #9d9d9d;
        }

        .sp-fee {
            float: right;
            font-size: 8px;
            color: red;
        }

        .img-responsive {
            margin-left: 8px;
        }

        .li-row {
            margin-right: 3px;
            border-bottom: 1px solid #d1d1d1;
            border-top: 1px solid #d1d1d1;
            padding-top: 5px;
            padding-bottom: 5px;
            margin-top: 5px;
        }
    </style>
    <script type="text/javascript">

        function isWeiXin() {
            var ua = navigator.userAgent.toLowerCase();
            var isWeixin = ua.indexOf('micromessenger') != -1;
            if (isWeixin) {
                return true;
            } else {
                return false;
            }
        }

        $(function () {
            if (isWeiXin()) {
                $("#btn_alipay").remove();
            }
        });

        function turn_AliPay() {
            layer.open({
                content: '确定要使用支付宝支付<%=course.Fee %>元购买<%=course.Name %>课程吗？'
                          , btn: ['立即支付', '我再想想']
                          , yes: function (index, layero) {
                              //按钮【立即支付】的回调，跳转到支付页面
                              window.location = '<%=Service.GlobleConfigService.GetWebConfig("WebSiteDomain").Value + "/AliPay/alipay"%>' + '?type=<%=course.Code %>';
                          }, btn2: function (index, layero) {
                              //按钮【我再逛逛】的回调,跳转到首页
                          }
            });
                      }


        function turn_WeixinPay() {
                          layer.open({
                              content: '确定要使用微信支付<%=course.Fee %>元购买<%=course.Name %>课程吗？'
                    , btn: ['立即支付', '我再想想']
                    , yes: function (index, layero) {
                        //按钮【立即支付】的回调，跳转到支付页面
                        window.location = "/WXPay/JsApiPayPage?course=<%=course.Code %>";
                    }, btn2: function (index, layero) {
                        //按钮【我再逛逛】的回调,跳转到首页
                    }
                });
                }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row bg-wh">
        <h5><b>|</b>购买课程</h5>
    </div>
    <div class="row">
        <div id="appendView">
            <a href="course_detail?code=<%=course.Code %>" style="background: url(../images/more.png) right no-repeat; background-size: 6%; display: inline-block; color: #696969;">
                <div class="row li-row">
                    <div class="col-sm-3 col-xs-3 ">
                        <img src="<%=course.Remark %>" class="img-responsive" />
                    </div>
                    <div class="col-sm-9 col-xs-9 padding-left-right-0">
                        <div class="li-name"><%=course.Name %></div>
                        <div class="li-title"><%=course.Title %></div>
                    </div>
                </div>

            </a>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12 col-xs-12 " style="font-size: 16px; color: red; font-weight: bolder; text-align: center;">
            课程费用：<%=course.Fee %>
        </div>
        <div class="col-sm-12 col-xs-12">
            <div style="padding-top: 20px;">
                <input type="button" class="btn btn-block btn-info btn-pay" id="btn_alipay" value="支付宝支付" onclick="turn_AliPay()" />
                <br />
                <input type="button" class="btn btn-block  btn-success btn-pay" style="background-color: #59bb14; border-color: #59bb14;" value="微信支付" onclick="turn_WeixinPay()" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
</asp:Content>
