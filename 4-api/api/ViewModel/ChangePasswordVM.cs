using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.ViewModel
{
    public class ChangePasswordVM
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        public string Password { get; set; }
        public string CheckPassword { get; set; } // simpan konfirmasi password
    }
}