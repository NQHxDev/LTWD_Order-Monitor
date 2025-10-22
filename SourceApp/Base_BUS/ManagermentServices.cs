using Base_DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_BUS
{
    public class ManagermentServices
    {
        private static ManagermentServices _instance;

        public static ManagermentServices Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ManagermentServices();
                return _instance;
            }
        }

        private readonly OrderRepository _repo;
        private readonly FoodRepository _foodRepo;

        private ManagermentServices()
        {
            _repo = new OrderRepository();
            _foodRepo = new FoodRepository();
        }

        public SalesSummary GetSalesFoods(DateTime start, DateTime end)
        {
            var orders = _repo.GetListOrder()
                .Where(o => o.created_at.HasValue && o.created_at.Value >= start && o.created_at.Value <= end)
                .ToList();

            var total = orders.Sum(o => (decimal?)o.total_price) ?? 0m;

            return new SalesSummary { Total = total, DayStart = start, DayEnd = end };
        }

        public SalesSummary GetSalesToday()
        {
            var today = DateTime.Now.Date;
            return GetSalesFoods(today, today.AddDays(1).AddTicks(-1));
        }

        public SalesSummary GetSalesThisWeek()
        {
            var today = DateTime.Now.Date;
            int diff = (7 + (int)today.DayOfWeek - (int)DayOfWeek.Monday) % 7;
            var startOfWeek = today.AddDays(-diff);
            return GetSalesFoods(startOfWeek, startOfWeek.AddDays(7).AddTicks(-1));
        }

        public SalesSummary GetSalesThisMonth()
        {
            var now = DateTime.Now;
            var start = new DateTime(now.Year, now.Month, 1);
            var end = start.AddMonths(1).AddTicks(-1);
            return GetSalesFoods(start, end);
        }

        public SalesSummary GetSalesThisYear()
        {
            var now = DateTime.Now;
            var start = new DateTime(now.Year, 1, 1);
            var end = start.AddYears(1).AddTicks(-1);
            return GetSalesFoods(start, end);
        }

        public List<(DateTime Date, decimal Total)> GetDailySales(DateTime from, DateTime to)
        {
            var orders = _repo.GetListOrder()
                .Where(o => o.created_at.HasValue && o.created_at.Value >= from && o.created_at.Value <= to)
                .ToList();

            var dailySales = orders
                .GroupBy(o => o.created_at?.Date)
                .Where(g => g.Key.HasValue)
                .Select(g => new { Date = g.Key.Value, Total = g.Sum(x => (decimal?)x.total_price) ?? 0m })
                .OrderBy(x => x.Date)
                .ToList();

            return dailySales.Select(x => (Date: x.Date, Total: x.Total)).ToList();
        }

        public List<BestSellerItem> GetBestSellers(int top = 8, DateTime? from = null, DateTime? to = null)
        {
            var orderDetails = _repo.GetListOrderDetail();
            var orders = _repo.GetListOrder();
            var foods = _foodRepo.GetListFoods();

            if (from.HasValue && to.HasValue)
            {
                var filteredOrderIds = orders
                    .Where(o => o.created_at >= from && o.created_at <= to)
                    .Select(o => o.oder_id)
                    .ToList();

                orderDetails = orderDetails
                    .Where(d => filteredOrderIds.Contains(d.order_id))
                    .ToList();
            }

            var bestSellers = orderDetails
                .GroupBy(d => d.food_id)
                .Select(g => new
                {
                    FoodId = g.Key,
                    Quantity = g.Sum(x => (int?)x.quantity) ?? 0,
                    Revenue = g.Sum(x => (decimal?)(x.quantity * x.price)) ?? 0m
                })
                .Join(foods, g => g.FoodId, f => f.food_id,
                    (g, f) => new BestSellerItem
                    {
                        FoodId = g.FoodId,
                        Name = f.name,
                        Quantity = g.Quantity,
                        TotalMoney = g.Revenue
                    })
                .OrderByDescending(x => x.Quantity)
                .Take(top)
                .ToList();

            return bestSellers;
        }
    }

    public class SalesSummary
    {
        public decimal Total { get; set; }
        public DateTime DayStart { get; set; }
        public DateTime DayEnd { get; set; }
    }

    public class BestSellerItem
    {
        public int FoodId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal TotalMoney { get; set; }
    }
}