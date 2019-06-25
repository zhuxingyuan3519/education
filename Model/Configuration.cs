using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//Configuration
		 [EnitityMapping(TableName = "Configuration")]
	public class Configuration
	{
   		     
			public decimal InMinFloat{get;set;}        
				public decimal InMaxFloat{get;set;}        
				public int InBaseMoney{get;set;}        
				public int InMinMoney{get;set;}        
				public decimal OutMinFloat{get;set;}        
				public decimal OutMaxFloat{get;set;}        
				public int ServiceRemindDay{get;set;}        
				public int TempQuotaRemindDay{get;set;}        
				public int StagingRemindDay{get;set;}        
				public int POSPoint{get;set;}        
				public string CommonURL{get;set;}        
				public int ZoneSJCount{get;set;}        
		   
	}
}

