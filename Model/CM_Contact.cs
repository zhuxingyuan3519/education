using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//CM_Contact
		 [EnitityMapping(TableName = "CM_Contact")]
	public class CM_Contact
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int Id{get;set;}        
				public string Code{get;set;}        
				public string Name{get;set;}        
				public string QQ{get;set;}        
				public string Tel{get;set;}        
				public string HomeTel{get;set;}        
				public string Address{get;set;}        
				public string WeiXin{get;set;}        
				public string MarkImg{get;set;}        
				public bool IsDeleted{get;set;}        
				public DateTime CreatedTime{get;set;}        
				public string CreatedBy{get;set;}        
				public DateTime LastUpdateTime{get;set;}        
				public string LastUpdateBy{get;set;}        
				public int Status{get;set;}        
				public string Remark{get;set;}        
				public int Company{get;set;}        
		   
	}
}

