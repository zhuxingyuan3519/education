using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//CM_PosExpense
		 [EnitityMapping(TableName = "CM_PosExpense")]
	public class CM_PosExpense
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int Id{get;set;}        
				public string Code{get;set;}        
				public string PosNum{get;set;}        
				public decimal ExpenseMoney{get;set;}        
				public int ExpenseCount{get;set;}        
				public decimal Rate{get;set;}        
				public decimal TakeOffMoney{get;set;}        
				public DateTime ExpenseDate{get;set;}        
				public bool IsGetMoney{get;set;}        
				public DateTime GetMoneyDate{get;set;}        
				public bool IsTake{get;set;}        
				public DateTime TakeTime{get;set;}        
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

