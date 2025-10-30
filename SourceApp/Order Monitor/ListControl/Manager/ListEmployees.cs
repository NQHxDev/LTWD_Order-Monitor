using Base_BUS;
using Base_DAL.ContextDatabase;
using Org.BouncyCastle.Asn1.Cmp;
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
        private RadioButton lastChecked = null;
        private ComboBox cmbStatus;
        private TextBox txtReason;

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

            Panel topPanel = new Panel();
            topPanel.Width = 1400;
            topPanel.Height = 400;
            topPanel.Margin = new Padding(0, 0, 0, 20);
            topPanel.BackColor = Color.FromArgb(70, 70, 70);
            topPanel.BorderStyle = BorderStyle.FixedSingle;

            Panel leftPanel = new Panel();
            leftPanel.Dock = DockStyle.Left;
            leftPanel.Width = 500;
            leftPanel.Padding = new Padding(10);
            leftPanel.BackColor = Color.FromArgb(70, 70, 70);

            Panel rightPanel = new Panel();
            rightPanel.Dock = DockStyle.Right;
            rightPanel.Width = 900;
            rightPanel.Padding = new Padding(10);
            rightPanel.BackColor = Color.FromArgb(70, 70, 70);

            CreateAccountControls(leftPanel);

            ManagerChangeAccount(rightPanel);

            topPanel.Controls.Add(leftPanel);
            topPanel.Controls.Add(rightPanel);

            flowPanel.Controls.Add(topPanel);

            Label listTitle = new Label()
            {
                Text = "Danh sách Nhân sự:",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 15)
            };
            flowPanel.Controls.Add(listTitle);

            foreach (var acc in AccountServices.Instance.GetAllAccounts())
            {
                flowPanel.Controls.Add(CreateEmployeePanel(acc));
            }
        }

        private void CreateAccountControls(Panel parentPanel)
        {
            GroupBox grpCreate = new GroupBox();
            grpCreate.Text = "Tạo tài khoản";
            grpCreate.ForeColor = Color.White;
            grpCreate.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grpCreate.Dock = DockStyle.Top;
            grpCreate.Height = 280;
            grpCreate.Padding = new Padding(15);

            TableLayoutPanel layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Top;
            layout.ColumnCount = 3;
            layout.RowCount = 4;
            layout.Padding = new Padding(20, 30, 20, 20);
            layout.AutoSize = true;

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));

            Font labelFont = new Font("Segoe UI", 10, FontStyle.Regular);

            Label lblUser = new Label()
            {
                Text = "Username:",
                ForeColor = Color.White,
                AutoSize = true,
                Font = labelFont,
                Margin = new Padding(0, 0, 0, 5)
            };
            TextBox txtUser = new TextBox() { Width = 180 };

            Label lblName = new Label()
            {
                Text = "Họ tên:",
                ForeColor = Color.White,
                AutoSize = true,
                Font = labelFont,
                Margin = new Padding(0, 0, 0, 5)
            };
            TextBox txtName = new TextBox() { Width = 180 };

            Label lblPass = new Label()
            {
                Text = "Password:",
                ForeColor = Color.White,
                AutoSize = true,
                Font = labelFont,
                Margin = new Padding(0, 0, 0, 5)
            };
            TextBox txtPass = new TextBox() { Width = 180, UseSystemPasswordChar = true };

            Label lblRole = new Label()
            {
                Text = "Vai trò:",
                ForeColor = Color.White,
                AutoSize = true,
                Font = labelFont,
                Margin = new Padding(0, 0, 0, 5)
            };
            ComboBox cmbRole = new ComboBox()
            {
                Width = 180,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRole.Items.AddRange(new object[] { "Staff", "Leader" });
            cmbRole.SelectedIndex = 0;

            Button btnAdd = new Button()
            {
                Text = "Tạo mới",
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Size = new Size(120, 35),
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.None,
                Margin = new Padding(0, 20, 0, 0)
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

                txtUser.Clear();
                txtPass.Clear();
                txtName.Clear();
                cmbRole.SelectedIndex = 0;

                LoadEmployees();
            };

            layout.Controls.Add(lblUser, 0, 0);
            layout.Controls.Add(lblName, 2, 0);
            layout.Controls.Add(txtUser, 0, 1);
            layout.Controls.Add(txtName, 2, 1);
            layout.Controls.Add(lblPass, 0, 2);
            layout.Controls.Add(lblRole, 2, 2);
            layout.Controls.Add(txtPass, 0, 3);
            layout.Controls.Add(cmbRole, 2, 3);
            layout.Controls.Add(btnAdd, 3, 3);

            Label lblSelectNotice = new Label()
            {
                Text = "Quản lý Nhân sự",
                ForeColor = Color.LightYellow,
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter
            };

            FlowLayoutPanel container = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true
            };
            container.Controls.Add(layout);
            grpCreate.Controls.Add(container);

            parentPanel.Controls.Add(grpCreate);
            parentPanel.Controls.Add(lblSelectNotice);
        }

        private void ManagerChangeAccount(Panel parentPanel)
        {
            TableLayoutPanel manageLayout = new TableLayoutPanel();
            manageLayout.Dock = DockStyle.Top;
            manageLayout.ColumnCount = 2;
            manageLayout.RowCount = 1;
            manageLayout.AutoSize = true;
            manageLayout.Padding = new Padding(20, 10, 20, 10);
            manageLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            manageLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            GroupBox grpPassword = new GroupBox();
            grpPassword.Text = "Đổi mật khẩu";
            grpPassword.ForeColor = Color.White;
            grpPassword.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grpPassword.Dock = DockStyle.Fill;
            grpPassword.Height = 250;
            grpPassword.Padding = new Padding(15);

            TableLayoutPanel passLayout = new TableLayoutPanel();
            passLayout.Dock = DockStyle.Fill;
            passLayout.RowCount = 4;
            passLayout.ColumnCount = 1;
            passLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            passLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            passLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            passLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            Label lblNewPass = new Label()
            {
                Text = "Mật khẩu mới:",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                AutoSize = true
            };
            TextBox txtNewPass = new TextBox()
            {
                Width = 220,
                UseSystemPasswordChar = true
            };

            Label lblConfirmPass = new Label()
            {
                Text = "Xác nhận mật khẩu:",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 0)
            };
            TextBox txtConfirmPass = new TextBox()
            {
                Width = 220,
                UseSystemPasswordChar = true
            };

            Label lblPassMessage = new Label()
            {
                ForeColor = Color.LightYellow,
                AutoSize = true,
                Margin = new Padding(0, 5, 0, 0)
            };

            Button btnSavePass = new Button()
            {
                Text = "Lưu",
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 10, 0, 0)
            };

            btnSavePass.Click += (s, e) =>
            {
                lblPassMessage.Text = "";
                if (lastChecked == null || lastChecked.Tag == null)
                {
                    lblPassMessage.Text = "Vui lòng chọn một nhân viên từ danh sách!";
                    lblPassMessage.ForeColor = Color.LightCoral;
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNewPass.Text) || string.IsNullOrWhiteSpace(txtConfirmPass.Text))
                {
                    lblPassMessage.Text = "Vui lòng nhập đầy đủ thông tin!";
                    lblPassMessage.ForeColor = Color.LightCoral;
                    return;
                }

                if (txtNewPass.Text != txtConfirmPass.Text)
                {
                    lblPassMessage.Text = "Hai mật khẩu không khớp!";
                    lblPassMessage.ForeColor = Color.LightCoral;
                    return;
                }

                var selectedAcc = lastChecked.Tag as account;
                AccountServices.Instance.UpdatePassword(selectedAcc.ac_id, txtNewPass.Text);

                lblPassMessage.Text = "Cập nhật mật khẩu thành công!";
                lblPassMessage.ForeColor = Color.LightGreen;

                txtNewPass.Clear();
                txtConfirmPass.Clear();
            };

            passLayout.Controls.Add(lblNewPass, 0, 0);
            passLayout.Controls.Add(txtNewPass, 0, 1);
            passLayout.Controls.Add(lblConfirmPass, 0, 2);
            passLayout.Controls.Add(txtConfirmPass, 0, 3);
            passLayout.Controls.Add(btnSavePass, 0, 4);
            passLayout.Controls.Add(lblPassMessage, 0, 5);
            grpPassword.Controls.Add(passLayout);

            GroupBox grpStatus = new GroupBox();
            grpStatus.Text = "Thay đổi trạng thái";
            grpStatus.ForeColor = Color.White;
            grpStatus.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grpStatus.Dock = DockStyle.Fill;
            grpStatus.Padding = new Padding(15);

            TableLayoutPanel statusLayout = new TableLayoutPanel();
            statusLayout.Dock = DockStyle.Fill;
            statusLayout.RowCount = 6;
            statusLayout.ColumnCount = 1;
            statusLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            statusLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            statusLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            statusLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            statusLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            statusLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            Label lblStatus = new Label()
            {
                Text = "Trạng thái:",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                AutoSize = true
            };

            cmbStatus = new ComboBox()
            {
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatus.Items.AddRange(new object[] { "Hoạt động", "Bị cấm" });
            cmbStatus.SelectedIndex = 0;

            Label lblReason = new Label()
            {
                Text = "Lý do Thay đổi:",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 0)
            };

            txtReason = new TextBox()
            {
                Width = 250,
                Height = 60,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            Label lblStatusMessage = new Label()
            {
                ForeColor = Color.LightYellow,
                AutoSize = true,
                Margin = new Padding(0, 5, 0, 0)
            };

            Button btnUpdateStatus = new Button()
            {
                Text = "Cập nhật",
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 10, 0, 0)
            };

            btnUpdateStatus.Click += (s, e) =>
            {
                lblStatusMessage.Text = "";
                if (lastChecked == null || lastChecked.Tag == null)
                {
                    lblStatusMessage.Text = "Vui lòng chọn một nhân viên từ danh sách!";
                    lblStatusMessage.ForeColor = Color.LightCoral;
                    return;
                }

                var selectedAcc = lastChecked.Tag as account;
                int newStatus = cmbStatus.SelectedIndex == 0 ? 0 : -1;

                if (newStatus == -1 && string.IsNullOrWhiteSpace(txtReason.Text))
                {
                    lblStatusMessage.Text = "Vui lòng nhập lý do khi cấm nhân viên!";
                    lblStatusMessage.ForeColor = Color.LightCoral;
                    return;
                }

                AccountServices.Instance.UpdateStatus(selectedAcc.ac_id, newStatus, txtReason.Text.Trim());

                lblStatusMessage.Text = "Cập nhật trạng thái thành công!";
                lblStatusMessage.ForeColor = Color.LightGreen;

                LoadEmployees();
            };

            statusLayout.Controls.Add(lblStatus, 0, 0);
            statusLayout.Controls.Add(cmbStatus, 0, 1);
            statusLayout.Controls.Add(lblReason, 0, 2);
            statusLayout.Controls.Add(txtReason, 0, 3);
            statusLayout.Controls.Add(btnUpdateStatus, 0, 4);
            statusLayout.Controls.Add(lblStatusMessage, 0, 5);
            grpStatus.Controls.Add(statusLayout);

            manageLayout.Controls.Add(grpPassword, 0, 0);
            manageLayout.Controls.Add(grpStatus, 1, 0);

            Label lblSelectNotice = new Label()
            {
                Text = "Vui lòng chọn 1 nhân viên từ danh sách bên dưới",
                ForeColor = Color.LightYellow,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                Dock = DockStyle.Top,
                Height = 25,
                TextAlign = ContentAlignment.MiddleCenter
            };

            parentPanel.Controls.Clear();
            parentPanel.Controls.Add(manageLayout);
            parentPanel.Controls.Add(lblSelectNotice);
        }

        private Panel CreateEmployeePanel(account acc)
        {
            Panel panel = new Panel();
            panel.Width = 950;
            panel.Height = 50;
            panel.Padding = new Padding(10);
            panel.Margin = new Padding(8);
            panel.BackColor = Color.FromArgb(75, 75, 75);
            panel.BorderStyle = BorderStyle.FixedSingle;

            RadioButton radSelect = new RadioButton();
            radSelect.AutoSize = true;
            radSelect.Margin = new Padding(0, 5, 15, 0);
            radSelect.Tag = acc;

            radSelect.CheckedChanged += (s, e) =>
            {
                if (radSelect.Checked)
                {
                    if (lastChecked != null && lastChecked != radSelect)
                    {
                        lastChecked.Checked = false;
                    }
                    lastChecked = radSelect;
                    cmbStatus.SelectedIndex = acc.acc_status == 0 ? 0 : 1;
                    txtReason.Text = Convert.ToString(acc.reason) ?? "";
                }
            };

            // Thông tin chính
            TableLayoutPanel table = new TableLayoutPanel();
            table.AutoSize = true;
            table.ColumnCount = 6;
            table.BackColor = Color.Transparent;
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));  // ID
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200)); // Username
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200)); // Name
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120)); // Role
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200)); // Status
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150)); // Date

            void AddCell(string text)
            {
                Label lbl = new Label()
                {
                    Text = text,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10),
                    AutoSize = true,
                    Dock = DockStyle.Fill
                };
                table.Controls.Add(lbl);
            }

            AddCell($"ID: {acc.ac_id}");
            AddCell($"Username: {acc.username}");
            AddCell($"Name: {acc.name}");
            AddCell($"Role: {(acc.role == 1 ? "Leader" : "Staff")}");
            AddCell($"Trạng thái: {(acc.acc_status == 0 ? "Hoạt động" : "Bị cấm")}");
            AddCell($"Ngày gia nhập: {acc.created_at?.ToString("dd/MM/yyyy")}");

            FlowLayoutPanel header = new FlowLayoutPanel();
            header.FlowDirection = FlowDirection.LeftToRight;
            header.AutoSize = true;
            header.Controls.Add(radSelect);
            header.Controls.Add(table);

            panel.Controls.Add(header);

            return panel;
        }
    }
}