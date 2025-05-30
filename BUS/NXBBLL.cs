using LibraryManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.BUS
{
    public class NXBBLL
    {
        private readonly NXBRepository _repository;

        public NXBBLL()
        {
            _repository = new NXBRepository();
        }

        public void ThemNXB(string tenNXB)
        {
            if (string.IsNullOrWhiteSpace(tenNXB))
                throw new ArgumentException("Tên nhà xuất bản không được để trống.");
            _repository.AddNXB(tenNXB);
        }
    }
}
