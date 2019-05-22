using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication5.Data;

namespace WebApplication5.Areas.Identity.Pages.Application
{
    [Authorize(Roles ="TaskMaster")]
    public class AddTaskModel : PageModel
    {
        private ApplicationDbContext myContext;

        [BindProperty]
        [Display(Name = "Task Short Description:")]
        [DataType(DataType.Text)]
        public String DisplayedDescription { get; set; }

        [BindProperty]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Task Description:")]
        public String TaskDescription { get; set; }

        [TempData]
        [DataType(DataType.Text)]
        public String OnTaskAdded { get; set; }

        public AddTaskModel(ApplicationDbContext myContext)
        {
            this.myContext = myContext;
        }

        public void OnPostNewTask()
        {
            TaskData myNewData = new TaskData();
            myNewData.TaskStartDate = DateTime.Now;
            if (String.IsNullOrEmpty(TaskDescription)|| String.IsNullOrEmpty(DisplayedDescription))
            {
                OnTaskAdded = "Error: Empty Description!";
                return;
            }
            myNewData.TaskDescription = TaskDescription;
            myNewData.ShortTaskDescription = DisplayedDescription;
            myContext.myTasks.Add(myNewData);

            myContext.SaveChanges();
            DisplayedDescription = "";
            TaskDescription = "";
            OnTaskAdded = "Success!";
        }


        public void OnGet()
        {
        }
    }
}