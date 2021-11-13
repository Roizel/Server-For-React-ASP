using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.Cars.Abstract;
using Web.Cars.Data;
using Web.Cars.Data.Identity;
using Web.Cars.Exceptions;
using Web.Cars.Models;

namespace Web.Cars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppEFContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public UsersController(AppEFContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }
        [Route("all")]
        [HttpGet]
        public IActionResult GetUsers()
        {
            Thread.Sleep(2000);
            var list = _context.Users
                .Select(x => _mapper.Map<UserItemViewModel>(x))
                .ToList();
            return Ok(list);
        }
        [Route("delete/{id}")] /*[HttpPost("register")] - хз чого, но так не робить, треба писати HttpPost i Route*/
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id) /*FromBody - без цього в буде приходити null(хз чого)*/
        {
            Thread.Sleep(2000);
            try
            {
                var user = _context.Users.SingleOrDefault(x => x.Id == id);
                if (user == null)
                    return NotFound(); /*Bedrik*/
                if (user.Photo != null)
                {
                    var directory = Path.Combine(Directory.GetCurrentDirectory(), "images");
                    var FilePath = Path.Combine(directory, user.Photo);
                    System.IO.File.Delete(FilePath);
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok();
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
        [Route("edit/{id}")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users
                .SingleOrDefault(x => x.Id == id);
            return Ok(_mapper.Map<UserEditViewModel>(user));
        }
        [HttpPost("save/{id}")]
        public async Task<IActionResult> Save([FromForm] UserSaveViewModel user, int Id)
        {
            try
            {
                AppUser editedUser = await _userManager.FindByIdAsync(Id.ToString());
                editedUser.Email = user.Email;
                editedUser.FIO = user.FIO;
                //AppUser editeduser = new AppUser()
                //{
                //    Id = Id,
                //    Email = user.Email,
                //    FIO = user.FIO,
                //    UserName = user.Email,
                //};
                string fileName = string.Empty;
                if (user.Photo != null) /*Images*/
                {
                    string randomFilename = Path.GetRandomFileName() +
                        Path.GetExtension(user.Photo.FileName);

                    string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "images");
                    fileName = Path.Combine(dirPath, randomFilename);
                    using (var file = System.IO.File.Create(fileName))
                    {
                        user.Photo.CopyTo(file);
                    }
                    editedUser.Photo = randomFilename;
                }
                var result = await _userManager.UpdateAsync(editedUser);
                await _context.SaveChangesAsync();
                if (result.Succeeded)
                {
                    return Ok("all ok");
                }
                else
                {
                    return BadRequest(new AccountError("Something went wrong on server"));
                }

                //}
            }
            catch (AccountException aex) /*If Bad, send errors to Frontend*/
            {

                return BadRequest(aex.AccountError);
            }
            catch (Exception ex) /*For undefined exceptions*/
            {
                return BadRequest(new AccountError("Something went wrong on server: " + ex.Message)); /*Send bedrik to frontend*/
            }
        }
    }
}
