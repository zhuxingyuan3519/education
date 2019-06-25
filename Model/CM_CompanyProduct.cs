using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//CM_CompanyProduct
		 [EnitityMapping(TableName = "CM_CompanyProduct")]
	public class CM_CompanyProduct
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int Id{get;set;}        
				public string Code{get;set;}        
				public string CompanyCode{get;set;}        
				public string ProductCode{get;set;}        
				public string Trade{get;set;}        
				public DateTime BuyTime{get;set;}        
				public bool IsDeleted{get;set;}        
				public DateTime CreatedTime{get;set;}        
				public string CreatedBy{get;set;}        
				public int Status{get;set;}        
				public string Remark{get;set;}        
				public int Company{get;set;}        
		   
	}
}

