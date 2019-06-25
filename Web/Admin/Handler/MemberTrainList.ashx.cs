using DBUtility;
using MethodHelper;
using Model;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Web.Admin.Handler
{
    /// <summary>
    /// FHLogList 的摘要说明
    /// </summary>
    public class MemberTrainList: BaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string strWhere = "'1'='1' and t1.IsDeleted=0 and t1.EndTime <>'' and t1.AnswerBeginTime <>'' and t1.AnswerEndTime <>'' ";

            if(!string.IsNullOrEmpty(context.Request["nRegistBeginTime"]))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(context.Request["nRegistBeginTime"]) + " 00:00:00", out time);
                strWhere += " and t1.BeginTime>='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if(!string.IsNullOrEmpty(context.Request["nRegistEndTime"]))
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(HttpUtility.UrlDecode(context.Request["nRegistEndTime"]) + " 23:59:59", out time);
                strWhere += " and t1.BeginTime<='" + time.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            if(!string.IsNullOrEmpty(context.Request["hidType"])) //是否是查询奖金池分红
            {
                strWhere += " and t1.CodeType='" + context.Request["hidType"] + "'";
            }
            if(!string.IsNullOrEmpty(context.Request["nName"])) //会员昵称
            {
                strWhere += " and t2.MName like '%" + context.Request["nName"] + "%'";
            }
            if(!string.IsNullOrEmpty(context.Request["nTitle"])) //会员昵称
            {
                strWhere += " and t2.MID like '%" + context.Request["nTitle"] + "%'";
            }
            int count;

            string export = context.Request["export"];
            if(export == "1")
            {
                pageIndex = 1;
                pageSize = int.MaxValue;
            }
            DataTable dt = CommonBase.GetPageDataTable(pageIndex, pageSize, strWhere, "t1.*,t2.MName,t2.MID", "t1.BeginTime DESC", "T_TrainHeader t1 left join Member t2 on t1.UserCode=t2.ID", out count);
            dt.Columns.Add("CutTime", typeof(string));
            dt.Columns.Add("ReviewTimeString", typeof(string));
            dt.Columns.Add("MemoryTimeString", typeof(string));
            dt.Columns.Add("AnswerTimeString", typeof(string));
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                switch(dt.Rows[i]["CodeType"].ToString())
                {
                    case "1": dt.Rows[i]["CodeType"] = "初级混合词训练"; break;
                    case "2": dt.Rows[i]["CodeType"] = "数字训练"; break;
                    case "3": dt.Rows[i]["CodeType"] = "扑克牌训练"; break;
                    case "4": dt.Rows[i]["CodeType"] = "字母训练"; break;
                    case "5": dt.Rows[i]["CodeType"] = "高级混合词训练"; break;
                }
                dt.Rows[i]["CutTime"] = Convert.ToDateTime(dt.Rows[i]["BeginTime"]).ToString("yyyy/MM/dd HH:mm");

                TimeSpan mskts = new TimeSpan(0, 0, Convert.ToInt32(dt.Rows[i]["ReviewTime"]));
                string str = "";
                str = (mskts.Hours > 0 ? mskts.Hours : 0) + "时 " + (mskts.Minutes > 0 ? mskts.Minutes : 0) + "分 " + mskts.Seconds + "秒";

                dt.Rows[i]["ReviewTimeString"] = str;

                //记忆时间
                DateTime memoryEndTime = MethodHelper.ConvertHelper.ToDateTime(dt.Rows[i]["EndTime"], Convert.ToDateTime(dt.Rows[i]["BeginTime"]));
                TimeSpan EndDateTimeSpan = new TimeSpan(memoryEndTime.Ticks);
                TimeSpan BeginDateTimeSpan = new TimeSpan(Convert.ToDateTime(dt.Rows[i]["BeginTime"]).Ticks);
                TimeSpan ts = EndDateTimeSpan.Subtract(BeginDateTimeSpan);

                dt.Rows[i]["MemoryTimeString"] = (ts.Hours < 10 ? ("0" + ts.Hours) : ts.Hours.ToString()) + ":" + (ts.Minutes < 10 ? ("0" + ts.Minutes) : ts.Minutes.ToString()) + ":" + (ts.Seconds < 10 ? ("0" + ts.Seconds) : ts.Seconds.ToString());
                //答题时间
                DateTime answerBeginTime = MethodHelper.ConvertHelper.ToDateTime(dt.Rows[i]["AnswerBeginTime"], Convert.ToDateTime(dt.Rows[i]["BeginTime"]));
                DateTime answerEndTime = MethodHelper.ConvertHelper.ToDateTime(dt.Rows[i]["AnswerEndTime"], Convert.ToDateTime(dt.Rows[i]["BeginTime"]));

                TimeSpan EndDateTimeSpan2 = new TimeSpan(answerEndTime.Ticks);
                TimeSpan BeginDateTimeSpan2 = new TimeSpan(answerBeginTime.Ticks);
                TimeSpan ts2 = EndDateTimeSpan2.Subtract(BeginDateTimeSpan2);
                dt.Rows[i]["AnswerTimeString"] = (ts2.Hours < 10 ? ("0" + ts2.Hours) : ts2.Hours.ToString()) + ":" + (ts2.Minutes < 10 ? ("0" + ts2.Minutes) : ts2.Minutes.ToString()) + ":" + (ts2.Seconds < 10 ? ("0" + ts2.Seconds) : ts2.Seconds.ToString());

                //计算正确率
                decimal per = MethodHelper.ConvertHelper.ToDecimal(dt.Rows[i]["CorrectCount"], 0) / MethodHelper.ConvertHelper.ToDecimal(dt.Rows[i]["TrainCount"], 1);// (decimal)dt.Rows[i]["TrainCount"];
                dt.Rows[i]["Remark"] = (per * 100).ToString("F2") + "%";

            }
            //是否导出excel
            if(export == "1")
            {
                Dictionary<string, string> listFields = new Dictionary<string, string>();
                listFields.Add("RowNumber", "序号");
                listFields.Add("FHMCode", "账号");
                listFields.Add("MName", "姓名");
                listFields.Add("FHMoney", "奖励金额");
                listFields.Add("CutTime", "奖励时间");
                listFields.Add("FHType", "支付方式");
                listFields.Add("Remark", "奖励事由");
                ExportExcel(context, "用户奖励列表", dt, listFields);
            }
            else
                context.Response.Write(JsonHelper.GetAdminDataTableToJson(dt, count));

        }


    }
}