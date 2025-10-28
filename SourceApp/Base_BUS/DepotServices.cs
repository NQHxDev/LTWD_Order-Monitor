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

        public class DepotOrderItem
        {
            public int? ItemId { get; set; }
            public string ItemName { get; set; }
            public int UnitId { get; set; }
            public decimal Quantity { get; set; }
            public bool IsNew { get; set; }
        }

        public void SaveOrderImport(int created_ByID, List<DepotOrderItem> selectedItems)
        {
            using (var contextDB = new OrderMonitor())
            {
                var import = new import
                {
                    create_at = DateTime.Now,
                    import_status = 0,
                    created_by = created_ByID,
                    total_item = selectedItems.Count,
                };
                contextDB.import.Add(import);
                contextDB.SaveChanges();

                foreach (var item in selectedItems)
                {
                    int itemId;

                    if (item.IsNew)
                    {
                        var newItem = new item
                        {
                            name = item.ItemName,
                            unit_id = item.UnitId,
                            import_price = 0,
                            is_active = true,
                            quantity = 0
                        };
                        contextDB.item.Add(newItem);
                        contextDB.SaveChanges();
                        itemId = newItem.item_id;
                    }
                    else
                    {
                        itemId = item.ItemId ?? 0;
                    }

                    var detail = new import_detail
                    {
                        import_id = import.import_id,
                        item_id = itemId,
                        quantity = item.Quantity
                    };
                    contextDB.import_detail.Add(detail);
                }
                contextDB.SaveChanges();
            }
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

        public List<import> GetListOrderImport()
        {
            var listOrderImport = new List<import>();
            using (var conDatabase = new OrderMonitor())
            {
                listOrderImport = conDatabase.import.OrderByDescending(i => i.import_id).ToList();
            }
            return listOrderImport;
        }

        public List<import_detail> GetImportDetailByID(int import_id)
        {
            var listImportDetail = new List<import_detail>();
            using (var conDatabase = new OrderMonitor())
            {
                listImportDetail = conDatabase.import_detail
                    .Include("account")
                    .Include("item")
                    .Where(id => id.import_id == import_id)
                    .ToList();
            }
            return listImportDetail;
        }
    }
}
