using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class QDP
    {
        public int MaQDP { get; set; }
        public string LyDo { get; set; }
        public decimal TienPhat { get; set; }

        public override string ToString()
        {
            return $"{LyDo} - {TienPhat} đ";
        }
    }
}
