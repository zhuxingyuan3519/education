using DBUtility;
using MethodHelper;
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
    /// 开始答题页面
    /// </summary>
    public partial class exam_select: System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if(!string.IsNullOrEmpty(Request.QueryString["Action"]))
                {
                    if(Request.QueryString["Action"].ToUpper() == "QUERY")
                    {
                        Response.Write(btnQuery_Click());
                        Response.End();
                    }
                    //else if(Request.QueryString["Action"].ToUpper() == "GET")
                    //{
                    //    Response.Write(GetMtjMid());
                    //    Response.End();
                    //}
                }


                string evaluationHeaderCode = Request.QueryString["code"];
                if(!string.IsNullOrEmpty(evaluationHeaderCode))
                {
                    T_EvaluationHeader header = CommonBase.GetModel<T_EvaluationHeader>(evaluationHeaderCode);
                    if(header == null)
                    {
                        Response.Write("未查询到测评信息");
                        Response.End();
                    }
                    //EvaluationHeader = header;

                    DateTime EndTime = MethodHelper.ConvertHelper.ToDateTime(header.EvalEndTime, header.EvalBeginTime);
                    TimeSpan EndDateTimeSpan = new TimeSpan(EndTime.Ticks);
                    TimeSpan BeginDateTimeSpan = new TimeSpan(header.EvalBeginTime.Ticks);
                    TimeSpan ts = EndDateTimeSpan.Subtract(BeginDateTimeSpan);
                    //AnswerTime = (ts.Hours < 10 ? ("0" + ts.Hours) : ts.Hours.ToString()) + ":" + (ts.Minutes < 10 ? ("0" + ts.Minutes) : ts.Minutes.ToString()) + ":" + (ts.Seconds < 10 ? ("0" + ts.Seconds) : ts.Seconds.ToString());



                    //string mes = (((double)EvaluationHeader.CorrectCount / EvaluationHeader.QuestionCount) * 100.00).ToString("F2") + "%";


                    //试卷信息
                    T_EvaluationPaper paper = CommonBase.GetModel<T_EvaluationPaper>(header.PaperCode);
                    if(paper != null)
                    {
                        //EvaluationPaper = paper;
                    }

                    //用户信息
                    Model.Member user = CommonBase.GetModel<Model.Member>(header.UserCode);
                    if(user != null)
                    {
                        //UserModel = user;
                    }
                }
            }
        }

        protected string btnQuery_Click()
        {
            string pageIndex = Request["pageIndex"];
            string paSize = Request["pageSize"];
            string mname = Request["nMName"];
            string tel = Request["nTel"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);

            string strWhere = "1=1 ";
            if(Session["Member"] != null)
            {
                Model.Member TModel = Session["Member"] as Model.Member;
                strWhere += " and t1.UserCode='" + TModel.ID + "'";
            }
            else
            {
                strWhere += " and t3.MName='" + mname + "' and (t3.FatherTel='" + tel + "' or t3.MatherTel='" + tel + "' or t3.OtherTel='" + tel + "')";
            }

            string evaluationCode = Request["evaluationCode"];

            string tables = "dbo.T_EvaluationHeader t1 LEFT JOIN dbo.T_EvaluationPaper t2 ON t1.PaperCode=t2.Code LEFT JOIN  dbo.Member t3 ON t1.UserCode=t3.ID";
            string fields = "t1.Code,t1.EvalBeginTime,t1.EvalEndTime,t2.Name AS PaperName,t3.ID AS UserCode,t3.MID,t3.MName";
            string orderBy = "t1.EvalBeginTime DESC";
            DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, strWhere, fields, orderBy, tables);
            return JsonHelper.DataSetToJson(dt);
        }


    }
}