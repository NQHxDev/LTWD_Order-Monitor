using Base_DAL.ContextDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Base_DAL.Repositories
{
    public class FoodRepository
    {
        public List<food> GetListFoods()
        {
            using (var context = new OrderMonitor())
            {
                return context.food.ToList();
            }
        }

        public List<unit> GetAllUnits()
        {
            using (var context = new OrderMonitor())
            {
                return context.unit.ToList();
            }
        }

        public List<item> GetAllItems()
        {
            using (var context = new OrderMonitor())
            {
                return context.item.Include(i => i.unit).ToList();
            }
        }

        public List<food_ingredient> GetAllFoodIngredients()
        {
            using (var context = new OrderMonitor())
            {
                return context.food_ingredient
                    .Include(fi => fi.item)
                    .Include(fi => fi.item.unit)
                    .ToList();
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
    }
}
