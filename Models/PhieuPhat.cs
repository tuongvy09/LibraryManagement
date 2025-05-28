using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class PhieuPhat
    {
        public int MaPhieuPhat { get; set; }
        public int MaDG { get; set; }         
        public string HoTen { get; set; }     
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }

        public List<QDP> LoiViPhams { get; set; } = new List<QDP>();
    }
}
