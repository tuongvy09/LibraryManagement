using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class DocGiaDTO
    {
        public int MaDocGia { get; set; }
        public int? MaLoaiDG { get; set; }
        public string HoTen { get; set; }
        public int Tuoi { get; set; }
        public string SoDT { get; set; }
        public string CCCD { get; set; }
        public string GioiTinh { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public DateTime NgayDangKy { get; set; }
        public decimal TienNo { get; set; }
        public bool TrangThai { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public int? MaThe { get; set; }

        // Navigation properties
        public string TenLoaiDG { get; set; }
        public string MaTheString { get; set; }
    }
}
