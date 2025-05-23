using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class BorrowSlipDetail
    {
        public int SlipDetailID { get; set; }
        public int SlipID { get; set; }
        public int BookID { get; set; }
        public bool IsReturned { get; set; } // Trạng thái đã trả hay chưa
    }
}
