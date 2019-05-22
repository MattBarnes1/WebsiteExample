using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication5.Data;

namespace WebApplication5
{
    public class DisplayPollListModel : PageModel
    {
        private ILogger<DisplayPollListModel> myLogger;
        ApplicationDbContext myContext;
        UserManager<UserData> myManager;
        public DisplayPollListModel(UserManager<UserData> myManager, ApplicationDbContext myContext, ILogger<DisplayPollListModel> myLogger)
        {
            this.myLogger = myLogger;
            this.myManager = myManager;
            this.myContext = myContext;
        }

        public class PollModel
        {
            public Poll FakePoll;
            public PollModel( Poll aPoll)
            {
                FakePoll = aPoll;
                Body = aPoll.PollQuestion;
                Choices = aPoll.Choices.ToArray();//TODO: Check this
            }

            public Poll GetPoll()
            {
                return FakePoll;
            }


            public PollModel()
            {
                Body = "";
            }

            public String Body { get; private set; }
            public PollChoice[] Choices { get; private set; }
        }
        
        public void OnPostPostVote(int Vote, int PollID)
        {
            var getPolls = myContext.myPolls.Where(delegate (Poll aPoll)
            {
                return aPoll.ID == PollID;
            });
            if (getPolls.Count() == 0) return;
            var myPoll = getPolls.ElementAt(0);
            if (!myPoll.isFinished)
            {
                var Loader = myContext.Entry<Poll>(myPoll).Collection<PollChoice>("Choices");
                Loader.Load();
                myPoll.Choices = Loader.CurrentValue.ToArray();
                var ret = myManager.GetUserAsync(User);
                ret.Wait();
                if (CheckForDoubleVoting(myPoll, ret.Result)) return;
                var myCHoice = myPoll.Choices.ElementAt(Vote);
                if (myCHoice.ThoseThatPickedMe != null)
                {
                    UserData[] myNewData = new UserData[myCHoice.ThoseThatPickedMe.Count() + 1];
                    Array.Copy(myCHoice.ThoseThatPickedMe.ToArray(), myNewData, myCHoice.ThoseThatPickedMe.Count());
                    myNewData[myCHoice.ThoseThatPickedMe.Count()] = ret.Result;
                    myCHoice.ThoseThatPickedMe = myNewData;
                }
                else
                {
                    myCHoice.ThoseThatPickedMe = new List<UserData>();
                    myCHoice.ThoseThatPickedMe.Add(ret.Result);
                }
                CheckSumOfVotes(myPoll);
                myContext.Update(myCHoice);                
                myContext.Update(myPoll);
                myContext.SaveChanges();
            }
            ReloadVoting();
        }

        private bool CheckForDoubleVoting(Poll aPoll, UserData Result)
        {
            var ChoiceArray = aPoll.Choices.ToArray();
            for(int i = 0; i < ChoiceArray.Length; i++)
            {
                if(ChoiceArray[i].ThoseThatPickedMe != null)
                {
                    if(ChoiceArray[i].ThoseThatPickedMe.Contains(Result))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void CheckSumOfVotes(Poll aPoll)
        {
            int UserCount = 0;
            for(int i= 0; i < aPoll.Choices.Count; i++)
            {
                if(aPoll.Choices.ElementAt(i).ThoseThatPickedMe != null)
                {
                    UserCount += aPoll.Choices.ElementAt(i).ThoseThatPickedMe.Count();
                }
            }
            var Count = myManager.Users.Where(delegate (UserData D)
            {
                return D.Joined <= aPoll.PollCreationDate;
            }).Count();

            aPoll.isFinished = (UserCount == Count);
        }
        

        public JsonResult OnGetAbstainedPolls()
        {
            Task<UserData> myData = myManager.GetUserAsync(User);
            myData.Wait();
            if (!myData.IsCompletedSuccessfully)
            {
                myLogger.LogCritical("INVALID USER REQUESTED STUFF FROM POLL");
                //TODO: sign out
                Redirect("/Error");
            }
            var myPolls = myContext.myPolls.Where(delegate (Poll D)
            {
                if(!D.isFinished)
                {
                    foreach (PollChoice A in D.Choices)
                    {
                        if (!A.ThoseThatPickedMe.Contains(myData.Result))
                        {
                            return true;
                        }
                    }
                }
                return false;
            });
            return new JsonResult(myPolls.Count() > 0);
        }

        public class returnStruct
        {
            Poll myPoll { get; set; }

        };

        public JsonResult OnGetSelectedPoll(string id)
        {
            int i;
            if (int.TryParse(id, out i))
            {
                var SelectPossibleToVoteIn = myContext.myPolls.Include("Choices").First(delegate (Poll aPoll)
                {
                    return aPoll.ID == i;
                });
                foreach(PollChoice A in SelectPossibleToVoteIn.Choices)
                {
                    var Loader = myContext.Entry<PollChoice>(A).Collection<UserData>("ThoseThatPickedMe");
                    Loader.Load();
                    A.ThoseThatPickedMe = Loader.CurrentValue.ToArray();

                }
                return new JsonResult(SelectPossibleToVoteIn);
            }
            return new JsonResult(null);
        }

        [BindProperty]
        public List<SelectListItem> SelectPossibleToVoteInList { get; set; }

        [BindProperty]
        public List<SelectListItem> ArchivedVotesList { get; set; }

        public void OnGet()
        {
            ReloadVoting();
        }

        private void ReloadVoting()
        {
            SelectPossibleToVoteInList = new List<SelectListItem>();
            ArchivedVotesList = new List<SelectListItem>();
            var SelectPossibleToVoteIn = myContext.myPolls.Where(delegate (Poll aPoll)
            {
                return !aPoll.isFinished;
            });
            var ret = myManager.GetUserAsync(User);
            ret.Wait();
            if(ret.IsCompletedSuccessfully)
            {
                var ArchivedVotes = myContext.myPolls.Include("Choices").Where(delegate (Poll aPoll)
                {
                    return aPoll.isFinished;
                }).ToList();
                foreach (Poll A in SelectPossibleToVoteIn)
                {
                    //var Loader = myContext.Entry<Poll>(A).Collection<PollChoice>("Choices");
                   // Loader.Load();
                   // A.Choices = Loader.CurrentValue.ToArray();
                    if (!CheckForDoubleVoting(A, ret.Result))
                    {
                        SelectListItem B = new SelectListItem();
                        B.Text = A.Name;
                        B.Value = A.ID.ToString();
                        SelectPossibleToVoteInList.Add(B);
                    }
                    else
                    {
                        SelectListItem B = new SelectListItem();
                        B.Text = A.Name;
                        B.Value = A.ID.ToString();
                        ArchivedVotesList.Add(B);
                    }
                }
            }
        }
    }
}