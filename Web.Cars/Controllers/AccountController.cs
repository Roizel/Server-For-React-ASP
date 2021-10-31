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
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtTokenService _tokenService;
        public AccountController(IUserService _userService, UserManager<AppUser> userManager, IJwtTokenService tokenService)
        {
            userService = _userService;
            _userManager = userManager;
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            try /*If Good, send OK to Frontend*/
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
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email); /*І так ясно*/
                if (await _userManager.CheckPasswordAsync(user, model.Password)) /*І так ясно*/
                {
                    string token = _tokenService.CreateToken(user); /*Create token and send it to client*/
                    return Ok(
                        new { token }
                    );
                }
                else
                {

                    var exc = new AccountError();
                    exc.Errors.Invalid.Add("Пароль не вірний!");
                    throw new AccountException(exc);
                }
            }
            catch (AccountException aex)
            {
                return BadRequest(aex.AccountError);
            }
            catch
            {
                return BadRequest(new AccountError("Щось пішло не так!"));
            }
        }
    }



}
