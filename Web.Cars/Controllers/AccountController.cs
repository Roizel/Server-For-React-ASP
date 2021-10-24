using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Web.Cars.Abstract;
using Web.Cars.Data.Identity;
using Web.Cars.Exceptions;
using Web.Cars.Models;
using Web.Cars.Services;

namespace Web.Cars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService userService;
        public AccountController(IUserService _userService)
        {
            userService = _userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            try /*If Good, aend OK to Frontend*/
            {
                string token = await userService.CreateUser(model); /*Create user*/
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
            catch(Exception ex) /*For undefined exceptions*/
            {
                return BadRequest(new AccountError("Something went wrong on server" + ex.Message)); /*Send bedrik to frontend*/
            }
        }
    }



}
