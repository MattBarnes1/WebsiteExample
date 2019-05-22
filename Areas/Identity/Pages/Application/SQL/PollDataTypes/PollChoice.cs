using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5
{
    public class PollChoice
    {
        [Required]
        [Key]
        public int PollID { get; set; }
        [Required]
        public String mySelectedChoice { get; set; }
        public ICollection<UserData> ThoseThatPickedMe { get; set; }
    }
}
