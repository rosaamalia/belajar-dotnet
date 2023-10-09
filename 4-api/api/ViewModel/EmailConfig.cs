using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.ViewModel
{
    public class EmailConfig
    {
        public string Nama {get; set;}
        public string Host { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string AppPassword { get; set; }
    }
}