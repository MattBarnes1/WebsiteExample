using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Areas.Identity.Pages.Application
{
    public class TaskData
    {
        public enum TaskState
        {
            INCOMPLETE,
            COMPLETED,
            AWAITING_COMPLETION_APPROVAL
        }

        [Key]
        public int TaskID { get; set; }
        [Required]
        public String TaskDescription { get; set; }
        public String ClaimedByID { get; set; } = null;
        [Required]
        public DateTime TaskStartDate { get; set; }
        [Required]
        public TaskState myState { get; set; } = TaskState.INCOMPLETE;
        public bool MarkedCompleted { get; set; }
        public DateTime TaskEnd { get; set; }
        [Required]
        public String ShortTaskDescription { get; set; }

    }
}
