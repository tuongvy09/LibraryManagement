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
    public class TheLoaiRepository : DBConnection
    {
        public void AddTheLoai(int qdSoTuoi, string tenTheLoai)
        {
            string query = "INSERT INTO TheLoai (QDSoTuoi, TenTheLoai) VALUES (@QDSoTuoi, @TenTheLoai)";

            using (SqlConnection conn = new DBConnection().GetConnection())
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@QDSoTuoi", qdSoTuoi);
                        cmd.Parameters.AddWithValue("@TenTheLoai", tenTheLoai ?? (object)DBNull.Value); // tránh lỗi nếu null

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể thêm thể loại.\nChi tiết: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void UpdateTheLoai(int maTheLoai, int qdSoTuoi)
        {
            string query = "UPDATE TheLoai SET QDSoTuoi = @QDSoTuoi WHERE MaTheLoai = @MaTheLoai";

            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
                    cmd.Parameters.AddWithValue("@QDSoTuoi", qdSoTuoi);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTheLoai(int maTheLoai)
        {
            string query = "DELETE FROM TheLoai WHERE MaTheLoai = @MaTheLoai";

            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable SearchTheLoai(string keyword)
        {
            DataTable dataTable = new DataTable();
            string query = "SELECT * FROM TheLoai WHERE TenTheLoai LIKE @Keyword";

            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }

        public List<TheLoai> GetAllTheLoai()
        {
            List<TheLoai> result = new List<TheLoai>();
            string query = "SELECT * FROM TheLoai";

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
                                TheLoai theLoai = new TheLoai
                                {
                                    MaTheLoai = reader.GetInt32(0),
                                    QDSoTuoi = reader.GetInt32(1),
                                    TenTheLoai = reader.GetString(2)
                                };
                                result.Add(theLoai);
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
