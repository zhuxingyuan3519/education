
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
    public partial class VideoEdit: BasePage
    {
        protected Model.EN_Video tempModel = null;
        protected override void SetPowerZone()
        {
            rep_PrivageList.DataSource = CacheService.RoleList.Where(c => c.IsDeleted == false && c.IsAdmin == false);
            rep_PrivageList.DataBind();
        }
        protected override void SetValue(string code)
        {
            Model.EN_Video model;
            if(tempModel != null)
            {
                model = tempModel;
            }
            else
            {
                model = CommonBase.GetModel<Model.EN_Video>(code);
            }
            if(model != null)
            {
                tempModel = model;
                txt_Name.Value = model.Title;
                txt_remark.Value = model.Remark;
                hid_coverImg.Value = model.CoverImage;
                hidId.Value = model.Code;
                hid_fileSize.Value = model.Size.ToString();
                hid_fileType.Value = model.Format;
                hid_realName.Value = model.Name;
                hid_uploadName.Value = model.Path;
                div_coverImg.InnerHtml = "<img  src='/Attachment/Video/" + model.CoverImage + "' class='img_coverimg' />";
                sp_FileName.InnerHtml = model.Name;
            }

        }
        protected string GetCheckStatus(string code)
        {
            string result = "";
            string id = Request.QueryString["ID"];
            if(tempModel == null && !string.IsNullOrEmpty(id))
                tempModel = CommonBase.GetModel<Model.EN_Video>(id);

            if(tempModel != null && tempModel.Authority.Contains(code))
            {
                result = " checked='checked'";
            }
            return result;
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