using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//CM_ChangeMoney
		 [EnitityMapping(TableName = "CM_ChangeMoney")]
	public class CM_ChangeMoney
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int Id{get;set;}        
				public string Code{get;set;}        
				public decimal ChangeMoney{get;set;}        
				public DateTime ChangeDate{get;set;}        
				public string ArchiveId{get;set;}        
				public string CardId{get;set;}        
				public string Name{get;set;}        
				public string ChangedBy{get;set;}        
				public int Company{get;set;}        
				public bool IsDeleted{get;set;}        
				public DateTime CreatedTime{get;set;}        
				public string CreatedBy{get;set;}        
				public DateTime LastUpdateTime{get;set;}        
				public string LastUpdateBy{get;set;}        
				public int Status{get;set;}        
				public string Remark{get;set;}        
		   
	}
}

