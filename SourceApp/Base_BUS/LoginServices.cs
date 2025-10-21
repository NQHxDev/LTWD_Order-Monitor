using Base_DAL.ContextDatabase;
using Base_DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_BUS
{
    public class LoginServices
    {
        private readonly AccountRepository accountRepo = new AccountRepository();

        public account CurrentUser { get; private set; }
        public account CurrentAdmin { get; private set; }

        public bool IsLoggedIn => CurrentUser != null || CurrentAdmin != null;

        public account Login(string username, string password)
        {
            var user = accountRepo.GetAccountLogin(username, password);
            if (user == null)
                return null;

            if (user.role == 1)
                CurrentAdmin = user;
            else
                CurrentUser = user;

            return user;
        }

        public void Logout(string roleUser)
        {
            if (roleUser == "admin")
            {
                CurrentAdmin = null;
            } 
            else
            {
                CurrentUser = null;
            }
        }
    }
}
