using Base_BUS;
using Order_Monitor.ListControl.Depot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Order_Monitor.ListControl
{
    public partial class DepotImport : UserControl
    {
        private Panel mainContainer;
        private FlowLayoutPanel flowPanel;
        Button btnOrderImportStatus;

        public event Action BackButtonClicked;

        private int import_ByID;

        public DepotImport(int loginID)
        {
            InitializeComponent();

            // Main container
            mainContainer = new Panel();
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.BackColor = Color.FromArgb(60, 60, 60);
            mainContainer.Padding = new Padding(10);
            this.Controls.Add(mainContainer);

            import_ByID = loginID;

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

            flowPanel = new FlowLayoutPanel();
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.AutoScroll = true;
            flowPanel.WrapContents = true;
            flowPanel.FlowDirection = FlowDirection.LeftToRight;
            flowPanel.Padding = new Padding(10);
            flowPanel.AutoSize = false;
            flowPanel.AutoSizeMode = AutoSizeMode.GrowOnly;

            Label spacer = new Label();
            spacer.Dock = DockStyle.Top;
            spacer.Height = 45;
            spacer.Padding = new Padding(0, 0, 0, 5);

            mainPanel.Controls.Add(flowPanel);
            mainPanel.Controls.Add(buttonPanel);
            mainPanel.Controls.Add(spacer);

            mainContainer.Controls.Add(mainPanel);

            LoadOrderDepot();
        }

        private void LoadOrderDepot()
        {
            flowPanel.Controls.Clear();

            var listOrderImport = DepotServices.Instance.GetListOrderImport();

            foreach (var orderImport in listOrderImport)
            {
                string totalPriceOrder = "Số tiền ước tính";
                string statusTextButtom = "Chờ duyệt";
                Color statusColor = Color.FromArgb(133, 133, 133);

                if (orderImport.import_status == 2)
                {
                    totalPriceOrder = "Tổng tiền Đơn hàng";
                    statusTextButtom = "Hoàn thành";
                    statusColor = Color.LightGreen;
                }
                else if (orderImport.import_status == 1)
                {
                    statusTextButtom = "Nhập kho";
                    statusColor = Color.FromArgb(0, 122, 204);
                }

                Panel card = new Panel();
                card.Width = 260;
                card.Height = 250;
                card.BackColor = Color.FromArgb(45, 45, 45);
                card.BorderStyle = BorderStyle.FixedSingle;
                card.Margin = new Padding(10);
                card.Padding = new Padding(10);
                card.Cursor = Cursors.Hand;

                Label lblName = new Label();
                lblName.Text = $"Mã Đơn đặt hàng: #{orderImport.import_id}";
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
                    $"Nhân viên Đặt hàng: {AccountServices.Instance.GetNameUser(orderImport.created_by)}\n\n" +
                    $"Số lượng Nguyên liệu: {orderImport.total_item}\n" +
                    $"{totalPriceOrder}: {orderImport.total_price:N0} VNĐ\n" +
                    $"Ngày đặt đơn hàng: {orderImport.create_at?.ToString("dd/MM/yyyy") ?? ""}";

                btnOrderImportStatus = new Button()
                {
                    Text = statusTextButtom,
                    Font = new Font("Tahoma", 11, FontStyle.Bold),
                    Dock = DockStyle.Bottom,
                    Height = 40,
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = statusColor,
                };

                if(orderImport.import_status != 1)
                {
                    btnOrderImportStatus.Enabled = false;
                }
                btnOrderImportStatus.Click += (s, e) => HadlingImportPanel();

                card.Controls.Add(lblDetail);
                card.Controls.Add(btnOrderImportStatus);
                card.Controls.Add(lblName);

                flowPanel.Controls.Add(card);
            }

            if (flowPanel.Controls.Count == 0)
            {
                Label lblEmpty = new Label();
                lblEmpty.Text = "Không có nguyên liệu nào trong kho";
                lblEmpty.Font = new Font("Tahoma", 12, FontStyle.Italic);
                lblEmpty.ForeColor = Color.Gray;
                lblEmpty.Dock = DockStyle.Fill;
                lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
                flowPanel.Controls.Add(lblEmpty);
            }
        }

        private void HadlingImportPanel()
        {
            mainContainer.Visible = false;

            HandlingImport hadlingImport = new HandlingImport(import_ByID);
            hadlingImport.Dock = DockStyle.Fill;
            hadlingImport.BackButtonClicked += () =>
            {
                this.Controls.Remove(hadlingImport);
                mainContainer.Visible = true;
            };
            this.Controls.Add(hadlingImport);
        }
    }
}
