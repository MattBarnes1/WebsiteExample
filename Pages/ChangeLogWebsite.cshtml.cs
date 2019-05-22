using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication5.Pages
{
    [AllowAnonymous]
    public class ChangeLogWebsiteModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public String ChangeLog { get; set; }
        public void OnGet()
        {
            ChangeLog = FileEditingService.ReadFileAsString("~/ChangeLog/Website.txt");
            ChangeLog = ChangeLog.Replace("\r\n", "<br>").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");

        }
    }
}