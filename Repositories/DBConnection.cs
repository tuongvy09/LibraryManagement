using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Forms;

namespace LibraryManagement.Repositories
{
    public class DBConnection
    {
        public SqlConnection conn;
        public DBConnection()
        {
            try
            {
                // Thay đổi lại Data Source bên dưới nếu tên server bạn khác
                string connectionString = @"Data Source=LAPTOP-S0F5B0E7;Initial Catalog=FinalProjectLtWins;Integrated Security=True";

                conn = new SqlConnection(connectionString);
                conn.Open(); // Mở kết nối ngay khi khởi tạo (nếu muốn)
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kết nối đến cơ sở dữ liệu thất bại: " + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CloseConnection()
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }

    }
}
