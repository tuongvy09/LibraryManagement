using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Repositories
{
    public class PhieuPhatDAO
    {
        private DBConnection dbConnection;

        public PhieuPhatDAO()
        {
            dbConnection = new DBConnection();
        }

        // Lấy tất cả phiếu phạt kèm thông tin độc giả
        public List<PhieuPhat> GetAllPhieuPhat()
        {
            var list = new List<PhieuPhat>();
            string query = @"
                SELECT pp.MaPhieuPhat, dg.HoTen, dg.MaDocGia
                FROM PhieuPhat pp
                INNER JOIN DocGia dg ON pp.MaDG = dg.MaDocGia"; // Lưu ý: FK ở bảng PhieuPhat là MaDG, liên kết với MaDocGia

            using (SqlConnection con = dbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var phieuPhat = new PhieuPhat
                        {
                            MaPhieuPhat = reader.GetInt32(0),
                            HoTen = reader.IsDBNull(1) ? null : reader.GetString(1),
                            MaDG = reader.GetInt32(2)
                        };

                        phieuPhat.LoiViPhams = GetLoiViPhamByPhieuPhat(phieuPhat.MaPhieuPhat);
                        phieuPhat.TongTien = phieuPhat.LoiViPhams.Sum(l => l.TienPhat);

                        list.Add(phieuPhat);
                    }
                }
            }

            return list;
        }

        // Lấy danh sách lỗi vi phạm theo phiếu phạt
        public List<QDP> GetLoiViPhamByPhieuPhat(int maPhieuPhat)
        {
            var list = new List<QDP>();
            string query = @"
                SELECT q.MaQDP, q.LyDo, q.TienPhat
                FROM PhieuPhat_QDP pq
                INNER JOIN QDP q ON pq.MaQDP = q.MaQDP
                WHERE pq.MaPhieuPhat = @MaPhieuPhat";

            using (SqlConnection con = dbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@MaPhieuPhat", maPhieuPhat);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var qdp = new QDP
                        {
                            MaQDP = reader.GetInt32(0),
                            LyDo = reader.IsDBNull(1) ? null : reader.GetString(1),
                            TienPhat = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)
                        };
                        list.Add(qdp);
                    }
                }
            }

            return list;
        }

        public List<QDP> GetAllQDP()
        {
            var list = new List<QDP>();
            string query = "SELECT MaQDP, LyDo, TienPhat FROM QDP";

            using (var con = dbConnection.GetConnection())
            using (var cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var qdp = new QDP
                        {
                            MaQDP = reader.GetInt32(0),
                            LyDo = reader.IsDBNull(1) ? null : reader.GetString(1),
                            TienPhat = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)
                        };
                        list.Add(qdp);
                    }
                }
            }

            return list;
        }

        // Thêm phiếu phạt mới cùng danh sách lỗi vi phạm
        public bool AddPhieuPhat(PhieuPhat phieuPhat, List<int> danhSachMaQDP)
        {
            string queryInsertPhieuPhat = @"
                INSERT INTO PhieuPhat (MaDG, TrangThai)
                OUTPUT INSERTED.MaPhieuPhat
                VALUES (@MaDG, @TrangThai)";

            using (SqlConnection con = dbConnection.GetConnection())
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    // Thêm phiếu phạt
                    using (SqlCommand cmd = new SqlCommand(queryInsertPhieuPhat, con, trans))
                    {
                        cmd.Parameters.AddWithValue("@MaDG", phieuPhat.MaDG);
                        cmd.Parameters.AddWithValue("@TrangThai", phieuPhat.TrangThai ?? "Chưa thanh toán");

                        int maPhieuPhat = (int)cmd.ExecuteScalar();

                        // Thêm chi tiết lỗi vi phạm
                        foreach (int maQDP in danhSachMaQDP)
                        {
                            string queryInsertChiTiet = @"
                                INSERT INTO PhieuPhat_QDP (MaPhieuPhat, MaQDP) 
                                VALUES (@MaPhieuPhat, @MaQDP)";
                            using (SqlCommand cmdChiTiet = new SqlCommand(queryInsertChiTiet, con, trans))
                            {
                                cmdChiTiet.Parameters.AddWithValue("@MaPhieuPhat", maPhieuPhat);
                                cmdChiTiet.Parameters.AddWithValue("@MaQDP", maQDP);
                                cmdChiTiet.ExecuteNonQuery();
                            }
                        }
                    }

                    trans.Commit();
                    return true;
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return false;
                }
            }
        }

        public List<PhieuPhat> SearchPhieuPhat(string keyword)
        {
            var list = new List<PhieuPhat>();
            string query = @"
        SELECT pp.MaPhieuPhat, dg.HoTen, dg.MaDocGia
        FROM PhieuPhat pp
        INNER JOIN DocGia dg ON pp.MaDG = dg.MaDocGia
        WHERE pp.MaPhieuPhat LIKE @Keyword OR dg.HoTen LIKE @Keyword";

            using (SqlConnection con = dbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                // Nếu keyword là số, vẫn convert thành string để LIKE
                cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var phieuPhat = new PhieuPhat
                        {
                            MaPhieuPhat = reader.GetInt32(0),
                            HoTen = reader.IsDBNull(1) ? null : reader.GetString(1),
                            MaDG = reader.GetInt32(2)
                        };

                        phieuPhat.LoiViPhams = GetLoiViPhamByPhieuPhat(phieuPhat.MaPhieuPhat);
                        phieuPhat.TongTien = phieuPhat.LoiViPhams.Sum(l => l.TienPhat);

                        list.Add(phieuPhat);
                    }
                }
            }

            return list;
        }

        // Xóa phiếu phạt và chi tiết vi phạm liên quan
        public bool DeletePhieuPhat(int maPhieuPhat)
        {
            using (SqlConnection con = dbConnection.GetConnection())
            {
                con.Open();
                using (SqlTransaction trans = con.BeginTransaction())
                {
                    try
                    {
                        // Xóa chi tiết vi phạm trước
                        string deleteChiTietQuery = "DELETE FROM PhieuPhat_QDP WHERE MaPhieuPhat = @MaPhieuPhat";
                        using (SqlCommand cmd = new SqlCommand(deleteChiTietQuery, con, trans))
                        {
                            cmd.Parameters.AddWithValue("@MaPhieuPhat", maPhieuPhat);
                            cmd.ExecuteNonQuery();
                        }

                        // Xóa phiếu phạt
                        string deletePhieuPhatQuery = "DELETE FROM PhieuPhat WHERE MaPhieuPhat = @MaPhieuPhat";
                        using (SqlCommand cmd = new SqlCommand(deletePhieuPhatQuery, con, trans))
                        {
                            cmd.Parameters.AddWithValue("@MaPhieuPhat", maPhieuPhat);
                            int affectedRows = cmd.ExecuteNonQuery();

                            if (affectedRows == 0)
                            {
                                trans.Rollback();
                                return false;
                            }
                        }

                        trans.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
        }

        public bool InsertPhieuPhat(PhieuPhat phieuPhat)
        {
            // Giả sử phiếu phạt chỉ lưu MaDG, ngày lập (nếu cần) và tự sinh MaPhieuPhat
            // Sau khi insert phiếu phạt, cần insert các lỗi vi phạm (PhieuPhat_QDP)
            // Giao dịch để đảm bảo atomic

            using (SqlConnection con = dbConnection.GetConnection())
            using (SqlCommand cmd = con.CreateCommand())
            {
                con.Open();
                var transaction = con.BeginTransaction();
                cmd.Transaction = transaction;

                try
                {
                    // Insert Phiếu Phạt, giả sử bảng PhieuPhat có cột MaDG, NgayLap (nếu cần)
                    string insertPhieuPhatQuery = @"
                        INSERT INTO PhieuPhat (MaDG, NgayLap)
                        VALUES (@MaDG, @NgayLap);
                        SELECT SCOPE_IDENTITY();";

                    cmd.CommandText = insertPhieuPhatQuery;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@MaDG", phieuPhat.MaDG);
                    cmd.Parameters.AddWithValue("@NgayLap", DateTime.Now);

                    // Lấy MaPhieuPhat mới tạo
                    int newMaPhieuPhat = Convert.ToInt32(decimal.ToInt32((decimal)cmd.ExecuteScalar()));

                    // Insert các lỗi vi phạm vào bảng liên kết PhieuPhat_QDP
                    foreach (var loiViPham in phieuPhat.LoiViPhams)
                    {
                        cmd.CommandText = @"
                            INSERT INTO PhieuPhat_QDP (MaPhieuPhat, MaQDP)
                            VALUES (@MaPhieuPhat, @MaQDP)";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@MaPhieuPhat", newMaPhieuPhat);
                        cmd.Parameters.AddWithValue("@MaQDP", loiViPham.MaQDP);

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public bool UpdatePhieuPhat(PhieuPhat phieuPhat, List<int> danhSachMaQDP)
        {
            using (SqlConnection con = dbConnection.GetConnection())
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    // 1. Cập nhật thông tin chung (Trạng thái)
                    string updatePhieuPhatQuery = @"
                UPDATE PhieuPhat
                SET TrangThai = @TrangThai
                WHERE MaPhieuPhat = @MaPhieuPhat";

                    using (SqlCommand cmd = new SqlCommand(updatePhieuPhatQuery, con, trans))
                    {
                        cmd.Parameters.AddWithValue("@TrangThai", phieuPhat.TrangThai ?? "Chưa thanh toán");
                        cmd.Parameters.AddWithValue("@MaPhieuPhat", phieuPhat.MaPhieuPhat);
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Xóa các lỗi vi phạm cũ
                    string deleteOldQDPQuery = "DELETE FROM PhieuPhat_QDP WHERE MaPhieuPhat = @MaPhieuPhat";
                    using (SqlCommand cmd = new SqlCommand(deleteOldQDPQuery, con, trans))
                    {
                        cmd.Parameters.AddWithValue("@MaPhieuPhat", phieuPhat.MaPhieuPhat);
                        cmd.ExecuteNonQuery();
                    }

                    // 3. Thêm lại các lỗi vi phạm mới
                    foreach (int maQDP in danhSachMaQDP)
                    {
                        string insertQDPQuery = @"
                    INSERT INTO PhieuPhat_QDP (MaPhieuPhat, MaQDP)
                    VALUES (@MaPhieuPhat, @MaQDP)";
                        using (SqlCommand cmd = new SqlCommand(insertQDPQuery, con, trans))
                        {
                            cmd.Parameters.AddWithValue("@MaPhieuPhat", phieuPhat.MaPhieuPhat);
                            cmd.Parameters.AddWithValue("@MaQDP", maQDP);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    trans.Commit();
                    return true;
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return false;
                }
            }
        }

        public PhieuPhat GetPhieuPhatById(int maPhieuPhat)
        {
            PhieuPhat phieuPhat = null;
            string query = @"
        SELECT pp.MaPhieuPhat, dg.HoTen, dg.MaDocGia, pp.TrangThai
        FROM PhieuPhat pp
        INNER JOIN DocGia dg ON pp.MaDG = dg.MaDocGia
        WHERE pp.MaPhieuPhat = @MaPhieuPhat";

            using (SqlConnection con = dbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@MaPhieuPhat", maPhieuPhat);
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        phieuPhat = new PhieuPhat
                        {
                            MaPhieuPhat = reader.GetInt32(0),
                            HoTen = reader.IsDBNull(1) ? null : reader.GetString(1),
                            MaDG = reader.GetInt32(2),
                            TrangThai = reader.IsDBNull(3) ? null : reader.GetString(3)
                        };
                    }
                }
            }

            if (phieuPhat != null)
            {
                phieuPhat.LoiViPhams = GetLoiViPhamByPhieuPhat(maPhieuPhat);
                phieuPhat.TongTien = phieuPhat.LoiViPhams.Sum(l => l.TienPhat);
            }

            return phieuPhat;
        }

        public List<QDP> GetQDPsByPhieuPhatId(int maPhieuPhat)
        {
            List<QDP> qdpList = new List<QDP>();

            string query = @"
        SELECT qdp.MaQDP, qdp.LyDo, qdp.TienPhat
        FROM PhieuPhat_QDP pq
        INNER JOIN QDP qdp ON pq.MaQDP = qdp.MaQDP
        WHERE pq.MaPhieuPhat = @MaPhieuPhat";

            using (SqlConnection con = dbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@MaPhieuPhat", maPhieuPhat);
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        QDP qdp = new QDP
                        {
                            MaQDP = reader.GetInt32(0),
                            LyDo = reader.IsDBNull(1) ? null : reader.GetString(1),
                            TienPhat = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)
                        };
                        qdpList.Add(qdp);
                    }
                }
            }

            return qdpList;
        }

    }
}
