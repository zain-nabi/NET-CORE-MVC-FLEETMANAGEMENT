using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Triton.FleetManagement.Web.Models;
using Triton.Service.Model.TritonFleetManagement.Custom;
using Triton.Service.Model.TritonFleetManagement.StoredProcs;
using Triton.Service.Model.TritonFleetManagement.Tables;

namespace Triton.FleetManagement.Web.Helper
{
    public class BookingsHelper
    {
        public proc_BookingDetails_GetByID DeleteBookings(proc_BookingDetails_GetByID model, int UserID)
        {
            var bookings = new proc_BookingDetails_GetByID();
            bookings.BookingsID = model.BookingsID;
            bookings.DeletedOn = model.DeletedOn;
            bookings.DeletedByUserID = UserID;

            return bookings;
        }

        public BookingsModel UpdateBooking(BookingsViewModel model, int UserID)
        {
            var bookingsModel = new BookingsModel();

            bookingsModel.Bookings = new Bookings()
            {
                BookingsID = model.BookingsModel.Bookings.BookingsID,
                CustomerID = model.BookingsModel.Bookings.CustomerID,
                VehicleID = model.BookingsModel.Bookings.VehicleID,
                ServiceCategoryTypesLCID = model.BookingsModel.Bookings.ServiceCategoryTypesLCID,
                MileAgeOrHourLCID = model.BookingsModel.Bookings.MileAgeOrHourLCID,
                MileAge = model.BookingsModel.Bookings.MileAge,
                Hour = model.BookingsModel.Bookings.Hour,
                EstimatedArrival = model.BookingsModel.Bookings.EstimatedArrival,
                ActualArrival = model.BookingsModel.Bookings.ActualArrival,
                Authorisation = model.BookingsModel.Bookings.Authorisation,
                Notes = model.BookingsModel.Bookings.Notes,
                QuotationsLCID = model.BookingsModel.Bookings.QuotationsLCID,
                BranchID = model.BookingsModel.Bookings.BranchID,
                OrderNumber = model.BookingsModel.Bookings.OrderNumber,
                isJobCard = model.BookingsModel.Bookings.isJobCard,
                StatusLCID = model.BookingsModel.Bookings.StatusLCID,
                MechanicEmployeeID = model.BookingsModel.Bookings.MechanicEmployeeID,
                CreatedOn = model.BookingsModel.Bookings.CreatedOn,
                CreatedByUserID = UserID,
            };
            bookingsModel.BookingReasonLCID = model.BookingsModel.BookingReasonLCID;
            bookingsModel.DeleteBookingReasonLCID = model.BookingsModel.DeleteBookingReasonLCID;

            return bookingsModel;
        }
    }
}
