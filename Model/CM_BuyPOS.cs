using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//CM_BuyPOS
		 [EnitityMapping(TableName = "CM_BuyPOS")]
	public class CM_BuyPOS
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int Id{get;set;}        
				public string Code{get;set;}        
				public string PayMID{get;set;}        
				public string Receiver{get;set;}        
				public string ReceiverTel{get;set;}        
				public string Address{get;set;}        
				public string Contacter{get;set;}        
				public string ContactTel{get;set;}        
				public string PayPic{get;set;}        
				public string PayPic2{get;set;}        
				public string PayPic3{get;set;}        
				public string PayPic4{get;set;}        
				public string PayPic5{get;set;}        
				public string SHRemark{get;set;}        
				public bool IsDeleted{get;set;}        
				public DateTime CreatedTime{get;set;}        
				public string CreatedBy{get;set;}        
				public int Status{get;set;}        
				public string Remark{get;set;}        
				public int Company{get;set;}        
		   
	}
}

