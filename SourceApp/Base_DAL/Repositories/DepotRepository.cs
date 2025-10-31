using Base_DAL.ContextDatabase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_DAL.Repositories
{
    public class DepotRepository
    {
        private readonly OrderMonitor ConDatabase = new OrderMonitor();

        public void UpdateImport(import importOrder)
        {
            ConDatabase.Entry(importOrder).State = System.Data.Entity.EntityState.Modified;
            ConDatabase.SaveChanges();
        }
    }
}
