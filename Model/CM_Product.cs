using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//CM_Product
		 [EnitityMapping(TableName = "CM_Product")]
	public class CM_Product
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int Id{get;set;}        
				public string Code{get;set;}        
				public decimal Money{get;set;}        
				public int Limit{get;set;}        
				public int MaxCardCount{get;set;}        
				public int Points{get;set;}        
				public bool IsDeleted{get;set;}        
				public DateTime CreatedTime{get;set;}        
				public string CreatedBy{get;set;}        
				public int Status{get;set;}        
				public string Remark{get;set;}        
				public int Company{get;set;}        
				public decimal XuFeiMoney{get;set;}        
		   
	}
}

