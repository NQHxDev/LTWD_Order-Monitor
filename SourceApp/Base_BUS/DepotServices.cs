using Base_DAL.ContextDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_BUS
{
    public class DepotServices
    {
        public List<depot> GetListItemDepot()
        {
            var listItem = new List<depot>();
            using (var conDatabase = new OrderMonitor())
            {
                listItem = conDatabase.depot.Include("item.unit").OrderBy(d => d.item_id).ToList();
            }
            return listItem;
        }
    }
}
