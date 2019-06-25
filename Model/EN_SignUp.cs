using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    [EnitityMapping(TableName = "EN_SignUp")]
    public class EN_SignUp
    {
        /// <summary>
        /// Code
        /// </summary>		
        [EnitityMapping(ColumnType = "KEY")]
        public string Code { get; set; }

        public string MCode { get; set; }
        public string CourseCode { get; set; }
        public string TrainingCode { get; set; }
        public string TecherCode { get; set; }
        public DateTime SignDate { get; set; }

        public decimal Fee { get; set; }
        /// <summary>
        /// Remark
        /// </summary>		
        public string Remark { get; set; }
        public int Sort { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }
    }
}
