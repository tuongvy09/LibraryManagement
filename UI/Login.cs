using LibraryManagement.Repositories;
using LibraryManagement.Services;
using System;
using System.Windows.Forms;

namespace LibraryManagement
{
    public partial class Login : Form
    {
        private AuthService authService;

        public Login()
        {
            InitializeComponent();

            // Khởi tạo chuỗi kết nối và inject phụ thuộc
            var dbConnection = new DBConnection();
            using (var conn = dbConnection.GetConnection())
            {
                if (conn == null)
                {
                    MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.");
                    return;
                }
                conn.Open();

                var userRepository = new UserRepository(dbConnection);
                authService = new AuthService(userRepository);

            }

        }

        private void Login_Load(object sender, EventArgs e)
        {
        }

        private void label6_Click(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = txtTen.Text.Trim();
            string password = txtPassWord.Text.Trim();

            if (authService.Login(username, password, out string role))
            {
                MessageBox.Show("Đăng nhập thành công! Quyền: " + role);
                this.Hide();
                new Home().Show(); // Có thể thay đổi theo role
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu sai!");
            }
        }
    }
}
