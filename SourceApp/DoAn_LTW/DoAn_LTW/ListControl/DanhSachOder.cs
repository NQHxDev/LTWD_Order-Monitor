using DoAn_LTW.ContextDatabase;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn_LTW.ListControl
{
    public partial class DanhSachOder : UserControl
    {
        private FlowLayoutPanel flowPanel;

        private static Dictionary<int, string> listFood = new Dictionary<int, string>();

        public DanhSachOder()
        {
            InitializeComponent();

            Label titleLabel = new Label();
            titleLabel.Font = new Font("Tahoma", 16, FontStyle.Bold);
            titleLabel.ForeColor = Color.White;
            titleLabel.Height = 50;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(titleLabel);

            flowPanel = new FlowLayoutPanel();
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.AutoScroll = true;
            flowPanel.WrapContents = true;
            flowPanel.FlowDirection = FlowDirection.LeftToRight;
            this.Controls.Add(flowPanel);

            WebSocketManager.OnMessageReceived += HandleWebSocketMessage;
        }

        private void AddOrderCard(JToken order)
        {
            Panel orderPanel = new Panel();
            orderPanel.BackColor = Color.White;
            orderPanel.Padding = new Padding(15);
            orderPanel.Margin = new Padding(10, 15, 10, 15);
            orderPanel.Width = 320;
            orderPanel.Height = 400;
            orderPanel.BorderStyle = BorderStyle.None;

            // Tiêu đề
            Label lblTitle = new Label();
            lblTitle.Text = $"Order #{order["orderId"]}";
            lblTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(220, 120, 0);
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 25;
            lblTitle.Padding = new Padding(0, 0, 0, 5);

            // Order Detail
            Label lblCustomer = new Label();
            lblCustomer.Text = $"Khách hàng: {order["customer_name"]}\nSĐT: {order["customer_phone"]}";
            lblCustomer.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            lblCustomer.ForeColor = Color.FromArgb(100, 100, 100);
            lblCustomer.Dock = DockStyle.Top;
            lblCustomer.Height = 45;
            lblCustomer.Padding = new Padding(0, 0, 0, 8);

            // Dòng phân cách
            Panel separator = new Panel();
            separator.Height = 2;
            separator.BackColor = Color.FromArgb(240, 240, 240);
            separator.Dock = DockStyle.Top;

            // Danh sách món ăn
            FlowLayoutPanel itemsPanel = new FlowLayoutPanel();
            itemsPanel.FlowDirection = FlowDirection.TopDown;
            itemsPanel.WrapContents = false;
            itemsPanel.Dock = DockStyle.Fill;
            itemsPanel.AutoScroll = true;

            foreach (var item in order["cart"])
            {
                string foodName = DataCache.GetFoodName((int)item["id"]);
                int qty = (int)item["quantity"];
                int price = (int)item["price"];
                int total = price * qty;

                Panel itemPanel = new Panel();
                itemPanel.Width = 270;
                itemPanel.Height = 25;
                itemPanel.Margin = new Padding(0, 0, 0, 5);

                Label lblItemName = new Label();
                lblItemName.Text = $"• {foodName}";
                lblItemName.ForeColor = Color.FromArgb(60, 60, 60);
                lblItemName.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                lblItemName.AutoSize = true;
                lblItemName.Location = new Point(0, 5);
                itemPanel.Controls.Add(lblItemName);

                Label lblItemQty = new Label();
                lblItemQty.Text = $"x{qty}";
                lblItemQty.ForeColor = Color.FromArgb(60, 60, 60);
                lblItemQty.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lblItemQty.AutoSize = true;
                lblItemQty.Location = new Point(250, 5);
                itemPanel.Controls.Add(lblItemQty);

                itemsPanel.Controls.Add(itemPanel);
            }

            // Note khách hàng
            Label lblNote = new Label();
            lblNote.Text = $"*Note: {order["note"]}";
            lblNote.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            lblNote.ForeColor = Color.FromArgb(100, 100, 100);
            lblNote.Dock = DockStyle.Bottom;
            lblNote.Height = 50;
            lblNote.Padding = new Padding(0, 0, 0, 8);

            // Nút Nhận đơn
            Button btnNhanDon = new Button();
            btnNhanDon.Text = "Nhận Đơn";
            btnNhanDon.Dock = DockStyle.Bottom;
            btnNhanDon.Height = 38;
            btnNhanDon.BackColor = Color.FromArgb(76, 175, 80);
            btnNhanDon.ForeColor = Color.White;
            btnNhanDon.FlatStyle = FlatStyle.Flat;
            btnNhanDon.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnNhanDon.FlatAppearance.BorderSize = 0;

            btnNhanDon.Click += async (s, e) =>
            {
                btnNhanDon.Text = "Đã Nhận";
                btnNhanDon.Enabled = false;
                btnNhanDon.BackColor = Color.FromArgb(120, 120, 120);

                var allIngredients = new List<food_ingredient>();

                foreach (var item in order["cart"])
                {
                    int foodId = (int)item["id"];
                    var ingredients = GetIngredientsByFoodId(foodId);
                    allIngredients.AddRange(ingredients);
                }

                var parentForm = this.FindForm();
                var dangThucHienPanel = FindDangThucHienPanel(parentForm);

                if (dangThucHienPanel != null)
                {
                    dangThucHienPanel.AddOrder(order, allIngredients);
                }

                await Task.Delay(3000);
                flowPanel.Controls.Remove(orderPanel);
            };

            Label spacer = new Label();
            spacer.Dock = DockStyle.Top;
            spacer.Height = 25;
            spacer.Padding = new Padding(0, 0, 0, 5);
            
            orderPanel.Controls.Add(lblNote);
            orderPanel.Controls.Add(btnNhanDon);
            orderPanel.Controls.Add(separator);
            orderPanel.Controls.Add(itemsPanel);
            orderPanel.Controls.Add(separator);
            orderPanel.Controls.Add(lblCustomer);
            orderPanel.Controls.Add(lblTitle);
            orderPanel.Controls.Add(spacer);

            flowPanel.Controls.Add(orderPanel);
        }

        private DangThucHien FindDangThucHienPanel(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is DangThucHien)
                    return (DangThucHien)control;

                var found = FindDangThucHienPanel(control);
                if (found != null)
                    return found;
            }
            return null;
        }

        private List<food_ingredient> GetIngredientsByFoodId(int foodId)
        {
            return DataCache.FoodIngredients.TryGetValue(foodId, out var ingredients)
                ? ingredients
                : new List<food_ingredient>();
        }

        private void HandleWebSocketMessage(string msg)
        {
            try
            {
                var json = JObject.Parse(msg);
                if (json["type"]?.ToString() == "orderFood")
                {
                    var order = json["payload"];
                    this.Invoke((MethodInvoker)(() =>
                    {
                        AddOrderCard(order);
                    }));
                }
            }
            catch (Exception ex)
            {
                Log("Parse error: " + ex.Message);
            }
        }

        private void Log(string message)
        {
            string logPath = Application.StartupPath + "\\ws_log.txt";

            System.IO.File.AppendAllText(logPath, DateTime.Now + " - " + message + Environment.NewLine);
        }
    }
}
