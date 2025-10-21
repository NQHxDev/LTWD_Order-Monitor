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
        private static LoginServices _instance;

        public static LoginServices Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LoginServices();
                return _instance;
            }
        }

        private readonly AccountRepository accountRepo;

        public account Current_Staff { get; private set; }
        public account Current_Leader { get; private set; }

        public bool IsLogged_Staff => Current_Staff != null;

        public bool IsLogged_Leader => Current_Leader != null;

        private LoginServices()
        {
            accountRepo = new AccountRepository();
        }

        public account Login(string username, string password)
        {
            var userLogin = accountRepo.GetAccountLogin(username, password);
            if (userLogin == null)
                return null;

            if (userLogin.role == 1)
                Current_Leader = userLogin;
            else
                Current_Staff = userLogin;

            return userLogin;
        }

        public void Logout(string roleUser)
        {
            if (roleUser == "admin")
            {
                Current_Leader = null;
            } 
            else
            {
                Current_Staff = null;
            }
        }
    }
}
