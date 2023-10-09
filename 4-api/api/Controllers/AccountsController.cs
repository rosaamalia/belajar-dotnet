using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using api.Repositories;
using Microsoft.AspNetCore.Mvc;
using api.ViewModel;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        
        private readonly AccountRepository repository;

        public AccountsController(AccountRepository repository) {
            this.repository = repository;
        }

        [HttpPost("login")]
        public ActionResult Login(string email, string password) {
            // Cek email di database
            var emailExist = repository.GetEmployeeByEmail(email);
            if(emailExist == null) {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Email belum terdaftar."});
            }
            
            var result = repository.Login(email, password);

            if(result==false){
                return BadRequest( new {status = HttpStatusCode.BadRequest, message = "Email atau Password salah."});
            }

            return Ok(new {status = HttpStatusCode.OK, message = "Login berhasil."});
        }

        [HttpPost("forget-password")]
        public ActionResult ForgetPassword(string email)
        {
            var result = repository.ForgetPassword(email);
            if(result) {
                return Ok(new {status = HttpStatusCode.OK, message = "Kode OTP berhasil dikirim."});
            }

            return BadRequest(new {status = HttpStatusCode.BadRequest, message = "Email belum terdaftar."});
        }

        [HttpPost("change-password")]
        public ActionResult ChangePassword(ChangePasswordVM ChangePassword) {
            if(ChangePassword.OTP == "") {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Kode OTP belum terisi."});
            }
            // Cek email di database
            var emailExist = repository.GetEmployeeByEmail(ChangePassword.Email);
            if(emailExist == null) {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Email belum terdaftar."});
            }

            // Cek password sudah sesuai dengan konfirmasi password atau belum
            if(ChangePassword.Password != ChangePassword.CheckPassword) {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Konfirmasi password yang dimasukkan salah."});
            }

            // Cek OTP sudah digunakan atau belum
            if(repository.IsUsed(ChangePassword.Email)) {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Kode OTP sudah digunakan."});
            }

            // Cek OTP sudah expired atau belum
            if(repository.Expired(ChangePassword.Email)) {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Kode OTP sudah kedaluwarsa."});
            
            }

            // Cek OTP
            var result = repository.ChangePassword(ChangePassword);
            if(result==false) {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Kode OTP yang dimasukkan salah."});
            }
            return Ok(new { status = HttpStatusCode.OK, message = "Password berhasil diubah."});
        }
    }
}