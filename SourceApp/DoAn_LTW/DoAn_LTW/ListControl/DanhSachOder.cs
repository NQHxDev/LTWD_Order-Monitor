using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            this.BackColor = Color.FromArgb(87, 86, 79);
            Label titleLabel = new Label();
            titleLabel.Text = "Danh sách Order";
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

            loadFoodList();

            // Subcribe WebSocket event
            WebSocketManager.OnMessageReceived += HandleWebSocketMessage;
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

        private void AddOrderCard(JToken order)
        {
            Panel orderPanel = new Panel();
            orderPanel.BackColor = Color.White;
            orderPanel.Padding = new Padding(15);
            orderPanel.Margin = new Padding(10, 15, 10, 15);
            orderPanel.Width = 320;
            orderPanel.Height = 400;
            orderPanel.BorderStyle = BorderStyle.None;

            // Tiêu đề (Order ID)
            Label lblTitle = new Label();
            lblTitle.Text = $"ORDER #{order["orderId"]}";
            lblTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(45, 45, 45);
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 25;
            lblTitle.Padding = new Padding(0, 0, 0, 5);

            // Order Detail
            Label lblCustomer = new Label();
            lblCustomer.Text = $"Order ID: {order["orderId"]} ✧•✧ {order["customer_name"]}";
            lblCustomer.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            lblCustomer.ForeColor = Color.FromArgb(100, 100, 100);
            lblCustomer.Dock = DockStyle.Top;
            lblCustomer.Height = 28;
            lblCustomer.Padding = new Padding(0, 0, 0, 8);

            // Dòng phân cách
            Panel separator = new Panel();
            separator.Height = 1;
            separator.BackColor = Color.FromArgb(240, 240, 240);
            separator.Dock = DockStyle.Top;

            // Panel chứa danh sách món ăn
            FlowLayoutPanel itemsPanel = new FlowLayoutPanel();
            itemsPanel.FlowDirection = FlowDirection.TopDown;
            itemsPanel.WrapContents = false;
            itemsPanel.Dock = DockStyle.Fill;
            itemsPanel.AutoScroll = true;

            foreach (var item in order["cart"])
            {
                string foodName = GetFoodNameById((int)item["id"]);
                int qty = (int)item["quantity"];
                int price = (int)item["price"];
                int total = price * qty;

                Panel itemPanel = new Panel();
                itemPanel.Width = 270;
                itemPanel.Height = 25;
                itemPanel.Margin = new Padding(0, 0, 0, 5);

                Label lblItemName = new Label();
                lblItemName.Text = $"{foodName}";
                lblItemName.ForeColor = Color.FromArgb(60, 60, 60);
                lblItemName.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                lblItemName.AutoSize = true;
                lblItemName.Location = new Point(0, 5);
                itemPanel.Controls.Add(lblItemName);

                Label lblItemPrice = new Label();
                lblItemPrice.Text = $"SL: {qty}";
                lblItemPrice.ForeColor = Color.FromArgb(60, 60, 60);
                lblItemPrice.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                lblItemPrice.AutoSize = true;
                lblItemPrice.Location = new Point(200, 5);
                itemPanel.Controls.Add(lblItemPrice);

                itemsPanel.Controls.Add(itemPanel);
            }

            // Dòng phân cách dưới cùng
            Panel separatorBottom = new Panel();
            separatorBottom.Height = 1;
            separatorBottom.BackColor = Color.FromArgb(240, 240, 240);
            separatorBottom.Dock = DockStyle.Bottom;

            // Nút Nhận đơn
            Button btnNhanDon = new Button();
            btnNhanDon.Text = "NHẬN ĐƠN";
            btnNhanDon.Dock = DockStyle.Bottom;
            btnNhanDon.Height = 38;
            btnNhanDon.BackColor = Color.FromArgb(76, 175, 80);
            btnNhanDon.ForeColor = Color.White;
            btnNhanDon.FlatStyle = FlatStyle.Flat;
            btnNhanDon.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnNhanDon.FlatAppearance.BorderSize = 0;

            btnNhanDon.Click += (s, e) =>
            {
                btnNhanDon.Text = "ĐÃ NHẬN";
                btnNhanDon.Enabled = false;
                btnNhanDon.BackColor = Color.FromArgb(120, 120, 120);
            };

            orderPanel.Controls.Add(btnNhanDon);
            orderPanel.Controls.Add(separatorBottom);
            orderPanel.Controls.Add(itemsPanel);
            orderPanel.Controls.Add(separator);
            orderPanel.Controls.Add(lblCustomer);
            orderPanel.Controls.Add(lblTitle);

            // Thêm panel vào flowPanel
            flowPanel.Controls.Add(orderPanel);
        }

        private void Log(string message)
        {
            string logPath = Application.StartupPath + "\\ws_log.txt";
            System.IO.File.AppendAllText(logPath, DateTime.Now + " - " + message + Environment.NewLine);
        }

        private void loadFoodList()
        {
            try
            {
                ConnectDB connection = new ConnectDB();
                listFood = connection.loadFoodList();
            }
            catch (Exception ex)
            {
                Log("LoadFoodList error: " + ex.Message);
            }
        }

        private string GetFoodNameById(int id)
        {
            if (listFood.ContainsKey(id))
                return listFood[id];
            return $"Món ID: {id}";
        }
    }
}
