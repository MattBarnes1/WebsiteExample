using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication5
{
    public class UserClaimsPrincipal : UserClaimsPrincipalFactory<UserData, IdentityRole>
    {
        public UserClaimsPrincipal(
            UserManager<UserData> userManager
            , RoleManager<IdentityRole> roleManager
            , IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
        { }

        public async override Task<ClaimsPrincipal> CreateAsync(UserData user)
        {
            var principal = await base.CreateAsync(user);



            return principal;
        }
    }
}
