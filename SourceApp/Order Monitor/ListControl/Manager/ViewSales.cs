using Base_BUS;
using Base_DAL.ContextDatabase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Order_Monitor.ListControl.Manager
{
    public partial class ViewSales : UserControl
    {
        private Panel mainContainer;
        private Panel flowPanel;

        public event Action BackButtonClicked;

        private int leader_ID;

        public ViewSales(int leaderID)
        {
            InitializeComponent();

            // Main Container
            mainContainer = new Panel();
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.BackColor = Color.FromArgb(60, 60, 60);
            mainContainer.Padding = new Padding(10);
            this.Controls.Add(mainContainer);

            leader_ID = leaderID;

            InitializePanel();
        }

        private void InitializePanel()
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

            flowPanel = new Panel();
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.AutoScroll = true;
            flowPanel.BackColor = Color.FromArgb(45, 45, 45);

            Label spacer = new Label();
            spacer.Dock = DockStyle.Top;
            spacer.Height = 45;
            spacer.Padding = new Padding(0, 0, 0, 5);

            mainPanel.Controls.Add(flowPanel);
            mainPanel.Controls.Add(buttonPanel);
            mainPanel.Controls.Add(spacer);

            mainContainer.Controls.Add(mainPanel);

            LoadRevenueOrders();
        }

        private void LoadRevenueOrders()
        {
            flowPanel.Controls.Clear();

            SplitContainer split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterWidth = 8,
                BackColor = Color.FromArgb(45, 45, 45),
                Panel1MinSize = 300
            };

            split.HandleCreated += (s, e) =>
            {
                split.SplitterDistance = split.Width / 3;
            };

            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(50, 50, 50),
                Padding = new Padding(10)
            };

            SplitContainer leftSplit = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 31,
                SplitterWidth = 6,
                BackColor = Color.FromArgb(45, 45, 45),
                Panel1MinSize = 31
            };

            Panel statPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(55, 55, 55)
            };

            FlowLayoutPanel statCards = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true
            };

            var sHour = ManagermentServices.Instance.GetSalesOneHour();
            var sMonth = ManagermentServices.Instance.GetSalesThisMonth();
            var sWeek = ManagermentServices.Instance.GetSalesThisWeek();
            var sDay = ManagermentServices.Instance.GetSalesToday();

            statCards.Controls.Add(CreateStatCard("Doanh thu 1 giờ", sHour.Total));
            statCards.Controls.Add(CreateStatCard("Doanh thu hôm nay", sDay.Total));
            statCards.Controls.Add(CreateStatCard("Doanh thu tuần", sWeek.Total));
            statCards.Controls.Add(CreateStatCard("Doanh thu tháng", sMonth.Total));

            statPanel.Controls.Add(statCards);

            TableLayoutPanel orderListPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(45, 45, 45),
                Padding = new Padding(10),
                ColumnCount = 3,
                RowCount = 0,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };

            orderListPanel.ColumnStyles.Clear();
            for (int i = 0; i < 3; i++)
            {
                orderListPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            }

            var orders = OrderServices.Instance.GetListOrder();

            foreach (var order in orders)
            {
                orderListPanel.Controls.Add(CreateOrderCard(order, rightPanel));
            }

            leftSplit.Panel1.Controls.Add(statPanel);
            leftSplit.Panel2.Controls.Add(orderListPanel);

            Label placeholder = new Label
            {
                Text = "Chọn một đơn hàng để xem chi tiết",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 12, FontStyle.Italic)
            };
            rightPanel.Controls.Add(placeholder);

            split.Panel1.Controls.Add(leftSplit);
            split.Panel2.Controls.Add(rightPanel);

            flowPanel.Controls.Add(split);
        }

        private Panel CreateStatCard(string title, decimal revenue)
        {
            Panel card = new Panel
            {
                Width = 180,
                Height = 100,
                Margin = new Padding(10),
                BackColor = Color.FromArgb(70, 70, 70)
            };

            Label lblTitle = new Label
            {
                Text = title,
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblTotalRevenue = new Label
            {
                Text = $"{revenue:N0} VNĐ",
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            card.Controls.Add(lblTotalRevenue);
            card.Controls.Add(lblTitle);
            return card;
        }

        private Panel CreateOrderCard(list_order order, Panel rightPanel)
        {
            Panel card = new Panel()
            {
                Width = 260,
                Height = 280,
                BackColor = Color.FromArgb(45, 45, 45),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10),
                Padding = new Padding(10),
                Cursor = Cursors.Hand
            };

            Label lblName = new Label()
            {
                Text = $"Mã đơn Đặt hàng: #{order.oder_id}",
                Font = new Font("Tahoma", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Height = 30,
                Dock = DockStyle.Top
            };

            Label lblSatus = new Label()
            {
                Text = GetStatusText(order.status),
                Font = new Font("Tahoma", 10, FontStyle.Bold),
                ForeColor = GetStatusColor(order.status),
                AutoSize = false,
                Height = 30,
                Dock = DockStyle.Top
            };

            Label lblDetail = new Label()
            {
                Dock = DockStyle.Fill,
                ForeColor = Color.Gainsboro,
                Font = new Font("Tahoma", 10),
                Padding = new Padding(0, 5, 0, 5),
                Text =
                $"Tên khách hàng: {order.customer_name}\n\n" +
                $"Số lượng Món: {order.count_food}\n" +
                $"Tổng hóa đơn: {order.total_price:N0} VNĐ\n\n" +
                $"Ngày đặt đơn hàng: {order.created_at?.ToString("dd/MM/yyyy") ?? ""}"
            };

            Button btnDetail = new Button()
            {
                Text = "Chi tiết",
                Font = new Font("Tahoma", 11, FontStyle.Bold),
                Dock = DockStyle.Bottom,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(3, 161, 252),
                Tag = order.oder_id
            };

            btnDetail.Click += (s, e) =>
            {
                LoadOrderDetail(order, rightPanel);
            };

            card.Controls.Add(lblDetail);
            card.Controls.Add(btnDetail);
            card.Controls.Add(lblSatus);
            card.Controls.Add(lblName);

            return card;
        }

        private void LoadOrderDetail(list_order order, Panel rightPanel)
        {
            rightPanel.Controls.Clear();

            if (order == null) return;

            Label lblHeader = new Label
            {
                Text = $"Chi tiết Đơn hàng #{order.oder_id}",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 40
            };

            Panel infoPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 110,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(55, 55, 55)
            };

            Label lblInfo = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10),
                Text =
                    $"👤 Người đặt: {order.customer_name ?? "Không rõ"}\n" +
                    $"📦 Trạng thái: ",
                AutoSize = false
            };

            Label lblStatus = new Label
            {
                Text = GetStatusText(order.status),
                ForeColor = GetStatusColor(order.status),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(120, 28),
                AutoSize = true
            };

            Label lblDates = new Label
            {
                Dock = DockStyle.Bottom,
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                Height = 40,
                Text =
                    $"🕒 Ngày tạo: {order.created_at?.ToString("dd/MM/yyyy HH:mm") ?? "N/A"}\n" +
                    $"🗓️ Cập nhật gần nhất: {order.updated_at?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa có"}"
            };

            infoPanel.Controls.Add(lblDates);
            infoPanel.Controls.Add(lblStatus);
            infoPanel.Controls.Add(lblInfo);

            ListView listView = new ListView
            {
                Dock = DockStyle.Top,
                Height = 220,
                View = View.Details,
                FullRowSelect = true,
                BackColor = Color.FromArgb(60, 60, 60),
                Enabled = false
            };

            listView.Columns.Add("Tên Nguyên liệu", 190);
            listView.Columns.Add("SL", 50);
            listView.Columns.Add("Đơn giá", 90);

            foreach (var detail in OrderServices.Instance.GetOrderDetailByID(order.oder_id))
            {
                listView.Items.Add(new ListViewItem(new[]
                {
                    detail.food.name,
                    $"x{detail.quantity}",
                    $"{detail.price:N0}.000 VNĐ"
                }));
            }

            rightPanel.Controls.Add(listView);
            rightPanel.Controls.Add(infoPanel);
            rightPanel.Controls.Add(lblHeader);
        }

        private Color GetStatusColor(int status)
        {
            switch (status)
            {
                case -1:
                    return Color.Red;
                case 0:
                    return Color.Orange;
                case 1:
                    return Color.LightSkyBlue;
                case 2:
                    return Color.LimeGreen;
                default:
                    return Color.Gray;
            }
        }
        private string GetStatusText(int status)
        {
            switch (status)
            {
                case -1:
                    return "❌ Từ chối";
                case 0:
                    return "⏳ Chờ duyệt";
                case 1:
                    return "⏳ Đang thực hiện";
                case 2:
                    return "✅ Đã hoàn thành";
                default:
                    return "Không xác định";
            }
        }
    }
}
