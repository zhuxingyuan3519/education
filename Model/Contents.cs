using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//Contents
		 [EnitityMapping(TableName = "Contents")]
	public class Contents
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public string CID{get;set;}        
				public string CTitle{get;set;}        
				public int CLevel{get;set;}        
				public string CAddress{get;set;}        
				public string CFID{get;set;}        
				public bool CState{get;set;}        
				public int CIndex{get;set;}        
				public string CImage{get;set;}        
				public bool VState{get;set;}        
				public bool IsQuickMenu{get;set;}        
				public string Remark{get;set;}        
				public bool IsOuterLink{get;set;}        
				public string OuterAddress{get;set;}        
				public string QuickMenuName{get;set;}        
		   
	}
}

