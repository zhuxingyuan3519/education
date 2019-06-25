
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
    public partial class TrainEdit: BasePage
    {
        protected Model.EN_Video tempModel = null;
        protected override void SetPowerZone()
        {

        }
        protected override void SetValue(string code)
        {
            ddl_codeType.Value = code;
            //根据类型获取到列表
            rep_list.DataSource = CommonBase.GetList<T_CodingMemory>("CodeType='" + code + "' and IsDeleted=0 order by Sort");
            rep_list.DataBind();
        }
        protected Model.EN_Video GetModel(Model.EN_Video model)
        {
            model.Name = Request.Form["hid_realName"];
            model.Title = Request.Form["txt_Name"];
            model.Path = Request.Form["hid_uploadName"];
            model.Format = Request.Form["hid_fileType"];
            model.Size = MethodHelper.ConvertHelper.ToDecimal(Request.Form["hid_fileSize"], 0);
            model.Remark = Request.Form["txt_remark"];
            model.CoverImage = Request.Form["hid_coverImg"];
            model.Authority = Request.Form["chk_privage"];
            return model;
        }
        protected override string btnAdd_Click()
        {
            try
            {
                List<CommonObject> listComm = new List<CommonObject>();
                if(!string.IsNullOrEmpty(Request["hidId"]))
                { //修改
                    Model.EN_Video model = CommonBase.GetModel<Model.EN_Video>(Request.Form["hidId"]);
                    if(model != null)
                    {
                        model = GetModel(model);
                        CommonBase.Update<Model.EN_Video>(model, listComm);
                        if(CommonBase.RunListCommit(listComm))
                            return CommonHelper.Response(true, "操作成功！");
                        else
                            return CommonHelper.Response(true, "操作失败，请重试！");
                    }
                }
                else
                {
                    Model.EN_Video model = new Model.EN_Video();
                    model = GetModel(model);
                    model.Sort = 1;
                    model.IsDeleted = false;
                    model.Status = 1;
                    model.Code = GetGuid;
                    model.CreatedTime = DateTime.Now;
                    CommonBase.Insert<Model.EN_Video>(model, listComm);
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