using LibraryManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.BUS
{
    public class TheLoaiBLL
    {
        private readonly TheLoaiRepository _repository;

        public TheLoaiBLL()
        {
            _repository = new TheLoaiRepository();
        }

        public void ThemTheLoai(int qdSoTuoi, string tenTheLoai)
        {
            if (qdSoTuoi < 0)
            {
                throw new ArgumentException("Số tuổi không được nhỏ hơn 0.");
            }

            if (string.IsNullOrWhiteSpace(tenTheLoai))
            {
                throw new ArgumentException("Tên thể loại không được để trống.");
            }

            _repository.AddTheLoai(qdSoTuoi, tenTheLoai);
        }
    }
}
