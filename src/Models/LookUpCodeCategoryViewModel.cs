using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Triton.Model.TritonGroup.Tables;
using Triton.Service.Model.TritonGroup.Custom;

namespace Triton.FleetManagement.Web.Models
{
    public class LookUpCodeCategoryViewModel
    {
        public List<LookupCodeCategoriesModel> LookUpCodeCategoriesList { get; set; }
        public List<LookupCodeCategoriesModel> LookUpCodesList { get; set; }
        public LookUpCodes LookUpCodes { get; set; }
        public LookupCodeCategories LookUpCodeCategories { get; set; }
        public int CreatedByUserID { get; set; }
        public int LookUpCodeCategoryID { get; set; }
        public string  Category { get; set; }
    }
}
