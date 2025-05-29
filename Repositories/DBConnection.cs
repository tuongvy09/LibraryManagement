using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LibraryManagement.Repositories
{
    public class DBConnection
    {
        private readonly string connectionString = @"Data Source=DESKTOP-IDTU04D;Initial Catalog=LibraryManagement1;Integrated Security=True";

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
