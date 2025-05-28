using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using LibraryManagement.Models;

namespace LibraryManagement.Repositories
{
    public class CuonSachRepository : DBConnection
    {
        // Thêm Cuốn Sách
        public void AddCuonSach(int maDauSach, string trangThaiSach, string tenCuonSach)
        {
            string query = "INSERT INTO CuonSach (MaDauSach, TrangThaiSach, TenCuonSach) VALUES (@MaDauSach, @TrangThaiSach, @TenCuonSach)";
            SqlCommand cmd = new SqlCommand(query, GetConnection());
            cmd.Parameters.AddWithValue("@MaDauSach", maDauSach);
            cmd.Parameters.AddWithValue("@TrangThaiSach", trangThaiSach);
            cmd.Parameters.AddWithValue("@TenCuonSach", tenCuonSach);
            GetConnection().Open();
            cmd.ExecuteNonQuery();
            GetConnection().Close();
        }

        // Cập nhật Cuốn Sách
        public void UpdateCuonSach(
            int maCuonSach,
            string tenDauSach,     // Tên của đầu sách
            int maDauSach,
            int maTheLoai,
            int maNXB,
            string trangThai,
            List<int> danhSachMaTacGia)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                // Lấy MaDauSach từ MaCuonSach
                using (SqlCommand cmd = new SqlCommand("SELECT MaDauSach FROM CuonSach WHERE MaCuonSach = @MaCuonSach", conn))
                {
                    cmd.Parameters.AddWithValue("@MaCuonSach", maCuonSach);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        maDauSach = Convert.ToInt32(result);
                    }
                    else
                    {
                        throw new Exception("Không tìm thấy cuốn sách.");
                    }
                }

                // Cập nhật DauSach
                string updateDauSach = @"UPDATE DauSach 
                                 SET TenDauSach = @TenDauSach, 
                                     MaTheLoai = @MaTheLoai, 
                                     MaNXB = @MaNXB 
                                 WHERE MaDauSach = @MaDauSach";
                using (SqlCommand cmd = new SqlCommand(updateDauSach, conn))
                {
                    cmd.Parameters.AddWithValue("@TenDauSach", tenDauSach);
                    cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
                    cmd.Parameters.AddWithValue("@MaNXB", maNXB);
                    cmd.Parameters.AddWithValue("@MaDauSach", maDauSach);
                    cmd.ExecuteNonQuery();
                }

                // Cập nhật TrangThaiSach trong CuonSach
                using (SqlCommand cmd = new SqlCommand("UPDATE CuonSach SET TrangThaiSach = @TrangThai WHERE MaCuonSach = @MaCuonSach", conn))
                {
                    cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                    cmd.Parameters.AddWithValue("@MaCuonSach", maCuonSach);
                    cmd.ExecuteNonQuery();
                }

                // Cập nhật lại danh sách tác giả
                UpdateTacGia_DauSach(conn, maDauSach, danhSachMaTacGia);

                conn.Close();
            }
        }

        private void UpdateTacGia_DauSach(SqlConnection conn, int maDauSach, List<int> danhSachMaTacGia)
        {
            // Xóa tác giả cũ
            using (SqlCommand deleteCmd = new SqlCommand("DELETE FROM DauSach_TacGia WHERE MaDauSach = @MaDauSach", conn))
            {
                deleteCmd.Parameters.AddWithValue("@MaDauSach", maDauSach);
                deleteCmd.ExecuteNonQuery();
            }

            // Thêm mới
            foreach (int maTacGia in danhSachMaTacGia)
            {
                using (SqlCommand insertCmd = new SqlCommand("INSERT INTO DauSach_TacGia (MaDauSach, MaTacGia) VALUES (@MaDauSach, @MaTacGia)", conn))
                {
                    insertCmd.Parameters.AddWithValue("@MaDauSach", maDauSach);
                    insertCmd.Parameters.AddWithValue("@MaTacGia", maTacGia);
                    insertCmd.ExecuteNonQuery();
                }
            }
        }

        // Xoá Cuốn Sách
        public void DeleteCuonSach(int maCuonSach)
        {
            string query = "DELETE FROM CuonSach WHERE MaCuonSach = @MaCuonSach";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaCuonSach", maCuonSach);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<CuonSachDetailModel> GetAllCuonSachDetails()
        {
            List<CuonSachDetailModel> result = new List<CuonSachDetailModel>();
            string query = @"
        SELECT 
            ds.TenDauSach,
            cs.TenCuonSach,
            cs.MaCuonSach,
            cs.TrangThaiSach,
            tl.TenTheLoai,
            tl.QDSoTuoi,
            nxb.TenNSB,
            STUFF((
                SELECT ', ' + tg.TenTG
                FROM DauSach_TacGia dstg
                INNER JOIN TacGia tg ON dstg.MaTacGia = tg.MaTacGia
                WHERE dstg.MaDauSach = ds.MaDauSach
                FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS TacGias
        FROM CuonSach cs
        INNER JOIN DauSach ds ON cs.MaDauSach = ds.MaDauSach
        INNER JOIN TheLoai tl ON ds.MaTheLoai = tl.MaTheLoai
        INNER JOIN NXB nxb ON ds.MaNXB = nxb.MaNXB";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new CuonSachDetailModel
                            {
                                MaCuonSach = Convert.ToInt32(reader["MaCuonSach"]),
                                TenDauSach = reader["TenDauSach"].ToString(),
                                TenCuonSach = reader["TenCuonSach"].ToString(),
                                TrangThaiSach = reader["TrangThaiSach"].ToString(),
                                TenTheLoai = reader["TenTheLoai"].ToString(),
                                QDSoTuoi = Convert.ToInt32(reader["QDSoTuoi"]),
                                TenNSB = reader["TenNSB"].ToString(),
                                TacGias = reader["TacGias"].ToString()
                                            .Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
                                            .ToList()
                            };

                            result.Add(item);
                        }
                    }
                }
            }
            return result;
        }

        public List<CuonSachDetailModel> SearchCuonSach(string keyword)
        {
            List<CuonSachDetailModel> result = new List<CuonSachDetailModel>();

            string query = @"
        SELECT cs.TenCuonSach, cs.TrangThaiSach, tl.TenTheLoai, tl.QDSoTuoi,
               nxb.TenNSB, ds.TenDauSach, tg.TenTG
        FROM CuonSach cs
        JOIN DauSach ds ON cs.MaDauSach = ds.MaDauSach
        JOIN TheLoai tl ON ds.MaTheLoai = tl.MaTheLoai
        JOIN NXB nxb ON ds.MaNXB = nxb.MaNXB
        JOIN DauSach_TacGia tgds ON ds.MaDauSach = tgds.MaDauSach
        JOIN TacGia tg ON tgds.MaTacGia = tg.MaTacGia
        WHERE cs.TenCuonSach LIKE @Keyword
           OR tg.TenTG LIKE @Keyword
           OR nxb.TenNSB LIKE @Keyword
    ";
            var db = new DBConnection();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cuonSach = new CuonSachDetailModel
                            {
                                TenCuonSach = reader["TenCuonSach"].ToString(),
                                TrangThaiSach = reader["TrangThaiSach"].ToString(),
                                TenTheLoai = reader["TenTheLoai"].ToString(),
                                QDSoTuoi = Convert.ToInt32(reader["QDSoTuoi"]),
                                TenNSB = reader["TenNSB"].ToString(),
                                TenDauSach = reader["TenDauSach"].ToString(),
                                TacGias = new List<string> { reader["TenTG"].ToString() }
                            };

                            // Gộp tên tác giả nếu có trùng cuốn sách
                            var existing = result.FirstOrDefault(x => x.TenCuonSach == cuonSach.TenCuonSach);
                            if (existing != null)
                            {
                                if (!existing.TacGias.Contains(cuonSach.TacGias[0]))
                                    existing.TacGias.Add(cuonSach.TacGias[0]);
                            }
                            else
                            {
                                result.Add(cuonSach);
                            }
                        }
                    }
                }

                conn.Close();
            }

            return result;
        }

        public CuonSachDetailModel GetCuonSachById(int maCuonSach)
        {
            CuonSachDetailModel result = null;

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                // Lấy thông tin cơ bản của cuốn sách
                string query = @"
            SELECT 
                cs.TenCuonSach, cs.TrangThaiSach,
                ds.TenDauSach,
                ds.MaDauSach,
                tl.TenTheLoai, tl.QDSoTuoi,
                nxb.TenNSB
            FROM CuonSach cs
            JOIN DauSach ds ON cs.MaDauSach = ds.MaDauSach
            JOIN TheLoai tl ON ds.MaTheLoai = tl.MaTheLoai
            JOIN NXB nxb ON ds.MaNXB = nxb.MaNXB
            WHERE cs.MaCuonSach = @maCuonSach";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@maCuonSach", maCuonSach);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new CuonSachDetailModel
                        {
                            TenCuonSach = reader.GetString(0),
                            TrangThaiSach = reader.GetString(1),
                            TenDauSach = reader.GetString(2),
                            MaDauSach = reader.GetInt32(3),
                            TenTheLoai = reader.GetString(4),
                            QDSoTuoi = reader.GetInt32(5),
                            TenNSB = reader.GetString(6),
                            TacGias = new List<string>(),
                        };
                    }
                }

                // Lấy danh sách tác giả
                if (result != null)
                {
                    string tacGiaQuery = @"
                    SELECT tg.MaTacGia, tg.TenTG
                    FROM TacGia tg
                    JOIN DauSach_TacGia dstg ON tg.MaTacGia = dstg.MaTacGia
                    JOIN CuonSach cs ON dstg.MaDauSach = cs.MaDauSach
                    WHERE cs.MaCuonSach = @maCuonSach;";

                    SqlCommand tgCmd = new SqlCommand(tacGiaQuery, conn);
                    tgCmd.Parameters.AddWithValue("@maCuonSach", maCuonSach);

                    using (SqlDataReader reader = tgCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.TacGias.Add(reader.GetString(1));
                        }
                    }
                }
            }

            return result;
        }


        public List<SachHot> GetTop10CuonSachHot()
        {
            List<SachHot> danhSachHot = new List<SachHot>();

            string query = @"
        SELECT TOP 10 
            cs.TenCuonSach,
            ds.TenDauSach,
            COUNT(*) AS SoLuongMuon
        FROM PhieuMuonSach_CuonSach pmcs
        JOIN CuonSach cs ON pmcs.MaSach = cs.MaCuonSach
        JOIN DauSach ds ON cs.MaDauSach = ds.MaDauSach
        GROUP BY cs.TenCuonSach, ds.TenDauSach
        ORDER BY SoLuongMuon DESC
    ";

            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SachHot sach = new SachHot
                    {
                        TenCuonSach = reader["TenCuonSach"].ToString(),
                        TenDauSach = reader["TenDauSach"].ToString(),
                        SoLuongMuon = Convert.ToInt32(reader["SoLuongMuon"])
                    };
                    danhSachHot.Add(sach);
                }
            }

            return danhSachHot;
        }

    }
}
