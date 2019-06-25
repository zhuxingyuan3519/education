<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MessageDetail.aspx.cs" Inherits="Web.Admin.Message.MessageDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title></title>
    <link href="/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <!-- jQuery -->
    <script src="/js/jquery.min.js"></script>
    <script src="/js/layer-v2.2/layer/layer.js"></script>
    <script src="/js/Verification.js"></script>
    <script src="/js/common.js"></script>

    <!-- Modernizr -->
    <script src="slider/modernizr.js"></script>
    <!-- HTML 5 shim for IE backwards compatibility -->
    <!-- [if lt IE 9]>
    <script src="js/html5.js">
    </script>
    <![endif]-->
    <style type="text/css">
        .row {
            margin-left: 0px;
            margin-right: 0px;
            margin-top:10px;
        }

        .divMContent {
            border: 2px solid #e1e1e1;
            border-radius: 2%;
            padding-left: 10px;
            width: 80%;
            margin-left: 20px;
            margin-top: 5px;
            color: black;
        }

        .sendName {
            color: black;
            font-size: 14px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid" style="padding: 10px;">
            <div class="row">
                <div class="col-md-12">
                    <div class="sendName" style="" id="divSender" runat="server"></div>
                    <div id="divMContent" class="divMContent" runat="server"></div>
                </div>
            </div>
            <asp:Repeater ID="rep_responseMsgList" runat="server">
                <ItemTemplate>
                    <div class="row">
                        <div class="col-md-12" style="">
                            <div class="sendName" style=""><%#Eval("SendName") %>：<%#Eval("CreatedTime") %></div>
                            <div class="divMContent" style=""><%#Eval("Message") %></div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <div class="row">
                <div style="color: black; font-size: 14px; border-top: 2px solid #e1e1e1; margin-top: 10px">回复消息：</div>
                <textarea id="myEditor" style="width: 100%; height: 100px"></textarea>
            </div>
        </div>
    </form>
    <link href="/umeditor/themes/default/css/umeditor.css" type="text/css" rel="stylesheet">
    <script src="/umeditor/umeditor.config.js"></script>
    <script src="/umeditor/umeditor.min.js"></script>
    <script type="text/javascript" src="/umeditor/lang/zh-cn/zh-cn.js"></script>
    <script type="text/javascript">
        var um;
        $(function () {
            um = UM.getEditor('myEditor');
        });
        function updatenongjiinfo() {
            return um.getContent();
        }

    </script>
</body>
</html>

