<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StructNet.aspx.cs" Inherits="Web.Admin.Member.StructNet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>学员排位图</title>
    <script type="text/javascript" src="../../js/jquery.min.js"></script>
    <script src="../../js/layer-v2.2/layer/layer.js"></script>
    <script type="text/javascript" src="../../js/visChart/vis.js"></script>
    <link href="../../js/visChart/vis.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #mynetwork {
            width: 1300px;
            height: 800px;
        }
    </style>
    <script type="text/javascript">
     
        function draw(serdata, linedata) {
            var nodes = eval(serdata);
            var edges = eval(linedata);
            if (nodes.length <= 0) {
                alert("暂无排位图信息")
            }
            // create a network
            var container = document.getElementById('mynetwork');
            var data = {
                nodes: nodes,
                edges: edges
            };
            var options = {
                layout: {
                    hierarchical: {
                        direction: "UD",
                        sortMethod: "directed"
                    }
                },
                interaction: { dragNodes: false },
                physics: {
                    enabled: false
                }
                //layout: {},
                //physics: {
                //    maxVelocity: 0.0000001,
                //    minVelocity: 0.0000001
                //}
            };//这种情况每次打开都现实的不一样
            //var options = { layout: { randomSeed: 10 } };
            var network = new vis.Network(container, data, options);
            network.on("click", function (params) {
                //params.event = "[original event]";
                //alert(params.nodes);
                //alert(params.label);
                if (params.nodes != '') {
                    $.ajax({
                        type: 'post',
                        url: '/Admin/Member/StructNet?Action=Modify',
                        data: 'userCode=' + params.nodes,
                        success: function (info) {
                            $("#divDebtInfo").html(info).show();
                            var layindexof = layer.open({
                                type: 1 //此处以iframe举例
                               , title: '学员信息'
                               , area: ['550px', '350px']
                               , shade: [0.8, '#393D49']
                               , content: $("#divDebtInfo")
                                , btn: ['关闭']
                                , yes: function (index) {
                                    layer.close(index);
                                }
                                 , cancel: function (index) {
                                     layer.close(index);
                                 }
                            });
                        }
                    });
                }
            });

        }

        $(function () {
            var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            setChange();
            //draw("[{id:'3f29a04968f042ce97af49ff77e49e00',label:'会员2'},{id:'476ae24bbdb041a5aed9aad2ec3dba2b',label:'会员4'},{id:'73fd9cd20bb54c3b80151ceabaf6e0b4',label:'会员3'},{id:'f1ee6ec135e44f4b9dc0df7bf4516d05',label:'会员1'}]", "[{from:'f1ee6ec135e44f4b9dc0df7bf4516d05',to:'3f29a04968f042ce97af49ff77e49e00'}]");
            layer.close(index);
        });

        function setChange() {
            var index = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
            ////增加查询条件
            //var dataQuery = "hid_Code=" + $("#hid_Code").val() + "&txt_MinMoney=" + $("#txt_MinMoney").val() + "&txt_MaxMoney=" + $("#txt_MaxMoney").val();
            //var legled = $("input[name='legled'][checked]").val()
            //dataQuery += "&legled=" + legled;
            //var searchAreaId = '';
            //if ($("#divAreaSearch").html().trim() != '') {
            //    $(".areaspan").each(function () {
            //        var areaId = $(this).find("input:last").val();
            //        if (areaId != 'undefined' && areaId.trim() != '') {
            //            searchAreaId += areaId+',';
            //        }
            //    });
            //}
            //dataQuery += "&searchAreaId=" + searchAreaId;
            var dataQuery = "MCode=''";
            $.ajax({
                type: 'post',
                url: '/Admin/Member/StructNet?Action=Add',
                data: dataQuery,
                success: function (info) {
                    var linedata = '[' + info.split('*')[1] + ']';
                    var serisedata = '[' + info.split('*')[0] + ']';
                    draw(serisedata, linedata);
                    layer.close(index);
                }
            });
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="tree_table" id="mynetwork">
        </div>
         <div class=" layerUIPayInfo" id="divDebtInfo" style="display: none; color: black; padding-top: 10px; padding-left: 10px; font-size: 16px">
            </div>
    </form>
</body>
</html>
