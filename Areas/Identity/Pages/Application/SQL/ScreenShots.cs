using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5
{
    public class ScreenShots
    {
        [Key]
        public int ID { get; set; }
        public string BlogFile { get; set; }
        public string CalculateBlogHash { get; set; }
        public string ApproverID { get; set; } = "Not Imp";
    }
}
