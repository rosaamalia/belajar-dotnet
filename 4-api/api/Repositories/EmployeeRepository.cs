using api.Context;
using api.Interfaces;
using api.Model;
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

        public IEnumerable<Employee> Get()
        {
            // context dari schema database context, employees dari dbset
            return context.Employees.ToList();
        }

        public Employee Get(string NIK)
        {
            var data = context.Employees.Find(NIK);
            return data;
        }

        public int Insert(Employee employee) // jika return 0 error, jika >=1 berhasil
        {
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

            employee.NIK = newNIK;
            context.Employees.Add(employee); // menambahkan data dari argumen method
            var save = context.SaveChanges(); // menyimpan hasil data yang disimpan
            return save;
        }

        public int Update(Employee employee)
        {
            context.Entry(employee).State = EntityState.Modified;
            var result = context.SaveChanges();
            Console.WriteLine(result);
            return result;
        }

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

    }
}