using System;
using System.Linq;
using System.Threading.Tasks;
using Triton.Service.Model.TritonFleetManagement.Custom;
using Triton.Service.Model.TritonFleetManagement.StoredProcs;
using Triton.Service.Data;
using Triton.FleetManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Triton.Service.Utils;

namespace Triton.FleetManagement.Web.Helper
{
    public class VehicleHelper : Controller
    {
        public VehicleModel CreateVehicle(VehicleViewModel model, int UserID)
        {
            VehicleModel vehicleModel = new VehicleModel();
            vehicleModel.Vehicles = model.Vehicle.Vehicles;
            vehicleModel.Vehicles.CreatedByUserID = UserID;
            vehicleModel.Vehicles.CreatedOn = System.DateTime.Now;
            vehicleModel.Licenses = model.Vehicle.Licenses;
            vehicleModel.TailLiftServices = model.Vehicle.TailLiftServices;

            return vehicleModel;
        }

        public proc_Vehicle_Update UpdateVehicle(VehicleViewModel model)
        {
            var procUpdate = new proc_Vehicle_Update();

            procUpdate.VehicleID = model.VehicleDetailsByID.VehicleID;
            procUpdate.CustomerID = model.VehicleDetailsByID.CustomerID;
            procUpdate.RegistrationNumber = model.VehicleDetailsByID.RegistrationNumber;
            procUpdate.FleetNumber = model.VehicleDetailsByID.FleetNumber;
            procUpdate.VinNumber = model.VehicleDetailsByID.VinNumber;
            procUpdate.EngineNumber = model.VehicleDetailsByID.EngineNumber;
            procUpdate.VehicleYear = model.VehicleDetailsByID.VehicleYear;
            procUpdate.GVM = model.VehicleDetailsByID.GVM;
            procUpdate.ServiceIntervalLCID = model.VehicleDetailsByID.ServiceIntervalLCID;
            if(model.VehicleDetailsByID.TailLift == false)
            {
                procUpdate.TailLift = model.VehicleDetailsByID.TailLift;
                procUpdate.TailLiftTypeLCID = null;
                procUpdate.ServiceDate = null;
                procUpdate.Description = null;
            }
            else
            {
                procUpdate.TailLift = model.VehicleDetailsByID.TailLift;
                procUpdate.Description = model.VehicleDetailsByID.tlsDescription;
                if (model.VehicleDetailsByID.TailLiftTypeLCID == 0)
                {
                    procUpdate.TailLiftTypeLCID = null;
                }
                else
                {
                    procUpdate.TailLiftTypeLCID = model.VehicleDetailsByID.TailLiftTypeLCID;
                }
                string val = model.VehicleDetailsByID.tlsServiceDate.ToShortDateString();
                if (val == "0001/01/01")
                {
                    procUpdate.ServiceDate = null;
                }
                else
                {
                    procUpdate.ServiceDate = model.VehicleDetailsByID.tlsServiceDate;
                }
            }
            

            procUpdate.VehicleClassLCID = model.VehicleDetailsByID.VehicleClassLCID;
            procUpdate.VehicleBrandLCID = model.VehicleDetailsByID.VehicleBrandLCID;                    
            procUpdate.LicenseNumber = model.VehicleDetailsByID.LicenseNumber;
            procUpdate.Expiry = model.VehicleDetailsByID.Expiry;

            return procUpdate;
        }
    }
}
