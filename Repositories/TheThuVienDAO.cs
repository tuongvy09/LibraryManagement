using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Repositories
{
    public class TheThuVienDAO
    {
        private DBConnection dbConnection;

        public TheThuVienDAO()
        {
            dbConnection = new DBConnection();
        }

        public List<TheThuVien> GetAllTheThuVien()
        {
            List<TheThuVien> theThuViens = new List<TheThuVien>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT tt.MaThe, tt.MaDG, tt.NgayCap, tt.NgayHetHan,
                           dg.HoTen as TenDocGia, dg.SoDT
                    FROM TheThuVien tt
                    LEFT JOIN DocGia dg ON tt.MaDG = dg.MaDocGia
                    ORDER BY tt.MaThe DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TheThuVien theThuVien = new TheThuVien()
                    {
                        MaThe = Convert.ToInt32(reader["MaThe"]),
                        MaDG = reader["MaDG"] == DBNull.Value ? null : (int?)reader["MaDG"],
                        NgayCap = Convert.ToDateTime(reader["NgayCap"]),
                        NgayHetHan = Convert.ToDateTime(reader["NgayHetHan"]),
                        TenDocGia = reader["TenDocGia"]?.ToString() ?? "",
                        SoDT = reader["SoDT"]?.ToString() ?? ""
                    };
                    theThuViens.Add(theThuVien);
                }
            }
            return theThuViens;
        }

        public bool InsertTheThuVien(TheThuVien theThuVien)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    INSERT INTO TheThuVien (MaDG, NgayCap, NgayHetHan)
                    VALUES (@MaDG, @NgayCap, @NgayHetHan)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDG", theThuVien.MaDG ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NgayCap", theThuVien.NgayCap);
                cmd.Parameters.AddWithValue("@NgayHetHan", theThuVien.NgayHetHan);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // NEW: Insert và return ID của thẻ vừa tạo
        public int InsertTheThuVienAndGetId(TheThuVien theThuVien)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    INSERT INTO TheThuVien (MaDG, NgayCap, NgayHetHan)
                    OUTPUT INSERTED.MaThe
                    VALUES (@MaDG, @NgayCap, @NgayHetHan)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDG", theThuVien.MaDG ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NgayCap", theThuVien.NgayCap);
                cmd.Parameters.AddWithValue("@NgayHetHan", theThuVien.NgayHetHan);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        // NEW: Lấy thẻ mới nhất của độc giả
        public TheThuVien GetLatestCardByDocGia(int maDG)
        {
            TheThuVien theThuVien = null;

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT TOP 1 tt.MaThe, tt.MaDG, tt.NgayCap, tt.NgayHetHan,
                           dg.HoTen as TenDocGia, dg.SoDT
                    FROM TheThuVien tt
                    LEFT JOIN DocGia dg ON tt.MaDG = dg.MaDocGia
                    WHERE tt.MaDG = @MaDG
                    ORDER BY tt.MaThe DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDG", maDG);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    theThuVien = new TheThuVien()
                    {
                        MaThe = Convert.ToInt32(reader["MaThe"]),
                        MaDG = reader["MaDG"] == DBNull.Value ? null : (int?)reader["MaDG"],
                        NgayCap = Convert.ToDateTime(reader["NgayCap"]),
                        NgayHetHan = Convert.ToDateTime(reader["NgayHetHan"]),
                        TenDocGia = reader["TenDocGia"]?.ToString() ?? "",
                        SoDT = reader["SoDT"]?.ToString() ?? ""
                    };
                }
            }
            return theThuVien;
        }

        public bool UpdateTheThuVien(TheThuVien theThuVien)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    UPDATE TheThuVien SET 
                        MaDG = @MaDG,
                        NgayCap = @NgayCap,
                        NgayHetHan = @NgayHetHan
                    WHERE MaThe = @MaThe";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaThe", theThuVien.MaThe);
                cmd.Parameters.AddWithValue("@MaDG", theThuVien.MaDG ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NgayCap", theThuVien.NgayCap);
                cmd.Parameters.AddWithValue("@NgayHetHan", theThuVien.NgayHetHan);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteTheThuVien(int maThe)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                // Hard delete vì thẻ thư viện không có trạng thái
                string query = "DELETE FROM TheThuVien WHERE MaThe = @MaThe";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaThe", maThe);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<TheThuVien> SearchTheThuVien(string searchText)
        {
            List<TheThuVien> theThuViens = new List<TheThuVien>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT tt.MaThe, tt.MaDG, tt.NgayCap, tt.NgayHetHan,
                           dg.HoTen as TenDocGia, dg.SoDT
                    FROM TheThuVien tt
                    LEFT JOIN DocGia dg ON tt.MaDG = dg.MaDocGia
                    WHERE CAST(tt.MaThe AS NVARCHAR) LIKE @SearchText
                       OR dg.HoTen LIKE @SearchText
                       OR dg.SoDT LIKE @SearchText
                    ORDER BY tt.MaThe DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TheThuVien theThuVien = new TheThuVien()
                    {
                        MaThe = Convert.ToInt32(reader["MaThe"]),
                        MaDG = reader["MaDG"] == DBNull.Value ? null : (int?)reader["MaDG"],
                        NgayCap = Convert.ToDateTime(reader["NgayCap"]),
                        NgayHetHan = Convert.ToDateTime(reader["NgayHetHan"]),
                        TenDocGia = reader["TenDocGia"]?.ToString() ?? "",
                        SoDT = reader["SoDT"]?.ToString() ?? ""
                    };
                    theThuViens.Add(theThuVien);
                }
            }
            return theThuViens;
        }

        public TheThuVien GetTheThuVienById(int maThe)
        {
            TheThuVien theThuVien = null;

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT tt.MaThe, tt.MaDG, tt.NgayCap, tt.NgayHetHan,
                           dg.HoTen as TenDocGia, dg.SoDT
                    FROM TheThuVien tt
                    LEFT JOIN DocGia dg ON tt.MaDG = dg.MaDocGia
                    WHERE tt.MaThe = @MaThe";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaThe", maThe);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    theThuVien = new TheThuVien()
                    {
                        MaThe = Convert.ToInt32(reader["MaThe"]),
                        MaDG = reader["MaDG"] == DBNull.Value ? null : (int?)reader["MaDG"],
                        NgayCap = Convert.ToDateTime(reader["NgayCap"]),
                        NgayHetHan = Convert.ToDateTime(reader["NgayHetHan"]),
                        TenDocGia = reader["TenDocGia"]?.ToString() ?? "",
                        SoDT = reader["SoDT"]?.ToString() ?? ""
                    };
                }
            }
            return theThuVien;
        }

        // Lấy danh sách độc giả để binding vào ComboBox
        public DataTable GetDocGiaForComboBox()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT MaDocGia as MaDG, HoTen as TenDocGia 
                    FROM DocGia 
                    WHERE TrangThai = 1
                    ORDER BY HoTen";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }

        // Kiểm tra độc giả đã có thẻ thư viện chưa
        public bool CheckDocGiaHasCard(int maDG, int? excludeMaThe = null)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM TheThuVien WHERE MaDG = @MaDG";

                if (excludeMaThe.HasValue)
                {
                    query += " AND MaThe != @ExcludeMaThe";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDG", maDG);

                if (excludeMaThe.HasValue)
                {
                    cmd.Parameters.AddWithValue("@ExcludeMaThe", excludeMaThe.Value);
                }

                conn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        // Thống kê số thẻ được cấp theo tháng
        public List<ThongKeTheThuVienDTO> GetThongKeTheThuVienTheoThang(int nam)
        {
            List<ThongKeTheThuVienDTO> results = new List<ThongKeTheThuVienDTO>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT 
                        MONTH(NgayCap) as Thang,
                        COUNT(*) as SoTheCapMoi
                    FROM TheThuVien 
                    WHERE YEAR(NgayCap) = @Nam
                    GROUP BY MONTH(NgayCap)
                    ORDER BY Thang";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nam", nam);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var result = new ThongKeTheThuVienDTO
                    {
                        Thang = Convert.ToInt32(reader["Thang"]),
                        TenThang = "Tháng " + Convert.ToInt32(reader["Thang"]),
                        SoTheCapMoi = Convert.ToInt32(reader["SoTheCapMoi"]),
                        Nam = nam
                    };
                    results.Add(result);
                }
            }
            return results;
        }

        // Thống kê thẻ hết hạn
        public List<TheThuVien> GetTheHetHan()
        {
            List<TheThuVien> theThuViens = new List<TheThuVien>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT tt.MaThe, tt.MaDG, tt.NgayCap, tt.NgayHetHan,
                           dg.HoTen as TenDocGia, dg.SoDT
                    FROM TheThuVien tt
                    LEFT JOIN DocGia dg ON tt.MaDG = dg.MaDocGia
                    WHERE tt.NgayHetHan < GETDATE()
                    ORDER BY tt.NgayHetHan ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TheThuVien theThuVien = new TheThuVien()
                    {
                        MaThe = Convert.ToInt32(reader["MaThe"]),
                        MaDG = reader["MaDG"] == DBNull.Value ? null : (int?)reader["MaDG"],
                        NgayCap = Convert.ToDateTime(reader["NgayCap"]),
                        NgayHetHan = Convert.ToDateTime(reader["NgayHetHan"]),
                        TenDocGia = reader["TenDocGia"]?.ToString() ?? "",
                        SoDT = reader["SoDT"]?.ToString() ?? ""
                    };
                    theThuViens.Add(theThuVien);
                }
            }
            return theThuViens;
        }

        // Thống kê thẻ sắp hết hạn (trong vòng 30 ngày)
        public List<TheThuVien> GetTheSapHetHan(int soNgay = 30)
        {
            List<TheThuVien> theThuViens = new List<TheThuVien>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT tt.MaThe, tt.MaDG, tt.NgayCap, tt.NgayHetHan,
                           dg.HoTen as TenDocGia, dg.SoDT
                    FROM TheThuVien tt
                    LEFT JOIN DocGia dg ON tt.MaDG = dg.MaDocGia
                    WHERE tt.NgayHetHan BETWEEN GETDATE() AND DATEADD(DAY, @SoNgay, GETDATE())
                    ORDER BY tt.NgayHetHan ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SoNgay", soNgay);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TheThuVien theThuVien = new TheThuVien()
                    {
                        MaThe = Convert.ToInt32(reader["MaThe"]),
                        MaDG = reader["MaDG"] == DBNull.Value ? null : (int?)reader["MaDG"],
                        NgayCap = Convert.ToDateTime(reader["NgayCap"]),
                        NgayHetHan = Convert.ToDateTime(reader["NgayHetHan"]),
                        TenDocGia = reader["TenDocGia"]?.ToString() ?? "",
                        SoDT = reader["SoDT"]?.ToString() ?? ""
                    };
                    theThuViens.Add(theThuVien);
                }
            }
            return theThuViens;
        }
    }

    // DTO cho thống kê thẻ thư viện
    public class ThongKeTheThuVienDTO
    {
        public int Thang { get; set; }
        public string TenThang { get; set; }
        public int SoTheCapMoi { get; set; }
        public int Nam { get; set; }
    }
}
