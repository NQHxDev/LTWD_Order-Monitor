using Base_BUS;
using Base_DAL.ContextDatabase;
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
    public partial class OrderReceived : UserControl
    {
        private FoodServices foodServices = new FoodServices();

        private FlowLayoutPanel flowPanel;

        public event Action BackButtonClicked;

        public OrderReceived()
        {
            InitializeComponent();

            // Main container
            Panel mainContainer = new Panel();
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.BackColor = Color.FromArgb(60, 60, 60);
            mainContainer.Padding = new Padding(10);
            this.Controls.Add(mainContainer);

            InitializePanel(mainContainer);

            LoadReceivedOrders();
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

            // Flow panel cho các order card
            flowPanel = new FlowLayoutPanel();
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.AutoScroll = true;
            flowPanel.WrapContents = true;
            flowPanel.FlowDirection = FlowDirection.LeftToRight;
            flowPanel.Padding = new Padding(10);

            Label spacer = new Label();
            spacer.Dock = DockStyle.Top;
            spacer.Height = 45;
            spacer.Padding = new Padding(0, 0, 0, 5);

            mainPanel.Controls.Add(flowPanel);
            mainPanel.Controls.Add(buttonPanel);
            mainPanel.Controls.Add(spacer);

            container.Controls.Add(mainPanel);
        }

        private void LoadReceivedOrders()
        {
            try
            {
                var completedOrders = foodServices.GetOrdersByStatus(1);

                foreach (var order in completedOrders)
                {
                    AddReceivedOrderCard(order);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải order đã nhận: " + ex.Message);
            }
        }

        private void AddReceivedOrderCard(JToken order)
        {
            Panel orderPanel = new Panel();
            orderPanel.BackColor = Color.White;
            orderPanel.Padding = new Padding(12);
            orderPanel.Margin = new Padding(8, 10, 8, 10);
            orderPanel.Width = 280;
            orderPanel.Height = 200;
            orderPanel.BorderStyle = BorderStyle.None;

            // Tiêu đề Order ID
            Label lblOrderId = new Label();
            lblOrderId.Text = $"Order #{order["orderId"]}";
            lblOrderId.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblOrderId.ForeColor = Color.FromArgb(0, 120, 215);
            lblOrderId.Dock = DockStyle.Top;
            lblOrderId.Height = 25;
            lblOrderId.Padding = new Padding(0, 0, 0, 5);

            // Thông tin khách hàng
            Label lblCustomer = new Label();
            lblCustomer.Text = $"Khách hàng: {order["customer_name"]}\nSĐT: {order["customer_phone"]}";
            lblCustomer.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblCustomer.ForeColor = Color.FromArgb(80, 80, 80);
            lblCustomer.Dock = DockStyle.Top;
            lblCustomer.Height = 35;
            lblCustomer.Padding = new Padding(0, 0, 0, 5);

            // Tổng số món
            int totalItems = order["cart"]?.Count() ?? 0;
            Label lblItemsCount = new Label();
            lblItemsCount.Text = $"Số món: {totalItems}";
            lblItemsCount.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblItemsCount.ForeColor = Color.FromArgb(80, 80, 80);
            lblItemsCount.Dock = DockStyle.Top;
            lblItemsCount.Height = 20;
            lblItemsCount.Padding = new Padding(0, 0, 0, 5);

            // Tổng tiền
            Label lblTotalPrice = new Label();
            lblTotalPrice.Text = $"Tổng tiền: {order["total_price"]:N0}đ";
            lblTotalPrice.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTotalPrice.ForeColor = Color.FromArgb(220, 120, 0);
            lblTotalPrice.Dock = DockStyle.Top;
            lblTotalPrice.Height = 25;
            lblTotalPrice.Padding = new Padding(0, 0, 0, 5);

            // Note
            Label lblNote = new Label();
            lblNote.Text = $"*Note: {order["note"]}";
            lblNote.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            lblNote.ForeColor = Color.FromArgb(100, 100, 100);
            lblNote.Dock = DockStyle.Top;
            lblNote.Height = 50;
            lblNote.Padding = new Padding(0, 0, 0, 8);

            // Thời gian tạo
            if (DateTime.TryParse(order["created_at"]?.ToString(), out DateTime createdTime))
            {
                Label lblTime = new Label();
                lblTime.Text = $"Thời gian: {createdTime:HH:mm dd/MM}";
                lblTime.Font = new Font("Segoe UI", 8, FontStyle.Italic);
                lblTime.ForeColor = Color.FromArgb(120, 120, 120);
                lblTime.Dock = DockStyle.Bottom;
                lblTime.Height = 18;
                lblTime.TextAlign = ContentAlignment.MiddleRight;
                orderPanel.Controls.Add(lblTime);
            }

            Panel separator = new Panel();
            separator.Height = 1;
            separator.BackColor = Color.FromArgb(230, 230, 230);
            separator.Dock = DockStyle.Bottom;
            separator.Margin = new Padding(0, 5, 0, 5);

            orderPanel.Controls.Add(lblNote);
            orderPanel.Controls.Add(lblTotalPrice);
            orderPanel.Controls.Add(lblItemsCount);
            orderPanel.Controls.Add(lblCustomer);
            orderPanel.Controls.Add(lblOrderId);
            orderPanel.Controls.Add(separator);

            flowPanel.Controls.Add(orderPanel);
        }
    }
}