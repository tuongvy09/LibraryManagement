using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class ThuThu
    {
        public int MaThuThu { get; set; }
        public string TenThuThu { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public DateTime NgayBatDauLam { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public bool TrangThai { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public int? UserID { get; set; }

        // Navigation properties
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
