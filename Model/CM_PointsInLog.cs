using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//CM_PointsInLog
		 [EnitityMapping(TableName = "CM_PointsInLog")]
	public class CM_PointsInLog
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public int Id{get;set;}        
				public string Code{get;set;}        
				public string FromMID{get;set;}        
				public string ToMID{get;set;}        
				public decimal PointCount{get;set;}        
				public DateTime InTime{get;set;}        
				public bool IsDeleted{get;set;}        
				public DateTime CreatedTime{get;set;}        
				public string CreatedBy{get;set;}        
				public int Status{get;set;}        
				public string Remark{get;set;}        
				public int Company{get;set;}        
		   
	}
}

