namespace DoAn_LTW
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.btnDanhSach = new System.Windows.Forms.Button();
            this.btnDangThucHien = new System.Windows.Forms.Button();
            this.btnMenuFood = new System.Windows.Forms.Button();
            this.btnKho = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnQuanLy = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDanhSach
            // 
            this.btnDanhSach.Location = new System.Drawing.Point(34, 47);
            this.btnDanhSach.Name = "btnDanhSach";
            this.btnDanhSach.Size = new System.Drawing.Size(175, 54);
            this.btnDanhSach.TabIndex = 0;
            this.btnDanhSach.Text = "Danh Sách";
            this.btnDanhSach.UseVisualStyleBackColor = true;
            this.btnDanhSach.Click += new System.EventHandler(this.HandleChangePanel);
            // 
            // btnDangThucHien
            // 
            this.btnDangThucHien.Location = new System.Drawing.Point(34, 158);
            this.btnDangThucHien.Name = "btnDangThucHien";
            this.btnDangThucHien.Size = new System.Drawing.Size(175, 54);
            this.btnDangThucHien.TabIndex = 0;
            this.btnDangThucHien.Text = "Đang Thực Hiện";
            this.btnDangThucHien.UseVisualStyleBackColor = true;
            this.btnDangThucHien.Click += new System.EventHandler(this.HandleChangePanel);
            // 
            // btnMenuFood
            // 
            this.btnMenuFood.Location = new System.Drawing.Point(34, 393);
            this.btnMenuFood.Name = "btnMenuFood";
            this.btnMenuFood.Size = new System.Drawing.Size(175, 54);
            this.btnMenuFood.TabIndex = 0;
            this.btnMenuFood.Text = "Foods";
            this.btnMenuFood.UseVisualStyleBackColor = true;
            this.btnMenuFood.Click += new System.EventHandler(this.HandleChangePanel);
            // 
            // btnKho
            // 
            this.btnKho.Location = new System.Drawing.Point(34, 270);
            this.btnKho.Name = "btnKho";
            this.btnKho.Size = new System.Drawing.Size(175, 54);
            this.btnKho.TabIndex = 0;
            this.btnKho.Text = "Kho";
            this.btnKho.UseVisualStyleBackColor = true;
            this.btnKho.Click += new System.EventHandler(this.HandleChangePanel);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Location = new System.Drawing.Point(249, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1451, 909);
            this.panel1.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 17F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(631, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(206, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Danh sách Order";
            // 
            // btnQuanLy
            // 
            this.btnQuanLy.Location = new System.Drawing.Point(34, 520);
            this.btnQuanLy.Name = "btnQuanLy";
            this.btnQuanLy.Size = new System.Drawing.Size(175, 54);
            this.btnQuanLy.TabIndex = 0;
            this.btnQuanLy.Text = "Quản Lý";
            this.btnQuanLy.UseVisualStyleBackColor = true;
            this.btnQuanLy.Click += new System.EventHandler(this.HandleChangePanel);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.ClientSize = new System.Drawing.Size(1712, 933);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnKho);
            this.Controls.Add(this.btnQuanLy);
            this.Controls.Add(this.btnMenuFood);
            this.Controls.Add(this.btnDangThucHien);
            this.Controls.Add(this.btnDanhSach);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NQH Dev";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button btnDanhSach;
        private System.Windows.Forms.Button btnDangThucHien;
        private System.Windows.Forms.Button btnMenuFood;
        private System.Windows.Forms.Button btnKho;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnQuanLy;
    }
}
