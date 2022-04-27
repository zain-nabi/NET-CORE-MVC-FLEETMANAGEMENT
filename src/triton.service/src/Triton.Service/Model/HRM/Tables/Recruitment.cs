using Dapper.Contrib.Extensions;
using System;

namespace Triton.Service.Model.HRM.Tables
{
    [Table("Recruitment")]
    public class Recruitment
    {
        [Key]
        public int RecruitmentID { get; set; }
        public bool Active { get; set; }
        public int JobProfileID { get; set; }
        public int StatusLCID { get; set; }
        public int MyProperty { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime ? DeletedOn { get; set; }
        public int ? DeletedByUserID { get; set; }
    }
}
