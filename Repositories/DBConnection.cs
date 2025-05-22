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
                conn = new SqlConnection(@"Data Source=DESKTOP-6LGUMNF;Initial Catalog=FinalProjectLtWins;Integrated Security=True");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Login failed");
            }
        }
    }
}
