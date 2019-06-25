using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//Roles
		 [EnitityMapping(TableName = "Roles")]
	public class Roles
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public string RType{get;set;}        
				public string RName{get;set;}        
				public int RIndex{get;set;}        
				public bool CMessage{get;set;}        
				public bool IsAdmin{get;set;}        
				public bool CanSH{get;set;}        
				public bool CanLogin{get;set;}        
				public bool VState{get;set;}        
				public bool Super{get;set;}        
				public string RColor{get;set;}        
		   
	}
}

