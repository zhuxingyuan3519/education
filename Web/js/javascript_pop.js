var cidList = new Array(); // 选择行数据
var tUrl = ''; //查询URL
var tCondition = ''; //查询条件
var pageIndex = 0; // 页面索引初始值
var pageSize = 10; // 每页显示条数初始化，修改显示条数，修改这里即可
var count = 0;

function SearchByCondition() {
    PageLoad();
}
var tt = 0;
function PageLoad() {
    InitTable(0);
}

function page(count) {
    $("#Pagination").pagination(count, {
        callback: PageCallback,
        prev_text: '上一页',       //上一页按钮里text
        next_text: '下一页',       //下一页按钮里text
        items_per_page: pageSize,  //显示条数
        num_display_entries: 6,    //连续分页主体部分分页条目数
        current_page: pageIndex,   //当前页索引
        num_edge_entries: 2        //两侧首尾分页条目数
    });
}

function PageCallback(index, jq) {
    InitTable(index);
}

function InitTable(pageIndex) {
    if (tt > 2)
        return;
    else
        tt++;
    $.ajax({
        type: "POST",
        dataType: "json",
        url: tUrl,      //确认到一般处理程序请求数据
        data: "pageIndex=" + (pageIndex + 1) + "&pageSize=" + pageSize + tCondition,
        success: function (arr) {
            $("#Result tr:gt(0)").remove();
            $("#Result").append(JsonToTable(arr.PageData));
            if (count != arr.TotalCount) {
                count = arr.TotalCount;
                page(arr.TotalCount);
            }
        }
    });
    tt = 0;
}
/* 查询行数据 */


//table转josn
function TableToJson(tableid) {
    var txt = "[";
    var table = document.getElementById(tableid);
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");
    for (var j = 1; j < row.length; j++) {
        var r = "{";
        var tds = row[j].getElementsByTagName("td");
        for (var i = 0; i < col.length; i++) {
            r += "\"" + col[i].innerHTML + "\"\:\"" + tds[i].getElementsByTagName("input")[0].value + "\",";
        }
        r = r.substring(0, r.length - 1)
        r += "},";
        txt += r;
    }
    txt = txt.substring(0, txt.length - 1);
    txt += "]";
    return txt;
}

function JsonToTable(str) {
    var trs = str.split('≌');
    var table = '';
    for (var i = 0; i < trs.length - 1; i++) {
        var tds = trs[i].split('~');
        table += "<tr>";
        for (var j = 0; j < tds.length; j++) {
            table += "<td>";
            table += tds[j].replace('#T', '') + '&nbsp;';
        }
        table += "</td></tr>";
    }
    return table;
}
