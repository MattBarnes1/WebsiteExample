using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication5.Pages
{
    [AllowAnonymous]
    public class DevBlogsModel : PageModel
    {

        public class UserWithBlog
        {
            public String User; //TODO: make this public name when importing Userdata
            public String Blog;
        }

        Dictionary<String, List<String>> UserBlogNames = new Dictionary<string, List<String>>();

        public DevBlogsModel(UserManager<UserData> myIDUser)
        {
            this.myIDUser = myIDUser;
            //TODO: add a search that goes through UserData ID for public name and append to create it.


          /*  String[] myDirectories = FileEditingService.GetAllHomeDirectories();
            for (int i = 0; i < myDirectories.Length; i++)
            {
                myDirectories[i] = (myDirectories[i] + Path.DirectorySeparatorChar + "DevBlogs");
                string[] myFiles = FileEditingService.GetFilenamesInDirectory(myDirectories[i]);
                if (myFiles != null)
                {
                }
            }*/
        }

        private UserManager<UserData> myIDUser;


        public String[] GetUserWebDevFiles(String Id)
        {
            return null;
        }

        [BindProperty]
        String myHtmlCode { get; set; }

        public void OnGet()
        {
            if (RouteData.Values["UserName"] != null && RouteData.Values["Blog"] != null)
            {
               
            }

        }

    }
}