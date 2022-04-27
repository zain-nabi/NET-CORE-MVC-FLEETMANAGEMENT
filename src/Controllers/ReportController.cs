using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Triton.FleetManagement.Web.Helper;
using Triton.FleetManagement.Web.Models;
using Triton.Service.Data;

namespace Triton.FleetManagement.Web.Controllers
{
    [Authorize(Roles = "Admin Director, Managing Director, Service Advisor, Development Manager, Developer, Junior Developer, Bm, Admin Manager")]
    public class ReportController : Controller
    {
        public async Task<IActionResult> SalesPerCustomer()
        {
            var model = new SalesPerCustomerModel
            {
                CustomerList = await ARCUSService.GetAsync(),
                ShowReport = false,
                OESHDT = await OESHDTService.GetAsync(),
                Peroids = PeriodHelper.Peroids()

            };

            return View(model);
        }


        public async Task<IActionResult> SalesSummary()
        {
            var model = new SalesPerCustomerModel
            {
                OESHDT = await OESHDTService.GetAsync(),
                Peroids = PeriodHelper.Peroids(),
                ShowReport = false
            };

            return View(model);

        }

        public async Task<IActionResult> SalesPerCategory()
        {
            var model = new SalesPerCategoryModel
            {
                ICLOCList = await ICLOCService.GetAsync(),
                CategoryList = await ICCATGService.GetAsync(),
                OESHDT = await OESHDTService.GetAsync(),
                Peroids = PeriodHelper.Peroids(),
                ShowReport = false
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> SalesPerCustomer(SalesPerCustomerModel salesPerCustomerModel)
        {
            try
            {
                var model = new SalesPerCustomerModel
                {
                    CustomerList = await ARCUSService.GetAsync(),
                    ShowReport = true,
                    OESHDT = await OESHDTService.GetAsync(),
                    Peroids = PeriodHelper.Peroids(),
                    Url = "http://Tiger/ReportServer/Pages/ReportViewer.aspx?/TFM/TFMSalesPerCustomer&PERIOD=@PERIOD&YEAR=@YEAR&CUST=@CUST&rs:ParameterLanguage=&rc:Parameters=Collapsed&rs:Command=Render&rs:Format=HTML4.0"
                     .Replace("@PERIOD", salesPerCustomerModel.Month)
                     .Replace("@YEAR", salesPerCustomerModel.Year)
                     .Replace("@CUST", salesPerCustomerModel.SelectedCustomer.ToString())
                };

                return View(model);
            }
            catch (System.Exception)
            {
                throw;
            }

        }

        [HttpPost]
        public async Task<IActionResult> SalesSummary(SalesPerCustomerModel salesPerCustomerModel)
        {
            try
            {
                
                var model = new SalesPerCustomerModel
                {
                    CustomerList = await ARCUSService.GetAsync(),
                    ShowReport = true,
                    OESHDT = await OESHDTService.GetAsync(),
                    Peroids = PeriodHelper.Peroids(),
                    Url = "http://Tiger/ReportServer/Pages/ReportViewer.aspx?/TFM/TFMSalesSummary&PERIOD=@PERIOD&YEAR=@YEAR&rs:ParameterLanguage=&rc:Parameters=Collapsed&rs:Command=Render&rs:Format=HTML4.0"
                    .Replace("@PERIOD", salesPerCustomerModel.Month)
                    .Replace("@YEAR", salesPerCustomerModel.Year)

                };

                return View(model);
            }
            catch (System.Exception)
            {
                throw;
            }

        }

        [HttpPost]
        public async Task<IActionResult> SalesPerCategory(SalesPerCategoryModel salesPerCategoryModel)
        {
            try
            {
               
                var model = new SalesPerCategoryModel
                {
                    ICLOCList = await ICLOCService.GetAsync(),
                    CategoryList = await ICCATGService.GetAsync(),
                    ShowReport = true,
                    OESHDT = await OESHDTService.GetAsync(),
                    Peroids = PeriodHelper.Peroids(),
                    Url = $"http://Tiger/ReportServer/Pages/ReportViewer.aspx?/TFM/TFMSalesPerCategory&PERIOD=@PERIOD&YEAR=@YEAR&CATEGORY=@CATEGORY&LOCATION=@LOCATION&rs:ParameterLanguage=&rc:Parameters=Collapsed&rs:Command=Render&rs:Format=HTML4.0"
                    .Replace("@PERIOD", salesPerCategoryModel.Month)
                    .Replace("@YEAR", salesPerCategoryModel.Year)
                    .Replace("@CATEGORY", salesPerCategoryModel.SelectedCategory.ToString())
                    .Replace("@LOCATION", salesPerCategoryModel.SelectedICLOC.ToString())

                };

                return View(model);
            }
            catch (System.Exception)
            {

                throw;
            }
        }


    }
}
