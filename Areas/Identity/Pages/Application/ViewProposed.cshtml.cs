using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication5.Data;

namespace WebApplication5.Areas.Identity.Pages.Application
{
    public class ViewProposedModel : PageModel
    {
        private ApplicationDbContext myContext;
        private UserManager<UserData> myUsers;

        public ViewProposedModel(ApplicationDbContext myContext, UserManager<UserData> myUsers)
        {
            this.myContext = myContext;
            this.myUsers = myUsers;
        }

        [BindProperty]
        public List<SelectListItem> MyLoreList { get; set; }

        public void RebuildLoreList()
        {
            MyLoreList = new List<SelectListItem>();
            String myUserID = myUsers.GetUserId(User);
            var myLore = myContext.myLore.Where(delegate (Lore D)
            {
                return D.LoreIdeaStatus == Lore.LoreData.PROPOSED;
            });
            foreach (Lore A in myLore)
            {
                MyLoreList.Add(new SelectListItem()
                {
                    Text = A.LoreName,
                    Value = A.ID.ToString(),
                });
            }
        }

        public JsonResult OnGetChangeFile(string fileid)
        {
            RebuildLoreList();
            int myID;
            if (int.TryParse(fileid, out myID))
            {
                String myUserID = myUsers.GetUserId(User);
                var myReturn = myContext.myLore.Where(delegate (Lore D)
                {
                    return (D.ID == myID && D.LoreIdeaStatus == Lore.LoreData.PROPOSED);
                });
                if (myReturn.Count() > 0)
                {
                    var myElement = myReturn.ElementAt(0);
                    return new JsonResult(myElement);
                }
            }
            return new JsonResult(null);
        }
        public void OnGet()
        {
            RebuildLoreList();
        }
    }
}