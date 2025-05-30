using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LibraryManagement.Repositories
{
    public class DBConnection
    {
        private readonly string connectionString = @"Data Source=LAPTOP-S0F5B0E7;Initial Catalog=FinalProjectLtWins;Integrated Security=True";

        public SqlConnection GetConnection()
        {
            try
            {
                var conn = new SqlConnection(connectionString);
                return conn;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tạo kết nối đến CSDL.\nChi tiết: {ex.Message}", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
