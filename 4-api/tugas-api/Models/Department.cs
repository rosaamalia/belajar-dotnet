using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace tugas_api.Models
{
    public class Department
    {
        [Key]
        public string Dept_ID { get; set; }
        public string Name { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}