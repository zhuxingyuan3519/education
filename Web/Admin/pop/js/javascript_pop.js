var tUrl = ''; //查询URL
var pageIndex = 0; // 页面索引初始值
var pageSize = 20; // 每页显示条数初始化，修改显示条数，修改这里即可

var searchPram = [];
function SearchByCondition(searchPram) {
    getPageList(1, searchPram);
}

function IsExistsInJsonArray(keyName, searchPram, curr, isReplace) {
    var flag = false;
    $.each(searchPram, function (index, value) {
        if (value.name == keyName) {//存在
            flag = true;
            if (isReplace)
                searchPram[index].value = curr;
            return false;
        }
    });
    return flag;
}

function getPageList(curr, searchPram) {
    var loadIndex = layer.load(1, { shade: [0.5, '#393D49'] }); //0代表加载的风格，支持0-2
    //分页参数，需要传到后台的
    if (!IsExistsInJsonArray('pageIndex', searchPram, curr, true))
        searchPram.push({ name: "pageIndex", value: curr });
    if (!IsExistsInJsonArray('pageSize', searchPram, curr, false))
        searchPram.push({ name: "pageSize", value: pageSize });
    //ajax获取
    $.ajax({
        type: "post", cache: false, url: tUrl, dataType: "json", data: searchPram, success: function (res) {
            var gettpl = document.getElementById('demo').innerHTML;
            laytpl(gettpl).render(res.Rows, function (html) {
                $('#layerAppendView').html(html);
            });
            var pages = Math.ceil(res.Total / pageSize); //得到总页数
             {
                //显示分页
                laypage({
                    cont: 'pageContent', //容器。值支持id名、原生dom对象，jquery对象。【如该容器为】：<div id="page1"></div>
                    pages: pages, //通过后台拿到的总页数
                    skin: 'molv', //皮肤
                    curr: curr || 1, //当前页
                    jump: function (obj, first) { //触发分页后的回调
                        if (!first) { //点击跳页触发函数自身，并传递当前页：obj.curr
                            getPageList(obj.curr, searchPram);
                        }
                    }
                });
            }
        }
    });
    layer.close(loadIndex);
}


function formatDate(date, format) {
    if (!date) return;
    if (!format) format = "yyyy-MM-dd";
    switch (typeof date) {
        case "string":
            date = new Date(date.replace(/-/, "/"));
            break;
        case "number":
            date = new Date(date);
            break;
    }
    if (!date instanceof Date) return;
    var dict = {
        "yyyy": date.getFullYear(),
        "M": date.getMonth() + 1,
        "d": date.getDate(),
        "H": date.getHours(),
        "m": date.getMinutes(),
        "s": date.getSeconds(),
        "MM": ("" + (date.getMonth() + 101)).substr(1),
        "dd": ("" + (date.getDate() + 100)).substr(1),
        "HH": ("" + (date.getHours() + 100)).substr(1),
        "mm": ("" + (date.getMinutes() + 100)).substr(1),
        "ss": ("" + (date.getSeconds() + 100)).substr(1)
    };
    return format.replace(/(yyyy|MM?|dd?|HH?|ss?|mm?)/g, function () {
        return dict[arguments[0]];
    });
}

