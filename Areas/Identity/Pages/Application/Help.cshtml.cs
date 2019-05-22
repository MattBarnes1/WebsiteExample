using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication5.Areas.Identity.Pages.Application
{
    [AllowAnonymous]
    public class HelpModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}