using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Repositories
{
    public class PhieuMuonDAO
    {
        private DBConnection dbConnection;

        public PhieuMuonDAO()
        {
            dbConnection = new DBConnection();
        }

        public List<PhieuMuon> GetAllPhieuMuon()
        {
            List<PhieuMuon> list = new List<PhieuMuon>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT pm.MaPhieu, pm.MaDocGia, dg.HoTen as TenDocGia
                    FROM PhieuMuonSach pm
                    LEFT JOIN DocGia dg ON pm.MaDocGia = dg.MaDocGia
                    ORDER BY pm.MaPhieu DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new PhieuMuon
                    {
                        MaPhieu = Convert.ToInt32(reader["MaPhieu"]),
                        MaDocGia = reader["MaDocGia"] == DBNull.Value ? null : (int?)reader["MaDocGia"],
                        TenDocGia = reader["TenDocGia"]?.ToString() ?? ""
                    });
                }
            }

            return list;
        }

        public bool InsertPhieuMuon(PhieuMuon pm)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    INSERT INTO PhieuMuonSach (MaDocGia)
                    VALUES (@MaDocGia);
                    SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaDocGia", pm.MaDocGia ?? (object)DBNull.Value);

                conn.Open();
                var result = cmd.ExecuteScalar();
                pm.MaPhieu = Convert.ToInt32(result);
                return pm.MaPhieu > 0;
            }
        }

        public bool UpdatePhieuMuon(PhieuMuon pm)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = "UPDATE PhieuMuonSach SET MaDocGia = @MaDocGia WHERE MaPhieu = @MaPhieu";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPhieu", pm.MaPhieu);
                cmd.Parameters.AddWithValue("@MaDocGia", pm.MaDocGia ?? (object)DBNull.Value);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeletePhieuMuon(int maPhieu)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    DELETE FROM PhieuMuonSach_CuonSach WHERE MaPhieu = @MaPhieu;
                    DELETE FROM PhieuMuonSach WHERE MaPhieu = @MaPhieu";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPhieu", maPhieu);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool AddCuonSachToPhieu(int maPhieu, int maCuonSach)
        {
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    INSERT INTO PhieuMuonSach_CuonSach (MaPhieu, MaSach)
                    VALUES (@MaPhieu, @MaSach)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPhieu", maPhieu);
                cmd.Parameters.AddWithValue("@MaSach", maCuonSach);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<int> GetCuonSachInPhieu(int maPhieu)
        {
            List<int> list = new List<int>();
            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = "SELECT MaSach FROM PhieuMuonSach_CuonSach WHERE MaPhieu = @MaPhieu";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPhieu", maPhieu);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(Convert.ToInt32(reader["MaSach"]));
                }
            }
            return list;
        }

        public PhieuMuon GetPhieuMuonById(int maPhieu)
        {
            PhieuMuon pm = null;

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT pm.MaPhieu, pm.MaDocGia, dg.HoTen as TenDocGia
                    FROM PhieuMuonSach pm
                    LEFT JOIN DocGia dg ON pm.MaDocGia = dg.MaDocGia
                    WHERE pm.MaPhieu = @MaPhieu";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaPhieu", maPhieu);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    pm = new PhieuMuon
                    {
                        MaPhieu = Convert.ToInt32(reader["MaPhieu"]),
                        MaDocGia = reader["MaDocGia"] == DBNull.Value ? null : (int?)reader["MaDocGia"],
                        TenDocGia = reader["TenDocGia"]?.ToString() ?? ""
                    };
                }
            }

            return pm;
        }

        public List<PhieuMuon> SearchPhieuMuon(string searchText)
        {
            List<PhieuMuon> list = new List<PhieuMuon>();

            using (SqlConnection conn = dbConnection.GetConnection())
            {
                string query = @"
                    SELECT pm.MaPhieu, pm.MaDocGia, dg.HoTen as TenDocGia
                    FROM PhieuMuonSach pm
                    LEFT JOIN DocGia dg ON pm.MaDocGia = dg.MaDocGia
                    WHERE CAST(pm.MaPhieu AS NVARCHAR) LIKE @SearchText
                       OR dg.HoTen LIKE @SearchText
                    ORDER BY pm.MaPhieu DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new PhieuMuon
                    {
                        MaPhieu = Convert.ToInt32(reader["MaPhieu"]),
                        MaDocGia = reader["MaDocGia"] == DBNull.Value ? null : (int?)reader["MaDocGia"],
                        TenDocGia = reader["TenDocGia"]?.ToString() ?? ""
                    });
                }
            }

            return list;
        }
    }
}
