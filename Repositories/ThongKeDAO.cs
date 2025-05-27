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
    }
}
