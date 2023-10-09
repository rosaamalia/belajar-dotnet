using api.Context;
using api.Interfaces;
using api.Models;
using api.ViewModel;
using Microsoft.EntityFrameworkCore;

// file repositori digunakan untuk menyimpan logic kepada database

namespace api.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        // constructor
        private readonly MyContext context;

        public EmployeeRepository(MyContext context) {
            this.context = context;
        }
        // interface harus mengimplementasikan semua method yang ada
        public int Delete(string NIK)
        {
            var data = context.Employees.Find(NIK);
            context.Remove(data);
            var result = context.SaveChanges();
            return result;

        }

        public IEnumerable<GetEmployeeVM> Get()
        {
            var data = context.Employees
                .Join(context.Accounts,
                        e => e.NIK,
                        a => a.NIK,
                        (employees, accounts) => new {employees, accounts}
                )
                .Join(context.Profilings,
                        e => e.accounts.NIK,
                        p => p.NIK,
                        (combined, profilings) => new {combined, profilings}
                )
                .Join(context.Educations,
                        c => c.profilings.Education_id,
                        e => e.Id,
                        (combined, educations) => new {combined, educations}
                )
                .Join(context.Universities,
                        c => c.educations.University_Id,
                        u => u.Id,
                        (combined, university) => new GetEmployeeVM {
                            FullName = combined.combined.combined.employees.FirstName + " " + combined.combined.combined.employees.LastName,
                            Phone = combined.combined.combined.employees.Phone,
                            BirthDate = combined.combined.combined.employees.BirthDate,
                            Salary = combined.combined.combined.employees.Salary,
                            Email = combined.combined.combined.employees.Email,
                            Gender = combined.combined.combined.employees.Gender,
                            GPA = combined.educations.GPA,
                            University_Name = university.Name

                        }
                ).ToList();
                
            return (IEnumerable<GetEmployeeVM>)data;
            // context dari schema database context, employees dari dbset
            // return context.Employees.ToList();
        }

        public GetEmployeeVM Get(string NIK)
        {
            var data = context.Employees.Where(e => e.NIK == NIK)
                .Join(context.Accounts,
                        e => e.NIK,
                        a => a.NIK,
                        (employees, accounts) => new {employees, accounts}
                )
                .Join(context.Profilings,
                        e => e.accounts.NIK,
                        p => p.NIK,
                        (combined, profilings) => new {combined, profilings}
                )
                .Join(context.Educations,
                        c => c.profilings.Education_id,
                        e => e.Id,
                        (combined, educations) => new {combined, educations}
                )
                .Join(context.Universities,
                        c => c.educations.University_Id,
                        u => u.Id,
                        (combined, university) => new GetEmployeeVM {
                            FullName = combined.combined.combined.employees.FirstName + " " + combined.combined.combined.employees.LastName,
                            Phone = combined.combined.combined.employees.Phone,
                            BirthDate = combined.combined.combined.employees.BirthDate,
                            Salary = combined.combined.combined.employees.Salary,
                            Email = combined.combined.combined.employees.Email,
                            Gender = combined.combined.combined.employees.Gender,
                            GPA = combined.educations.GPA,
                            University_Name = university.Name

                        }
                ).FirstOrDefault();

            return data;
        }

        public int Insert(RegisterVM employee) // jika return 0 error, jika >=1 berhasil
        {
            // sebelum yang menggunakan ViewModel
            // employee.NIK = newNIK;
            // context.Employees.Add(employee); // menambahkan data dari argumen method
            // var save = context.SaveChanges(); // menyimpan hasil data yang disimpan
            // return save;
            

            // menggunakan ViewModel
            string newNIK = GenerateNIK();
            // Employee
            Employee employeeData = new Employee {
                NIK = newNIK,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Phone = employee.Phone,
                BirthDate = employee.BirthDate,
                Salary = employee.Salary,
                Email = employee.Email,
                Gender = (Models.Gender)employee.Gender,
            };
            context.Employees.Add(employeeData);
            var saveEmployee = context.SaveChanges();

            // Account
            Account accountData = new Account {
                NIK = newNIK,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(employee.Password, 13)
                // Password = PasswordHashing(employee.Password);
            };
            context.Accounts.Add(accountData);
            var saveAccount = context.SaveChanges();

            // Education
            Education educationData = new Education {
                Degree = (Models.Degree)employee.Degree,
                GPA = employee.GPA,
                University_Id = employee.University_Id
            };
            context.Educations.Add(educationData);
            var saveEducation = context.SaveChanges();

            // Profiling
            Profiling profilingData = new Profiling {
                NIK = newNIK,
                Education_id = educationData.Id
            };
            context.Profilings.Add(profilingData);
            var saveProfiling = context.SaveChanges();

            if((saveEmployee > 0) && (saveAccount > 0) && (saveEducation > 0) && (saveProfiling > 0)) {
                return 1;
            } else {
                return 0;
            }
        }

        public string GenerateNIK() {
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
            return newNIK;
        }

        public int Update(Employee employee)
        {
            context.Entry(employee).State = EntityState.Modified;
            var result = context.SaveChanges();
            return result;
        }

        // utility
        public bool CheckEmailUnique(string email) {
            var data = context.Employees.AsNoTracking().FirstOrDefault(employee => employee.Email == email);
            if(data == null){
                return false;
            }
            return true;
        }

        public bool CheckPhoneUnique(string phone) {
            var data = context.Employees.AsNoTracking().FirstOrDefault(employee => employee.Phone == phone);
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

        public bool CheckUniversityExist(int id) {
            var data = context.Universities.Find(id);
            if(data == null) {
                return false;
            }
            return true;
        }

    }
}