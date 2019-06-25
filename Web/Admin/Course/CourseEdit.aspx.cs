
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

namespace Web.Admin.Course
{
    public partial class CourseEdit: BasePage
    {
        protected string txtNContent = "";
        protected override void SetPowerZone()
        {

        }
        protected override void SetValue(string code)
        {
            Model.EN_Course model = CommonBase.GetModel<Model.EN_Course>(code);
            if(model != null)
            {
                txt_Name.Value = model.Name;
                txt_Fee.Value = model.Fee.ToString();
                hidId.Value = model.Code;
                ddl_Leavel.Value = model.Leavel;
                txtNContent = model.Remark;
                txt_Title.Value = model.Title;
            }

        }

        protected Model.EN_Course GetModel(Model.EN_Course model)
        {
            model.Name = Request.Form["txt_Name"];
            model.Fee = MethodHelper.ConvertHelper.ToDecimal(Request.Form["txt_Fee"], 0);
            model.Leavel = Request.Form["ddl_Leavel"];
             model.Title = Request.Form["txt_Title"];
            model.Remark= HttpUtility.UrlDecode(Request.Form["hdContent"]);
            return model;
        }
        protected override string btnAdd_Click()
        {
            try
            {
                List<CommonObject> listComm = new List<CommonObject>();
                if(!string.IsNullOrEmpty(Request["hidId"]))
                { //修改
                    Model.EN_Course model = CommonBase.GetModel<Model.EN_Course>(Request.Form["hidId"]);
                    if(model != null)
                    {
                        model = GetModel(model);
                        CommonBase.Update<Model.EN_Course>(model, listComm);
                        LogService.Log(TModel, "2", TModel.MID + "修改课程" + model.Name, listComm);
                        if(CommonBase.RunListCommit(listComm))
                            return CommonHelper.Response(true, "操作成功！");
                        else
                            return CommonHelper.Response(true, "操作失败，请重试！");
                    }
                }
                else
                {
                    Model.EN_Course model = new Model.EN_Course();
                    model = GetModel(model);
                    //创建课程序号
                    string sql = "SELECT CONVERT(INT,MAX(Code)) FROM dbo.EN_Course";
                    object obj = CommonBase.GetSingle(sql);
                    int codeOrder = Convert.ToInt32(obj) + 1;
                    model.Sort = codeOrder;
                    model.IsDeleted = false;
                    model.Status = 1;
                    string code = "";
                    if(codeOrder < 10)
                        code = "000" + codeOrder;
                    if(codeOrder > 10 && codeOrder < 100)
                        code = "00" + codeOrder;
                    model.Code = code;

                    CommonBase.Insert<Model.EN_Course>(model, listComm);

                    LogService.Log(TModel, "2", TModel.MID + "创建课程" + model.Name, listComm);
                    if(CommonBase.RunListCommit(listComm))
                        return CommonHelper.Response(true, model.Code);
                    else
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