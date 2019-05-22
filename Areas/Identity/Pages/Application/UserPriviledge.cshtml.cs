using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication5.Data;

namespace WebApplication5.Areas.Identity.Pages.Application
{
    [Authorize(Roles = "HR")]
    public class UserViewerModel : PageModel
    { 
        public class OutputData
        {
            [BindProperty]
            public String ID { get; set; }
            [BindProperty]
            public String UserName { get; set; }
            [BindProperty]
            public bool[] myRoles { get; set; }
        }

        [BindProperty(SupportsGet = true)]
        public List<OutputData> MyData { get; set; } = new List<OutputData>();
        /*            CheckCreateRole("Modeler", RoleManager);
            CheckCreateRole("HR", RoleManager);
            CheckCreateRole("Writer", RoleManager);
            CheckCreateRole("LeadWriter", RoleManager);
            CheckCreateRole("LeadModeler", RoleManager);
            CheckCreateRole("TaskMaster", RoleManager);*/

        [BindProperty]
        public List<String> PossibleRoles { get; set; }

        UserManager<UserData> myUsers;
        RoleManager<IdentityRole> myRoles;
        ApplicationDbContext myContext;
        public UserViewerModel(ApplicationDbContext myContext, UserManager<UserData> myUsers, RoleManager<IdentityRole> myRoles)
        { //
            this.myContext = myContext;
            //Todo make sure this is updated each time or only on get?
            //TODO: get possible roles
            this.myRoles = myRoles;
            this.myUsers = myUsers;
        }



        public void OnPost()
        {
            SetupRoles();
            List<string> Approved = new List<string>();
            List<string> Denied = new List<string>();
            for (int i = 0; i < MyData.Count(); i++)
            {
                Approved.Clear();
                Denied.Clear();
                Task<UserData> myUser = myUsers.FindByIdAsync(MyData[i].ID);
                myUser.Wait();
                for (int r=0; r < MyData[i].myRoles.Length; r++)
                {
                    if(MyData[i].myRoles[r]) //TODO: Opt
                    {
                        var result = myUsers.IsInRoleAsync(myUser.Result, PossibleRoles[r]);
                        result.Wait();
                        if (!result.Result)
                        {
                            Approved.Add(PossibleRoles[r]);
                        }
                    }
                    else
                    {
                        var result = myUsers.IsInRoleAsync(myUser.Result, PossibleRoles[r]);
                        result.Wait();
                        if (result.Result)
                        {
                            Denied.Add(PossibleRoles[r]);
                        }
                    }
                }


                var AddRole = AddRoleIfNotPresent(MyData[i].ID, Approved.ToArray());
                AddRole.Wait();//TODD: Fix these
                var RemoveRole = RemoveRoleNotPresent(MyData[i].ID, Denied.ToArray());
                RemoveRole.Wait();
                myUsers.UpdateAsync(myUser.Result).Wait();
                myContext.SaveChanges();
            }
        }

        public Task<IdentityResult> AddRoleIfNotPresent(String myUserID, String[] RoleName)
        {

            Task<UserData> myUser = myUsers.FindByIdAsync(myUserID);
            myUser.Wait();
            if (myUser.Result == null)
            {
                throw new Exception("User ID: " + myUserID + "not found!");
            }
            else
            {
                return myUsers.AddToRolesAsync(myUser.Result, RoleName);
            }
        }
        public Task<IdentityResult> RemoveRoleNotPresent(String myUserID, String[] RoleName)
        {
            Task<UserData> myUser = myUsers.FindByIdAsync(myUserID);
            myUser.Wait();
            if (myUser.Result == null)
            {
                throw new Exception("User ID: " + myUserID + "not found!");
            }
            else
            {
                return myUsers.RemoveFromRolesAsync(myUser.Result, RoleName);
            }
        }


        public void OnGet()
        {
            MyData.Clear();
            SetupRoles();
            var B = myUsers.Users.ToList();
            for (int i = 0; i < myUsers.Users.Count(); i++)
            {
                var A = B[i];
                var myIList = myUsers.GetRolesAsync(A);
                myIList.Wait();
                OutputData MyNewUser = new OutputData()
                {
                    ID = B[i].Id,
                    UserName = B[i].UserName,
                    myRoles = new bool[PossibleRoles.Count()]
                };
                for (int g = 0; g < PossibleRoles.Count(); g++)
                {
                    MyNewUser.myRoles[g] = (myIList.Result.Contains(PossibleRoles[g]));
                }
                MyData.Add(MyNewUser);
            }
        }

        private void SetupRoles()
        {
            if (PossibleRoles == null)
            {
                PossibleRoles = new List<string>();
            }else
            {
                PossibleRoles.Clear();
            }
            var Callthis = myRoles.Roles.ToList();
                for (int r = 0; r < myRoles.Roles.Count(); r++)
                {
                    PossibleRoles.Add(Callthis[r].Name);
                }
        }
    }
}