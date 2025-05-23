using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Repositories
{
    public class BorrowSlipDetailRepository
    {
        private DBConnection db;

        public BorrowSlipDetailRepository(DBConnection dbConnection)
        {
            this.db = dbConnection;
        }

        public List<BorrowSlipDetail> GetBySlipId(int slipId)
        {
            var list = new List<BorrowSlipDetail>();
            string query = "SELECT * FROM BorrowSlipDetails WHERE SlipID = @SlipID";
            using (SqlCommand cmd = new SqlCommand(query, db.conn))
            {
                cmd.Parameters.AddWithValue("@SlipID", slipId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new BorrowSlipDetail
                        {
                            SlipDetailID = (int)reader["SlipDetailID"],
                            SlipID = (int)reader["SlipID"],
                            BookID = (int)reader["BookID"],
                            IsReturned = (bool)reader["IsReturned"]
                        });
                    }
                }
            }
            return list;
        }
    }
}
