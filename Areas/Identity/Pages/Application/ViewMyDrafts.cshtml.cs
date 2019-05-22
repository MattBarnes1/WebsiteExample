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

namespace WebApplication5
{
    public class ViewMyDraftsModel : PageModel
    {
        private ApplicationDbContext myContext;
        private UserManager<UserData> myUsers;

        [BindProperty]
        public List<SelectListItem> MyLoreList { get; set; }

        public ViewMyDraftsModel(ApplicationDbContext myContext, UserManager<UserData> myUsers)
        {
            this.myContext = myContext;
            this.myUsers = myUsers;
        }

        public void OnGet()
        {
            RebuildLoreList();

        }
        
        public void OnPostNewItem(string MyNewItemName)
        {
           var Results = myContext.myLore.Where(delegate (Lore D)
            {
                return (D.LoreName.CompareTo(MyNewItemName) == 0);
            });
            if(Results.Count() == 0)
            {
                Lore myLore = new Lore();
                String myUserID = myUsers.GetUserId(User);
                myLore.LoreName = MyNewItemName;
                myLore.ProposerUserId = myUserID;
                myContext.myLore.Add(myLore);
                myContext.SaveChanges();
            }
            else
            {
            }
        }

        public void OnPostSubmitToCanon(string DisplayArea, string PostID)
        {
            if (DisplayArea == null) return;
            int myID;
            String myUserID = myUsers.GetUserId(User);
            if (int.TryParse(PostID, out myID))
            {
                var myReturn = myContext.myLore.First(delegate (Lore D)
                {
                    return (D.ID == myID && D.ProposerUserId == myUserID && (D.LoreIdeaStatus == Lore.LoreData.DRAFT || D.LoreIdeaStatus == Lore.LoreData.REJECTED));
                });
                myReturn.LoreIdeaStatus = Lore.LoreData.PROPOSED;
                myContext.myLore.Update(myReturn);
                myContext.SaveChanges();
            }
            RebuildLoreList();
        }

        public void OnGetUpdateFile(string fileid, string fileText)
        {
            int myID;
            if (int.TryParse(fileid, out myID))
            {
                String myUserID = myUsers.GetUserId(User);
                var myReturn = myContext.myLore.Where(delegate (Lore D)
                {
                    return (D.ID == myID && D.ProposerUserId == myUserID && D.LoreIdeaStatus == Lore.LoreData.DRAFT);
                });
                myReturn.ElementAt(0).LoreBody = fileText;
                myContext.myLore.Update(myReturn.ElementAt(0));
                myContext.SaveChanges();
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
                    return (D.ID == myID && D.ProposerUserId == myUserID && (D.LoreIdeaStatus == Lore.LoreData.DRAFT || D.LoreIdeaStatus == Lore.LoreData.REJECTED));
                });
                if(myReturn.Count() > 0)
                {
                  return new JsonResult(myReturn.ElementAt(0));
                }
            }
            return new JsonResult(null);
        }

        public JsonResult HasRejectedLore()
        {
            String myUserID = myUsers.GetUserId(User);
            var myLore = myContext.myLore.Where(delegate (Lore D)
            {
                return D.ProposerUserId == myUserID && (D.LoreIdeaStatus == Lore.LoreData.DRAFT || D.LoreIdeaStatus == Lore.LoreData.REJECTED);
            });
            return new JsonResult(!(myLore.Count() == 0));
        }

        
        private void RebuildLoreList()
        {
            MyLoreList = new List<SelectListItem>();
            String myUserID = myUsers.GetUserId(User);
            var myLore = myContext.myLore.Where(delegate (Lore D)
            {
                return D.ProposerUserId == myUserID && (D.LoreIdeaStatus == Lore.LoreData.DRAFT ||D.LoreIdeaStatus == Lore.LoreData.REJECTED);
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
    }
}