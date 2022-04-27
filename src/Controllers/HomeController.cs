using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Triton.Model.Utils;
using Triton.FleetManagement.Web.Models;
using System.Threading.Tasks;
using Triton.FleetManagement.Web.Helper;
using System;
using Triton.Service.Data;
using Newtonsoft.Json;
using System.Linq;

namespace Triton.FleetManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly IClaimsTransformation _claims;

        public HomeController(IClaimsTransformation claims)
        {
            _claims = claims;
        }

        //Administrator,IT,Manco
        //[Authorize(Roles = "Admin,BM,IT")]
        public async Task<IActionResult> Index()
        {
            var dashboardViewModel = new DashboardViewModel();
            DateTime startDate = DateTime.Now.FirstDayOfWeek();
            DateTime endDate = DateTime.Now.LastDayOfWeek();
            dashboardViewModel.DashboardModel = await TFMDashboardService.GetAllAsync(startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            var barGraphLabels = dashboardViewModel.DashboardModel.BookingsBarGraphModelList.ToList().Select(x => x.DayOfTheWeek.ToString("dd MMMM yyyy")).ToList();
            var barGraphValues = dashboardViewModel.DashboardModel.BookingsBarGraphModelList.ToList().Select(x => x.Bookings).ToList();
            dashboardViewModel.barGraphLabels = JsonConvert.SerializeObject(barGraphLabels);
            dashboardViewModel.barGraphValues = JsonConvert.SerializeObject(barGraphValues);
            return View(dashboardViewModel);
        }

        //[Authorize(Roles = "Admin, BM")]
        public IActionResult Message(string type, string url)
        {
            if (url == null)
            {
                url = "Home/Index";
            }

            var model = new MessageModel
            {
                Message = Triton.Service.Utils.StringHelper.Html.UpdateRecordSuccessMessage,
                //Route = "Index",
                // Controller = "Home",
                Title = Triton.Service.Utils.StringHelper.Html.UpdateRecordSuccessTitle,
                Icon = "fas fa-check-circle text-success",
                Url = url
            };

            switch (type)
            {
                case Triton.Service.Utils.StringHelper.Types.NoRecords:
                    model.Title = "Oops";
                    model.Message = "Something has gone wrong... Contact IT";
                    model.Icon = Triton.Service.Utils.StringHelper.Html.FailedIcon;
                    model.Type = "NoRecords";
                    break;
                case Triton.Service.Utils.StringHelper.Types.UpdateSuccess:
                    model.Message = Triton.Service.Utils.StringHelper.Html.UpdateRecordSuccessMessage;
                    model.Title = Triton.Service.Utils.StringHelper.Html.UpdateRecordSuccessTitle;
                    model.Type = null;
                    break;
                case Triton.Service.Utils.StringHelper.Types.UpdateFailed:
                    model.Title = Triton.Service.Utils.StringHelper.Html.UpdateRecordFailedTitle;
                    model.Message = Triton.Service.Utils.StringHelper.Html.UpdateRecordFailedMessage;
                    model.Icon = Triton.Service.Utils.StringHelper.Html.FailedIcon;
                    model.Type = null;
                    break;
                case Triton.Service.Utils.StringHelper.Types.SaveSuccess:
                    model.Title = Triton.Service.Utils.StringHelper.Html.SaveRecordSuccessMessage;
                    model.Message = Triton.Service.Utils.StringHelper.Html.SaveRecordSuccessTitle;
                    model.Icon = Triton.Service.Utils.StringHelper.Html.SuccessIcon;
                    model.Type = null;
                    break;
                case Triton.Service.Utils.StringHelper.Types.DeleteSuccess:
                    model.Title = Triton.Service.Utils.StringHelper.Html.DeleteRecordSuccessMessage;
                    model.Message = Triton.Service.Utils.StringHelper.Html.DeleteRecordSuccessTitle;
                    model.Icon = Triton.Service.Utils.StringHelper.Html.SuccessIcon;
                    model.Type = null;
                    break;
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
