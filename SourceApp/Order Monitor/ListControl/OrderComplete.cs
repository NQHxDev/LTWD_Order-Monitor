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

namespace Order_Monitor.ListControl
{
    public partial class OrderComplete : UserControl
    {
        private FlowLayoutPanel flowPanel;

        public event Action BackButtonClicked;

        public OrderComplete()
        {
            InitializeComponent();

            // Main container
            Panel mainContainer = new Panel();
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.BackColor = Color.FromArgb(60, 60, 60);
            mainContainer.Padding = new Padding(10);
            this.Controls.Add(mainContainer);

            InitializePanel(mainContainer);

            LoadCompletedOrders();
        }

        private void InitializePanel(Panel container)
        {
            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.FromArgb(60, 60, 60);

            Panel buttonPanel = new Panel();
            buttonPanel.Height = 50;
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.BackColor = Color.FromArgb(60, 60, 60);

            // Nút quay lại
            Button btnBack = new Button();
            btnBack.Text = "Quay Lại";
            btnBack.Size = new Size(120, 35);
            btnBack.Location = new Point(0, 10);
            btnBack.BackColor = Color.FromArgb(0, 122, 204);
            btnBack.ForeColor = Color.White;
            btnBack.FlatStyle = FlatStyle.Flat;

            buttonPanel.Controls.AddRange(new Control[] { btnBack });
            btnBack.Click += (s, e) => BackButtonClicked?.Invoke();

            flowPanel = new FlowLayoutPanel();
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.AutoScroll = true;
            flowPanel.WrapContents = true;
            flowPanel.FlowDirection = FlowDirection.LeftToRight;
            flowPanel.Padding = new Padding(10);
            flowPanel.AutoSize = false;
            flowPanel.AutoSizeMode = AutoSizeMode.GrowOnly;

            Label spacer = new Label();
            spacer.Dock = DockStyle.Top;
            spacer.Height = 45;
            spacer.Padding = new Padding(0, 0, 0, 5);

            mainPanel.Controls.Add(flowPanel);
            mainPanel.Controls.Add(buttonPanel);
            mainPanel.Controls.Add(spacer);

            container.Controls.Add(mainPanel);
        }

        private void LoadCompletedOrders()
        {
            try
            {
                var completedOrders = DataCache.GetOrdersByStatus(2);

                foreach (var order in completedOrders)
                {
                    AddCompletedOrderCard(order);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải order đã nhận: " + ex.Message);
            }
        }

        private void AddCompletedOrderCard(JToken order)
        {
            Panel orderPanel = new Panel();
            orderPanel.BackColor = Color.FromArgb(240, 255, 240);
            orderPanel.Padding = new Padding(12);
            orderPanel.Margin = new Padding(8, 10, 8, 10);
            orderPanel.Width = 280;
            orderPanel.Height = 400;
            orderPanel.BorderStyle = BorderStyle.None;

            // Tiêu đề Order ID
            Label lblOrderId = new Label();
            lblOrderId.Text = $"Order #{order["orderId"]}";
            lblOrderId.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblOrderId.ForeColor = Color.FromArgb(0, 120, 80);
            lblOrderId.Dock = DockStyle.Top;
            lblOrderId.Height = 25;
            lblOrderId.Padding = new Padding(0, 0, 0, 5);

            // Thông tin khách hàng
            Label lblCustomer = new Label();
            lblCustomer.Text = $"Khách hàng: {order["customer_name"]}\nSĐT: {order["customer_phone"]}";
            lblCustomer.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblCustomer.ForeColor = Color.FromArgb(60, 80, 60);
            lblCustomer.Dock = DockStyle.Top;
            lblCustomer.Height = 35;
            lblCustomer.Padding = new Padding(0, 0, 0, 5);

            // Danh sách món ăn
            FlowLayoutPanel itemsPanel = new FlowLayoutPanel();
            itemsPanel.FlowDirection = FlowDirection.TopDown;
            itemsPanel.Size = new Size(250, 220);
            itemsPanel.WrapContents = false;
            itemsPanel.Dock = DockStyle.Top;

            foreach (var item in order["cart"])
            {
                int foodId = item["id"] != null ? (int)item["id"] : (int)item["food_id"];
                string foodName = item["name"]?.ToString() ?? item["food_name"]?.ToString() ?? DataCache.GetFoodName(foodId);
                int qty = (int)item["quantity"];
                int price = (int)item["price"];
                int total = price * qty;

                Panel itemPanel = new Panel();
                itemPanel.Width = 230;
                itemPanel.Height = 25;
                itemPanel.Margin = new Padding(0, 0, 0, 5);

                Label lblItemName = new Label();
                lblItemName.Text = $"• {foodName}";
                lblItemName.ForeColor = Color.FromArgb(50, 50, 50);
                lblItemName.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                lblItemName.AutoSize = true;
                lblItemName.Location = new Point(0, 5);
                itemPanel.Controls.Add(lblItemName);

                Label lblItemQty = new Label();
                lblItemQty.Text = $"x{qty}";
                lblItemQty.ForeColor = Color.FromArgb(50, 50, 50);
                lblItemQty.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lblItemQty.AutoSize = true;
                lblItemQty.Location = new Point(200, 5);
                itemPanel.Controls.Add(lblItemQty);

                itemsPanel.Controls.Add(itemPanel);
            }

            // Tổng tiền
            Label lblTotalPrice = new Label();
            lblTotalPrice.Text = $"Tổng tiền: {order["total_price"]:N0}đ";
            lblTotalPrice.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTotalPrice.ForeColor = Color.FromArgb(0, 160, 100);
            lblTotalPrice.Dock = DockStyle.Top;
            lblTotalPrice.Height = 25;
            lblTotalPrice.Padding = new Padding(0, 0, 0, 5);

            // Note
            Label lblNote = new Label();
            lblNote.Text = $"*Note: {order["note"]}";
            lblNote.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            lblNote.ForeColor = Color.FromArgb(80, 100, 80);
            lblNote.Dock = DockStyle.Top;
            lblNote.Height = 50;
            lblNote.Padding = new Padding(0, 0, 0, 8);

            // Thời gian hoàn thành
            if (DateTime.TryParse(order["updated_at"]?.ToString(), out DateTime createdTime))
            {
                Label lblTime = new Label();
                lblTime.Text = $"Thời gian: {createdTime:HH:mm dd/MM}";
                lblTime.Font = new Font("Segoe UI", 8, FontStyle.Italic);
                lblTime.ForeColor = Color.FromArgb(100, 140, 100);
                lblTime.Dock = DockStyle.Bottom;
                lblTime.Height = 18;
                lblTime.TextAlign = ContentAlignment.MiddleRight;
                orderPanel.Controls.Add(lblTime);
            }

            Panel separatorBottom = new Panel();
            separatorBottom.Height = 1;
            separatorBottom.BackColor = Color.FromArgb(179, 177, 177);
            separatorBottom.Dock = DockStyle.Bottom;
            separatorBottom.Margin = new Padding(0, 5, 0, 5);

            Panel separatorTop = new Panel();
            separatorTop.Height = 1;
            separatorTop.BackColor = Color.FromArgb(179, 177, 177);
            separatorTop.Dock = DockStyle.Top;

            orderPanel.Controls.Add(lblNote);
            orderPanel.Controls.Add(lblTotalPrice);
            orderPanel.Controls.Add(itemsPanel);
            orderPanel.Controls.Add(separatorTop);
            orderPanel.Controls.Add(lblCustomer);
            orderPanel.Controls.Add(lblOrderId);
            orderPanel.Controls.Add(separatorBottom);

            flowPanel.Controls.Add(orderPanel);
        }
    }
}
