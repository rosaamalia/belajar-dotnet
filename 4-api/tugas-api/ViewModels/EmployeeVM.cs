using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tugas_api.ViewModels
{
    public class EmployeeVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } // sebagai tanda karyawan masih aktif atau tidak
        public string Department_id { get; set; } // sebagai foreign key untuk tabel department

    }

    public class GetEmployeeAndDepartmentVM
    {
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public DepartmentWithIdVM Department { get; set; }
    }
}