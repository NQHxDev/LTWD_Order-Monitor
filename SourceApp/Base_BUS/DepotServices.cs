using Base_DAL.ContextDatabase;
using Base_DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_BUS
{
    public class DepotServices
    {
        private readonly DepotRepository depotRepo;
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
            depotRepo = new DepotRepository();
        }

        public class DepotOrderItem
        {
            public int? ItemId { get; set; }
            public string ItemName { get; set; }
            public int UnitId { get; set; }
            public decimal Quantity { get; set; }
            public bool IsNew { get; set; }
        }

        public import GetImportByID(int orderImportID)
        {
            using (var contextDB = new OrderMonitor())
            {
                return contextDB.import.Include("account").FirstOrDefault(i => i.import_id == orderImportID);
            }
        }

        public import GetImportByIDWithDetails(int orderImportID)
        {
            using (var contextDB = new OrderMonitor())
            {
                return contextDB.import
                    .Include("account")
                    .Include("import_detail")
                    .Include("import_detail.item")
                    .Include("import_detail.item.unit")
                    .FirstOrDefault(i => i.import_id == orderImportID);
            }
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
                    total_price = 0
                };

                contextDB.import.Add(import);
                contextDB.SaveChanges();

                decimal totalPrice = 0;

                foreach (var item in selectedItems)
                {
                    int itemId;
                    decimal importPrice = 0;

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
                        importPrice = 0;
                    }
                    else
                    {
                        var currentItem = contextDB.item.FirstOrDefault(x => x.item_id == item.ItemId);

                        if (currentItem != null)
                        {
                            itemId = currentItem.item_id;
                            importPrice = currentItem.import_price;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    totalPrice += importPrice * item.Quantity;

                    var detail = new import_detail
                    {
                        import_id = import.import_id,
                        item_id = itemId,
                        quantity = item.Quantity
                    };

                    contextDB.import_detail.Add(detail);
                }

                import.total_price = totalPrice;

                contextDB.SaveChanges();
            }
        }

        public void UpdateImportOrder(import importOrder)
        {
            if (importOrder == null)
                return;
            depotRepo.UpdateImport(importOrder);
        }

        public (int pending, int approvedToday, int rejectedToday) GetImportStatistics()
        {
            using (var db = new OrderMonitor())
            {
                var pending = db.import.Count(x => x.import_status == 0);
                var approvedToday = db.import.Count(x => x.import_status == 1
                                        && DbFunctions.TruncateTime(x.create_at) == DateTime.Today);
                var rejectedToday = db.import.Count(x => x.import_status == 2
                                        && DbFunctions.TruncateTime(x.create_at) == DateTime.Today);

                return (pending, approvedToday, rejectedToday);
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

        public List<import> GetListOrderImportUnconfirmed()
        {
            var listOrderImport = new List<import>();
            using (var conDatabase = new OrderMonitor())
            {
                listOrderImport = conDatabase.import
                    .Include(i => i.account)
                    .Where(x => x.import_status == 0)
                    .OrderByDescending(x => x.create_at)
                    .ToList();
            }
            return listOrderImport;
        }

        public List<import_detail> GetImportDetailByID(int orderImportID)
        {
            var listImportDetail = new List<import_detail>();
            using (var conDatabase = new OrderMonitor())
            {
                listImportDetail = conDatabase.import_detail
                    .Include("item.unit")
                    .Where(id => id.import_id == orderImportID)
                    .ToList();
            }
            return listImportDetail;
        }
    }
}
