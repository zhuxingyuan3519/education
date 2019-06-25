using DBUtility;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class reward_setting : BasePage
    {
        protected override void SetPowerZone()
        {
            TD_SHMoney shmoneyGudong = CommonBase.GetList<TD_SHMoney>("Code='GuDong' and RoleCode='" + TModel.ID + "'").FirstOrDefault();
            if (shmoneyGudong != null)
            {
                txt_gudong.Value = (shmoneyGudong.TJFloat * 100).ToString();
            }

            //获取到股东比例
            TD_SHMoney shmoneyLaoshi = CommonBase.GetList<TD_SHMoney>("Code='LaoShi' and RoleCode='" + TModel.ID + "'").FirstOrDefault();
            if (shmoneyLaoshi != null)
            {
                txt_laoshi.Value = (shmoneyLaoshi.TJFloat * 100).ToString();
            }
        }

        protected override string btnAdd_Click()
        {
            List<CommonObject> listComm = new List<CommonObject>();
            string gudong = Request.Form[NameHeader + "txt_gudong"];
            string laoshi = Request.Form[NameHeader + "txt_laoshi"];

            decimal gudongBili = MethodHelper.ConvertHelper.ToDecimal(gudong, 0) / 100;
            decimal laoshiBili = MethodHelper.ConvertHelper.ToDecimal(laoshi, 0) / 100;
            if (gudongBili + laoshiBili > 1)
            {
                return "-3";
            }
            //获取到股东比例
            TD_SHMoney shmoneyGudong = CommonBase.GetList<TD_SHMoney>("Code='GuDong' and RoleCode='" + TModel.ID + "'").FirstOrDefault();
            if (shmoneyGudong == null)
            {
                shmoneyGudong = new TD_SHMoney();
                shmoneyGudong.Company = 0;
                shmoneyGudong.FHFloat = 0;
                shmoneyGudong.TJIndex = 1;
                shmoneyGudong.Remark = "股东分配收益";
                shmoneyGudong.Field1 = "1";
                shmoneyGudong.Code = "GuDong";
                shmoneyGudong.RoleCode = TModel.ID;
                shmoneyGudong.TJFloat = gudongBili;
                CommonBase.Insert<TD_SHMoney>(shmoneyGudong, listComm);
            }
            else
            {
                shmoneyGudong.TJFloat = gudongBili;
                CommonBase.Update<TD_SHMoney>(shmoneyGudong, new string[] { "TJFloat" }, listComm);
            }

            //获取到股东比例
            TD_SHMoney shmoneyLaoshi = CommonBase.GetList<TD_SHMoney>("Code='LaoShi' and RoleCode='" + TModel.ID + "'").FirstOrDefault();
            if (shmoneyLaoshi == null)
            {
                shmoneyLaoshi = new TD_SHMoney();
                shmoneyLaoshi.Company = 0;
                shmoneyLaoshi.FHFloat = 0;
                shmoneyLaoshi.TJIndex = 1;
                shmoneyLaoshi.Remark = "老师分配收益";
                shmoneyLaoshi.Field1 = "1";
                shmoneyLaoshi.Code = "LaoShi";
                shmoneyLaoshi.RoleCode = TModel.ID;
                shmoneyLaoshi.TJFloat = gudongBili;
                CommonBase.Insert<TD_SHMoney>(shmoneyLaoshi, listComm);
            }
            else
            {
                shmoneyLaoshi.TJFloat = laoshiBili;
                CommonBase.Update<TD_SHMoney>(shmoneyLaoshi, new string[] { "TJFloat" }, listComm);
            }


            if (CommonBase.RunListCommit(listComm))
            {
                return "1";
            }
            return "0";
        }
    }
}