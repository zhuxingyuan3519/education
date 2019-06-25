//============================== 定义属性 ==========
var calendarDisplay = false; //是否显示
var yearBegin = 2015; //开始时间
var yearEnd = 2999; //截至年
var monthEnd = 12; //截至月
var dayEnd = 31; //截至日
var dayStrDef = "<table class=\"calendar\" width='100%' cellpadding='0' border='1' bordercolorlight='#C0C0C0' bordercolordark='#C0C0C0'><tr>";
var dayStr = dayStrDef;
var yearC = getNow(1);
var monthC = getNow(2);
var dayC = getNow(3);
var dateFormat = "yyyy-MM-dd"; //自定义格式
var dateObj; //和外部的交換控件
var statStr = "";
var dateList; //日期列表
var maxLen = 200; //日期的最大長度,如果沒有指定，為100



function getNow(dateType) {
    var dateTemp = new Date();
   // alert(dateTemp.getFullYear());
    switch (dateType) {
        case 0:
            nowTemp = dateTemp.getFullYear() + "-" + (dateTemp.getMonth() + 1) + "-" + dateTemp.getDate();
            break;
        case 1:
            nowTemp = dateTemp.getFullYear();
            break;
        case 2:
            nowTemp = dateTemp.getMonth();
            break;
        case 3:
            nowTemp = dateTemp.getDate();
            break;
        case 4:
            nowTemp = dateTemp.getDay();
            break;
    }
    return nowTemp;
}


function createCalendar() {
    dayStr = dayStrDef;
    var lastDay = getLastDay(yearC, parseInt(monthC) + 1);
    var startDay = getWeekDay(yearC, parseInt(monthC) + 1, 1);
    var d = 1;
    for (w = 0; w <= lastDay + startDay - 1; w++) {
        if (w != 0 && w % 7 == 0) {
            dayStr += "</tr><tr>";
        }
        if (w >= startDay) {
            dayStr += "<td onclick='changeDateList(" + d + ");daySpace.innerHTML = createCalendar();' style='cursor:pointer' >";
            if (isInDateList(d)) {
                dayStr += "<b><font color=red>";
            }
            dayStr += d;
            d++;
        }
        else {
            dayStr += "<td>";
            dayStr += "&nbsp;";
        }
    }
    dayStr += "</tr></table>";
    return dayStr;
}

function getWeekDay(year, month, day) {
    var d = new Date();
    d.setFullYear(year);
    d.setMonth(month - 1);
    d.setDate(day);
    s = d.getDay();
    return s;
}


function getLastDay(year, month) {
    if (month < 8) {
        if (month % 2 != 0) {
            return 31;
        }
        if (month == 2) {
            if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0)) {
                return 29;
            }
            return 28;
        }
        else {
            return 30;
        }
    }
    if (month % 2 != 0) {
        return 30;
    }
    return 31;
}

function one2two(d) {
    var s = d;
    if (d < 10) {
        s = "0" + d;
    }
    return s;
}

function transferDate(day) {
    dayC = day;
    datevalue = dateFormat.replace("yyyy", yearC);
    datevalue = datevalue.replace("MM", one2two(parseInt(monthC) + 1));
    datevalue = datevalue.replace("dd", one2two(dayC));
    return datevalue;
}

function selectCalendar(dateType, datevalue) {
    var d = new Date(yearC, monthC, dayC);
    if (dateType == "year") {
        year = parseInt(yearC) + datevalue;
        if (year >= yearEnd) {
            year = yearEnd;
            if (monthC > monthEnd) {
                d.setMonth(monthEnd);
            }
        }
        if (year < yearBegin) {
            year = yearBegin;
        }
        d.setFullYear(year);
    }
    if (dateType == "month") {
        month = parseInt(monthC) + datevalue;
        if (yearC >= yearEnd && month > monthEnd) {
            month = monthEnd;
            calendarState.innerHTML = "Max month";
        }
        if (yearC == yearBegin && month < 0) {
            month = 0;
            calendarState.innerHTML = "Min month";
        }
        d.setDate(1);
        d.setMonth(month);
    }
    yearC = d.getFullYear();
    monthC = d.getMonth();
    setCalendarDef();
    daySpace.innerHTML = createCalendar();
}

function initDateList(dateObj) {
    var str = dateObj.value;
    if (str.trim() == "") {
        dateList = new Array();
    } else {
        dateList = str.split(";");
    }
    checkLength();
}

function changeDateList(day) {
    var str1 = transferDate(day);
    for (i = 0; i < dateList.length; i++) {
        if (dateList[i] == str1) {
            dateList.splice(i, 1);
            saveDateList();
            return;
        }
    }
    if (checkLength()) {
        dateList[dateList.length] = str1;
        saveDateList();
        return;
    } else if (maxLen == 1) {
        dateList[0] = str1;
        saveDateList();
        return;
    }
}

function checkLength() {
    if (dateList.length >= maxLen) {
        calendarState.innerHTML = "too long";
        return false;
    }
    return true;
}

function saveDateList() {
    dateObj.value = dateList.join(";");
}

function isInDateList(day) {
    var str1 = transferDate(day);
    for (i = 0; i < dateList.length; i++) {
        if (dateList[i] == str1) {
            return true;
        }
    }
    return false;
}

function calendarShow(calendarObj) {
    if (calendarDisplay) {
        calendar.style.display = "none";
        dayStr = dayStrDef;
        return;
    }
    else {
        dateObj = calendarObj;
        calendar.style.display = "block";
        initDateList(dateObj);
    }
    createSelect(document.all.calendarYear, yearBegin, yearEnd, "year");
    createSelect(document.all.calendarMonth, 1, 12, "month");
    setCalendarDef();
    objL = document.body.scrollLeft + window.event.x - 10;
    objT = calendarObj.getBoundingClientRect().top + calendarObj.clientHeight;
    calendar.style.left = objL - 2;
    calendar.style.top = objT + 1;
    setCalendarvalue();
    daySpace.innerHTML = createCalendar();
    var calendarDiv = document.getElementById('calendar');
//    GetPosition(calendarObj);
//    alert("left:" + calendarObj.left + ",Top:" + calendarObj.top);
//    alert("left:"+calendarObj.offsetLeft + ",Top:" + calendarObj.offsetTop);
     calendarDiv.style.left = "25%";
     calendarDiv.style.top = "60%";
}
function calendarShow1(calendarObj) {
    maxLen = 1;
    calendarShow(calendarObj);
}
function calendarShowN(calendarObj, maxLength) {
    maxLen = maxLength;
    calendarShow(calendarObj);

}

function createSelect(selectObj, begin, end, selectType) {
    for (i = begin; i <= end; i++) {
        value = i;
        if (selectType == "month") {
            var value = i - 1;
        }
        selectObj.options[i - begin] = new Option(i, value);
    }
}


function defSelect(selectObj, defvalue) {
    for (i = 0; i < selectObj.length; i++) {
        if (selectObj.options[i].value == defvalue) {
            selectObj.options[i].selected = true;
            return;
        }
    }
}


function setCalendarvalue() {
    yearC = document.all.calendarYear.value;
    monthC = document.all.calendarMonth.value;
    daySpace.innerHTML = createCalendar();
}

//================================================== Validate =====
String.prototype.trim = function () { //String's Trim()
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

function isNull(strTemp) { //判断是否为空
    if (strTemp == null || strTemp.trim() == "") {
        return true;
    }
    return false;
}

//=============================================================
//================================================ Main() =====
function setCalendarDef() {
    defSelect(document.all.calendarYear, yearC);
    defSelect(document.all.calendarMonth, monthC);
}

function calendarHidden() {
    if (calendarDisplay) {
        calendarShow();
        calendarDisplay = false;
    }
    else if (calendar.style.display == "block") {
        calendarDisplay = true;
    }
}
document.onclick = calendarHidden;
cStr = "<style>.calendar {border-collapse: collapse;text-align:center}td {font-size:9pt;fontFamily=arial,sans-serif;} .title01 {background-color:#008080;color:#FFFFFF;} input {font-size:9pt;fontFamily=arial,sans-serif;}select {font-size:9pt;font-family:arial,sans-serif;}.mouseOver {background-color: #e6e7e8;}.mouseOut {background-color: #ffffff;} .calendarSelect{height:30px;}</style>";
cStr += "<div onclick='calendarDisplay=false' id='calendar' Author='smart' style=\"background-color=#ffffff; display:none;position: absolute !important;z-index:999;filter :\'progid:DXImageTransform.Microsoft.Shadow(direction=135,color=#787878,strength=5)\'\">";
cStr += "<table style='background-color:rgb(162, 193, 243)' class=\"calendar\" cellpadding='0' border='1' width='300' bordercolorlight='#000000' bordercolordark='#000000'>";
cStr += "<tr><td colspan='7' bgcolor='#E1E1E1'>";
cStr += "<span style='cursor:pointer' onclick='selectCalendar(\"year\",-1)' onMouseOut=\"calendarState.innerHTML=statStr\">&lt;&lt;</span>&nbsp; <span style=\"cursor:pointer\" onclick=\"selectCalendar('month',-1)\" onMouseOut=\"calendarState.innerHTML=statStr\">&lt;</span>";
cStr += "&nbsp;<select name=\"calendarYear\" onChange=\"setCalendarvalue()\" class='calendarSelect'></select><select  class='calendarSelect' name=\"calendarMonth\" onChange=\"setCalendarvalue()\"></select>&nbsp;";
cStr += "<span style=\"cursor:pointer\" onclick=\"selectCalendar('month',1)\" onMouseOut=\"calendarState.innerHTML=statStr\">&gt;</span>&nbsp; <span style=\"cursor:pointer\" onclick=\"selectCalendar('year',1)\" onMouseOut=\"calendarState.innerHTML=statStr\">&gt;&gt;</span></td>";
cStr += "</tr><tr class=\"title01\"><td>日</td><td>一</td><td>二</td><td>三</td><td>四</td><td>五</td><td>六</td></tr>";
cStr += "<tr><td colspan=\"7\"><div id=\"daySpace\"></div></td></tr>";
cStr += "<tr><td colspan=\"7\" bgcolor=\"#E1E1E1\"><table width=\"100%\" cellpadding='0'><tr><td width=\"60%\">";
cStr += "<div style=\"font-family:Arial;font-size:8pt\" id=\"calendarState\" onDblclick=\"calendarState.innerHTML=''\" onMouseOut=\"calendarState.innerHTML='" + statStr + "'\">" + statStr + "</div>";
cStr += "</td><td><span style=\"font-family:Arial;font-size:8pt;color:ff0000;cursor:pointer\" onclick=\"saveDateList();calendarHidden();\">[确定]</span><span style=\"font-family:Arial;font-size:8pt;color:ff0000;cursor:pointer\" onclick=\"calendarHidden()\">[关闭]</span></td></tr></table></td></tr></table></div>";
document.write(cStr);



 function GetPosition(obj)
 {
  var left = 0;
  var top   = 0;
  while(obj != document.body)
  {
        left = obj.offsetLeft;
        top   = obj.offsetTop;
        obj = obj.offsetParent;
  }
  alert("Left Is : " + left + "\r\n" + "Top   Is : " + top);
 }