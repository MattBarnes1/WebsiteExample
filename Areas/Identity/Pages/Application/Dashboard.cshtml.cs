using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication5.Data;

namespace WebApplication5.Areas.Identity.Pages.Application
{
    public class DashboardModel : PageModel
    {
        public DashboardModel(ApplicationDbContext myContext, UserManager<UserData> CurrentUser)
        {
            this.myContext = myContext;
            this.UserHandler = CurrentUser;
            myPossibleTasks = myContext.myTasks.Where<TaskData>(delegate (TaskData D)
            {
                return (!String.IsNullOrEmpty(D.ClaimedByID) && D.ClaimedByID.CompareTo(UserHandler.GetUserId(User)) == 0 && (D.myState == TaskData.TaskState.INCOMPLETE || D.myState == TaskData.TaskState.INCOMPLETE));
            });

        }

        public ApplicationDbContext myContext { get; }
        public UserManager<UserData> UserHandler { get; }

        IEnumerable<TaskData> myPossibleTasks;

        IEnumerable<EventData> myPossibleEvents;

        public IEnumerable<EventData> GetEvents(DateTime myEvents)
        {
            return myContext.myGlobalEvents.Where(delegate (EventData A)
            {
                return A.StartDate.Date == myEvents.Date;
            });
        }
        public IEnumerable<TaskData> GetTasks(DateTime myEvents)
        {
            return myPossibleTasks.Where(delegate (TaskData A)
            {
                return A.TaskEnd.Date == myEvents.Date;
            });
        }


        //TODO: Don't allow access to add/remove item
        public void OnGet()
        {
        }
    }
}