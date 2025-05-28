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
    public class BienLaiDAO
    {
        private DBConnection dbConnection;

        public BienLaiDAO()
        {
            dbConnection = new DBConnection();
        }

        public List<BienLai> GetAllBienLai()
        {
            List<BienLai> bienLais = new List<BienLai>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT bl.MaBienLai, bl.MaDocGia, bl.NgayTraTT, bl.HinhThucThanhToan, bl.TienTra,
                           dg.HoTen as TenDocGia, dg.SoDT, dg.CCCD
                    FROM BienLai bl
                    LEFT JOIN DocGia dg ON bl.MaDocGia = dg.MaDocGia
                    ORDER BY bl.MaBienLai DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    BienLai bienLai = new BienLai()
                    {
                        MaBienLai = Convert.ToInt32(reader["MaBienLai"]),
                        MaDocGia = reader["MaDocGia"] == DBNull.Value ? null : (int?)reader["MaDocGia"],
                        NgayTraTT = Convert.ToDateTime(reader["NgayTraTT"]),
                        HinhThucThanhToan = reader["HinhThucThanhToan"]?.ToString() ?? "",
                        TienTra = Convert.ToDecimal(reader["TienTra"]),
                        TenDocGia = reader["TenDocGia"]?.ToString() ?? "",
                        SoDT = reader["SoDT"]?.ToString() ?? "",
                        CCCD = reader["CCCD"]?.ToString() ?? ""
                    };
                    bienLais.Add(bienLai);
                }
            }
            return bienLais;
        }

        public bool InsertBienLai(BienLai bienLai)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    INSERT INTO BienLai (MaDocGia, NgayTraTT, HinhThucThanhToan, TienTra)
                    VALUES (@MaDocGia, @NgayTraTT, @HinhThucThanhToan, @TienTra)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDocGia", bienLai.MaDocGia ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NgayTraTT", bienLai.NgayTraTT);
                cmd.Parameters.AddWithValue("@HinhThucThanhToan", bienLai.HinhThucThanhToan);
                cmd.Parameters.AddWithValue("@TienTra", bienLai.TienTra);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateBienLai(BienLai bienLai)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    UPDATE BienLai SET 
                        MaDocGia = @MaDocGia,
                        NgayTraTT = @NgayTraTT,
                        HinhThucThanhToan = @HinhThucThanhToan,
                        TienTra = @TienTra
                    WHERE MaBienLai = @MaBienLai";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaBienLai", bienLai.MaBienLai);
                cmd.Parameters.AddWithValue("@MaDocGia", bienLai.MaDocGia ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NgayTraTT", bienLai.NgayTraTT);
                cmd.Parameters.AddWithValue("@HinhThucThanhToan", bienLai.HinhThucThanhToan);
                cmd.Parameters.AddWithValue("@TienTra", bienLai.TienTra);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteBienLai(int maBienLai)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = "DELETE FROM BienLai WHERE MaBienLai = @MaBienLai";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaBienLai", maBienLai);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<BienLai> SearchBienLai(string searchText)
        {
            List<BienLai> bienLais = new List<BienLai>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT bl.MaBienLai, bl.MaDocGia, bl.NgayTraTT, bl.HinhThucThanhToan, bl.TienTra,
                           dg.HoTen as TenDocGia, dg.SoDT, dg.CCCD
                    FROM BienLai bl
                    LEFT JOIN DocGia dg ON bl.MaDocGia = dg.MaDocGia
                    WHERE CAST(bl.MaBienLai AS NVARCHAR) LIKE @SearchText
                       OR dg.HoTen LIKE @SearchText
                       OR dg.SoDT LIKE @SearchText
                       OR dg.CCCD LIKE @SearchText
                       OR bl.HinhThucThanhToan LIKE @SearchText
                    ORDER BY bl.MaBienLai DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    BienLai bienLai = new BienLai()
                    {
                        MaBienLai = Convert.ToInt32(reader["MaBienLai"]),
                        MaDocGia = reader["MaDocGia"] == DBNull.Value ? null : (int?)reader["MaDocGia"],
                        NgayTraTT = Convert.ToDateTime(reader["NgayTraTT"]),
                        HinhThucThanhToan = reader["HinhThucThanhToan"]?.ToString() ?? "",
                        TienTra = Convert.ToDecimal(reader["TienTra"]),
                        TenDocGia = reader["TenDocGia"]?.ToString() ?? "",
                        SoDT = reader["SoDT"]?.ToString() ?? "",
                        CCCD = reader["CCCD"]?.ToString() ?? ""
                    };
                    bienLais.Add(bienLai);
                }
            }
            return bienLais;
        }

        public BienLai GetBienLaiById(int maBienLai)
        {
            BienLai bienLai = null;

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT bl.MaBienLai, bl.MaDocGia, bl.NgayTraTT, bl.HinhThucThanhToan, bl.TienTra,
                           dg.HoTen as TenDocGia, dg.SoDT, dg.CCCD
                    FROM BienLai bl
                    LEFT JOIN DocGia dg ON bl.MaDocGia = dg.MaDocGia
                    WHERE bl.MaBienLai = @MaBienLai";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaBienLai", maBienLai);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    bienLai = new BienLai()
                    {
                        MaBienLai = Convert.ToInt32(reader["MaBienLai"]),
                        MaDocGia = reader["MaDocGia"] == DBNull.Value ? null : (int?)reader["MaDocGia"],
                        NgayTraTT = Convert.ToDateTime(reader["NgayTraTT"]),
                        HinhThucThanhToan = reader["HinhThucThanhToan"]?.ToString() ?? "",
                        TienTra = Convert.ToDecimal(reader["TienTra"]),
                        TenDocGia = reader["TenDocGia"]?.ToString() ?? "",
                        SoDT = reader["SoDT"]?.ToString() ?? "",
                        CCCD = reader["CCCD"]?.ToString() ?? ""
                    };
                }
            }
            return bienLai;
        }

        // Lấy danh sách độc giả để binding vào ComboBox
        public DataTable GetDocGiaForComboBox()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT MaDocGia, HoTen as TenDocGia 
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

        // Lấy danh sách hình thức thanh toán
        public List<string> GetHinhThucThanhToan()
        {
            return new List<string>
            {
                "Tiền mặt",
                "Chuyển khoản",
                "Thẻ tín dụng",
                "Ví điện tử"
            };
        }

        // Thống kê doanh thu theo tháng
        public List<ThongKeDoanhThuDTO> GetThongKeDoanhThuTheoThang(int nam)
        {
            List<ThongKeDoanhThuDTO> results = new List<ThongKeDoanhThuDTO>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT 
                        MONTH(NgayTraTT) as Thang,
                        SUM(TienTra) as TongTien,
                        COUNT(*) as SoLuongBienLai
                    FROM BienLai 
                    WHERE YEAR(NgayTraTT) = @Nam
                    GROUP BY MONTH(NgayTraTT)
                    ORDER BY Thang";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nam", nam);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var result = new ThongKeDoanhThuDTO
                    {
                        Thang = Convert.ToInt32(reader["Thang"]),
                        TenThang = "Tháng " + Convert.ToInt32(reader["Thang"]),
                        TongTien = Convert.ToDecimal(reader["TongTien"]),
                        SoLuongBienLai = Convert.ToInt32(reader["SoLuongBienLai"]),
                        Nam = nam
                    };
                    results.Add(result);
                }
            }
            return results;
        }
    }

    // DTO cho thống kê doanh thu
    public class ThongKeDoanhThuDTO
    {
        public int Thang { get; set; }
        public string TenThang { get; set; }
        public decimal TongTien { get; set; }
        public int SoLuongBienLai { get; set; }
        public int Nam { get; set; }
    }
}