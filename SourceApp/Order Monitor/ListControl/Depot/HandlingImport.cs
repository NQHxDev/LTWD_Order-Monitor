using Base_BUS;
using Base_DAL.ContextDatabase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Order_Monitor.ListControl.Depot
{
    public partial class HandlingImport : UserControl
    {
        private Panel mainContainer;
        private FlowLayoutPanel flowPanel;
        private Panel headerPanel;

        public event Action BackButtonClicked;

        private int import_ByID;
        private int orderImportID;

        private List<TempImportDetail> confirmedItems = new List<TempImportDetail>();

        public HandlingImport(int loginID, int import_ID)
        {
            InitializeComponent();

            mainContainer = new Panel();
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.BackColor = Color.FromArgb(60, 60, 60);
            mainContainer.Padding = new Padding(10);
            this.Controls.Add(mainContainer);

            import_ByID = loginID;
            orderImportID = import_ID;

            InitializePanel();
        }

        private class TempImportDetail
        {
            public int DetailId { get; set; }
            public decimal Quantity { get; set; }
            public decimal UnitPrice { get; set; }
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
            btnBack.Margin = new Padding(10, 10, 10, 10);
            btnBack.BackColor = Color.FromArgb(0, 122, 204);
            btnBack.ForeColor = Color.White;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Click += (s, e) => BackButtonClicked?.Invoke();

            Button btnSaveImport = new Button();
            btnSaveImport.Text = "Hoàn thành Nhập";
            btnSaveImport.Size = new Size(150, 35);
            btnSaveImport.Margin = new Padding(10, 10, 10, 10);
            btnSaveImport.BackColor = Color.FromArgb(0, 150, 100);
            btnSaveImport.ForeColor = Color.White;
            btnSaveImport.FlatStyle = FlatStyle.Flat;
            btnSaveImport.Click += (s, e) => SaveImportDetail();

            buttonPanel.Controls.AddRange(new Control[] { btnBack, btnSaveImport });

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

            LoadOrderImportDetails();
        }

        private void LoadOrderImportDetails()
        {
            flowPanel.Controls.Clear();

            var importInfo = DepotServices.Instance.GetImportByID(orderImportID);

            string stringTitleDetail = "Chi tiết:\n";
            if (importInfo != null)
            {
                stringTitleDetail += $" - Mã đơn: #{importInfo.import_id}\n";
                stringTitleDetail += $" - Người tạo: {importInfo.account?.name ?? "N/A"}\n";
                stringTitleDetail += $" - Ngày tạo: {importInfo.create_at:dd/MM/yyyy}";
            }

            headerPanel = new Panel
            {
                Height = 250,
                BackColor = Color.FromArgb(35, 35, 38),
                Margin = new Padding(5, 5, 5, 15),
                Padding = new Padding(10),
                AutoSize = false
            };

            Label lblTitle = new Label
            {
                Text = "Đơn Nhập Hàng",
                AutoSize = false,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Tahoma", 13, FontStyle.Bold),
                ForeColor = Color.White
            };

            Label lblTitleDetail = new Label
            {
                Text = stringTitleDetail,
                AutoSize = false,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Tahoma", 9),
                ForeColor = Color.White,
                Height = 150
            };

            headerPanel.Controls.Add(lblTitleDetail);
            headerPanel.Controls.Add(lblTitle);
            flowPanel.Controls.Add(headerPanel);

            var listItemImport = DepotServices.Instance.GetImportDetailByID(orderImportID);

            if (listItemImport == null || listItemImport.Count == 0)
            {
                Label lblEmpty = new Label
                {
                    Text = "Không có nguyên liệu nào trong đơn nhập này.",
                    Font = new Font("Tahoma", 12, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Width = flowPanel.Width - 40,
                    Height = 100,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                flowPanel.Controls.Add(lblEmpty);
                return;
            }

            int soThuTu = 1;
            foreach (var detail in listItemImport)
            {
                Panel card = new Panel
                {
                    Width = 260,
                    Height = 200,
                    BackColor = Color.FromArgb(45, 45, 45),
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10),
                    Padding = new Padding(10),
                    Cursor = Cursors.Hand
                };

                Label lblName = new Label
                {
                    Text = $"[{soThuTu}] - {detail.item.name}",
                    Font = new Font("Tahoma", 11, FontStyle.Bold),
                    ForeColor = Color.White,
                    AutoSize = false,
                    Dock = DockStyle.Top,
                    Height = 25
                };
                soThuTu++;

                Label lblQuantity = new Label
                {
                    Text = $"Số lượng đặt: {detail.quantity}",
                    Font = new Font("Tahoma", 9, FontStyle.Italic),
                    ForeColor = Color.Gainsboro,
                    Dock = DockStyle.Top,
                    Height = 20
                };

                Panel controlPanel = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 120,
                    Padding = new Padding(10),
                    BackColor = Color.FromArgb(50, 50, 50)
                };

                Label lblNhap = new Label
                {
                    Text = "SL nhập:",
                    ForeColor = Color.White,
                    Font = new Font("Tahoma", 10),
                    AutoSize = true,
                    Location = new Point(10, 15)
                };

                NumericUpDown txtQty = new NumericUpDown
                {
                    Minimum = 0,
                    Maximum = 99999,
                    DecimalPlaces = 2,
                    Value = detail.quantity,
                    Location = new Point(80, 10),
                    Width = 120,
                    BackColor = Color.FromArgb(40, 40, 40),
                    ForeColor = Color.White,
                    Tag = detail
                };

                Label lblPrice = new Label
                {
                    Text = "Giá:",
                    ForeColor = Color.White,
                    Font = new Font("Tahoma", 10),
                    AutoSize = true,
                    Location = new Point(10, 55)
                };

                NumericUpDown txtPrice = CreatePriceItem(detail.item.import_price);
                txtPrice.Location = new Point(80, 50);
                txtPrice.Width = 120;

                Button btnConfirm = new Button
                {
                    Text = "Xác nhận",
                    Width = 100,
                    Height = 30,
                    Location = new Point(10, 85),
                    BackColor = Color.FromArgb(0, 150, 100),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };

                btnConfirm.Click += (s, e) =>
                {
                    var d = (import_detail)txtQty.Tag;
                    var realQty = txtQty.Value;
                    var price = txtPrice.Value;

                    if (realQty > d.quantity)
                    {
                        MessageBox.Show($"Số lượng nhập của {d.item.name} vượt quá đặt hàng!",
                            "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var existing = confirmedItems.FirstOrDefault(x => x.DetailId == d.im_detl_id);
                    if (existing != null)
                    {
                        existing.Quantity = realQty;
                        existing.UnitPrice = price;
                    }
                    else
                    {
                        confirmedItems.Add(new TempImportDetail
                        {
                            DetailId = d.im_detl_id,
                            Quantity = realQty,
                            UnitPrice = price
                        });
                    }

                    btnConfirm.Enabled = false;
                    btnConfirm.BackColor = Color.Gray;
                };

                txtQty.ValueChanged += (s, e) => { btnConfirm.Enabled = true; btnConfirm.BackColor = Color.FromArgb(0, 150, 100); };
                txtPrice.ValueChanged += (s, e) => { btnConfirm.Enabled = true; btnConfirm.BackColor = Color.FromArgb(0, 150, 100); };

                controlPanel.Controls.Add(lblNhap);
                controlPanel.Controls.Add(txtQty);
                controlPanel.Controls.Add(lblPrice);
                controlPanel.Controls.Add(txtPrice);
                controlPanel.Controls.Add(btnConfirm);

                card.Controls.Add(controlPanel);
                card.Controls.Add(lblQuantity);
                card.Controls.Add(lblName);

                flowPanel.Controls.Add(card);
            }
        }

        private void SaveImportDetail()
        {
            if (!confirmedItems.Any())
            {
                MessageBox.Show("Chưa có nguyên liệu nào được xác nhận!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var context = new OrderMonitor())
            {
                var import = context.import.FirstOrDefault(x => x.import_id == orderImportID);
                if (import == null) return;

                decimal totalPrice = 0;

                foreach (var temp in confirmedItems)
                {
                    var detailDb = context.import_detail.FirstOrDefault(x => x.im_detl_id == temp.DetailId);
                    if (detailDb == null) continue;

                    detailDb.unit_price = temp.UnitPrice;
                    detailDb.quantity = temp.Quantity;
                    detailDb.import_date = DateTime.Now;
                    detailDb.import_by = import_ByID;

                    var itemDb = context.item.FirstOrDefault(x => x.item_id == detailDb.item_id);
                    if (itemDb != null)
                    {
                        itemDb.quantity += (int)temp.Quantity;
                        itemDb.import_price = temp.UnitPrice;
                    }

                    totalPrice += temp.UnitPrice * temp.Quantity;
                }

                import.import_status = 2;
                import.total_price = totalPrice;
                import.update_by = import_ByID;
                context.SaveChanges();
            }

            MessageBox.Show("Đã hoàn thành nhập hàng!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            BackButtonClicked?.Invoke();
        }

        private NumericUpDown CreatePriceItem(decimal price)
        {
            var numericUpDown = new NumericUpDown()
            {
                Minimum = 0,
                Maximum = 1000000000,
                DecimalPlaces = 0,
                Increment = 100,
                Width = 100,
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                Value = price > 0 ? price : 0
            };

            try
            {
                decimal safeValue = decimal.Truncate(Math.Min(Math.Max(price, 0), 1000000000));
                numericUpDown.Value = safeValue;
            }
            catch
            {
                numericUpDown.Value = 0;
            }

            return numericUpDown;
        }
    }
}
