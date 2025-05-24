using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Windows.Forms;

    namespace LibraryManagement.Repositories
    {
        public class TacGiaRepository : DBConnection
        {
            public void AddTacGia(string tenTG)
            {
                string query = "INSERT INTO TacGia (TenTG) VALUES (@TenTG)";
                SqlCommand cmd = new SqlCommand(query, GetConnection());
                cmd.Parameters.AddWithValue("@TenTG", tenTG);
                GetConnection().Open();
                cmd.ExecuteNonQuery();
                GetConnection().Close();
            }

            public void UpdateTacGia(int maTacGia, string tenTG)
            {
                string query = "UPDATE TacGia SET TenTG = @TenTG WHERE MaTacGia = @MaTacGia";
                SqlCommand cmd = new SqlCommand(query, GetConnection());
                cmd.Parameters.AddWithValue("@MaTacGia", maTacGia);
                cmd.Parameters.AddWithValue("@TenTG", tenTG);
                GetConnection().Open();
                cmd.ExecuteNonQuery();
                GetConnection().Close();
            }

            public void DeleteTacGia(int maTacGia)
            {
                string query = "DELETE FROM TacGia WHERE MaTacGia = @MaTacGia";
                SqlCommand cmd = new SqlCommand(query, GetConnection());
                cmd.Parameters.AddWithValue("@MaTacGia", maTacGia);
                GetConnection().Open();
                cmd.ExecuteNonQuery();
                GetConnection().Close();
            }

            public List<(int MaTacGia, string TenTG)> GetAllTacGia()
            {
                List<(int, string)> result = new List<(int, string)>();
                string query = "SELECT MaTacGia, TenTG FROM TacGia";

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
                                    result.Add((reader.GetInt32(0), reader.GetString(1)));
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

            public List<string> SearchTacGia(string keyword)
            {
                List<string> result = new List<string>();
                string query = "SELECT TenTG FROM TacGia WHERE TenTG LIKE @Keyword";
                SqlCommand cmd = new SqlCommand(query, GetConnection());
                cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                GetConnection().Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(reader.GetString(0));
                }
                GetConnection().Close();
                return result;
            }
        }
    }

}
