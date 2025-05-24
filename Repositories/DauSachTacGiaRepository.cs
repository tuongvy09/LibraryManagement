using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Repositories
{
    public class DauSachTacGiaRepository : DBConnection
    {
        public void AddDauSachTacGia(int maDauSach, int maTacGia)
        {
            string query = "INSERT INTO DauSach_TacGia (MaDauSach, MaTacGia) VALUES (@MaDauSach, @MaTacGia)";
            SqlCommand cmd = new SqlCommand(query, GetConnection());
            cmd.Parameters.AddWithValue("@MaDauSach", maDauSach);
            cmd.Parameters.AddWithValue("@MaTacGia", maTacGia);
            GetConnection().Open();
            cmd.ExecuteNonQuery();
            GetConnection().Close();
        }

        public void DeleteDauSachTacGia(int maDauSach, int maTacGia)
        {
            string query = "DELETE FROM DauSach_TacGia WHERE MaDauSach = @MaDauSach AND MaTacGia = @MaTacGia";
            SqlCommand cmd = new SqlCommand(query, GetConnection());
            cmd.Parameters.AddWithValue("@MaDauSach", maDauSach);
            cmd.Parameters.AddWithValue("@MaTacGia", maTacGia);
            GetConnection().Open();
            cmd.ExecuteNonQuery();
            GetConnection().Close();
        }

        public List<(int MaDauSach, int MaTacGia)> GetAll()
        {
            List<(int, int)> result = new List<(int, int)>();
            string query = "SELECT MaDauSach, MaTacGia FROM DauSach_TacGia";
            SqlCommand cmd = new SqlCommand(query, GetConnection());
            GetConnection().Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add((reader.GetInt32(0), reader.GetInt32(1)));
            }
            GetConnection().Close();
            return result;
        }
    }

}
