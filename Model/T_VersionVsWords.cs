using System;
using DBUtility;
using System.Collections.Generic;
using System.Data;
namespace Model
{
    //T_VersionVsWords
    [EnitityMapping(TableName = "T_VersionVsWords")]
    public class T_VersionVsWords
    {

        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }

        public string Version { get; set; }
        public string Grade { get; set; }
        public string Leavel { get; set; }
        public string Unit { get; set; }

        public string WIndex { get; set; }
        public string WordCode { get; set; }

        public string Remark { get; set; }
        public string CreatedBy { get; set; }
        public bool IsDeleted { get; set; }

        public int Status { get; set; }
        public int Sort { get; set; }

        public DateTime CreatedTime { get; set; }

    }
}

