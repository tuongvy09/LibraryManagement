using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryManagement.Repositories
{
    public class PhieuMuonDAO
    {
        private DBConnection dbConnection;

        public PhieuMuonDAO()
        {
            dbConnection = new DBConnection();
        }

        public List<PhieuMuon> GetAllPhieuMuon()
        {
            List<PhieuMuon> list = new List<PhieuMuon>();

            string query = @"
        SELECT ms.MaMuonSach, ms.MaPhieu, dg.MaDocGia, dg.HoTen AS TenDocGia,
               ms.NgayMuon, ms.NgayTra, ms.TrangThaiM, ms.GiaMuon, ms.SoNgayMuon, ms.TienCoc,
               cs.TenCuonSach
        FROM MuonSach ms
        JOIN PhieuMuonSach pms ON ms.MaPhieu = pms.MaPhieu
        JOIN DocGia dg ON pms.MaDocGia = dg.MaDocGia
        JOIN PhieuMuonSach_CuonSach pmcs ON pmcs.MaPhieu = pms.MaPhieu
        JOIN CuonSach cs ON cs.MaCuonSach = pmcs.MaSach
        ORDER BY ms.MaMuonSach
    ";

            using (SqlConnection con = dbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Dictionary<int, PhieuMuon> map = new Dictionary<int, PhieuMuon>();

                    while (reader.Read())
                    {
                        int maMuonSach = reader.GetInt32(0);

                        if (!map.ContainsKey(maMuonSach))
                        {
                            var pm = new PhieuMuon
                            {
                                MaMuonSach = maMuonSach,
                                MaPhieu = reader.GetInt32(1),
                                MaDocGia = reader.GetInt32(2),
                                TenDocGia = reader.GetString(3),
                                NgayMuon = reader.GetDateTime(4),
                                NgayTra = reader.GetDateTime(5),
                                TrangThaiM = reader.GetString(6),
                                GiaMuon = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7),
                                SoNgayMuon = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                                TienCoc = reader.IsDBNull(9) ? 0 : reader.GetDecimal(9),
                                DanhSachTenCuonSach = new List<string>()
                            };

                            map[maMuonSach] = pm;
                        }

                        // Add cuốn sách vào danh sách
                        string tenCuonSach = reader.IsDBNull(10) ? "" : reader.GetString(10);
                        if (!string.IsNullOrWhiteSpace(tenCuonSach))
                            map[maMuonSach].DanhSachTenCuonSach.Add(tenCuonSach);
                    }

                    list = map.Values.ToList();
                }
            }

            return list;
        }

        public bool AddPhieuMuon(PhieuMuon phieuMuon, List<int> danhSachMaCuonSach)
        {
            using (SqlConnection con = dbConnection.GetConnection())
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        string insertPhieuQuery = @"
                    INSERT INTO PhieuMuonSach (MaDocGia)
                    VALUES (@MaDocGia);
                    SELECT CAST(scope_identity() AS int);
                ";
                        int maPhieu;
                        using (SqlCommand cmd = new SqlCommand(insertPhieuQuery, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaDocGia", phieuMuon.MaDocGia);
                            maPhieu = (int)cmd.ExecuteScalar();
                        }

                        string insertMuonSachQuery = @"
                    INSERT INTO MuonSach 
                        (MaPhieu, NgayMuon, NgayTra, TrangThaiM, GiaMuon, SoNgayMuon, TienCoc)
                    VALUES
                        (@MaPhieu, @NgayMuon, @NgayTra, @TrangThaiM, @GiaMuon, @SoNgayMuon, @TienCoc);
                    SELECT CAST(scope_identity() AS int);
                ";
                        int maMuonSach;
                        using (SqlCommand cmd = new SqlCommand(insertMuonSachQuery, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaPhieu", maPhieu);
                            cmd.Parameters.AddWithValue("@NgayMuon", phieuMuon.NgayMuon);
                            cmd.Parameters.AddWithValue("@NgayTra", phieuMuon.NgayTra);
                            cmd.Parameters.AddWithValue("@TrangThaiM", phieuMuon.TrangThaiM);
                            cmd.Parameters.AddWithValue("@GiaMuon", phieuMuon.GiaMuon);
                            cmd.Parameters.AddWithValue("@SoNgayMuon", phieuMuon.SoNgayMuon);
                            cmd.Parameters.AddWithValue("@TienCoc", phieuMuon.TienCoc);

                            maMuonSach = (int)cmd.ExecuteScalar();
                        }

                        string insertChiTietQuery = @"
                                INSERT INTO PhieuMuonSach_CuonSach (MaPhieu, MaSach)
                                VALUES (@MaPhieu, @MaSach);";

                        foreach (int maCuonSach in danhSachMaCuonSach)
                        {
                            using (SqlCommand cmd = new SqlCommand(insertChiTietQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@MaPhieu", maPhieu);
                                cmd.Parameters.AddWithValue("@MaSach", maCuonSach);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thêm phiếu mượn:\n" + ex.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
        }

        public bool UpdatePhieuMuon(PhieuMuon phieuMuon)
        {
            string updateQuery = @"
            UPDATE MuonSach
            SET NgayMuon = @NgayMuon,
                NgayTra = @NgayTra,
                TrangThaiM = @TrangThaiM,
                GiaMuon = @GiaMuon,
                SoNgayMuon = @SoNgayMuon,
                TienCoc = @TienCoc
            WHERE MaMuonSach = @MaMuonSach
        ";

            using (SqlConnection con = dbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(updateQuery, con))
            {
                try
                {
                    con.Open();

                    cmd.Parameters.AddWithValue("@NgayMuon", phieuMuon.NgayMuon);
                    cmd.Parameters.AddWithValue("@NgayTra", phieuMuon.NgayTra);
                    cmd.Parameters.AddWithValue("@TrangThaiM", phieuMuon.TrangThaiM ?? "");
                    cmd.Parameters.AddWithValue("@GiaMuon", phieuMuon.GiaMuon);
                    cmd.Parameters.AddWithValue("@SoNgayMuon", phieuMuon.SoNgayMuon);
                    cmd.Parameters.AddWithValue("@TienCoc", phieuMuon.TienCoc);
                    cmd.Parameters.AddWithValue("@MaMuonSach", phieuMuon.MaMuonSach);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool DeletePhieuMuon(int maMuonSach)
        {
            using (SqlConnection con = dbConnection.GetConnection())
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        int maPhieu = 0;
                        string queryGetMaPhieu = "SELECT MaPhieu FROM MuonSach WHERE MaMuonSach = @MaMuonSach";
                        using (SqlCommand cmd = new SqlCommand(queryGetMaPhieu, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaMuonSach", maMuonSach);
                            object result = cmd.ExecuteScalar();
                            if (result == null)
                            {
                                transaction.Rollback();
                                return false;
                            }
                            maPhieu = Convert.ToInt32(result);
                        }

                        string deleteChiTietQuery = "DELETE FROM PhieuMuonSach_CuonSach WHERE MaPhieu = @MaPhieu";
                        using (SqlCommand cmd = new SqlCommand(deleteChiTietQuery, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaPhieu", maPhieu);
                            cmd.ExecuteNonQuery();
                        }

                        string deleteMuonSachQuery = "DELETE FROM MuonSach WHERE MaMuonSach = @MaMuonSach";
                        using (SqlCommand cmd = new SqlCommand(deleteMuonSachQuery, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaMuonSach", maMuonSach);
                            cmd.ExecuteNonQuery();
                        }

                        string deletePhieuQuery = "DELETE FROM PhieuMuonSach WHERE MaPhieu = @MaPhieu";
                        using (SqlCommand cmd = new SqlCommand(deletePhieuQuery, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MaPhieu", maPhieu);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public List<PhieuMuon> GetChiTietPhieu(int maPhieu)
        {
            List<PhieuMuon> list = new List<PhieuMuon>();

            string query = @"
        SELECT ms.MaMuonSach, dg.HoTen AS TenDocGia, cs.TenCuonSach,
               ms.NgayMuon, ms.NgayTra, ms.TrangThaiM, ms.GiaMuon, ms.SoNgayMuon, ms.TienCoc
        FROM MuonSach ms
        JOIN PhieuMuonSach pms ON ms.MaPhieu = pms.MaPhieu
        JOIN DocGia dg ON pms.MaDocGia = dg.MaDocGia
        JOIN PhieuMuonSach_CuonSach pmcs ON pms.MaPhieu = pmcs.MaPhieu
        JOIN CuonSach cs ON pmcs.MaSach = cs.MaCuonSach
        WHERE pms.MaPhieu = @MaPhieu";

            using (SqlConnection con = dbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@MaPhieu", maPhieu);
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PhieuMuon pm = new PhieuMuon
                        {
                            MaMuonSach = reader.GetInt32(0),
                            TenDocGia = reader.GetString(1),
                            TenCuonSach = reader.GetString(2),
                            NgayMuon = reader.GetDateTime(3),
                            NgayTra = reader.GetDateTime(4),
                            TrangThaiM = reader.GetString(5),
                            GiaMuon = reader.IsDBNull(6) ? 0 : reader.GetDecimal(6),
                            SoNgayMuon = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                            TienCoc = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8)
                        };
                        list.Add(pm);
                    }
                }
            }

            return list;
        }

        public List<int> GetDanhSachMaCuonSachByMaPhieu(int maPhieu)
        {
            List<int> list = new List<int>();
            string query = "SELECT MaCuonSach FROM PhieuMuonSach_CuonSach WHERE MaMuonSach = @MaPhieu";

            using (SqlConnection con = dbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@MaPhieu", maPhieu);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetInt32(0));
                    }
                }
            }
            return list;
        }

        public PhieuMuon GetPhieuMuonById(int maMuonSach)
        {
            PhieuMuon phieu = null;
            string query = @"
    SELECT TOP 1 
        ms.MaMuonSach,
        dg.HoTen AS TenDocGia,
        cs.TenCuonSach,
        ms.NgayMuon,
        ms.NgayTra,
        ms.TrangThaiM,
        ms.GiaMuon,
        ms.SoNgayMuon,
        ms.TienCoc
    FROM MuonSach ms
    JOIN PhieuMuonSach pms ON ms.MaPhieu = pms.MaPhieu
    JOIN DocGia dg ON pms.MaDocGia = dg.MaDocGia
    JOIN PhieuMuonSach_CuonSach pmcs ON pmcs.MaPhieu = ms.MaPhieu
    JOIN CuonSach cs ON pmcs.MaSach = cs.MaCuonSach
    WHERE ms.MaMuonSach = @MaMuonSach";

            using (SqlConnection con = dbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@MaMuonSach", maMuonSach);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        phieu = new PhieuMuon()
                        {
                            MaMuonSach = reader.GetInt32(reader.GetOrdinal("MaMuonSach")),
                            TenDocGia = reader["TenDocGia"]?.ToString() ?? "",
                            TenCuonSach = reader["TenCuonSach"]?.ToString() ?? "",
                            NgayMuon = reader.GetDateTime(reader.GetOrdinal("NgayMuon")),
                            NgayTra = reader["NgayTra"] == DBNull.Value ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("NgayTra")),
                            TrangThaiM = reader["TrangThaiM"]?.ToString() ?? "",
                            GiaMuon = reader["GiaMuon"] == DBNull.Value ? 0 : reader.GetDecimal(reader.GetOrdinal("GiaMuon")),
                            SoNgayMuon = reader["SoNgayMuon"] == DBNull.Value ? 0 : reader.GetInt32(reader.GetOrdinal("SoNgayMuon")),
                            TienCoc = reader["TienCoc"] == DBNull.Value ? 0 : reader.GetDecimal(reader.GetOrdinal("TienCoc"))
                        };
                    }
                }
            }
            return phieu;
        }

        public int? GetMaDocGiaFromName(string tenDocGia)
        {
            int? maDocGia = null;
            string query = "SELECT MaDocGia FROM DocGia WHERE TenDocGia = @TenDocGia";

            using (SqlConnection con = dbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@TenDocGia", tenDocGia);
                con.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int ma))
                {
                    maDocGia = ma;
                }
            }

            return maDocGia;
        }

        public List<int> GetDanhSachMaCuonSach(string tenDauSach)
        {
            List<int> list = new List<int>();
            string query = @"
        SELECT cs.MaCuonSach
        FROM CuonSach cs
        JOIN DauSach ds ON cs.MaDauSach = ds.MaDauSach
        WHERE ds.TenDauSach = @TenDauSach";

            using (SqlConnection con = dbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@TenDauSach", tenDauSach);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetInt32(0));
                    }
                }
            }

            return list;
        }
    }
}
