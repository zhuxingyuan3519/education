
using DBUtility;
using MethodHelper;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.TrainManage
{
    public partial class EditWord: BasePage
    {
        protected override void SetPowerZone()
        {
            //GetDictBindDDL("BookVersion", ddl_Version);
            //ddl_Version.Items.Insert(0, new ListItem("--请选择版本--", ""));

            //GetDictBindDDL("Grade", ddl_Grade);
            //ddl_Grade.Items.Insert(0, new ListItem("--请选择年级--", ""));

            //GetDictBindDDL("Leavel", ddl_Leavel);
            //ddl_Leavel.Items.Insert(0, new ListItem("--请选择学期--", ""));
        }
        protected override void SetValue(string code)
        {
            T_Words word = CommonBase.GetModel<T_Words>(code);
            if(word != null)
            {
                hidId.Value = word.Code;
                txt_Association1.Value = word.Association1;
                txt_Association2.Value = word.Association2;
                txt_Association3.Value = word.Association3;
                txt_Association4.Value = word.Association4;
                txt_Chinese.Value = word.Chinese;
                txt_English.Value = word.English;
                txt_Module1.Value = word.Module1;
                txt_Module2.Value = word.Module2;
                txt_Module3.Value = word.Module3;
                txt_Module4.Value = word.Module4;
                txt_Phonetic.Value = word.Phonetic;
            }
        }

        protected T_Words GetModel(T_Words word)
        {
            word.Association1 = Request.Form["txt_Association1"];
            word.Association2 = Request.Form["txt_Association2"];
            word.Association3 = Request.Form["txt_Association3"];
            word.Association4 = Request.Form["txt_Association4"];
            word.Chinese = Request.Form["txt_Chinese"];
            word.English = Request.Form["txt_English"];
            string wordSigle = word.English;
            if(wordSigle.IndexOf("'") > 0)
            {
                wordSigle = word.English.Replace("'", "@");
            }
            word.English = wordSigle;

            word.Module1 = Request.Form["txt_Module1"];
            word.Module2 = Request.Form["txt_Module2"];
            word.Module3 = Request.Form["txt_Module3"];
            word.Module4 = Request.Form["txt_Module4"];
            word.Phonetic = Request.Form["txt_Phonetic"];
            return word;
        }
        protected override string btnAdd_Click()
        {
            string id = Request.Form["hidId"];
            T_Words word;
            List<CommonObject> listCommon = new List<CommonObject>();
            if(string.IsNullOrEmpty(id))
            {
                word = new T_Words();
                word = GetModel(word);
                word.CreatedTime = DateTime.Now;
                word.Sort = 1;
                word.Status = 1;
                word.IsDeleted = false;
                CommonBase.Insert<T_Words>(word, listCommon);
            }
            else
            {
                word = CommonBase.GetModel<T_Words>(id);
                word = GetModel(word);
                CommonBase.Update<T_Words>(word, listCommon);
            }
            if(CommonBase.RunListCommit(listCommon))
                return CommonHelper.Response(true, word.Code);
            else
            {
                return CommonHelper.Response(false, "操作失败，请重试！");
            }
        }

    }
}