using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Repositories
{
    public class DauSachTacGiaRepository : DBConnection
    {
        public void AddTacGiaChoDauSach(int maDauSach, int maTacGia)
        {
            using (SqlConnection conn = GetConnection())
            {
                string query = "INSERT INTO DauSach_TacGia (MaDauSach, MaTacGia) VALUES (@MaDauSach, @MaTacGia)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDauSach", maDauSach);
                    cmd.Parameters.AddWithValue("@MaTacGia", maTacGia);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public void UpdateTacGiaChoDauSach(int maDauSach, List<int> danhSachMaTacGiaMoi)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Xóa tất cả tác giả cũ của đầu sách
                        string deleteQuery = "DELETE FROM DauSach_TacGia WHERE MaDauSach = @MaDauSach";
                        using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn, transaction))
                        {
                            deleteCmd.Parameters.AddWithValue("@MaDauSach", maDauSach);
                            deleteCmd.ExecuteNonQuery();
                        }

                        // 2. Thêm các tác giả mới vào đầu sách
                        string insertQuery = "INSERT INTO DauSach_TacGia (MaDauSach, MaTacGia) VALUES (@MaDauSach, @MaTacGia)";
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn, transaction))
                        {
                            insertCmd.Parameters.Add("@MaDauSach", SqlDbType.Int).Value = maDauSach;
                            insertCmd.Parameters.Add("@MaTacGia", SqlDbType.Int);

                            foreach (int maTacGiaMoi in danhSachMaTacGiaMoi)
                            {
                                insertCmd.Parameters["@MaTacGia"].Value = maTacGiaMoi;
                                insertCmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;  // Ném lại lỗi cho caller xử lý
                    }
                }
            }
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
        public List<int> LayDanhSachMaTacGiaTheoDauSach(int maDauSach)
        {
            List<int> maTacGiaList = new List<int>();

            using (SqlConnection conn = GetConnection())
            {
                string query = "SELECT MaTacGia FROM DauSach_TacGia WHERE MaDauSach = @MaDauSach";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDauSach", maDauSach);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            maTacGiaList.Add(reader.GetInt32(0));
                        }
                    }
                }
            }

            return maTacGiaList;
        }

    }

}
