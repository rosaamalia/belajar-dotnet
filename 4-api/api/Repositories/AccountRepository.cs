using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;

namespace api.Repositories
{
    public class AccountRepository
    {
        private readonly MyContext context;

        public AccountRepository(MyContext context) {
            this.context = context;
        }

        public bool Login(string email, string password) {
            var data = context.Employees.FirstOrDefault(employee => employee.Email == email);
            var account = context.Accounts.Single(account => account.NIK == data.NIK);

            bool isValid = BCrypt.Net.BCrypt.EnhancedVerify(password, account.Password);
            return isValid;
        }
    }
}