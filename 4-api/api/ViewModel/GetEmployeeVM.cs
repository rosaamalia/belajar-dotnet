using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.ViewModel
{
    public class GetEmployeeVM
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public int Salary { get; set; }
        public string Email { get; set; }
        public Gender Gender {get; set; }
        public string GPA { get; set; }
        public string University_Name {get; set; }
    }
}