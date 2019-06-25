using DBUtility;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ThoughtWorks.QRCode.Codec;
using Model;
using System.Data;

namespace Web.m.app
{
    public partial class apply_up: BasePage
    {
        protected decimal shmoney = 0;
        protected override void SetPowerZone()
        {
            Sys_Role role = CacheService.RoleList.FirstOrDefault(c => c.Code == "Student");
            if(role != null)
            {
                shmoney = MethodHelper.ConvertHelper.ToDecimal(role.Remark, 0);
            }

            string mid = Request.QueryString["mid"];
            if(!string.IsNullOrEmpty(mid))
            {
                //查询到这个会员
                Model.Member upMember = CommonBase.GetModel<Model.Member>(mid);
                if(upMember != null)
                {
                    hid_mid.Value = mid;
                    sp_mid.InnerHtml = upMember.MID;
                }
            }
            else
            {
                hid_mid.Value = TModel.ID;
                sp_mid.InnerHtml = TModel.MID;
            }

        }
    }
}