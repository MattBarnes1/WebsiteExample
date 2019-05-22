using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5
{
    public class Lore
    {
        [Key]
        public int ID { get; set; }
        public enum LoreData
        {
            DRAFT,
            PROPOSED,
            ENACTED,
            REJECTED,
            PERMANENT_REJECTION
        }
        [Required]
        public LoreData LoreIdeaStatus { get; set; } = LoreData.DRAFT;
        public String LoreBody { get; set; } = "";
        [Required]
        public String LoreName { get; set; } //Make sure this is unique from others.
        [Required]
        public String ProposerUserId { get; set; }
        public Lore[] myOpposition { get; set; }
    }
}
