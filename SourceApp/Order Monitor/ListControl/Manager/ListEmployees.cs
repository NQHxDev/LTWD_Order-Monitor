using Base_BUS;
using Base_DAL.ContextDatabase;
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

namespace Order_Monitor.ListControl.Manager
{
    public partial class ListEmployees : UserControl
    {
        private Panel mainContainer;
        private FlowLayoutPanel flowPanel;

        public event Action BackButtonClicked;

        private int leader_ID;

        public ListEmployees(int leaderID)
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

            flowPanel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = false,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(10),
                AutoSize = false,
            };

            Label spacer = new Label();
            spacer.Dock = DockStyle.Top;
            spacer.Height = 45;
            spacer.Padding = new Padding(0, 0, 0, 5);

            mainPanel.Controls.Add(flowPanel);
            mainPanel.Controls.Add(buttonPanel);
            mainPanel.Controls.Add(spacer);

            mainContainer.Controls.Add(mainPanel);

            LoadEmployees();
        }

        private void LoadEmployees()
        {
            flowPanel.Controls.Clear();

            foreach (var acc in AccountServices.Instance.GetAllAccounts())
            {
                flowPanel.Controls.Add(CreateEmployeePanel(acc));
            }

            // Thêm panel tạo nhân sự mới
            flowPanel.Controls.Add(CreateAddEmployeePanel());
        }

        private Panel CreateEmployeePanel(account acc)
        {
            // Panel bao quanh
            Panel panel = new Panel();
            panel.Width = 700;
            panel.AutoSize = true;
            panel.Padding = new Padding(10);
            panel.Margin = new Padding(8);
            panel.BackColor = Color.FromArgb(75, 75, 75);
            panel.BorderStyle = BorderStyle.FixedSingle;

            // Checkbox mở rộng
            CheckBox chkExpand = new CheckBox();
            chkExpand.AutoSize = true;
            chkExpand.Margin = new Padding(0, 5, 10, 0);

            // Thông tin chính
            Label lblInfo = new Label();
            lblInfo.AutoSize = true;
            lblInfo.ForeColor = Color.White;
            lblInfo.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblInfo.Text =
                $"ID: {acc.ac_id} | Username: {acc.username} | Name: {acc.name} | " +
                $"Role: {(acc.role == 1 ? "Leader" : "Staff")} | " +
                $"Trạng thái: {(acc.acc_status == 0 ? "Hoạt động" : "Bị cấm")} | " +
                $"Ngày gia nhập: {acc.created_at?.ToString("dd/MM/yyyy")}";

            FlowLayoutPanel header = new FlowLayoutPanel();
            header.FlowDirection = FlowDirection.LeftToRight;
            header.AutoSize = true;
            header.Controls.Add(chkExpand);
            header.Controls.Add(lblInfo);

            // Panel chi tiết (ẩn)
            Panel pnlDetails = new Panel();
            pnlDetails.Visible = false;
            pnlDetails.Dock = DockStyle.Top;
            pnlDetails.AutoSize = true;
            pnlDetails.Padding = new Padding(10);
            pnlDetails.BackColor = Color.FromArgb(55, 55, 55);

            // Đổi mật khẩu
            Label lblPass = new Label() { Text = "Đổi mật khẩu:", ForeColor = Color.White, AutoSize = true };
            TextBox txtPass = new TextBox() { Width = 150, UseSystemPasswordChar = true };
            Button btnSavePass = new Button()
            {
                Text = "Lưu",
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            btnSavePass.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtPass.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu mới!");
                    return;
                }
                AccountServices.Instance.UpdatePassword(acc.ac_id, txtPass.Text);

                MessageBox.Show("Cập nhật mật khẩu thành công!");
                txtPass.Clear();
            };

            // Cập nhật trạng thái
            Label lblStatus = new Label() { Text = "Trạng thái:", ForeColor = Color.White, AutoSize = true };
            ComboBox cmbStatus = new ComboBox() { Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbStatus.Items.AddRange(new object[] { "Hoạt động", "Bị cấm" });
            cmbStatus.SelectedIndex = acc.acc_status == 0 ? 0 : 1;

            Button btnUpdateStatus = new Button()
            {
                Text = "Cập nhật",
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            btnUpdateStatus.Click += (s, e) =>
            {
                int newStatus = cmbStatus.SelectedIndex == 0 ? 0 : -1;
                
                AccountServices.Instance.UpdateStatus(acc.ac_id, newStatus);

                MessageBox.Show("Đã cập nhật trạng thái!");
                LoadEmployees();
            };

            FlowLayoutPanel inner = new FlowLayoutPanel();
            inner.FlowDirection = FlowDirection.LeftToRight;
            inner.AutoSize = true;
            inner.WrapContents = true;
            inner.Controls.AddRange(new Control[]
            {
                lblPass, txtPass, btnSavePass,
                lblStatus, cmbStatus, btnUpdateStatus
            });

            pnlDetails.Controls.Add(inner);
            panel.Controls.Add(pnlDetails);
            panel.Controls.Add(header);

            chkExpand.CheckedChanged += (s, e) =>
            {
                pnlDetails.Visible = chkExpand.Checked;
            };

            return panel;
        }

        private Panel CreateAddEmployeePanel()
        {
            Panel panel = new Panel();
            panel.Width = 700;
            panel.AutoSize = true;
            panel.Padding = new Padding(10);
            panel.Margin = new Padding(8);
            panel.BackColor = Color.FromArgb(80, 80, 80);
            panel.BorderStyle = BorderStyle.FixedSingle;

            Label lblTitle = new Label()
            {
                Text = "➕ Thêm nhân sự mới",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 30
            };

            // Tạo label và textbox cho từng trường
            Label lblUser = new Label() { Text = "Username:", ForeColor = Color.White, AutoSize = true, Margin = new Padding(5, 8, 5, 0) };
            TextBox txtUser = new TextBox() { Width = 120 };

            Label lblPass = new Label() { Text = "Password:", ForeColor = Color.White, AutoSize = true, Margin = new Padding(15, 8, 5, 0) };
            TextBox txtPass = new TextBox() { Width = 120, UseSystemPasswordChar = true };

            Label lblName = new Label() { Text = "Name:", ForeColor = Color.White, AutoSize = true, Margin = new Padding(15, 8, 5, 0) };
            TextBox txtName = new TextBox() { Width = 120 };

            Label lblRole = new Label() { Text = "Role:", ForeColor = Color.White, AutoSize = true, Margin = new Padding(15, 8, 5, 0) };
            ComboBox cmbRole = new ComboBox() { Width = 100, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbRole.Items.AddRange(new object[] { "Staff", "Leader" });
            cmbRole.SelectedIndex = 0;

            Button btnAdd = new Button()
            {
                Text = "Tạo mới",
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Margin = new Padding(20, 5, 0, 0)
            };

            btnAdd.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPass.Text))
                {
                    MessageBox.Show("Vui lòng nhập Username và Password!");
                    return;
                }

                if (AccountServices.Instance.CheckUsername(txtUser.Text))
                {
                    MessageBox.Show("Username đã tồn tại!");
                    return;
                }

                account newAcc = new account()
                {
                    username = txtUser.Text,
                    password = txtPass.Text,
                    name = txtName.Text,
                    role = cmbRole.SelectedIndex == 1 ? 1 : 0,
                    acc_status = 0,
                    created_at = DateTime.Now
                };

                AccountServices.Instance.AddNewAccount(newAcc);
                MessageBox.Show("Thêm nhân sự thành công!");
                LoadEmployees();
            };

            // Giao diện ngang: Label - TextBox xen kẽ
            FlowLayoutPanel inner = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                WrapContents = true
            };
            inner.Controls.AddRange(new Control[]
            {
                lblUser, txtUser,
                lblPass, txtPass,
                lblName, txtName,
                lblRole, cmbRole,
                btnAdd
            });

            panel.Controls.Add(inner);
            panel.Controls.Add(lblTitle);

            return panel;
        }
    }
}
