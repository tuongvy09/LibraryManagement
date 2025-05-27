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
}
