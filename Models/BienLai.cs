using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class BienLai
    {
        public int MaBienLai { get; set; }
        public int? MaDocGia { get; set; }
        public DateTime NgayTraTT { get; set; }
        public string HinhThucThanhToan { get; set; }
        public decimal TienTra { get; set; }

        // Navigation properties
        public string TenDocGia { get; set; }
        public string SoDT { get; set; }
        public string CCCD { get; set; }
    }
}
