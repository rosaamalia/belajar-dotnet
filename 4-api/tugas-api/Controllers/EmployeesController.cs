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
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeRepository repository;

        public EmployeesController(EmployeeRepository repository) {
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

        [HttpGet("{NIK}")]
        public virtual ActionResult Get(string NIK) {
            var data = repository.Get(NIK);
            if(data != null) {
                return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil ditemukan.", data = data });
            } else {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan."});
            }
        }

        [HttpPost]
        public virtual ActionResult Insert(EmployeeVM employee) {
            if(repository.CheckPhoneUnique(employee.PhoneNumber, null)==true) {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Nomor ponsel sudah terdaftar." });
            } else if(repository.CheckDepartmentExist(employee.Department_id)==false) {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Departemen tidak tersedia." });
            }
            
            repository.Insert(employee);
            return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil ditambahkan." });
        }

        [HttpDelete("{NIK}")]
        public virtual ActionResult Delete(string NIK) {
            if(repository.CheckNIKExist(NIK)==false) {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "NIK tidak ditemukan." });
            }

            repository.Delete(NIK);
            return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil dihapus." });
        }

        [HttpPut("{NIK}")]
        public virtual ActionResult Update(string NIK, EmployeeVM employee) {
            if(repository.CheckNIKExist(NIK)==false) {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "NIK tidak ditemukan." });
            } else if(repository.CheckPhoneUnique(employee.PhoneNumber, NIK)==true) {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Nomor ponsel sudah terdaftar."});
            }

            repository.Update(NIK, employee);
            return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil diubah." });
        }

        // get
        [HttpGet("active")]
        public ActionResult GetActiveEmployees() {
            var data = repository.GetActiveEmployees(); // mengambil fungsi dari repositori
            if(data.Count() != 0) {
                return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil ditemukan.", data = data });
            } else {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan."});
            }
        }

        [HttpGet("inactive")]
        public ActionResult GetInactiveEmployees() {
            var data = repository.GetInactiveEmployees(); // mengambil fungsi dari repositori
            if(data.Count() != 0) {
                return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil ditemukan.", data = data });
            } else {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan."});
            }
        }

        [HttpGet("active/{department_id}")]
        public ActionResult GetActiveEmployees(string department_id) {
            var data = repository.GetActiveEmployeesByDepartment(department_id); // mengambil fungsi dari repositori
            if(data.Count() != 0) {
                return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil ditemukan.", data = data });
            } else {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan."});
            }
        }

        [HttpGet("inactive/{department_id}")]
        public ActionResult GetInactiveEmployees(string department_id) {
            var data = repository.GetActiveEmployeesByDepartment(department_id); // mengambil fungsi dari repositori
            if(data.Count() != 0) {
                return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil ditemukan.", data = data });
            } else {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan."});
            }
        }
    }
}