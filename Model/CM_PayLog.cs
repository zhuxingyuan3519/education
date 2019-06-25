using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//CM_PayLog
		 [EnitityMapping(TableName = "CM_PayLog")]
	public class CM_PayLog
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int Id{get;set;}        
				public string Code{get;set;}        
				public string PayMID{get;set;}        
				public string PayWay{get;set;}        
				public string PayID{get;set;}        
				public string PayForMID{get;set;}        
				public DateTime PayTime{get;set;}        
				public decimal PayMoney{get;set;}        
				public string ProductCode{get;set;}        
				public int ProductCount{get;set;}        
				public string PayPic{get;set;}        
				public string PayPic2{get;set;}        
				public string PayPic3{get;set;}        
				public bool IsDeleted{get;set;}        
				public DateTime CreatedTime{get;set;}        
				public string CreatedBy{get;set;}        
				public int Status{get;set;}        
				public string Remark{get;set;}        
				public int Company{get;set;}        
		   
	}
}

