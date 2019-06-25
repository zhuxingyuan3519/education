using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//Sys_BankInfo
		 [EnitityMapping(TableName = "Sys_BankInfo")]
	public class Sys_BankInfo
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public long Id{get;set;}        
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
                public string BankTel { get; set; }
                public string BankMail { get; set; }        
	}
}

