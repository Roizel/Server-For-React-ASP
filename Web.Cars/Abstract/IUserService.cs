using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Cars.Models;

namespace Web.Cars.Abstract
{
    public interface IUserService
    {  /*Create Abstract Method*/
        public Task<string> CreateUser(RegisterViewModel model);
    }
}
