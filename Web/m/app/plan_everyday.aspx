<%@ Page Title="" Language="C#" MasterPageFile="~/m/app/kgjSite.Master" AutoEventWireup="true" CodeBehind="plan_everyday.aspx.cs" Inherits="Web.m.app.plan_everyday" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .itemlist_delete_btn_a {
            color: white;
            margin: 0px;
            float: right;
            display: block;
            position: relative;
            text-align: center;
            text-decoration: none !important;
            padding: 13% 10px 10px 10px;
            width: 33.33%;
            height: 100%;
            font-size: 16px;
        }

        .itemlist_delete_container {
            display: none;
            background-color: #ffffff;
            width: 180px;
            padding-top: 0px;
            border-bottom: 1px solid #d1d1d1;
            border-top: 1px solid #d1d1d1;
        }

        .bankimg {
            width: 100%;
            height: auto !important;
            margin-top: 2px;
        }
        .text-left{padding:0px;padding-left:5px;}
        .financelable {
            margin-left: -7px;
        }
        .financelable span{font-size:12px !important;}
        .in-list-first{width:50%;float:left;padding-right:10px;text-align:right;font-size: 12px !important;color:blue;padding-bottom:4px;}
        .in-list-second{width:50%;float:left;padding-left:10px;text-align:left;font-size: 12px !important;color:blue;padding-bottom:4px;}
    </style>
     <script src="/common/js/jquery.touchSwipe.min.js"></script>
    <script src="/common/laypage/laypage.js"></script>
    <script src="/common/laytpl/laytpl.js"></script>
        <script type="text/javascript">
            $(function () {
                getPageList(1);
            });

            function addSwipe() {
                var toLeft = '180px'; //向左移动的距离'
                //滑动方法
                $(".itemlist").swipe({
                    swipe: function (event, direction, distance, duration, fingerCount) {
                        $(".itemlist_delete").hide();
                        $(".itemlist").css("left", "0px");
                        var thisObj = $(this);
                        if (direction == 'left') {
                            thisObj.animate({ left: '-' + toLeft }, "fast", function () {
                                thisObj.children(".itemlist_delete").show().css({ "right": "-" + toLeft, "width": toLeft });
                            });
                        }
                        else if (direction == 'right') {
                            thisObj.children(".itemlist_delete").hide();
                            thisObj.css("left", "0px");
                        }
                    },
                    allowPageScroll: "auto"
                });
            }

            function setStoreOrExpenseStatus() {
                //设置存入和消费状态，是否改变颜色
                $(".storestatus").each(function () {
                    if ($(this).data("stats") == "2") {
                        $(this).css("color", "green");
                        $(this).find("span").html("已还款");
                    }
                });
                $(".expensestatus").each(function () {
                    if ($(this).data("stats") == "2") {
                        $(this).css("color", "green");
                        $(this).find("span").html("已消费");
                    }
                });
            }

            function getPageList(curr) {
                var pagesi = 10;
                var fromDate = $("#txtFromDate").val().trim();
                var toDate = $("#txtToDate").val().trim();
                var bank = $("#ddl_Bank").val().trim();
                //分页参数，需要传到后台的
                var param = { cid: '', pageIndex: curr, pageSize: pagesi, fromDate: fromDate, toDate: toDate, bank: bank };
                //ajax获取
                $.getJSON("/Handler/GetEveryDayPlanPageList.ashx?rand=" + Math.random(), param, function (res) {
                    $("#spTotalStoreMoney").html(res.totalStoreMoney);
                    $("#spTotalExpenseMoney").html(res.totalExpenseMoney);
                    $("#spTotalCostMoney").html(res.totalCostMoney);
                    if (res.Total <= 0) {
                        $('#appendView').html("当日无工作");
                        $("#spTotalStoreMoney").html(0);
                        $("#spTotalExpenseMoney").html(0);
                        $("#spTotalCostMoney").html(0);
                        return;
                    }
                    else {
                        setGuides('6', '');
                    }
                    var gettpl = document.getElementById('demo').innerHTML;
                    laytpl(gettpl).render(res.Rows, function (html) {
                        $('#appendView').html(html);
                        setStoreOrExpenseStatus();
                        addSwipe();
                    });
                    var pages = Math.ceil(res.Total / pagesi); //得到总页数
                    //显示分页
                    laypage({
                        cont: 'pageContent', //容器。值支持id名、原生dom对象，jquery对象。【如该容器为】：<div id="page1"></div>
                        pages: pages, //通过后台拿到的总页数
                        groups: 0,
                        first: false,
                        last: false,
                        curr: curr || 1, //当前页
                        jump: function (obj, first) { //触发分页后的回调
                            if (!first) { //点击跳页触发函数自身，并传递当前页：obj.curr
                                getPageList(obj.curr);
                            }
                        }
                    });
                });
            }

            function deletePlanDetail(obj, planDetailCode) {
                //删除某一天的规划,把删除日的存入和刷出要分到今天之后的其他天
                //如果执行过消费或存入，就不能删除
                if ($(obj).data("ststus") == "2" || $(obj).data("estatus") == "2") {
                    layerAlert("您已执行过消费或存入操作，不能删除！"); return false;
                }
                else {
                    layer.open({
                        content: '确定要删除该规划信息吗？'
                   , btn: ['确定', '取消']
                   , yes: function (index, layero) {
                       //删除，根据Code
                       layerLoading();
                       var info = {
                           type: 'deletePlanDetail',
                           code: planDetailCode
                       };
                       var result = GetAjaxString(info);
                       closeLayerLoading();
                       if (result == "0") {
                           layerMsg("操作成功");
                           $(obj).parent().parent().remove();
                           //getPageList(1);
                       }
                       else {
                           layerAlert("操作失败，请重试");
                       }
                       layer.close(index);
                   }, no: function (index, layero) {
                       layer.close(index);
                   }
                    });

                }
            }

            function setExpenseOrStatusMoney(type, domObj, planDetailCode) {
                if (type == 2 && $(domObj).data("ststus") == "2") { //type=2存入
                    layerAlert("您已执行过该笔存入，请勿重复存入"); return false;
                }
                if (type == 1 && $(domObj).data("estatus") == "2") { //type=1消费
                    layerAlert("您已执行过该笔消费，请勿重复消费"); return false;
                }
                //找到这一行的可用余额
                var bankcode = $(domObj).parent().data("bankcode");
                var thisBalanceMoney = $(domObj).parent().parent().find(".bank-" + bankcode);
                
                var thisBalanceMoneyVal = parseFloat(thisBalanceMoney.html().trim());

                var expenseMoney=parseFloat($(domObj).parent().data("expensemoney"));
                var storeMoney = parseFloat($(domObj).parent().data("storemoney"));


                layer.open({
                    content: '确定要执行该操作吗？'
                , btn: ['确定', '取消']
                , yes: function (index, layero) {
                    //删除，根据Code
                    layerLoading();
                    var info = {
                        type: 'setExpenseOrStatusMoney',
                        code: planDetailCode,
                        opertype: type
                    };
                    var result = GetAjaxString(info);
                    closeLayerLoading();
                    if (result == "0") {
                        layerMsg("操作成功", { time: 1500 });
                        //重新加载查询
                        //getPageList(1);
                        //return false;

                        if (type == 1) {
                            $(domObj).attr("data-estatus", "2");
                            $(domObj).data("estatus", "2");
                            $(domObj).parent().children(".control_delete").attr("data-estatus", "2");
                            $(domObj).parent().parent().find(".expensestatus").css("color", "green");
                            $(domObj).parent().parent().find(".expensestatus").find("span").html("已消费");
                            //消费完了之后，更新整个可用余额
                            $(".bank-" + bankcode).each(function () {
                                var oneBalanceMoneyVal = parseFloat($(this).html().trim());
                                //消费的话是余额减去消费的金额
                                $(this).html(oneBalanceMoneyVal - expenseMoney);
                            });

                        }
                        else if (type == 2) {
                            $(domObj).attr("data-ststus", "2");
                            $(domObj).data("ststus", "2");
                            $(domObj).parent().children(".control_delete").attr("data-ststus", "2");
                            $(domObj).parent().parent().find(".storestatus").css("color", "green");
                            $(domObj).parent().parent().find(".storestatus").find("span").html("已还款");

                            //还款完了之后，更新整个可用余额
                            $(".bank-" + bankcode).each(function () {
                                var oneBalanceMoneyVal = parseFloat($(this).html().trim());
                                //还款的话是余额减去消费的金额
                                $(this).html(oneBalanceMoneyVal +storeMoney);
                            });

                            //还款完了之后，更新整个"还需还款"
                            $(".lessStoreMoney-" + bankcode).each(function () {
                                var oneLessStoreMoneyVal = parseFloat($(this).html().trim());
                                //还款的话是余额减去消费的金额
                                $(this).html(oneLessStoreMoneyVal - storeMoney);
                            });

                        }
                    }
                    else if (result == "7") {
                        var alertInfo = "您的账号已到可用时间！使用该功能需支付<%=Service.CacheService.GlobleConfig.Field1%>元续用该功能";
                        layer.open({
                            content: alertInfo
                          , btn: ['立即支付', '我再逛逛']
                          , yes: function (index, layero) {
                              //按钮【立即支付】的回调，跳转到支付页面
                              window.location = '<%=Service.GlobleConfigService.GetWebConfig("WebSiteDomain").Value + "/AliPay/alipay"%>';
                          }, btn2: function (index, layero) {
                              //按钮【我再逛逛】的回调,跳转到首页
                              window.location = "index";
                          }
                        });
                          return false;
                      }
                      else if (result == "8") {
                          var alertInfo = "您的体验时间已过期！使用该功能需支付<%=Service.CacheService.GlobleConfig.Field1%>元成为缴费会员";
                          layer.open({
                              content: alertInfo
                            , btn: ['立即支付', '我再逛逛']
                            , yes: function (index, layero) {
                                //按钮【立即支付】的回调，跳转到支付页面
                                window.location = '<%=Service.GlobleConfigService.GetWebConfig("WebSiteDomain").Value + "/AliPay/alipay"%>';
                             }, btn2: function (index, layero) {
                                 //按钮【我再逛逛】的回调,跳转到首页
                                 window.location = "index";
                             }
                           });
                             return false;
                         }

                         else {
                             layerAlert("操作失败，请重试");
                         }
                    layer.close(index);
                }, no: function (index, layero) {
                    layer.close(index);
                }
                });
     }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <!--卡片管理标题********************-->
    <div class="row bg-wh">
        <h5><b>|</b>每日计划</h5>
    </div>
    <div class="row">
             <select id="ddl_Bank" class="form-control"  runat="server" onchange="getPageList(1)">
                </select>
             <input type="text" id="txtFromDate" placeholder="开始日期" class="searchinbartxt hidden" "/> <input  class="searchinbartxt hidden"  type="text" id="txtToDate" placeholder="结束日期"/>
    </div>
    <div class="row  list-border">
          今日总还款:<span id="spTotalStoreMoney"></span>
         今日总消费:<span id="spTotalExpenseMoney"></span><br />
          <span id="spTotalCostMoney" style="display:none"></span>
    </div>

    <div id="appendView" ></div>
    <div class="clearfix"></div>
    <div class="row-fluid" style="margin-top: 20px; text-align: center" id="pageContent">
    </div>
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentFooterHolder" runat="server">
     <script id="demo" type="text/html">
     {{# for (var i = 0; i<d.length; i++){ }}
			<div   class="row marg list-border itemlist" >
                <div class="clickButton" data-code="{{ d[i].Code}}">

                       <div class="col-sm-12 col-xs-12 div_in_list">
                          <div class="in-list-first">  卡号后四位：{{ d[i].CardID }}</div>
                           <div  class="in-list-second">  本期应还款：{{ d[i].BillMoney }}</div>
				    </div>
                    <div class="col-sm-4 col-xs-4 div_in_list">
					    <img src="/{{ d[i].BankImg }}" class="bankimg"/>
				    </div>
				    <div class="col-sm-8 col-xs-8 div_in_list financelable">
                          <span class="col-sm-12 col-xs-12 text-left">
                           <span  class="col-sm-6 col-xs-6 padding-left-right-0"><span class="">可用余额:</span><span class="bank-{{ d[i].Bank }}-{{ d[i].CardID }}">{{ d[i].CurrentBalanceMoney }}</span></span>
                          <span class="col-sm-6 col-xs-6 padding-left-right-0"><span class=""> 还需还款:</span><span class="lessStoreMoney-{{ d[i].Bank }}-{{ d[i].CardID }}">{{ d[i].LeaveStoreMoney }}</span></span>
				       </span>

				       <span class="col-sm-12 col-xs-12 text-left">
                           <span data-stats="{{ d[i].ExpenseStatus }}" class="col-sm-6 col-xs-6 padding-left-right-0 expensestatus "><span class="expenselable"> {{ d[i].POSFirstIndustry }}</span>:{{ d[i].ExpenseMoney }}</span>
                            <span data-stats="{{ d[i].StoreStatus }}" class="col-sm-6 col-xs-6 padding-left-right-0 storestatus"><span class="storelable"> 今日还款</span>:{{ d[i].StoreMoney }}</span>
				       </span>
				    </div>
                </div>

                 <div class="itemlist_delete delButton itemlist_delete_container"  data-code="{{ d[i].Code}}"  data-bankcode="{{ d[i].Bank }}-{{ d[i].CardID }}" data-expensemoney="{{ d[i].ExpenseMoney }}"  data-storemoney="{{ d[i].StoreMoney }}">
                        <a class="itemlist_delete_btn_a  btn-danger control_delete" onclick="deletePlanDetail(this,'{{ d[i].Code}}')" data-ststus="{{ d[i].StoreStatus}}"  data-estatus="{{ d[i].ExpenseStatus}}">删除</a>
                       <a class="itemlist_delete_btn_a  btn-success control_store" onclick="setExpenseOrStatusMoney(2, this,'{{ d[i].Code}}') " data-ststus="{{ d[i].StoreStatus}}"  data-estatus="{{ d[i].ExpenseStatus}}">还款</a>
                       <a class="itemlist_delete_btn_a  btn-info control_expense" onclick="setExpenseOrStatusMoney(1, this,'{{ d[i].Code}}') " data-ststus="{{ d[i].StoreStatus}}"  data-estatus="{{ d[i].ExpenseStatus}}">消费</a>
                 </div>
			</div>
        {{# } }}
    </script>
</asp:Content>

