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
                return Ok(data);
            } else {
                return NotFound("Data tidak tersedia.");
            }
        }

        [HttpGet("{NIK}")]
        public virtual ActionResult Get(string NIK) {
            var data = repository.Get(NIK);
            if(data != null) {
                return Ok(data);
            } else {
                return NotFound("Data tidak tersedia.");
            }
        }

        [HttpPost]
        public virtual ActionResult Insert(Employee employee) {
            if(repository.CheckEmailUnique(employee.Email)==true) {
                return BadRequest("Email tidak boleh duplikat.");
            } else if(repository.CheckPhoneUnique(employee.Phone)==true) {
                return BadRequest("Phone tidak boleh duplikat.");
            }
            
            var result = repository.Insert(employee);
            if(result > 0) {
                return Ok("Data berhasil ditambahkan.");
            } else {
                return Problem("Data tidak berhasil ditambahkan.");
            }
        }

        [HttpDelete("{NIK}")]
        public virtual ActionResult Delete(string NIK) {
            var result = repository.Delete(NIK);
            if(result > 0) {
                return Ok("Data berhasil dihapus.");
            } else {
                return NotFound("Data tidak tersedia.");
            }
        }

        [HttpPut]
        public virtual ActionResult Update(Employee employee) {
            if(repository.CheckNIKExist(employee.NIK)==false) {
                return NotFound("NIK tidak ditemukan.");
            } else if(repository.CheckEmailUnique(employee.Email)==true) {
                return BadRequest("Email tidak boleh duplikat.");
            } else if(repository.CheckPhoneUnique(employee.Phone)==true) {
                return BadRequest("Phone tidak boleh duplikat.");
            }

            var result = repository.Update(employee);
            if(result > 0) {
                return Ok("Data berhasil diubah.");
            } else {
                return Problem("Data tidak berhasil diubah.");
            }
        }
    }
}