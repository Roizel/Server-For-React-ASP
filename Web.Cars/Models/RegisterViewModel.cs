using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Cars.Models
{
    public class RegisterViewModel
    {
        public string FIO { get; set; }
        public string Email { get; set; }
        public IFormFile Photo { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
