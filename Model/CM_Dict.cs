using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//CM_Dict
		 [EnitityMapping(TableName = "CM_Dict")]
	public class CM_Dict
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int Id{get;set;}        
				public string ParentCode{get;set;}        
				public string Code{get;set;}        
				public string Name{get;set;}        
				public bool IsDeleted{get;set;}        
				public DateTime CreatedTime{get;set;}        
				public string CreatedBy{get;set;}        
				public DateTime? LastUpdateTime{get;set;}        
				public string LastUpdateBy{get;set;}        
				public int Status{get;set;}        
				public string Remark{get;set;}        
				public int Company{get;set;}        
		   
	}
}

