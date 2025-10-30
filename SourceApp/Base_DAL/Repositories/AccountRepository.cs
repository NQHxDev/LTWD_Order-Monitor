using Base_DAL.ContextDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_DAL.Repositories
{
    public class AccountRepository
    {
        private readonly OrderMonitor ConDatabase = new OrderMonitor();

        public List<account> GetAllAccounts()
        {
            return ConDatabase.account.ToList();
        }

        public void Add_Account(account newAccount)
        {
            ConDatabase.account.Add(newAccount);
            ConDatabase.SaveChanges();
        }

        public void UpdateAccount(account acc)
        {
            ConDatabase.Entry(acc).State = System.Data.Entity.EntityState.Modified;
            ConDatabase.SaveChanges();
        }

        public account GetAccountLogin(string username, string password)
        {
            return ConDatabase.account.FirstOrDefault(acc => acc.username == username && acc.password == password);
        }

        public account GetAccountById(int? accountId)
        {
            return ConDatabase.account.FirstOrDefault(acc => acc.ac_id == accountId);
        }
    }
}
