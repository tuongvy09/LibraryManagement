using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LibraryManagement.UserControls
{
    partial class SachControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.dgvBooks = new System.Windows.Forms.DataGridView();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnManageAuthors = new System.Windows.Forms.Button();
            this.btnManagePublishers = new System.Windows.Forms.Button();
            this.btnManageCategories = new System.Windows.Forms.Button();
            this.btnManageTitles = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBooks)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvBooks
            // 
            this.dgvBooks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBooks.Location = new System.Drawing.Point(20, 60);
            this.dgvBooks.Name = "dgvBooks";
            this.dgvBooks.ReadOnly = true;
            this.dgvBooks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBooks.Size = new System.Drawing.Size(790, 300);
            this.dgvBooks.TabIndex = 2;
            this.dgvBooks.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBooks_CellContentClick);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(20, 20);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 20);
            this.txtSearch.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(230, 20);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(20, 380);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 30);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Thêm";
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(110, 380);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 30);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "Sửa";
            this.btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(200, 380);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 30);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Xóa";
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // btnManageAuthors
            // 
            this.btnManageAuthors.Location = new System.Drawing.Point(300, 380);
            this.btnManageAuthors.Name = "btnManageAuthors";
            this.btnManageAuthors.Size = new System.Drawing.Size(120, 30);
            this.btnManageAuthors.TabIndex = 6;
            this.btnManageAuthors.Text = "Quản lý Tác giả";
            this.btnManageAuthors.Click += new System.EventHandler(this.BtnManageAuthors_Click);
            // 
            // btnManagePublishers
            // 
            this.btnManagePublishers.Location = new System.Drawing.Point(430, 380);
            this.btnManagePublishers.Name = "btnManagePublishers";
            this.btnManagePublishers.Size = new System.Drawing.Size(120, 30);
            this.btnManagePublishers.TabIndex = 7;
            this.btnManagePublishers.Text = "Quản lý NXB";
            this.btnManagePublishers.Click += new System.EventHandler(this.BtnManagePublishers_Click);
            // 
            // btnManageCategories
            // 
            this.btnManageCategories.Location = new System.Drawing.Point(560, 380);
            this.btnManageCategories.Name = "btnManageCategories";
            this.btnManageCategories.Size = new System.Drawing.Size(120, 30);
            this.btnManageCategories.TabIndex = 8;
            this.btnManageCategories.Text = "Quản lý Thể loại";
            this.btnManageCategories.Click += new System.EventHandler(this.BtnManageCategories_Click);
            // 
            // btnManageTitles
            // 
            this.btnManageTitles.Location = new System.Drawing.Point(690, 380);
            this.btnManageTitles.Name = "btnManageTitles";
            this.btnManageTitles.Size = new System.Drawing.Size(120, 30);
            this.btnManageTitles.TabIndex = 9;
            this.btnManageTitles.Text = "Quản lý Đầu sách";
            this.btnManageTitles.Click += new System.EventHandler(this.BtnManageTitles_Click);
            // btnDownloadTemplate
            this.btnDownloadTemplate = new System.Windows.Forms.Button();
            this.btnDownloadTemplate.Location = new System.Drawing.Point(20, 420);
            this.btnDownloadTemplate.Name = "btnDownloadTemplate";
            this.btnDownloadTemplate.Size = new System.Drawing.Size(160, 30);
            this.btnDownloadTemplate.TabIndex = 10;
            this.btnDownloadTemplate.Text = "Tải file mẫu Excel";
            this.btnDownloadTemplate.BackColor = System.Drawing.Color.FromArgb(115, 154, 79);
            this.btnDownloadTemplate.ForeColor = System.Drawing.Color.White;
            this.btnDownloadTemplate.Click += new System.EventHandler(this.BtnDownloadTemplate_Click);
            this.Controls.Add(this.btnDownloadTemplate);
            // btnUploadFile
            this.btnUploadFile = new System.Windows.Forms.Button();
            this.btnUploadFile.Location = new System.Drawing.Point(190, 420);
            this.btnUploadFile.Name = "btnUploadFile";
            this.btnUploadFile.Size = new System.Drawing.Size(160, 30);
            this.btnUploadFile.TabIndex = 11;
            this.btnUploadFile.Text = "Tải file Excel lên";
            this.btnUploadFile.BackColor = System.Drawing.Color.FromArgb(91, 155, 213);
            this.btnUploadFile.ForeColor = System.Drawing.Color.White;
            this.btnUploadFile.Click += new System.EventHandler(this.BtnUploadFile_Click);
            this.Controls.Add(this.btnUploadFile);

            // 
            // BookControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dgvBooks);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnManageAuthors);
            this.Controls.Add(this.btnManagePublishers);
            this.Controls.Add(this.btnManageCategories);
            this.Controls.Add(this.btnManageTitles);
            this.Controls.Add(this.btnDownloadTemplate);
            this.Controls.Add(this.btnUploadFile);
            this.Name = "BookControl";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBooks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            SetButtonStyle(this.btnAdd);
            SetButtonStyle(this.btnEdit);
            SetButtonStyle(this.btnDelete);
            SetButtonStyle(this.btnManageAuthors);
            SetButtonStyle(this.btnManagePublishers);
            SetButtonStyle(this.btnManageCategories);
            SetButtonStyle(this.btnManageTitles);
            SetButtonStyle(this.btnSearch);
            SetButtonStyle(this.btnDownloadTemplate);
            SetButtonStyle(this.btnUploadFile);

        }

        private System.Windows.Forms.DataGridView dgvBooks;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnManageAuthors;
        private System.Windows.Forms.Button btnManagePublishers;
        private System.Windows.Forms.Button btnManageCategories;
        private System.Windows.Forms.Button btnManageTitles;
        private System.Windows.Forms.Button btnDownloadTemplate;
        private System.Windows.Forms.Button btnUploadFile;

        private void SetButtonStyle(Button btn)
        {
            btn.BackColor = System.Drawing.Color.FromArgb(115, 154, 79);
            btn.ForeColor = System.Drawing.Color.White;
        }

    }

}
