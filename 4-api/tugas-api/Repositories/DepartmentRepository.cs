using tugas_api.Context;
using tugas_api.Repositories.Interfaces;
using tugas_api.Models;
using tugas_api.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace tugas_api.Repositories
{
    public class DepartmentRepository
    {
        private readonly MyContext context;

        public DepartmentRepository(MyContext context) {
            this.context = context;
        }

        public IEnumerable<Department> Get(){
            return context.Departments.ToList();
        }

        public Object GetPaging(DataTableParamVM inputData) {
            var totalRecord = 0;
            var filterRecord = 0;

            var data = context.Departments.AsQueryable();
            totalRecord = data.Count();

            // search data when search value found
            if (!string.IsNullOrEmpty(inputData.searchValue)) {
                data = data.Where(x => x.Dept_ID.ToLower().Contains(inputData.searchValue.ToLower()) || x.Name.ToLower().Contains(inputData.searchValue.ToLower()));
            }
            // get total count of records after search
            filterRecord = data.Count();

            //sort data
            if (!string.IsNullOrEmpty(inputData.sortColumn) && !string.IsNullOrEmpty(inputData.sortColumnDirection)) 
            {
                data = data.OrderBy(inputData.sortColumn + " " + inputData.sortColumnDirection);
                // data = data.OrderBy((item) => item.PropertyToOrderBy);
            }

            //pagination
            var dataList = data.Skip(inputData.skip).Take(inputData.pageSize).ToList();
            var returnObj = new {
                draw = inputData.draw, recordsTotal = totalRecord, recordsFiltered = filterRecord, data = dataList
            };

            return returnObj;
        }

        public Department Get(string id){
            return context.Departments.Find(id);
        }

        public int Insert(DepartmentVM name){
            string Id = "";
            var lastData = context.Departments.OrderBy(data => data.Dept_ID).LastOrDefault();
            if(lastData == null) {
                // kalau ternyata gak ada data di database, otomatis urutan 001
                Id = "D" + "001";
            } else {
                // ada data terakhir, ambil 3 karakter string dari NIK (nomor urut)
                var lastId = lastData.Dept_ID;
                string lastThree = lastId.Substring(lastId.Length-3);
                
                // convert jadi int terus tambah satu
                int nextSequence = int.Parse(lastThree) + 1;
                Id = "D" + nextSequence.ToString("000"); // convert jadi string
            }

            var departmentData = new Department {
                Dept_ID = Id,
                Name = name.Name
            };

            context.Departments.Add(departmentData);
            var saveDepartment = context.SaveChanges();

            return saveDepartment;
        }

        public int Update(string id, DepartmentVM department){
            var data = context.Departments.Find(id);
            data.Name = department.Name;
            var result = context.SaveChanges();
            return result;
        }

        public int Delete(string id){
            var data = context.Departments.Find(id);
            context.Remove(data);
            var result = context.SaveChanges();
            return result;
        }

        public bool CheckNameUnique(string name) {
            var data = context.Departments.AsNoTracking().FirstOrDefault(department => department.Name == name);
            if(data == null){
                return false;
            }
            return true;
        }

        public bool CheckEmployeeExist(string department_id) {
            var data = context.Employees.AsNoTracking().FirstOrDefault(employee => employee.Department_id == department_id);
            if(data == null){
                return false;
            }
            return true;
        }
    }
}