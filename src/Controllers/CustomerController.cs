using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Triton.FleetManagement.Web.Helper;
using Triton.FleetManagement.Web.Models;
using Triton.Service.Data;
using Triton.Service.Model.TritonFleetManagement.Custom;
using Triton.Service.Model.TritonFleetManagement.Tables;
using Triton.Service.Utils;

namespace Triton.FleetManagement.Web.Controllers
{
    [Authorize(Roles = "Admin Manager, Admin Director, Managing Director, Development Manager, Developer, Junior Developer, Bm")]
    public class CustomerController : Controller
    {

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var customer = await CustomerFleetManagementService.LookUpCodesAsync();
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Index(CustomerModel customerModel)
        {
                if (await CustomerFleetManagementService.IsCustomerNameExists(customerModel.Customer.Name))
                {
                    var customerService = await CustomerFleetManagementService.LookUpCodesAsync();
                    customerService.ErrorMessage = string.Format("CustomerName {0} exists on database ", customerModel.Customer.Name);
                    customerService.Customer = customerModel.Customer;
                    customerService.ContactTypes = customerModel.ContactTypes;

                    return View("Index", customerService);
                }
                customerModel.Customer.CreatedByUserID = User.GetUserId();
                await CustomerFleetManagementService.InsertAsync(customerModel);

            string redirectUrl = string.Format("/Customer/GetAllCustomers");

            return RedirectToAction("Message", "Home", new { type = StringHelper.Types.SaveSuccess, url = redirectUrl });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customerViewModel = new CustomerViewModel();
            customerViewModel.CustomerModelList = await CustomerFleetManagementService.GetAllCustomersAsync();
            return View(customerViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersById(int customerID)
        {
            var customer = await CustomerFleetManagementService.GetCustomerByIdAsync(customerID);
            if(customer.CountFiles == null)
            {
                customer.CountFiles = new CustomerDocumentRepositoryModel();
                customer.CountFiles.DocumentRepositoryID = 0;
            }
            return View(customer);
        }

        [HttpGet]
        public async Task<IActionResult> Files(int customerID)
        {
            var documentSizeConverter = new DocumentSizeConverter();
            var customerViewModel = new CustomerViewModel();
            customerViewModel.CustomerModel = await CustomerFleetManagementService.GetCustomerByIdAsync(customerID);
            customerViewModel.ITCTypeDocumentList = new SelectList(customerViewModel.CustomerModel.ITCType, "LookUpCodeID", "Name");
            if(customerViewModel.CustomerModel.CountFiles == null)
            {
                customerViewModel.CustomerModel.CountFiles = new CustomerDocumentRepositoryModel();
                customerViewModel.CustomerModel.CountFiles.DocumentRepositoryID = 0;
            }
            foreach (var item in customerViewModel.CustomerModel.CustomerDocumentRepositoryList)
            {
                item.fileExtension = new FileInfo(item.ImgName);
                item.DocumentSize = documentSizeConverter.SizeSuffix(item.ImgLength, 1);
            }
            return View(customerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Files(List<IFormFile> File, int[] customerID, int categoryDocument)
        {           
            foreach (var file in File)
            {
                if(file.Length > 0)
                {
                    var document = new CustomerDocumentRepositoryModel
                    {
                        ImgName = file.FileName,
                        ImgContentType = file.ContentType,
                        ImgLength = Convert.ToInt32(file.Length),
                        CreatedByUserID = User.GetUserId(),
                        customerID = customerID[0],    
                        DocumentCategoryLCID = categoryDocument
                    };
                    var dataStream = new MemoryStream();
                    await file.CopyToAsync(dataStream);
                    document.ImgData = dataStream.ToArray();
                    await CustomerFleetManagementService.InsertDocument(document);
                }
            }

            return RedirectToAction("Files", "Customer", new { customerID = customerID[0] });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFiles(int documentRepositoryID, int customerID)
        {
            var customerDocumentRepository = new CustomerDocumentRepositoryModel();
            customerDocumentRepository.DocumentRepositoryID = documentRepositoryID;
            customerDocumentRepository.customerID = customerID;
            customerDocumentRepository.DeletedByUserID = User.GetUserId();
            await CustomerFleetManagementService.DeleteFile(customerDocumentRepository);

            return RedirectToAction("GetCustomersById", "Customer", new { customerID = customerID });
        }

        [HttpGet]
        public async Task<IActionResult> Download(int DocumentRepositoryID)
        {
            var documents = await CustomerFleetManagementService.ViewDocumentAsync(DocumentRepositoryID);
            return File(documents.ImgData, documents.ImgContentType);
        }


        [HttpGet]
        public async Task<IActionResult> DeleteCustomer(int customerID)
        {
            var customer = await CustomerFleetManagementService.GetCustomerByIdAsync(customerID);
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> EditJobCustomer(CustomerModel customerModel)
        {

            customerModel.Customer.CreatedByUserID = User.GetUserId();
            var result = await CustomerFleetManagementService.UpdateAsync(customerModel);

            const string redirectUrl = "/Customer/GetAllCustomers";

            return result ? RedirectToAction("Message", "Home", new { StringHelper.Types.UpdateSuccess, url = redirectUrl })
                          : RedirectToAction("Message", "Home", new { StringHelper.Types.UpdateFailed, url = redirectUrl });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteCustomer(CustomerViewModel customer)
        {

            customer.CustomerModel.Customer.DeletedByUserID = User.GetUserId();
            var customerViewModel = new CustomerViewModel();
            customerViewModel.CustomerModel = await CustomerFleetManagementService.GetCustomerByIdAsync(customer.CustomerModel.Customer.CustomerID);
            long lookupCodeID = 484;
            customer.CustomerModel.Customer.DeletedByUserID = User.GetUserId();
            customer.CustomerModel.Customer.AccountStatusTypeLCID = Convert.ToInt32(lookupCodeID);
            var result = await CustomerFleetManagementService.DeleteAsync(customer.CustomerModel.Customer);

            const string redirectUrl = "/Customer/GetAllCustomers";

            return result ? RedirectToAction("Message", "Home", new { StringHelper.Types.UpdateSuccess, url = redirectUrl })
                          : RedirectToAction("Message", "Home", new { StringHelper.Types.UpdateFailed, url = redirectUrl });
        }


        [HttpGet]
        public async Task<IActionResult> DeletePartialView(int customerID)
        {
            var customerViewModel = new CustomerViewModel();
            customerViewModel.CustomerModel = await CustomerFleetManagementService.GetCustomerByIdAsync(customerID);
            return PartialView("DeletePartialView", customerViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AuditTrail(int customerID)
        {
            var cvm = new CustomerViewModel();
            cvm.CustomerAuditModelList = await CustomerFleetManagementService.GetCustomerAuditAsync(customerID);
            cvm.customerID = customerID;
            cvm.CustomerModel = await CustomerFleetManagementService.GetCustomerByIdAsync(customerID);
            if (cvm.CustomerModel.CountFiles == null)
            {
                cvm.CustomerModel.CountFiles = new CustomerDocumentRepositoryModel();
                cvm.CustomerModel.CountFiles.DocumentRepositoryID = 0;
            }
            return View(cvm);
        }
    }
}
