using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//CM_PayInfo
		 [EnitityMapping(TableName = "CM_PayInfo")]
	public class CM_PayInfo
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int Id{get;set;}        
				public string Code{get;set;}        
				public string KFQQ{get;set;}        
				public string KFWeichat{get;set;}        
				public string Tel{get;set;}        
				public string Contact{get;set;}        
				public string WeichatPayPic{get;set;}        
				public string AiPayPic{get;set;}        
				public string OtherPayInfo{get;set;}        
				public int AreaId{get;set;}        
				public string MID{get;set;}        
				public bool IsDeleted{get;set;}        
				public DateTime CreatedTime{get;set;}        
				public string CreatedBy{get;set;}        
				public int Status{get;set;}        
				public string Remark{get;set;}        
				public int Company{get;set;}        
		   
	}
}

