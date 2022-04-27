using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Triton.FleetManagement.Web.Models;
using Triton.Service.Data;
using Triton.Service.Utils;

namespace Triton.FleetManagement.Web.Controllers
{
    [Authorize(Roles = "Admin Director, Development Manager, Developer, Junior Developer, Bm")]
    public class LookUpCodesController : Controller
    {
        private const int _systemId = 30;

        [HttpGet]
        public async Task<IActionResult> LookUpCodes()
        {
            var lookUpCodesModel = new LookUpCodeCategoryViewModel { LookUpCodesList = await LookUpCodesService.GetAllLookUpCodes(_systemId) };
            return View(lookUpCodesModel);
        }

        [HttpGet]
        public async Task<IActionResult> LookUpCodesPerCategory(int LookupcodeCategoryID, int CreatedByUserID, string Category)
        {
            var lookUpCodesModel = new LookUpCodeCategoryViewModel 
            { 
                LookUpCodesList = await LookUpCodesService.GetLookUpCodesPerCategory(LookupcodeCategoryID, _systemId) ,
                LookUpCodeCategoryID = LookupcodeCategoryID,
                CreatedByUserID = CreatedByUserID,
                Category = Category
            };
            return View(lookUpCodesModel);
        }

        [HttpGet]
        public IActionResult Create(int LookUpCodeCategoryID, int CreatedByUserID, string Category)
        {
            var lookUpCodesModel = new LookUpCodeCategoryViewModel()
            {
                LookUpCodeCategoryID = LookUpCodeCategoryID,
                CreatedByUserID = CreatedByUserID,
                Category = Category
            };
            return View(lookUpCodesModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LookUpCodeCategoryViewModel model)
        {
            model.LookUpCodes.CreatedByUserID = model.CreatedByUserID;
            model.LookUpCodes.LookupcodeCategoryID = model.LookUpCodeCategoryID;
            model.LookUpCodes.CreatedOn = System.DateTime.Now;
            var result = await LookUpCodesService.InsertLookUpCodeAsync(model.LookUpCodes);

            string redirectUrl = string.Format("/LookUpCodes/LookUpCodesPerCategory?LookUpCodeCategoryID={0}&CreatedByUserID={1}&Category={2}", model.LookUpCodes.LookupcodeCategoryID, model.LookUpCodes.CreatedByUserID, model.Category);

            return result ? RedirectToAction("Message", "Home", new { type =  StringHelper.Types.SaveSuccess, url = redirectUrl })
                          : RedirectToAction("Message", "Home", new { type =  StringHelper.Types.SaveFailed, url = redirectUrl });
        }


        [HttpGet]
        public async Task<IActionResult> Update(int LookUpCodeID, string Category)
        {
            var lookUpCodeCategoryModel = new LookUpCodeCategoryViewModel();
            lookUpCodeCategoryModel.LookUpCodes = await LookUpCodesService.GetLookUpCodeByID(LookUpCodeID);
            lookUpCodeCategoryModel.Category = Category;
            return View(lookUpCodeCategoryModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(LookUpCodeCategoryViewModel model)
        {
            var result = await LookUpCodesService.UpdateLookUpCodeAsync(model.LookUpCodes);

            string redirectUrl = string.Format("/LookUpCodes/LookUpCodesPerCategory?LookUpCodeCategoryID={0}&CreatedByUserID={1}&Category={2}", model.LookUpCodes.LookupcodeCategoryID, model.LookUpCodes.CreatedByUserID, model.Category);

            return result ? RedirectToAction("Message", "Home", new { type = StringHelper.Types.UpdateSuccess, url = redirectUrl })
                          : RedirectToAction("Message", "Home", new { type = StringHelper.Types.UpdateFailed, url = redirectUrl });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int LookUpCodeID, string Category)
        {
            var lookUpCodeCategoryModel = new LookUpCodeCategoryViewModel();
            lookUpCodeCategoryModel.LookUpCodes = await LookUpCodesService.GetLookUpCodeByID(LookUpCodeID);
            lookUpCodeCategoryModel.Category = Category;
            return View(lookUpCodeCategoryModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(LookUpCodeCategoryViewModel model)
        {
            bool result;
            if (model.LookUpCodes.DeletedByUserID == null)
            {
                result = await LookUpCodesService.DeleteLookUpCodeAsync(model.LookUpCodes);
                string redirectUrl = string.Format("/LookUpCodes/LookUpCodesPerCategory?LookUpCodeCategoryID={0}&CreatedByUserID={1}&Category={2}", model.LookUpCodes.LookupcodeCategoryID, model.LookUpCodes.CreatedByUserID, model.Category);

                return result ? RedirectToAction("Message", "Home", new { type = StringHelper.Types.UpdateSuccess, url = redirectUrl })
                              : RedirectToAction("Message", "Home", new { type = StringHelper.Types.UpdateFailed, url = redirectUrl });
            }
            else
            {
                 model.LookUpCodes.DeletedByUserID = null;
                 model.LookUpCodes.DeletedOn = null;
                 result = await LookUpCodesService.UpdateLookUpCodeAsync(model.LookUpCodes);
                 string redirectUrl = string.Format("/LookUpCodes/LookUpCodesPerCategory?LookUpCodeCategoryID={0}&CreatedByUserID={1}&Category={2}", model.LookUpCodes.LookupcodeCategoryID, model.LookUpCodes.CreatedByUserID, model.Category);

                 return result ? RedirectToAction("Message", "Home", new { type = StringHelper.Types.UpdateSuccess, url = redirectUrl })
                              : RedirectToAction("Message", "Home", new { type = StringHelper.Types.UpdateFailed, url = redirectUrl });
            }
        }
    }
}
