using Base_DAL.ContextDatabase;
using Base_DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_BUS
{
    public class AccountServices
    {
        private static AccountServices _instance;

        public static AccountServices Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AccountServices();
                return _instance;
            }
        }

        private readonly AccountRepository accountRepo;

        public account Current_Staff { get; private set; }
        public account Current_Leader { get; private set; }

        public bool IsLogged_Staff => Current_Staff != null;

        public bool IsLogged_Leader => Current_Leader != null;

        private AccountServices()
        {
            accountRepo = new AccountRepository();
        }

        public List<account> GetAllAccounts()
        {
            return accountRepo.GetAllAccounts();
        }

        public void AddNewAccount(account newAccount)
        {
            accountRepo.Add_Account(newAccount);
        }

        public bool CheckUsername(string username)
        {
            var allAccounts = accountRepo.GetAllAccounts();
            return allAccounts.Any(acc => acc.username == username);
        }

        public void UpdatePassword(int accountId, string newPassword)
        {
            var account = accountRepo.GetAccountById(accountId);
            if (account != null)
            {
                account.password = newPassword;
                accountRepo.UpdateAccount(account);
            }
        }

        public void UpdateStatus(int accountId, int newStatus)
        {
            var account = accountRepo.GetAccountById(accountId);
            if (account != null)
            {
                account.acc_status = newStatus;
                accountRepo.UpdateAccount(account);
            }
        }

        public string GetNameUser(int? accountId)
        {
            var account = accountRepo.GetAccountById(accountId);
            return account != null ? account.name : $"User ID: {accountId}";
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
            if (roleUser == "leader")
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
