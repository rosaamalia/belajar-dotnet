using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Education
    {
        [Key]
        public int Id { get; set; }
        public Degree Degree { get; set; }
        public string GPA { get; set; }
        // bisa pakai [ForeignKey("University")]
        public int University_Id { get; set; } // foreign key dari University
        public ICollection<Profiling> Profilings { get; set; }
        public University University { get; set; }
    }

    public enum Degree {
        D3,
        D4,
        S1,
        S2,
        S3
    }
}