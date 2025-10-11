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
using Microsoft.VisualBasic;

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

            // Panel Button
            Panel buttonsPanel = new Panel();
            buttonsPanel.Dock = DockStyle.Bottom;
            buttonsPanel.Height = 45;
            buttonsPanel.Padding = new Padding(10, 5, 10, 5);

            // Nút Nhận đơn
            Button btnNhanDon = new Button();
            btnNhanDon.Text = "Nhận Đơn";
            btnNhanDon.Width = 130;
            btnNhanDon.Height = 35;
            btnNhanDon.BackColor = Color.FromArgb(76, 175, 80);
            btnNhanDon.ForeColor = Color.White;
            btnNhanDon.FlatStyle = FlatStyle.Flat;
            btnNhanDon.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnNhanDon.FlatAppearance.BorderSize = 0;
            btnNhanDon.Location = new Point(10, 5);

            // Nút Hủy đơn
            Button btnHuyDon = new Button();
            btnHuyDon.Text = "Hủy Đơn";
            btnHuyDon.Width = 130;
            btnHuyDon.Height = 35;
            btnHuyDon.BackColor = Color.FromArgb(240, 80, 80);
            btnHuyDon.ForeColor = Color.White;
            btnHuyDon.FlatStyle = FlatStyle.Flat;
            btnHuyDon.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnHuyDon.FlatAppearance.BorderSize = 0;
            btnHuyDon.Location = new Point(btnNhanDon.Right + 10, 5);

            buttonsPanel.Controls.Add(btnNhanDon);
            buttonsPanel.Controls.Add(btnHuyDon);

            btnNhanDon.Click += async (s, e) =>
            {
                btnNhanDon.Text = "Đã Nhận";
                btnNhanDon.Enabled = false;
                btnNhanDon.BackColor = Color.FromArgb(120, 120, 120);

                int orderId = (int)order["orderId"];
                SendOrderStatus(orderId, "accepted");

                var allIngredients = new List<food_ingredient>();

                foreach (var item in order["cart"])
                {
                    int foodId = (int)item["id"];
                    var ingredients = DataCache.GetIngredientsByFoodId(foodId);
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

            btnHuyDon.Click += (s, e) =>
            {
                int orderId = (int)order["orderId"];

                string reason = Interaction.InputBox(
                    "Nhập lý do hủy đơn:", "Hủy đơn #" + orderId, "");

                if (string.IsNullOrWhiteSpace(reason))
                {
                    MessageBox.Show("Bạn phải nhập lý do hủy đơn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SendOrderStatus(orderId, "cancelled", reason);

                flowPanel.Controls.Remove(orderPanel);
            };


            Label spacer = new Label();
            spacer.Dock = DockStyle.Top;
            spacer.Height = 25;
            spacer.Padding = new Padding(0, 0, 0, 5);

            orderPanel.Controls.Add(lblNote);

            orderPanel.Controls.Add(buttonsPanel);

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

        private void SendOrderStatus(int orderId, string status, string reason = "")
        {
            var data = new
            {
                type = "updateOrderStatus",
                payload = new
                {
                    orderId = orderId,
                    status = status,
                    reason = reason
                }
            };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            WebSocketManager.Send(json);
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
