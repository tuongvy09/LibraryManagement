using LibraryManagement.Repositories.LibraryManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.BUS
{
    public class TacGiaBLL
    {
        private readonly TacGiaRepository _repository;

        public TacGiaBLL()
        {
            _repository = new TacGiaRepository();
        }

        public void ThemTacGia(string tenTG)
        {
            if (string.IsNullOrWhiteSpace(tenTG))
            {
                throw new ArgumentException("Tên tác giả không được để trống.");
            }

            _repository.AddTacGia(tenTG);
        }
    }
}
