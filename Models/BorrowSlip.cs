using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class BorrowSlip
    {
        public int SlipID { get; set; }
        public int ReaderID { get; set; } // ID của độc giả mượn sách
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; } // Có thể null nếu chưa trả
    }
}
