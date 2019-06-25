using System; 
using DBUtility;
using System.Collections.Generic; 
using System.Data;
namespace Model{
	 	//Sys_Menu
        [EnitityMapping(TableName = "Sys_Menu")]
	public class Sys_Menu
	{
   		     
      	/// <summary>
		/// Id
        /// </summary>		
		 [EnitityMapping(ColumnType ="KEY")]
		public string Id{get;set;}        
		/// <summary>
		/// ParentCode
        /// </summary>		
		 
		public string ParentCode{get;set;}        
		/// <summary>
		/// Name
        /// </summary>		
		 
		public string Name{get;set;}        
		/// <summary>
		/// Icon
        /// </summary>		
		 
		public string Icon{get;set;}        
		/// <summary>
		/// URL
        /// </summary>		
		 
		public string URL{get;set;}        
		/// <summary>
		/// MenuIndex
        /// </summary>		
		 
		public int MenuIndex{get;set;}        
		/// <summary>
		/// Company
        /// </summary>		
		 
		public string Company{get;set;}        
		/// <summary>
		/// IsDeleted
        /// </summary>		
		 
		public bool IsDeleted{get;set;}        
		/// <summary>
		/// CreatedTime
        /// </summary>		
		 
		public DateTime CreatedTime{get;set;}        
		/// <summary>
		/// CreatedBy
        /// </summary>		
		 
		public string CreatedBy{get;set;}        
		/// <summary>
		///? LastUpdateTime
        /// </summary>		
		 
		public DateTime? LastUpdateTime{get;set;}        
		/// <summary>
		/// LastUpdateBy
        /// </summary>		
		 
		public string LastUpdateBy{get;set;}        
		/// <summary>
		/// Status
        /// </summary>		
		 
		public int Status{get;set;}        
		/// <summary>
		/// Remark
        /// </summary>		
		 
		public string Remark{get;set;}        
		/// <summary>
		/// Field1
        /// </summary>		
		 
		public string Field1{get;set;}        
		/// <summary>
		/// Field2
        /// </summary>		
		 
		public string Field2{get;set;}        
		/// <summary>
		/// Field3
        /// </summary>		
		 
		public string Field3{get;set;}        
		/// <summary>
		/// Field4
        /// </summary>		
		 
		public string Field4{get;set;}        
		/// <summary>
		/// Field5
        /// </summary>		
		 
		public string Field5{get;set;}        
		   
	}
}

