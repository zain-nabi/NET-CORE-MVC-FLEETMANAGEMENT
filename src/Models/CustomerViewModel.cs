using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Triton.Service.Model.TritonFleetManagement.Custom;

namespace Triton.FleetManagement.Web.Models
{
    public class CustomerViewModel
    {
        public IEnumerable<SelectListItem> ITCTypeDocumentList { get; set; }
        public CustomerModel CustomerModel { get; set; }

        public List<CustomerModel> CustomerModelList { get; set; }

        public int customerID { get; set; }
        public List<CustomerAuditModel> CustomerAuditModelList { get; set; }
    }
}
