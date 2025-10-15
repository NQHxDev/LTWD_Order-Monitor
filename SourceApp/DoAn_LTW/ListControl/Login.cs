using DoAn_LTW.ContextDatabase;
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
    public partial class Login : UserControl
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblUser;
        private Label lblPass;

        private OrderMonitor DB;
        public int RequiredRole { get; set; } = 0;

        public event Action<account> OnLoginSuccess;

        public event Action<account, bool> OnLoginAttempt;
        
        public Login()
        {
            InitializeComponent();
            Label titleLabel = new Label();
            titleLabel.Font = new Font("Tahoma", 16, FontStyle.Bold);
            titleLabel.ForeColor = Color.White;
            titleLabel.Height = 50;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(titleLabel);

            DB = new OrderMonitor();
            InitializePanel();
        }

        private void InitializePanel()
        {
            lblUser = new Label();
            lblUser.Text = "Tên đăng nhập:";
            lblUser.ForeColor = Color.White;
            lblUser.Font = new Font("Tahoma", 11, FontStyle.Regular);
            lblUser.AutoSize = true;
            lblUser.Location = new Point(100, 120);
            this.Controls.Add(lblUser);

            txtUsername = new TextBox();
            txtUsername.Location = new Point(250, 115);
            txtUsername.Width = 200;
            txtUsername.Font = new Font("Tahoma", 11);
            this.Controls.Add(txtUsername);

            lblPass = new Label();
            lblPass.Text = "Mật khẩu:";
            lblPass.ForeColor = Color.White;
            lblPass.Font = new Font("Tahoma", 11, FontStyle.Regular);
            lblPass.AutoSize = true;
            lblPass.Location = new Point(100, 170);
            this.Controls.Add(lblPass);

            txtPassword = new TextBox();
            txtPassword.Location = new Point(250, 165);
            txtPassword.Width = 200;
            txtPassword.Font = new Font("Tahoma", 11);
            txtPassword.PasswordChar = '●';
            this.Controls.Add(txtPassword);

            btnLogin = new Button();
            btnLogin.Text = "Đăng nhập";
            btnLogin.Font = new Font("Tahoma", 11, FontStyle.Bold);
            btnLogin.BackColor = Color.FromArgb(0, 122, 204);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Width = 150;
            btnLogin.Height = 40;
            btnLogin.Location = new Point(250, 220);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += BtnLogin_Click;
            this.Controls.Add(btnLogin);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var userLogin = DB.account.FirstOrDefault(a => a.username == username && a.password == password);
                if (userLogin != null)
                {
                    bool isValidRole = CheckUserRole(userLogin);

                    if (isValidRole)
                    {
                        string userRole = userLogin.role == 1 ? "Admin" : "Staff";
                        DataCache.Login(userLogin, userRole);

                        txtUsername.Text = "";
                        txtPassword.Text = "";

                        OnLoginSuccess?.Invoke(userLogin);
                    }
                    else
                    {
                        MessageBox.Show("Bạn không có quyền truy cập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    OnLoginAttempt?.Invoke(userLogin, isValidRole);
                }
                else
                {
                    MessageBox.Show("Tài khoản, Mật khẩu sai!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi hệ thống: " + ex.Message);
                OnLoginAttempt?.Invoke(null, false);
            }
        }

        private bool CheckUserRole(account userLogin)
        {
            if (userLogin.role == 1)
                return true;

            if (RequiredRole == 0 && userLogin.role == 0)
                return true;

            return false;
        }
    }
}
