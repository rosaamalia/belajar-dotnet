using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using api.Repositories;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("login")]
        public ActionResult Login(string email, string password) {
            if(email==null || password==null) {
                return BadRequest(new {status = HttpStatusCode.BadRequest, message = "Email atau password tidak boleh kosong."});
            }

            var result = repository.Login(email, password);

            if(result==false){
                return BadRequest( new {status = HttpStatusCode.BadRequest, message = "Email atau Password salah."});
            }

            return Ok(new {status = HttpStatusCode.OK, message = "Login berhasil."});
        }
    }
}