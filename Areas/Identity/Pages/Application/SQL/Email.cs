using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5
{
    public class Email
    {
        [Required]
        [Key]
        public int id { get; set; }
        [Required]
        public String UserIDFrom { get; set; }
        [Required]
        public String UserIDTo { get; set; }
        [Required]
        public String Body { get; set; }
        [Required]
        public int EmailRepliedTo { get; set; } = 0;
        [Required]
        public bool ReadByRecipiant { get; set; }
        [Required]
        public DateTime TimeSent { get; set; }
    }
}
