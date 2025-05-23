using LibraryManagement.Models;
using LibraryManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Services
{
    internal class BorrowService
    {
        private BorrowSlipRepository slipRepo;
        private BorrowSlipDetailRepository detailRepo;

        public BorrowService(BorrowSlipRepository s, BorrowSlipDetailRepository d)
        {
            slipRepo = s;
            detailRepo = d;
        }

        public List<BorrowSlip> GetAllSlips()
        {
            return slipRepo.GetAll();
        }

        public List<BorrowSlipDetail> GetDetailsBySlipId(int slipId)
        {
            return detailRepo.GetBySlipId(slipId);
        }
    }
}
