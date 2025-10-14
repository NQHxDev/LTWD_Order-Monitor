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
    public partial class QuanLy : UserControl
    {
        private Panel mainContainer;
        private FlowLayoutPanel flowPanel;

        private string loginName = string.Empty;

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

            if (!DataCache.IsLoggedInAdmin)
            {
                var loginPanel = new Login();
                loginPanel.Dock = DockStyle.Fill;
                loginPanel.RequiredRole = 1;

                loginPanel.OnLoginSuccess += (userLogin) =>
                {
                    loginName = userLogin.name;
                    LoadView();
                };
                // Xử lý Đăng nhập sai Role
                loginPanel.OnLoginAttempt += (userLogin, isValid) =>
                {
                    if (!isValid)
                    {
                        if (userLogin != null)
                        {
                            DataCache.Logout();
                            LoadView();
                        }
                    }
                };
                mainContainer.Controls.Add(loginPanel);
            }
            else
            {
                InitializePanel();
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
            //btnOrder.Click += (s, e) => .....();

            Button btnImport = new Button();
            btnImport.Text = "Nhập kho";
            btnImport.Size = new Size(120, 35);
            btnImport.BackColor = Color.FromArgb(0, 120, 215);
            btnImport.ForeColor = Color.White;
            btnImport.FlatStyle = FlatStyle.Flat;
            btnImport.Margin = new Padding(10, 10, 10, 10);
            //btnImport.Click += (s, e) => .....();

            Button btnExport = new Button();
            btnExport.Text = "Xuất kho";
            btnExport.Size = new Size(120, 35);
            btnExport.BackColor = Color.FromArgb(0, 120, 215);
            btnExport.ForeColor = Color.White;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Margin = new Padding(10, 10, 10, 10);
            //btnExport.Click += (s, e) => .....();

            Button btnLogout = new Button();
            btnLogout.Text = "Đăng xuất";
            btnLogout.Size = new Size(100, 35);
            btnLogout.BackColor = Color.FromArgb(200, 50, 50);
            btnLogout.ForeColor = Color.White;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Margin = new Padding(10, 10, 10, 10);
            btnLogout.Click += (s, e) =>
            {
                DataCache.Logout("Admin");
                LoadView();
            };

            Label lblWelcome = new Label();
            lblWelcome.Text = $"Xin chào: {loginName}";
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
    }
}
