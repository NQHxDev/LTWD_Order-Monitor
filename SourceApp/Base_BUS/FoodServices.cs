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
    public class FoodServices
    {
        private static FoodServices _instance;

        public static FoodServices Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FoodServices();
                return _instance;
            }
        }

        private readonly FoodRepository _repo;

        public Dictionary<int, food> Foods = new Dictionary<int, food>();
        public Dictionary<int, item> Items = new Dictionary<int, item>();
        public Dictionary<int, unit> Units = new Dictionary<int, unit>();
        public Dictionary<int, List<food_ingredient>> FoodIngredients = new Dictionary<int, List<food_ingredient>>();

        private FoodServices()
        {
            _repo = new FoodRepository();
        }

        public void Initialize()
        {
            try
            {
                var units = _repo.GetAllUnits();
                Units = units.ToDictionary(u => u.unit_id);

                var items = _repo.GetAllItems();
                Items = items.ToDictionary(i => i.item_id);

                var foods = _repo.GetListFoods();
                Foods = foods.ToDictionary(f => f.food_id);

                var ingredients = _repo.GetAllFoodIngredients();
                FoodIngredients = ingredients
                    .GroupBy(fi => fi.food_id)
                    .ToDictionary(g => g.Key, g => g.ToList());
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("ws_log.txt", $"[{DateTime.Now}] Initialize error: {ex.Message}\n");
            }
        }

        public string GetFoodName(int foodID)
        {
            return Foods.ContainsKey(foodID) ? Foods[foodID].name : $"Món ID: {foodID}";
        }

        public List<food_ingredient> GetIngredientsByFoodId(int foodId)
        {
            return FoodIngredients.ContainsKey(foodId) ? FoodIngredients[foodId] : new List<food_ingredient>();
        }

        public string GetUnitName(int? unitId)
        {
            return (unitId.HasValue && Units.TryGetValue(unitId.Value, out var u)) ? u.name : "";
        }
    }
}
