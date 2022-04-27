using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Triton.Service.Model.TritonFleetManagement.Custom;
using Triton.Service.Model.TritonFleetManagement.Tables;

namespace Triton.FleetManagement.Web.Models
{
    public class ScaleHourViewModel
    {
        public IEnumerable<SelectListItem> CostScaleUsersModelList { get; set; }
        public ScaleHoursModel ScaleHoursModel { get; set; }
        public List<ScaleHoursModel> ScaleHoursModelList { get; set; }
        public bool ShowReport { get; set; }
        public string ScaleExistErrorMessage { get; set; }
        public int quantitySelectorValue { get; set; }
        public int employeeID { get; set; }
        public List<ScaleHourAuditModel> ScaleHourAuditModelList { get; set; }
    }
}
