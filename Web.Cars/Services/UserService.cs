using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Cars.Abstract;
using Web.Cars.Data;
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
        private readonly AppEFContext _context;
        public UserService(UserManager<AppUser> userManager,
             IJwtTokenService jwtTokenService,
             SignInManager<AppUser> signInManager,
             IMapper mapper, AppEFContext context)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _context = context;
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

        public void UpdateUser(UserSaveViewModel model)
        {
            try
            {
                var user = _context.Users
                    .SingleOrDefault(x => x.Id == model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.FIO = model.FIO;
                    string fileName = String.Empty;
                    if (model.Photo != null)
                    {
                        string randomFilename = user.Photo;

                        string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "images");
                        fileName = Path.Combine(dirPath, randomFilename);
                        using (var file = File.Create(fileName))
                        {
                            model.Photo.CopyTo(file);
                        }
                    }
                    _context.SaveChanges();

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
