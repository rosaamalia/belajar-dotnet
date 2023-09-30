using System.Net;
using api.Model;
using api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
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
            if(data != null) {
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
        public virtual ActionResult Insert(Employee employee) {
            if(repository.CheckEmailUnique(employee.Email)==true) {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Email tidak boleh duplikat." });
            } else if(repository.CheckPhoneUnique(employee.Phone)==true) {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Phone tidak boleh duplikat." });
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

        [HttpPut]
        public virtual ActionResult Update(Employee employee) {
            if(repository.CheckNIKExist(employee.NIK)==false) {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "NIK tidak ditemukan." });
            } else if(repository.CheckEmailUnique(employee.Email)==true) {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Email telah digunakan."});
            } else if(repository.CheckPhoneUnique(employee.Phone)==true) {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Phone telah digunakan."});
            }

            repository.Update(employee);
            return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil diubah." });
        }
    }
}