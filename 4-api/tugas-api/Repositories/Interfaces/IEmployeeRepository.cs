using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tugas_api.Models;
using tugas_api.ViewModels;

namespace tugas_api.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<GetEmployeeAndDepartmentVM> Get();
        Employee Get(string NIK);
        int Insert(EmployeeVM employee);
        int Update(string NIK, EmployeeVM employee);
        int Delete(string NIK);
        IEnumerable<GetEmployeeAndDepartmentVM> GetActiveEmployees();
        IEnumerable<GetEmployeeAndDepartmentVM> GetInactiveEmployees();
        IEnumerable<GetEmployeeAndDepartmentVM> GetActiveEmployeesByDepartment(string department_id);
        IEnumerable<GetEmployeeAndDepartmentVM> GetInactiveEmployeesByDepartment(string department_id);
        // IEnumerable<TotalEmployees> GetTotalActiveEmployeesByDepartment(string department_id);
    }
}