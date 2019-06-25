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
    /// 开始答题页面
    /// </summary>
    public partial class exam_version: BasePage
    {
        protected string ErrMsg = string.Empty;
        protected override void SetPowerZone()
        {
            GetDictBindDDL("BookVersion", ddl_version);
            ddl_version.Items.Insert(0, new ListItem("--请选择版本--", ""));

            GetDictBindDDL("Grade", ddl_grade);
            ddl_grade.Items.Insert(0, new ListItem("--请选择年级--", ""));

            GetDictBindDDL("Leavel", ddl_leavel);
            ddl_leavel.Items.Insert(0, new ListItem("--请选择学期--", ""));

            GetDictBindDDL("Unit", ddl_unit);
            ddl_unit.Items.Insert(0, new ListItem("--请选择章节--", ""));
        }
        protected override string btnAdd_Click()
        {
            List<CommonObject> listComm = new List<CommonObject>();
            string version = Request.Form[NameHeader + "ddl_version"];
            string grade = Request.Form[NameHeader + "ddl_grade"];
            string leavel = Request.Form[NameHeader + "ddl_leavel"];
            string unit = Request.Form[NameHeader + "ddl_unit"];
            string word = Request.Form[NameHeader + "txt_writeContent"];
            //英文字母拼写，单词与单词之间用“，”隔开
            if(!string.IsNullOrEmpty(word))
            {
                string[] array = word.Trim().Split(new char[] { '，', ',' });
                //获取到最大的顺序
                string sql_num = "SELECT ISNULL(MAX(CONVERT(INT, t1.WIndex)),0) FROM dbo.T_VersionVsWords t1  WHERE  t1.Version='" + version + "' AND t1.Grade='" + grade + "' AND t1.Leavel='" + leavel + "' AND t1.Unit='" + unit + "'";
                sql_num = "SELECT COUNT(1) FROM dbo.T_VersionVsWords t1  WHERE  t1.Version='" + version + "' AND t1.Grade='" + grade + "' AND t1.Leavel='" + leavel + "' AND t1.Unit='" + unit + "'";
                int windex = MethodHelper.ConvertHelper.ToInt32(CommonBase.GetSingle(sql_num), 0);
                foreach(string sigle in array)
                {
                    if(!string.IsNullOrEmpty(sigle))
                    {
                        string wordSigle = sigle;
                        if(wordSigle.IndexOf("'") > 0)
                        {
                            wordSigle = sigle.Replace("'","@");
                        }
                        //查询到这个单词
                        T_Words wordModel = CommonBase.GetList<T_Words>("English='" + wordSigle.TrimStart().TrimEnd() + "'").FirstOrDefault();
                        if(wordModel == null)
                        {
                            wordModel = new T_Words();
                            wordModel.Code = GetGuid;
                            wordModel.Version = "0";//归属非基础词库
                            wordModel.English = wordSigle;
                            wordModel.IsDeleted = false;
                            wordModel.CreatedTime = DateTime.Now;
                            wordModel.Sort = 1;
                            wordModel.Status = 1;
                            CommonBase.Insert<T_Words>(wordModel, listComm);
                        }
                        //查询到这个版本对应的单词
                        T_VersionVsWords versionVsWord = CommonBase.GetList<T_VersionVsWords>("Version='" + version + "' and Grade='" + grade + "' and Leavel='" + leavel + "' and Unit='" + unit + "' and WordCode='" + wordModel.Code + "'").FirstOrDefault();
                        if(versionVsWord == null)
                        {
                            versionVsWord = new T_VersionVsWords();
                            versionVsWord.Code = GetGuid;
                            versionVsWord.CreatedTime = DateTime.Now;
                            versionVsWord.Grade = grade;
                            versionVsWord.IsDeleted = false;
                            versionVsWord.Leavel = leavel;
                            versionVsWord.Sort = 1;
                            versionVsWord.Status = 1;
                            versionVsWord.Unit =  unit;
                            versionVsWord.Version = version;
                            versionVsWord.CreatedBy = TModel.ID;
                            windex = windex+1;
                            versionVsWord.WIndex = windex.ToString();
                            versionVsWord.WordCode = wordModel.Code;
                            CommonBase.Insert<T_VersionVsWords>(versionVsWord, listComm);
                        }
                    }
                }
                if(CommonBase.RunListCommit(listComm))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            return "0";
        }
    }
}