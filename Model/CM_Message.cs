﻿using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//CM_Message
		 [EnitityMapping(TableName = "CM_Message")]
	public class CM_Message
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int Id{get;set;}        
				public string Code{get;set;}        
				public string MType{get;set;}        
				public string MID{get;set;}        
				public DateTime MsgTime{get;set;}        
				public string LeaveMsg{get;set;}        
				public int Company{get;set;}        
				public bool IsDeleted{get;set;}        
				public DateTime CreatedTime{get;set;}        
				public string CreatedBy{get;set;}        
				public DateTime LastUpdateTime{get;set;}        
				public string LastUpdateBy{get;set;}        
				public int Status{get;set;}        
				public string Remark{get;set;}        
		   
	}
}

