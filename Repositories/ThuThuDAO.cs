using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Models;

namespace LibraryManagement.Repositories
{
    public class ThuThuDAO
    {
        private DBConnection dbConnection;

        public ThuThuDAO()
        {
            dbConnection = new DBConnection();
        }

        public List<ThuThu> GetAllThuThu()
        {
            List<ThuThu> thuThus = new List<ThuThu>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT tt.MaThuThu, tt.TenThuThu, tt.Email, tt.SoDienThoai, 
                           tt.DiaChi, tt.NgayBatDauLam, tt.NgaySinh, tt.GioiTinh,
                           tt.TrangThai, tt.NgayTao, tt.NgayCapNhat, tt.UserID,
                           u.Username, u.Role
                    FROM ThuThu tt
                    LEFT JOIN Users u ON tt.UserID = u.UserID
                    ORDER BY tt.TenThuThu";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ThuThu thuThu = new ThuThu()
                    {
                        MaThuThu = Convert.ToInt32(reader["MaThuThu"]),
                        TenThuThu = reader["TenThuThu"].ToString(),
                        Email = reader["Email"]?.ToString(),
                        SoDienThoai = reader["SoDienThoai"]?.ToString(),
                        DiaChi = reader["DiaChi"]?.ToString(),
                        NgayBatDauLam = reader["NgayBatDauLam"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["NgayBatDauLam"]),
                        NgaySinh = reader["NgaySinh"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["NgaySinh"]),
                        GioiTinh = reader["GioiTinh"]?.ToString(),
                        TrangThai = reader["TrangThai"] == DBNull.Value ? true : Convert.ToBoolean(reader["TrangThai"]),
                        NgayTao = reader["NgayTao"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["NgayTao"]),
                        NgayCapNhat = reader["NgayCapNhat"] == DBNull.Value ? null : (DateTime?)reader["NgayCapNhat"],
                        UserID = reader["UserID"] == DBNull.Value ? null : (int?)reader["UserID"],
                        Username = reader["Username"]?.ToString(),
                        Role = reader["Role"]?.ToString()
                    };
                    thuThus.Add(thuThu);
                }
            }
            return thuThus;
        }

        public bool InsertThuThu(ThuThu thuThu, string username, string password)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Tạo user trước
                    string userQuery = @"
                        INSERT INTO Users (Username, Password, Role) 
                        VALUES (@Username, @Password, @Role);
                        SELECT SCOPE_IDENTITY();";

                    SqlCommand userCmd = new SqlCommand(userQuery, conn, transaction);
                    userCmd.Parameters.AddWithValue("@Username", username);
                    userCmd.Parameters.AddWithValue("@Password", password); // Nên hash password
                    userCmd.Parameters.AddWithValue("@Role", "ThuThu");

                    int userID = Convert.ToInt32(userCmd.ExecuteScalar());

                    // Tạo thủ thư
                    string thuThuQuery = @"
                        INSERT INTO ThuThu (TenThuThu, Email, SoDienThoai, DiaChi, 
                                          NgayBatDauLam, NgaySinh, GioiTinh, TrangThai, 
                                          NgayTao, UserID) 
                        VALUES (@TenThuThu, @Email, @SoDienThoai, @DiaChi, 
                                @NgayBatDauLam, @NgaySinh, @GioiTinh, @TrangThai, 
                                @NgayTao, @UserID)";

                    SqlCommand thuThuCmd = new SqlCommand(thuThuQuery, conn, transaction);
                    thuThuCmd.Parameters.AddWithValue("@TenThuThu", thuThu.TenThuThu);
                    thuThuCmd.Parameters.AddWithValue("@Email", thuThu.Email ?? (object)DBNull.Value);
                    thuThuCmd.Parameters.AddWithValue("@SoDienThoai", thuThu.SoDienThoai ?? (object)DBNull.Value);
                    thuThuCmd.Parameters.AddWithValue("@DiaChi", thuThu.DiaChi ?? (object)DBNull.Value);
                    thuThuCmd.Parameters.AddWithValue("@NgayBatDauLam", thuThu.NgayBatDauLam);
                    thuThuCmd.Parameters.AddWithValue("@NgaySinh", thuThu.NgaySinh);
                    thuThuCmd.Parameters.AddWithValue("@GioiTinh", thuThu.GioiTinh ?? (object)DBNull.Value);
                    thuThuCmd.Parameters.AddWithValue("@TrangThai", thuThu.TrangThai);
                    thuThuCmd.Parameters.AddWithValue("@NgayTao", DateTime.Now);
                    thuThuCmd.Parameters.AddWithValue("@UserID", userID);

                    int result = thuThuCmd.ExecuteNonQuery();

                    transaction.Commit();
                    return result > 0;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public bool UpdateThuThu(ThuThu thuThu)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    UPDATE ThuThu SET 
                        TenThuThu = @TenThuThu,
                        Email = @Email,
                        SoDienThoai = @SoDienThoai,
                        DiaChi = @DiaChi,
                        NgayBatDauLam = @NgayBatDauLam,
                        NgaySinh = @NgaySinh,
                        GioiTinh = @GioiTinh,
                        TrangThai = @TrangThai,
                        NgayCapNhat = @NgayCapNhat
                    WHERE MaThuThu = @MaThuThu";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaThuThu", thuThu.MaThuThu);
                cmd.Parameters.AddWithValue("@TenThuThu", thuThu.TenThuThu);
                cmd.Parameters.AddWithValue("@Email", thuThu.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SoDienThoai", thuThu.SoDienThoai ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DiaChi", thuThu.DiaChi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NgayBatDauLam", thuThu.NgayBatDauLam);
                cmd.Parameters.AddWithValue("@NgaySinh", thuThu.NgaySinh);
                cmd.Parameters.AddWithValue("@GioiTinh", thuThu.GioiTinh ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TrangThai", thuThu.TrangThai);
                cmd.Parameters.AddWithValue("@NgayCapNhat", DateTime.Now);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteThuThu(int maThuThu)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = "UPDATE ThuThu SET TrangThai = 0 WHERE MaThuThu = @MaThuThu";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaThuThu", maThuThu);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<ThuThu> SearchThuThu(string searchText)
        {
            List<ThuThu> thuThus = new List<ThuThu>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT tt.MaThuThu, tt.TenThuThu, tt.Email, tt.SoDienThoai, 
                           tt.DiaChi, tt.NgayBatDauLam, tt.NgaySinh, tt.GioiTinh,
                           tt.TrangThai, tt.NgayTao, tt.NgayCapNhat, tt.UserID,
                           u.Username, u.Role
                    FROM ThuThu tt
                    LEFT JOIN Users u ON tt.UserID = u.UserID
                    WHERE tt.TenThuThu LIKE @SearchText 
                       OR tt.Email LIKE @SearchText 
                       OR tt.SoDienThoai LIKE @SearchText
                       OR u.Username LIKE @SearchText
                    ORDER BY tt.TenThuThu";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ThuThu thuThu = new ThuThu()
                    {
                        MaThuThu = Convert.ToInt32(reader["MaThuThu"]),
                        TenThuThu = reader["TenThuThu"].ToString(),
                        Email = reader["Email"]?.ToString(),
                        SoDienThoai = reader["SoDienThoai"]?.ToString(),
                        DiaChi = reader["DiaChi"]?.ToString(),
                        NgayBatDauLam = reader["NgayBatDauLam"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["NgayBatDauLam"]),
                        NgaySinh = reader["NgaySinh"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["NgaySinh"]),
                        GioiTinh = reader["GioiTinh"]?.ToString(),
                        TrangThai = reader["TrangThai"] == DBNull.Value ? true : Convert.ToBoolean(reader["TrangThai"]),
                        NgayTao = reader["NgayTao"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["NgayTao"]),
                        NgayCapNhat = reader["NgayCapNhat"] == DBNull.Value ? null : (DateTime?)reader["NgayCapNhat"],
                        UserID = reader["UserID"] == DBNull.Value ? null : (int?)reader["UserID"],
                        Username = reader["Username"]?.ToString(),
                        Role = reader["Role"]?.ToString()
                    };
                    thuThus.Add(thuThu);
                }
            }
            return thuThus;
        }
    }
}
