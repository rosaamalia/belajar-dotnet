using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Profiling
    {
        [Key]
        public string NIK { get; set; } // foreign key dari Account
        public int Education_id { get; set; } // foreign key dari University
        public Account Account { get; set; }
        public Education Education { get; set; }
    }
}