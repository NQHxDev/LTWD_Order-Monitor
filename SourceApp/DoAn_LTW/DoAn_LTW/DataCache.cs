using DoAn_LTW.ContextDatabase;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DoAn_LTW
{
    public static class DataCache
    {
        public static Dictionary<int, food> Foods { get; private set; } = new Dictionary<int, food>();
        public static Dictionary<int, item> Items { get; private set; } = new Dictionary<int, item>();
        public static Dictionary<int, unit> Units { get; private set; } = new Dictionary<int, unit>();
        public static Dictionary<int, List<food_ingredient>> FoodIngredients { get; private set; } = new Dictionary<int, List<food_ingredient>>();

        private static bool isLoaded = false;

        public static void Initialize()
        {
            if (isLoaded) return;

            try
            {
                using (var context = new OrderMonitor())
                {
                    // Load đơn vị
                    var units = context.unit.ToList();
                    Units = units.ToDictionary(u => u.unit_id, u => u);

                    // Load nguyên liệu
                    var items = context.item.Include(i => i.unit).ToList();
                    Items = items.ToDictionary(i => i.item_id, i => i);

                    // Load Food
                    var foods = context.food.ToList();
                    Foods = foods.ToDictionary(f => f.food_id, f => f);

                    // Load Công thức Food
                    var ingredients = context.food_ingredient
                        .Include(fi => fi.item)
                        .Include(fi => fi.item.unit)
                        .ToList();

                    FoodIngredients = ingredients
                        .GroupBy(fi => fi.food_id)
                        .ToDictionary(g => g.Key, g => g.ToList());

                    isLoaded = true;
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("ws_log.txt", $"[{DateTime.Now}] DataCache error: {ex.Message}\n");
            }
        }

        public static List<JObject> GetPendingOrders()
        {
            var ordersList = new List<JObject>();

            try
            {
                using (var context = new OrderMonitor())
                {
                    var pendingOrders = context.list_order
                        .Include(o => o.order_detail.Select(od => od.food))
                        .Where(o => o.status == 0)
                        .ToList();

                    foreach (var order in pendingOrders)
                    {
                        var cartArray = new JArray();

                        foreach (var detail in order.order_detail)
                        {
                            string foodName = GetFoodName(detail.food_id);
                            cartArray.Add(new JObject
                            {
                                ["id"] = detail.food_id,
                                ["name"] = foodName,
                                ["quantity"] = detail.quantity,
                                ["price"] = (int)detail.price
                            });
                        }

                        var orderJson = new JObject
                        {
                            ["orderId"] = order.oder_id,
                            ["customer_name"] = order.customer_name ?? "Khách lạ",
                            ["customer_phone"] = order.customer_phone ?? "",
                            ["note"] = order.note ?? "",
                            ["cart"] = cartArray
                        };

                        ordersList.Add(orderJson);
                    }
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("ws_log.txt", $"[{DateTime.Now}] GetPendingOrders error: {ex.Message}\n");
            }

            return ordersList;
        }

        public static List<JToken> GetCompletedOrders()
        {
            using (var context = new OrderMonitor())
            {
                var completedOrders = context.list_order
                    .Include(o => o.order_detail.Select(od => od.food))
                    .Where(o => o.status == 1)
                    .OrderByDescending(o => o.updated_at)
                    .ToList();

                var result = new List<JToken>();

                foreach (var order in completedOrders)
                {
                    var cartArray = new JArray();
                    foreach (var detail in order.order_detail)
                    {
                        cartArray.Add(new JObject
                        {
                            ["food_id"] = detail.food_id,
                            ["food_name"] = detail.food?.name ?? "Không rõ món",
                            ["quantity"] = detail.quantity,
                            ["price"] = (int)detail.price
                        });
                    }

                    var orderJson = new JObject
                    {
                        ["orderId"] = order.oder_id,
                        ["customer_name"] = order.customer_name ?? "Khách lạ",
                        ["customer_phone"] = order.customer_phone ?? "",
                        ["note"] = order.note ?? "",
                        ["total_price"] = order.total_price,
                        ["cart"] = cartArray,
                        ["status"] = order.status,
                        ["created_at"] = order.created_at
                    };

                    result.Add(orderJson);
                }

                return result;
            }
        }


        public static string GetFoodName(int id)
        {
            return Foods.TryGetValue(id, out var f) ? f.name : $"Món ID: {id}";
        }

        public static string GetUnitName(int? unitId)
        {
            return (unitId.HasValue && Units.TryGetValue(unitId.Value, out var u)) ? u.name : "";
        }

        public static List<food_ingredient> GetIngredientsByFoodId(int foodId)
        {
            return FoodIngredients.TryGetValue(foodId, out var list) ? list : new List<food_ingredient>();
        }
    }
}
