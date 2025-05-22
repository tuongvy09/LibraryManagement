using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            using (SqlCommand cmd = new SqlCommand(query, dbConnection.conn))
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
            return user;
        }
    }

}
