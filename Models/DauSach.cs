using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class DauSach
    {
        public int MaDauSach { get; set; }
        public string TenDauSach { get; set; }
        public int MaNXB { get; set; }
        public int MaTheLoai { get; set; }
        public int? NamXuatBan { get; set; }
        public decimal? GiaTien { get; set; }
        public int? SoTrang { get; set; }
        public string NgonNgu { get; set; }
        public string MoTa { get; set; }


    }


    public class DauSachDTO
    {
        public int MaDauSach { get; set; }
        public string TenDauSach { get; set; }
        public int MaTheLoai { get; set; }
        public string TenTheLoai { get; set; }
        public int? MaNXB { get; set; }
        public string TenNXB { get; set; }
        public string TacGia { get; set; }

        public int? NamXuatBan { get; set; }
        public decimal? GiaTien { get; set; }
        public int? SoTrang { get; set; }
        public string NgonNgu { get; set; }
        public string MoTa { get; set; }
    }

}
