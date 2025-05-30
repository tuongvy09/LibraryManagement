using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class TheThuVien
    {
        public int MaThe { get; set; }
        public int? MaDG { get; set; }
        public DateTime NgayCap { get; set; }
        public DateTime NgayHetHan { get; set; }

        // Navigation properties
        public string TenDocGia { get; set; }
        public string SoDT { get; set; }
        public bool TrangThaiThe => DateTime.Now <= NgayHetHan;
    }

    public class ThongKeTheThuVienDTO
    {
        public int Thang { get; set; }
        public string TenThang { get; set; }
        public int SoTheCapMoi { get; set; }
        public int Nam { get; set; }
    }
}
