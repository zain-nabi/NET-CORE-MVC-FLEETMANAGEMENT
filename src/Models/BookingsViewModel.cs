using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Triton.Service.Model.TritonFleetManagement.Custom;
using Triton.Service.Model.TritonFleetManagement.StoredProcs;
using Triton.Service.Model.TritonFleetManagement.Tables;

namespace Triton.FleetManagement.Web.Models
{
    public class BookingsViewModel
    {
        public IEnumerable<SelectListItem> MechanicStatusTypesList { get; set; }
        public IEnumerable<SelectListItem> MechanicalEmployeesList { get; set; }
        public IEnumerable<SelectListItem> CustomerList { get; set; }
        public IEnumerable<SelectListItem> VehicleList { get; set; }
        public BookingsModel BookingsModel { get; set; }
        public PartReasonViewModel JobCardBookingsModel { get; set; }
        public List<proc_Bookings_BookingReasons_Customers_Select> Bookings { get; set; }
        public proc_Bookings_BookingReasons_Customers_Select BookingReasonsPerJobCard { get; set; }
        public int CustomerID { get; set; }
        public IEnumerable<SelectListItem> BookingsPerCustomerList { get; set; }
        public bool ShowReport { get; set; }
        public proc_BookingDetails_GetByID BookingDetailsByID { get; set; }
        public proc_BookingDetails_GetByID JobCardBookingDetailsByID { get; set; }
        public List<BookingAuditModel> BookingAuditModelList { get; set; }
        public int BookingID { get; set; }
        public int VehicleID { get; set; }
        public OutworksCommission OutworksCommission { get; set; }
        public List<LookupCodeModel> PartsList { get; set; }
        public IEnumerable<SelectListItem> SelectedPartsList { get; set; }
        public List<PartReason> Parts { get; set; }
        public List<PartReason> Labour { get; set; }
        public List<PartReason> Consumables { get; set; }
        public List<PartReason> Tyres { get; set; }
        public OutworksCommission OutworksCommissionViewModel { get; set; }
    }

    public class BookingVehicleDocument
    {
        public List<VehicleDocument> VehicleDocumentList { get; set; }
        public List<DocumentRepository> DocumentRepositoryList { get; set; }
    }
}
