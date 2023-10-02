using api.Models;
using api.ViewModel;

namespace api.Interfaces {
    interface IEmployeeRepository {
        IEnumerable<GetEmployeeVM> Get();
        GetEmployeeVM Get(string NIK);
        int Insert(RegisterVM employee);
        int Update(Employee employee);
        int Delete(string NIK); 
    }
}