using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Cars.Data;

namespace Web.Cars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppEFContext _context;
        public UsersController(AppEFContext context)
        {
            _context = context;
        }
        [Route("all")]
        [HttpGet]
        public IActionResult GetUsers()
        {
            var list = _context.Users.Select(x => new {
               fio = x.FIO,
               Email = x.Email,
               Image = "/images/" + x.Photo
            }).ToList();
            return Ok(list);
        }
    }
}
