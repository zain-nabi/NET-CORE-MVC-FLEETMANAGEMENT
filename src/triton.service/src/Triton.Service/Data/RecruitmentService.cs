using System;
using System.Threading.Tasks;
using Triton.Service.Model.HRM.Tables;
using Triton.Service.Utils;

namespace Triton.Service.Data
{
    public class RecruitmentService
    {
        public static async Task<bool> InsertAsync(Recruitment recruitment)
        {
            return await RestApiHelper.InsertAsync(new Uri(UrlHelper.Api.RecruitmentApi, $"{UrlHelper.Controller.Recruitment}InsertAsync"), recruitment);
        }
    }
}
