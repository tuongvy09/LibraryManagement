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

        public int AddDauSach(string tenDauSach, int maTheLoai, int maNXB)
        {
            using (SqlConnection conn = GetConnection())
            {
                string query = @"INSERT INTO DauSach (MaTheLoai, MaNXB, TenDauSach) 
                         OUTPUT INSERTED.MaDauSach 
                         VALUES (@MaTheLoai, @MaNXB, @TenDauSach)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenDauSach", tenDauSach);
                    cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
                    cmd.Parameters.AddWithValue("@MaNXB", maNXB);

                    conn.Open();
                    int maDauSach = (int)cmd.ExecuteScalar();
                    conn.Close();
                    return maDauSach;
                }
            }
        }

        public void UpdateDauSach(int maDauSach, int maTheLoai, int maNXB)
        {
            string query = "UPDATE DauSach SET MaTheLoai = @MaTheLoai, MaNXB = @MaNXB WHERE MaDauSach = @MaDauSach";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDauSach", maDauSach);
                    cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
                    cmd.Parameters.AddWithValue("@MaNXB", maNXB);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Xóa đầu sách
        public void DeleteDauSach(int maDauSach)
        {
            string query = "DELETE FROM DauSach WHERE MaDauSach = @MaDauSach";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDauSach", maDauSach);
                    cmd.ExecuteNonQuery();
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
                                TacGia = reader.IsDBNull(6) ? null : reader.GetString(6)
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
    STRING_AGG(tg.TenTG, ', ') AS TacGia
FROM DauSach ds
INNER JOIN TheLoai tl ON ds.MaTheLoai = tl.MaTheLoai
LEFT JOIN NXB nxb ON ds.MaNXB = nxb.MaNXB
LEFT JOIN DauSach_TacGia dstg ON ds.MaDauSach = dstg.MaDauSach
LEFT JOIN TacGia tg ON dstg.MaTacGia = tg.MaTacGia
GROUP BY ds.MaDauSach, ds.TenDauSach, tl.MaTheLoai, tl.TenTheLoai, nxb.MaNXB, nxb.TenNSB
ORDER BY ds.TenDauSach
";

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
                                TacGia = reader.IsDBNull(6) ? null : reader.GetString(6)
                            };
                            list.Add(item);
                        }
                    }
                }
            }

            return list;
        }

    }

}
