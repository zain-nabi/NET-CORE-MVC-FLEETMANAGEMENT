using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Triton.Service.Model.TritonFleetManagement.Custom;

namespace Triton.FleetManagement.Web.Models
{
    public class DashboardViewModel
    {
        public DashboardModel DashboardModel { get; set; }
        public string barGraphLabels { get; set; }
        public string barGraphValues { get; set; }
    }
}
