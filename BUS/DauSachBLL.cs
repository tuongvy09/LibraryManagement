using LibraryManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.BUS
{
    public class DauSachBLL
    {
        private readonly DauSachRepository _repo;

        public DauSachBLL()
        {
            _repo = new DauSachRepository();
        }

        public void XoaDauSach(int maDauSach)
        {
            _repo.DeleteDauSach(maDauSach);
        }
    }
}
