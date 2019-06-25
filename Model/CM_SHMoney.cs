using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//CM_SHMoney
		 [EnitityMapping(TableName = "CM_SHMoney")]
	public class CM_SHMoney
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int Id{get;set;}        
				public string Code{get;set;}        
				public string RoleCode{get;set;}        
				public int TJIndex{get;set;}        
				public decimal TJFloat{get;set;}        
				public decimal FHFloat{get;set;}        
				public bool IsDeleted{get;set;}        
				public DateTime CreatedTime{get;set;}        
				public string CreatedBy{get;set;}        
				public int Status{get;set;}        
				public string Remark{get;set;}        
				public int Company{get;set;}        
		   
	}
}

