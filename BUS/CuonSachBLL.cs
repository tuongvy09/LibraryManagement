using LibraryManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.BUS
{
    public class CuonSachBLL
    {
        private CuonSachRepository _repo;

        public CuonSachBLL()
        {
            _repo = new CuonSachRepository();
        }

        public void ThemCuonSach(int maDauSach, string trangThai, string tenCuonSach)
        {
            if (string.IsNullOrWhiteSpace(tenCuonSach))
                throw new ArgumentException("Tên cuốn sách không được để trống!");

            if (string.IsNullOrWhiteSpace(trangThai))
                throw new ArgumentException("Trạng thái sách không được để trống!");

            if (maDauSach <= 0)
                throw new ArgumentException("Mã đầu sách không hợp lệ!");

            _repo.AddCuonSach(maDauSach, trangThai, tenCuonSach);
        }
    }
}
