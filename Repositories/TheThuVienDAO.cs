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

        // Lấy danh sách tất cả thẻ thư viện
        public DataTable LayTheThuVien()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Mã thẻ");
            dt.Columns.Add("Tên độc giả");
            dt.Columns.Add("Số ĐT");
            dt.Columns.Add("Ngày cấp");
            dt.Columns.Add("Ngày hết hạn");
            dt.Columns.Add("Trạng thái");

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT TOP 1000 tt.MaThe, tt.MaDG, tt.NgayCap, tt.NgayHetHan,
                           ISNULL(dg.HoTen, N'Chưa gán') as TenDocGia, 
                           ISNULL(dg.SoDT, '') as SoDT
                    FROM TheThuVien tt
                    LEFT JOIN DocGia dg ON tt.MaDG = dg.MaDocGia
                    ORDER BY tt.MaThe DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DateTime ngayHetHan = Convert.ToDateTime(reader["NgayHetHan"]);
                    string trangThai = ngayHetHan > DateTime.Now ? "Còn hiệu lực" : "Hết hạn";

                    dt.Rows.Add(
                        reader["MaThe"].ToString(),
                        reader["TenDocGia"].ToString(),
                        reader["SoDT"].ToString(),
                        Convert.ToDateTime(reader["NgayCap"]).ToString("dd/MM/yyyy"),
                        ngayHetHan.ToString("dd/MM/yyyy"),
                        trangThai
                    );
                }
            }
            return dt;
        }

        // Lấy danh sách độc giả cho ComboBox
        public DataTable LayDocGiaChoComboBox()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaDG", typeof(int));
            dt.Columns.Add("TenDocGia", typeof(string));

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT MaDocGia as MaDG, HoTen as TenDocGia 
                    FROM DocGia 
                    WHERE TrangThai = 1
                    ORDER BY HoTen";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dt.Rows.Add(
                        Convert.ToInt32(reader["MaDG"]),
                        reader["TenDocGia"].ToString()
                    );
                }
            }
            return dt;
        }

        // Thêm thẻ thư viện mới
        public bool ThemTheThuVien(int? maDocGia, DateTime ngayCap, DateTime ngayHetHan, ref string err)
        {
            try
            {
                // Validate input
                if (ngayCap > DateTime.Now)
                {
                    err = "Ngày cấp không được lớn hơn ngày hiện tại";
                    return false;
                }

                if (ngayHetHan <= ngayCap)
                {
                    err = "Ngày hết hạn phải lớn hơn ngày cấp";
                    return false;
                }

                // Kiểm tra độc giả có tồn tại không (nếu có)
                if (maDocGia.HasValue)
                {
                    if (!KiemTraDocGiaTonTai(maDocGia.Value))
                    {
                        err = "Độc giả không tồn tại hoặc đã bị vô hiệu hóa";
                        return false;
                    }

                    // Kiểm tra độc giả đã có thẻ thư viện chưa
                    if (CheckDocGiaHasCard(maDocGia.Value))
                    {
                        err = "Độc giả đã có thẻ thư viện";
                        return false;
                    }
                }

                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        // Thêm thẻ mới
                        string insertQuery = @"
                            INSERT INTO TheThuVien (MaDG, NgayCap, NgayHetHan)
                            OUTPUT INSERTED.MaThe
                            VALUES (@MaDG, @NgayCap, @NgayHetHan)";

                        SqlCommand insertCmd = new SqlCommand(insertQuery, conn, transaction);
                        insertCmd.Parameters.AddWithValue("@MaDG", maDocGia ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@NgayCap", ngayCap);
                        insertCmd.Parameters.AddWithValue("@NgayHetHan", ngayHetHan);

                        int newMaThe = Convert.ToInt32(insertCmd.ExecuteScalar());

                        // Cập nhật MaThe trong bảng DocGia nếu có
                        if (maDocGia.HasValue)
                        {
                            string updateQuery = @"
                                UPDATE DocGia SET MaThe = @MaThe, NgayCapNhat = @NgayCapNhat
                                WHERE MaDocGia = @MaDocGia";

                            SqlCommand updateCmd = new SqlCommand(updateQuery, conn, transaction);
                            updateCmd.Parameters.AddWithValue("@MaThe", newMaThe);
                            updateCmd.Parameters.AddWithValue("@NgayCapNhat", DateTime.Now);
                            updateCmd.Parameters.AddWithValue("@MaDocGia", maDocGia.Value);
                            updateCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 2627:
                    case 2601:
                        err = "Dữ liệu đã tồn tại (trùng lặp)";
                        break;
                    case 547:
                        err = "Vi phạm ràng buộc khóa ngoại";
                        break;
                    default:
                        err = "Lỗi cập nhật database: " + ex.Message;
                        break;
                }
                return false;
            }
            catch (Exception ex)
            {
                err = "Lỗi hệ thống: " + ex.Message;
                return false;
            }
        }

        // Cập nhật thẻ thư viện
        public bool CapNhatTheThuVien(int maThe, int? maDocGia, DateTime ngayCap, DateTime ngayHetHan, ref string err)
        {
            try
            {
                // Validate input
                if (ngayCap > DateTime.Now)
                {
                    err = "Ngày cấp không được lớn hơn ngày hiện tại";
                    return false;
                }

                if (ngayHetHan <= ngayCap)
                {
                    err = "Ngày hết hạn phải lớn hơn ngày cấp";
                    return false;
                }

                // Lấy thông tin thẻ hiện tại
                TheThuVien theThuVienCu = GetTheThuVienById(maThe);
                if (theThuVienCu == null)
                {
                    err = "Không tìm thấy thẻ thư viện với mã: " + maThe;
                    return false;
                }

                int? oldMaDocGia = theThuVienCu.MaDG;

                // Chỉ kiểm tra validation độc giả khi thực sự thay đổi độc giả
                if (maDocGia != oldMaDocGia)
                {
                    if (maDocGia.HasValue)
                    {
                        if (!KiemTraDocGiaTonTai(maDocGia.Value))
                        {
                            err = "Độc giả không tồn tại hoặc đã bị vô hiệu hóa";
                            return false;
                        }

                        // Kiểm tra độc giả đã có thẻ khác chưa (trừ thẻ hiện tại)
                        if (CheckDocGiaHasCard(maDocGia.Value, maThe))
                        {
                            err = "Độc giả đã có thẻ thư viện khác";
                            return false;
                        }
                    }
                }

                using (SqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        // Cập nhật thẻ thư viện
                        string updateTheQuery = @"
                            UPDATE TheThuVien SET 
                                MaDG = @MaDG,
                                NgayCap = @NgayCap,
                                NgayHetHan = @NgayHetHan
                            WHERE MaThe = @MaThe";

                        SqlCommand updateTheCmd = new SqlCommand(updateTheQuery, conn, transaction);
                        updateTheCmd.Parameters.AddWithValue("@MaThe", maThe);
                        updateTheCmd.Parameters.AddWithValue("@MaDG", maDocGia ?? (object)DBNull.Value);
                        updateTheCmd.Parameters.AddWithValue("@NgayCap", ngayCap);
                        updateTheCmd.Parameters.AddWithValue("@NgayHetHan", ngayHetHan);
                        updateTheCmd.ExecuteNonQuery();

                        // Cập nhật MaThe trong bảng DocGia khi thay đổi độc giả
                        if (maDocGia != oldMaDocGia)
                        {
                            // Xóa liên kết cũ nếu có
                            if (oldMaDocGia.HasValue)
                            {
                                string clearOldQuery = @"
                                    UPDATE DocGia SET MaThe = NULL, NgayCapNhat = @NgayCapNhat
                                    WHERE MaDocGia = @MaDocGia";

                                SqlCommand clearOldCmd = new SqlCommand(clearOldQuery, conn, transaction);
                                clearOldCmd.Parameters.AddWithValue("@NgayCapNhat", DateTime.Now);
                                clearOldCmd.Parameters.AddWithValue("@MaDocGia", oldMaDocGia.Value);
                                clearOldCmd.ExecuteNonQuery();
                            }

                            // Tạo liên kết mới nếu có
                            if (maDocGia.HasValue)
                            {
                                string setNewQuery = @"
                                    UPDATE DocGia SET MaThe = @MaThe, NgayCapNhat = @NgayCapNhat
                                    WHERE MaDocGia = @MaDocGia";

                                SqlCommand setNewCmd = new SqlCommand(setNewQuery, conn, transaction);
                                setNewCmd.Parameters.AddWithValue("@MaThe", maThe);
                                setNewCmd.Parameters.AddWithValue("@NgayCapNhat", DateTime.Now);
                                setNewCmd.Parameters.AddWithValue("@MaDocGia", maDocGia.Value);
                                setNewCmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 2627:
                    case 2601:
                        err = "Dữ liệu đã tồn tại (trùng lặp)";
                        break;
                    case 547:
                        err = "Vi phạm ràng buộc khóa ngoại";
                        break;
                    default:
                        err = "Lỗi cập nhật database: " + ex.Message;
                        break;
                }
                return false;
            }
            catch (Exception ex)
            {
                err = "Lỗi hệ thống: " + ex.Message;
                return false;
            }
        }

        // Lấy thẻ còn hiệu lực
        public DataTable LayTheThuVienConHieuLuc()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Mã thẻ");
            dt.Columns.Add("Tên độc giả");
            dt.Columns.Add("Số ĐT");
            dt.Columns.Add("Ngày cấp");
            dt.Columns.Add("Ngày hết hạn");

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT tt.MaThe, tt.MaDG, tt.NgayCap, tt.NgayHetHan,
                           ISNULL(dg.HoTen, N'Chưa gán') as TenDocGia, 
                           ISNULL(dg.SoDT, '') as SoDT
                    FROM TheThuVien tt
                    LEFT JOIN DocGia dg ON tt.MaDG = dg.MaDocGia
                    WHERE tt.NgayHetHan > GETDATE()
                    ORDER BY tt.NgayHetHan";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dt.Rows.Add(
                        reader["MaThe"].ToString(),
                        reader["TenDocGia"].ToString(),
                        reader["SoDT"].ToString(),
                        Convert.ToDateTime(reader["NgayCap"]).ToString("dd/MM/yyyy"),
                        Convert.ToDateTime(reader["NgayHetHan"]).ToString("dd/MM/yyyy")
                    );
                }
            }
            return dt;
        }

        // Lấy thẻ hết hạn
        public DataTable LayTheThuVienHetHan()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Mã thẻ");
            dt.Columns.Add("Tên độc giả");
            dt.Columns.Add("Số ĐT");
            dt.Columns.Add("Ngày cấp");
            dt.Columns.Add("Ngày hết hạn");
            dt.Columns.Add("Hết hạn từ");

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT tt.MaThe, tt.MaDG, tt.NgayCap, tt.NgayHetHan,
                           ISNULL(dg.HoTen, N'Chưa gán') as TenDocGia, 
                           ISNULL(dg.SoDT, '') as SoDT
                    FROM TheThuVien tt
                    LEFT JOIN DocGia dg ON tt.MaDG = dg.MaDocGia
                    WHERE tt.NgayHetHan <= GETDATE()
                    ORDER BY tt.NgayHetHan DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DateTime ngayHetHan = Convert.ToDateTime(reader["NgayHetHan"]);
                    int soNgayHetHan = (DateTime.Now - ngayHetHan).Days;

                    dt.Rows.Add(
                        reader["MaThe"].ToString(),
                        reader["TenDocGia"].ToString(),
                        reader["SoDT"].ToString(),
                        Convert.ToDateTime(reader["NgayCap"]).ToString("dd/MM/yyyy"),
                        ngayHetHan.ToString("dd/MM/yyyy"),
                        $"{soNgayHetHan} ngày"
                    );
                }
            }
            return dt;
        }

        // Tìm kiếm thẻ thư viện
        public DataTable TimKiemTheThuVien(string tuKhoa)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Mã thẻ");
            dt.Columns.Add("Tên độc giả");
            dt.Columns.Add("Số ĐT");
            dt.Columns.Add("Ngày cấp");
            dt.Columns.Add("Ngày hết hạn");
            dt.Columns.Add("Trạng thái");

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT TOP 500 tt.MaThe, tt.MaDG, tt.NgayCap, tt.NgayHetHan,
                           ISNULL(dg.HoTen, N'Chưa gán') as TenDocGia, 
                           ISNULL(dg.SoDT, '') as SoDT
                    FROM TheThuVien tt
                    LEFT JOIN DocGia dg ON tt.MaDG = dg.MaDocGia
                    WHERE CAST(tt.MaThe AS NVARCHAR) LIKE @SearchText
                       OR dg.HoTen LIKE @SearchText
                       OR dg.SoDT LIKE @SearchText
                    ORDER BY tt.MaThe DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchText", "%" + tuKhoa + "%");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DateTime ngayHetHan = Convert.ToDateTime(reader["NgayHetHan"]);
                    string trangThai = ngayHetHan >= DateTime.Now ? "Còn hiệu lực" : "Hết hạn";

                    dt.Rows.Add(
                        reader["MaThe"].ToString(),
                        reader["TenDocGia"].ToString(),
                        reader["SoDT"].ToString(),
                        Convert.ToDateTime(reader["NgayCap"]).ToString("dd/MM/yyyy"),
                        ngayHetHan.ToString("dd/MM/yyyy"),
                        trangThai
                    );
                }
            }
            return dt;
        }

        // Lấy thông tin thẻ thư viện theo mã
        public TheThuVien LayTheThuVienTheoMa(int maThe)
        {
            return GetTheThuVienById(maThe);
        }

        // Lấy danh sách độc giả chưa có thẻ
        public DataTable LayDocGiaChuaCoThe()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaDG", typeof(int));
            dt.Columns.Add("TenDocGia", typeof(string));

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT MaDocGia as MaDG, HoTen as TenDocGia 
                    FROM DocGia 
                    WHERE TrangThai = 1 AND MaThe IS NULL
                    ORDER BY HoTen";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dt.Rows.Add(
                        Convert.ToInt32(reader["MaDG"]),
                        reader["TenDocGia"].ToString()
                    );
                }
            }
            return dt;
        }

        // Lấy thông tin độc giả theo mã
        public DocGiaDTO LayDocGiaTheoMa(int maDocGia)
        {
            DocGiaDTO docGia = null;

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = "SELECT * FROM DocGia WHERE MaDocGia = @MaDocGia";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDocGia", maDocGia);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    docGia = new DocGiaDTO
                    {
                        MaDocGia = Convert.ToInt32(reader["MaDocGia"]),
                        HoTen = reader["HoTen"].ToString(),
                        SoDT = reader["SoDT"].ToString(),
                        TrangThai = Convert.ToBoolean(reader["TrangThai"])
                    };
                }
            }
            return docGia;
        }

        // Helper methods
        private bool KiemTraDocGiaTonTai(int maDocGia)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM DocGia WHERE MaDocGia = @MaDocGia AND TrangThai = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDocGia", maDocGia);

                conn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        // Các method cũ vẫn giữ nguyên để tương thích
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
            return LayDocGiaChoComboBox();
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
