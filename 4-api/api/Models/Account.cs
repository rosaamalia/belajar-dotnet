using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using api.Models;

namespace api.Models
{
    public class Account
    {
        [Key]
        public string NIK { get; set; } // foreign key dari model Employee, didefiniiskan di dalam Context
        public string Password { get; set; }
        public string? OTP { get; set; }
        public Employee Employee { get; set; }
        public Profiling Profiling { get; set; }
    }
}