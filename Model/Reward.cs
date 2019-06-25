using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using DBUtility;
namespace Model{
	 	//Reward
		 [EnitityMapping(TableName = "Reward")]
	public class Reward
	{
   		     
	  
		   [EnitityMapping(ColumnType = "KEY")]
				public string RewardType{get;set;}        
				public string RewardName{get;set;}        
				public bool RewardState{get;set;}        
				public bool NeedProcess{get;set;}        
				public int RewardIndex{get;set;}        
		   
	}
}

