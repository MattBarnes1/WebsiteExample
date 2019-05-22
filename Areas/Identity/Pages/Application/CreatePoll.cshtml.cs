using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication5.Data;

namespace WebApplication5.Areas.Identity.Pages.Application
{
    [BindProperties]
    public class CreatePollModel : PageModel
    {
        private ApplicationDbContext myContext;

        [Display(Name="Poll Name")]
        [DataType(DataType.Text)]
        public String Name { get; set; }
        [Display(Name = "Poll Question")]
        [DataType(DataType.MultilineText)]
        public string PollQuestion { get; set; }

        public List<String> Choices { get; set; }
        public string Error { get; private set; }

        public CreatePollModel(ApplicationDbContext myContext)
        {
            this.myContext = myContext;
        }

        public void OnPostRemoveChoice(String Value)
        {
            Choices.Remove(Value);
        }

        public void OnPostAddChoice(String AddID, String NewItem)
        {
            Choices.Add("");
        }
        
        public void OnPostFinishedQuestion()
        {
            if(Choices == null || Choices.Count == 0)
            {
                Error = "Error: No questions!";
            }
            else
            {
                Poll myNewPoll = new Poll();
                List<PollChoice> myPollChoice = new List<PollChoice>();
                foreach(String A in Choices)
                {
                    PollChoice myChoice = new PollChoice();
                    myChoice.mySelectedChoice = A;
                    myPollChoice.Add(myChoice);
                }
                myNewPoll.PollQuestion = PollQuestion;
                myNewPoll.Name = Name;
                myNewPoll.PollCreationDate = DateTime.Now;
                myNewPoll.isFinished = false;
                myNewPoll.Choices = myPollChoice.ToArray();
                myContext.myPolls.Add(myNewPoll);
                myContext.SaveChanges();
                myPollChoice.Clear();
                PollQuestion = "";
                Name = "";
            }
        }


        public void OnGet()
        {

        }
    }
}