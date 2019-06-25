using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using DBUtility;
namespace Model
{
    //Sys_StandardArea
    [EnitityMapping(TableName = "Sys_StandardArea")]
    public class Sys_StandardArea
    {
        [EnitityMapping(ColumnType = "KEY")]
        public string AdCode { get; set; }
        public string CityCode { get; set; }
        public string CityTelCode { get; set; }
        public string ProCode { get; set; }
        public string Name { get; set; }
        public string Center { get; set; }
        public string Level { get; set; }
        public int LevelInt { get; set; }
        public int Sort { get; set; }
        public bool Status { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Remark { get; set; }
    }
}

