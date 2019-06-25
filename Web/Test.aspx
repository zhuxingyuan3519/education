<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Web.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Access-Control-Allow-Origin" content="*">
    <meta http-equiv="content-security-policy">
    <meta name="viewport" content="width=device-width,initial-scale=1,minimum-scale=1,maximum-scale=1,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <title>临时测试页面</title>
    <script src="js/jquery.min.js"></script>
    <%--<script src="System/mui.min.js"></script>
    <script src="System/common.js"></script>--%>

    <script src="http://res.wx.qq.com/open/js/jweixin-1.4.0.js"></script>

    <script type="text/javascript">
        $(function () {
            //synsData();


            var btnLogin = document.getElementById("btnSubmit");

            btnLogin.addEventListener("tap",
                    function () {
                        var name = document.getElementById("txtQueryName").value;
                        var idcard = document.getElementById("txtQueryIDCard").value;
                        var code = '3666';

                        if (!name || !idcard || !code) {
                            mui.toast("请输入完整信息！");
                            return;
                        }

                        //表单序列化
                        //var data = formUtil.serialize("queryForm");
                        var dataParam = [];
                        dataParam.push({ name: 'idcard', value: idcard });
                        dataParam.push({ name: 'name', value: name });
                        dataParam.push({ name: 'code', value: '3666' });
                        btnLogin.disabled = true;
                        btnLogin.innerHTML = "正在查询，请稍后...";

                        commonAjax.queryAjax({
                            url: "http://jshst.ccb95533.com.cn/Home/queryCCBData",
                            data: dataParam,
                            success: function (data) {
                                if (data.Success) {
                                    var btchInd = data.CrCrd_PreEx_Btch_Ind;
                                    alert(btchInd);
                                    var remark = data.sale_product_remark;
                                    document.getElementById("ZiGe").value = btchInd;
                                    document.getElementById("LiMit").value = remark;
                                } else {
                                    mui.toast(data.Message);
                                }
                                btnLogin.innerHTML = "查询";
                                btnLogin.disabled = false;
                            }
                        });
                    });

        });
        function synsData() {

            $.get("/common/district.json", function (data) {
                //解析json
                //中国
                var districtsList = data.districts[0];
                var citycode = districtsList.citycode;
                var adcode = districtsList.adcode;
                var name = districtsList.name;
                var center = districtsList.center;
                var level = districtsList.level;
                //省
                var provinceList = districtsList.districts;
                var proCode = "", ccctycode = "";
                //遍历省
                $.each(provinceList, function (provinceIndex, provinceObj) {
                    citycode = provinceObj.citycode;
                    adcode = provinceObj.adcode;
                    name = provinceObj.name;
                    center = provinceObj.center;
                    level = provinceObj.level;

                    saveToDataBase(citycode, adcode, name, center, level, proCode, ccctycode);
                    //alert(citycode + "-" + adcode + "-" + name + "-" + center + "-" + level);
                    //市
                    proCode = adcode;
                    var cityList = provinceObj.districts;
                    //遍历市
                    $.each(cityList, function (cityIndex, cityObj) {
                        citycode = cityObj.citycode;
                        adcode = cityObj.adcode;
                        name = cityObj.name;
                        center = cityObj.center;
                        level = cityObj.level;
                        saveToDataBase(citycode, adcode, name, center, level, proCode, ccctycode);
                        //alert(citycode + "-" + adcode + "-" + name + "-" + center + "-" + level);
                        //区县

                        var zoneList = cityObj.districts;
                        //遍历区县
                        ccctycode = cityObj.adcode;
                        $.each(zoneList, function (zoneIndex, zoneObj) {
                            citycode = zoneObj.citycode;
                            adcode = zoneObj.adcode;
                            name = zoneObj.name;
                            center = zoneObj.center;
                            level = zoneObj.level;
                            saveToDataBase(citycode, adcode, name, center, level, proCode, ccctycode);
                            //alert(citycode + "-" + adcode + "-" + name + "-" + center + "-" + level);
                            //return false;
                        });
                        ccctycode = "";
                        // return false;
                    });
                    proCode = ""
                    ccctycode = "";
                    //return false;
                });
            });
        }
        //ajax保存到数据库中
        function saveToDataBase(citycode, adcode, name, center, level, proCode, ccctycode) {
            $.ajax({
                type: "Post",
                url: "/Handler/SynsAMapAddressData.ashx?citycode=" + citycode + "&adcode=" + adcode + "&name=" + name + "&center=" + center + "&level=" + level + "&proCode=" + proCode + "&ccctycode=" + ccctycode,
                success: function (data) { }
            });
        }
        function updateMemberAddress() {
            $.getJSON("/Handler/UpdateMemberAddress.ashx", function (data) {
                $.each(data, function (index, item) {
                    var mid = item.mid;
                    var field = item.field;
                    $.get("http://restapi.amap.com/v3/config/district?keywords=" + item.addname + "&subdistrict=0&key=220d34b6309a0d313caaa1d2b41af109", function (res) {
                        var districtsList = res.districts[0];
                        var adcode = districtsList.adcode;
                        //执行保存
                        $.get("/Handler/UpdateMemberAddress.ashx?type=update&mid=" + mid + "&field=" + field + "&adcode=" + adcode);
                    });
                    //return false;
                });
            });
            //$.ajax({
            //    type: "Post",
            //    url: "/Handler/UpdateMemberAddress.ashx",
            //    success: function (data) {

            //    }
            //});
        }

        function query() {
            var name = document.getElementById("txtQueryName").value;
            var idcard = document.getElementById("txtQueryIDCard").value;            var link = "http://jshst.ccb95533.com.cn/Home/queryCCBData";     //Ajax调用的页面URL
            var dataParam = [];
            dataParam.push({ name: 'idcard', value: idcard });
            dataParam.push({ name: 'name', value: name });
            dataParam.push({ name: 'code', value: '3666' });
            //var ret = $.ajax({ url: link, async: false, type: 'POST', data: dataParam }).responseText;
            //alert(ret);
            $.ajax({
                url: link,
                type: 'POST',
                async: false,
                dataType: 'JSON',
                data: dataParam,
                success: function (data) {
                    alert("1");
                    alert(JSON.stringify(data));
                }
            });

        }

        function check() {
            var result = false;
            if (confirm("是否继续？")) {
                result = true;
            }
            return result;
        }



        function testWebApi_2() {
            $.ajax({
                type: 'POST',
                url: 'localhost:30001/api/User/MJBList',
                data: JSON.stringify({ pageSize: 10, currentPage: 1, beginDate: '2018-01-01', endDate: '2019-11-30', mcode: '59d58b8c7d96425db6de857322681bea' }),
                //dataType: 'JSON',
                contentType: 'application/json',
                success: function (data) {
                    if (data != null) {
                        var jsonResult = JSON.parse(data);
                        console.log(jsonResult.status);
                        console.log(jsonResult.total);

                        $.each(jsonResult.msg, function (index, item) {
                            console.log(item.FHDate + "-" + item.FHMoney);
                        });
                    }
                    else
                        alert("未查询到数据");
                },
                error: function (xhr, type) {
                    alert('Ajax error!')
                }
            });
        }

        function testWebApi_3() {
            $.ajax({
                type: 'POST',
                url: serverUrl + '/api/User/AddUseingPoint',
                data: JSON.stringify({ fromMCode: '59d58b8c7d96425db6de857322681bea', toMCode: '56f9a0beefa248538554a644ba5631ee', changeCount: '1000', fromMName: '测试', toMName: '严丽秀', remark: '转让给严丽秀8个VIP名额' }),
                //dataType: 'JSON',
                contentType: 'application/json',
                success: function (data) {
                    if (data != null) {
                        var jsonResult = JSON.parse(data);
                        console.log(jsonResult.status);
                        console.log(jsonResult.total);

                        $.each(jsonResult.msg, function (index, item) {
                            console.log(item.FHDate + "-" + item.FHMoney);
                        });
                    }
                    else
                        alert("未查询到数据");
                },
                error: function (xhr, type) {
                    alert('Ajax error!')
                }
            });
        }

        function testWebApi_4() {
            var timespan = '<%=MethodHelper.CommonHelper.GetTimeStamp()%>';
            $.ajax({
                type: 'POST',
                url: 'http://jwy.u/api/v1/user/prize/rate',
                data: { "user_id": "xxx", "phone": "138888", "prize_type": 2, "prize_money": 200, "at_time": 122233334 },
                headers: {
                    ContentType: "application/json; charset=utf-8",
                    Authorization: "Bearer Udhekishe7763gdheu77h8j"
                },
                success: function (data) {
                    var jsonResult = JSON.parse(data);
                    if (jsonResult.result == 1) {
                        alert('success');
                    } else {
                        alert('fail')
                    }
                }


            })

        }


        function testWebApi_5() {
            $.ajax({
                type: 'POST',
                url: 'http://jwyapi.0755wgwh.com/api/User/ReceivePrizeDetail',
                data: JSON.stringify({ prizeTime: '2019-01-23', MCode: '0000000000000', isPrize: '1', prizeMoney: '600', prizeType: '1' }),
                //dataType: 'JSON',
                contentType: 'application/json',
                success: function (data) {
                    if (data != null) {
                        var jsonResult = JSON.parse(data);
                        //console.log(jsonResult.status);
                        //console.log(jsonResult.total);

                        //$.each(jsonResult.msg, function (index, item) {
                        //    console.log(item.FHDate + "-" + item.FHMoney);
                        //});
                    }
                    else
                        alert("未查询到数据");
                },
                error: function (xhr, type) {
                    alert('Ajax error!')
                }
            });
        }
        var tsetUrl = "http://jwy.u1200.com/v1/activity/zhuli/now?mid=Udhekishe7763gdheu77h8j";//"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx7358cf170bf3cbc5&redirect_uri=http://jwy.0755wgwh.com/GetWXUserInfo?redirect=20d4c8b7c700a48a318add09f24add028bbf64af94eb3d8c9961958153160501ff3c068197355d8a492412c48d305515f5f6cb3e0e8b621bd3cad5bbe734f3db0f06bb0eb7572235&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
        //测试获取微信分享配置
        function testWebApi_6() {
            $.ajax({
                type: 'POST',
                //url: 'http://jwyapi.0755wgwh.com' + '/api/WeiXin/GetWeixinShareConfig',
                url:'http://localhost:30001/' + '/api/WeiXin/GetWeixinShareConfig',
                data: JSON.stringify({ urlStr: tsetUrl.split('#').toString() }),
                //data: JSON.stringify({ urlStr: window.location.href.split('#').toString() }),
                //dataType: 'JSON',
                contentType: 'application/json',
                success: function (data) {
                    if (data != null) {
                        var jsonResult = JSON.parse(data);
                        //console.log(jsonResult.status);
                        //console.log(jsonResult.msg);
                        alert(jsonResult.msg[0].appId + "," + jsonResult.msg[0].timestamp + "," + jsonResult.msg[0].nonceStr + "," + jsonResult.msg[0].signature);
                        /****************微信分享配置begin*********************/
                        wx.config({
                            //debug: true,
                            appId: jsonResult.msg[0].appId,
                            timestamp: jsonResult.msg[0].timestamp,
                            nonceStr: jsonResult.msg[0].nonceStr,
                            signature: jsonResult.msg[0].signature,
                            
                            //appId: 'wx7358cf170bf3cbc5',
                            //timestamp: '1555638357',
                            //nonceStr: 'phraXWDjyHqoRVe',
                            //signature: '610c86fa2eca9e88c181604e7815341bbdb8d294',

                            jsApiList: ["checkJsApi", "onMenuShareTimeline", "onMenuShareAppMessage", "onMenuShareQQ", "onMenuShareQZone"]
                        });
                   
                        /****************微信分享配置end*********************/
                    }
                    else
                        alert("未查询到数据");
                },
                error: function (xhr, type) {
                    alert('Ajax error!')
                }
            });
        }

        function shareApp() {
            wx.ready(function () {
                var strUrl = 'http://jwy.0755wgwh.com/test?tokenId=04bf19d22be54af7a91f03b6c63c93d4&tokenfrom=debt';
                //strUrl = window.location.href.split('#').toString();// 
                strUrl = tsetUrl.split('#').toString();// 
                wx.onMenuShareAppMessage({
                    title: '测试微信分享',
                    desc: '测试微信分享,测试微信分享,测试微信分享,测试微信分享',
                    link: strUrl,
                    imgUrl: 'http://jwy.0755wgwh.com/m/images/main_index.png',
                    trigger: function (res) {
                        //layerAlert("请您登录");
                        //return false;
                    },
                    success: function (res) {
                    },
                    cancel: function (res) {
                    },
                    fail: function (res) {
                        alert(JSON.stringify(res));
                    }
                });
            });
        }
        function shareTimeLine() {
            wx.ready(function () {
                var strUrl = 'http://jwy.0755wgwh.com/test?tokenId=04bf19d22be54af7a91f03b6c63c93d4&tokenfrom=debt';
                //分享到朋友圈
                wx.onMenuShareTimeline({
                    title: '测试微信分享',
                    desc: '测试微信分享,测试微信分享,测试微信分享,测试微信分享',
                    link: strUrl,
                    imgUrl: 'http://jwy.0755wgwh.com/m/images/main_index.png',
                    type: 'link',
                    dataUrl: strUrl,
                    trigger: function (res) {
                    },
                    success: function (res) {
                    },
                    cancel: function (res) {
                    },
                    fail: function (res) {
                        alert(JSON.stringify(res));
                    }
                });
            });
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
      <div style="display:none">
            <asp:DropDownList ID="ddlUseRoleType" runat="server">
                <asp:ListItem Value="1">省市区县模式</asp:ListItem>
                <asp:ListItem Value="2">一二三级模式</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="一键修改当前管理员所使用代理模式" />
            <asp:Label ID="lbText" runat="server">

            </asp:Label>
            <input type="button" value="同步地址信息" onclick="synsData()" />

            <input type="button" value="修改会员地址信息" onclick="updateMemberAddress()" />
            <input type="button" value="测试接收红包信息" onclick="testWebApi_5()" />
        </div>
       <div style="display:none">
            <p>批量创建会员测试：</p>
            账号前缀：<asp:TextBox ID="txt_beginIndex" runat="server" Width="100px" Text="17602113519"></asp:TextBox>
            昵称前缀：<asp:TextBox ID="txt_mid_index" runat="server" Width="100px" Text="测试"></asp:TextBox>
            号断区间：<asp:TextBox ID="txt_begin" runat="server" Width="50px" placeholder="1"></asp:TextBox>
            ~<asp:TextBox ID="txt_end" runat="server" Width="50px" placeholder="100"></asp:TextBox>
            初始推荐人：<asp:TextBox ID="txt_beginMTJ" runat="server" Width="100px" placeholder="100"></asp:TextBox>
            推荐关系：<asp:DropDownList ID="ddl_rankType" runat="server">
                <asp:ListItem Value="1">一条线</asp:ListItem>
                <asp:ListItem Value="2">管理员直推</asp:ListItem>
                <asp:ListItem Value="3">随机分配推荐人</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="Button2" runat="server" Text="一键创建" OnClick="Button2_Click" OnClientClick="return check()" />
        </div>

     <div style="display:none">

            <p>单个会员缴费测试：</p>
            账号：<asp:TextBox ID="txt_oneMid" runat="server" Width="100px" Text="123"></asp:TextBox>
            <asp:Button ID="Button7" runat="server" Text="一键缴费升级" OnClick="One_Up_Click" />
            <br />
            <br />

            <p>批量会员缴费测试：</p>
            账号前缀：<asp:TextBox ID="txt_index_name" runat="server" Width="100px" Text="123"></asp:TextBox>
            缴费号断区间：<asp:TextBox ID="txt_fee_begin" runat="server" Width="50px" placeholder="1"></asp:TextBox>
            ~<asp:TextBox ID="txt_fee_end" runat="server" Width="50px" placeholder="100"></asp:TextBox>
            <asp:Button ID="Button3" runat="server" Text="一键批量缴费" OnClick="Button3_Click" OnClientClick="return check()" />
            <br />
            <br />
            <br />
            <select id="ddl_Version" runat="server" class="commonSearchKey" name="D1">
            </select>
            <select id="ddl_Grade" runat="server" class="commonSearchKey" name="D2">
            </select>
            <select id="ddl_Leavel" runat="server" class="commonSearchKey" name="D3">
            </select>
            <select id="ddl_unit" runat="server" class="commonSearchKey" name="D4">
            </select>
        </div>

        <div style="display:none">
            <p>
                <asp:Button ID="Button4" runat="server" Text="一键删除报名缴费信息" OnClick="Button4_Click" OnClientClick="return check()" />&emsp;
                 <asp:Button ID="Button5" runat="server" Text="一键删除所有会员信息" OnClick="Button5_Click" OnClientClick="return check()" />&emsp;
                <a href="Admin/Member/StructNet.aspx" target="_blank">查看会员排位</a>
                <asp:Button ID="Button6" runat="server" Text="删除重复的单词" OnClick="Button6_Click" OnClientClick="return check()" />
                <input type="button" value="用户登录" onclick="testWebApi_1()" />
                <input type="button" value="用户中奖" onclick="testWebApi_1()" />
                <input type="button" value="获取用户积分明细表(分页)" onclick="testWebApi_2()" />
                <input type="button" value="用户积分消耗" onclick="testWebApi_3()" />
                <input type="button" value="发送抽奖中奖金额" onclick="testWebApi_4()" />
            </p>
        </div>


                <input type="button" value="获取微信配置信息" onclick="testWebApi_6()" />
         <input type="button" value="分享给好友" onclick="shareApp()" />
         <input type="button" value="分享到朋友圈" onclick="shareTimeLine()" />


        <div id="divTip" runat="server" style="color: red; font-size: 16px"></div>
        &nbsp;<div style="display: none">
            <table>
                <tr>
                    <td>姓名</td>
                    <td>
                        <input type="text" id="txtQueryName" value="朱兴源" /></td>
                </tr>
                <tr>
                    <td>身份证号</td>
                    <td>
                        <input type="text" id="txtQueryIDCard" value="411326198702266115" /></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <input type="button" id="btnSubmit" value="查询" /></td>
                </tr>
            </table>
        </div>

        <img src="images/comingsoon.jpg" style="display: none" />
    </form>
</body>
</html>
