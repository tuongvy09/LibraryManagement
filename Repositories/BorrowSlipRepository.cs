using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Repositories
{
    public class BorrowSlipRepository
    {
        private DBConnection db;

        public BorrowSlipRepository(DBConnection dbConnection)
        {
            this.db = dbConnection;
        }

        public List<BorrowSlip> GetAll()
        {
            var list = new List<BorrowSlip>();
            string query = "SELECT * FROM BorrowSlips";
            using (SqlCommand cmd = new SqlCommand(query, db.conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new BorrowSlip
                        {
                            SlipID = (int)reader["SlipID"],
                            ReaderID = (int)reader["ReaderID"],
                            BorrowDate = (DateTime)reader["BorrowDate"],
                            ReturnDate = reader["ReturnDate"] as DateTime?
                        });
                    }
                }
            }
            return list;
        }
    }
}
