using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryManagement.Repositories
{
    public class NXBRepository : DBConnection
    {
        public void AddNXB(string tenNXB)
        {
            using (SqlConnection conn = GetConnection())
            {
                string query = "INSERT INTO NXB (TenNSB) VALUES (@TenNSB)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenNSB", tenNXB);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateNXB(int maNXB, string tenNXB)
        {
            string query = "UPDATE NXB SET TenNSB = @TenNXB WHERE MaNXB = @MaNXB";

            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNXB", maNXB);
                    cmd.Parameters.AddWithValue("@TenNSB", tenNXB);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteNXB(int maNXB)
        {
            string query = "DELETE FROM NXB WHERE MaNXB = @MaNXB";

            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNXB", maNXB);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Hàm tìm kiếm NXB theo tên (partial match)
        public DataTable SearchNXB(string tenNXB)
        {
            string query = "SELECT * FROM NXB WHERE TenNSB LIKE @TenNSB";

            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenNSB", "%" + tenNXB + "%");

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public List<NhaXuatBan> GetAllNXB()
        {
            List<NhaXuatBan> result = new List<NhaXuatBan>();
            string query = "SELECT MaNXB, TenNSB FROM NXB";

            var db = new DBConnection();
            using (SqlConnection conn = db.GetConnection())
            {
                if (conn == null)
                {
                    MessageBox.Show("Không thể tạo kết nối đến CSDL.");
                    return result;
                }

                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                NhaXuatBan nxb = new NhaXuatBan
                                {
                                    MaNXb = reader.GetInt32(0),
                                    TenNXB = reader.GetString(1)
                                };
                                result.Add(nxb);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi truy vấn CSDL: {ex.Message}");
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
            }

            return result;
        }
    }

}
