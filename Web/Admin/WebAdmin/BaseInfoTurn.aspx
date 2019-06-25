<%@ Page Language="C#" AutoEventWireup="true"  %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript">
        function setUrlCookie() {
            document.cookie = 'lasturl=WebAdmin/BaseInfoTurn.aspx'; document.cookie = 'lasturlname=网站基本设置';
        }
    </script>
</head>
<body>
   <div id="mempay">
        <div id="finance" class="container">
            <form id="form1">
                
        <div class="row">
            <div class="col-sm-12" style="padding:10px">
              <input type="button" class="btn btn-success" value="常见问题" onclick="setUrlCookie(); callhtml('WebAdmin/WebConfig.aspx?id=commonquestion', '常见问题');" />&emsp;
                <input type="button" class="btn btn-success" value="H5电子广告"   onclick="setUrlCookie(); callhtml('WebAdmin/WebConfig.aspx?id=h5dzhb', 'H5电子广告');" />&emsp;
                <input type="button" class="btn btn-success" value="海报"   onclick="setUrlCookie(); callhtml('WebAdmin/WebConfig.aspx?id=hb', '海报');" />&emsp;
                <input type="button" class="btn btn-success" value="宣传单"   onclick="setUrlCookie(); callhtml('WebAdmin/WebConfig.aspx?id=xcd', '宣传单');" />&emsp;
                <input type="button" class="btn btn-success" value="视频"   onclick="setUrlCookie(); callhtml('WebAdmin/WebConfig.aspx?id=sp', '视频');" />&emsp;
               <input type="button" class="btn btn-success hidden" value="关于我们" onclick="setUrlCookie(); callhtml('WebAdmin/WebConfig.aspx?id=aboutus', '关于我们');" />&emsp;
                <input type="button" class="btn btn-success" value="服务协议"  onclick="setUrlCookie(); callhtml('WebAdmin/WebConfig.aspx?id=serviceprotrol', '服务协议');"/>
                <input type="button" class="btn btn-success" value="奖励政策"  onclick="setUrlCookie(); callhtml('WebAdmin/WebConfig.aspx?id=incentive', '奖励政策');"/>
            </div>
             <%--      
               <div class="col-sm-12" style="padding:10px">
                <input type="button" class="btn btn-success" value="常见问题" onclick="setUrlCookie(); callhtml('WebAdmin/WebConfig.aspx?id=commonquestion', '常见问题');" />&emsp;
                <input type="button" class="btn btn-danger" value="版本信息"  onclick="setUrlCookie(); callhtml('WebAdmin/WebConfig.aspx?id=version', '版本信息');"/>
            </div>

                <div class="col-sm-12" style="padding:10px">
                <input type="button" class="btn btn-success" value="首页轮播广告" onclick="setUrlCookie(); callhtml('WebAdmin/POSBankInfo?code=2', '首页轮播广告');" />&emsp;
                <input type="button" class="btn btn-danger" value="信用卡中心轮播广告"  onclick="setUrlCookie(); callhtml('WebAdmin/POSBankInfo?code=3', '信用卡中心轮播广告');"/>
            </div>

              <div class="col-sm-12" style="padding:10px">
                <input type="button" class="btn btn-success" value="学习资料" onclick="setUrlCookie(); callhtml('Message/CardNotice.aspx?type=0', '学习资料');" />&emsp;
                <input type="button" class="btn btn-danger" value="信用贷款"  onclick="setUrlCookie(); callhtml('Message/NoticeList.aspx?id=4', '信用贷款');"/>
            </div>

              <div class="col-sm-12" style="padding:10px">
                <input type="button" class="btn btn-success" value="办卡地址" onclick="setUrlCookie(); callhtml('WebAdmin/CardList.aspx', '办卡地址');" />&emsp;
                <input type="button" class="btn btn-danger" value="新手指南"  onclick="callhtml('Message/NoticeList.aspx?id=5', '新手指南');"/>
            </div>

              <div class="col-sm-12" style="padding:10px">
                <input type="button" class="btn btn-success" value="信贷超市" onclick="setUrlCookie(); callhtml('WebAdmin/credit_market', '信贷超市');" />&emsp;
                  <input type="button" class="btn btn-danger" value="办卡地址" onclick="setUrlCookie(); callhtml('WebAdmin/CardList.aspx', '办卡地址');" />
                  
            </div>--%>
              <div class="col-sm-12" style="padding:10px">
            </div>

              <div class="col-sm-12" style="padding:10px">
            </div>

        </div>
    </form>
              </div>
        </div>
</body>
</html>
