using DBUtility;
using Service;
using SSOInterface;
using SSOModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using Model;
using MethodHelper;
using System.Net;
using System.IO;
using System.Text;
using LitJson;
using Newtonsoft.Json;
//using vmimeNET;

namespace Web
{
    public partial class Test: BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

                //string redirect = Request.QueryString["redirect"];
                //MethodHelper.LogHelper.WriteTextLog("Test", "获取到参数redirect:" + redirect, DateTime.Now);
                ////1、获取到code
                //string code = Request.QueryString["code"];
                //MethodHelper.LogHelper.WriteTextLog("Test", "获取到微信传递的code:" + code, DateTime.Now);
                ////2、使用code换取access_token
                //string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + Service.WXPay.business.WeixinShare.APPID;
                //url += "&secret=" + Service.WXPay.business.WeixinShare.APPSecret;
                //url += "&code=" + code;
                //url += "&grant_type=authorization_code";
                //string result = WxPayAPI.HttpService.Get(url);
                //MethodHelper.LogHelper.WriteTextLog("Test", "使用code换取access_token返回结果：" + result, DateTime.Now);

                //JsonData jd = JsonMapper.ToObject(result);
                ////获取access_token
                //string access_token = (string)jd["access_token"];
                ////获取用户openid
                //string openid = (string)jd["openid"];

                //MethodHelper.LogHelper.WriteTextLog("Test", "获取到access_token：" + access_token + "，openid：" + openid, DateTime.Now);

                ////3、使用access_token获取用户信息
                //url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token;
                //url += "&openid=" + openid;
                //url += "&lang=zh_CN";
                //string resultInfo = WxPayAPI.HttpService.Get(url);
                //Service.WXPay.business.OAuthUser user = JsonConvert.DeserializeObject<Service.WXPay.business.OAuthUser>(resultInfo);




                //divTip.InnerHtml = "微信昵称：" + user.nickname + "，openid:" + user.openid + "。<br/>用户头像：<img src=\"" + user.headimgurl + "\"/>";

                //MethodHelper.LogHelper.WriteTextLog("Test", "使用access_token获取用户信息：" + resultInfo, DateTime.Now);




                GetDictBindDDL("BookVersion", ddl_Version);
                ddl_Version.Items.Insert(0, new ListItem("--版本--", ""));

                GetDictBindDDL("Grade", ddl_Grade);
                ddl_Grade.Items.Insert(0, new ListItem("--年级--", ""));

                GetDictBindDDL("Leavel", ddl_Leavel);
                ddl_Leavel.Items.Insert(0, new ListItem("--学期--", ""));

                GetDictBindDDL("Unit", ddl_unit);
                ddl_unit.Items.Insert(0, new ListItem("--章节--", ""));


                //AddressService.SynsAddressFromAMap();


                //                string requestXml = @"<Request>
                //  <SystemUnique>
                //     <SystemID>m</SystemID>
                //     <SystemPwd>26tew234sa3y3e3w</SystemPwd>
                //  </SystemUnique>
                //  <RequestInfo>
                //    <UserCode>zhuxy</UserCode>
                //    <UserPwd>123</UserPwd>
                //  </RequestInfo>
                //</Request>";
                //                string responseCode = string.Empty, responseString = string.Empty;
                //                SSOMember SSOModel = SSO.Login(requestXml, out responseCode, out responseString);
                //                if (responseCode == "0")//登录成功
                //                {

                //                    Response.Write(responseString + "：用户账号：" + SSOModel.MID);
                //                }
                //                else
                //                {
                //                    Response.Write(responseString);
                //                }
            }
        }

        //        protected void TestMail()
        //        {
        //using (EmailBuilder i_Email = new EmailBuilder("John Miller <jmiller@gmail.com>", 
        //                                               "vmime.NET Test Email"))
        //{
        //    i_Email.AddTo("recipient1@gmail.com"); // or "Name <email>"
        //    i_Email.AddTo("recipient2@gmail.com");

        //    i_Email.SetPlainText("This is the plain message part.\r\n" +
        //                         "This is Chinese: \x65B9\x8A00\x5730\x9EDE");

        //    i_Email.SetHtmlText ("This is the <b>HTML</b> message part.<br/>" +
        //                         "<img src=\"cid:VmimeLogo\"/><br/>" +
        //                         "(This image is an embedded object)<br/>" +
        //                         "This is Chinese: \x65B9\x8A00\x5730\x9EDE");

        //    i_Email.AddEmbeddedObject("E:\\Images\\Logo.png", "", "VmimeLogo");

        //    i_Email.AddAttachment("E:\\Documents\\ReadMe.txt", "", "");

        //    //i_Email.SetHeaderField(Email.eHeaderField.Organization, "ElmueSoft");

        //    String s_Email = i_Email.Generate();
        //} // i_Email.Dispose()
        //        }





        protected void Button1_Click(object sender, EventArgs e)
        {
            string tiemSpan = "180021212";
            var contentType = "application/json; charset=utf-8";
            string bodyJson = "{ \"user_id\": \"erter345tertertrfg\", \"phone\": \"17602113519\", \"prize_type\": 2, \"prize_money\": 10000, \"at_time\": " + tiemSpan + " }";
            //由于接口需要格林威治时间故将当前时间转为格林威治时间再将格林威治时间字符串转成日期类型传入头部
            var nowDateStr = DateTime.Now.ToString("r");
            var nowTimeGMT = DateTime.Parse(nowDateStr);
            var utf8 = Encoding.UTF8;
            var headerAuthorization = "Bearer Udhekishe7763gdheu77h8j";
            var url = "http://jwy.u1200.com/api/v1/user/prize/rate";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "post";
            req.Headers.Add("Authorization", headerAuthorization);
            req.ContentType = contentType;
            req.Date = nowTimeGMT;

            byte[] bytes = utf8.GetBytes(bodyJson);
            req.ContentLength = bytes.Length;
            Stream reqstream = req.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);

            string result = string.Empty;
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                using(StreamReader sr = new StreamReader(resp.GetResponseStream(), utf8))
                {
                    result = sr.ReadToEnd();
                }
            }
            catch(Exception ex)
            {


            }
            divTip.InnerHtml = result;
            string test = result;
            return;









            List<CommonObject> listComm = new List<CommonObject>();

            //存储过程查出来该代理商名下的所有会员
            SqlParameter[] parameters = {
                    new SqlParameter("@MID", SqlDbType.VarChar, 50),
                    new SqlParameter("@RoleCode", SqlDbType.VarChar, 50)
                 };
            parameters[0].Value = 1197;
            parameters[1].Value = "";
            DataTable dtUpperTJMemberTbl = CommonBase.GetProduceTable(parameters, "Proc_CountTDCount");
            foreach(DataRow dr in dtUpperTJMemberTbl.Rows)
            {
                //string memberID = dr["ID"].ToString();
                //string update3 = "UPDATE dbo.Member SET Agent =" + mom.ID + " WHERE ID =" + memberID;
                //listComm.Add(new CommonObject(update3, null));
            }




            ////0、查询到超级管理员
            //Model.Member manageModel = CommonBase.GetList<Model.Member>("RoleCode='Manage'").FirstOrDefault();
            ////1、查找到系统中的管理员
            //string update0 = "UPDATE dbo.Member SET UseRoleType =2";
            //listComm.Add(new CommonObject(update0, null));
            //Model.Member adminModel = CommonBase.GetList<Model.Member>("RoleCode='Admin'").FirstOrDefault();
            ////2、修改系统中人员归属代理商Company字段
            //string update1 = "UPDATE dbo.Member SET Company =" + adminModel.ID + " WHERE ID NOT IN (" + manageModel.ID + "," + adminModel.ID + ")";
            //listComm.Add(new CommonObject(update1, null));
            ////3、查询出系统中的所有代理商
            //List<Model.Member> listAgentMember = CommonBase.GetList<Model.Member>("RoleCode not in ('Manage','Admin','Member','VIP')");
            //foreach (Model.Member mom in listAgentMember)
            //{
            //    string updateToRoleCode = "Zone";
            //    if (mom.RoleCode == "Zone")
            //        updateToRoleCode = "3F";
            //    if (mom.RoleCode == "3F")
            //        updateToRoleCode = "Zone";


            //    if (mom.RoleCode == "City")
            //        updateToRoleCode = "2F";
            //    if (mom.RoleCode == "2F")
            //        updateToRoleCode = "City";


            //    if (mom.RoleCode == "Province")
            //        updateToRoleCode = "1F";
            //    if (mom.RoleCode == "1F")
            //        updateToRoleCode = "Province";
            //    int updateAreaId = int.Parse(CacheService.RoleList.Where(c => c.Code == updateToRoleCode).FirstOrDefault().AreaLeave);

            //    int guishuAgentId = adminModel.ID;

            //    string update2 = "UPDATE dbo.Member SET RoleCode ='" + updateToRoleCode + "' ,AreaId=" + updateAreaId + ",Agent=" + guishuAgentId + " WHERE ID =" + mom.ID;
            //    listComm.Add(new CommonObject(update2, null));

            //    //存储过程查出来该代理商名下的所有会员
            //    SqlParameter[] parameters = {
            //        new SqlParameter("@MID", SqlDbType.VarChar, 50),
            //        new SqlParameter("@RoleCode", SqlDbType.VarChar, 50)
            //     };
            //    parameters[0].Value = mom.ID;
            //    parameters[1].Value = "";
            //    DataTable dtUpperTJMemberTbl = CommonBase.GetProduceTable(parameters, "Proc_CountTDCount");
            //    foreach (DataRow dr in dtUpperTJMemberTbl.Rows)
            //    {
            //        string memberID = dr["ID"].ToString();
            //        string update3 = "UPDATE dbo.Member SET Agent =" + mom.ID + " WHERE ID =" + memberID;
            //        listComm.Add(new CommonObject(update3, null));
            //    }
            //}
            if(CommonBase.RunListCommit(listComm))
            {
                lbText.Text = "执行成功";
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string indexName = txt_beginIndex.Text;
            if(string.IsNullOrEmpty(indexName))
                indexName = "1380000";
            string indexMID = txt_mid_index.Text;
            if(string.IsNullOrEmpty(indexMID))
                indexMID = "测试";
            string ranktype = ddl_rankType.Text;

            string begin = txt_begin.Text.Trim();
            string end = txt_end.Text.Trim();
            string beginMTJ = txt_beginMTJ.Text;
            string lastID = string.Empty;

            var list = CommonBase.GetList<Model.Member>("MID='" + beginMTJ + "'");
            if(list.Count > 0)
                lastID = list[0].ID;
            else
            {
                divTip.InnerHtml = "初始推荐人不存在。";
                return;
            }

            for(int i = int.Parse(begin); i <= int.Parse(end); i++)
            {
                Model.Member member = new Model.Member();
                member.Company = "0";
                member.ID = MethodHelper.CommonHelper.GetGuid;
                if(ranktype == "1")
                {
                    member.MTJ = lastID;
                    lastID = member.ID;
                }
                else if(ranktype == "2")
                {
                    member.MTJ = "18";
                }
                else if(ranktype == "3")
                {
                    string sql = "  SELECT TOP 1 * FROM dbo.Member  ORDER BY NEWID();";
                    DataTable dt = CommonBase.GetTable(sql);
                    if(dt.Rows.Count > 0)
                        member.MTJ = dt.Rows[0]["ID"].ToString();
                    else
                        member.MTJ = "18";
                }
                member.IsClock = false;
                member.IsClose = false;
                member.MCreateDate = DateTime.Now;
                member.MDate = DateTime.Now;
                string ms = i.ToString();
                if(i < 10)
                    ms = "000" + i;
                if(i < 100 & i >= 10)
                    ms = "00" + i;
                if(i < 1000 & i >= 100)
                    ms = "0" + i;
                member.MID = indexName + ms;
                member.Tel = member.MID;
                member.MName = indexMID + ms;
                member.MState = true;
                member.Password = "5287C912ECA1635E";
                member.RoleCode = "Member";
                member.UseBeginTime = DateTime.Now.AddDays(-1);
                member.UseEndTime = DateTime.Now.AddDays(ConvertHelper.ToInt32(CacheService.GlobleConfig.Field2, 30) - 1);

                member.UseRoleType = 2;
                CommonBase.Insert<Model.Member>(member);
            }
            divTip.InnerHtml = "批量创建会员成功";
        }



        protected void One_Up_Click(object sender, EventArgs e)
        {
            Model.Member member = CommonBase.GetList<Member>("MID='" + txt_oneMid.Text.Trim() + "'").FirstOrDefault();
            if(member != null)
            {
                Model.Sys_Role roles = CacheService.RoleList.First(c => c.Code == "Student");
                #region   2、缴费表中插入一条数据
                Model.TD_PayLog payModel = new Model.TD_PayLog();
                payModel.Code = MethodHelper.CommonHelper.GetGuid;
                payModel.PayType = "微信支付";
                payModel.PayWay = "微信支付";
                payModel.ProductCode = MethodHelper.CommonHelper.CreateNo();
                payModel.Company = 0;
                payModel.CreatedBy = member.MID;
                payModel.CreatedTime = DateTime.Now;
                payModel.IsDeleted = false;
                payModel.PayForMID = "admin";
                payModel.PayMID = member.MID;
                payModel.PayMoney = MethodHelper.ConvertHelper.ToDecimal(roles.Remark, 0);
                payModel.PayTime = DateTime.Now;
                payModel.Status = 0;
                payModel.PayID = member.ID;
                payModel.Remark = "缴费成为" + roles.Name;
                #endregion
                if(CommonBase.Insert<Model.TD_PayLog>(payModel))
                {
                    SignUpService.SignUp(payModel, roles, member, member, "测试缴费");
                    divTip.InnerHtml = "测试缴费成功";
                }
            }
            else
            {
                divTip.InnerHtml = "测试缴费会员不存在";
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string begin = txt_fee_begin.Text.Trim();
            string end = txt_fee_end.Text.Trim();
            string indexName = txt_index_name.Text;
            if(string.IsNullOrEmpty(indexName))
                indexName = "1380000";
            var list = CommonBase.GetList<Model.Member>("");
            Model.Member TModel = list.FirstOrDefault(c => c.ID == "18");
            Model.Sys_Role roles = CacheService.RoleList.First(c => c.Code == "Student");
            for(int i = int.Parse(begin); i <= int.Parse(end); i++)
            {
                string ms = i.ToString();
                if(i < 10)
                    ms = "000" + i;
                if(i < 100 & i >= 10)
                    ms = "00" + i;
                if(i < 1000 & i >= 100)
                    ms = "0" + i;
                string MID = indexName + ms;
                Model.Member member = list.FirstOrDefault(c => c.MID == MID);
                if(member != null)
                {
                    List<CommonObject> listComm = new List<CommonObject>();
                    #region   2、缴费表中插入一条数据
                    Model.TD_PayLog payModel = new Model.TD_PayLog();
                    payModel.Code = MethodHelper.CommonHelper.GetGuid;
                    payModel.PayType = "微信支付";
                    payModel.PayWay = "微信支付";
                    payModel.ProductCode = MethodHelper.CommonHelper.CreateNo();
                    payModel.Company = 0;
                    payModel.CreatedBy = member.MID;
                    payModel.CreatedTime = DateTime.Now;
                    payModel.IsDeleted = false;
                    payModel.PayForMID = "admin";
                    payModel.PayMID = member.MID;
                    payModel.PayMoney = MethodHelper.ConvertHelper.ToDecimal(roles.Remark, 0);
                    payModel.PayTime = DateTime.Now;
                    payModel.Status = 0;
                    payModel.PayID = member.ID;
                    payModel.Remark = "缴费成为" + roles.Name;
                    #endregion
                    if(CommonBase.Insert<Model.TD_PayLog>(payModel))
                    {
                        SignUpService.SignUp(payModel, roles, member, member, "测试缴费");
                    }
                    Thread.Sleep(1500);
                }
            }
            divTip.InnerHtml = "测试缴费成功";
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            string deleteSql = @"TRUNCATE TABLE dbo.TD_PayLog;
TRUNCATE TABLE dbo.EN_SignUp;
TRUNCATE TABLE dbo.TD_FHLog;
TRUNCATE TABLE dbo.TD_TXLog;
TRUNCATE TABLE dbo.DB_Log;
UPDATE dbo.Member SET RoleCode='Member',MSH=0 WHERE MID<>'admin';
            TRUNCATE TABLE dbo.M_Rank";
            CommonBase.RunSql(deleteSql);
            divTip.InnerHtml = "删除成功";
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            string deleteSql = @"DELETE FROM dbo.Member WHERE MID<>'admin'";
            CommonBase.RunSql(deleteSql);
            divTip.InnerHtml = "删除成功";
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            //删除重复的单词
            //           DataTable Words = CommonBase.GetTable(@"select * from T_Words where English in (
            //select English  from T_Words group by (English) having(COUNT(English))>=2
            //)
            //order by English");

            List<T_Words> listWord = CommonBase.GetList<T_Words>(@"English in (
 select English  from T_Words group by (English) having(COUNT(English))>=2 )
 order by English");
            foreach(T_Words word in listWord)
            {
                var selectWords = listWord.Where(c => c.English == word.English);
                //删除掉没有模块拆分或情景联想的单词，只保留一个
                foreach(T_Words org in selectWords)
                {

                }
            }

            //重置序号
            //DataTable dtVersionWords = CommonBase.GetTable("SELECT DISTINCT Version,Grade,Leavel,Unit FROM dbo.T_VersionVsWords");
            //foreach (DataRow row in dtVersionWords.Rows)
            //{
            //    string sql = "SELECT * FROM dbo.T_VersionVsWords WHERE Version='" + row["Version"].ToString() + "' AND Grade='" + row["Grade"].ToString() + "' AND Leavel='" + row["Leavel"].ToString() + "' AND Unit='" + row["Unit"].ToString() + "'  AND CreatedTime<='2018-12-11 23:40:40' order by  CreatedTime ASC,CONVERT(INT,ISNULL(WIndex,'1')) asc";
            //    DataTable dt = CommonBase.GetTable(sql);
            //    int i = 1;
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        string updateSql = "UPDATE dbo.T_VersionVsWords SET WIndex='" + i + "' WHERE Code='" + dr["Code"].ToString() + "'";
            //        CommonBase.RunSql(updateSql);
            //        i++;
            //    }
            //}


            return;

            List<CommonObject> listCommon = new List<CommonObject>();
            //DataTable dtVersionWords = CommonBase.GetTable("select * from T_VersionVsWords");
            List<T_VersionVsWords> listVersion = CommonBase.GetList<T_VersionVsWords>();
            foreach(T_VersionVsWords word in listVersion)
            {
                string result = System.Text.RegularExpressions.Regex.Replace(word.Unit, @"[^0-9]+", "");
                word.Unit = result;
                CommonBase.Update<T_VersionVsWords>(word, new string[] { "Unit" });
            }
            if(CommonBase.RunListCommit(listCommon))
                Response.Write("执行成功");
            else
                Response.Write("执行失败");
            return;




            //string deleteSql = @"DELETE FROM dbo.Member WHERE MID<>'admin'";
            //CommonBase.RunSql(deleteSql);
            //divTip.InnerHtml = "删除成功";
            List<T_Words> listAllWards = CommonBase.GetList<T_Words>();

            DataTable dtWords = CommonBase.GetTable("select * from T_Words");
            DataTable dtWordResult = dtWords.Clone();
            foreach(DataRow row in dtWords.Rows)
            {
                row["English"] = row["English"].ToString().TrimStart().TrimEnd();
            }

            //dtWordResult = dtWords.DefaultView.ToTable(true, "English");
            //dtWordResult.DefaultView.Sort = "English asc";

            List<T_Words> listResult = new List<T_Words>();
            List<T_Words> listToDeleted = new List<T_Words>();

            for(int i = listAllWards.Count - 1; i >= 0; i--)
            {
                var word = listResult.FirstOrDefault(c => c.English.TrimEnd().TrimStart() == listAllWards[i].English.TrimEnd().TrimStart());
                if(word != null)
                {
                    listToDeleted.Add(word);
                    continue;
                }
                List<T_Words> checkWord = listAllWards.Where(c => c.English.TrimEnd().TrimStart() == listAllWards[i].English.TrimEnd().TrimStart()).ToList();
                if(checkWord.Count > 1)
                {
                    //找到一个
                    T_Words one = checkWord.FirstOrDefault(c => !string.IsNullOrEmpty(c.Module1));
                    if(one != null)
                    {
                        listResult.Add(one);
                    }
                    else
                    {
                        T_Words oneC = checkWord.FirstOrDefault(c => c.Phonetic.IndexOf('[') > 0);
                        if(oneC != null)
                        {
                            listResult.Add(oneC);
                        }
                        else
                        {
                            listResult.Add(checkWord[0]);
                        }
                    }
                }
                else if(checkWord.Count == 1)
                    listResult.Add(checkWord[0]);
            }
            int coun = listResult.Count;


            //对于删除的单词
            foreach(T_Words deletWord in listToDeleted)
            {
                var word = listResult.FirstOrDefault(c => c.English.TrimEnd().TrimStart() == deletWord.English.TrimEnd().TrimStart());
                if(word != null)
                {
                    listCommon.Add(new CommonObject("update   [dbo].[T_VersionVsWords] SET WordCode='" + word.Code + "' WHERE  WordCode='" + deletWord.Code + "'", null));
                    //查找到对应的教材版本
                    //List<T_VersionVsWords> listToUpdateVersion = CommonBase.GetList<T_VersionVsWords>("WordCode='" + deletWord.Code + "'");
                }
                //删除需要删除的
                CommonBase.Delete<T_Words>(deletWord, listCommon);
            }

            if(CommonBase.RunListCommit(listCommon))
                Response.Write("执行成功");
            else
                Response.Write("执行失败");

        }

    }

}

