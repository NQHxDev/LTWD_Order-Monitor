using Base_DAL.ContextDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<order_detail> GetListOrderDetail()
        {
            using (var context = new OrderMonitor())
            {
                return context.order_detail.ToList();
            }
        }
    }
}
