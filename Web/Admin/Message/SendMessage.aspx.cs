using DBUtility;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Message
{
    public partial class SendMessage : BasePage
    {
        protected override void SetPowerZone()
        {
            //repMemberList.DataSource = CommonBase.GetList<Model.Member>("IsDeleted=0 and Field5='Member'");
            //repMemberList.DataBind();
            var list = CacheService.RoleList.Where(c => c.RIndex > 2).OrderBy(c => c.RIndex);
            string appendhtml = string.Empty;
            foreach (Sys_Role role in list)
            {
                appendhtml += "&emsp;<input type='checkbox' value='" + role.Code + "' name='rolelist'/>" + role.Name;
            }
            divRoles.InnerHtml = appendhtml;

            System.Web.UI.HtmlControls.HtmlSelect selectControl = ddl_Province;// this.Page.FindControl(NameHeader + ddl_Province.ClientID) as System.Web.UI.HtmlControls.HtmlSelect;
            if (selectControl != null)
            {
                selectControl.DataSource = CacheService.SatandardAddressList.Where(c => c.Level == "20");
                selectControl.DataTextField = "Name";
                selectControl.DataValueField = "Id";
                selectControl.DataBind();
                selectControl.Items.Insert(0, new ListItem("--请选择--", ""));
            }
        }

        protected override string btnModify_Click()
        {
            List<CommonObject> listCommon = new List<CommonObject>();
            string isAll = Request.Form["chkAll"];
            string message = Request.Form["editor"];
            List<Model.Member> receiveList = new List<Model.Member>();
            List<Model.Member> listAllMember = CommonBase.GetList<Model.Member>("");
            if (!string.IsNullOrEmpty(isAll))
            {
                //全体发送
                //List<Model.Member> listAll = CommonBase.GetList<Model.Member>("RoleCode<>'Manage' ");
                var list = listAllMember.Where(c => c.RoleCode != "Manage");
                receiveList.AddRange(list);
            }
            #region 选择几个会员发送
            string chkAgentMemberID = Request.Form["chkMemberCode"];
            if (!string.IsNullOrEmpty(chkAgentMemberID))
            {
                string[] checkM = chkAgentMemberID.Split(',');
                foreach (string mcode in checkM)
                {
                    if (!string.IsNullOrEmpty(mcode))
                    {
                        //Model.Member member = CommonBase.GetModel<Model.Member>(mcode);
                        Model.Member member = listAllMember.FirstOrDefault(c => c.ID.ToString() == mcode);
                        if (member != null)
                        {
                            if (!receiveList.Contains(member)) //发送列表中不包含该会员才添加，避免重复发送
                                receiveList.Add(member);
                        }
                    }
                }
            }
            #endregion

            #region 选择几个代理商发送
            string chkAgentD = Request.Form["chkSendMemberCode"];
            if (!string.IsNullOrEmpty(chkAgentD))
            {
                string[] checkM = chkAgentD.Split(',');
                foreach (string mcode in checkM)
                {
                    if (!string.IsNullOrEmpty(mcode))
                    {
                        //Model.Member member = CommonBase.GetModel<Model.Member>(mcode);
                        Model.Member member = listAllMember.FirstOrDefault(c => c.ID.ToString() == mcode);
                        if (member != null)
                        {
                            if (!receiveList.Contains(member)) //发送列表中不包含该会员才添加，避免重复发送
                                receiveList.Add(member);
                        }
                    }
                }
            }
            #endregion

            #region 选择几个群组发送
            string roleCodeList = Request.Form["rolelist"];
            if (!string.IsNullOrEmpty(roleCodeList))
            {
                string[] roleArray = roleCodeList.Split(',');
                foreach (string role in roleArray)
                {
                    if (!string.IsNullOrEmpty(role))
                    {
                        //List<Model.Member> listAll = CommonBase.GetList<Model.Member>("RoleCode<>'" + role + "' ");
                        var listAll = listAllMember.Where(c => c.RoleCode == role);
                        foreach (Model.Member member in listAll)
                        {
                            if (!receiveList.Contains(member)) //发送列表中不包含该会员才添加，避免重复发送
                                receiveList.Add(member);
                        }
                    }
                }
            }
            #endregion

            #region 按区域发送
            string province = Request.Form["ddl_Province"];
            string city = Request.Form["ddl_City"];
            string zone = Request.Form["ddl_Zone"];
            if (!string.IsNullOrEmpty(zone))
            {
                //List<Model.Member> listAll = CommonBase.GetList<Model.Member>("Zone='" + zone + "' ");
                var listAll = listAllMember.Where(c => c.Zone == zone);// CommonBase.GetList<Model.Member>("Zone='" + zone + "' ");
                foreach (Model.Member member in listAll)
                {
                    if (!receiveList.Contains(member)) //发送列表中不包含该会员才添加，避免重复发送
                        receiveList.Add(member);
                }
            }
            else if (!string.IsNullOrEmpty(city))
            {
                //List<Model.Member> listAll = CommonBase.GetList<Model.Member>("City='" + city + "' ");
                var listAll = listAllMember.Where(c => c.City == city);
                foreach (Model.Member member in listAll)
                {
                    if (!receiveList.Contains(member)) //发送列表中不包含该会员才添加，避免重复发送
                        receiveList.Add(member);
                }
            }
            else if (!string.IsNullOrEmpty(province))
            {
                //List<Model.Member> listAll = CommonBase.GetList<Model.Member>("Province='" + province + "' ");
                var listAll = listAllMember.Where(c => c.Province == province);
                foreach (Model.Member member in listAll)
                {
                    if (!receiveList.Contains(member)) //发送列表中不包含该会员才添加，避免重复发送
                        receiveList.Add(member);
                }
            }
            #endregion

            #region 代理商名下会员

            string chkAgentID = Request.Form["chkAgentCode"];
            if (!string.IsNullOrEmpty(chkAgentID))
            {
                string[] checkM = chkAgentID.Split(',');
                foreach (string mcode in checkM)
                {
                    if (!string.IsNullOrEmpty(mcode))
                    {
                        //List<Model.Member> listAll = CommonBase.GetList<Model.Member>("AreaId='" + mcode + "' ");
                        var listAll = listAllMember.Where(c => c.Agent.ToString() == mcode);
                        foreach (Model.Member member in listAll)
                        {
                            if (!receiveList.Contains(member)) //发送列表中不包含该会员才添加，避免重复发送
                                receiveList.Add(member);
                        }
                    }
                }
            }
            #endregion

            Service.MessageService.SendNewMessage(TModel, receiveList, message, listCommon);

            if (CommonBase.RunListCommit(listCommon))
                return "1";
            return "0";
        }
    }
}