using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Triton.FleetManagement.Web.Helper;
using Triton.FleetManagement.Web.Models;
using Triton.Model.LeaveManagement.Tables;
using Triton.Model.TritonGroup.Tables;
using Triton.Service.Data;
using Triton.Service.Model.TritonFleetManagement.Custom;
using Triton.Service.Model.TritonFleetManagement.StoredProcs;
using Triton.Service.Model.TritonFleetManagement.Tables;
using Triton.Service.Utils;

namespace Triton.FleetManagement.Web.Controllers
{
    [Authorize(Roles = "Admin Director, Service Advisor, Managing Director, Development Manager, Developer, Junior Developer, Bm")]
    public class BookingsController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(int customerID)
        {
            var bookings = await BookingsService.LookUpCodesAsync(customerID);
            return View(bookings);
        }

        [HttpGet]
        public async Task<ActionResult<List<LookUpCodes>>> GetParts(string LookUpCodeIDParts, string detail)
        {
            var model = new List<LookUpCodes>();
            var result = await LookUpCodesService.GetInventoryByLookUpCodeID(Convert.ToInt32(LookUpCodeIDParts));

            foreach (var item in result.PartsConsumablesLaboutLookUpCodes.Where(x => x.Detail.StartsWith(detail, StringComparison.OrdinalIgnoreCase)))
            {
                var ps = new LookUpCodes
                {
                    LookUpCodeID = item.LookUpCodeID,
                    Detail = item.Detail,
                    AdditionalField1Value = item.AdditionalField1Value,
                    Name = item.Name,
                    AdditionalField1Name = item.AdditionalField1Name,
                    FAIconString = item.FAIconString
                };
                model.Add(ps);
            }
            return model;
        }


        [HttpPost]
        public async Task<IActionResult> Index(BookingsModel bookingsModel, int customerID)
        {
            string estimatedArrivalDate = bookingsModel.Bookings.EstimatedArrival.Value.ToString("yyyy-MM-dd");
            var Exist = await BookingsService.CheckIfBookingExist(bookingsModel.Bookings.CustomerID, bookingsModel.Bookings.VehicleID, estimatedArrivalDate);
            if(Exist.OrderNumber == "True")
            {
                bookingsModel = await BookingsService.LookUpCodesAsync(customerID);
                bookingsModel.BookingExistMessage = "Vehicle booking already exist";
                bookingsModel.ExistingBookingsID = Exist.BookingsID;
                return View(bookingsModel);
            }

            bookingsModel.Bookings.CreatedByUserID = User.GetUserId();
            bookingsModel.Bookings.StatusLCID = 595;
            var result = await BookingsService.InsertAsync(bookingsModel);

            const string redirectUrl = "/Bookings/GetAllBookings";

            return result ? RedirectToAction("Message", "Home", new { type = StringHelper.Types.SaveSuccess, url = redirectUrl })
                          : RedirectToAction("Message", "Home", new { type = StringHelper.Types.SaveFailed, url = redirectUrl });
        }

        [HttpGet]
        public async Task<IActionResult> JobCard(int BookingsID, int VehicleID)
        {

            var bvm = new BookingsViewModel();
            bvm.JobCardBookingsModel = await BookingsService.GetInventoryByBookingID(BookingsID);
            bvm.OutworksCommissionViewModel = bvm.JobCardBookingsModel.OutworksCommission;
            bvm.BookingsModel = await BookingsService.LookUpCodesAsync(BookingsID);
            bvm.JobCardBookingDetailsByID = await BookingsService.GetBookingDetailsByCustomerID(BookingsID);
            bvm.BookingReasonsPerJobCard = await BookingsService.GetBookingReasonsPerJobCard(BookingsID);
            if (bvm.JobCardBookingDetailsByID != null)
            {
                bvm.BookingID = bvm.JobCardBookingDetailsByID.BookingsID;
                bvm.VehicleID = bvm.JobCardBookingDetailsByID.VehicleID;
            }
            else
            {
                bvm.BookingID = BookingsID;
                bvm.VehicleID = VehicleID;
            }

            return View(bvm);
        }

        [HttpPost]
        public async Task<IActionResult> JobCard(BookingsViewModel model)
        {
            if(model.BookingsModel.PartReasonLCID != null)
            {
                model.BookingsModel.PartsList =
                    JsonConvert.DeserializeObject<List<Triton.Service.Model.TritonFleetManagement.Tables.PartReason>>(WebUtility.UrlDecode(model.BookingsModel.PartReasonLCID) ?? string.Empty);
               
            }

            if (model.BookingsModel.LabourReasonLCID != null)
            {
                model.BookingsModel.LabourList =
                    JsonConvert.DeserializeObject<List<Triton.Service.Model.TritonFleetManagement.Custom.LabourReason>>(WebUtility.UrlDecode(model.BookingsModel.LabourReasonLCID) ?? string.Empty);

            }

            if (model.BookingsModel.ConsumableReasonLCID != null)
            {
                model.BookingsModel.ConsumablesList =
                    JsonConvert.DeserializeObject<List<Triton.Service.Model.TritonFleetManagement.Custom.ConsumableReason>>(WebUtility.UrlDecode(model.BookingsModel.ConsumableReasonLCID) ?? string.Empty);

            }

            if (model.BookingsModel.TyreReasonLCID != null)
            {
                model.BookingsModel.TyresList =
                    JsonConvert.DeserializeObject<List<Triton.Service.Model.TritonFleetManagement.Custom.TyreReason>>(WebUtility.UrlDecode(model.BookingsModel.TyreReasonLCID) ?? string.Empty);

            }
            model.BookingsModel.OutworksCommission.BookingID = model.JobCardBookingDetailsByID.BookingsID;
            model.BookingsModel.OutworksCommission.VehicleID = model.JobCardBookingDetailsByID.VehicleID;
            model.BookingsModel.OutworksCommission.CreatedBy = User.GetUserId();
            var ans = BookingsService.InsertPartReasons(model.BookingsModel);
            var result = await BookingsService.InsertPartsBookingReasonAndOutworksCommissionAsync(model.BookingsModel);
            const string redirectUrl = "/Bookings/GetAllBookings";

            return result ? RedirectToAction("Message", "Home", new { type = StringHelper.Types.SaveSuccess, url = redirectUrl })
                          : RedirectToAction("Message", "Home", new { type = StringHelper.Types.SaveFailed, url = redirectUrl });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bvm = new BookingsViewModel();
            bvm.BookingsPerCustomerList = new SelectList(await GetCustomers(), "CustomerID", "Name");
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;
            string startDate = StartDate.ToString("yyyy-MM-dd");
            string endDate = EndDate.ToString("yyyy-MM-dd");
            bvm.Bookings = await BookingsService.GetBookingsPerCustomer(0, startDate, endDate);
            bvm.ShowReport = true;
            return View(bvm);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllBookings(int CustomerID, string daterange)
        {
            var bvm = new BookingsViewModel();

            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;
            if (daterange != null)
            {
                var dateSplit = daterange.Split("-");
                StartDate = Convert.ToDateTime(dateSplit[0].Trim());
                EndDate = Convert.ToDateTime(dateSplit[1].Trim());
            }
            string startDate = StartDate.ToString("yyyy-MM-dd");
            string endDate = EndDate.ToString("yyyy-MM-dd");

            bvm.Bookings = await BookingsService.GetBookingsPerCustomer(CustomerID, startDate, endDate);
            bvm.BookingsPerCustomerList = new SelectList(await GetCustomers(), "CustomerID", "Name");
            bvm.ShowReport = true;
            return View(bvm);
        }

        [HttpGet]
        public async Task<IActionResult> GetBookingsById(int bookingsID)
        {
            var bvm = new BookingsViewModel();
            bvm.BookingsModel = await BookingsService.GetBookingsByIdAsync(bookingsID);
            if(bvm.BookingsModel.OutworksCommission == null)
            {
                bvm.BookingsModel.OutworksCommission = new OutworksCommission();
            }
            bvm.MechanicStatusTypesList = new SelectList(bvm.BookingsModel.MechanicTypes, "LookUpCodeID", "Name");
            bvm.MechanicalEmployeesList = new SelectList(await GetAllMechanicalEmployees(bookingsID), "EmployeeID", "FullNames");
            bvm.CustomerList = new SelectList(bvm.BookingsModel.Customers, "CustomerID", "Name");
            bvm.VehicleList = new SelectList(bvm.BookingsModel.Vehicles, "VehicleID", "RegistrationNumber");
            bvm.ShowReport = true;
            bvm.BookingID = bookingsID;
            bvm.PartsList = bvm.BookingsModel.Parts;

            var products = bvm.BookingsModel.Parts.Where(p => bvm.BookingsModel.PartReasons.Any(l => p.LookUpCodeID == l.InventoryID))
                           .ToList();
            bvm.SelectedPartsList = new SelectList(products, "LookUpCodeID", "Detail"); ;
            return View(bvm);
        }


        [HttpGet]
        public async Task<IActionResult> DeleteBookings(int bookingsID)
        {
            var bvm = new BookingsViewModel();
            bvm.BookingDetailsByID = await BookingsService.GetBookingDetailsByID(bookingsID);
            return View(bvm);
        }

        [HttpPost]
        public async Task<IActionResult> EditBookings(BookingsViewModel model)
        {
            var bookingsHelper = new BookingsHelper();
            var partReason = new List<PartReason>();
            var PartReasonViewModel = new PartReasonViewModel();
            var result = await BookingsService.UpdateAsync(bookingsHelper.UpdateBooking(model, User.GetUserId()));

            const string redirectUrl = "/Bookings/GetAllBookings";

            return result ? RedirectToAction("Message", "Home", new { StringHelper.Types.UpdateSuccess, url = redirectUrl })
                          : RedirectToAction("Message", "Home", new { StringHelper.Types.UpdateFailed, url = redirectUrl });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteBookings(proc_BookingDetails_GetByID bookingDetails)
        {
            var bookingsHelper = new BookingsHelper();
            var result = await BookingsService.DeleteBooking(bookingsHelper.DeleteBookings(bookingDetails, User.GetUserId()));

            const string redirectUrl = "/Bookings/GetAllBookings";

            return result ? RedirectToAction("Message", "Home", new { StringHelper.Types.UpdateSuccess, url = redirectUrl })
                          : RedirectToAction("Message", "Home", new { StringHelper.Types.UpdateFailed, url = redirectUrl });
        }

        public async Task<JsonResult> GetVendorCodesPerCustomer()
        {
            var vendorcodes = await BookingsService.GetVendorCodesPerCustomer();
            foreach(var item in vendorcodes)
            {
                if(item.VendorCodes == null)
                {
                    item.VendorCodes = "N/A";
                }
            }
            return Json(vendorcodes);
        }

        private async Task<List<CustomersModels>> GetCustomers()
        {
            var customerList = await BookingsService.GetAllCustomersAsync();
            customerList.Insert(0, new CustomersModels { CustomerID = 0, Name = "All" });
            return customerList;
        }

        private async Task<List<Employees>> GetAllMechanicalEmployees(int BookingsID)
        {
            var bookings = await BookingsService.GetBookingsByIdAsync(BookingsID);
            bookings.MechanicalEmployees.Insert(0, new Employees { EmployeeID = 0, FullNames = "None" });
            return bookings.MechanicalEmployees;
        }

        [HttpGet]
        public IActionResult InsertDocument(int bookingsID)
        {
            var booking = new Bookings();
            booking.BookingsID = bookingsID;
            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> DocumentUpload(List<IFormFile> File, int[] bookingID)
        {
            string s = "";
            var result = false;
            foreach (var item in File)
            {
                if (item.Length > 0 && item.Length <= 4000000)
                {
                    using (var ms = new MemoryStream())
                    {
                        item.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        s = Convert.ToBase64String(fileBytes);

                        var document = new DocumentRepository
                        {
                            ImgName = item.FileName,
                            ImgData = Encoding.ASCII.GetBytes(s),
                            ImgContentType = item.ContentType,
                            ImgLength = Convert.ToInt32(item.Length),
                            CreatedByUserID = User.GetUserId(),
                        };
                        result = await BookingsService.InsertDocument(document, bookingID[0]);
                    }
                }
            }
            return RedirectToAction("ViewDocuments", "Bookings", new { bookingsID = bookingID[0] });
        }

        [HttpGet]
        public async Task<IActionResult> ViewDocuments(int bookingsID)
        {
            var documents = await BookingsService.ViewDocumentAsync(bookingsID);
            //documents[0].BookingID = bookingsID;
            if (documents.Count != 0)
            {
                foreach (var item in documents)
                {
                    item.BookingID = bookingsID;
                    item.ImgDataStr = Encoding.ASCII.GetString(item.ImgData, 0, item.ImgData.Length);
                }
                return View(documents);
            }
            else
            {
                List<DocumentVehicleModel> vd = new List<DocumentVehicleModel>();
                DocumentVehicleModel vd1 = new DocumentVehicleModel();
                vd1.BookingID = bookingsID;
                vd1.ImgName = "Temp";
                vd.Add(vd1);
                return View(vd);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteDocumentAsync(int vehicleDocumentID, int bookingID)
        {
            DocumentVehicleModel dvm = new DocumentVehicleModel();

            var allDocuments = BookingsService.ViewDocumentAsync(bookingID);
            foreach (var item in allDocuments.Result)
            {
                if (item.VehicleDocumentID == vehicleDocumentID)
                {
                    dvm = item;
                }
            }
            var result = await BookingsService.DeleteDocumentAsync(dvm);
            return RedirectToAction("ViewDocuments", "Bookings", new { bookingsID = dvm.BookingID });
        }

        [HttpGet]
        public async Task<IActionResult> AuditTrail(int bookingID)
        {
            var bvm = new BookingsViewModel();
            bvm.BookingAuditModelList = await BookingsService.GetBookingAuditAsync(bookingID);
            bvm.BookingID = bookingID;
            return View(bvm);
        }
    }
}
