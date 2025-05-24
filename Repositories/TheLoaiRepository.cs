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
    public class TheLoaiRepository : DBConnection
    {
        public void AddTheLoai(int qdSoTuoi)
        {
            string query = "INSERT INTO TheLoai (QDSoTuoi) VALUES (@QDSoTuoi)";
            SqlCommand cmd = new SqlCommand(query, GetConnection());
            cmd.Parameters.AddWithValue("@QDSoTuoi", qdSoTuoi);
            GetConnection().Open();
            cmd.ExecuteNonQuery();
            GetConnection().Close();
        }

        public void UpdateTheLoai(int maTheLoai, int qdSoTuoi)
        {
            string query = "UPDATE TheLoai SET QDSoTuoi = @QDSoTuoi WHERE MaTheLoai = @MaTheLoai";
            SqlCommand cmd = new SqlCommand(query, GetConnection());
            cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
            cmd.Parameters.AddWithValue("@QDSoTuoi", qdSoTuoi);
            GetConnection().Open();
            cmd.ExecuteNonQuery();
            GetConnection().Close();
        }

        public void DeleteTheLoai(int maTheLoai)
        {
            string query = "DELETE FROM TheLoai WHERE MaTheLoai = @MaTheLoai";
            SqlCommand cmd = new SqlCommand(query, GetConnection());
            cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
            GetConnection().Open();
            cmd.ExecuteNonQuery();
            GetConnection().Close();
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
