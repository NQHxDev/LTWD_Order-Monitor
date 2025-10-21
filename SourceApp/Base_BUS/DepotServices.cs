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
        private static DepotServices _instance;

        public static DepotServices Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DepotServices();
                return _instance;
            }
        }

        private DepotServices()
        {
        }

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
