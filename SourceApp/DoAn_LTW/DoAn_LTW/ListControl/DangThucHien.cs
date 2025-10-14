using DoAn_LTW.ContextDatabase;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DoAn_LTW.ListControl
{
    public partial class DangThucHien : UserControl
    {
        private FlowLayoutPanel flowPanel;

        public DangThucHien()
        {
            InitializeComponent();

            Label titleLabel = new Label();
            titleLabel.Font = new Font("Tahoma", 16, FontStyle.Bold);
            titleLabel.ForeColor = Color.White;
            titleLabel.Height = 50;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(titleLabel);

            // Khởi tạo FlowLayoutPanel
            flowPanel = new FlowLayoutPanel();
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.AutoScroll = true;
            flowPanel.WrapContents = true;
            flowPanel.FlowDirection = FlowDirection.LeftToRight;
            this.Controls.Add(flowPanel);
        }

        public void AddOrder(JToken order, List<food_ingredient> ingredients)
        {
            this.Invoke((MethodInvoker)(() =>
            {
                AddOrderCard(order, ingredients);
            }));
        }

        public void LoadOrders()
        {
            flowPanel.Controls.Clear();

            var orders = DataCache.GetOrdersByStatus(1);

            foreach (var order in orders)
            {
                var ingredients = new List<food_ingredient>();
                foreach (var item in order["cart"])
                {
                    int foodId = (int)item["id"];
                    ingredients.AddRange(DataCache.GetIngredientsByFoodId(foodId));
                }

                AddOrderCard(order, ingredients);
            }
        }

        private void AddOrderCard(JToken order, List<food_ingredient> ingredients)
        {
            Panel orderPanel = new Panel();
            orderPanel.BackColor = Color.White;
            orderPanel.Padding = new Padding(15);
            orderPanel.Margin = new Padding(10, 15, 10, 15);
            orderPanel.Width = 330;
            orderPanel.Height = 650;
            orderPanel.BorderStyle = BorderStyle.None;
            orderPanel.AutoScroll = true;

            // Tiêu đề
            Label lblTitle = new Label();
            lblTitle.Text = $"Order #{order["orderId"]} - Đang thực hiện";
            lblTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(220, 120, 0);
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 25;
            lblTitle.Padding = new Padding(0, 0, 0, 5);

            // Tên khách hàng
            Label lblCustomer = new Label();
            lblCustomer.Text = $"Tên khách hàng: {order["customer_name"]}";
            lblCustomer.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            lblCustomer.ForeColor = Color.FromArgb(100, 100, 100);
            lblCustomer.Dock = DockStyle.Top;
            lblCustomer.Height = 25;
            lblCustomer.Padding = new Padding(0, 0, 0, 8);

            // Dòng phân cách
            Panel separator = new Panel();
            separator.Height = 1;
            separator.BackColor = Color.FromArgb(240, 240, 240);
            separator.Dock = DockStyle.Top;

            FlowLayoutPanel orderItemsPanel = new FlowLayoutPanel();
            orderItemsPanel.FlowDirection = FlowDirection.TopDown;
            orderItemsPanel.WrapContents = false;
            orderItemsPanel.Dock = DockStyle.Fill;
            orderItemsPanel.AutoScroll = true;

            foreach (var item in order["cart"])
            {
                int foodId = (int)item["id"];
                string foodName = DataCache.GetFoodName((int)item["id"]);
                int qty = (int)item["quantity"];

                // Food
                Panel itemPanel = new Panel();
                itemPanel.Width = 300;
                itemPanel.Height = 25;
                itemPanel.Margin = new Padding(0, 5, 0, 3);

                Label lblItemName = new Label();
                lblItemName.Text = $"• {foodName}";
                lblItemName.ForeColor = Color.FromArgb(60, 60, 60);
                lblItemName.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                lblItemName.AutoSize = true;
                lblItemName.Location = new Point(0, 3);
                itemPanel.Controls.Add(lblItemName);

                Label lblItemQty = new Label();
                lblItemQty.Text = $"x{qty}";
                lblItemQty.ForeColor = Color.FromArgb(60, 60, 60);
                lblItemQty.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lblItemQty.AutoSize = true;
                lblItemQty.Location = new Point(250, 3);
                itemPanel.Controls.Add(lblItemQty);

                orderItemsPanel.Controls.Add(itemPanel);

                // Nguyên liệu
                var foodIngredients = ingredients
                    .Where(i => i.food_id == foodId)
                    .ToList();

                if (foodIngredients.Any())
                {
                    FlowLayoutPanel ingrPanel = new FlowLayoutPanel();
                    ingrPanel.FlowDirection = FlowDirection.TopDown;
                    ingrPanel.WrapContents = false;
                    ingrPanel.AutoSize = true;
                    ingrPanel.Padding = new Padding(15, 0, 0, 5);

                    foreach (var ingr in foodIngredients)
                    {
                        string unitName = DataCache.GetUnitName(ingr.item?.unit_id);
                        string displayText = $"{ingr.quantity} {unitName}";

                        Label lblIngr = new Label();
                        lblIngr.Text = $"- {ingr.item?.name ?? "Nguyên liệu"}: {displayText}";
                        lblIngr.ForeColor = Color.FromArgb(100, 100, 100);
                        lblIngr.Font = new Font("Segoe UI", 8.5f, FontStyle.Italic);
                        lblIngr.AutoSize = true;
                        ingrPanel.Controls.Add(lblIngr);
                    }

                    orderItemsPanel.Controls.Add(ingrPanel);
                }
            }

            // Note khách hàng
            Label lblNote = new Label();
            lblNote.Text = $"*Note: {order["note"]}";
            lblNote.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            lblNote.ForeColor = Color.FromArgb(100, 100, 100);
            lblNote.Dock = DockStyle.Bottom;
            lblNote.Height = 50;
            lblNote.Padding = new Padding(0, 0, 0, 8);

            Button btnHoanThanh = new Button();
            btnHoanThanh.Text = "Hoàn Thành";
            btnHoanThanh.Dock = DockStyle.Bottom;
            btnHoanThanh.Height = 35;
            btnHoanThanh.BackColor = Color.FromArgb(33, 150, 243);
            btnHoanThanh.ForeColor = Color.White;
            btnHoanThanh.FlatStyle = FlatStyle.Flat;
            btnHoanThanh.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnHoanThanh.FlatAppearance.BorderSize = 0;

            btnHoanThanh.Click += (s, e) =>
            {
                int orderId = (int)order["orderId"];
                try
                {
                    using (var context = new OrderMonitor())
                    {
                        var entity = context.list_order.FirstOrDefault(o => o.oder_id == orderId);
                        if (entity != null)
                        {
                            entity.status = 2;
                            entity.updated_at = DateTime.Now;
                            context.SaveChanges();
                        }
                    }

                    WebSocketManager.SendOrderStatus(orderId, "completed");
                    flowPanel.Controls.Remove(orderPanel);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật trạng thái: " + ex.Message);
                }
            };

            Label spacer = new Label();
            spacer.Dock = DockStyle.Top;
            spacer.Height = 25;
            spacer.Padding = new Padding(0, 0, 0, 5);

            orderPanel.Controls.Add(lblNote);
            orderPanel.Controls.Add(btnHoanThanh);
            orderPanel.Controls.Add(orderItemsPanel);
            orderPanel.Controls.Add(separator);
            orderPanel.Controls.Add(lblCustomer);
            orderPanel.Controls.Add(lblTitle);
            orderPanel.Controls.Add(spacer);

            flowPanel.Controls.Add(orderPanel);
        }
    }
}