using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using System.Threading.Tasks;
using Web.Cars.Abstract;
using Web.Cars.Data.Identity;
using Web.Cars.Exceptions;
using Web.Cars.Models;

namespace Web.Cars.Services
{
    public class UserService : IUserService /*Inherited interface*/
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwtTokenService;
        public UserService(UserManager<AppUser> userManager,
             IJwtTokenService jwtTokenService,
             SignInManager<AppUser> signInManager,
             IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<string> CreateUser(RegisterViewModel model)
        {
            var user = _mapper.Map<AppUser>(model); /*Map AppUser to model*/
            string fileName = String.Empty;
            if (model.Photo != null) /*Images*/
            {
                string randomFilename = Path.GetRandomFileName() +
                    Path.GetExtension(model.Photo.FileName);

                string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "images");
                fileName = Path.Combine(dirPath, randomFilename);
                using (var file = System.IO.File.Create(fileName))
                {
                    model.Photo.CopyTo(file);
                }
                user.Photo = randomFilename;
            }

            var result = await _userManager.CreateAsync(user, model.Password); /*Create user*/
            if (!result.Succeeded)
            {
                if (!string.IsNullOrEmpty(fileName))
                    System.IO.File.Delete(fileName);
                AccountError accountError = new AccountError(); /*Create castom Exception*/
                foreach (var item in result.Errors)
                {
                    accountError.Errors.Invalid.Add(item.Description); /*Add Exceptions to our castom Exceptions*/
                }
                throw new AccountException(accountError); /*Send errors to AccountController*/
            }

            return _jwtTokenService.CreateToken(user);
        }

        //public async Task<string> DeleteUser(string Email)
        //{
        //    var user = await _userManager.FindByEmailAsync(Email);
        //    if (user.Email != null)
        //    {
        //        await _userManager.DeleteAsync(user);
        //        return "User Delete";
        //    }
        //    else
        //    {
        //        string error = "This user does not exist";
        //        AccountError accountError = new AccountError();
        //        accountError.Errors.Invalid.Add(error);
        //        throw new AccountException(accountError);
        //    }
        //}
    }
}
