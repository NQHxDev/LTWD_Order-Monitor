using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAn_LTW.ContextDatabase;

namespace DoAn_LTW.ListControl
{
    public partial class MenuFood : UserControl
    {
        private List<food> foodList = new List<food>();
        private List<item> availableItems = new List<item>();
        private List<food_ingredient> currentIngredients = new List<food_ingredient>();

        // Controls Food list
        private DataGridView dgvFood;
        private Button btnAddNewFood;
        private Button btnEditFood;
        private Button btnDeleteFood;

        // Controls Food panel
        private Panel pnlFoodDetail;
        private TextBox txtFoodName;
        private TextBox txtFoodPrice;
        private TextBox txtFoodDescription;
        private Button btnSaveFood;
        private Button btnCancel;
        private ComboBox cmbFoodStatus;

        // Controls Food Ingredients
        private DataGridView dgvIngredients;
        private ComboBox cmbItems;
        private TextBox txtQuantity;
        private ComboBox cmbUnits;
        private Button btnAddIngredient;
        private Button btnRemoveIngredient;
        private Label lblRecipeTitle;

        private bool isEditing = false;
        private int editingFoodId = -1;

        public MenuFood()
        {
            InitializeComponent();
            InitializeData();
            InitializeControls();
            LoadFoodData();
        }

        private void InitializeData()
        {
            DataCache.Initialize();
            availableItems = DataCache.Items.Values.Where(i => i.is_active).ToList();
        }

        private void InitializeControls()
        {
            this.ForeColor = Color.White;
            this.Padding = new Padding(10);

            // Title Label
            Label titleLabel = new Label();
            titleLabel.Font = new Font("Tahoma", 16, FontStyle.Bold);
            titleLabel.ForeColor = Color.White;
            titleLabel.Height = 50;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(titleLabel);

            // Main container
            Panel mainContainer = new Panel();
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.BackColor = Color.FromArgb(60, 60, 60);
            mainContainer.Padding = new Padding(10);
            this.Controls.Add(mainContainer);

            // Food list section
            InitializeFoodListSection(mainContainer);

            // Food detail section
            InitializeFoodDetailSection(mainContainer);

            // Initially show food list, hide detail
            ShowFoodListSection();
        }

        private void InitializeFoodListSection(Panel container)
        {
            Panel foodListPanel = new Panel();
            foodListPanel.Dock = DockStyle.Fill;
            foodListPanel.BackColor = Color.FromArgb(60, 60, 60);
            foodListPanel.Name = "pnlFoodList";

            // Buttons panel
            Panel buttonPanel = new Panel();
            buttonPanel.Height = 50;
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.BackColor = Color.FromArgb(60, 60, 60);

            btnAddNewFood = new Button();
            btnAddNewFood.Text = "Thêm Món Mới";
            btnAddNewFood.Size = new Size(120, 35);
            btnAddNewFood.Location = new Point(0, 10);
            btnAddNewFood.BackColor = Color.FromArgb(0, 122, 204);
            btnAddNewFood.ForeColor = Color.White;
            btnAddNewFood.FlatStyle = FlatStyle.Flat;
            btnAddNewFood.Click += BtnAddNewFood_Click;

            btnEditFood = new Button();
            btnEditFood.Text = "Sửa Món";
            btnEditFood.Size = new Size(100, 35);
            btnEditFood.Location = new Point(130, 10);
            btnEditFood.BackColor = Color.FromArgb(0, 122, 204);
            btnEditFood.ForeColor = Color.White;
            btnEditFood.FlatStyle = FlatStyle.Flat;
            btnEditFood.Click += BtnEditFood_Click;

            btnDeleteFood = new Button();
            btnDeleteFood.Text = "Xóa Món";
            btnDeleteFood.Size = new Size(100, 35);
            btnDeleteFood.Location = new Point(240, 10);
            btnDeleteFood.BackColor = Color.FromArgb(200, 60, 60);
            btnDeleteFood.ForeColor = Color.White;
            btnDeleteFood.FlatStyle = FlatStyle.Flat;
            btnDeleteFood.Click += BtnDeleteFood_Click;

            buttonPanel.Controls.AddRange(new Control[] { btnAddNewFood, btnEditFood, btnDeleteFood });

            dgvFood = new DataGridView();
            dgvFood.Dock = DockStyle.Fill;
            dgvFood.BackgroundColor = Color.FromArgb(45, 45, 48);
            dgvFood.ForeColor = Color.Black;
            dgvFood.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFood.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFood.RowHeadersVisible = false;
            dgvFood.ReadOnly = true;
            dgvFood.RowTemplate.Height = 40;

            dgvFood.Columns.Add("food_id", "ID");
            dgvFood.Columns.Add("name", "Tên Món");
            dgvFood.Columns.Add("price", "Giá Bán");
            dgvFood.Columns.Add("description", "Mô Tả");
            dgvFood.Columns.Add("status", "Trạng Thái");

            dgvFood.Columns["food_id"].Width = 30;
            dgvFood.Columns["name"].Width = 100;
            dgvFood.Columns["price"].Width = 60;
            dgvFood.Columns["description"].Width = 180;
            dgvFood.Columns["status"].Width = 80;

            dgvFood.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvFood.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFood.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Bold);
            dgvFood.EnableHeadersVisualStyles = false;

            dgvFood.DefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvFood.DefaultCellStyle.ForeColor = Color.White;
            dgvFood.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvFood.DefaultCellStyle.SelectionForeColor = Color.White;

            Label spacer = new Label();
            spacer.Dock = DockStyle.Top;
            spacer.Height = 45;
            spacer.Padding = new Padding(0, 0, 0, 5);

            foodListPanel.Controls.Add(dgvFood);
            foodListPanel.Controls.Add(buttonPanel);
            foodListPanel.Controls.Add(spacer);

            container.Controls.Add(foodListPanel);
        }

        private void InitializeFoodDetailSection(Panel container)
        {
            pnlFoodDetail = new Panel();
            pnlFoodDetail.Dock = DockStyle.Fill;
            pnlFoodDetail.BackColor = Color.FromArgb(60, 60, 60);
            pnlFoodDetail.Visible = false;
            pnlFoodDetail.Name = "pnlFoodDetail";

            // Food info panel
            Panel foodInfoPanel = new Panel();
            foodInfoPanel.Height = 150;
            foodInfoPanel.Dock = DockStyle.Top;
            foodInfoPanel.BackColor = Color.FromArgb(70, 70, 70);
            foodInfoPanel.Padding = new Padding(10);

            Label lblFoodName = new Label() { Text = "Tên Món:", Location = new Point(10, 20), Size = new Size(80, 20) };
            txtFoodName = new TextBox() { Location = new Point(100, 17), Size = new Size(250, 25), BackColor = Color.FromArgb(45, 45, 48), ForeColor = Color.White };

            Label lblFoodPrice = new Label() { Text = "Giá Bán:", Location = new Point(10, 60), Size = new Size(80, 20) };
            txtFoodPrice = new TextBox() { Location = new Point(100, 57), Size = new Size(150, 25), BackColor = Color.FromArgb(45, 45, 48), ForeColor = Color.White };

            Label lblFoodDescription = new Label() { Text = "Mô Tả:", Location = new Point(10, 100), Size = new Size(80, 20) };
            txtFoodDescription = new TextBox() { Location = new Point(100, 97), Size = new Size(300, 25), BackColor = Color.FromArgb(45, 45, 48), ForeColor = Color.White };

            Label lblFoodStatus = new Label() { Text = "Trạng Thái:", Location = new Point(420, 20), Size = new Size(80, 20) };
            cmbFoodStatus = new ComboBox()
            {
                Location = new Point(510, 17),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White
            };

            var statusItems = new List<StatusItem>
            {
                new StatusItem { Text = "Hoạt động", Value = 0 },
                new StatusItem { Text = "Ngừng bán", Value = 1 }
            };

            cmbFoodStatus.DisplayMember = "Text";
            cmbFoodStatus.ValueMember = "Value";
            cmbFoodStatus.DataSource = statusItems;


            foodInfoPanel.Controls.AddRange(new Control[] { 
                lblFoodName,
                txtFoodName,
                lblFoodPrice,
                txtFoodPrice,
                lblFoodDescription,
                txtFoodDescription,
                lblFoodStatus,
                cmbFoodStatus
            });

            // Title
            lblRecipeTitle = new Label();
            lblRecipeTitle.Text = "Công thức món ăn:";
            lblRecipeTitle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            lblRecipeTitle.ForeColor = Color.White;
            lblRecipeTitle.Height = 40;
            lblRecipeTitle.Dock = DockStyle.Top;
            lblRecipeTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblRecipeTitle.Padding = new Padding(10, 0, 0, 0);

            // Ingredients list
            dgvIngredients = new DataGridView();
            dgvIngredients.Height = 200;
            dgvIngredients.Dock = DockStyle.Top;
            dgvIngredients.BackgroundColor = Color.FromArgb(45, 45, 48);
            dgvIngredients.ForeColor = Color.Black;
            dgvIngredients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvIngredients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIngredients.RowHeadersVisible = false;
            dgvIngredients.ReadOnly = true;

            dgvIngredients.Columns.Add("item_name", "Nguyên Liệu");
            dgvIngredients.Columns.Add("quantity", "Số Lượng");
            dgvIngredients.Columns.Add("unit_name", "Đơn Vị");

            dgvIngredients.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgvIngredients.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvIngredients.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Bold);
            dgvIngredients.EnableHeadersVisualStyles = false;

            dgvIngredients.DefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgvIngredients.DefaultCellStyle.ForeColor = Color.White;
            dgvIngredients.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvIngredients.DefaultCellStyle.SelectionForeColor = Color.White;

            // Add ingredients panel
            Panel addIngredientPanel = new Panel();
            addIngredientPanel.Height = 100;
            addIngredientPanel.Dock = DockStyle.Top;
            addIngredientPanel.BackColor = Color.FromArgb(70, 70, 70);
            addIngredientPanel.Padding = new Padding(10);

            Label lblItem = new Label() { Text = "Nguyên Liệu:", Location = new Point(10, 15), Size = new Size(80, 20) };
            cmbItems = new ComboBox()
            {
                Location = new Point(100, 12),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                DisplayMember = "name",
                ValueMember = "item_id",
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White
            };

            Label lblQuantity = new Label() { Text = "Số Lượng:", Location = new Point(10, 50), Size = new Size(80, 20) };
            txtQuantity = new TextBox() { Location = new Point(100, 47), Size = new Size(100, 25), BackColor = Color.FromArgb(45, 45, 48), ForeColor = Color.White };

            Label lblUnit = new Label() { Text = "Đơn Vị:", Location = new Point(210, 50), Size = new Size(50, 20) };
            cmbUnits = new ComboBox()
            {
                Location = new Point(270, 47),
                Size = new Size(100, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White
            };

            btnAddIngredient = new Button()
            {
                Text = "Thêm",
                Location = new Point(380, 45),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAddIngredient.Click += BtnAddIngredient_Click;

            btnRemoveIngredient = new Button()
            {
                Text = "Xóa",
                Location = new Point(470, 45),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(200, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRemoveIngredient.Click += BtnRemoveIngredient_Click;

            addIngredientPanel.Controls.AddRange(new Control[] {
                lblItem, cmbItems, lblQuantity, txtQuantity, lblUnit, cmbUnits, btnAddIngredient, btnRemoveIngredient
            });

            // Action buttons panel
            Panel actionPanel = new Panel();
            actionPanel.Height = 60;
            actionPanel.Dock = DockStyle.Bottom;
            actionPanel.BackColor = Color.FromArgb(60, 60, 60);

            btnSaveFood = new Button();
            btnSaveFood.Text = "Lưu Món";
            btnSaveFood.Size = new Size(100, 35);
            btnSaveFood.Location = new Point(300, 12);
            btnSaveFood.BackColor = Color.FromArgb(0, 122, 204);
            btnSaveFood.ForeColor = Color.White;
            btnSaveFood.FlatStyle = FlatStyle.Flat;
            btnSaveFood.Click += BtnSaveFood_Click;

            btnCancel = new Button();
            btnCancel.Text = "Hủy";
            btnCancel.Size = new Size(100, 35);
            btnCancel.Location = new Point(410, 12);
            btnCancel.BackColor = Color.FromArgb(100, 100, 100);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Click += BtnCancel_Click;

            actionPanel.Controls.AddRange(new Control[] { btnSaveFood, btnCancel });

            Label spacer = new Label();
            spacer.Dock = DockStyle.Top;
            spacer.Height = 45;
            spacer.Padding = new Padding(0, 0, 0, 5);

            pnlFoodDetail.Controls.AddRange(new Control[] {
                actionPanel, addIngredientPanel, dgvIngredients, lblRecipeTitle, foodInfoPanel, spacer
            });

            container.Controls.Add(pnlFoodDetail);
        }

        private void LoadFoodData()
        {
            dgvFood.Rows.Clear();
            foodList = DataCache.Foods.Values.ToList();

            foreach (var food in foodList)
            {
                string statusText = food.status == 0 ? "Hoạt động" : "Ngừng bán";
                dgvFood.Rows.Add(food.food_id, food.name, $"{food.price:N0} .000 VNĐ", food.description, statusText);
            }

            // Load available items for recipe
            cmbItems.DataSource = availableItems;
            cmbItems.DisplayMember = "name";
            cmbItems.ValueMember = "item_id";

            // Load units
            var units = DataCache.Units.Values.ToList();
            cmbUnits.DataSource = units;
            cmbUnits.DisplayMember = "name";
            cmbUnits.ValueMember = "unit_id";
        }

        private void ShowFoodListSection()
        {
            foreach (Control control in this.Controls[1].Controls)
            {
                if (control.Name == "pnlFoodList")
                    control.Visible = true;
                else if (control.Name == "pnlFoodDetail")
                    control.Visible = false;
            }
        }

        private void ShowFoodDetailSection()
        {
            foreach (Control control in this.Controls[1].Controls)
            {
                if (control.Name == "pnlFoodList")
                    control.Visible = false;
                else if (control.Name == "pnlFoodDetail")
                    control.Visible = true;
            }
        }

        private void BtnAddNewFood_Click(object sender, EventArgs e)
        {
            isEditing = false;
            editingFoodId = -1;

            txtFoodName.Text = "";
            txtFoodPrice.Text = "";
            txtFoodDescription.Text = "";
            cmbFoodStatus.SelectedValue = 0;
            currentIngredients.Clear();
            RefreshIngredientsGrid();

            ShowFoodDetailSection();
        }

        private void BtnEditFood_Click(object sender, EventArgs e)
        {
            if (dgvFood.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn món ăn để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isEditing = true;
            editingFoodId = Convert.ToInt32(dgvFood.SelectedRows[0].Cells["food_id"].Value);

            var food = DataCache.Foods[editingFoodId];
            int statusValue = Convert.ToInt32(food.status);

            txtFoodName.Text = food.name;
            txtFoodPrice.Text = food.price.ToString();
            txtFoodDescription.Text = food.description;
            cmbFoodStatus.SelectedValue = statusValue;

            currentIngredients = DataCache.GetIngredientsByFoodId(editingFoodId);
            RefreshIngredientsGrid();

            ShowFoodDetailSection();
        }

        private void BtnDeleteFood_Click(object sender, EventArgs e)
        {
            if (dgvFood.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn món ăn để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa món ăn này?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show("Hành động này rất nguy hiểm! Xóa làm gì vại -.- [NQHxDev]!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAddIngredient_Click(object sender, EventArgs e)
        {
            if (cmbItems.SelectedItem == null || string.IsNullOrEmpty(txtQuantity.Text))
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu và nhập số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtQuantity.Text, out decimal quantity) || quantity <= 0)
            {
                MessageBox.Show("Số lượng phải là số dương!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItem = (item)cmbItems.SelectedItem;
            var selectedUnit = (unit)cmbUnits.SelectedItem;

            if (currentIngredients.Any(i => i.item_id == selectedItem.item_id))
            {
                MessageBox.Show("Nguyên liệu này đã được thêm vào công thức!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var ingredient = new food_ingredient
            {
                item_id = selectedItem.item_id,
                quantity = quantity,
                item = selectedItem
            };

            currentIngredients.Add(ingredient);
            RefreshIngredientsGrid();

            txtQuantity.Text = "";
        }

        private void BtnRemoveIngredient_Click(object sender, EventArgs e)
        {
            if (dgvIngredients.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string itemName = dgvIngredients.SelectedRows[0].Cells["item_name"].Value.ToString();
            currentIngredients.RemoveAll(i => i.item.name == itemName);
            RefreshIngredientsGrid();
        }

        private void RefreshIngredientsGrid()
        {
            dgvIngredients.Rows.Clear();
            foreach (var ingredient in currentIngredients)
            {
                string unitName = ingredient.item?.unit?.name ?? DataCache.GetUnitName(ingredient.item?.unit_id);
                dgvIngredients.Rows.Add(ingredient.item.name, ingredient.quantity, unitName);
            }
        }

        private void BtnSaveFood_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFoodName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên món!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtFoodPrice.Text, out int price) || price < 0)
            {
                MessageBox.Show("Giá bán không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (currentIngredients.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một nguyên liệu vào công thức!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var context = new OrderMonitor())
                {
                    food food;

                    if (isEditing && editingFoodId > 0)
                    {
                        food = context.food.FirstOrDefault(f => f.food_id == editingFoodId);
                        if (food == null)
                        {
                            MessageBox.Show("Không tìm thấy món ăn để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        food = new food
                        {
                            created_at = DateTime.Now
                        };
                        context.food.Add(food);
                    }

                    // Cập nhật dữ liệu
                    food.name = txtFoodName.Text;
                    food.price = price;
                    food.description = txtFoodDescription.Text;
                    food.status = Convert.ToInt16(cmbFoodStatus.SelectedValue);

                    context.SaveChanges();

                    // Cập nhật công thức
                    var existingIngredients = context.food_ingredient.Where(fi => fi.food_id == food.food_id).ToList();
                    context.food_ingredient.RemoveRange(existingIngredients);

                    foreach (var ingredient in currentIngredients)
                    {
                        context.food_ingredient.Add(new food_ingredient
                        {
                            food_id = food.food_id,
                            item_id = ingredient.item_id,
                            quantity = ingredient.quantity
                        });
                    }

                    context.SaveChanges();
                }

                RefreshFoodGridFromDb();

                isEditing = false;
                editingFoodId = -1;
                ShowFoodListSection();

                MessageBox.Show("Lưu món ăn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu món ăn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshFoodGridFromDb()
        {
            using (var ctx = new OrderMonitor())
            {
                var foods = ctx.food.ToList();

                dgvFood.Rows.Clear();
                foreach (var f in foods)
                {
                    string statusText = f.status == 0 ? "Hoạt động" : "Ngừng bán";
                    dgvFood.Rows.Add(f.food_id, f.name, $"{f.price:N0} .000 VNĐ", f.description, statusText);
                }
            }
            DataCache.Initialize();
        }


        private void BtnCancel_Click(object sender, EventArgs e)
        {
            isEditing = false;
            editingFoodId = -1;
            ShowFoodListSection();
        }
    }
}