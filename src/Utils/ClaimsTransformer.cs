using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using Triton.Service.Data;
using Triton.Service.Utils;

namespace TritonFleetManagement.Web.Utils
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        private readonly IConfiguration _configuration;

        public ClaimsTransformer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var database = $"{_configuration.GetSection("System").GetSection("Database").Value}";

            //var user = await _userService.FindBysAmAccountName(principal.GetUserName().Replace("TRITONEXPRESS\\", ""), database);
            //_connection.GetAsync<UserInformation>(StringHelpers.Controllers.Users, $"/{sAmAccountName}/{database}");
            var user = await UserService.FindBysAmAccountName(principal.GetUserName().Replace("TRITONEXPRESS\\", ""), "TritonFleetManagement");

            var ci = (ClaimsIdentity)principal.Identity;
            ci.AddClaim(new Claim("UserID", user.UserID.ToString()));
            ci.AddClaim(new Claim("Name", $"{user.FirstName} {user.LastName}"));
            ci.AddClaim(new Claim("EmployeeID", user.EmployeeID.ToString()));
            ci.AddClaim(new Claim("CostCentreID", user.CostCentreID.ToString()));
            ci.AddClaim(new Claim("sAMAccountName", user.sAMAccountName));
            ci.AddClaim(new Claim("RoleNames", user.RoleNames));
            ci.AddClaim(new Claim("Email", user.Email));
            ci.AddClaim(new Claim("JobProfile", user.JobProfile));
            ci.AddClaim(new Claim("BranchName", user.BranchName));
           
            var roleSplit = user.RoleNames.Split(",");

            foreach (var item in roleSplit)
            {
                //var c = new Claim(ClaimTypes.Role, item);
                var c = new Claim(ci.RoleClaimType, item);
                ci.AddClaim(c);
            }

            return principal;
        }
    }
}
