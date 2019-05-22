using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5
{
    public class Poll
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public string PollQuestion { get; set; }
        [Required]
        public ICollection<PollChoice> Choices { get; set; }
        [Required]
        public bool isFinished { get; set; } = false;
        [Required]
        public DateTime PollCreationDate { get; set; }
    }
}
