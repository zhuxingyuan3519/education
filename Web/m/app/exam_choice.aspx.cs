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
    public partial class exam_choice : BasePage
    {
        protected string ErrMsg = string.Empty;
        protected override void SetPowerZone()
        {
            GetDictBindDDL("BookVersion", ddl_version);
            ddl_version.Items.Insert(0, new ListItem("--教材版本--", ""));

            GetDictBindDDL("Grade", ddl_grade);
            ddl_grade.Items.Insert(0, new ListItem("--年级--", ""));

            GetDictBindDDL("Leavel", ddl_leavel);
            ddl_leavel.Items.Insert(0, new ListItem("--学期--", ""));

            //GetDictBindDDL("Unit", ddl_unit);
            //ddl_unit.Items.Insert(0, new ListItem("--章节--", ""));

            //绑定测评试卷
            List<T_EvaluationPaper> listPaper= CommonBase.GetList<T_EvaluationPaper>("IsDeleted=0 order by  CreatedTime desc");
            ddl_paperList.DataSource = listPaper;
            ddl_paperList.DataTextField = "Name";
            ddl_paperList.DataValueField = "Code";
            ddl_paperList.DataBind();
            ddl_paperList.Items.Insert(0, new ListItem("--请选择试卷--", ""));
        }
        protected override string btnAdd_Click()
        {
            string version = Request.Form[NameHeader + "ddl_version"];
            string grade = Request.Form[NameHeader + "ddl_grade"];
            string leavel = Request.Form[NameHeader + "ddl_leavel"];
            string unit = Request.Form[NameHeader + "ddl_unit"];
            //查看所选择项目后台是否配置了试卷
            T_EvaluationPaper paper = CommonBase.GetList<T_EvaluationPaper>("Version='" + version + "' and Grade='" + grade + "' and Leavel='" + leavel + "' and Unit='" + unit + "'").FirstOrDefault();
            if (paper == null)
                return CommonHelper.Response(false, "操作失败：您选择的测评范围没有找到对应的测评试卷！");
            return CommonHelper.Response(true, paper.Code + "$" + paper.Name);
        }


        protected override string btnQuery_Click()
        {
            string pageIndex = Request["pageIndex"];
            string paSize = Request["pageSize"];
            string mname = Request["nMName"];
            string tel = Request["nTel"];
            int currentIndex = int.Parse(pageIndex);
            int pageSize = int.Parse(paSize);

            string strWhere = "1=1 ";
            if (Session["Member"] != null)
            {
                Model.Member TModel = Session["Member"] as Model.Member;
                strWhere += " and t1.UserCode='" + TModel.ID + "'";
            }
            else
            {
                strWhere += " and (t3.MName='" + tel + "' or t3.FatherTel='" + tel + "' or t3.MatherTel='" + tel + "' or t3.OtherTel='" + tel + "')";
            }

            string evaluationCode = Request["evaluationCode"];

            string tables = "dbo.T_EvaluationHeader t1  LEFT JOIN  dbo.Member t3 ON t1.UserCode=t3.ID";
            string fields = "t1.Code,t1.EvalBeginTime,t1.EvalEndTime,t1.PaperCode AS PaperName,t3.ID AS UserCode,t3.MID,t3.MName";
            string orderBy = "t1.EvalBeginTime DESC";
            DataSet dt = CommonBase.GetPageList(currentIndex, pageSize, strWhere, fields, orderBy, tables);
            return JsonHelper.DataSetToJson(dt);
        }


    

    }
}