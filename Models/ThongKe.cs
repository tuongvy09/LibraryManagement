using System;

namespace LibraryManagement.Models
{
    public class ThongKeDocGiaTheoThangDTO
    {
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int SoLuongDocGiaMoi { get; set; }
        public string ThangNam => $"{Thang:00}/{Nam}";
    }

    public class ThongKeTienMuonDocGiaDTO
    {
        public int MaDocGia { get; set; }
        public string HoTen { get; set; }
        public decimal TongTienMuon { get; set; }
        public decimal TongTienPhat { get; set; }
        public decimal TongCong => TongTienMuon + TongTienPhat;
        public int SoLanMuon { get; set; }
        public DateTime? LanMuonGanNhat { get; set; }
    }

    public class ThongKeTienTheoThangDTO
    {
        public int Thang { get; set; }
        public int Nam { get; set; }
        public decimal TongTienMuon { get; set; }
        public decimal TongTienPhat { get; set; }
        public decimal TongCong => TongTienMuon + TongTienPhat;
        public string ThangNam => $"{Thang:00}/{Nam}";
    }

    public class ThongKeDoanhThuTheoThangDTO
    {
        public int Thang { get; set; }
        public int Nam { get; set; }
        public decimal TongDoanhThu { get; set; }
        public int SoGiaoDich { get; set; }
        public string ThangNam => $"{Thang:00}/{Nam}";
        public string DoanhThuDisplay => TongDoanhThu.ToString("N0") + " VNĐ";
    }

    public class ThongKeSachMuonTheoTheLoaiDTO
    {
        public string TenTheLoai { get; set; }
        public int SoLuongMuon { get; set; }
    }

    public class ThongKeSachMuonTheoDocGiaDTO
    {
        public string TenDocGia { get; set; }
        public int SoLuongMuon { get; set; }
    }

    public class ThongKeSachMuonTheoThangDTO
    {
        public string TenDauSach { get; set; }
        public int SoLuongMuon { get; set; }
    }

    public class SachMuonDTO
    {
        public string MaSach { get; set; }
        public string TenSach { get; set; }
        public string TenDocGia { get; set; }
        public DateTime NgayMuon { get; set; }
        public DateTime NgayTraDuKien { get; set; }
    }
}
