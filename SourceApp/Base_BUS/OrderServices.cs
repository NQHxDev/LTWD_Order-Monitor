using Base_DAL.ContextDatabase;
using Base_DAL.Repositories;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_BUS
{
    public class OrderServices
    {
        private static OrderServices _instance;

        public static OrderServices Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new OrderServices();
                return _instance;
            }
        }

        private readonly OrderRepository orderRepo;

        private OrderServices()
        {
            orderRepo = new OrderRepository();
        }

        public List<list_order> GetListOrder()
        {
            return orderRepo.GetListOrderWithDetails();
        }

        public List<JObject> GetOrdersByStatus(int status)
        {
            var result = new List<JObject>();
            try
            {
                var orders = orderRepo.GetOrdersByStatus(status);

                foreach (var order in orders)
                {
                    var cartArray = new JArray();
                    foreach (var detail in order.order_detail)
                    {
                        cartArray.Add(new JObject
                        {
                            ["id"] = detail.food_id,
                            ["name"] = FoodServices.Instance.GetFoodName(detail.food_id),
                            ["quantity"] = detail.quantity,
                            ["price"] = (int)detail.price
                        });
                    }

                    var orderJson = new JObject
                    {
                        ["orderId"] = order.oder_id,
                        ["customer_name"] = order.customer_name ?? "Khách lạ",
                        ["cart"] = cartArray,
                        ["status"] = order.status,
                        ["total_price"] = order.total_price
                    };

                    result.Add(orderJson);
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("ws_log.txt", $"[{DateTime.Now}] GetOrdersByStatus({status}) error: {ex.Message}\n");
            }

            return result;
        }
    
        public List<order_detail> GetOrderDetailByID(int orderID)
        {
            return orderRepo.GetOrderDetailByID(orderID);
        }
    }
}
