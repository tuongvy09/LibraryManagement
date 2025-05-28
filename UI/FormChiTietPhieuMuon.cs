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
    public partial class FormChiTietPhieuMuon : Form
    {
        private PhieuMuon thongTin;

        public FormChiTietPhieuMuon(PhieuMuon thongTin)
        {
            this.thongTin = thongTin;

            InitializeCustomComponent();
            HienThiThongTin();
        }

        private void InitializeCustomComponent()
        {
            this.Text = "Chi tiết Phiếu Mượn";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            Label lblTitle = new Label()
            {
                Text = "CHI TIẾT PHIẾU MƯỢN",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#739a4f"),
                AutoSize = true,
                Location = new Point(30, 20)
            };
            this.Controls.Add(lblTitle);

            int y = 70;
            foreach (var item in TaoLabelThongTin())
            {
                item.Location = new Point(30, y);
                item.Size = new Size(420, 30);
                this.Controls.Add(item);
                y += 40;
            }
        }

        private List<Label> TaoLabelThongTin()
        {
            return new List<Label>
        {
            TaoLabel($"Mã Phiếu Mượn: {thongTin.MaMuonSach}"),
            TaoLabel($"Tên Độc Giả: {thongTin.TenDocGia}"),
            TaoLabel($"Tên Cuốn Sách: {thongTin.TenCuonSach}"),
            TaoLabel($"Ngày Mượn: {thongTin.NgayMuon:dd/MM/yyyy}"),
            TaoLabel($"Ngày Trả: {thongTin.NgayTra:dd/MM/yyyy}"),
            TaoLabel($"Trạng Thái: {thongTin.TrangThaiM}"),
            TaoLabel($"Giá Mượn: {thongTin.GiaMuon:N0} VNĐ"),
            TaoLabel($"Số Ngày Mượn: {thongTin.SoNgayMuon}"),
            TaoLabel($"Tiền Cọc: {thongTin.TienCoc:N0} VNĐ")
        };
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
        }
    }
}
