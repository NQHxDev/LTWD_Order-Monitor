using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn_LTW
{
    public partial class Form1 : Form
    {
        private ListControl.DanhSachOder danhSachOrder;
        private ListControl.DangThucHien dangThucHien;
        private ListControl.Kho dsKho;
        private ListControl.QuanLy quanLy;

        public Form1()
        {
            InitializeComponent();
            initControls();

            WebSocketManager.Connect();
            WebSocketManager.OnMessageReceived += HandleMessage;
            DataCache.Initialize();
            ShowPanel("DanhSach");
        }

        private void HandleMessage(string msg)
        {
            Console.WriteLine("Form1 nhận: " + msg);
        }

        private void initControls()
        {
            // Khởi tạo các UserControls
            danhSachOrder = new ListControl.DanhSachOder();
            dangThucHien = new ListControl.DangThucHien();
            dsKho = new ListControl.Kho();
            quanLy = new ListControl.QuanLy();

            danhSachOrder.Dock = DockStyle.Fill;
            dangThucHien.Dock = DockStyle.Fill;
            dsKho.Dock = DockStyle.Fill;
            quanLy.Dock = DockStyle.Fill;

            panel1.Controls.Add(danhSachOrder);
            panel1.Controls.Add(dangThucHien);
            panel1.Controls.Add(dsKho);
            panel1.Controls.Add(quanLy);
        }


        private void highlightButton(Button selectedButton)
        {
            btnDanhSach.BackColor = SystemColors.Control;
            btnDangThucHien.BackColor = SystemColors.Control;
            btnKho.BackColor = SystemColors.Control;
            btnQuanLy.BackColor = SystemColors.Control;

            // Highlight button được chọn
            selectedButton.BackColor = Color.LightBlue;
        }

        private void ShowPanel(string panelName)
        {
            danhSachOrder.Visible = false;
            dangThucHien.Visible = false;
            dsKho.Visible = false;
            quanLy.Visible = false;

            switch (panelName)
            {
                case "DanhSach":
                    danhSachOrder.Visible = true;
                    lblTitle.Text = "Danh sách Order";
                    highlightButton(btnDanhSach);
                    break;
                case "DangThucHien":
                    dangThucHien.Visible = true;
                    lblTitle.Text = "Đang Thực Hiện";
                    highlightButton(btnDangThucHien);
                    break;
                case "Kho":
                    dsKho.Visible = true;
                    lblTitle.Text = "Quản lý Kho";
                    highlightButton(btnKho);
                    break;
                case "QuanLy":
                    quanLy.Visible = true;
                    lblTitle.Text = "Quản lý Bếp";
                    highlightButton(btnQuanLy);
                    break;
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            ShowPanel("DanhSach");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            switch (clickedButton.Text)
            {
                case "Danh Sách":
                    ShowPanel("DanhSach");
                    break;
                case "Đang Thực Hiện":
                    ShowPanel("DangThucHien");
                    break;
                case "Kho":
                    ShowPanel("Kho");
                    break;
                case "Quản Lý":
                    ShowPanel("QuanLy");
                    break;
            }
        }
    }
}
