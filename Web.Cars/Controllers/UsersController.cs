using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Cars.Abstract;
using Web.Cars.Data;
using Web.Cars.Exceptions;
using Web.Cars.Models;

namespace Web.Cars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppEFContext _context;
        private readonly IUserService _userService;
        public UsersController(AppEFContext context, IUserService userService)
        {
            _userService = userService;
            _context = context;
        }
        [Route("all")]
        [HttpGet]
        public IActionResult GetUsers()
        {
            var list = _context.Users.Select(x => new 
            {
               fio = x.FIO,
               Email = x.Email,
               Image = "/images/" + x.Photo
            }).ToList();
            return Ok(list);
        }
        [Route("delete")] /*[HttpPost("register")] - хз чого, но так не робить, треба писати HttpPost i Route*/
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody]string email) /*FromBody - без цього в буде приходити null(хз чого)*/
        {
            try /*If Good, send OK to Frontend*/
            {
                string token = await _userService.DeleteUser(email); /*Call DeleteUser from /Services/UserService.cs*/
                if (token == null)
                    return BadRequest(); /*Bedrik*/

                return Ok(new
                {
                    token /*All good*/
                });
            }
            catch (AccountException aex) /*If Bad, send errors to Frontend*/
            {
                return BadRequest(aex.AccountError);
            }
            catch (Exception ex) /*For undefined exceptions*/
            {
                return BadRequest(new AccountError("Something went wrong on server " + ex.Message)); /*Send bedrik to frontend*/
            }
        }
    }
}
