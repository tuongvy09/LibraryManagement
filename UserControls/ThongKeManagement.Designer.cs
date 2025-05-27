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
            ChartArea chartArea1 = new ChartArea();
            ChartArea chartArea2 = new ChartArea();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblNam = new System.Windows.Forms.Label();
            this.cboNam = new System.Windows.Forms.ComboBox();
            this.lblThang = new System.Windows.Forms.Label();
            this.cboThang = new System.Windows.Forms.ComboBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabDocGia = new System.Windows.Forms.TabPage();
            this.chartDocGia = new Chart();
            this.tabDoanhthu = new System.Windows.Forms.TabPage();
            this.chartTien = new Chart();
            this.tabChitiet = new System.Windows.Forms.TabPage();
            this.dgvThongKe = new System.Windows.Forms.DataGridView();
            this.lblThongKeThang = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartDocGia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTien)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabDocGia.SuspendLayout();
            this.tabDoanhthu.SuspendLayout();
            this.tabChitiet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongKe)).BeginInit();
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
            this.lblTitle.Size = new System.Drawing.Size(258, 41);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "THỐNG KÊ BÁO CÁO";
            // 
            // lblNam
            // 
            this.lblNam.AutoSize = true;
            this.lblNam.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(154)))), ((int)(((byte)(79)))));
            this.lblNam.Location = new System.Drawing.Point(27, 86);
            this.lblNam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNam.Name = "lblNam";
            this.lblNam.Size = new System.Drawing.Size(46, 23);
            this.lblNam.TabIndex = 1;
            this.lblNam.Text = "Năm:";
            // 
            // cboNam
            // 
            this.cboNam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNam.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.cboNam.Location = new System.Drawing.Point(81, 82);
            this.cboNam.Margin = new System.Windows.Forms.Padding(4);
            this.cboNam.Name = "cboNam";
            this.cboNam.Size = new System.Drawing.Size(100, 31);
            this.cboNam.TabIndex = 2;
            this.cboNam.SelectedIndexChanged += new System.EventHandler(this.CboNam_SelectedIndexChanged);
            // 
            // lblThang
            // 
            this.lblThang.AutoSize = true;
            this.lblThang.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(154)))), ((int)(((byte)(79)))));
            this.lblThang.Location = new System.Drawing.Point(210, 86);
            this.lblThang.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblThang.Name = "lblThang";
            this.lblThang.Size = new System.Drawing.Size(58, 23);
            this.lblThang.TabIndex = 3;
            this.lblThang.Text = "Tháng:";
            // 
            // cboThang
            // 
            this.cboThang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboThang.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.cboThang.Location = new System.Drawing.Point(276, 82);
            this.cboThang.Margin = new System.Windows.Forms.Padding(4);
            this.cboThang.Name = "cboThang";
            this.cboThang.Size = new System.Drawing.Size(80, 31);
            this.cboThang.TabIndex = 4;
            this.cboThang.SelectedIndexChanged += new System.EventHandler(this.CboThang_SelectedIndexChanged);
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(390, 82);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(120, 35);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Xuất báo cáo";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabDocGia);
            this.tabControl.Controls.Add(this.tabDoanhthu);
            this.tabControl.Controls.Add(this.tabChitiet);
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(27, 135);
            this.tabControl.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1533, 493);
            this.tabControl.TabIndex = 6;
            // 
            // tabDocGia
            // 
            this.tabDocGia.Controls.Add(this.chartDocGia);
            this.tabDocGia.Location = new System.Drawing.Point(4, 32);
            this.tabDocGia.Margin = new System.Windows.Forms.Padding(4);
            this.tabDocGia.Name = "tabDocGia";
            this.tabDocGia.Padding = new System.Windows.Forms.Padding(4);
            this.tabDocGia.Size = new System.Drawing.Size(1525, 457);
            this.tabDocGia.TabIndex = 0;
            this.tabDocGia.Text = "Độc giả mới theo tháng";
            this.tabDocGia.UseVisualStyleBackColor = true;
            // 
            // chartDocGia
            // 
            chartArea1.Name = "ChartArea1";
            this.chartDocGia.ChartAreas.Add(chartArea1);
            this.chartDocGia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartDocGia.Location = new System.Drawing.Point(4, 4);
            this.chartDocGia.Margin = new System.Windows.Forms.Padding(4);
            this.chartDocGia.Name = "chartDocGia";
            this.chartDocGia.Size = new System.Drawing.Size(1517, 449);
            this.chartDocGia.TabIndex = 0;
            this.chartDocGia.Text = "chartDocGia";
            // 
            // tabDoanhthu
            // 
            this.tabDoanhthu.Controls.Add(this.chartTien);
            this.tabDoanhthu.Location = new System.Drawing.Point(4, 32);
            this.tabDoanhthu.Margin = new System.Windows.Forms.Padding(4);
            this.tabDoanhthu.Name = "tabDoanhthu";
            this.tabDoanhthu.Padding = new System.Windows.Forms.Padding(4);
            this.tabDoanhthu.Size = new System.Drawing.Size(1525, 457);
            this.tabDoanhthu.TabIndex = 1;
            this.tabDoanhthu.Text = "Doanh thu theo tháng";
            this.tabDoanhthu.UseVisualStyleBackColor = true;
            // 
            // chartTien
            // 
            chartArea2.Name = "ChartArea1";
            this.chartTien.ChartAreas.Add(chartArea2);
            this.chartTien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartTien.Location = new System.Drawing.Point(4, 4);
            this.chartTien.Margin = new System.Windows.Forms.Padding(4);
            this.chartTien.Name = "chartTien";
            this.chartTien.Size = new System.Drawing.Size(1517, 449);
            this.chartTien.TabIndex = 0;
            this.chartTien.Text = "chartTien";
            // 
            // tabChitiet
            // 
            this.tabChitiet.Controls.Add(this.dgvThongKe);
            this.tabChitiet.Controls.Add(this.lblThongKeThang);
            this.tabChitiet.Location = new System.Drawing.Point(4, 32);
            this.tabChitiet.Margin = new System.Windows.Forms.Padding(4);
            this.tabChitiet.Name = "tabChitiet";
            this.tabChitiet.Padding = new System.Windows.Forms.Padding(4);
            this.tabChitiet.Size = new System.Drawing.Size(1525, 457);
            this.tabChitiet.TabIndex = 2;
            this.tabChitiet.Text = "Chi tiết tiền mượn + phạt";
            this.tabChitiet.UseVisualStyleBackColor = true;
            // 
            // dgvThongKe
            // 
            this.dgvThongKe.AllowUserToAddRows = false;
            this.dgvThongKe.AllowUserToDeleteRows = false;
            this.dgvThongKe.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvThongKe.BackgroundColor = System.Drawing.Color.White;
            this.dgvThongKe.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvThongKe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvThongKe.Location = new System.Drawing.Point(8, 40);
            this.dgvThongKe.Margin = new System.Windows.Forms.Padding(4);
            this.dgvThongKe.MultiSelect = false;
            this.dgvThongKe.Name = "dgvThongKe";
            this.dgvThongKe.ReadOnly = true;
            this.dgvThongKe.RowHeadersWidth = 51;
            this.dgvThongKe.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvThongKe.Size = new System.Drawing.Size(1509, 409);
            this.dgvThongKe.TabIndex = 1;
            // 
            // lblThongKeThang
            // 
            this.lblThongKeThang.AutoSize = true;
            this.lblThongKeThang.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThongKeThang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(154)))), ((int)(((byte)(79)))));
            this.lblThongKeThang.Location = new System.Drawing.Point(8, 8);
            this.lblThongKeThang.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblThongKeThang.Name = "lblThongKeThang";
            this.lblThongKeThang.Size = new System.Drawing.Size(301, 28);
            this.lblThongKeThang.TabIndex = 0;
            this.lblThongKeThang.Text = "Top độc giả có chi phí cao nhất";
            // 
            // ThongKeManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.cboThang);
            this.Controls.Add(this.lblThang);
            this.Controls.Add(this.cboNam);
            this.Controls.Add(this.lblNam);
            this.Controls.Add(this.lblTitle);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ThongKeManagement";
            this.Size = new System.Drawing.Size(1600, 652);
            ((System.ComponentModel.ISupportInitialize)(this.chartDocGia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTien)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabDocGia.ResumeLayout(false);
            this.tabDoanhthu.ResumeLayout(false);
            this.tabChitiet.ResumeLayout(false);
            this.tabChitiet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongKe)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblNam;
        private System.Windows.Forms.ComboBox cboNam;
        private System.Windows.Forms.Label lblThang;
        private System.Windows.Forms.ComboBox cboThang;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabDocGia;
        private Chart chartDocGia;
        private System.Windows.Forms.TabPage tabDoanhthu;
        private Chart chartTien;
        private System.Windows.Forms.TabPage tabChitiet;
        private System.Windows.Forms.DataGridView dgvThongKe;
        private System.Windows.Forms.Label lblThongKeThang;
    }
}
