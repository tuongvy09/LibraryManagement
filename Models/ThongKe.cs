using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class ThongKeTienMuonDTO
    {
        public int MaDocGia { get; set; }
        public string HoTen { get; set; }
        public decimal TongTienMuon { get; set; }
        public int SoLanMuon { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
    }

    public class ThongKeTienMuonVaPhatDTO
    {
        public int MaDocGia { get; set; }
        public string HoTen { get; set; }
        public decimal TongTienMuon { get; set; }
        public decimal TongTienPhat { get; set; }
        public decimal TongCong { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
    }

    public class ThongKeDocGiaMoiDTO
    {
        public int Thang { get; set; }
        public string TenThang { get; set; }
        public int SoDocGiaMoi { get; set; }
        public int Nam { get; set; }
    }
}
