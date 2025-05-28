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
    public partial class FormChiTietPhieuPhat : Form
    {
        private readonly PhieuPhat thongTin;

        public FormChiTietPhieuPhat(PhieuPhat thongTin)
        {
            this.thongTin = thongTin;

            InitializeCustomComponent();
            HienThiThongTin();
        }

        private void InitializeCustomComponent()
        {
            this.Text = "Chi tiết Phiếu Phạt";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            Label lblTitle = new Label()
            {
                Text = "CHI TIẾT PHIẾU PHẠT",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#b03a2e"),
                AutoSize = true,
                Location = new Point(30, 20)
            };
            this.Controls.Add(lblTitle);

            int y = 70;
            foreach (var label in TaoLabelThongTin())
            {
                label.Location = new Point(30, y);
                label.Size = new Size(420, 30);
                this.Controls.Add(label);
                y += 40;
            }
        }

        private List<Label> TaoLabelThongTin()
        {
            return new List<Label>
            {
                TaoLabel($"Mã Phiếu Phạt: {thongTin.MaPhieuPhat}"),
                TaoLabel($"Tên Độc Giả: {thongTin.HoTen}"),
                TaoLabel($"Tổng Tiền Phạt: {thongTin.TongTien:N0} VNĐ"),
                TaoLabel($"Lỗi Vi Phạm: {GetLoiViPhamText()}")
            };
        }

        private string GetLoiViPhamText()
        {
            if (thongTin.LoiViPhams == null || !thongTin.LoiViPhams.Any())
                return "Không có thông tin lỗi";

            return string.Join(", ", thongTin.LoiViPhams.Select(l => $"{l.LyDo} ({l.TienPhat:N0} VNĐ)"));
        }

        private Label TaoLabel(string text)
        {
            return new Label()
            {
                Text = text,
                Font = new Font("Segoe UI", 12),
                AutoSize = true
            };
        }

        private void HienThiThongTin()
        {
            // Thông tin đã hiển thị trong khởi tạo label
        }
    }
}
