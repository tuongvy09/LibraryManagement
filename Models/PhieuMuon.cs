using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class PhieuMuon
    {
        public int MaMuonSach { get; set; }
        public int MaPhieu { get; set; }
        public int MaDocGia { get; set; }
        public string TenDocGia { get; set; }
        private string _tenCuonSach;

        public string TenCuonSach
        {
            get
            {
                return DanhSachTenCuonSach != null
                    ? string.Join(", ", DanhSachTenCuonSach)
                    : _tenCuonSach;
            }
            set
            {
                _tenCuonSach = value;
            }
        }
        public List<string> DanhSachTenCuonSach { get; set; }
        public DateTime NgayMuon { get; set; }
        public DateTime NgayTra { get; set; }
        public string TrangThaiM { get; set; }
        public decimal GiaMuon { get; set; }
        public int SoNgayMuon { get; set; }
        public decimal TienCoc { get; set; }
    }
}
