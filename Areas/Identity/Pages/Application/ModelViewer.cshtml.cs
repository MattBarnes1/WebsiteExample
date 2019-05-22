using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication5.Areas.Identity.Pages.Application
{
    public class ModelViewerModel : PageModel
    {
        public void OnGet()
        {
        }

        public string GetVerticies()
        {
            return "[]";
        }
    }
}