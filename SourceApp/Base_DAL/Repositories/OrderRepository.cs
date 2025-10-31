using Base_DAL.ContextDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Base_DAL.Repositories
{
    public class OrderRepository
    {
        public OrderRepository()
        {
        }

        public List<list_order> GetListOrder()
        {
            using (var context = new OrderMonitor())
            {
                return context.list_order.ToList();
            }
        }

        public List<list_order> GetListOrderWithDetails()
        {
            using (var context = new OrderMonitor())
            {
                return context.list_order
                    .Include("order_detail")
                    .OrderBy(order => order.status == 0 ? 1 :
                                 order.status == 1 ? 2 :
                                 order.status == 2 ? 3 :
                                 order.status == -1 ? 4 : 5)
                    .ThenBy(order => order.created_at)
                    .ToList();
            }
        }

        public List<order_detail> GetListOrderDetail()
        {
            using (var context = new OrderMonitor())
            {
                return context.order_detail.ToList();
            }
        }

        public List<list_order> GetOrdersByStatus(int status)
        {
            using (var context = new OrderMonitor())
            {
                var query = context.list_order
                    .Include(o => o.order_detail)
                    .Include(o => o.order_detail.Select(od => od.food))
                    .Where(o => o.status == status);

                if (status == 1 || status == 2)
                    query = query.OrderByDescending(o => o.updated_at);

                return query.ToList();
            }
        }

        public List<order_detail> GetOrderDetailByID(int orderID)
        {
            using (var context = new OrderMonitor())
            {
                return context.order_detail
                    .Include("food")
                    .Where(od => od.order_id == orderID)
                    .ToList();
            }
        }
    }
}
