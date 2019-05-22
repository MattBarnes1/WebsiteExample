using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5
{
    public enum RequestType
    {
        BUG_REPORT,
        FEATURE_REQUEST
    }

    public class Request
    {
        [Key]
        public int IDKey { get; set; }

        [Required]
        public String RequestersID { get; set; }

        [Required]
        public String RequestBody { get; set; }

        [Required]
        public RequestType myType { get; set; }

    }
}
