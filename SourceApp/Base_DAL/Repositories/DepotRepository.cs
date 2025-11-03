using Base_DAL.ContextDatabase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Base_DAL.Repositories
{
    public class DepotRepository
    {
        private readonly OrderMonitor ConDatabase = new OrderMonitor();

        public void UpdateImport(import importOrder)
        {
            if (importOrder == null) return;

            var currentOrderImport = ConDatabase.import.Find(importOrder.import_id);
            if (currentOrderImport == null) return;

            currentOrderImport.import_status = importOrder.import_status;
            currentOrderImport.reason = importOrder.reason;
            currentOrderImport.update_by = importOrder.update_by;
            currentOrderImport.update_at = importOrder.update_at;
            currentOrderImport.total_price = importOrder.total_price;
            currentOrderImport.total_item = importOrder.total_item;

            ConDatabase.SaveChanges();
        }

    }
}
