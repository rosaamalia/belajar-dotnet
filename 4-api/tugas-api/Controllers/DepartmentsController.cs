using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tugas_api.Models;
using tugas_api.Repositories;
using tugas_api.ViewModels;

namespace tugas_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly DepartmentRepository repository;

        public DepartmentsController(DepartmentRepository repository) {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult Get() {
            var data = repository.Get(); // mengambil fungsi dari repositori
            if(data.Count() != 0) {
                return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil ditemukan.", data = data });
            } else {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan."});
            }
        }

        [HttpGet("paging")]
        public ActionResult GetPaging() {
            var draw = Request.Query["draw"].FirstOrDefault();
            var sortColumn = Request.Query["columns[" + Request.Query["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Query["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Query["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Query["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Query["start"].FirstOrDefault() ?? "0");

            var inputData = new DataTableParamVM {
                draw = draw,
                sortColumn = sortColumn,
                sortColumnDirection = sortColumnDirection,
                searchValue = searchValue,
                pageSize = pageSize,
                skip = skip,
            };
            // inputData.

            var data = repository.GetPaging(inputData);

            // var data = repository.Get().AsQueryable();

            // totalRecord = data.Count();
            // // search data when search value found
            // if (!string.IsNullOrEmpty(searchValue)) {
            //     data = data.Where(x => x.Dept_ID.ToLower().Contains(searchValue.ToLower()) || x.Name.ToLower().Contains(searchValue.ToLower()));
            // }
            // // get total count of records after search
            // filterRecord = data.Count();

            // //sort data
            // if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)) 
            // {
            //     data = data.OrderBy(sortColumn + " " + sortColumnDirection);
            //     // data = data.OrderBy((item) => item.PropertyToOrderBy);
            // }


            // //pagination
            // var empList = data.Skip(skip).Take(pageSize).ToList();
            // var returnObj = new {
            //     draw = draw, recordsTotal = totalRecord, recordsFiltered = filterRecord, data = empList
            // };

            // Console.WriteLine(sortColumn);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public virtual ActionResult Get(string id) {
            var data = repository.Get(id);
            if(data != null) {
                return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil ditemukan.", data = data });
            } else {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan."});
            }
        }

        [HttpPost]
        public virtual ActionResult Insert(DepartmentVM department) {
            if(repository.CheckNameUnique(department.Name)==true) {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Nama departemen sudah ada." });
            }
            
            repository.Insert(department);
            return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil ditambahkan." });
        }

        [HttpDelete("{id}")]
        public virtual ActionResult Delete(string id) {
            if(repository.CheckEmployeeExist(id)==true) {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Data tidak dapat dihapus, karena masih ada data employee." });
            }

            repository.Delete(id);
            return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil dihapus." });
        }

        [HttpPut("{id}")]
        public virtual ActionResult Update(string id, DepartmentVM department) {
            if(repository.CheckNameUnique(department.Name)==true) {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Nama departemen sudah ada." });
            }

            repository.Update(id, department);
            return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil diubah." });
        }
    }
}