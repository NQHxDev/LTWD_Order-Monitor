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

namespace Order_Monitor.ListControl.Manager
{
    public partial class ViewDepot : UserControl
    {
        private Panel mainContainer;
        private FlowLayoutPanel flowPanel;

        public event Action BackButtonClicked;

        private int leader_ID;

        public ViewDepot(int leaderID)
        {
            InitializeComponent();
            Label titleLabel = new Label();
            titleLabel.Font = new Font("Tahoma", 16, FontStyle.Bold);
            titleLabel.ForeColor = Color.White;
            titleLabel.Height = 50;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(titleLabel);

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

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel();
            buttonPanel.Height = 60;
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.BackColor = Color.FromArgb(60, 60, 60);
            buttonPanel.FlowDirection = FlowDirection.LeftToRight;
            buttonPanel.WrapContents = false;
            buttonPanel.Padding = new Padding(10, 10, 10, 10);
            buttonPanel.AutoScroll = true;

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
            flowPanel.AutoScroll = false;

            Label spacer = new Label();
            spacer.Dock = DockStyle.Top;
            spacer.Height = 45;
            spacer.Padding = new Padding(0, 0, 0, 5);

            mainPanel.Controls.Add(flowPanel);
            mainPanel.Controls.Add(buttonPanel);
            mainPanel.Controls.Add(spacer);

            mainContainer.Controls.Add(mainPanel);

            LoadDepotItems();
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
    }
}
