using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Models;

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
                using (SqlConnection conn = GetConnection())
                {
                    string query = "INSERT INTO TacGia (TenTG) VALUES (@TenTG)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TenTG", tenTG);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thêm Tác giả: " + ex.Message);
                    }
                }
            }

            public void UpdateTacGia(int maTacGia, string tenTG)
            {
                string query = "UPDATE TacGia SET TenTG = @TenTG WHERE MaTacGia = @MaTacGia";

                using (SqlConnection conn = GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaTacGia", maTacGia);
                    cmd.Parameters.AddWithValue("@TenTG", tenTG);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            public void DeleteTacGia(int maTacGia)
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    string deleteLinksQuery = "DELETE FROM DauSach_TacGia WHERE MaTacGia = @MaTacGia";
                    using (SqlCommand cmdLinks = new SqlCommand(deleteLinksQuery, conn))
                    {
                        cmdLinks.Parameters.AddWithValue("@MaTacGia", maTacGia);
                        cmdLinks.ExecuteNonQuery();
                    }

                    string deleteTacGiaQuery = "DELETE FROM TacGia WHERE MaTacGia = @MaTacGia";
                    using (SqlCommand cmdDelete = new SqlCommand(deleteTacGiaQuery, conn))
                    {
                        cmdDelete.Parameters.AddWithValue("@MaTacGia", maTacGia);
                        cmdDelete.ExecuteNonQuery();
                    }
                }
            }

            public List<TacGia> GetAllTacGia()
            {
                List<TacGia> result = new List<TacGia>();
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
                                    TacGia tg = new TacGia
                                    {
                                        MaTacGia = reader.GetInt32(0),
                                        TenTG = reader.GetString(1)
                                    };
                                    result.Add(tg);
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

            public List<TacGia> GetTacGias(string searchTerm = "")
            {
                List<TacGia> list = new List<TacGia>();

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    string query = "SELECT * FROM TacGia";

                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        query += " WHERE TenTG LIKE @SearchTerm";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(searchTerm))
                        {
                            cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                        }

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TacGia tg = new TacGia()
                                {
                                    MaTacGia = (int)reader["MaTacGia"],
                                    TenTG = reader["TenTG"].ToString(),
                                };
                                list.Add(tg);
                            }
                        }
                    }
                }
                return list;
            }
        }
    }

}
