using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.ViewModel;

namespace api.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        bool Login(string Email, string Password);
        bool ForgetPassword(string Email);
        bool ChangePassword(ChangePasswordVM ChangePassword);
    }
}