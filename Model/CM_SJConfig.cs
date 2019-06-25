using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//CM_SJConfig
		 [EnitityMapping(TableName = "CM_SJConfig")]
	public class CM_SJConfig
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int Id{get;set;}        
				public string Code{get;set;}        
				public string FromRoleCode{get;set;}        
				public string ToRoleCode{get;set;}        
				public decimal TJFloat{get;set;}        
				public int TJCount{get;set;}        
				public int TDCount{get;set;}        
				public string TJRoleCode{get;set;}        
				public int TJRoleCount{get;set;}        
				public bool IsDeleted{get;set;}        
				public DateTime CreatedTime{get;set;}        
				public string CreatedBy{get;set;}        
				public int Status{get;set;}        
				public string Remark{get;set;}        
				public int Company{get;set;}        
		   
	}
}

