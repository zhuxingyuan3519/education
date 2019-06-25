using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//CM_FHLog
		 [EnitityMapping(TableName = "CM_FHLog")]
	public class CM_FHLog
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int Id{get;set;}        
				public string Code{get;set;}        
				public string FHType{get;set;}        
				public DateTime FHDate{get;set;}        
				public decimal FHMoney{get;set;}        
				public string MID{get;set;}        
				public string FHRoleCode{get;set;}        
				public bool IsDeleted{get;set;}        
				public DateTime CreatedTime{get;set;}        
				public string CreatedBy{get;set;}        
				public int Status{get;set;}        
				public string Remark{get;set;}        
				public int Company{get;set;}        
		   
	}
}

