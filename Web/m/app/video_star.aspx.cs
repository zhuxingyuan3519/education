using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.m.app
{
    public partial class video_star: BasePage
    {
        protected string VideoName = string.Empty, VideoCoverImg = string.Empty, VideoType = string.Empty, VideoPath = string.Empty, VideoRemark = string.Empty;
        protected int VideoCount = 0;
        protected override void SetPowerZone()
        {
            string code = Request.QueryString["code"];
            if(!string.IsNullOrEmpty(code))
            {
                Model.EN_Video video = CommonBase.GetModel<Model.EN_Video>(code);
                if(video != null)
                {
                    VideoName = video.Title;
                    VideoCoverImg = "/Attachment/Video/" + video.CoverImage;
                    VideoType = video.Format;
                    string[] array = video.Authority.Split(',');
                    //1F,2F,3F,VIP
                    string thisRole = TModel.RoleCode;
                    bool isCanSee = false;
                    switch(thisRole)
                    {
                        case "Member": if(video.Authority.Contains("Member")) isCanSee = true; break;
                        case "Teacher": if(video.Authority.Contains("Teacher")) isCanSee = true; break;
                        case "Student": if(video.Authority.Contains("Student")) isCanSee = true; break;
                        case "VIP": if(video.Authority.Contains("VIP") || video.Authority.Contains("Member")) isCanSee = true; break;
                        case "1F": if(video.Authority.Contains("1F") || video.Authority.Contains("VIP") || video.Authority.Contains("Member") || video.Authority.Contains("Student")) isCanSee = true; break;
                        case "2F": if(video.Authority.Contains("2F") || video.Authority.Contains("1F") || video.Authority.Contains("VIP") || video.Authority.Contains("Member") || video.Authority.Contains("Student")) isCanSee = true; break;
                        case "3F": if(video.Authority.Contains("3F") || video.Authority.Contains("2F") || video.Authority.Contains("1F") || video.Authority.Contains("VIP") || video.Authority.Contains("Member") || video.Authority.Contains("Student")) isCanSee = true; break;
                    }

                    if(isCanSee)
                        VideoPath = "/Attachment/Video/" + video.Path;
                    else
                        VideoPath = "";
                    //VideoPath = Base64Code(VideoPath);
                    VideoRemark = video.Remark;
                    VideoCount = video.Sort;
                    string updateCount = "UPDATE EN_Video SET Sort=Sort+1 WHERE Code='" + video.Code + "'";
                    CommonBase.GetSingle(updateCount);
                }
            }
        }

        public string Base64Code(string Message)
        {
            byte[] encData_byte = new byte[Message.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(Message);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }

    }
}