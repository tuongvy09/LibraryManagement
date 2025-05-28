using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Models;

namespace LibraryManagement.Repositories
{
    public class ThongKeDAO
    {
        private readonly DBConnection dbConnection = new DBConnection();

        // ✅ Thống kê số lượng độc giả mới theo tháng
        public List<ThongKeDocGiaTheoThangDTO> GetThongKeDocGiaTheoThang(int nam)
        {
            List<ThongKeDocGiaTheoThangDTO> result = new List<ThongKeDocGiaTheoThangDTO>();

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    string query = @"
                        SELECT 
                            MONTH(NgayDangKy) as Thang,
                            YEAR(NgayDangKy) as Nam,
                            COUNT(*) as SoLuongDocGiaMoi
                        FROM DocGia 
                        WHERE YEAR(NgayDangKy) = @Nam
                            AND NgayDangKy IS NOT NULL
                        GROUP BY YEAR(NgayDangKy), MONTH(NgayDangKy)
                        ORDER BY MONTH(NgayDangKy)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nam", nam);

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ThongKeDocGiaTheoThangDTO item = new ThongKeDocGiaTheoThangDTO()
                                {
                                    Thang = Convert.ToInt32(reader["Thang"]),
                                    Nam = Convert.ToInt32(reader["Nam"]),
                                    SoLuongDocGiaMoi = Convert.ToInt32(reader["SoLuongDocGiaMoi"])
                                };
                                result.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi GetThongKeDocGiaTheoThang: {ex.Message}");
                // Return empty list thay vì throw exception
                return new List<ThongKeDocGiaTheoThangDTO>();
            }

            return result;
        }

        // ✅ Tính tổng tiền mượn của từng độc giả trong tháng
        public List<ThongKeTienMuonDocGiaDTO> GetThongKeTienMuonDocGia(int thang, int nam)
        {
            List<ThongKeTienMuonDocGiaDTO> result = new List<ThongKeTienMuonDocGiaDTO>();

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    // ✅ Simplified query để tránh lỗi JOIN phức tạp
                    string query = @"
                        SELECT 
                            dg.MaDocGia,
                            dg.HoTen,
                            ISNULL(SUM(lpm.ChiPhi), 0) as TongTienMuon,
                            ISNULL(SUM(qdp.TienPhat), 0) as TongTienPhat,
                            COUNT(DISTINCT pms.MaPhieu) as SoLanMuon,
                            MAX(ms.NgayMuon) as LanMuonGanNhat
                        FROM DocGia dg
                        LEFT JOIN PhieuMuonSach pms ON dg.MaDocGia = pms.MaDocGia
                        LEFT JOIN MuonSach ms ON pms.MaPhieu = ms.MaPhieu 
                            AND MONTH(ms.NgayMuon) = @Thang 
                            AND YEAR(ms.NgayMuon) = @Nam
                        LEFT JOIN LoaiPhieuMuon lpm ON pms.MaPhieu = lpm.MaLPhieuMuon
                        LEFT JOIN PhieuPhat pp ON dg.MaDocGia = pp.MaDG
                        LEFT JOIN PhieuPhat_QDP ppq ON pp.MaPhieuPhat = ppq.MaPhieuPhat
                        LEFT JOIN QDP qdp ON ppq.MaQDP = qdp.MaQDP
                        WHERE dg.TrangThai = 1
                        GROUP BY dg.MaDocGia, dg.HoTen
                        HAVING ISNULL(SUM(lpm.ChiPhi), 0) > 0 OR ISNULL(SUM(qdp.TienPhat), 0) > 0
                        ORDER BY (ISNULL(SUM(lpm.ChiPhi), 0) + ISNULL(SUM(qdp.TienPhat), 0)) DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Thang", thang);
                        cmd.Parameters.AddWithValue("@Nam", nam);

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ThongKeTienMuonDocGiaDTO item = new ThongKeTienMuonDocGiaDTO()
                                {
                                    MaDocGia = Convert.ToInt32(reader["MaDocGia"]),
                                    HoTen = reader["HoTen"].ToString(),
                                    TongTienMuon = reader["TongTienMuon"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TongTienMuon"]),
                                    TongTienPhat = reader["TongTienPhat"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TongTienPhat"]),
                                    SoLanMuon = Convert.ToInt32(reader["SoLanMuon"]),
                                    LanMuonGanNhat = reader["LanMuonGanNhat"] == DBNull.Value ? null : (DateTime?)reader["LanMuonGanNhat"]
                                };
                                result.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi GetThongKeTienMuonDocGia: {ex.Message}");
                return new List<ThongKeTienMuonDocGiaDTO>();
            }

            return result;
        }

        // ✅ Thống kê tổng tiền mượn + phạt theo tháng 
        public List<ThongKeTienTheoThangDTO> GetThongKeTienTheoThang(int nam)
        {
            List<ThongKeTienTheoThangDTO> result = new List<ThongKeTienTheoThangDTO>();

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    string query = @"
                        SELECT 
                            MONTH(ms.NgayMuon) as Thang,
                            YEAR(ms.NgayMuon) as Nam,
                            ISNULL(SUM(lpm.ChiPhi), 0) as TongTienMuon,
                            ISNULL(SUM(qdp.TienPhat), 0) as TongTienPhat
                        FROM MuonSach ms
                        LEFT JOIN PhieuMuonSach pms ON ms.MaPhieu = pms.MaPhieu
                        LEFT JOIN LoaiPhieuMuon lpm ON pms.MaPhieu = lpm.MaLPhieuMuon
                        LEFT JOIN PhieuPhat pp ON pms.MaDocGia = pp.MaDG
                        LEFT JOIN PhieuPhat_QDP ppq ON pp.MaPhieuPhat = ppq.MaPhieuPhat
                        LEFT JOIN QDP qdp ON ppq.MaQDP = qdp.MaQDP
                        WHERE YEAR(ms.NgayMuon) = @Nam
                        GROUP BY YEAR(ms.NgayMuon), MONTH(ms.NgayMuon)
                        ORDER BY MONTH(ms.NgayMuon)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nam", nam);

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ThongKeTienTheoThangDTO item = new ThongKeTienTheoThangDTO()
                                {
                                    Thang = Convert.ToInt32(reader["Thang"]),
                                    Nam = Convert.ToInt32(reader["Nam"]),
                                    TongTienMuon = reader["TongTienMuon"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TongTienMuon"]),
                                    TongTienPhat = reader["TongTienPhat"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TongTienPhat"])
                                };
                                result.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi GetThongKeTienTheoThang: {ex.Message}");
                return new List<ThongKeTienTheoThangDTO>();
            }

            return result;
        }

        // ✅ Lấy chi tiết tiền mượn + phạt của một độc giả
        public ThongKeTienMuonDocGiaDTO GetChiTietTienMuonDocGia(int maDocGia)
        {
            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    string query = @"
                        SELECT 
                            dg.MaDocGia,
                            dg.HoTen,
                            ISNULL(SUM(lpm.ChiPhi), 0) as TongTienMuon,
                            ISNULL(SUM(qdp.TienPhat), 0) as TongTienPhat,
                            COUNT(DISTINCT pms.MaPhieu) as SoLanMuon,
                            MAX(ms.NgayMuon) as LanMuonGanNhat
                        FROM DocGia dg
                        LEFT JOIN PhieuMuonSach pms ON dg.MaDocGia = pms.MaDocGia
                        LEFT JOIN MuonSach ms ON pms.MaPhieu = ms.MaPhieu
                        LEFT JOIN LoaiPhieuMuon lpm ON pms.MaPhieu = lpm.MaLPhieuMuon
                        LEFT JOIN PhieuPhat pp ON dg.MaDocGia = pp.MaDG
                        LEFT JOIN PhieuPhat_QDP ppq ON pp.MaPhieuPhat = ppq.MaPhieuPhat
                        LEFT JOIN QDP qdp ON ppq.MaQDP = qdp.MaQDP
                        WHERE dg.MaDocGia = @MaDocGia
                        GROUP BY dg.MaDocGia, dg.HoTen";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaDocGia", maDocGia);

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new ThongKeTienMuonDocGiaDTO()
                                {
                                    MaDocGia = Convert.ToInt32(reader["MaDocGia"]),
                                    HoTen = reader["HoTen"].ToString(),
                                    TongTienMuon = reader["TongTienMuon"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TongTienMuon"]),
                                    TongTienPhat = reader["TongTienPhat"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TongTienPhat"]),
                                    SoLanMuon = Convert.ToInt32(reader["SoLanMuon"]),
                                    LanMuonGanNhat = reader["LanMuonGanNhat"] == DBNull.Value ? null : (DateTime?)reader["LanMuonGanNhat"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi GetChiTietTienMuonDocGia: {ex.Message}");
                return null;
            }

            return null;
        }

        // ✅ NEW - Thống kê doanh thu theo tháng
        public List<ThongKeDoanhThuTheoThangDTO> GetThongKeDoanhThuTheoThang(int nam)
        {
            List<ThongKeDoanhThuTheoThangDTO> result = new List<ThongKeDoanhThuTheoThangDTO>();

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    string query = @"
                SELECT 
                    MONTH(NgayTraTT) as Thang,
                    YEAR(NgayTraTT) as Nam,
                    SUM(TienTra) as TongDoanhThu,
                    COUNT(*) as SoGiaoDich
                FROM BienLai 
                WHERE YEAR(NgayTraTT) = @Nam
                    AND NgayTraTT IS NOT NULL
                    AND TienTra > 0
                GROUP BY YEAR(NgayTraTT), MONTH(NgayTraTT)
                ORDER BY MONTH(NgayTraTT)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nam", nam);

                        System.Diagnostics.Debug.WriteLine($"🔍 Executing query với @Nam = {nam}");

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ThongKeDoanhThuTheoThangDTO item = new ThongKeDoanhThuTheoThangDTO()
                                {
                                    Thang = Convert.ToInt32(reader["Thang"]),
                                    Nam = Convert.ToInt32(reader["Nam"]),
                                    TongDoanhThu = reader["TongDoanhThu"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TongDoanhThu"]),
                                    SoGiaoDich = Convert.ToInt32(reader["SoGiaoDich"])
                                };
                                result.Add(item);

                                System.Diagnostics.Debug.WriteLine($"📊 DAO: Tháng {item.Thang}, Doanh thu: {item.TongDoanhThu:N0}, Giao dịch: {item.SoGiaoDich}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi GetThongKeDoanhThuTheoThang: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"❌ Stack trace: {ex.StackTrace}");
                return new List<ThongKeDoanhThuTheoThangDTO>();
            }

            System.Diagnostics.Debug.WriteLine($"📈 DAO trả về {result.Count} records cho năm {nam}");
            return result;
        }

        public List<ThongKeSachMuonTheoTheLoaiDTO> GetThongKeSachMuonTheoTheLoai()
        {
            List<ThongKeSachMuonTheoTheLoaiDTO> result = new List<ThongKeSachMuonTheoTheLoaiDTO>();

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    string query = @"
                SELECT tl.TenTheLoai, COUNT(*) AS SoLuongMuon
                FROM PhieuMuonSach_CuonSach pmcs
                JOIN CuonSach cs ON pmcs.MaSach = cs.MaCuonSach
                JOIN DauSach ds ON cs.MaDauSach = ds.MaDauSach
                JOIN TheLoai tl ON ds.MaTheLoai = tl.MaTheLoai
                GROUP BY tl.TenTheLoai
                ORDER BY SoLuongMuon DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ThongKeSachMuonTheoTheLoaiDTO item = new ThongKeSachMuonTheoTheLoaiDTO()
                                {
                                    TenTheLoai = reader["TenTheLoai"].ToString(),
                                    SoLuongMuon = Convert.ToInt32(reader["SoLuongMuon"])
                                };
                                result.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi GetThongKeSachMuonTheoTheLoai: {ex.Message}");
                return new List<ThongKeSachMuonTheoTheLoaiDTO>();
            }

            return result;
        }

        public List<ThongKeSachMuonTheoDocGiaDTO> GetThongKeSachMuonTheoDocGia()
        {
            List<ThongKeSachMuonTheoDocGiaDTO> result = new List<ThongKeSachMuonTheoDocGiaDTO>();

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    string query = @"
                SELECT dg.HoTen AS TenDocGia, COUNT(*) AS SoLuongMuon
                FROM PhieuMuonSach pms
                JOIN DocGia dg ON pms.MaDocGia = dg.MaDocGia
                JOIN PhieuMuonSach_CuonSach pmcs ON pms.MaPhieu = pmcs.MaPhieu
                GROUP BY dg.HoTen
                ORDER BY SoLuongMuon DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ThongKeSachMuonTheoDocGiaDTO item = new ThongKeSachMuonTheoDocGiaDTO()
                                {
                                    TenDocGia = reader["TenDocGia"].ToString(),
                                    SoLuongMuon = Convert.ToInt32(reader["SoLuongMuon"])
                                };
                                result.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi GetThongKeSachMuonTheoDocGia: {ex.Message}");
                return new List<ThongKeSachMuonTheoDocGiaDTO>();
            }

            return result;
        }

        public List<ThongKeSachMuonTheoThangDTO> GetThongKeSachMuonTheoThang(int nam, int thang)
        {
            List<ThongKeSachMuonTheoThangDTO> result = new List<ThongKeSachMuonTheoThangDTO>();

            try
            {
                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    string query = @"
                SELECT ds.TenDauSach, COUNT(*) AS SoLuongMuon
                FROM MuonSach ms
                JOIN PhieuMuonSach_CuonSach pmcs ON ms.MaPhieu = pmcs.MaPhieu
                JOIN CuonSach cs ON pmcs.MaSach = cs.MaCuonSach
                JOIN DauSach ds ON cs.MaDauSach = ds.MaDauSach
                WHERE YEAR(ms.NgayMuon) = @Nam AND MONTH(ms.NgayMuon) = @Thang
                GROUP BY ds.TenDauSach
                ORDER BY SoLuongMuon DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nam", nam);
                        cmd.Parameters.AddWithValue("@Thang", thang);

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ThongKeSachMuonTheoThangDTO item = new ThongKeSachMuonTheoThangDTO()
                                {
                                    TenDauSach = reader["TenDauSach"].ToString(),
                                    SoLuongMuon = Convert.ToInt32(reader["SoLuongMuon"])
                                };
                                result.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi GetThongKeSachMuonTheoThang: {ex.Message}");
                return new List<ThongKeSachMuonTheoThangDTO>();
            }

            return result;
        }

        public List<SachMuonDTO> GetTatCaSachDangMuon()
        {
            List<SachMuonDTO> list = new List<SachMuonDTO>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
            SELECT 
                cs.MaCuonSach,
                cs.TenCuonSach,
                ds.TenDauSach,
                dg.HoTen AS TenDocGia,
                ms.NgayMuon,
                ms.NgayTra AS NgayTraDuKien
            FROM MuonSach ms
            JOIN PhieuMuonSach pms ON ms.MaPhieu = pms.MaPhieu
            JOIN DocGia dg ON pms.MaDocGia = dg.MaDocGia
            JOIN PhieuMuonSach_CuonSach pmcs ON pms.MaPhieu = pmcs.MaPhieu
            JOIN CuonSach cs ON pmcs.MaSach = cs.MaCuonSach
            JOIN DauSach ds ON cs.MaDauSach = ds.MaDauSach
            WHERE ms.TrangThaiM = N'Đang mượn'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dto = new SachMuonDTO
                        {
                            MaSach = reader["MaCuonSach"].ToString(),
                            TenSach = reader["TenDauSach"].ToString(),
                            TenDocGia = reader["TenDocGia"].ToString(),
                            NgayMuon = Convert.ToDateTime(reader["NgayMuon"]),
                            NgayTraDuKien = Convert.ToDateTime(reader["NgayTraDuKien"])
                        };
                        list.Add(dto);
                    }
                }
            }

            return list;
        }

    }
}
