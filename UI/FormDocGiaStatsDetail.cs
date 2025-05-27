using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryManagement.UI
{
    public partial class FormDocGiaStatsDetail : Form
    {
        public FormDocGiaStatsDetail(ThongKeTienMuonDocGiaDTO stats)
        {
            InitializeComponent(stats);
        }

        private void InitializeComponent(ThongKeTienMuonDocGiaDTO stats)
        {
            this.Text = $"Thống kê chi tiết - {stats.HoTen}";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Title
            Label lblTitle = new Label()
            {
                Text = $"THỐNG KÊ CHI TIẾT",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#739a4f"),
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Tên độc giả
            Label lblTen = new Label()
            {
                Text = $"Độc giả: {stats.HoTen} (Mã: {stats.MaDocGia})",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 60),
                AutoSize = true
            };
            this.Controls.Add(lblTen);

            // Panel thông tin
            Panel panelInfo = new Panel()
            {
                Location = new Point(20, 100),
                Size = new Size(440, 200),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Các label thông tin
            string[] infos = {
                $"💰 Tổng tiền mượn sách: {stats.TongTienMuon:N0} VNĐ",
                $"⚠️ Tổng tiền phạt: {stats.TongTienPhat:N0} VNĐ",
                $"💵 Tổng cộng: {stats.TongCong:N0} VNĐ",
                $"📚 Số lần mượn: {stats.SoLanMuon} lần",
                $"📅 Lần mượn gần nhất: {(stats.LanMuonGanNhat?.ToString("dd/MM/yyyy") ?? "Chưa mượn")}"
            };

            int yPos = 20;
            foreach (string info in infos)
            {
                Label lbl = new Label()
                {
                    Text = info,
                    Font = new Font("Segoe UI", 11),
                    Location = new Point(15, yPos),
                    AutoSize = true
                };
                panelInfo.Controls.Add(lbl);
                yPos += 30;
            }

            this.Controls.Add(panelInfo);

            // Button đóng
            Button btnClose = new Button()
            {
                Text = "Đóng",
                Location = new Point(200, 320),
                Size = new Size(80, 30),
                BackColor = ColorTranslator.FromHtml("#6c757d"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }
    }
}
