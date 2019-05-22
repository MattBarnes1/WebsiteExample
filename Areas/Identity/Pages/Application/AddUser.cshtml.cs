using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication5.Areas.Identity.Pages.Application
{
    [Authorize(Roles = "HR")]
    public class AddUserModel : PageModel
    {

        public List<String> PossibleRoles = new List<String>();
        public class NewUserData
        {
            [Required]
            public String UserName { get; set; }
            [Required]
            public String UserPassword { get; set; }
            [Required]
            public bool[] UserRoles { get; set; }
        }

        private void SetupRoles()
        {
            if (PossibleRoles == null)
            {
                PossibleRoles = new List<string>();
            }
            else
            {
                PossibleRoles.Clear();
            }
            var Callthis = myRoles.Roles.ToList();
            for (int r = 0; r < myRoles.Roles.Count(); r++)
            {
                PossibleRoles.Add(Callthis[r].Name);
            }
        }

        UserManager<UserData> myUsers;
        RoleManager<IdentityRole> myRoles;
        public AddUserModel(UserManager<UserData> myUsers, RoleManager<IdentityRole> myRoles)
        {
            wasCompletedSuccessfully = false;
            //TODO: get possible roles
            this.myRoles = myRoles;
            this.myUsers = myUsers;
        }
        [BindProperty]
        public NewUserData MyData { get; set; }

        [TempData]
        public string Error { get; set; }

        bool wasCompletedSuccessfully;

        public void OnPost()
        {
            SetupRoles();
            Error = "";
            if (String.IsNullOrEmpty(MyData.UserName)) Error = "Username is Empty!";
            if (String.IsNullOrEmpty(MyData.UserPassword)) Error = " Password is Empty!";
            if (MyData.UserRoles.All<bool>(delegate (bool a)
            {
                return a == false;
            }))
            {
                Error = "No Roles Selected!";
            }
            if (!String.IsNullOrEmpty(Error))
            {
                return;
            }
            UserData newUser = new UserData { UserName = MyData.UserName, SecurityStamp = Guid.NewGuid().ToString(), Joined = DateTime.Now};
            Task<IdentityResult> myResult = myUsers.CreateAsync(newUser, MyData.UserPassword);
            myResult.Wait();
            List<String> UserRoles = new List<string>();
            if (myResult.IsCompletedSuccessfully)
            {
                for (int i = 0; i < PossibleRoles.Count; i++)
                {
                    if(MyData.UserRoles[i])
                    {
                        UserRoles.Add(PossibleRoles[i]);
                    }
                }
               var retVal = myUsers.AddToRolesAsync(newUser, UserRoles.ToArray());
                retVal.Wait();
                if(!retVal.IsCompletedSuccessfully)
                {
                    Error = "Failed to add roles: " + UserRoles.ToString() + " Database Restored!";
                    myUsers.DeleteAsync(newUser).Wait();
                }
            }
            else
            {
                Error = "Failed to add user: " + myResult.Exception.ToString();
            }
            if(String.IsNullOrEmpty(Error))
            {
                FileEditingService.CreateUserHome(newUser, UserRoles);
            }
            Error = "Success!";
        }

        public void OnGet()
        {
            SetupRoles();
            wasCompletedSuccessfully = false;
            MyData = new NewUserData()
            {
                UserName = "",
                UserPassword = "",
                UserRoles = new bool[myRoles.Roles.Count()]
            };
        }
    }
}