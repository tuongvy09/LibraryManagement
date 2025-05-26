using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using LibraryManagement.Models;

namespace LibraryManagement.Repositories
{
    public class LoaiDocGiaDAO
    {
        private DBConnection dbConnection;

        public LoaiDocGiaDAO()
        {
            dbConnection = new DBConnection();
        }

        public List<LoaiDocGia> GetAllLoaiDocGia()
        {
            List<LoaiDocGia> loaiDocGias = new List<LoaiDocGia>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = "SELECT MaLoaiDG, TenLoaiDG, DoTuoi FROM LoaiDocGia ORDER BY TenLoaiDG";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    LoaiDocGia loaiDG = new LoaiDocGia()
                    {
                        MaLoaiDG = Convert.ToInt32(reader["MaLoaiDG"]),
                        TenLoaiDG = reader["TenLoaiDG"].ToString(),
                        DoTuoi = Convert.ToInt32(reader["DoTuoi"])
                    };
                    loaiDocGias.Add(loaiDG);
                }
            }
            return loaiDocGias;
        }
    }
}

