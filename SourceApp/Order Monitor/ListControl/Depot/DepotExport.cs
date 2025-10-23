using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.Entity;
using System.Windows.Forms;
using Base_DAL.ContextDatabase;

namespace Order_Monitor.ListControl
{
    public partial class DepotExport : UserControl
    {
        private Panel mainContainer;
        private FlowLayoutPanel panelSelectedItems;

        private ComboBox cmbItems;
        private TextBox txtQuantity;
        private TextBox txtNote;
        private Button btnAdd;
        private Button btnSave;
        private Button btnCancel;
        Label lblQtyMax;

        private List<ExportItem> selectedItems = new List<ExportItem>();
        private List<depot> depotItems;

        public event Action BackButtonClicked;

        private int export_ByID;
        private decimal currentQuantityItem = 0;

        private Dictionary<int, decimal> itemQuantities = new Dictionary<int, decimal>();

        private class DepotItem
        {
            public depot Depot { get; set; }
            public string DisplayName => Depot?.item?.name ?? "Item ID: -1";
            public int ItemId => Depot?.item_id ?? 0;
            public decimal Quantity => Depot?.quantity ?? 0;
            public string UnitName => Depot?.item?.unit?.name ?? "";
        }

        public DepotExport(int loginID)
        {
            InitializeComponent();
            Label titleLabel = new Label();
            titleLabel.Font = new Font("Tahoma", 16, FontStyle.Bold);
            titleLabel.ForeColor = Color.White;
            titleLabel.Height = 50;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(titleLabel);

            export_ByID = loginID;

            // Main container
            mainContainer = new Panel();
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.BackColor = Color.FromArgb(60, 60, 60);
            mainContainer.Padding = new Padding(10);
            this.Controls.Add(mainContainer);

            InitializePanel();
            LoadDepotItems();
        }

        private void InitializePanel()
        {
            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.FromArgb(60, 60, 60);

            Label spacer = new Label();
            spacer.Dock = DockStyle.Top;
            spacer.Height = 45;
            spacer.Padding = new Padding(0, 0, 0, 5);

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

            panelSelectedItems = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20),
                BackColor = Color.FromArgb(55, 55, 55),
                WrapContents = true,
                AutoScrollMargin = new Size(10, 10),
                FlowDirection = FlowDirection.LeftToRight
            };

            Panel panelImport = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 250,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(50, 50, 50)
            };

            loadFormExport(panelImport);

            mainPanel.Controls.AddRange(new Control[]
            {
                panelSelectedItems,
                panelImport,
                buttonPanel,
                spacer
            });
            panelSelectedItems.BringToFront();

            mainContainer.Controls.Add(mainPanel);
        }

        private void loadFormExport(Panel panelImport)
        {
            int leftStart = 10;
            int topStart = 10;

            Font labelFont = new Font("Tahoma", 12);
            Font textBoxFont = new Font("Tahoma", 12);
            Font buttonFont = new Font("Tahoma", 12);

            Label lblSelectItem = new Label
            {
                Text = "Chọn Item:",
                ForeColor = Color.White,
                Left = leftStart,
                Top = topStart,
                Width = 150,
                Font = labelFont
            };

            cmbItems = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 250,
                Left = leftStart,
                Top = topStart + 30,
                Font = textBoxFont
            };

            lblQtyMax = new Label
            {
                Text = $"Còn lại: 0 Kilogram",
                ForeColor = Color.White,
                Left = leftStart,
                Top = topStart + 66,
                Width = 250,
                Font = labelFont
            };

            Label lblQty = new Label
            {
                Text = "Nhập số lượng:",
                ForeColor = Color.White,
                Left = leftStart,
                Top = topStart + 95,
                Width = 130,
                Font = labelFont
            };

            txtQuantity = new TextBox()
            {
                Width = 120,
                Left = leftStart + 130,
                Top = topStart + 95,
                Font = textBoxFont
            };

            Label lblNote = new Label
            {
                Text = "Ghi chú:",
                ForeColor = Color.White,
                Left = leftStart,
                Top = topStart + 125,
                Width = 130,
                Font = labelFont
            };

            txtNote = new TextBox()
            {
                Width = 250,
                Left = leftStart,
                Top = topStart + 150,
                Font = textBoxFont
            };

            btnAdd = new Button
            {
                Text = "Thêm",
                Left = leftStart,
                Top = topStart + 195,
                Width = 100,
                Height = 30,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = buttonFont
            };
            btnAdd.Click += BtnAdd_Click;

            btnSave = new Button
            {
                Text = "Lưu",
                Left = leftStart + 270,
                Top = topStart + 20,
                Width = 120,
                Height = 30,
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = buttonFont
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Hủy",
                Left = leftStart + 270,
                Top = topStart + 65,
                Width = 120,
                Height = 33,
                BackColor = Color.IndianRed,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = buttonFont
            };
            btnCancel.Click += (s, e) => ClearAll();

            panelImport.Controls.AddRange(new Control[]
            {
                lblSelectItem, cmbItems,
                lblQtyMax, lblQty, txtQuantity,
                lblNote, txtNote,
                btnAdd, btnSave, btnCancel
            });
        }

        private void LoadDepotItems()
        {
            using (var contextDB = new OrderMonitor())
            {
                depotItems = contextDB.depot
                    .Include(depot => depot.item.unit)
                    .Where(depot => depot.item.is_active)
                    .ToList();

                var wrappers = depotItems
                    .Select(depot => new DepotItem { Depot = depot })
                    .ToList();

                cmbItems.DataSource = wrappers;
                cmbItems.DisplayMember = "DisplayName";
                cmbItems.ValueMember = "ItemId";

                itemQuantities = depotItems.ToDictionary(depot => depot.item_id, depot => depot.quantity);
            }

            cmbItems.SelectedIndexChanged -= cmbItems_SelectedIndexChanged;
            cmbItems.SelectedIndexChanged += cmbItems_SelectedIndexChanged;

            UpdateCurrentItemQuantity();
        }

        private void UpdateCurrentItemQuantity()
        {
            var wrapper = cmbItems.SelectedItem as DepotItem;
            if (wrapper != null && wrapper.Depot != null)
            {
                int itemId = wrapper.ItemId;
                currentQuantityItem = itemQuantities.ContainsKey(itemId)
                    ? itemQuantities[itemId]
                    : 0;
                lblQtyMax.Text = $"Còn lại: {currentQuantityItem} {wrapper.UnitName}";
            }
            else
            {
                currentQuantityItem = 0;
                lblQtyMax.Text = "Còn lại: 0 Kilogram";
            }
        }

        private void cmbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCurrentItemQuantity();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtQuantity.Text, out decimal qty) || qty <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ!");
                return;
            }

            var depotItem = cmbItems.SelectedItem as DepotItem;
            if (depotItem == null)
            {
                MessageBox.Show("Vui lòng chọn Item hợp lệ!");
                return;
            }

            int itemId = depotItem.ItemId;
            decimal available = itemQuantities.ContainsKey(itemId) ? itemQuantities[itemId] : 0;

            if (qty > available)
            {
                MessageBox.Show($"Số lượng xuất vượt quá tồn kho! Hiện còn lại {available}", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            itemQuantities[itemId] = available - qty;

            // Thêm vào List Item xuất
            var existingItem = selectedItems.FirstOrDefault(i => i.ItemId == itemId);
            if (existingItem != null)
            {
                existingItem.Quantity += qty;
            }
            else
            {
                selectedItems.Add(new ExportItem
                {
                    ItemId = itemId,
                    ItemName = depotItem.DisplayName,
                    Quantity = qty,
                    Unit = depotItem.UnitName,
                    Note = txtNote.Text
                });
            }

            txtQuantity.Text = "";
            txtNote.Text = "";

            RenderSelectedItems();
            UpdateCurrentItemQuantity();
        }

        private void RenderSelectedItems()
        {
            panelSelectedItems.Controls.Clear();

            foreach (var ex in selectedItems.ToList())
            {
                Panel card = new Panel()
                {
                    Width = 250,
                    Height = 150,
                    BackColor = Color.FromArgb(70, 70, 70),
                    Margin = new Padding(10),
                    Padding = new Padding(10)
                };

                Label lblName = new Label()
                {
                    Text = ex.ItemName,
                    ForeColor = Color.White,
                    Font = new Font("Tahoma", 15, FontStyle.Bold),
                    Dock = DockStyle.Top,
                    Height = 35
                };

                Label lblQty = new Label()
                {
                    Text = $"Số lượng: {ex.Quantity} {ex.Unit}",
                    ForeColor = Color.LightGray,
                    Font = new Font("Tahoma", 9),
                    Dock = DockStyle.Top,
                    Height = 30
                };

                Label lblNote = new Label()
                {
                    Text = $"Note: {ex.Note}",
                    Font = new Font("Tahoma", 9, FontStyle.Bold),
                    ForeColor = Color.LightGray,
                    Font = new Font("Tahoma", 9),
                    Dock = DockStyle.Top,
                    Height = 30
                };

                Label lblNote = new Label()
                {
                    Text = $"Note: {ex.Note}",
                    Font = new Font("Tahoma", 8, FontStyle.Bold),
                    ForeColor = Color.LightGray,
                    Dock = DockStyle.Top,
                    Height = 50
                };

                Button btnRemove = new Button()
                {
                    Text = "Xóa",
                    Width = 70,
                    Height = 30,
                    BackColor = Color.IndianRed,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Dock = DockStyle.Bottom
                };

                btnRemove.Click += (s, e) =>
                {
                    if (ex.ItemId.HasValue && itemQuantities.ContainsKey(ex.ItemId.Value))
                    {
                        itemQuantities[ex.ItemId.Value] += ex.Quantity;
                    }

                    selectedItems.Remove(ex);

                    RenderSelectedItems();
                    UpdateCurrentItemQuantity();
                };

                card.Controls.Add(btnRemove);
                card.Controls.Add(lblNote);
                card.Controls.Add(lblQty);
                card.Controls.Add(lblName);

                panelSelectedItems.Controls.Add(card);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (selectedItems == null || selectedItems.Count == 0)
            {
                MessageBox.Show("Danh sách Xuất đang trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var contextDB = new OrderMonitor())
            {
                var export = new export
                {
                    export_date = DateTime.Now,
                    export_status = 0,
                    export_by = export_ByID
                };
                contextDB.export.Add(export);
                contextDB.SaveChanges();

                foreach (var item in selectedItems)
                {
                    int itemId = item.ItemId ?? 0;

                    var detail = new export_detail
                    {
                        export_id = export.export_id,
                        item_id = itemId,
                        quantity = item.Quantity,
                        note = item.Note
                    };
                    contextDB.export_detail.Add(detail);

                    var depot = contextDB.depot.FirstOrDefault(d => d.item_id == itemId);
                    if (depot != null)
                    {
                        depot.quantity = itemQuantities[itemId];
                    }
                }

                contextDB.SaveChanges();
                ClearAll();
                MessageBox.Show("Đã lưu phiếu Xuất kho thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ClearAll()
        {
            selectedItems.Clear();
            panelSelectedItems.Controls.Clear();
            txtQuantity.Text = "";
            txtNote.Text = "";
            LoadDepotItems();
        }
    }

    public class ExportItem
    {
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public string Note { get; set; }
    }
}
