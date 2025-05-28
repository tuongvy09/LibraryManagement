using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class CuonSach
    {
        public int MaCuonSach { get; set; }
        public int MaDauSach { get; set; }
        public string TrangThaiSach { get; set; }
    }

    public class SachHot
    {
        public string TenCuonSach { get; set; }
        public string TenDauSach { get; set; }
        public int SoLuongMuon { get; set; }
    }

}
