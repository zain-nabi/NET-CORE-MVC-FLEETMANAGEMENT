using System.Collections.Generic;
using Triton.FleetManagement.Web.Helper;
using Triton.Service.Model.TFFDAT.Tables;

namespace Triton.FleetManagement.Web.Models
{
    public class SalesPerCategoryModel
    {
        public List<ICCATG> CategoryList { get; set; }
        public List<ICLOC> ICLOCList { get; set; }
        public List<OESHDT> OESHDT { get; set; }
        public List<PeriodHelper> Peroids { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string SelectedCategory { get; set; }
        public string SelectedICLOC { get; set; }
        public string SelectedDate { get; set; }
        public bool ShowReport { get; set; }
        public string Url { get; set; }
    }
}
