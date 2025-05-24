using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class CuonSachDetailModel
    {
        public int MaCuonSach { get; set; }
        public int MaDauSach { get; set; }
        public string TenDauSach { get; set; }
        public string TenCuonSach { get; set; }
        public string TrangThaiSach { get; set; }
        public string  TenTheLoai{ get; set; }
        public int QDSoTuoi { get; set; }
        public string TenNSB { get; set; }

        public List<string> TacGias { get; set; }
    }

}
