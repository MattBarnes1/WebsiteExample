using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication5.Data;

namespace WebApplication5.Areas.Identity.Pages.Application
{
    public class EmailSystemModel : PageModel
    {
        public class NewEmail
        {
            public int ID { get; set; }
            public String To { get; set; }
            public String From { get; set; }
            public String Body { get; set; }
            public String ReplyBody { get; set; }
        }
        //https://www.jerriepelser.com/blog/razor-pages-muliple-handlers/
        [BindProperty]
        public NewEmail myNewEmail { get; set; }

        [BindProperty]
        public String ReplyID { get; set; }

        public Email[] MailToMe { get; set; }

        private UserManager<UserData> myUsers;
        ApplicationDbContext myContext;

        public Dictionary<DateTime, List<Email>> Emails = new Dictionary<DateTime, List<Email>>();

        public EmailSystemModel(ApplicationDbContext myContext, UserManager<UserData> myUsers)
        {
            this.myUsers = myUsers;
            this.myContext = myContext;
        }

        public void OnPost()
        {
             int i = 0;
        }

        public void OnPostStartReply(String id)
        {
            ReplyID = id;
            var myOldEmail = GetEmailByID(id);
            if (myOldEmail != null)
            {
                myNewEmail.From = myOldEmail.UserIDFrom;
                myNewEmail.To = myUsers.GetUserName(User);
                myNewEmail.ReplyBody = myOldEmail.Body;
                myNewEmail.Body = "";
                myOldEmail.ReadByRecipiant = true;
                myContext.Update(myOldEmail);
                myContext.SaveChanges();
            }
        }

        public Email FindEmailFrom(out UserData Recipient, String ID, out UserData Writer)
        {
            if (VerifyUsersInfo(out Recipient, out Writer))
            {
                return GetEmailByID(ID);
            }
            return null;
        }

        public JsonResult OnGetHasUnreadEmails()
        {
            UpdateEmails();
            try
            {
                var retVals = MailToMe.First(delegate (Email D)
                {
                    return D.ReadByRecipiant == false;
                });
                return new JsonResult(retVals != null);
            }
            catch(Exception e)
            {
                return new JsonResult(false);
            }
        }

        private Email GetEmailByID(string ID)
        {
            UpdateEmails();
            int id;
            if (int.TryParse(ID, out id))
            {
                var myRepliedToEmail = MailToMe.Where(delegate (Email A)
                {
                    return A.id == id;
                });

                if (myRepliedToEmail.Count() > 0)
                {
                    return myRepliedToEmail.ElementAt(0);
                }
            }
            return null;
        }

        public bool VerifyUsersInfo(out UserData Recipient, out UserData Writer)
        {
            UpdateEmails();
            var taskRecipient = myUsers.FindByNameAsync(myNewEmail.To);
            taskRecipient.Wait();
            Recipient = taskRecipient.Result;
            var taskWriter = myUsers.GetUserAsync(User);
            taskWriter.Wait();
            Writer = taskWriter.Result;
            return (taskWriter.Result != null && taskRecipient.Result != null);
        }

        public void OnPostReply(String id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                ReplyID = "";
                var myEmail = GetEmailByID(id);
                if (myEmail == null)
                {
                    Redirect("/Error");
                    return;
                }
                if (myEmail != null)
                {
                    Email email = new Email();
                    email.Body = myNewEmail.Body;
                    email.TimeSent = DateTime.Now;
                    email.UserIDTo = myEmail.UserIDFrom;
                    email.UserIDFrom = myEmail.UserIDTo;
                    email.EmailRepliedTo = myEmail.id;
                    myContext.myEmails.Add(email);
                    myNewEmail.Body = "";
                    myNewEmail.From = "";
                    myNewEmail.ReplyBody = "";
                    myNewEmail.To = "";
                    myContext.SaveChanges();
                }
            }
            else
            {
                //Check if user exists:
                UserData Recipient;
                UserData Writer;
                if (VerifyUsersInfo(out Recipient, out Writer))
                {
                    Email email = new Email();
                    email.Body = myNewEmail.Body;
                    email.TimeSent = DateTime.Now;
                    email.UserIDTo = Recipient.Id;
                    email.UserIDFrom = Writer.Id;
                    myContext.myEmails.Add(email);
                    myContext.SaveChanges();
                    Error = "Success!";
                    myNewEmail = new NewEmail();
                }
                else
                {
                    Error = "Failed to send!";
                }
            }
        }

        [BindProperty]
        public String ResponseFromServer { get; set; }

        public class JSONData
        {
            public String Date;
            public String TopID = "A";
            public String BottomID = "B";
            public List<Email> myEmails = new List<Email>();
            public List<String> Usernames = new List<string>();
        }

        public JsonResult OnGetAjaxEmailListAsync()
        {
            UpdateEmails();
            List<JSONData> myData = new List<JSONData>();
            if(MailToMe.Count() > 0)
            {
                DateTime Last = MailToMe.ElementAt(0).TimeSent.Date;
                JSONData first = new JSONData();
                first.Date = Last.Date.ToShortDateString();
                foreach (Email A in MailToMe)
                {
                    if (A.TimeSent.Date == Last)
                    {
                        first.myEmails.Add(A);
                        AddUserToJSONList(first, A);
                    }
                    else
                    {
                        myData.Add(first);
                        String NEXTTOP = first.TopID + first.TopID;
                        String NEXTBOT = first.BottomID + first.BottomID;
                        first = new JSONData();
                        first.TopID = NEXTTOP;
                        first.BottomID = NEXTBOT;
                        Last = A.TimeSent.Date;
                        first.Date = Last.Date.ToShortDateString();
                        first.myEmails.Add(A);
                        AddUserToJSONList(first, A);
                    }
                }
                myData.Add(first); //Adds the last JSONData object we were dealing with;
            }

            return new JsonResult(myData);
        }

        private void AddUserToJSONList(JSONData first, Email A)
        {
            var myTask = myUsers.FindByIdAsync(A.UserIDFrom);
            myTask.Wait();
            String myUsername = "Error!";
            if (myTask.IsCompletedSuccessfully)
            {
                myUsername = myTask.Result.UserName;
            }
            first.Usernames.Add(myUsername);
        }


        public void OnPostDeleteAsync(String id)
        {
            string UserId = myUsers.GetUserId(User);
            int TryGetInt;
            if (!int.TryParse(id, out TryGetInt)) return;
            var Q = myContext.myEmails.Where(delegate (Email D)
            {
                return D.UserIDTo == UserId && D.id == TryGetInt;
            }).ToArray();
            if (Q.Count() > 0)
            {
                var EMAIL = Q.ElementAt(0);
                int g = myContext.myEmails.Count();
                myContext.myEmails.Remove(EMAIL);
                g = myContext.myEmails.Count();
                myContext.SaveChanges();
                g = myContext.myEmails.Count();
            }
        }


        public void OnPostSend()
        {
            //Check if user exists:
            UserData Recipient;
            UserData Writer;
            if(VerifyUsersInfo(out Recipient, out Writer))
            {
                Email email = new Email();
                email.Body = myNewEmail.Body;
                email.TimeSent = DateTime.Now;
                email.UserIDTo = Recipient.Id;
                email.UserIDFrom = Writer.Id;
                myContext.myEmails.Add(email);
                myContext.SaveChanges();
                Error = "Success!";
                myNewEmail = new NewEmail();
            }
            else
            {
                Error = "Failed to send!";
            }
        }

        [TempData]
        public String Error { get; set; }
        public void OnGet()
        {
            string ID = (String)RouteData.Values["emailid"];
            int TryGetInt;
            UpdateEmails();

            if (int.TryParse(ID, out TryGetInt))
            {
                if (!String.IsNullOrEmpty(ID))
                {
                    var EmailThing = MailToMe.Where(delegate (Email A)
                    {
                        return A.id == TryGetInt;
                    });
                    if (EmailThing.Count() > 0)
                    {
                        var myEmailToDisplay = EmailThing.ElementAt(0);
                        myNewEmail = new NewEmail();
                        myNewEmail.Body = myEmailToDisplay.Body;
                        myNewEmail.ID = TryGetInt;
                        myNewEmail.To = myEmailToDisplay.UserIDTo;
                        var myTask = myUsers.FindByIdAsync(myEmailToDisplay.UserIDFrom);
                        myTask.Wait();
                        myNewEmail.To = myTask.Result.UserName;
                    }
                }
            }

            List<Email> CurrentList = new List<Email>();
            foreach (Email A in MailToMe)
            {
                List<Email> anEmail;
                if (Emails.TryGetValue(A.TimeSent.Date, out anEmail))
                {
                    anEmail.Add(A);
                }
                else
                {
                    anEmail = new List<Email>();
                    anEmail.Add(A);
                    if (Emails.TryAdd(A.TimeSent.Date, anEmail))
                    {
                        LocalRedirect("/Error");
                    }
                }
            }
        }

        public JsonResult OnGetMatchUserInfo(string UserString)
        {
            UpdateEmails();
            List<String> myPossibleMatches = new List<String>();

            foreach(UserData A in myUsers.Users)
            {
                if (A.UserName.ToLower().CompareTo(UserString.ToLower()) < 1)
                {
                    myPossibleMatches.Add(A.UserName);
                }
            }
            myPossibleMatches.Sort();
            return new JsonResult(myPossibleMatches);
        }

        private void UpdateEmails()
        {
            string UserId = myUsers.GetUserId(User);
            MailToMe = myContext.myEmails.Where(delegate (Email D)
            {
                return D.UserIDTo == UserId;
            }).OrderBy(delegate (Email A)
            {
                return A.TimeSent.Date;
            }).ToArray();
        }
    }
}