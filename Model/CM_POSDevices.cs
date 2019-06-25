using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//CM_POSDevices
		 [EnitityMapping(TableName = "CM_POSDevices")]
	public class CM_POSDevices
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int PId{get;set;}        
				public int Id{get;set;}        
				public string Code{get;set;}        
				public string FirstIndustry{get;set;}        
				public string SecondIndustry{get;set;}        
				public decimal Rate{get;set;}        
				public string Bank{get;set;}        
				public string BankCardNum{get;set;}        
				public string StoreName{get;set;}        
				public bool IsDeleted{get;set;}        
				public DateTime CreatedTime{get;set;}        
				public string CreatedBy{get;set;}        
				public DateTime LastUpdateTime{get;set;}        
				public string LastUpdateBy{get;set;}        
				public int Status{get;set;}        
				public string Remark{get;set;}        
				public int Company{get;set;}        
				public int RatePercent{get;set;}        
				public string BankCardName{get;set;}        
		   
	}
}

