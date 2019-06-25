using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//Sys_Address
		 [EnitityMapping(TableName = "Sys_Address")]
	public class Sys_Address
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int ID{get;set;}        
				public string ParentId{get;set;}        
				public DateTime CreateTime{get;set;}        
				public string CreatedBy{get;set;}        
				public int Status{get;set;}        
				public int Version{get;set;}        
				public int Sort{get;set;}        
				public bool IsDeleted{get;set;}        
				public string Code{get;set;}        
				public string Name{get;set;}        
				public string NameAlias{get;set;}        
				public int Level{get;set;}        
				public string Area{get;set;}        
		   
	}
}

