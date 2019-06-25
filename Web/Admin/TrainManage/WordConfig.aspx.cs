
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
    public partial class WordConfig: BasePage
    {
        protected Model.EN_Video tempModel = null;
        protected override void SetPowerZone()
        {
            string status = Request.QueryString["status"];
            if(!string.IsNullOrEmpty(status))
            {
                hidStatus.Value = status;
                List<T_CodingWord> listWords = CommonBase.GetList<T_CodingWord>("IsDeleted=0 and Status=" + status + " order by Sort");
                string contents = string.Empty;
                foreach(T_CodingWord word in listWords)
                {
                    if(string.IsNullOrEmpty(contents))
                        contents += word.CodeWord;
                    else
                        contents += " " + word.CodeWord;
                }
                txt_words.Value = contents;
            }
        }
        protected string[] DelRepeatData(string[] a)
        {
            return a.GroupBy(p => p).Select(p => p.Key).ToArray();
        }

        protected override string btnAdd_Click()
        {
            try
            {
                List<CommonObject> listComm = new List<CommonObject>();
                string words = Request.Form["txt_words"];
                string status = Request.Form["hidStatus"];
                //删除所有的词组
                listComm.Add(new CommonObject(" delete from dbo.T_CodingWord where Status=" + status, null));
                if(!string.IsNullOrEmpty(words))
                {
                    string[] array = words.Split(' ');
                    array = array.Where(c => !string.IsNullOrEmpty(c)).ToArray();
                    array = array.Where(c => c != "\n").ToArray();
                    //再一次去重复
                    array = DelRepeatData(array);
                    //array = array.Where((x, i) => array.FindIndex(z => z. == x.name) == i);

                    int i = 1;
                    foreach(string str in array)
                    {
                        if(!string.IsNullOrEmpty(str))
                        {
                            T_CodingWord word = new T_CodingWord();
                            word.Code = GetGuid;
                            word.CodeWord = str;
                            word.CreatedTime = DateTime.Now;
                            word.IsDeleted = false;
                            word.Sort = i;
                            word.Status = int.Parse(status);
                            CommonBase.Insert<T_CodingWord>(word, listComm);
                            i++;
                        }
                    }
                }
                if(CommonBase.RunListCommit(listComm))
                {
                    return CommonHelper.Response(true, "操作成功");
                }
                else
                {
                    return CommonHelper.Response(false, "操作失败，请联系管理员");
                }
            }
            catch(Exception e)
            {
                return CommonHelper.Response(false, "操作失败，请联系管理员");
            }
            return CommonHelper.Response(false, "操作失败，请联系管理员");
        }
    }
}