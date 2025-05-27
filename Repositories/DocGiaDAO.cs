using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Repositories
{
    public class DocGiaDAO
    {
        private DBConnection dbConnection;

        public DocGiaDAO()
        {
            dbConnection = new DBConnection();
        }

        public List<DocGiaDTO> GetAllDocGia()
        {
            List<DocGiaDTO> docGias = new List<DocGiaDTO>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT dg.MaDocGia, dg.MaLoaiDG, dg.HoTen, dg.Tuoi, dg.SoDT, 
                           dg.CCCD, dg.GioiTinh, dg.Email, dg.DiaChi, dg.NgayDangKy,
                           dg.TienNo, dg.TrangThai, dg.NgayTao, dg.NgayCapNhat, dg.MaThe,
                           ldg.TenLoaiDG
                    FROM DocGia dg
                    LEFT JOIN LoaiDocGia ldg ON dg.MaLoaiDG = ldg.MaLoaiDG
                    ORDER BY dg.HoTen";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DocGiaDTO docGia = new DocGiaDTO()
                    {
                        MaDocGia = Convert.ToInt32(reader["MaDocGia"]),
                        MaLoaiDG = reader["MaLoaiDG"] == DBNull.Value ? null : (int?)reader["MaLoaiDG"],
                        HoTen = reader["HoTen"].ToString(),
                        Tuoi = Convert.ToInt32(reader["Tuoi"]),
                        SoDT = reader["SoDT"].ToString(),
                        CCCD = reader["CCCD"]?.ToString(),
                        GioiTinh = reader["GioiTinh"]?.ToString(),
                        Email = reader["Email"]?.ToString(),
                        DiaChi = reader["DiaChi"]?.ToString(),
                        NgayDangKy = reader["NgayDangKy"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["NgayDangKy"]),
                        TienNo = reader["TienNo"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TienNo"]),
                        TrangThai = reader["TrangThai"] == DBNull.Value ? true : Convert.ToBoolean(reader["TrangThai"]),
                        NgayTao = reader["NgayTao"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["NgayTao"]),
                        NgayCapNhat = reader["NgayCapNhat"] == DBNull.Value ? null : (DateTime?)reader["NgayCapNhat"],
                        MaThe = reader["MaThe"] == DBNull.Value ? null : (int?)reader["MaThe"],
                        TenLoaiDG = reader["TenLoaiDG"]?.ToString()
                    };
                    docGias.Add(docGia);
                }
            }
            return docGias;
        }

        // Method cho độc giả chưa có thẻ
        public List<DocGiaDTO> GetDocGiaChuaCoThe()
        {
            List<DocGiaDTO> docGias = new List<DocGiaDTO>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
            SELECT dg.MaDocGia, dg.MaLoaiDG, dg.HoTen, dg.Tuoi, dg.SoDT, 
                   dg.CCCD, dg.GioiTinh, dg.Email, dg.DiaChi, dg.NgayDangKy,
                   dg.TienNo, dg.TrangThai, dg.NgayTao, dg.NgayCapNhat, dg.MaThe,
                   ldg.TenLoaiDG,
                   ttv.MaThe as TheThuVien_MaThe
            FROM DocGia dg
            LEFT JOIN LoaiDocGia ldg ON dg.MaLoaiDG = ldg.MaLoaiDG
            LEFT JOIN TheThuVien ttv ON dg.MaDocGia = ttv.MaDG
            WHERE ttv.MaDG IS NULL
            ORDER BY dg.HoTen";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DocGiaDTO docGia = new DocGiaDTO()
                    {
                        MaDocGia = Convert.ToInt32(reader["MaDocGia"]),
                        MaLoaiDG = reader["MaLoaiDG"] == DBNull.Value ? null : (int?)reader["MaLoaiDG"],
                        HoTen = reader["HoTen"].ToString(),
                        Tuoi = Convert.ToInt32(reader["Tuoi"]),
                        SoDT = reader["SoDT"].ToString(),
                        CCCD = reader["CCCD"]?.ToString(),
                        GioiTinh = reader["GioiTinh"]?.ToString(),
                        Email = reader["Email"]?.ToString(),
                        DiaChi = reader["DiaChi"]?.ToString(),
                        NgayDangKy = reader["NgayDangKy"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["NgayDangKy"]),
                        TienNo = reader["TienNo"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TienNo"]),
                        TrangThai = reader["TrangThai"] == DBNull.Value ? true : Convert.ToBoolean(reader["TrangThai"]),
                        NgayTao = reader["NgayTao"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["NgayTao"]),
                        NgayCapNhat = reader["NgayCapNhat"] == DBNull.Value ? null : (DateTime?)reader["NgayCapNhat"],
                        MaThe = reader["MaThe"] == DBNull.Value ? null : (int?)reader["MaThe"],
                        TenLoaiDG = reader["TenLoaiDG"]?.ToString()
                    };
                    docGias.Add(docGia);
                }
            }
            return docGias;
        }

        public bool InsertDocGia(DocGiaDTO docGia)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    INSERT INTO DocGia (MaLoaiDG, HoTen, Tuoi, SoDT, CCCD, GioiTinh,
                                      Email, DiaChi, NgayDangKy, TienNo, TrangThai, NgayTao) 
                    VALUES (@MaLoaiDG, @HoTen, @Tuoi, @SoDT, @CCCD, @GioiTinh,
                            @Email, @DiaChi, @NgayDangKy, @TienNo, @TrangThai, @NgayTao)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaLoaiDG", docGia.MaLoaiDG ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@HoTen", docGia.HoTen);
                cmd.Parameters.AddWithValue("@Tuoi", docGia.Tuoi);
                cmd.Parameters.AddWithValue("@SoDT", docGia.SoDT);
                cmd.Parameters.AddWithValue("@CCCD", docGia.CCCD ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@GioiTinh", docGia.GioiTinh ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", docGia.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DiaChi", docGia.DiaChi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NgayDangKy", DateTime.Now);
                cmd.Parameters.AddWithValue("@TienNo", 0);
                cmd.Parameters.AddWithValue("@TrangThai", true);
                cmd.Parameters.AddWithValue("@NgayTao", DateTime.Now);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateDocGia(DocGiaDTO docGia)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    UPDATE DocGia SET 
                        MaLoaiDG = @MaLoaiDG,
                        HoTen = @HoTen,
                        Tuoi = @Tuoi,
                        SoDT = @SoDT,
                        CCCD = @CCCD,
                        GioiTinh = @GioiTinh,
                        Email = @Email,
                        DiaChi = @DiaChi,
                        TrangThai = @TrangThai,
                        NgayCapNhat = @NgayCapNhat
                    WHERE MaDocGia = @MaDocGia";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDocGia", docGia.MaDocGia);
                cmd.Parameters.AddWithValue("@MaLoaiDG", docGia.MaLoaiDG ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@HoTen", docGia.HoTen);
                cmd.Parameters.AddWithValue("@Tuoi", docGia.Tuoi);
                cmd.Parameters.AddWithValue("@SoDT", docGia.SoDT);
                cmd.Parameters.AddWithValue("@CCCD", docGia.CCCD ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@GioiTinh", docGia.GioiTinh ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", docGia.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DiaChi", docGia.DiaChi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TrangThai", docGia.TrangThai);
                cmd.Parameters.AddWithValue("@NgayCapNhat", DateTime.Now);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteDocGia(int maDocGia)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = "UPDATE DocGia SET TrangThai = 0 WHERE MaDocGia = @MaDocGia";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDocGia", maDocGia);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<DocGiaDTO> SearchDocGia(string searchText)
        {
            List<DocGiaDTO> docGias = new List<DocGiaDTO>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT dg.MaDocGia, dg.MaLoaiDG, dg.HoTen, dg.Tuoi, dg.SoDT, 
                           dg.CCCD, dg.GioiTinh, dg.Email, dg.DiaChi, dg.NgayDangKy,
                           dg.TienNo, dg.TrangThai, dg.NgayTao, dg.NgayCapNhat, dg.MaThe,
                           ldg.TenLoaiDG
                    FROM DocGia dg
                    LEFT JOIN LoaiDocGia ldg ON dg.MaLoaiDG = ldg.MaLoaiDG
                    WHERE dg.HoTen LIKE @SearchText 
                       OR dg.SoDT LIKE @SearchText 
                       OR dg.CCCD LIKE @SearchText
                       OR dg.Email LIKE @SearchText
                    ORDER BY dg.HoTen";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DocGiaDTO docGia = new DocGiaDTO()
                    {
                        MaDocGia = Convert.ToInt32(reader["MaDocGia"]),
                        MaLoaiDG = reader["MaLoaiDG"] == DBNull.Value ? null : (int?)reader["MaLoaiDG"],
                        HoTen = reader["HoTen"].ToString(),
                        Tuoi = Convert.ToInt32(reader["Tuoi"]),
                        SoDT = reader["SoDT"].ToString(),
                        CCCD = reader["CCCD"]?.ToString(),
                        GioiTinh = reader["GioiTinh"]?.ToString(),
                        Email = reader["Email"]?.ToString(),
                        DiaChi = reader["DiaChi"]?.ToString(),
                        NgayDangKy = reader["NgayDangKy"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["NgayDangKy"]),
                        TienNo = reader["TienNo"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TienNo"]),
                        TrangThai = reader["TrangThai"] == DBNull.Value ? true : Convert.ToBoolean(reader["TrangThai"]),
                        NgayTao = reader["NgayTao"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["NgayTao"]),
                        NgayCapNhat = reader["NgayCapNhat"] == DBNull.Value ? null : (DateTime?)reader["NgayCapNhat"],
                        MaThe = reader["MaThe"] == DBNull.Value ? null : (int?)reader["MaThe"],
                        TenLoaiDG = reader["TenLoaiDG"]?.ToString()
                    };
                    docGias.Add(docGia);
                }
            }
            return docGias;
        }

        // Phương thức tính tổng tiền mượn của từng độc giả trong tháng
        public List<ThongKeTienMuonDTO> GetTongTienMuonTheoThang(int thang, int nam)
        {
            List<ThongKeTienMuonDTO> results = new List<ThongKeTienMuonDTO>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT 
                        dg.MaDocGia,
                        dg.HoTen,
                        SUM(ms.GiaMuon) as TongTienMuon,
                        COUNT(ms.MaMuonSach) as SoLanMuon
                    FROM DocGia dg
                    INNER JOIN PhieuMuonSach pms ON dg.MaDocGia = pms.MaDocGia
                    INNER JOIN MuonSach ms ON pms.MaPhieu = ms.MaPhieu
                    WHERE MONTH(ms.NgayMuon) = @Thang AND YEAR(ms.NgayMuon) = @Nam
                    GROUP BY dg.MaDocGia, dg.HoTen
                    ORDER BY TongTienMuon DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var result = new ThongKeTienMuonDTO
                    {
                        MaDocGia = Convert.ToInt32(reader["MaDocGia"]),
                        HoTen = reader["HoTen"].ToString(),
                        TongTienMuon = reader["TongTienMuon"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TongTienMuon"]),
                        SoLanMuon = Convert.ToInt32(reader["SoLanMuon"]),
                        Thang = thang,
                        Nam = nam
                    };
                    results.Add(result);
                }
            }
            return results;
        }

        // Phương thức tính tổng tiền mượn + phạt
        public List<ThongKeTienMuonVaPhatDTO> GetTongTienMuonVaPhat(int thang, int nam)
        {
            List<ThongKeTienMuonVaPhatDTO> results = new List<ThongKeTienMuonVaPhatDTO>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT 
                        dg.MaDocGia,
                        dg.HoTen,
                        ISNULL(SUM(ms.GiaMuon), 0) as TongTienMuon,
                        ISNULL(SUM(qdp.TienPhat), 0) as TongTienPhat,
                        (ISNULL(SUM(ms.GiaMuon), 0) + ISNULL(SUM(qdp.TienPhat), 0)) as TongCong
                    FROM DocGia dg
                    LEFT JOIN PhieuMuonSach pms ON dg.MaDocGia = pms.MaDocGia
                    LEFT JOIN MuonSach ms ON pms.MaPhieu = ms.MaPhieu 
                        AND MONTH(ms.NgayMuon) = @Thang AND YEAR(ms.NgayMuon) = @Nam
                    LEFT JOIN PhieuPhat pp ON dg.MaDocGia = pp.MaDG
                    LEFT JOIN PhieuPhat_QDP ppq ON pp.MaPhieuPhat = ppq.MaPhieuPhat
                    LEFT JOIN QDP qdp ON ppq.MaQDP = qdp.MaQDP
                    WHERE ms.MaMuonSach IS NOT NULL OR pp.MaPhieuPhat IS NOT NULL
                    GROUP BY dg.MaDocGia, dg.HoTen
                    ORDER BY TongCong DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var result = new ThongKeTienMuonVaPhatDTO
                    {
                        MaDocGia = Convert.ToInt32(reader["MaDocGia"]),
                        HoTen = reader["HoTen"].ToString(),
                        TongTienMuon = Convert.ToDecimal(reader["TongTienMuon"]),
                        TongTienPhat = Convert.ToDecimal(reader["TongTienPhat"]),
                        TongCong = Convert.ToDecimal(reader["TongCong"]),
                        Thang = thang,
                        Nam = nam
                    };
                    results.Add(result);
                }
            }
            return results;
        }

        // Thống kê số lượng độc giả mới theo tháng
        public List<ThongKeDocGiaMoiDTO> GetThongKeDocGiaMoiTheoThang(int nam)
        {
            List<ThongKeDocGiaMoiDTO> results = new List<ThongKeDocGiaMoiDTO>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT 
                        MONTH(NgayDangKy) as Thang,
                        COUNT(*) as SoDocGiaMoi
                    FROM DocGia 
                    WHERE YEAR(NgayDangKy) = @Nam
                    GROUP BY MONTH(NgayDangKy)
                    ORDER BY Thang";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nam", nam);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var result = new ThongKeDocGiaMoiDTO
                    {
                        Thang = Convert.ToInt32(reader["Thang"]),
                        TenThang = "Tháng " + Convert.ToInt32(reader["Thang"]),
                        SoDocGiaMoi = Convert.ToInt32(reader["SoDocGiaMoi"]),
                        Nam = nam
                    };
                    results.Add(result);
                }
            }
            return results;
        }
    }
}
