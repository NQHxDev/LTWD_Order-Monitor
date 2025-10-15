using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Data.Entity;
using System.Windows.Forms;
using Order_Monitor.ContextDatabase;

namespace Order_Monitor.ListControl
{
    public partial class DepotOrder : UserControl
    {
        private Panel mainContainer;
        private FlowLayoutPanel panelSelectedItems;

        private ComboBox cmbItems;
        private TextBox txtNewItemName;
        private ComboBox cmbUnits;
        private TextBox txtQuantity;
        private Button btnAdd;
        private Button btnSave;
        private Button btnCancel;

        private List<ExportItem> selectedItems = new List<ExportItem>();
        private List<item> availableItems;

        public event Action BackButtonClicked;

        public DepotOrder()
        {
            InitializeComponent();

            // Main container
            mainContainer = new Panel();
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.BackColor = Color.FromArgb(60, 60, 60);
            mainContainer.Padding = new Padding(10);
            this.Controls.Add(mainContainer);

            InitializePanel();
            LoadAvailableItems();
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
                WrapContents = true
            };

            Panel panelImport = new Panel()
            {
                Dock = DockStyle.Bottom,
                Height = 120,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(50, 50, 50)
            };

            loadFormImport(panelImport);

            mainPanel.Controls.Add(panelImport);
            mainPanel.Controls.Add(panelSelectedItems);
            mainPanel.Controls.Add(buttonPanel);
            mainPanel.Controls.Add(spacer);

            mainContainer.Controls.Add(mainPanel);
        }

        private void loadFormImport(Panel panelImport)
        {
            Label lblChoose = new Label()
            {
                Text = "Chọn Item hoặc nhập Item mới:",
                ForeColor = Color.White,
                Left = 10,
                Top = 10,
                Width = 300
            };

            cmbItems = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 200,
                Left = 10,
                Top = 35
            };

            txtNewItemName = new TextBox()
            {
                Width = 180,
                Left = 220,
                Top = 35
            };

            cmbUnits = new ComboBox()
            {
                Width = 100,
                Left = 410,
                Top = 35
            };

            txtQuantity = new TextBox()
            {
                Width = 100,
                Left = 520,
                Top = 35
            };

            btnAdd = new Button()
            {
                Text = "Thêm",
                Left = 630,
                Top = 33,
                Width = 80,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAdd.Click += BtnAdd_Click;

            btnSave = new Button()
            {
                Text = "Lưu",
                Left = 10,
                Top = 75,
                Width = 120,
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button()
            {
                Text = "Hủy",
                Left = 140,
                Top = 75,
                Width = 120,
                BackColor = Color.IndianRed,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += (s, e) => ClearAll();

            panelImport.Controls.Add(lblChoose);
            panelImport.Controls.Add(cmbItems);
            panelImport.Controls.Add(txtNewItemName);
            panelImport.Controls.Add(cmbUnits);
            panelImport.Controls.Add(txtQuantity);
            panelImport.Controls.Add(btnAdd);
            panelImport.Controls.Add(btnSave);
            panelImport.Controls.Add(btnCancel);
        }

        private void LoadAvailableItems()
        {
            using (var db = new OrderMonitor())
            {
                availableItems = db.item.Include(i => i.unit).Where(i => i.is_active).ToList();
                cmbItems.DataSource = availableItems;
                cmbItems.DisplayMember = "name";
                cmbItems.ValueMember = "item_id";

                cmbUnits.DataSource = db.unit.ToList();
                cmbUnits.DisplayMember = "abbreviation";
                cmbUnits.ValueMember = "unit_id";
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string itemName;
            int? itemId = null;
            int unitId;
            decimal qty;

            if (!decimal.TryParse(txtQuantity.Text, out qty) || qty <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ!");
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtNewItemName.Text))
            {
                // Item mới
                itemName = txtNewItemName.Text.Trim();
                unitId = (int)cmbUnits.SelectedValue;
                selectedItems.Add(new ExportItem { ItemName = itemName, UnitId = unitId, Quantity = qty, IsNew = true });
            }
            else
            {
                // Item có sẵn
                var selected = cmbItems.SelectedItem as item;
                if (selected == null)
                {
                    MessageBox.Show("Vui lòng chọn Item hợp lệ!");
                    return;
                }

                itemName = selected.name;
                itemId = selected.item_id;
                unitId = selected.unit_id;
                selectedItems.Add(new ExportItem { ItemId = itemId.Value, ItemName = itemName, UnitId = unitId, Quantity = qty, IsNew = false });
            }

            RenderSelectedItems();
        }

        private void RenderSelectedItems()
        {
            panelSelectedItems.Controls.Clear();

            foreach (var ex in selectedItems)
            {
                Panel card = new Panel()
                {
                    Width = 250,
                    Height = 120,
                    BackColor = Color.FromArgb(70, 70, 70),
                    Margin = new Padding(10),
                    Padding = new Padding(10)
                };

                Label lblName = new Label()
                {
                    Text = ex.ItemName + (ex.IsNew ? " *Mới" : ""),
                    ForeColor = Color.White,
                    Font = new Font("Tahoma", 12, FontStyle.Bold),
                    Dock = DockStyle.Top,
                    Height = 25
                };

                Label lblQty = new Label()
                {
                    Text = $"Số lượng: {ex.Quantity}",
                    ForeColor = Color.LightGray,
                    Dock = DockStyle.Top,
                    Height = 20
                };

                Label lblUnit = new Label()
                {
                    Text = "Đơn vị: " + GetUnitName(ex.UnitId),
                    ForeColor = Color.LightGray,
                    Dock = DockStyle.Top,
                    Height = 20
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
                    selectedItems.Remove(ex);
                    RenderSelectedItems();
                };

                card.Controls.Add(btnRemove);
                card.Controls.Add(lblUnit);
                card.Controls.Add(lblQty);
                card.Controls.Add(lblName);

                panelSelectedItems.Controls.Add(card);
            }
        }

        private string GetUnitName(int unitId)
        {
            return cmbUnits.Items.Cast<unit>().FirstOrDefault(u => u.unit_id == unitId)?.abbreviation ?? "";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            using (var contextDB = new OrderMonitor())
            {
                var import = new import
                {
                    create_at = DateTime.Now,
                    import_status = 0,
                    created_by = 1
                };
                contextDB.import.Add(import);
                contextDB.SaveChanges();

                foreach (var item in selectedItems)
                {
                    int itemId;

                    if (item.IsNew)
                    {
                        var newItem = new item
                        {
                            name = item.ItemName,
                            unit_id = item.UnitId,
                            import_price = 0,
                            is_active = true,
                            quantity = 0
                        };
                        contextDB.item.Add(newItem);
                        contextDB.SaveChanges();
                        itemId = newItem.item_id;
                    }
                    else
                    {
                        itemId = item.ItemId ?? 0;
                    }

                    var detail = new import_detail
                    {
                        import_id = import.import_id,
                        item_id = itemId,
                        quantity = item.Quantity
                    };
                    contextDB.import_detail.Add(detail);
                }

                contextDB.SaveChanges();
                ClearAll();
            }
        }

        private void ClearAll()
        {
            selectedItems.Clear();
            panelSelectedItems.Controls.Clear();
            txtNewItemName.Text = "";
            txtQuantity.Text = "";
        }
    }

    public class ExportItem
    {
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
        public bool IsNew { get; set; }
    }
}
