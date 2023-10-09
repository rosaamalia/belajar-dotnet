using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tugas_api.ViewModels
{
    public class DepartmentVM
    {
        public string Name { get; set; }
    }

    public class DepartmentWithIdVM
    { 
        public string Dept_ID { get; set; }
        public string Name { get; set; }
    }
    public class TotalEmployees
    {
        public string DepartmentName { get; set; }
        public int Total { get; set; }
    }
}