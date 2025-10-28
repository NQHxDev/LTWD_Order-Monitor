using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base_BUS;

namespace Order_Monitor.ListControl
{
    public partial class Kho : UserControl
    {
        private Panel mainContainer;
        private FlowLayoutPanel flowPanel;

        public Kho()
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

            if (!AccountServices.Instance.IsLogged_Staff)
            {
                var loginPanel = new Login();
                loginPanel.Dock = DockStyle.Fill;
                loginPanel.RequiredRole = 0;

                loginPanel.OnLoginSuccess += (userLogin) =>
                {
                    LoadView();
                };
                mainContainer.Controls.Add(loginPanel);
            }
            else
            {
                InitializePanel();
                LoadDepotItems();
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

            Button btnOrder = new Button();
            btnOrder.Text = "Đặt hàng";
            btnOrder.Size = new Size(120, 35);
            btnOrder.BackColor = Color.FromArgb(0, 120, 215);
            btnOrder.ForeColor = Color.White;
            btnOrder.FlatStyle = FlatStyle.Flat;
            btnOrder.Margin = new Padding(10, 10, 10, 10);
            btnOrder.Click += (s, e) => ViewDepotOrder();

            Button btnImport = new Button();
            btnImport.Text = "Nhập kho";
            btnImport.Size = new Size(120, 35);
            btnImport.BackColor = Color.FromArgb(0, 120, 215);
            btnImport.ForeColor = Color.White;
            btnImport.FlatStyle = FlatStyle.Flat;
            btnImport.Margin = new Padding(10, 10, 10, 10);
            btnImport.Click += (s, e) => ViewDepotImport();

            Button btnExport = new Button();
            btnExport.Text = "Xuất kho";
            btnExport.Size = new Size(120, 35);
            btnExport.BackColor = Color.FromArgb(0, 120, 215);
            btnExport.ForeColor = Color.White;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Margin = new Padding(10, 10, 10, 10);
            btnExport.Click += (s, e) => ViewDepotExport();

            Button btnLogout = new Button();
            btnLogout.Text = "Đăng xuất";
            btnLogout.Size = new Size(100, 35);
            btnLogout.BackColor = Color.FromArgb(200, 50, 50);
            btnLogout.ForeColor = Color.White;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Margin = new Padding(10, 10, 10, 10);
            btnLogout.Click += (s, e) =>
            {

                AccountServices.Instance.Logout("staff");
                LoadView();
            };

            Label lblWelcome = new Label();
            lblWelcome.Text = $"Xin chào: {AccountServices.Instance.Current_Staff.name}";
            lblWelcome.Dock = DockStyle.Fill;
            lblWelcome.Size = new Size(300, 35);
            lblWelcome.TextAlign = ContentAlignment.MiddleCenter;
            lblWelcome.Font = new Font("Tahoma", 14, FontStyle.Bold);
            lblWelcome.ForeColor = Color.White;

            buttonPanel.Controls.AddRange(new Control[] { btnOrder, btnImport, btnExport, btnLogout, lblWelcome });

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

        private void LoadDepotItems()
        {
            flowPanel.Controls.Clear();

            var listItemDepot = DepotServices.Instance.GetListItemDepot();

            foreach (var dep in listItemDepot)
            {
                var item = dep.item;
                string donVi = item?.unit?.abbreviation ?? "N/A";

                string statusText;
                Color statusColor;

                if (item.is_active) 
                {
                    statusText = "Còn HSD";
                    statusColor = Color.LightGreen;
                }
                else
                {
                    statusText = "Hết HSD";
                    statusColor = Color.OrangeRed;
                }

                Panel card = new Panel();
                card.Width = 260;
                card.Height = 180;
                card.BackColor = Color.FromArgb(45, 45, 45);
                card.BorderStyle = BorderStyle.FixedSingle;
                card.Margin = new Padding(10);
                card.Padding = new Padding(10);
                card.Cursor = Cursors.Hand;

                Label lblName = new Label();
                lblName.Text = $"[{item.item_id}] - {item.name}";
                lblName.Font = new Font("Tahoma", 11, FontStyle.Bold);
                lblName.ForeColor = Color.White;
                lblName.AutoSize = false;
                lblName.Height = 30;
                lblName.Dock = DockStyle.Top;

                Label lblDetail = new Label();
                lblDetail.Dock = DockStyle.Fill;
                lblDetail.ForeColor = Color.Gainsboro;
                lblDetail.Font = new Font("Tahoma", 10);
                lblDetail.Padding = new Padding(0, 5, 0, 5);
                lblDetail.Text =
                    $"Số lượng: {dep.quantity} {donVi}\n\n" +
                    $"Giá nhập gần nhất: {item.import_price:N0} VNĐ\n" +
                    $"Ngày nhập gần nhất: {dep.last_updated?.ToString("dd/MM/yyyy") ?? ""}";

                Label lblStatus = new Label();
                lblStatus.Text = statusText;
                lblStatus.Font = new Font("Tahoma", 9, FontStyle.Bold);
                lblStatus.ForeColor = statusColor;
                lblStatus.Dock = DockStyle.Bottom;
                lblStatus.Height = 25;
                lblStatus.TextAlign = ContentAlignment.MiddleRight;

                card.Controls.Add(lblDetail);
                card.Controls.Add(lblStatus);
                card.Controls.Add(lblName);

                flowPanel.Controls.Add(card);
            }

            if (flowPanel.Controls.Count == 0)
            {
                Label lblEmpty = new Label();
                lblEmpty.Text = "Không có nguyên liệu nào trong kho!";
                lblEmpty.Font = new Font("Tahoma", 12, FontStyle.Italic);
                lblEmpty.ForeColor = Color.Gray;
                lblEmpty.Dock = DockStyle.Fill;
                lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
                flowPanel.Controls.Add(lblEmpty);
            }
        }

        private void ViewDepotExport()
        {
            mainContainer.Visible = false;

            DepotExport depotExportPanel = new DepotExport(AccountServices.Instance.Current_Staff.ac_id);
            depotExportPanel.Dock = DockStyle.Fill;
            depotExportPanel.BackButtonClicked += () =>
            {
                this.Controls.Remove(depotExportPanel);
                mainContainer.Visible = true;
            };
            this.Controls.Add(depotExportPanel);
        }

        private void ViewDepotImport()
        {
            mainContainer.Visible = false;

            DepotImport depotImportPanel = new DepotImport(AccountServices.Instance.Current_Staff.ac_id);
            depotImportPanel.Dock = DockStyle.Fill;
            depotImportPanel.BackButtonClicked += () =>
            {
                this.Controls.Remove(depotImportPanel);
                mainContainer.Visible = true;
            };
            this.Controls.Add(depotImportPanel);
        }

        private void ViewDepotOrder()
        {
            mainContainer.Visible = false;

            DepotOrder depotOrderPanel = new DepotOrder(AccountServices.Instance.Current_Staff.ac_id);
            depotOrderPanel.Dock = DockStyle.Fill;
            depotOrderPanel.BackButtonClicked += () =>
            {
                this.Controls.Remove(depotOrderPanel);
                mainContainer.Visible = true;
            };
            this.Controls.Add(depotOrderPanel);
        }
    }
}
