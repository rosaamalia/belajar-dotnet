using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        bool Login(string email, string password);
        // void SendEmail(string email);
        bool ForgetPassword(string email);
    }
}