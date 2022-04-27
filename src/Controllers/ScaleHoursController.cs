using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Triton.FleetManagement.Web.Models;
using Triton.Service.Data;
using Triton.Service.Model.TritonFleetManagement.Custom;
using Triton.Service.Model.TritonFleetManagement.Tables;
using Triton.Service.Utils;

namespace Triton.FleetManagement.Web.Controllers
{
    [Authorize(Roles = "Admin Director, Service Advisor, Managing Director, Development Manager, Developer, Junior Developer, Bm")]
    public class ScaleHoursController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Search()
        {
            var scaleHourViewModel = new ScaleHourViewModel();
            scaleHourViewModel.ScaleHoursModelList = await ScaleHourService.GetEmployeesAndScaleHours();
            scaleHourViewModel.ShowReport = true;
            return View(scaleHourViewModel);
        }

        //[HttpPost]
        //public async Task<IActionResult> Search(int costScale)
        //{
        //    var scaleHourViewModel = new ScaleHourViewModel();
        //    scaleHourViewModel.ScaleHoursModelList = await ScaleHourService.GetEmployeesAndScaleHoursByCostScale(costScale);
        //    scaleHourViewModel.ShowReport = true;
        //    return View(scaleHourViewModel);
        //}


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var scaleHourViewModel = new ScaleHourViewModel();
            scaleHourViewModel.CostScaleUsersModelList = new SelectList(await ScaleHourService.GetEmployees(), "UserID", "Name");
            scaleHourViewModel.quantitySelectorValue = 1;
            return View(scaleHourViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ScaleHourViewModel model)
        {
            var Exist = await ScaleHourService.CheckIfEmployeeScaleHourExists(model.ScaleHoursModel.EmployeeID, model.ScaleHoursModel.CostScaleHour);
            if (Exist.EmployeeID == 1)
            {
                model.CostScaleUsersModelList = new SelectList(await ScaleHourService.GetEmployees(), "UserID", "Name");
                model.ScaleExistErrorMessage = "Employee scale hour already exist";
                if(model.ScaleHoursModel.CostScaleHour == 0)
                {
                    model.quantitySelectorValue = 1;
                }
                else
                {
                    model.quantitySelectorValue = model.ScaleHoursModel.CostScaleHour;
                }
                return View(model);
            }

            var scaleHours = new ScaleHours();
            scaleHours.EmployeeID = model.ScaleHoursModel.EmployeeID;
            scaleHours.CostScaleHour = model.ScaleHoursModel.CostScaleHour;
            scaleHours.CreatedByUserID = User.GetUserId();
            scaleHours.CreatedOn = DateTime.Now;
            await ScaleHourService.InsertScaleHour(scaleHours);
            const string redirectUrl = "/ScaleHours/Search";
            return RedirectToAction("Message", "Home", new { type = StringHelper.Types.SaveSuccess, url = redirectUrl });
        }

        [HttpGet]
        public async Task<IActionResult> Update(int employeeID)
        {
            var scaleHourViewModel = new ScaleHourViewModel();
            scaleHourViewModel.ScaleHoursModel = await ScaleHourService.GetEmployeeDetailsByID(employeeID);
            scaleHourViewModel.employeeID = employeeID;
            return View(scaleHourViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ScaleHourViewModel model)
        {
            var scaleHours = new ScaleHours();
            scaleHours.CostScaleHour = model.ScaleHoursModel.CostScaleHour;
            scaleHours.ScaleHourID = model.ScaleHoursModel.ScaleHourID;
            await ScaleHourService.UpdateScaleHour(scaleHours);
            const string redirectUrl = "/ScaleHours/Search";
            return RedirectToAction("Message", "Home", new { type = StringHelper.Types.UpdateSuccess, url = redirectUrl });
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int employeeID)
        {
            var scaleHourViewModel = new ScaleHourViewModel();
            scaleHourViewModel.ScaleHoursModel = await ScaleHourService.GetEmployeeDetailsByID(employeeID);
            return View(scaleHourViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ScaleHourViewModel model)
        {
            model.ScaleHoursModel.DeletedByUserID = User.GetUserId();
            await ScaleHourService.DeleteScaleHour(model.ScaleHoursModel);
            const string redirectUrl = "/ScaleHours/Search";
            return RedirectToAction("Message", "Home", new { type = StringHelper.Types.UpdateSuccess, url = redirectUrl });
        }

        [HttpGet]
        public async Task<IActionResult> AuditTrail(int employeeID)
        {
            var svm = new ScaleHourViewModel();
            svm.ScaleHourAuditModelList = await ScaleHourService.GetScaleHourAuditAsync(employeeID);
            svm.employeeID = employeeID;
            return View(svm);
        }
    }
}
