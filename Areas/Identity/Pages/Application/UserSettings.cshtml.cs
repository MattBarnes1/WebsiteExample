using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication5.Data;

namespace WebApplication5.Areas.Identity.Pages.Application
{
    public class PersonalSettingsModel : PageModel
    {
        public List<SelectListItem> PossibleBackgrounds = new List<SelectListItem>();
        private ApplicationDbContext myContext;
        private UserManager<UserData> myUsers;
        private string Error;

        public PersonalSettingsModel(ApplicationDbContext myContext, UserManager<UserData> myUsers)
        {
            this.myContext = myContext;
            this.myUsers = myUsers;
        }


        public JsonResult OnGetAjaxChangeBackground(string background)
        {                      
            LoadPossibleDefaultBackgrounds();
            if (!String.IsNullOrEmpty(background))
            {
                if(IsInPossibleBackground(background) != null && VerifyFileType(background))
                {
                    SetUserBackground(background);
                    return new JsonResult("");
                }
                else if(!VerifyFileType(background))
                {
                   return new JsonResult("Invalid File Type");
                }
                else
                {
                    return new JsonResult("Unspecified Error");
                }
            }
            else
            {
                return new JsonResult("Invalid selection was made! Value was empty!");
            }
        }

        private bool VerifyFileType(string background)
        {
            int lastIndex = background.LastIndexOf(".") + 1;
            if(lastIndex != -1)
            {
                string fileType = background.Substring(lastIndex, background.Length-lastIndex-1);
                Debug.Assert(!fileType.Contains('.'));
                if (fileType.Length != 3) return false;
                return (fileType.Contains("jpg") || fileType.Contains("png") || fileType.Contains("gif"));
            }
            return false;
        }

        [BindProperty]
        public String MyBackgroundURL { get; set; }
        [BindProperty]
        public SelectListItem SelectedBackground { get; set; }
        public void OnGet()
        {
            LoadPossibleDefaultBackgrounds();
            MyBackgroundURL = GetUserBackground();
            SelectListItem Return = IsInPossibleBackground(MyBackgroundURL);
            if (Return != null)
            {
                String retVal = Url.Page(Return.Value);
                SelectedBackground = Return;
                MyBackgroundURL = "Enter a website to use!";
            }
        }

        private SelectListItem IsInPossibleBackground(String Background)
        {
            foreach (SelectListItem A in PossibleBackgrounds)
            {
                if (A.Value.CompareTo(Background) == 0)
                {
                    return A;
                }
            }
            return null;
        }

        private bool SetUserBackground(String myString)
        {
            var myTasks = myUsers.GetUserAsync(User);
            myTasks.Wait();
            if(myTasks.IsCompletedSuccessfully)
            {
                myTasks.Result.BackgroundUrl = myString;
                myUsers.UpdateAsync(myTasks.Result);
                myContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GetUserBackground()
        {
           var myTasks = myUsers.GetUserAsync(User);
            myTasks.Wait();
            if(myTasks.IsCompletedSuccessfully)
            {
               return myTasks.Result.BackgroundUrl;
            }
            else
            {
                LocalRedirect("/Error");
                return "";
            }
        }

        private void LoadPossibleDefaultBackgrounds()
        {
            PossibleBackgrounds = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Pirates",
                    Value = "url(../../images/Pirates.gif)",
                },
                new SelectListItem()
                {
                    Text = "City Scape",
                    Value = "url(../../images/PEmY4dn.gif)",
                },
                new SelectListItem()
                {
                    Text = "Raining Castle",
                    Value = "url(../../images/Raining Castle.gif)",
                },
                new SelectListItem()
                {
                    Text = "Space Pizza",
                    Value = "url(../../images/Space Pizza.gif)",
                },
                new SelectListItem()
                {
                    Text = "Waterfall",
                    
                    Value = "url(../../images/Waterfall.gif)",
                },
                new SelectListItem()
                {
                    Text = "Zipping By",
                    Value = "url(../../images/Zipping By.gif)",
                },
                new SelectListItem()
                {
                    Text = "Flowing Water",
                    Value = "url(../../images/FlowingWater.gif)",
                }
            };


        }
    }
}