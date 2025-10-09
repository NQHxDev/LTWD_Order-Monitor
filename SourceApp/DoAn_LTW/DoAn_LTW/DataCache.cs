using DoAn_LTW.ContextDatabase;
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
