using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tugas_api.Models;
using tugas_api.ViewModels;

namespace tugas_api.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> Get();
        Department Get(string id);
        int Insert(DepartmentVM name);
        int Update(string id, DepartmentVM department);
        int Delete(string id);
    }
}