using tugas_api.Context;
using tugas_api.Repositories.Interfaces;
using tugas_api.Models;
using tugas_api.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace tugas_api.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MyContext context;

        public EmployeeRepository(MyContext context) {
            this.context = context;
        }

        /*
         * Get semua data Employee
         */
        public IEnumerable<GetEmployeeAndDepartmentVM> Get() {
            // var data = getEmployee();
            return getEmployee().ToList();
        }

        /*
         * Get data Employee berdasarkan NIK
         */
        public Employee Get(string NIK) {
            return context.Employees.Find(NIK);
        }

        /*
         * Insert data employee
         */
        public int Insert(EmployeeVM employee) {
            // generate NIK baru DDMMYY000
            string date = DateTime.Now.ToString("ddMMyy");
            string newNIK = "";

            // cek data terakhir di database
            var lastData = context.Employees.OrderBy(data => data.NIK).LastOrDefault();
            if(lastData == null) {
                // kalau ternyata gak ada data di database, otomatis urutan 001
                newNIK = date + "001";
            } else {
                // ada data terakhir, ambil 3 karakter string dari NIK (nomor urut)
                var nikLastData = lastData.NIK;
                string lastThree = nikLastData.Substring(nikLastData.Length-3);
                
                // convert jadi int terus tambah satu
                int nextSequence = int.Parse(lastThree) + 1;
                newNIK = date + nextSequence.ToString("000"); // convert jadi string
            }

            // generate email FirstNameLastName@berca.co.id (tambahin angka di belakangnya kalau ada nama yang sama)
            string domain = "@berca.co.id";
            string fullName = employee.FirstName + employee.LastName;
            string newEmail = "";

            // cek apakah email sudah digunakan
            var emailData = context.Employees.Where(e => e.Email.Contains(fullName)).OrderBy(data => data.NIK).LastOrDefault();;
            if(emailData == null) {
                newEmail = fullName + domain;
            } else {
                // pisahkan nama email dengan domainnya
                string[] emailSplit = Regex.Split(emailData.Email,"@");
                string emailName = (string)emailSplit[0];

                // ambil 3 karakter terakhir
                string lastThree = emailName.Substring(emailName.Length-3);
                Console.WriteLine(lastThree);
                if (int.TryParse(lastThree, out int number)) {
                    // jika 3 karakter terakhir adalah angka
                    int nextSequence = number + 1;
                    newEmail = fullName + nextSequence.ToString("000") + domain;
                } else {
                    newEmail = fullName + "001" + domain;
                }
            }

            Employee employeeData = new Employee {
                NIK = newNIK,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = newEmail.ToLower(),
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                IsActive = employee.IsActive,
                Department_id = employee.Department_id
            };
            context.Employees.Add(employeeData);
            var saveEmployee = context.SaveChanges();

            return saveEmployee;
        }

        public int Update(string NIK, EmployeeVM employee) {
            var data = context.Employees.Find(NIK);
            data.FirstName = employee.FirstName;
            data.LastName = employee.LastName;
            data.PhoneNumber = employee.PhoneNumber;
            data.Address = employee.Address;
            data.IsActive = employee.IsActive;
            data.Department_id = employee.Department_id;
            
            var result = context.SaveChanges();
            return result;
        }

        public int Delete(string NIK) {
            var data = context.Employees.Find(NIK);
            data.IsActive = false;
            var result = context.SaveChanges();
            return result;
        }

        // Fungsi get
        public IEnumerable<GetEmployeeAndDepartmentVM> GetActiveEmployees(){
            var employees = getEmployee().Where(e => e.IsActive == true).ToList();
            return (IEnumerable<GetEmployeeAndDepartmentVM>)employees;
        }

        public IEnumerable<GetEmployeeAndDepartmentVM> GetInactiveEmployees(){
            var employees = getEmployee().Where(e => e.IsActive == false).ToList();
            return (IEnumerable<GetEmployeeAndDepartmentVM>)employees;
        }

        public IEnumerable<GetEmployeeAndDepartmentVM> GetActiveEmployeesByDepartment(string department_id){
            var employees = getEmployee().Where(e => e.IsActive == true && e.Department.Dept_ID == department_id).ToList();
            return (IEnumerable<GetEmployeeAndDepartmentVM>)employees;
        }

        public IEnumerable<GetEmployeeAndDepartmentVM> GetInactiveEmployeesByDepartment(string department_id){
            var employees = getEmployee().Where(e => e.IsActive == false && e.Department.Dept_ID == department_id).ToList();
            return (IEnumerable<GetEmployeeAndDepartmentVM>)employees;
        }

        // public IEnumerable<TotalEmployees> GetTotalActiveEmployeesByDepartment(string department_id){
        //     var total = getEmployee()
        //                 .Where(e => e.IsActive == true)
        //                 .GroupBy(e => e.Department.Dept_ID == department_id)
        //                 .Select(e => new TotalEmployees{DepartmentName = e.Key, Total = e.Count()})
        //                 .ToList();
        //     return (IEnumerable<TotalEmployees>)total;
        // }

        // utility
        public IQueryable<GetEmployeeAndDepartmentVM> getEmployee() {
            var data = context.Employees
                .Join(context.Departments,
                        e => e.Department_id,
                        a => a.Dept_ID,
                        (employees, departments) =>  new GetEmployeeAndDepartmentVM{
                            NIK = employees.NIK,
                            FirstName = employees.FirstName,
                            LastName = employees.LastName,
                            Email = employees.Email,
                            PhoneNumber = employees.PhoneNumber,
                            Address = employees.Address,
                            IsActive = employees.IsActive,
                            Department = new DepartmentWithIdVM{
                                Dept_ID = departments.Dept_ID,
                                Name = departments.Name
                            }
                    });
            return data;
        }

        
        public bool CheckPhoneUnique(string phone) {
            var data = context.Employees.AsNoTracking().FirstOrDefault(employee => employee.PhoneNumber == phone);
            if(data == null){
                return false;
            }
            return true;
        }

        public bool CheckNIKExist(string NIK) {
            var data = context.Employees.AsNoTracking().FirstOrDefault(employee => employee.NIK == NIK);
            if(data == null){
                return false;
            }
            return true;
        }

        public bool CheckDepartmentExist(string id) {
            var data = context.Departments.Find(id);
            if(data == null) {
                return false;
            }
            return true;
        }
    }
}