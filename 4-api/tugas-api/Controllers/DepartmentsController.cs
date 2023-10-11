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
            // if(repository.CheckNIKExist(NIK)==false) {
            //     return NotFound(new { status = HttpStatusCode.NotFound, message = "NIK tidak ditemukan." });
            // }

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