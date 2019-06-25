using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//CM_StoreMoneyLog
		 [EnitityMapping(TableName = "CM_StoreMoneyLog")]
	public class CM_StoreMoneyLog
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int Id{get;set;}        
				public string Code{get;set;}        
				public string ArchiveId{get;set;}        
				public string Name{get;set;}        
				public string Bank{get;set;}        
				public string CardID{get;set;}        
				public decimal StoreMoney{get;set;}        
				public DateTime PlanDate{get;set;}        
				public DateTime StoreTime{get;set;}        
				public string StoredBy{get;set;}        
				public string PlanDetailId{get;set;}        
				public string PlanHeaderId{get;set;}        
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

