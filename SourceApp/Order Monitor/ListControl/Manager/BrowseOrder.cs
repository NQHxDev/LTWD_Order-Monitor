using Base_BUS;
using Base_DAL.ContextDatabase;
using Newtonsoft.Json.Linq;
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
    public partial class BrowseOrder : UserControl
    {
        private Panel mainContainer;
        private Panel flowPanel;

        public event Action BackButtonClicked;

        private int leader_ID;

        public BrowseOrder(int leaderID)
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

            LoadOrdersImport();
        }

        private void LoadOrdersImport()
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

            var stats = DepotServices.Instance.GetImportStatistics();

            statCards.Controls.Add(CreateStatCard("Đơn chờ duyệt", stats.pending, Color.Orange));
            statCards.Controls.Add(CreateStatCard("Đơn đã duyệt hôm nay", stats.approvedToday, Color.Green));
            statCards.Controls.Add(CreateStatCard("Đơn bị từ chối hôm nay", stats.rejectedToday, Color.Red));
            
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

            var orders = DepotServices.Instance.GetListOrderImportUnconfirmed();

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

        private Panel CreateStatCard(string title, int count, Color color)
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
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblCount = new Label
            {
                Text = count.ToString(),
                ForeColor = color,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            card.Controls.Add(lblCount);
            card.Controls.Add(lblTitle);
            return card;
        }

        private Panel CreateOrderCard(import orderImport, Panel rightPanel)
        {
            string totalPriceOrder = "Số tiền ước tính";

            if (orderImport.import_status == 2)
            {
                totalPriceOrder = "Tổng tiền Đơn hàng";
            }

            Panel card = new Panel()
            {
                Width = 260,
                Height = 250,
                BackColor = Color.FromArgb(45, 45, 45),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10),
                Padding = new Padding(10),
                Cursor = Cursors.Hand
            };

            Label lblName = new Label()
            {
                Text = $"Mã đơn Đặt hàng: #{orderImport.import_id}",
                Font = new Font("Tahoma", 12, FontStyle.Bold),
                ForeColor = Color.White,
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
                $"Nhân viên Đặt hàng: {AccountServices.Instance.GetNameUser(orderImport.created_by)}\n\n" +
                $"Số lượng Nguyên liệu: {orderImport.total_item}\n" +
                $"{totalPriceOrder}: {orderImport.total_price:N0} VNĐ\n" +
                $"Ngày đặt đơn hàng: {orderImport.create_at?.ToString("dd/MM/yyyy") ?? ""}"
            };

            Button btnDetail = new Button()
            {
                Text = "Chi tiết",
                Font = new Font("Tahoma", 11, FontStyle.Bold),
                Dock = DockStyle.Bottom,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(3, 161, 252),
                Tag = orderImport.import_id
            };

            btnDetail.Click += (s, e) =>
            {
                LoadOrderDetail(orderImport.import_id, rightPanel);
            };

            card.Controls.Add(lblDetail);
            card.Controls.Add(btnDetail);
            card.Controls.Add(lblName);

            return card;
        }

        private void LoadOrderDetail(int importId, Panel rightPanel)
        {
            rightPanel.Controls.Clear();

            var order = DepotServices.Instance.GetImportByIDWithDetails(importId);

            if (order == null) return;

            Label lblHeader = new Label
            {
                Text = $"Chi tiết Đơn hàng #{order.import_id}",
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

            string statusText;
            switch (order.import_status)
            {
                case -1:
                    statusText = "❌ Từ chối";
                    break;
                case 0:
                    statusText = "⏳ Chờ duyệt";
                    break;
                case 1:
                    statusText = "⏳ Chờ nhập kho";
                    break;
                case 2:
                    statusText = "✅ Đã hoàn thành";
                    break;
                default:
                    statusText = "Không xác định";
                    break;
            }

            Color statusColor;
            switch (order.import_status)
            {
                case -1:
                    statusColor = Color.Red;
                    break;
                case 0:
                case 1:
                    statusColor = Color.Orange;
                    break;
                case 2:
                    statusColor = Color.LimeGreen;
                    break;
                default:
                    statusColor = Color.Gray;
                    break;
            }

            Label lblInfo = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10),
                Text =
                    $"👤 Người đặt: {order.account?.name ?? "Không rõ"}\n" +
                    $"📦 Trạng thái: ",
                AutoSize = false
            };

            Label lblStatus = new Label
            {
                Text = statusText,
                ForeColor = statusColor,
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
                    $"🕒 Ngày tạo: {order.create_at?.ToString("dd/MM/yyyy HH:mm") ?? "N/A"}\n" +
                    $"🗓️ Cập nhật gần nhất: {order.update_at?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa có"}"
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

            listView.Columns.Add("Tên Nguyên liệu", 180);
            listView.Columns.Add("Số lượng", 100);
            listView.Columns.Add("Đơn vị", 50);

            foreach (var detail in order.import_detail)
            {
                listView.Items.Add(new ListViewItem(new[]
                {
                    detail.item.name,
                    detail.quantity.ToString(),
                    detail.item.unit?.abbreviation ?? "N/A"
                }));
            }

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10)
            };

            Button btnApprove = new Button
            {
                Text = "Duyệt đơn",
                BackColor = Color.Green,
                ForeColor = Color.White,
                Width = 100,
                Height = 35,
                FlatStyle = FlatStyle.Flat
            };
            btnApprove.Click += (s, e) =>
            {
                order.import_status = 1;
                order.update_by = leader_ID;
                order.update_at = DateTime.Now;
                DepotServices.Instance.UpdateImportOrder(order);

                MessageBox.Show("✅ Đã duyệt đơn hàng thành công!");
                LoadOrdersImport();
            };

            Button btnReject = new Button
            {
                Text = "Từ chối đơn",
                BackColor = Color.Red,
                ForeColor = Color.White,
                Width = 100,
                Height = 35,
                FlatStyle = FlatStyle.Flat
            };
            btnReject.Click += (s, e) =>
            {
                string reason = HandleRejectOrder(order.import_id);
                if (reason == null)
                    return;

                order.import_status = -1;
                order.reason = reason;
                order.update_by = leader_ID;
                order.update_at = DateTime.Now;
                DepotServices.Instance.UpdateImportOrder(order);

                MessageBox.Show("❌ Đã từ chối đơn hàng", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadOrdersImport();
            };

            buttonPanel.Controls.Add(btnApprove);
            buttonPanel.Controls.Add(btnReject);

            rightPanel.Controls.Add(buttonPanel);
            rightPanel.Controls.Add(listView);
            rightPanel.Controls.Add(infoPanel);
            rightPanel.Controls.Add(lblHeader);
        }

        private string HandleRejectOrder(int importId)
        {
            string reason = null;

            using (var dialog = new Form()
            {
                Text = $"Từ chối đơn hàng #{importId}",
                Size = new Size(400, 220),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            })
            {
                var lbl = new Label()
                {
                    Text = "Nhập lý do từ chối:",
                    Location = new Point(20, 20),
                    Font = new Font("Segoe UI", 10),
                    Width = 340
                };

                var txtReason = new TextBox()
                {
                    Location = new Point(20, 50),
                    Size = new Size(340, 80),
                    Multiline = true,
                    Font = new Font("Segoe UI", 9),
                    ScrollBars = ScrollBars.Vertical
                };

                var btnOK = new Button()
                {
                    Text = "Xác nhận",
                    Location = new Point(280, 140),
                    Size = new Size(80, 30),
                    BackColor = Color.FromArgb(0, 123, 255),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };

                btnOK.Click += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txtReason.Text))
                    {
                        MessageBox.Show("Vui lòng nhập lý do từ chối!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    reason = txtReason.Text.Trim();
                    dialog.DialogResult = DialogResult.OK;
                };

                dialog.Controls.AddRange(new Control[] { lbl, txtReason, btnOK });
                dialog.AcceptButton = btnOK;

                if (dialog.ShowDialog() == DialogResult.OK)
                    return reason;
                else
                    return null;
            }
        }
    }
}
