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
    public class AccountError /*Error*/
    {
        public AccountError() { }
        public AccountError(string message)
        {
            Errors.Invalid.Add(message);
        }
        public AccountErrorItem Errors { get; set; } = new AccountErrorItem(); /*exmp of AccountErrorItem */
    }

    public class AccountErrorItem
    {
        public List<string> Invalid { get; set; } = new List<string>(); /*List of Errors*/
    }

}
