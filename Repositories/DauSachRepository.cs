using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryManagement.Models;

namespace LibraryManagement.Repositories
{
    public class DauSachRepository : DBConnection
    {
        public List<DauSach> GetAllMaDauSach()
        {
            List<DauSach> list = new List<DauSach>();
            string query = "SELECT MaDauSach, TenDauSach FROM DauSach";

            var db = new DBConnection();
            using (SqlConnection conn = db.GetConnection())
            {
                if (conn == null)
                {
                    MessageBox.Show("Không thể kết nối đến database.");
                    return list;
                }

                conn.Open();  // Mở kết nối đúng đối tượng này

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new DauSach()
                            {
                                MaDauSach = reader.GetInt32(0),
                                TenDauSach = reader.GetString(1)
                            });
                        }
                    }
                }

                conn.Close(); // Đóng kết nối
            }

            return list;
        }

        public void AddDauSach(string tenDauSach, int maTheLoai, int maNXB)
        {
            using (SqlConnection conn = GetConnection())
            {
                string query = "INSERT INTO DauSach (MaTheLoai, MaNXB, TenDauSach) VALUES (@MaTheLoai, @MaNXB, @tenDauSach)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenDauSach", tenDauSach);
                    cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
                    cmd.Parameters.AddWithValue("@MaNXB", maNXB);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public void UpdateDauSach(int maDauSach, int maTheLoai, int maNXB)
        {
            string query = "UPDATE DauSach SET MaTheLoai = @MaTheLoai, MaNXB = @MaNXB WHERE MaDauSach = @MaDauSach";
            SqlCommand cmd = new SqlCommand(query, GetConnection());
            cmd.Parameters.AddWithValue("@MaDauSach", maDauSach);
            cmd.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
            cmd.Parameters.AddWithValue("@MaNXB", maNXB);
            GetConnection().Open();
            cmd.ExecuteNonQuery();
            GetConnection().Close();
        }

        public void DeleteDauSach(int maDauSach)
        {
            string query = "DELETE FROM DauSach WHERE MaDauSach = @MaDauSach";
            SqlCommand cmd = new SqlCommand(query, GetConnection());
            cmd.Parameters.AddWithValue("@MaDauSach", maDauSach);
            GetConnection().Open();
            cmd.ExecuteNonQuery();
            GetConnection().Close();
        }
    }

}
