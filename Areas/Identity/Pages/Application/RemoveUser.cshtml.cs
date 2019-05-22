using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication5.Areas.Identity.Pages.Application
{
    public class RemoveUserModel : PageModel
    {
        private UserManager<UserData> myUsers;

        public class UserToRemove
        {
            public bool[] RemoveThem;
            public UserData[] myUser;
        }

        [BindProperty]
        public UserToRemove myUserList { get; set; }

        public void OnPost()
        {
            for(int i = 0; i < myUserList.RemoveThem.Length; i++)
            {
                if(myUserList.RemoveThem[i])
                {
                    myUsers.DeleteAsync(myUserList.myUser[i]);
                }
            }
        }

        public RemoveUserModel(UserManager<UserData> Users)
        {
            this.myUsers = Users;
        }

        public void OnGet()
        {
            myUserList = new UserToRemove
            {
                RemoveThem = new bool[myUsers.Users.Count()],
                myUser = myUsers.Users.ToArray()
            };

        }
    }
}