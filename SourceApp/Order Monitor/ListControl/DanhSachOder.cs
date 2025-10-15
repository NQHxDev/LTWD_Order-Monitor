using Order_Monitor.ContextDatabase;
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
using Org.BouncyCastle.Asn1.X509;

namespace Order_Monitor.ListControl
{
    public partial class DanhSachOder : UserControl
    {
        private Panel mainContainer;
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

            // Main container
            mainContainer = new Panel();
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.BackColor = Color.FromArgb(60, 60, 60);
            mainContainer.Padding = new Padding(10);
            this.Controls.Add(mainContainer);

            InitializeOrderCard(mainContainer);

            WebSocketManager.OnMessageReceived += HandleWebSocketMessage;

            LoadPendingOrders();
        }

        private void InitializeOrderCard(Panel container)
        {
            Panel orderListPanel = new Panel();
            orderListPanel.Dock = DockStyle.Fill;
            orderListPanel.Name = "pnlOrderList";

            Panel buttonPanel = new Panel();
            buttonPanel.Height = 50;
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.BackColor = Color.FromArgb(60, 60, 60);

            Button btnShowReceived = new Button();
            btnShowReceived.Text = "Xem Order Đã Nhận";
            btnShowReceived.Size = new Size(120, 35);
            btnShowReceived.Location = new Point(10, 10);
            btnShowReceived.BackColor = Color.FromArgb(0, 120, 215);
            btnShowReceived.ForeColor = Color.White;
            btnShowReceived.FlatStyle = FlatStyle.Flat;
            btnShowReceived.Margin = new Padding(10, 10, 10, 10);
            btnShowReceived.Click += (s, e) => ShowReceivedOrders();

            Button btnShowCompleted = new Button();
            btnShowCompleted.Text = "Xem Order Đã Hoàn Thành";
            btnShowCompleted.Size = new Size(150, 35);
            btnShowCompleted.Location = new Point(140, 10);
            btnShowCompleted.BackColor = Color.FromArgb(0, 120, 215);
            btnShowCompleted.ForeColor = Color.White;
            btnShowCompleted.FlatStyle = FlatStyle.Flat;
            btnShowCompleted.Margin = new Padding(10, 10, 10, 10);
            btnShowCompleted.Click += (s, e) => ShowCompletedOrders();

            buttonPanel.Controls.AddRange(new Control[] { btnShowReceived, btnShowCompleted });

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

            orderListPanel.Controls.Add(flowPanel);
            orderListPanel.Controls.Add(buttonPanel);
            orderListPanel.Controls.Add(spacer);

            container.Controls.Add(orderListPanel);
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
                //int total = price * qty;

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

            btnNhanDon.Click += (s, e) => HandleAcceptOrder(order, orderPanel, btnNhanDon);

            btnHuyDon.Click += (s, e) => HandleCancelOrder(order, orderPanel);

            orderPanel.Controls.Add(lblNote);

            orderPanel.Controls.Add(buttonsPanel);

            orderPanel.Controls.Add(separator);
            orderPanel.Controls.Add(itemsPanel);
            orderPanel.Controls.Add(separator);
            orderPanel.Controls.Add(lblCustomer);
            orderPanel.Controls.Add(lblTitle);

            flowPanel.Controls.Add(orderPanel);
        }

        private async void HandleAcceptOrder(JToken order, Panel orderPanel, Button btnNhanDon)
        {
            btnNhanDon.Text = "Đã Nhận";
            btnNhanDon.Enabled = false;
            btnNhanDon.BackColor = Color.FromArgb(120, 120, 120);

            int orderId = (int)order["orderId"];
            WebSocketManager.SendOrderStatus(orderId, "accepted");

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

            UpdateStatusOrder(orderId, 1);

            await Task.Delay(2000);
            flowPanel.Controls.Remove(orderPanel);
        }

        private string staffFeedback = string.Empty;

        private void HandleCancelOrder(JToken order, Panel orderPanel)
        {
            int orderId = (int)order["orderId"];

            using (var dialog = new Form()
            {
                Text = $"Hủy đơn #{orderId}",
                Size = new Size(400, 200),
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            })
            {
                var txtReason = new TextBox()
                {
                    Location = new Point(20, 40),
                    Size = new Size(340, 100),
                    Multiline = true,
                    Font = new Font("Segoe UI", 9),
                    ScrollBars = ScrollBars.Vertical
                };

                var btnOK = new Button()
                {
                    Text = "Xác nhận",
                    Location = new Point(300, 5),
                    Size = new Size(75, 30),
                    BackColor = Color.FromArgb(0, 123, 255),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };

                var btnCancel = new Button()
                {
                    Text = "Hủy",
                    Location = new Point(285, 120),
                    Size = new Size(75, 30),
                    BackColor = Color.FromArgb(108, 117, 125),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };

                btnOK.Click += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txtReason.Text))
                    {
                        MessageBox.Show("Vui lòng nhập lý do hủy đơn!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    dialog.DialogResult = DialogResult.OK;
                };

                btnCancel.Click += (s, e) => { dialog.DialogResult = DialogResult.Cancel; };

                dialog.Controls.AddRange(new Control[] {
            new Label() { Text = "Lý do hủy đơn:", Location = new Point(20, 20), Font = new Font("Segoe UI", 10) },
            txtReason, btnOK, btnCancel
        });

                dialog.AcceptButton = btnOK;
                dialog.CancelButton = btnCancel;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    staffFeedback = txtReason.Text.Trim();
                    UpdateStatusOrder(orderId, -1);
                    WebSocketManager.SendOrderStatus(orderId, "cancelled", txtReason.Text.Trim());
                    flowPanel.Controls.Remove(orderPanel);
                }
            }
        }

        private void UpdateStatusOrder(int orderId, short status)
        {
            using (var context = new OrderMonitor())
            {
                var entity = context.list_order.FirstOrDefault(o => o.oder_id == orderId);
                if (entity != null)
                {
                    entity.status = status;
                    entity.updated_at = DateTime.Now;
                    if (status == -1 && !string.IsNullOrWhiteSpace(staffFeedback))
                    {
                        entity.staff_feedback = staffFeedback;
                        staffFeedback = string.Empty;
                    }
                    context.SaveChanges();
                }
            }
        }

        private void ShowReceivedOrders()
        {
            mainContainer.Visible = false;

            OrderReceived receivedPanel = new OrderReceived();
            receivedPanel.Dock = DockStyle.Fill;
            receivedPanel.BackButtonClicked += () =>
            {
                this.Controls.Remove(receivedPanel);
                mainContainer.Visible = true;
            };
            this.Controls.Add(receivedPanel);
        }

        private void ShowCompletedOrders()
        {
            mainContainer.Visible = false;

            OrderComplete completedPanel = new OrderComplete();
            completedPanel.Dock = DockStyle.Fill;
            completedPanel.BackButtonClicked += () =>
            {
                this.Controls.Remove(completedPanel);
                mainContainer.Visible = true;
            };
            this.Controls.Add(completedPanel);
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

        private void LoadPendingOrders()
        {
            try
            {
                var pendingOrders = DataCache.GetOrdersByStatus(0);

                foreach (var orderJson in pendingOrders)
                {
                    AddOrderCard(orderJson);
                }
            }
            catch (Exception ex)
            {
                Log("LoadPendingOrders error: " + ex.Message);
            }
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
