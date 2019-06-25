<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="share.aspx.cs" EnableViewState="false" Inherits="Web.share" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
    <meta name="format-detection" content="telephone=no">
    <meta http-equiv="x-rim-auto-match" content="none">
    <script src="/common/js/js_sdk.js"></script>
    <style type="text/css">
        #content {
            background-image: url('images/sharebackground.jpg');
            background-repeat: repeat;
            background-size: cover;
            background-repeat-y: no-repeat;
        }

        .spName {
            padding-top: 5%;
            padding-left: 21%;
            font-size: 20px;
        }

        .spMID {
            padding-top: 7%;
            padding-left: 25%;
            font-size: 20px;
        }

        .divTel {
            bottom: 7%;
            position: fixed;
            vertical-align: bottom;
            padding-left: 29%;
            font-size: 20px;
        }

        .divImg {
            text-align: center;
            padding: 5px 0px 20px 0px;
            margin: 0 auto;
            left: 0;
            right: 0;
            margin-top: 60%;
            margin-left: 20%;
            margin-right: 20%;
            padding-bottom: 100px;
        }

        .hidden {
            display: none;
        }
    </style>
    <script type="text/javascript">
        function shareTo() {
            //share('screenshot');
            //return false;
            if (is_kingkr_obj()) {
                var shareUrl = '<%=Request.Url%>';
                var shareTitle = '<%=Service.GlobleConfigService.GetWebConfig("ShareTitle").Value%>';
                var shareRemark = '<%=Service.GlobleConfigService.GetWebConfig("ShareRemark").Value%>';
                var sharImg = '<%=Service.GlobleConfigService.GetWebConfig("WebSiteDomain").Value+Service.CacheService.GlobleConfig.Field4 %>';
                var type = sdk_judge();
                if (type.ios == true) {
                    share('screenshot');
                    //share(shareRemark, sharImg, shareUrl, shareTitle);
                }
                else {
                    share('share', shareRemark, null, shareUrl, shareTitle);
                }
            }
        }
    </script>
    <title>我的分享</title>
</head>
<body>
    <form id="form1" runat="server" onclick="shareTo()">
        <div id="content">
            <div class="spName">
                <strong id="sName" runat="server" class="hidden"></strong>
            </div>
            <div class="spMID">
                <strong id="sMID" runat="server" class="hidden"></strong>
            </div>
            <div class="divImg">
                <img id="imgQCode" src="/Handler/QRCode.ashx?mid=<%=Request.QueryString["registcode"] %>" style="width: 70%; border: 10px solid white; margin-top: 9%;" />

                <p>
                    <strong id="spTel" runat="server" style="font-weight: bolder; color: white; font-size: 18px;">推荐码：</strong>
                </p>
            </div>
        </div>
    </form>
</body>
</html>
