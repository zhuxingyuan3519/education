using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//ConfigDictionary
		 [EnitityMapping(TableName = "ConfigDictionary")]
	public class ConfigDictionary
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public string DType{get;set;}        
		  
		   [EnitityMapping(ColumnType = "KEY")]
				public int StartLevel{get;set;}        
				public int EndLevel{get;set;}        
				public string DValue{get;set;}        
		  
		   [EnitityMapping(ColumnType = "KEY")]
				public string DKey{get;set;}        
				public string Remark{get;set;}        
		   
	}
}

