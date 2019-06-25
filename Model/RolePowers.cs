using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//RolePowers
		 [EnitityMapping(TableName = "RolePowers")]
	public class RolePowers
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int RID{get;set;}        
				public string RType{get;set;}        
				public string CID{get;set;}        
				public bool IFVerify{get;set;}        
				public int Company{get;set;}        
		   
	}
}

