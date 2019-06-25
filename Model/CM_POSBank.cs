using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//Sys_BankInfo
         [EnitityMapping(TableName = "CM_POSBank")]
    public class CM_POSBank
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public long Id{get;set;}      
             /// <summary>
           /// 1-POS超市图片；2-首页轮播图广告；3-信用卡中心banner广告图片，4-会员特供商品
             /// </summary>
				public string Code{get;set;}        
				public string Name{get;set;}        
				public string PicUrl{get;set;}        
				public string LinkUrl{get;set;}        
				public string Remark{get;set;}        
				public DateTime CreatedTime{get;set;}        
				public string CreatedBy{get;set;}        
				public DateTime? UpdatedTime{get;set;}        
				public string UpdatedBy{get;set;}        
				public bool IsDeleted{get;set;}        
				public bool Status{get;set;}        
				public string TiEAddress{get;set;}        
				public string TiERemark{get;set;}        
				public string DaiKuanRemark{get;set;}        
				public string DaiKuanAddress{get;set;}        
		   
	}
}

