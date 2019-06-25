<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Web.Admin.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <!-- Use title if it's in the page YAML frontmatter -->
    <title><%=Service.CacheService.GlobleConfig.Contacter %>后台管理系统</title>

    <link rel="stylesheet" type="text/css" href="css/bootstrap/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="css/libs/font-awesome.css" />
    <link rel="stylesheet" type="text/css" href="css/compiled/theme_styles.css" />
    <!--[if lt IE 9]>
		<script src="/Admin/js/html5shiv.js"></script>
		<script src="/Admin/js/respond.min.js"></script>
	<![endif]-->

    <link href="pop/css/pop.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
       
    </style>
</head>
<body>
    <div id="theme-wrapper">
        <header class="navbar" id="header-navbar">
<div class="container">
<a href="/Admin/Index" id="logo" class="navbar-brand" style="text-align:center"> 
   <span style="font-size:16px"><%=Service.CacheService.GlobleConfig.Contacter %></span> 
</a>
<div class="clearfix">
<button class="navbar-toggle" data-target=".navbar-ex1-collapse" data-toggle="collapse" type="button">
<span class="sr-only">Toggle navigation</span>
<span class="fa fa-bars"></span>
</button>
<div class="nav-no-collapse navbar-left pull-left hidden-sm hidden-xs">
<ul class="nav navbar-nav pull-left">
<li>
<a class="btn" id="make-small-nav">
<i class="fa fa-bars"></i>
</a>
</li>
</ul>
</div>
<div class="nav-no-collapse pull-right" id="header-nav">
<ul class="nav navbar-nav pull-right">
 

<li class="dropdown hidden-xs">
<a class="btn dropdown-toggle"  href="/Admin/Index">
<i class="fa fa-dashboard"></i>&nbsp;首页
</a>
</li>
 
<li class="dropdown profile-dropdown">
<a href="javascript:void(0)"  class="dropdown-toggle" data-toggle="dropdown">
<img src="/Admin/img/samples/user-l.png" alt=""/>
<span class="hidden-xs"><%=TModel.MID %></span> <b class="caret" style="margin-top: 15px;"></b>
</a>
<ul class="dropdown-menu">
<li><a href="javascript:void(0)" onclick="callhtml('/Admin/Member/ChangePwd.aspx','修改密码');"><i class="fa fa-cog"></i>修改密码</a></li>
</ul>
</li>
 
 <li class="dropdown hidden-xs" style="margin-right:50px">
<a class="btn dropdown-toggle" href="/Admin/Out">
<i class="fa fa-power-off"></i>&nbsp;安全退出
</a>
</li>


</ul>
</div>
</div>
</div>

</header>
        <div id="page-wrapper" class="container">
            <div class="row">
                <div id="nav-col">
                    <section id="col-left" class="col-left-nano">
<div id="col-left-inner" class="col-left-nano-content">
<div id="user-left-box" class="clearfix hidden-sm hidden-xs">
 <%--<img src="Admin/img/logon.png" class="userflag"/>--%>
<div class="user-box">
<span class="name">
欢迎<br/>
<%=TModel.MID %>
</span>
<span class="status">
<i class="fa fa-circle"></i> 在线
</span>
</div>
</div>
<div class="collapse navbar-collapse navbar-ex1-collapse" id="sidebar-nav">

<ul class="nav nav-pills nav-stacked">
 
    <%foreach (Model.Sys_Privage item in listFirstPowers)
      { %>
<li>
<a href="javascript:void(0)" class="dropdown-toggle">
<i class="<%=item.Icon %>" ></i>
<span class="firstMnuName"><%=item.Name %></span>&nbsp;&nbsp;
<i class="fa fa-chevron-circle-right drop-icon"></i>
</a>
    <ul class="submenu">
     <%foreach (Model.Sys_Privage item2 in GetPowers(item.Id.Trim()).Where(c => c.IsDeleted == false))
       { %>
<li>
<a href="javascript:void(0)" onclick="callhtml('<%=item2.URL %>','<%=item2.Name %>');">
<i class="<%=item2.Icon%>"></i>&nbsp;
<%=item2.Name %> 
</a>
</li> 
<%} %>
</ul>
</li>
 <%} %>
</ul>


</div>
</div>
</section>
                </div>
                <div id="content-wrapper">
                    <div class="row">
                        <div class="col-lg-12">
                            <div id="container" class="contant_index">
                                <div class="row">
                                    <%if (false)
                                      { %>
                                    <div class=" col-md-12">
                                        <input type="button" value="修改会员的推荐人" onclick="callhtml('Member/MemberChange', '修改会员推荐人')" class="btn btn-danger hidden" />&emsp;
                                         <input type="button" value="修改代理商的推荐人" onclick="callhtml('Member/AgentChange', '修改代理商推荐人')" class="btn btn-danger hidden" />&emsp;
                                 <input type="button" value="重置今日提醒" onclick="checkChange(2)" class="btn btn-info  hidden" />
                                        <input type="button" value="手动删除昨日未执行规划信息" onclick="checkChange(3)" class="btn btn-info  hidden" />
                                    </div>
                                    <div class=" col-md-12" style="padding: 10px;">
                                        手动操作：会员账号:
                                          <input type="text" id="txtTestMID" />
                                    <select id="ddlAgent" name="ddlagentLeavl" style="display:none">
                                        <option value="applyagent3f">三级分销商</option>
                                        <option value="applyagent2f">二级分销商</option>
                                    </select>
                                    </div>
                                    <div class=" col-md-12">
                                        &emsp; 
                                        <input type="button" value="会员缴费" onclick="checkChange(1)" class="btn btn-danger" />&emsp;
                                            <input type="button" value="升级分销商" onclick="checkChange(4)" class="btn btn-info  hidden" />
                                    </div>

                                    <%} %>
                                </div>
                                <%if (TModel.Role.IsAdmin || TModel.RoleCode == "Admin")
                                  { %>
                                <div class=" col-md-12">
                                    <div style="padding-top: 10px">
                                        用户活跃度查看：
                                        <input type="button" class="btn btn-info" value="最近七天" onclick="showChar(7)" />
                                        <input type="button" class="btn btn-info" value="最近十天" onclick="showChar(10)" />
                                        <input type="button" class="btn btn-info" value="最近一月" onclick="showChar(30)" />
                                    </div>
                                    <div id="main" style="width: 100%; height: 400px;"></div>
                                </div>
                                <%} %>
                            </div>
                        </div>
                    </div>


                    <footer id="footer-bar" class="row">
<p id="footer-copyright" class="col-xs-12">
&copy; 2016 <%=Service.CacheService.GlobleConfig.Contacter %>后台管理系统
</p>
</footer>
                </div>
            </div>
        </div>
    </div>

    <script src="/Admin/js/demo-skin-changer.js"></script>
    <script src="/Admin/js/jquery.js"></script>
    <script src="/Admin/js/bootstrap.js"></script>
    <script src="/Admin/js/scripts.js"></script>
    <script type="text/javascript" src="/js/layer-v2.2/layer/layer.js"></script>
    <script type="text/javascript" src="/Admin/pop/js/laytpl/laytpl.js"></script>
    <script type="text/javascript" src="/Admin/pop/js/laypage/laypage.js"></script>
    <script src="/Admin/pop/js/javascript_pop.js" type="text/javascript"></script>
    <script src="/Admin/pop/js/javascript_main.js" type="text/javascript"></script>
    <script src="/Admin/pop/js/ajax.js" type="text/javascript"></script>
    <script src="/Admin/SourceFiles/date/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/Verification.js"></script>
    <script type="text/javascript" src="/Admin/js/MessageAdminService.js"></script>

    <script type="text/javascript" src="/common/js/echarts.min.js"></script>

    <script type="text/javascript">
        function checkChange(type) {
            var testMID = $.trim($("#txtTestMID").val());
            if ($("#txtTestMID").val().trim() == "") {
                layer.alert("会员账号不能为空！");
                return false;
            }
            if (!confirm("请检查手动操作的信息，确定要执行该操作吗？")) return;

            if (type == 1) {
                if (!confirm("确定要为会员" + testMID + "缴费升级吗？"))
                    return false;
            }
            else if (type == 4) {
                if (!confirm("确定要为会员" + testMID + "升级为" + $("#ddlAgent").find("option:selected").text() + "吗？"))
                    return false;
            }
            var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            var rek = '<%=Request.RawUrl%>';
            rek = rek.substring(rek.lastIndexOf('/') + 1, rek.indexOf('?'));
            $.ajax({
                type: 'post',
                url: rek + '?Action=add',
                data: 'mid=' + testMID + "&type=" + type + "&ddlagentLeavl=" + $("#ddlAgent").val(),
                success: function (info) {
                    layer.close(index);
                    if (info == "0")
                        layer.alert("提交失败，请重试！");
                    else if (info == "1") {  //提交成功
                        layer.alert("操作成功！");
                    }
                    else
                        layer.alert(info);
                }
            });
        }
        var layindexof;
        var layiframe = '';
        $(function () {
            var isShowInfo = '<%=IsShowBaseInfo%>';
            if (isShowInfo == '1')
                callhtml('Agent/AgentInfo', '个人信息');
            //轮询消息
            checkNewMessageNotice();
        });

    </script>

    <script type="text/javascript">
        var myChart = echarts.init(document.getElementById('main'));
        var option = {
            title: {
                text: '用户活跃度分析'
            },
            tooltip: {
                trigger: 'axis'
            },

            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            toolbox: {
                feature: {
                    saveAsImage: {}
                }
            },
            xAxis: {
                type: 'category',
                boundaryGap: false,
                data: []
            },
            yAxis: {
                type: 'value'
            },
            series: [
                {
                    name: '登录人数',
                    type: 'line',
                    data: []
                }
            ]
        };
        function showChar(days) {
            //统计图表
            // 使用刚指定的配置项和数据显示图表。
            var count = [];
            var date = [];
            myChart.showLoading();
            //ajax获取
            $.ajax({
                type: "post",
                async: true,            //异步请求（同步请求将会锁住浏览器，用户其他操作必须等待请求完成才可以执行）
                url: "/Admin/Handler/CountActiveMember.ashx?days=" + days,    //请求发送到TestServlet处
                data: {},
                dataType: "json",        //返回数据形式为json
                success: function (result) {
                    if (result) {
                        myChart.hideLoading();
                        $.each(result.charData, function (index, item) {
                            count.push(item.logCount);
                            date.push(item.logDate);
                        });
                        myChart.setOption({        //加载数据图表
                            title: {
                                text: '最近' + days + '天用户登录情况图'
                            },
                            xAxis: {
                                data: date
                            },
                            yAxis: {
                                type: 'value'
                            },
                            series: [{
                                // 根据名字对应到相应的系列
                                name: '登录人数',
                                type: 'line',
                                data: count
                            }]
                        });
                    }
                }
            });
            myChart.setOption(option);

        }

    </script>
</body>
</html>
