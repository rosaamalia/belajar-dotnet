using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class University
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Education> Educations { get; set; }
    }
}