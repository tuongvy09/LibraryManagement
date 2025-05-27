using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class PhieuMuon
    {
        public int MaPhieu { get; set; }
        public int? MaDocGia { get; set; }
        public int? MaThuThu { get; set; }             
        public DateTime NgayMuon { get; set; }         
        public DateTime HanTra { get; set; }            

        // Dùng cho hiển thị
        public string TenDocGia { get; set; }
        public string TenThuThu { get; set; }
    }
}
