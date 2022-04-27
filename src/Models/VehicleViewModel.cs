using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Triton.Model.TritonGroup.Tables;
using Triton.Service.Model.TritonFleetManagement.Custom;
using Triton.Service.Model.TritonFleetManagement.StoredProcs;
using Triton.Service.Model.TritonFleetManagement.Tables;

namespace Triton.FleetManagement.Web.Models
{
    public class VehicleViewModel
    {
        public List<proc_Vehicle_License_Customer_TailLift_Select> Vehicles { get; set; }
        public proc_Vehicle_License_Customer_TailLift_Select VehicleDetailsByID { get; set; }
        public IEnumerable<SelectListItem> TailLiftTypes { get; set; }
        public IEnumerable<SelectListItem> ServiceIntervals { get; set; }
        public IEnumerable<SelectListItem> VehicleClasses { get; set; }
        public IEnumerable<SelectListItem> VehicleBrands { get; set; }
        public IEnumerable<SelectListItem> Customers { get; set; }
        public VehicleModel Vehicle { get; set; }
        public string CarExist { get; set; }
        public string CarExistMessage { get; set; }
        public int CustomerID { get; set; }
        public string RegistrationNumber { get; set; }
        public bool ShowReport { get; set; }
        public int VehicleID { get; set; }
        public List<VehicleAuditModel> VehicleAuditModelList { get; set; }
    }
}
