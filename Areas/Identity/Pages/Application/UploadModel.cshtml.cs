using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication5.Data;

namespace WebApplication5.Areas.Identity.Pages.Application
{
    [Authorize(Roles = "HR")]
    public class UploadModelModel : PageModel
    {
        private UserManager<UserData> myUsers;
        private ApplicationDbContext myContext;

        public class FileUpload
        {
            [Required]
            [Display(Name = "Title")]
            [StringLength(60, MinimumLength = 3)]
            public string Title { get; set; }

            [Required]
            [Display(Name = "New Mesh")]
            public IFormFile UploadPublicSchedule { get; set; }
            
        }

        public UploadModelModel(UserManager<UserData> myUsers, ApplicationDbContext myContext)
        {
            this.myUsers = myUsers;
            this.myContext = myContext;
        }

        public void OnPostUploadFile(IFormFile aFile)
        {
            if (!ModelState.IsValid)
            {
                return;
            }
            String UserID = myUsers.GetUserId(User);
            Stream aStream = aFile.OpenReadStream();
            StreamReader myReader = new StreamReader(aStream);
            var Value = myReader.ReadToEnd();
            FileEditingService.WriteStringToFile(Value, "~/Home/" + UserID + "/Models/" + aFile.FileName);
            return;
        }
    


        public void OnGet()
        {

        }
    }
}