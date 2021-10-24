using System;
using Web.Cars.Models;

namespace Web.Cars.Exceptions
{
    public class AccountException : Exception /*Create castom Exceptions for send errors to frontend*/
    {
        public AccountError AccountError { get; private set; } /*Create exmp of AccountError*/
        public AccountException(AccountError accountError)
        {
            AccountError = accountError;
        }
    }
}
