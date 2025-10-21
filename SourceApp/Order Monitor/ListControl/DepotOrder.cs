using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Data.Entity;
using System.Windows.Forms;
using Base_DAL.ContextDatabase;
using Base_BUS;

namespace Order_Monitor.ListControl
{
    public partial class DepotOrder : UserControl
    {
        private FoodServices foodServices = new FoodServices();

        private Panel mainContainer;
        private FlowLayoutPanel panelSelectedItems;

        private ComboBox cmbItems;
        private TextBox txtNewItemName;
        private ComboBox cmbUnits;
        private TextBox txtQuantity;
        private Button btnAdd;
        private Button btnSave;
        private Button btnCancel;

        private List<DepotOrderItem> selectedItems = new List<DepotOrderItem>();
        private List<item> availableItems;

        public event Action BackButtonClicked;

        private int created_ByID;

        public DepotOrder(int loginID)
        {
            InitializeComponent();

            // Main container
            mainContainer = new Panel();
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.BackColor = Color.FromArgb(60, 60, 60);
            mainContainer.Padding = new Padding(10);
            this.Controls.Add(mainContainer);

            created_ByID = loginID;

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

            loadFormImport(panelImport);

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

        private void loadFormImport(Panel panelImport)
        {
            int leftStart = 10;
            int topStart = 10;

            Font labelFont = new Font("Tahoma", 12);
            Font textBoxFont = new Font("Tahoma", 12);
            Font buttonFont = new Font("Tahoma", 12);

            Label lblNewItem = new Label
            {
                Text = "Tên Item mới:",
                ForeColor = Color.White,
                Left = leftStart,
                Top = topStart,
                Width = 250,
                Font = labelFont
            };

            txtNewItemName = new TextBox
            {
                Width = 250,
                Left = leftStart,
                Top = topStart + 20,
                Font = textBoxFont
            };

            Label lblSelectItem = new Label
            {
                Text = "Chọn Item:",
                ForeColor = Color.White,
                Left = leftStart,
                Top = topStart + 50,
                Width = 150,
                Font = labelFont
            };

            cmbItems = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 250,
                Left = leftStart,
                Top = topStart + 75,
                Font = textBoxFont
            };

            Label lblUnit = new Label
            {
                Text = "Đơn vị:",
                ForeColor = Color.White,
                Left = leftStart + 130,
                Top = topStart + 110,
                Width = 100,
                Font = labelFont
            };

            cmbUnits = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 120,
                Left = leftStart + 130,
                Top = topStart + 135,
                Font = textBoxFont
            };

            Label lblQty = new Label
            {
                Text = "Số lượng:",
                ForeColor = Color.White,
                Left = leftStart,
                Top = topStart + 110,
                Width = 130,
                Font = labelFont
            };

            txtQuantity = new TextBox()
            {
                Width = 115,
                Left = leftStart,
                Top = topStart + 135,
                Font = textBoxFont
            };

            btnAdd = new Button
            {
                Text = "Thêm",
                Left = leftStart,
                Top = topStart + 190,
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
                lblNewItem, txtNewItemName,
                lblSelectItem, cmbItems,
                lblUnit, cmbUnits,
                lblQty, txtQuantity,
                btnAdd, btnSave, btnCancel
            });
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

            DepotOrderItem existingItem = null;

            if (!string.IsNullOrWhiteSpace(txtNewItemName.Text))
            {
                // Item mới
                itemName = txtNewItemName.Text.Trim();
                unitId = (int)cmbUnits.SelectedValue;

                existingItem = selectedItems
                    .FirstOrDefault(i => i.IsNew &&
                                         i.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase) &&
                                         i.UnitId == unitId);

                if (existingItem != null)
                {
                    existingItem.Quantity += qty;
                }
                else
                {
                    selectedItems.Add(new DepotOrderItem
                    {
                        ItemName = itemName,
                        UnitId = unitId,
                        Quantity = qty,
                        IsNew = true
                    });
                }
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

                existingItem = selectedItems
                    .FirstOrDefault(i => !i.IsNew && i.ItemId == itemId);

                if (existingItem != null)
                {
                    existingItem.Quantity += qty;
                }
                else
                {
                    selectedItems.Add(new DepotOrderItem
                    {
                        ItemId = itemId,
                        ItemName = itemName,
                        UnitId = unitId,
                        Quantity = qty,
                        IsNew = false
                    });
                }
            }

            txtNewItemName.Text = "";
            txtQuantity.Text = "";

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
                    Text = ex.ItemName + (ex.IsNew ? " [New]" : ""),
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
                    Text = "Đơn vị: " + foodServices.GetUnitName(ex.UnitId),
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (selectedItems == null || selectedItems.Count == 0)
            {
                MessageBox.Show("Danh sách Nhập đang trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var contextDB = new OrderMonitor())
            {
                var import = new import
                {
                    create_at = DateTime.Now,
                    import_status = 0,
                    created_by = created_ByID
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

    public class DepotOrderItem
    {
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
        public bool IsNew { get; set; }
    }
}
