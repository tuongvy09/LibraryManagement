using System;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.UserControls
{
    partial class TheThuVienManagement
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabConHieuLuc;
        private System.Windows.Forms.TabPage tabHetHan;
        private System.Windows.Forms.DataGridView dgvConHieuLuc;
        private System.Windows.Forms.DataGridView dgvHetHan;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnGenerateQR;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();

            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabConHieuLuc = new System.Windows.Forms.TabPage();
            this.tabHetHan = new System.Windows.Forms.TabPage();
            this.dgvConHieuLuc = new System.Windows.Forms.DataGridView();
            this.dgvHetHan = new System.Windows.Forms.DataGridView();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnGenerateQR = new System.Windows.Forms.Button();

            this.tabControl.SuspendLayout();
            this.tabConHieuLuc.SuspendLayout();
            this.tabHetHan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConHieuLuc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHetHan)).BeginInit();
            this.SuspendLayout();

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(154)))), ((int)(((byte)(79)))));
            this.lblTitle.Location = new System.Drawing.Point(27, 25);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(364, 41);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "QUẢN LÝ THẺ THƯ VIỆN";

            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.ForeColor = System.Drawing.Color.Black;
            this.lblSearch.Location = new System.Drawing.Point(27, 86);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(83, 23);
            this.lblSearch.TabIndex = 1;
            this.lblSearch.Text = "Tìm kiếm:";

            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.ForeColor = System.Drawing.Color.Black;
            this.txtSearch.Location = new System.Drawing.Point(133, 82);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(400, 30);
            this.txtSearch.TabIndex = 2;

            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(154)))), ((int)(((byte)(79)))));
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(545, 82);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(107, 31);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);

            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(154)))), ((int)(((byte)(79)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(27, 135);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 43);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Thêm mới";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);

            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(160, 135);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(107, 43);
            this.btnEdit.TabIndex = 5;
            this.btnEdit.Text = "Gia hạn thẻ";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);

            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(280, 135);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(107, 43);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);

            // 
            // btnGenerateQR
            // 
            this.btnGenerateQR = new System.Windows.Forms.Button();
            this.btnGenerateQR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(39)))), ((int)(((byte)(176)))));
            this.btnGenerateQR.FlatAppearance.BorderSize = 0;
            this.btnGenerateQR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateQR.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateQR.ForeColor = System.Drawing.Color.White;
            this.btnGenerateQR.Location = new System.Drawing.Point(400, 135);
            this.btnGenerateQR.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGenerateQR.Name = "btnGenerateQR";
            this.btnGenerateQR.Size = new System.Drawing.Size(107, 43);
            this.btnGenerateQR.TabIndex = 7;
            this.btnGenerateQR.Text = "Tạo QR";
            this.btnGenerateQR.UseVisualStyleBackColor = false;
            this.btnGenerateQR.Click += new System.EventHandler(this.BtnGenerateQR_Click);

            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabConHieuLuc);
            this.tabControl.Controls.Add(this.tabHetHan);
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.tabControl.Location = new System.Drawing.Point(27, 197);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1533, 431);
            this.tabControl.TabIndex = 8;

            // 
            // tabConHieuLuc
            // 
            this.tabConHieuLuc.BackColor = System.Drawing.Color.White;
            this.tabConHieuLuc.Controls.Add(this.dgvConHieuLuc);
            this.tabConHieuLuc.ForeColor = System.Drawing.Color.Black;
            this.tabConHieuLuc.Location = new System.Drawing.Point(4, 29);
            this.tabConHieuLuc.Name = "tabConHieuLuc";
            this.tabConHieuLuc.Padding = new System.Windows.Forms.Padding(3);
            this.tabConHieuLuc.Size = new System.Drawing.Size(1525, 398);
            this.tabConHieuLuc.TabIndex = 0;
            this.tabConHieuLuc.Text = "Còn hiệu lực";

            // 
            // tabHetHan
            // 
            this.tabHetHan.BackColor = System.Drawing.Color.White;
            this.tabHetHan.Controls.Add(this.dgvHetHan);
            this.tabHetHan.ForeColor = System.Drawing.Color.Black;
            this.tabHetHan.Location = new System.Drawing.Point(4, 29);
            this.tabHetHan.Name = "tabHetHan";
            this.tabHetHan.Padding = new System.Windows.Forms.Padding(3);
            this.tabHetHan.Size = new System.Drawing.Size(1525, 398);
            this.tabHetHan.TabIndex = 1;
            this.tabHetHan.Text = "Hết hạn";

            // 
            // dgvConHieuLuc
            // 
            this.dgvConHieuLuc.AllowUserToAddRows = false;
            this.dgvConHieuLuc.AllowUserToDeleteRows = false;
            this.dgvConHieuLuc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvConHieuLuc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvConHieuLuc.BackgroundColor = System.Drawing.Color.White;
            this.dgvConHieuLuc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            // CHỈ IN ĐẬM HEADER
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(154)))), ((int)(((byte)(79)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvConHieuLuc.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvConHieuLuc.ColumnHeadersHeight = 30;
            this.dgvConHieuLuc.EnableHeadersVisualStyles = false;
            // NỘI DUNG MÀU ĐEN BÌNH THƯỜNG
            this.dgvConHieuLuc.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.dgvConHieuLuc.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvConHieuLuc.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(154)))), ((int)(((byte)(79)))));
            this.dgvConHieuLuc.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvConHieuLuc.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.dgvConHieuLuc.Location = new System.Drawing.Point(3, 3);
            this.dgvConHieuLuc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvConHieuLuc.MultiSelect = false;
            this.dgvConHieuLuc.Name = "dgvConHieuLuc";
            this.dgvConHieuLuc.ReadOnly = true;
            this.dgvConHieuLuc.RowHeadersVisible = false;
            this.dgvConHieuLuc.RowHeadersWidth = 51;
            this.dgvConHieuLuc.RowTemplate.Height = 25;
            this.dgvConHieuLuc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvConHieuLuc.Size = new System.Drawing.Size(1519, 392);
            this.dgvConHieuLuc.TabIndex = 0;
            this.dgvConHieuLuc.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvConHieuLuc_CellContentClick);
            this.dgvConHieuLuc.DoubleClick += new System.EventHandler(this.DgvConHieuLuc_DoubleClick);

            // 
            // dgvHetHan
            // 
            this.dgvHetHan.AllowUserToAddRows = false;
            this.dgvHetHan.AllowUserToDeleteRows = false;
            this.dgvHetHan.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvHetHan.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHetHan.BackgroundColor = System.Drawing.Color.White;
            this.dgvHetHan.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            // CHỈ IN ĐẬM HEADER
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(154)))), ((int)(((byte)(79)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHetHan.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvHetHan.ColumnHeadersHeight = 30;
            this.dgvHetHan.EnableHeadersVisualStyles = false;
            // NỘI DUNG MÀU ĐEN BÌNH THƯỜNG
            this.dgvHetHan.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.dgvHetHan.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvHetHan.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(154)))), ((int)(((byte)(79)))));
            this.dgvHetHan.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvHetHan.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.dgvHetHan.Location = new System.Drawing.Point(3, 3);
            this.dgvHetHan.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvHetHan.MultiSelect = false;
            this.dgvHetHan.Name = "dgvHetHan";
            this.dgvHetHan.ReadOnly = true;
            this.dgvHetHan.RowHeadersVisible = false;
            this.dgvHetHan.RowHeadersWidth = 51;
            this.dgvHetHan.RowTemplate.Height = 25;
            this.dgvHetHan.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHetHan.Size = new System.Drawing.Size(1519, 392);
            this.dgvHetHan.TabIndex = 0;
            this.dgvHetHan.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvHetHan_CellContentClick);
            this.dgvHetHan.DoubleClick += new System.EventHandler(this.DgvHetHan_DoubleClick);

            // 
            // TheThuVienManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnGenerateQR);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.lblTitle);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "TheThuVienManagement";
            this.Size = new System.Drawing.Size(1600, 652);
            this.Load += new System.EventHandler(this.TheThuVienManagement_Load);

            this.tabControl.ResumeLayout(false);
            this.tabConHieuLuc.ResumeLayout(false);
            this.tabHetHan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConHieuLuc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHetHan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
