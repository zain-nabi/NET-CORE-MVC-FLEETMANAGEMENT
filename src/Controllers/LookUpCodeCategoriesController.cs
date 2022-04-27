using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Triton.FleetManagement.Web.Models;
using Triton.Service.Data;
using Triton.Service.Utils;

namespace Triton.FleetManagement.Web.Controllers
{
    [Authorize(Roles = "Admin Director, Development Manager, Developer, Junior Developer, Bm")]
    public class LookUpCodeCategoriesController : Controller
    {
        private const int _systemId = 30;

        [HttpGet]
        public async Task<IActionResult> LookUpCodeCategories()
        {
            var lookUpCodeCategoryModel = new LookUpCodeCategoryViewModel { LookUpCodeCategoriesList = await LookUpCodeCategoriesService.GetAllLookUpCodeCategories(_systemId) };
            return View(lookUpCodeCategoryModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int LookUpCodeCategoryID)
        {
            var lookUpCodeCategoryModel = new LookUpCodeCategoryViewModel();
            lookUpCodeCategoryModel.LookUpCodeCategories = await LookUpCodeCategoriesService.GetLookUpCodeCategoryByID(LookUpCodeCategoryID);
            return View(lookUpCodeCategoryModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(LookUpCodeCategoryViewModel model)
        {
            var result = await LookUpCodeCategoriesService.UpdateLookUpCodeAsync(model.LookUpCodeCategories);

            const string redirectUrl = "/LookUpCodeCategories/LookUpCodeCategories";

            return result ? RedirectToAction("Message", "Home", new { type = StringHelper.Types.UpdateSuccess, url = redirectUrl })
                          : RedirectToAction("Message", "Home", new { type = StringHelper.Types.UpdateFailed, url = redirectUrl });
        }
    }
}
