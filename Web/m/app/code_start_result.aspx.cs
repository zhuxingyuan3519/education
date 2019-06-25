using DBUtility;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ThoughtWorks.QRCode.Codec;

namespace Web.m.app
{
    /// <summary>
    /// 答题结果页面
    /// </summary>
    public partial class code_start_result: System.Web.UI.Page
    {
        protected string ErrMsg = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                //获取到traincode
                string traincode = Request.QueryString["traincode"];
                T_TrainHeader header = CommonBase.GetModel<T_TrainHeader>(traincode);
                if(header != null)
                {
                    if(header.CodeType == "1")
                        sp_trainName.InnerHtml = "速记基础训练";
                    else if(header.CodeType == "2")
                        sp_trainName.InnerHtml = "数字训练";
                    else if(header.CodeType == "5")
                        sp_trainName.InnerHtml = "速记高手训练";
                    else if(header.CodeType == "4")
                        sp_trainName.InnerHtml = "字母训练";
                    else if(header.CodeType == "3")
                        sp_trainName.InnerHtml = "扑克牌训练";
                    sp_trainTime.InnerHtml = header.BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                    sp_trainCount.InnerHtml = header.TrainCount.ToString();
                    //复习时间，秒要转换成时分秒


                    TimeSpan mskts = new TimeSpan(0, 0, header.ReviewTime);
                    string str = "";
                    str = (mskts.Hours > 0 ? mskts.Hours : 0) + "时 " + (mskts.Minutes > 0 ? mskts.Minutes : 0) + "分 " + mskts.Seconds + "秒";
                    sp_reviewTime.InnerHtml = str;

                    //记忆时间
                    DateTime memoryEndTime = MethodHelper.ConvertHelper.ToDateTime(header.EndTime, header.BeginTime);
                    TimeSpan EndDateTimeSpan = new TimeSpan(memoryEndTime.Ticks);
                    TimeSpan BeginDateTimeSpan = new TimeSpan(header.BeginTime.Ticks);
                    TimeSpan ts = EndDateTimeSpan.Subtract(BeginDateTimeSpan);
                    sp_memoryTime.InnerHtml = (ts.Hours < 10 ? ("0" + ts.Hours) : ts.Hours.ToString()) + ":" + (ts.Minutes < 10 ? ("0" + ts.Minutes) : ts.Minutes.ToString()) + ":" + (ts.Seconds < 10 ? ("0" + ts.Seconds) : ts.Seconds.ToString());
                    //答题时间
                    DateTime answerBeginTime = MethodHelper.ConvertHelper.ToDateTime(header.AnswerBeginTime, header.BeginTime);
                    DateTime answerEndTime = MethodHelper.ConvertHelper.ToDateTime(header.AnswerEndTime, header.BeginTime);

                    EndDateTimeSpan = new TimeSpan(answerEndTime.Ticks);
                    BeginDateTimeSpan = new TimeSpan(answerBeginTime.Ticks);
                    ts = EndDateTimeSpan.Subtract(BeginDateTimeSpan);
                    sp_answerTime.InnerHtml = (ts.Hours < 10 ? ("0" + ts.Hours) : ts.Hours.ToString()) + ":" + (ts.Minutes < 10 ? ("0" + ts.Minutes) : ts.Minutes.ToString()) + ":" + (ts.Seconds < 10 ? ("0" + ts.Seconds) : ts.Seconds.ToString());





                    hid_trainCode.Value = header.Code;
                    //CommonBase.Update<T_TrainHeader>(header, new string[] { "AnswerBeginTime" });
                    int count;
                    DataTable dt = CommonBase.GetPageDataTable(1, 10000, "TrainCode='" + header.Code + "'", "5 SplicCount,CodeName,Sort,Status", "Sort asc", "T_TrainDetail", out count);

                    int correctCount = dt.Select("Status=2").Length;
                    int errorCount = dt.Select("Status=3 or Status=1").Length;

                    sp_correctCount.InnerHtml = correctCount.ToString();
                    sp_errorCount.InnerHtml = errorCount.ToString();
                    //计算正确率
                    decimal per = (decimal)correctCount / (decimal)header.TrainCount;
                    sp_correctPercent.InnerHtml = (per * 100).ToString("F2") + "%";

                    if(header.CodeType == "3")
                    {
                        //扑克牌的显示方式不一样
                        foreach(DataRow row in dt.Rows)
                        {
                            row["SplicCount"] = "10";
                            string codeName = row["CodeName"].ToString();
                            //设置花色
                            string pkType = codeName.Substring(0, 1);
                            string pkImge = "";
                            switch(pkType)
                            {
                                case "1": pkImge = "<b style='color:black'>♠</b>"; break;
                                case "2": pkImge = "<b style='color:red'>♥</b>"; break;
                                case "3": pkImge = "<b style='color:black'>♣</b>"; break;
                                case "4": pkImge = "<b style='color:red'>♦</b>"; break;
                            }
                            string pkNum = codeName.Substring(1, codeName.Length - 1);
                            string correctType = "×";
                            if(row["Status"].ToString() == "2")
                                correctType = "✓";
                            else
                                correctType = "×";
                            row["CodeName"] = pkImge + "<br/>" + pkNum + "<br/>" + correctType;
                        }
                    }



                    rep_list.DataSource = dt;
                    rep_list.DataBind();
                }
                else
                {
                    ErrMsg = "初始化记忆结果失败";
                }
            }
        }
    }
}