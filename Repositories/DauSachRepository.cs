using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryManagement.Models;

namespace LibraryManagement.Repositories
{
    public class DauSachRepository : DBConnection
    {
        public List<DauSach> GetAllMaDauSach()
        {
            List<DauSach> list = new List<DauSach>();
            string query = "SELECT MaDauSach, TenDauSach FROM DauSach";

            var db = new DBConnection();
            using (SqlConnection conn = db.GetConnection())
            {
                if (conn == null)
                {
                    MessageBox.Show("Không thể kết nối đến database.");
                    return list;
                }

                conn.Open();  // Mở kết nối đúng đối tượng này

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new DauSach()
                            {
                                MaDauSach = reader.GetInt32(0),
                                TenDauSach = reader.GetString(1)
                            });
                        }
                    }
                }

                conn.Close(); // Đóng kết nối
            }

            return list;
        }

        public int AddDauSach(string tenDauSach, int maTheLoai, int maNXB, int? namXuatBan, decimal? giaTien, int? soTrang, string ngonNgu, string mota)
        {
            using (SqlConnection conn = GetConnection())
            {
                string query = @"
            INSERT INTO DauSach (MaTheLoai, MaNXB, TenDauSach, NamXuatBan, GiaTien, SoTrang, NgonNgu, MoTa) 
            OUTPUT INSERTED.MaDauSach 
            VALUES (@MaTheLoai, @MaNXB, @TenDauSach, @NamXuatBan, @GiaTien, @SoTrang, @NgonNgu, @MoTa)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenDauSach", tenDauSach);
                    cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
                    cmd.Parameters.AddWithValue("@MaNXB", maNXB);
                    cmd.Parameters.AddWithValue("@NamXuatBan", (object)namXuatBan ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@GiaTien", (object)giaTien ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SoTrang", (object)soTrang ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NgonNgu", (object)ngonNgu ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MoTa", (object)mota ?? DBNull.Value);

                    conn.Open();
                    int maDauSach = (int)cmd.ExecuteScalar();
                    conn.Close();
                    return maDauSach;
                }
            }
        }

        public void UpdateDauSach(int maDauSach, int maTheLoai, int maNXB, int? namXuatBan, decimal? giaTien, int? soTrang, string ngonNgu, string mota)
        {
            string query = @"UPDATE DauSach 
                     SET MaTheLoai = @MaTheLoai, MaNXB = @MaNXB, NamXuatBan = @NamXuatBan, GiaTien = @GiaTien, SoTrang = @SoTrang, NgonNgu = @NgonNgu, @MoTa = MoTa
                     WHERE MaDauSach = @MaDauSach";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDauSach", maDauSach);
                    cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
                    cmd.Parameters.AddWithValue("@MaNXB", maNXB);
                    cmd.Parameters.AddWithValue("@NamXuatBan", (object)namXuatBan ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@GiaTien", (object)giaTien ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SoTrang", (object)soTrang ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NgonNgu", (object)ngonNgu ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MoTa", (object)mota ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Xóa đầu sách
        public void DeleteDauSach(int maDauSach)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Xóa các tác giả liên quan trước
                        string deleteTacGiaQuery = "DELETE FROM DauSach_TacGia WHERE MaDauSach = @MaDauSach";
                        using (SqlCommand cmd1 = new SqlCommand(deleteTacGiaQuery, conn, transaction))
                        {
                            cmd1.Parameters.AddWithValue("@MaDauSach", maDauSach);
                            cmd1.ExecuteNonQuery();
                        }

                        // Sau đó mới xóa đầu sách
                        string deleteDauSachQuery = "DELETE FROM DauSach WHERE MaDauSach = @MaDauSach";
                        using (SqlCommand cmd2 = new SqlCommand(deleteDauSachQuery, conn, transaction))
                        {
                            cmd2.Parameters.AddWithValue("@MaDauSach", maDauSach);
                            cmd2.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw; // Đẩy lỗi ra ngoài để báo
                    }
                }
            }
        }

        // Tìm kiếm đầu sách theo tên (LIKE)
        public List<DauSachDTO> SearchDauSach(string keyword)
        {
            List<DauSachDTO> result = new List<DauSachDTO>();

            string query = @"
        SELECT 
            ds.MaDauSach,
            ds.TenDauSach,
            tl.MaTheLoai,
            tl.TenTheLoai,
            nxb.MaNXB,
            nxb.TenNSB,
            ds.NamXuatBan,
            ds.GiaTien,
            ds.SoTrang,
            ds.NgonNgu,
            ds.MoTa,
            STUFF((
                SELECT ', ' + tg.TenTG
                FROM DauSach_TacGia dstg
                INNER JOIN TacGia tg ON dstg.MaTacGia = tg.MaTacGia
                WHERE dstg.MaDauSach = ds.MaDauSach
                FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS TacGia
        FROM DauSach ds
        INNER JOIN TheLoai tl ON ds.MaTheLoai = tl.MaTheLoai
        LEFT JOIN NXB nxb ON ds.MaNXB = nxb.MaNXB
        WHERE ds.TenDauSach LIKE @Keyword";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DauSachDTO dauSach = new DauSachDTO()
                            {
                                MaDauSach = reader.GetInt32(0),
                                TenDauSach = reader.GetString(1),
                                MaTheLoai = reader.GetInt32(2),
                                TenTheLoai = reader.GetString(3),
                                MaNXB = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                                TenNXB = reader.IsDBNull(5) ? null : reader.GetString(5),
                                NamXuatBan = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),
                                GiaTien = reader.IsDBNull(7) ? (decimal?)null : reader.GetDecimal(7),
                                SoTrang = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                                NgonNgu = reader.IsDBNull(9) ? null : reader.GetString(9),
                                TacGia = reader.IsDBNull(10) ? null : reader.GetString(10),
                                MoTa = reader.IsDBNull(9) ? null : reader.GetString(11),
                            };

                            result.Add(dauSach);
                        }
                    }
                }
            }

            return result;
        }

        public List<DauSachDTO> GetAllDauSachFullInfo()
        {
            List<DauSachDTO> list = new List<DauSachDTO>();

            string query = @"
        SELECT 
    ds.MaDauSach,
    ds.TenDauSach,
    tl.MaTheLoai,
    tl.TenTheLoai,
    nxb.MaNXB,
    nxb.TenNSB,
    ds.NamXuatBan,
    ds.GiaTien,
    ds.SoTrang,
    ds.NgonNgu,
    ds.MoTa,
    STRING_AGG(tg.TenTG, ', ') AS TacGia
FROM DauSach ds
INNER JOIN TheLoai tl ON ds.MaTheLoai = tl.MaTheLoai
LEFT JOIN NXB nxb ON ds.MaNXB = nxb.MaNXB
LEFT JOIN DauSach_TacGia dstg ON ds.MaDauSach = dstg.MaDauSach
LEFT JOIN TacGia tg ON dstg.MaTacGia = tg.MaTacGia
GROUP BY 
    ds.MaDauSach, ds.TenDauSach, ds.MoTa,
    tl.MaTheLoai, tl.TenTheLoai, 
    nxb.MaNXB, nxb.TenNSB, 
    ds.NamXuatBan, ds.GiaTien, ds.SoTrang, ds.NgonNgu
ORDER BY ds.TenDauSach
";

            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DauSachDTO item = new DauSachDTO()
                                {
                                    MaDauSach = reader.GetInt32(0),
                                    TenDauSach = reader.GetString(1),
                                    MaTheLoai = reader.GetInt32(2),
                                    TenTheLoai = reader.GetString(3),
                                    MaNXB = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                                    TenNXB = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    NamXuatBan = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),
                                    GiaTien = reader.IsDBNull(7) ? (decimal?)null : reader.GetDecimal(7),
                                    SoTrang = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                                    NgonNgu = reader.IsDBNull(9) ? null : reader.GetString(9),
                                    MoTa = reader.IsDBNull(10) ? null : reader.GetString(10),
                                    TacGia = reader.IsDBNull(11) ? null : reader.GetString(11)
                                };
                                list.Add(item);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                // hoặc log vào file/log system
            }

            return list;
        }

    }

}
