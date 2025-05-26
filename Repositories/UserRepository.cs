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
    public class UserRepository
    {
        private DBConnection dbConnection;

        public UserRepository(DBConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public User GetUserByUsername(string username)
        {
            User user = null;
            string query = "SELECT UserID, Username, Password, Role FROM Users WHERE Username = @Username";

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                UserID = (int)reader["UserID"],
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                Role = reader["Role"].ToString()
                            };
                        }
                    }
                }

                conn.Close();
            }
            return user;
        }
        public List<User> GetAllUsers()
        {
            List<User> userList = new List<User>();
            string query = "SELECT UserID, Username, Password, Role FROM Users";

            var db = new DBConnection();
            using (SqlConnection conn = db.GetConnection())
            {
                if (conn == null)
                {
                    MessageBox.Show("Không thể kết nối đến database.");
                    return userList;
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
                                userList.Add(new User()
                                {
                                    UserID = reader.GetInt32(0),
                                    Username = reader.GetString(1),
                                    Password = reader.GetString(2),
                                    Role = reader.GetString(3)
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi truy vấn dữ liệu: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            return userList;
        }

        public bool AddUser(string username, string password, string role)
        {
            string query = "INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, @Role)";

            var db = new DBConnection();
            using (SqlConnection conn = db.GetConnection())
            {
                if (conn == null)
                {
                    MessageBox.Show("Không thể kết nối đến database.");
                    return false;
                }

                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password); // Có thể mã hóa trước
                        cmd.Parameters.AddWithValue("@Role", role);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi SQL: " + ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public bool UpdateUser(int userId, string username, string password, string role)
        {
            string query = "UPDATE Users SET Username = @Username, Password = @Password, Role = @Role WHERE UserID = @UserID";

            var db = new DBConnection();
            using (SqlConnection conn = db.GetConnection())
            {
                if (conn == null)
                {
                    MessageBox.Show("Không thể kết nối đến database.");
                    return false;
                }

                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password); // Cân nhắc mã hóa
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi SQL: " + ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public bool DeleteUser(int userId)
        {
            string query = "DELETE FROM Users WHERE UserID = @UserID";

            var db = new DBConnection();
            using (SqlConnection conn = db.GetConnection())
            {
                if (conn == null)
                {
                    MessageBox.Show("Không thể kết nối đến database.");
                    return false;
                }

                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi SQL: " + ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }

}
