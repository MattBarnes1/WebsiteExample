using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5.Areas.Identity.Pages.Application.SQL
{
    public class BlogItem
    {
        [Key]
        public int BlogId { get; set; }
        public string BlogFile { get; set; }
        public string CalculateBlogHash { get; set; }
        public string Approver { get; set; } = "Not Imp";
    }
}
