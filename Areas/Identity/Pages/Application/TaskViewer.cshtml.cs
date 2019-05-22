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
    public class TaskViewerModel : PageModel
    {       
        public ApplicationDbContext myApplicationDB;


        [BindProperty]
        public TaskData[] myScheduledTasks { get; set; }
        [BindProperty]
        public TaskData[] myActiveTasks { get; set; }
        [BindProperty]
        public TaskData[] TaskMasterAllUnclaimedTask { get; set; }
        [BindProperty]
        public TaskData[] TaskMasterAllClaimedTasks { get; set; }



        [BindProperty]
        public int UserSelectedTasks { get; set; }

        private SignInManager<UserData> signInManager;
        UserManager<UserData> MyUsers;

        public TaskViewerModel(ApplicationDbContext myApplicationDB, UserManager<UserData> MyUsers, SignInManager<UserData> signInManager)
        {
            this.signInManager = signInManager;
            this.MyUsers = MyUsers;
            this.myApplicationDB = myApplicationDB;
        }
       
        public void OnPost()
        {
            //TODO: update user tasks.
            //TODO: check active tasks to prevent user from grabbing too many.
        }

        public void OnPostFinishTask(string TaskToFinish)
        {
            int myTaskID;
            var AllTasksRelatedToUser = myApplicationDB.myTasks.Where(delegate (TaskData D)
            {
                return D.ClaimedByID == MyUsers.GetUserId(User);
            });
            if (int.TryParse(TaskToFinish, out myTaskID))
            {
               TaskData myRetVal = AllTasksRelatedToUser.First(delegate (TaskData D)
               {
                    return !D.MarkedCompleted && D.TaskID == myTaskID;
               });
                if(myRetVal == null)
                {
                    myRetVal.MarkedCompleted = true;
                    myApplicationDB.myTasks.Update(myRetVal);
                    myApplicationDB.SaveChanges();
                }
            }
            else
            { //TODO: Error Handling
                
            }
        }


        public void OnPostRemoveTask(String TaskToDelete)
        {
            int myTaskID;
            if(int.TryParse(TaskToDelete, out myTaskID))
            {
                var retVal = myApplicationDB.myTasks.Where(delegate (TaskData D)
                {
                    return D.TaskID == myTaskID;
                });
                if(retVal.Count() != 0)
                {
                    myApplicationDB.myTasks.Remove(retVal.ElementAt(0));
                    if(!String.IsNullOrEmpty(retVal.ElementAt(0).ClaimedByID))
                    {
                        Task<UserData> myUserData = MyUsers.FindByIdAsync(retVal.ElementAt(0).ClaimedByID);
                        myUserData.Wait();
                        myUserData.Result.ClaimedTasks--;
                        MyUsers.UpdateAsync(myUserData.Result).Wait();
                        OnResult = "Successfully Removed!";
                    }
                    myApplicationDB.SaveChanges();
                }
                else
                {
                    OnResult = "Task not found.";
                }
            }
            else
            {
                OnResult = "Failed to find proper ID.";
            }
        }

        public void OnPostClearOwner(String TaskToClear)
        {
            int myTaskID;
            if (int.TryParse(TaskToClear, out myTaskID))
            {
                var retVal = myApplicationDB.myTasks.Where(delegate (TaskData D)
                {
                    return D.TaskID == myTaskID;
                });
                if (retVal.Count() != 0)
                {
                    TaskData A = retVal.ElementAt(0);
                    if (!String.IsNullOrEmpty(A.ClaimedByID))
                    {
                        Task<UserData> myUserData = MyUsers.FindByIdAsync(retVal.ElementAt(0).ClaimedByID);
                        myUserData.Wait();
                        myUserData.Result.ClaimedTasks--;
                        MyUsers.UpdateAsync(myUserData.Result).Wait();
                        OnResult = "Successfully Removed!";
                    }
                    A.ClaimedByID =  null;
                    myApplicationDB.myTasks.Update(A);
                    myApplicationDB.SaveChanges();
                } else
                {
                    OnResult = "Task not found.";
                }
            }
            else
            {
                OnResult = "Failed to find proper ID.";
            }
        }



        public void OnPostClaimTask(String TaskToClaim)
        {
            int myTaskID;
            if (int.TryParse(TaskToClaim, out myTaskID))
            {
                var retVal = myApplicationDB.myTasks.Where(delegate (TaskData D)
                {
                    return D.TaskID == myTaskID;
                });
                if (retVal.Count() != 0)
                {
                    TaskData A = retVal.ElementAt(0);
                    var myTask = MyUsers.GetUserAsync(User);
                    myTask.Wait();
                    A.ClaimedByID = myTask.Result.Id;
                    myTask.Result.ClaimedTasks++;
                    MyUsers.UpdateAsync(myTask.Result).Wait();
                    myApplicationDB.myTasks.Update(A);
                    myApplicationDB.SaveChanges();
                }
                else
                {
                    OnResult = "Task not found.";
                }
            }
            else
            {
                OnResult = "Failed to find proper ID.";
            }
        }

       public String OnResult { get; set; }


        public void OnGet()
        {
            CreateTables();

        }

        private void CreateTables()
        {
            if (User.IsInRole("TaskMaster"))
            {
                TaskMasterAllUnclaimedTask = myApplicationDB.myTasks.Where((TaskData D) => (D.ClaimedByID == null)).ToArray();
                TaskMasterAllClaimedTasks = myApplicationDB.myTasks.Where((TaskData D) => (D.ClaimedByID != null && !D.MarkedCompleted)).ToArray();
            }
            var AllTasksRelatedToUser = myApplicationDB.myTasks.Where(delegate (TaskData D)
            {
                return D.ClaimedByID == MyUsers.GetUserId(User);
            });

            myActiveTasks = AllTasksRelatedToUser.Where(delegate (TaskData D)
            {
                return !D.MarkedCompleted;
            }).ToArray();
            myScheduledTasks = AllTasksRelatedToUser.Where(delegate (TaskData D)
            {
                return D.TaskEnd != null && !D.MarkedCompleted;
            }).ToArray();
        }
    }
}