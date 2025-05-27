using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace LibraryManagement.Models
{
    public class LoaiDocGia
    {
        public int MaLoaiDG { get; set; }
        public string TenLoaiDG { get; set; }
        public int DoTuoi { get; set; }

        // Constructor mặc định
        public LoaiDocGia()
        {
        }

        // Constructor có tham số
        public LoaiDocGia(int maLoaiDG, string tenLoaiDG, int doTuoi)
        {
            MaLoaiDG = maLoaiDG;
            TenLoaiDG = tenLoaiDG;
            DoTuoi = doTuoi;
        }

        // Override ToString để hiển thị tên loại độc giả
        public override string ToString()
        {
            return TenLoaiDG;
        }

        // Property để hiển thị thông tin đầy đủ
        public string ThongTinDayDu => $"{TenLoaiDG} (Độ tuổi: {DoTuoi})";

        // Property để kiểm tra độ tuổi hợp lệ
        public bool IsValidAge(int tuoi)
        {
            return tuoi <= DoTuoi;
        }
    }
}

