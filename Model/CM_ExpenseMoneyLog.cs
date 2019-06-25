using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//CM_ExpenseMoneyLog
		 [EnitityMapping(TableName = "CM_ExpenseMoneyLog")]
	public class CM_ExpenseMoneyLog
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int Id{get;set;}        
				public string Code{get;set;}        
				public string ArchiveId{get;set;}        
				public string Name{get;set;}        
				public string Bank{get;set;}        
				public string CardID{get;set;}        
				public decimal ExpenseMoney{get;set;}        
				public DateTime PlanDate{get;set;}        
				public DateTime ExpenseTime{get;set;}        
				public string ExpensedBy{get;set;}        
				public string PosNum{get;set;}        
				public decimal PosRate{get;set;}        
				public decimal TakeOffMoney{get;set;}        
				public string PosFirstIndusty{get;set;}        
				public string PosSecondIndusty{get;set;}        
				public string PosStoreName{get;set;}        
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

