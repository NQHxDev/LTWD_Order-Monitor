using Base_BUS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Order_Monitor.ListControl.Manager;
using System.Windows.Forms.DataVisualization.Charting;

namespace Order_Monitor.ListControl
{
    public partial class QuanLy : UserControl
    {
        private ManagermentServices managermentInstance = ManagermentServices.Instance;

        private Panel mainContainer;
        private FlowLayoutPanel flowPanel;

        private Panel headerPanel;
        private FlowLayoutPanel cardsPanel;
        private Chart salesChart;
        private FlowLayoutPanel bestSellerPanel;
        private ComboBox cbRange;

        private string loginName = string.Empty;
        private int loginID = -1;

        public QuanLy()
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

            LoadView();
        }

        private void LoadView()
        {
            mainContainer.Controls.Clear();

            if (!LoginServices.Instance.IsLogged_Leader)
            {
                var loginPanel = new Login();
                loginPanel.Dock = DockStyle.Fill;
                loginPanel.RequiredRole = 1;

                loginPanel.OnLoginSuccess += (userLogin) =>
                {
                    loginName = userLogin.name;
                    loginID = userLogin.ac_id;
                    LoadView();
                };
                // Xử lý Đăng nhập sai Role
                loginPanel.OnLoginAttempt += (userLogin, isValid) =>
                {
                    if (!isValid)
                    {
                        if (userLogin != null)
                        {
                            LoginServices.Instance.Logout("leader");
                            LoadView();
                        }
                    }
                };
                mainContainer.Controls.Add(loginPanel);
            }
            else
            {
                InitializePanel();
                InitializeComponentManual();
                LoadSummaryCards();
                LoadChartFor("Week");
                LoadBestSellers();
            }
        }

        private void InitializePanel()
        {
            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.FromArgb(60, 60, 60);

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel();
            buttonPanel.Height = 60;
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.BackColor = Color.FromArgb(60, 60, 60);
            buttonPanel.FlowDirection = FlowDirection.LeftToRight;
            buttonPanel.WrapContents = false;
            buttonPanel.Padding = new Padding(10, 10, 10, 10);
            buttonPanel.AutoScroll = true;

            Button btnBrowseOrder = new Button();
            btnBrowseOrder.Text = "Duyệt đơn Đặt Hàng";
            btnBrowseOrder.Size = new Size(150, 35);
            btnBrowseOrder.BackColor = Color.FromArgb(0, 120, 215);
            btnBrowseOrder.ForeColor = Color.White;
            btnBrowseOrder.FlatStyle = FlatStyle.Flat;
            btnBrowseOrder.Margin = new Padding(10, 10, 10, 10);
            btnBrowseOrder.Click += (s, e) => ViewPanelBrowseOrder();

            Button btnViewDepot = new Button();
            btnViewDepot.Text = "Xem Kho hàng";
            btnViewDepot.Size = new Size(120, 35);
            btnViewDepot.BackColor = Color.FromArgb(0, 120, 215);
            btnViewDepot.ForeColor = Color.White;
            btnViewDepot.FlatStyle = FlatStyle.Flat;
            btnViewDepot.Margin = new Padding(10, 10, 10, 10);
            btnViewDepot.Click += (s, e) => ViewPanelDepot();

            Button btnViewListEmploy = new Button();
            btnViewListEmploy.Text = "Danh sách Nhân viên";
            btnViewListEmploy.Size = new Size(150, 35);
            btnViewListEmploy.BackColor = Color.FromArgb(0, 120, 215);
            btnViewListEmploy.ForeColor = Color.White;
            btnViewListEmploy.FlatStyle = FlatStyle.Flat;
            btnViewListEmploy.Margin = new Padding(10, 10, 10, 10);
            btnViewListEmploy.Click += (s, e) => ViewPanelListEmployees();

            Button btnLogout = new Button();
            btnLogout.Text = "Đăng xuất";
            btnLogout.Size = new Size(100, 35);
            btnLogout.BackColor = Color.FromArgb(200, 50, 50);
            btnLogout.ForeColor = Color.White;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Margin = new Padding(10, 10, 10, 10);
            btnLogout.Click += (s, e) =>
            {
                LoginServices.Instance.Logout("leader");
                loginName = string.Empty;
                loginID = -1;
                LoadView();
            };

            Label lblWelcome = new Label();
            lblWelcome.Text = $"Xin chào: {loginName}";
            lblWelcome.Dock = DockStyle.Fill;
            lblWelcome.Size = new Size(300, 35);
            lblWelcome.TextAlign = ContentAlignment.MiddleCenter;
            lblWelcome.Font = new Font("Tahoma", 14, FontStyle.Bold);
            lblWelcome.ForeColor = Color.White;

            buttonPanel.Controls.AddRange(new Control[] { btnBrowseOrder, btnViewDepot, btnViewListEmploy, btnLogout, lblWelcome });

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

            mainContainer.Controls.Add(mainPanel);
        }

        private void InitializeComponentManual()
        {
            flowPanel.Controls.Clear();
            flowPanel.AutoScroll = true;
            flowPanel.WrapContents = false;
            flowPanel.FlowDirection = FlowDirection.TopDown;
            flowPanel.Padding = new Padding(10);
            flowPanel.BackColor = Color.FromArgb(45, 45, 48);

            void AdjustChildWidths()
            {
                int innerWidth = flowPanel.ClientSize.Width - flowPanel.Padding.Horizontal - 10;
                foreach (Control c in flowPanel.Controls)
                {
                    c.Width = innerWidth;
                }
            }

            headerPanel = new Panel
            {
                Height = 60,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(35, 35, 38),
                Margin = new Padding(0, 0, 0, 10)
            };

            var lblTitle = new Label
            {
                Text = "Thống kê doanh số & Best sellers",
                AutoSize = false,
                Dock = DockStyle.Left,
                Width = 400,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Tahoma", 14, FontStyle.Bold),
                ForeColor = Color.White
            };

            cbRange = new ComboBox
            {
                Width = 160,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Right
            };
            cbRange.Items.AddRange(new[] { "Month", "Year", "Week", "Day" });
            cbRange.SelectedIndex = 0;
            cbRange.SelectedIndexChanged += (s, e) => LoadChartFor(cbRange.SelectedItem.ToString());

            headerPanel.Controls.Add(cbRange);
            headerPanel.Controls.Add(lblTitle);

            cardsPanel = new FlowLayoutPanel
            {
                Height = 90,
                FlowDirection = FlowDirection.LeftToRight,
                AutoScroll = false,
                Padding = new Padding(10),
                WrapContents = false,
                BackColor = Color.FromArgb(45, 45, 48),
                Margin = new Padding(0, 0, 0, 10)
            };

            salesChart = new Chart
            {
                Height = 320,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(45, 45, 48),
                Margin = new Padding(0, 0, 0, 10)
            };
            salesChart.ChartAreas.Add(new ChartArea("Main"));
            salesChart.Legends.Add(new Legend("L"));
            salesChart.ChartAreas["Main"].BackColor = Color.FromArgb(50, 50, 50);

            var bestLabel = new Label
            {
                Text = "Best Sellers",
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.White,
                Font = new Font("Tahoma", 12, FontStyle.Bold),
                Padding = new Padding(10, 0, 0, 0),
                BackColor = Color.FromArgb(45, 45, 48),
                Margin = new Padding(0, 0, 0, 5)
            };

            bestSellerPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(45, 45, 48),
                Margin = new Padding(0, 0, 0, 10)
            };

            flowPanel.Controls.Add(headerPanel);
            flowPanel.Controls.Add(cardsPanel);
            flowPanel.Controls.Add(salesChart);
            flowPanel.Controls.Add(bestLabel);
            flowPanel.Controls.Add(bestSellerPanel);

            flowPanel.Resize += (s, e) => AdjustChildWidths();
            flowPanel.Layout += (s, e) => AdjustChildWidths();

            AdjustChildWidths();
        }

        private void LoadSummaryCards()
        {
            cardsPanel.Controls.Clear();

            var sYear = managermentInstance.GetSalesThisYear();
            var sMonth = managermentInstance.GetSalesThisMonth();
            var sWeek = managermentInstance.GetSalesThisWeek();
            var sDay = managermentInstance.GetSalesToday();

            cardsPanel.Controls.Add(CreateCard("Doanh số năm", sYear.Total, sYear.DayStart, sYear.DayEnd));
            cardsPanel.Controls.Add(CreateCard("Doanh số tháng", sMonth.Total, sMonth.DayStart, sMonth.DayEnd));
            cardsPanel.Controls.Add(CreateCard("Doanh số tuần", sWeek.Total, sWeek.DayStart, sWeek.DayEnd));
            cardsPanel.Controls.Add(CreateCard("Doanh số hôm nay", sDay.Total, sDay.DayStart, sDay.DayEnd));
        }

        private Control CreateCard(string title, decimal money, DateTime from, DateTime to)
        {
            var card = new Panel
            {
                Width = 250,
                Height = 70,
                Margin = new Padding(10),
                BackColor = Color.FromArgb(60, 60, 63)
            };

            var lblTitle = new Label { 
                Text = title,
                Dock = DockStyle.Top,
                Height = 22,
                ForeColor = Color.White,
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                Padding = new Padding(8, 4, 0, 0)
            };
            var lblValue = new Label {
                Text = money.ToString("N0") + " VNĐ",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Tahoma", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 120, 215),
                Padding = new Padding(8, 0, 0, 0)
            };
            var lblRange = new Label {
                Text = $"{from:dd/MM/yyyy} - {to:dd/MM/yyyy}",
                Dock = DockStyle.Bottom,
                Height = 18,
                ForeColor = Color.LightGray,
                Font = new Font("Tahoma", 8),
                Padding = new Padding(8, 0, 0, 4)
            };

            card.Controls.Add(lblValue);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblRange);

            return card;
        }

        private void LoadChartFor(string range)
        {
            salesChart.Series.Clear();
            var series = new Series("Doanh số")
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.Date
            };
            salesChart.Series.Add(series);
            salesChart.ChartAreas["Main"].AxisX.LabelStyle.Format = "dd/MM";

            DateTime from, to;
            if (range == "Year")
            {
                var now = DateTime.Now;
                from = new DateTime(now.Year, 1, 1);
                to = from.AddYears(1).AddTicks(-1);

                // Lấy doanh số theo tháng
                var list = new List<(DateTime Date, decimal Total)>();
                for (int m = 1; m <= 12; m++)
                {
                    var s = new DateTime(now.Year, m, 1);
                    var e = s.AddMonths(1).AddTicks(-1);
                    var val = managermentInstance.GetSalesFoods(s, e).Total;
                    list.Add((s, val));
                }

                var xValues = list.Select(x => x.Date.ToString("MMM")).ToList();
                var yValues = list.Select(x => x.Total).ToList();

                series.ChartType = SeriesChartType.Column;
                series.Points.DataBindXY(xValues, yValues);

                salesChart.ChartAreas["Main"].AxisX.LabelStyle.Angle = -45;
                salesChart.Titles.Clear();
                salesChart.Titles.Add($"Doanh số theo tháng - {now.Year}");
            }
            else if (range == "Month")
            {
                var now = DateTime.Now;
                from = new DateTime(now.Year, now.Month, 1);
                to = from.AddMonths(1).AddTicks(-1);

                var daily = managermentInstance.GetDailySales(from, to);
                var xValues = daily.Select(d => d.Date.ToString("dd")).ToList();
                var yValues = daily.Select(d => d.Total).ToList();

                series.Points.DataBindXY(xValues, yValues);
                salesChart.Titles.Clear();
                salesChart.Titles.Add($"Doanh số ngày trong tháng {now.Month}/{now.Year}");
            }
            else if (range == "Week")
            {
                var today = DateTime.Now.Date;
                int diff = (7 + (int)today.DayOfWeek - (int)DayOfWeek.Monday) % 7;
                var startOfWeek = today.AddDays(-diff);

                var list = new List<(DateTime Date, decimal Total)>();
                for (int i = 0; i < 7; i++)
                {
                    var s = startOfWeek.AddDays(i);
                    var e = s.AddDays(1).AddTicks(-1);
                    var val = managermentInstance.GetSalesFoods(s, e).Total;
                    list.Add((s, val));
                }

                var xValues = list.Select(x => x.Date.ToString("ddd")).ToList();
                var yValues = list.Select(x => x.Total).ToList();

                series.Points.DataBindXY(xValues, yValues);
                salesChart.Titles.Clear();
                salesChart.Titles.Add($"Doanh số tuần ({startOfWeek:dd/MM} - {startOfWeek.AddDays(6):dd/MM})");
            }
            else // Day
            {
                var today = DateTime.Now.Date;
                // Hiển thị chi tiết theo giờ (0..23)
                var points = new List<(string Hour, decimal Total)>();
                for (int h = 0; h < 24; h++)
                {
                    var s = today.AddHours(h);
                    var e = s.AddHours(1).AddTicks(-1);
                    var total = managermentInstance.GetSalesFoods(s, e).Total;
                    points.Add((h.ToString("D2") + ":00", total));
                }

                var xValues = points.Select(p => p.Hour).ToList();
                var yValues = points.Select(p => p.Total).ToList();

                series.Points.DataBindXY(xValues, yValues);
                salesChart.Titles.Clear();
                salesChart.Titles.Add($"Doanh số theo giờ - {today:dd/MM/yyyy}");
            }
        }

        private void LoadBestSellers()
        {
            bestSellerPanel.Controls.Clear();

            var items = managermentInstance.GetBestSellers(10);
            if (!items.Any())
            {
                var lbl = new Label { Text = "Không có dữ liệu", ForeColor = Color.LightGray, AutoSize = true, Padding = new Padding(10) };
                bestSellerPanel.Controls.Add(lbl);
                return;
            }

            int maxQty = items.Max(x => x.Quantity);
            foreach (var it in items)
            {
                var card = new Panel
                {
                    Width = 220,
                    Height = 90,
                    Margin = new Padding(10),
                    BackColor = Color.FromArgb(60, 60, 63)
                };

                // image placeholder
                var img = new Panel { Width = 70, Height = 70, Left = 8, Top = 10, BackColor = Color.FromArgb(80, 80, 83) };
                // you can replace with PictureBox and load actual images if available

                var lblName = new Label { Text = it.Name, Left = 86, Top = 10, Width = 120, Height = 28, ForeColor = Color.White, Font = new Font("Tahoma", 9, FontStyle.Bold) };
                var lblQty = new Label { Text = $"Số lượng: {it.Quantity}", Left = 86, Top = 34, Width = 120, Height = 18, ForeColor = Color.LightGray, Font = new Font("Tahoma", 8) };

                // progress
                var pb = new ProgressBar { Left = 86, Top = 54, Width = 120, Height = 14 };
                var ratio = maxQty == 0 ? 0 : (int)Math.Round((double)it.Quantity / maxQty * 100);
                pb.Value = Math.Min(Math.Max(ratio, 0), 100);

                card.Controls.Add(img);
                card.Controls.Add(lblName);
                card.Controls.Add(lblQty);
                card.Controls.Add(pb);
                bestSellerPanel.Controls.Add(card);
            }
        }

        private void ViewPanelBrowseOrder()
        {
            mainContainer.Visible = false;

            BrowseOrder browerOrderPanel = new BrowseOrder(LoginServices.Instance.Current_Leader.ac_id);
            browerOrderPanel.Dock = DockStyle.Fill;
            browerOrderPanel.BackButtonClicked += () =>
            {
                this.Controls.Remove(browerOrderPanel);
                mainContainer.Visible = true;
            };
            this.Controls.Add(browerOrderPanel);
        }

        private void ViewPanelListEmployees()
        {
            mainContainer.Visible = false;

            ListEmployees listEmployeesPanel = new ListEmployees(LoginServices.Instance.Current_Leader.ac_id);
            listEmployeesPanel.Dock = DockStyle.Fill;
            listEmployeesPanel.BackButtonClicked += () =>
            {
                this.Controls.Remove(listEmployeesPanel);
                mainContainer.Visible = true;
            };
            this.Controls.Add(listEmployeesPanel);
        }

        private void ViewPanelDepot()
        {
            mainContainer.Visible = false;

            ViewDepot viewDepotPanel = new ViewDepot(LoginServices.Instance.Current_Leader.ac_id);
            viewDepotPanel.Dock = DockStyle.Fill;
            viewDepotPanel.BackButtonClicked += () =>
            {
                this.Controls.Remove(viewDepotPanel);
                mainContainer.Visible = true;
            };
            this.Controls.Add(viewDepotPanel);
        }
    }
}
