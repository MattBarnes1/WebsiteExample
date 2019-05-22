using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication5.Pages
{
    [AllowAnonymous]
    public class NewIndexModel : PageModel
    {
        private SignInManager<UserData> myRoleHandler;

        public NewIndexModel(SignInManager<UserData> myRole)
        {
            this.myRoleHandler = myRole;
        }

        public void OnGet()
        {
           if(myRoleHandler.IsSignedIn(User))
            {
                LocalRedirect("~/Identity/Application/Dashboard");
                
            }
        }
    }
}