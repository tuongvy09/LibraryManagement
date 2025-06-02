using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace LibraryManagement.UserControls
{
    partial class ThongKeManagement
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.cboThang = new System.Windows.Forms.ComboBox();
            this.lblThang = new System.Windows.Forms.Label();
            this.cboNam = new System.Windows.Forms.ComboBox();
            this.lblNam = new System.Windows.Forms.Label();
            this.tabSoLuongSachMuon = new System.Windows.Forms.TabPage();
            this.tabTop10SachMuonNhieuNhat = new System.Windows.Forms.TabPage();
            this.dgvBestSeller = new System.Windows.Forms.DataGridView();
            this.tabDoanhthu = new System.Windows.Forms.TabPage();
            this.chartTien = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabDocGia = new System.Windows.Forms.TabPage();
            this.chartDocGia = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.panelFilters.SuspendLayout();
            this.tabTop10SachMuonNhieuNhat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBestSeller)).BeginInit();
            this.tabDoanhthu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTien)).BeginInit();
            this.tabDocGia.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartDocGia)).BeginInit();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(154)))), ((int)(((byte)(79)))));
            this.lblTitle.Location = new System.Drawing.Point(27, 20);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(410, 46);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "📊 THỐNG KÊ BÁO CÁO";
            // 
            // panelFilters
            // 
            this.panelFilters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFilters.Controls.Add(this.btnExport);
            this.panelFilters.Controls.Add(this.cboThang);
            this.panelFilters.Controls.Add(this.lblThang);
            this.panelFilters.Controls.Add(this.cboNam);
            this.panelFilters.Controls.Add(this.lblNam);
            this.panelFilters.Location = new System.Drawing.Point(27, 75);
            this.panelFilters.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Size = new System.Drawing.Size(1533, 60);
            this.panelFilters.TabIndex = 1;
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(400, 12);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(175, 38);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "📋 Xuất báo cáo";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // cboThang
            // 
            this.cboThang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboThang.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboThang.FormattingEnabled = true;
            this.cboThang.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.cboThang.Location = new System.Drawing.Point(280, 15);
            this.cboThang.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboThang.Name = "cboThang";
            this.cboThang.Size = new System.Drawing.Size(89, 33);
            this.cboThang.TabIndex = 3;
            this.cboThang.SelectedIndexChanged += new System.EventHandler(this.CboThang_SelectedIndexChanged);
            // 
            // lblThang
            // 
            this.lblThang.AutoSize = true;
            this.lblThang.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(154)))), ((int)(((byte)(79)))));
            this.lblThang.Location = new System.Drawing.Point(200, 18);
            this.lblThang.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblThang.Name = "lblThang";
            this.lblThang.Size = new System.Drawing.Size(73, 25);
            this.lblThang.TabIndex = 2;
            this.lblThang.Text = "Tháng:";
            // 
            // cboNam
            // 
            this.cboNam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNam.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboNam.FormattingEnabled = true;
            this.cboNam.Items.AddRange(new object[] {
            "2020",
            "2021",
            "2022",
            "2023",
            "2024",
            "2025",
            "2026",
            "2027",
            "2028",
            "2029",
            "2030"});
            this.cboNam.Location = new System.Drawing.Point(69, 15);
            this.cboNam.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboNam.Name = "cboNam";
            this.cboNam.Size = new System.Drawing.Size(111, 33);
            this.cboNam.TabIndex = 1;
            this.cboNam.SelectedIndexChanged += new System.EventHandler(this.CboNam_SelectedIndexChanged);
            // 
            // lblNam
            // 
            this.lblNam.AutoSize = true;
            this.lblNam.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(154)))), ((int)(((byte)(79)))));
            this.lblNam.Location = new System.Drawing.Point(15, 18);
            this.lblNam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNam.Name = "lblNam";
            this.lblNam.Size = new System.Drawing.Size(59, 25);
            this.lblNam.TabIndex = 0;
            this.lblNam.Text = "Năm:";
            // 
            // tabSoLuongSachMuon
            // 
            this.tabSoLuongSachMuon.Location = new System.Drawing.Point(4, 34);
            this.tabSoLuongSachMuon.Margin = new System.Windows.Forms.Padding(4);
            this.tabSoLuongSachMuon.Name = "tabSoLuongSachMuon";
            this.tabSoLuongSachMuon.Padding = new System.Windows.Forms.Padding(4);
            this.tabSoLuongSachMuon.Size = new System.Drawing.Size(1525, 456);
            this.tabSoLuongSachMuon.TabIndex = 3;
            this.tabSoLuongSachMuon.Text = "Thống Kê Số Lượng Sách Mượn";
            this.tabSoLuongSachMuon.UseVisualStyleBackColor = true;
            // 
            // tabTop10SachMuonNhieuNhat
            // 
            this.tabTop10SachMuonNhieuNhat.Controls.Add(this.dgvBestSeller);
            this.tabTop10SachMuonNhieuNhat.Location = new System.Drawing.Point(4, 34);
            this.tabTop10SachMuonNhieuNhat.Margin = new System.Windows.Forms.Padding(4);
            this.tabTop10SachMuonNhieuNhat.Name = "tabTop10SachMuonNhieuNhat";
            this.tabTop10SachMuonNhieuNhat.Padding = new System.Windows.Forms.Padding(4);
            this.tabTop10SachMuonNhieuNhat.Size = new System.Drawing.Size(1525, 456);
            this.tabTop10SachMuonNhieuNhat.TabIndex = 0;
            this.tabTop10SachMuonNhieuNhat.Text = "📚 Top 10 sách mượn nhiều";
            this.tabTop10SachMuonNhieuNhat.UseVisualStyleBackColor = true;
            // 
            // dgvBestSeller
            // 
            this.dgvBestSeller.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBestSeller.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(154)))), ((int)(((byte)(79)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dgvBestSeller.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBestSeller.ColumnHeadersHeight = 29;
            this.dgvBestSeller.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBestSeller.EnableHeadersVisualStyles = false;
            this.dgvBestSeller.Location = new System.Drawing.Point(4, 4);
            this.dgvBestSeller.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvBestSeller.Name = "dgvBestSeller";
            this.dgvBestSeller.ReadOnly = true;
            this.dgvBestSeller.RowHeadersWidth = 51;
            this.dgvBestSeller.Size = new System.Drawing.Size(1517, 448);
            this.dgvBestSeller.TabIndex = 0;
            // 
            // tabDoanhthu
            // 
            this.tabDoanhthu.Controls.Add(this.chartTien);
            this.tabDoanhthu.Location = new System.Drawing.Point(4, 34);
            this.tabDoanhthu.Margin = new System.Windows.Forms.Padding(4);
            this.tabDoanhthu.Name = "tabDoanhthu";
            this.tabDoanhthu.Padding = new System.Windows.Forms.Padding(4);
            this.tabDoanhthu.Size = new System.Drawing.Size(1525, 456);
            this.tabDoanhthu.TabIndex = 1;
            this.tabDoanhthu.Text = "💰 Doanh thu theo tháng";
            this.tabDoanhthu.UseVisualStyleBackColor = true;
            // 
            // chartTien
            // 
            chartArea1.Name = "ChartArea1";
            this.chartTien.ChartAreas.Add(chartArea1);
            this.chartTien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartTien.Location = new System.Drawing.Point(4, 4);
            this.chartTien.Margin = new System.Windows.Forms.Padding(4);
            this.chartTien.Name = "chartTien";
            this.chartTien.Size = new System.Drawing.Size(1517, 448);
            this.chartTien.TabIndex = 0;
            this.chartTien.Text = "chartTien";
            // 
            // tabDocGia
            // 
            this.tabDocGia.Controls.Add(this.chartDocGia);
            this.tabDocGia.Location = new System.Drawing.Point(4, 34);
            this.tabDocGia.Margin = new System.Windows.Forms.Padding(4);
            this.tabDocGia.Name = "tabDocGia";
            this.tabDocGia.Padding = new System.Windows.Forms.Padding(4);
            this.tabDocGia.Size = new System.Drawing.Size(1525, 456);
            this.tabDocGia.TabIndex = 0;
            this.tabDocGia.Text = "📈 Độc giả mới theo tháng";
            this.tabDocGia.UseVisualStyleBackColor = true;
            // 
            // chartDocGia
            // 
            chartArea2.Name = "ChartArea1";
            this.chartDocGia.ChartAreas.Add(chartArea2);
            this.chartDocGia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartDocGia.Location = new System.Drawing.Point(4, 4);
            this.chartDocGia.Margin = new System.Windows.Forms.Padding(4);
            this.chartDocGia.Name = "chartDocGia";
            this.chartDocGia.Size = new System.Drawing.Size(1517, 448);
            this.chartDocGia.TabIndex = 0;
            this.chartDocGia.Text = "chartDocGia";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabDocGia);
            this.tabControl.Controls.Add(this.tabDoanhthu);
            this.tabControl.Controls.Add(this.tabTop10SachMuonNhieuNhat);
            this.tabControl.Controls.Add(this.tabSoLuongSachMuon);
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(27, 150);
            this.tabControl.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1533, 494);
            this.tabControl.TabIndex = 2;
            // 
            // ThongKeManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelFilters);
            this.Controls.Add(this.lblTitle);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ThongKeManagement";
            this.Size = new System.Drawing.Size(1600, 670);
            this.Load += new System.EventHandler(this.ThongKeManagement_Load);
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.tabTop10SachMuonNhieuNhat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBestSeller)).EndInit();
            this.tabDoanhthu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartTien)).EndInit();
            this.tabDocGia.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartDocGia)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.Label lblNam;
        private System.Windows.Forms.ComboBox cboNam;
        private System.Windows.Forms.Label lblThang;
        private System.Windows.Forms.ComboBox cboThang;
        private System.Windows.Forms.Button btnExport;
        private TabPage tabSoLuongSachMuon;
        private TabPage tabTop10SachMuonNhieuNhat;
        private DataGridView dgvBestSeller;
        private TabPage tabDoanhthu;
        private Chart chartTien;
        private TabPage tabDocGia;
        private Chart chartDocGia;
        private TabControl tabControl;
    }
}