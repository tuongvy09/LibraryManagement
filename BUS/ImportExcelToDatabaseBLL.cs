//using OfficeOpenXml;
//using System.Data.SqlClient;
//using System.IO;
//using System.Windows.Forms;
//using LibraryManagement.Repositories;

//namespace LibraryManagement.Services
//{
//    public class ExcelImporter
//    {
//        public void ImportExcelToDatabase(string filePath)
//        {
//            ExcelPackage.License.SetNonCommercialPersonal("Library Management App");
//            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
//            {
//                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
//                int rowCount = worksheet.Dimension.Rows;

//                DBConnection db = new DBConnection();
//                using (SqlConnection conn = db.GetConnection())
//                {
//                    conn.Open();

//                    for (int row = 2; row <= rowCount; row++)
//                    {
//                        string tenCuonSach = worksheet.Cells[row, 2].Text.Trim();
//                        string tenDauSach = worksheet.Cells[row, 3].Text.Trim();
//                        string trangThai = worksheet.Cells[row, 4].Text.Trim();
//                        string tenTheLoai = worksheet.Cells[row, 5].Text.Trim();
//                        int quyDinhTuoi = int.TryParse(worksheet.Cells[row, 6].Text.Trim(), out var tuoi) ? tuoi : 0;
//                        string tenNXB = worksheet.Cells[row, 7].Text.Trim();
//                        string tenTacGia = worksheet.Cells[row, 8].Text.Trim();

//                        int maTheLoai = GetOrInsertTheLoai(conn, tenTheLoai, quyDinhTuoi);
//                        int maNXB = GetOrInsertNXB(conn, tenNXB);
//                        int maTacGia = GetOrInsertTacGia(conn, tenTacGia);
//                        int maDauSach = GetOrInsertDauSach(conn, tenDauSach, maTheLoai, maNXB);

//                        InsertCuonSach(conn, maDauSach, tenCuonSach, trangThai);
//                        InsertDauSachTacGia(conn, maDauSach, maTacGia);
//                    }

//                    conn.Close();
//                }

//                MessageBox.Show("Import thành công!");
//            }
//        }

//        private int GetOrInsertTheLoai(SqlConnection conn, string ten, int tuoi)
//        {
//            string checkSql = "SELECT MaTheLoai FROM TheLoai WHERE TenTheLoai = @Ten";
//            string insertSql = "INSERT INTO TheLoai(TenTheLoai, QDSoTuoi) OUTPUT INSERTED.MaTheLoai VALUES(@Ten, @Tuoi)";

//            using (SqlCommand cmd = new SqlCommand(checkSql, conn))
//            {
//                cmd.Parameters.AddWithValue("@Ten", ten);
//                var result = cmd.ExecuteScalar();
//                if (result != null) return (int)result;
//            }

//            using (SqlCommand cmd = new SqlCommand(insertSql, conn))
//            {
//                cmd.Parameters.AddWithValue("@Ten", ten);
//                cmd.Parameters.AddWithValue("@Tuoi", tuoi);
//                return (int)cmd.ExecuteScalar();
//            }
//        }

//        private int GetOrInsertNXB(SqlConnection conn, string ten)
//        {
//            string checkSql = "SELECT MaNXB FROM NXB WHERE TenNSB = @Ten";
//            string insertSql = "INSERT INTO NXB(TenNSB) OUTPUT INSERTED.MaNXB VALUES(@Ten)";

//            using (SqlCommand cmd = new SqlCommand(checkSql, conn))
//            {
//                cmd.Parameters.AddWithValue("@Ten", ten);
//                var result = cmd.ExecuteScalar();
//                if (result != null) return (int)result;
//            }

//            using (SqlCommand cmd = new SqlCommand(insertSql, conn))
//            {
//                cmd.Parameters.AddWithValue("@Ten", ten);
//                return (int)cmd.ExecuteScalar();
//            }
//        }

//        private int GetOrInsertTacGia(SqlConnection conn, string ten)
//        {
//            string checkSql = "SELECT MaTacGia FROM TacGia WHERE TenTG = @Ten";
//            string insertSql = "INSERT INTO TacGia(TenTG) OUTPUT INSERTED.MaTacGia VALUES(@Ten)";

//            using (SqlCommand cmd = new SqlCommand(checkSql, conn))
//            {
//                cmd.Parameters.AddWithValue("@Ten", ten);
//                var result = cmd.ExecuteScalar();
//                if (result != null) return (int)result;
//            }

//            using (SqlCommand cmd = new SqlCommand(insertSql, conn))
//            {
//                cmd.Parameters.AddWithValue("@Ten", ten);
//                return (int)cmd.ExecuteScalar();
//            }
//        }

//        private int GetOrInsertDauSach(SqlConnection conn, string ten, int maTheLoai, int maNXB)
//        {
//            string checkSql = "SELECT MaDauSach FROM DauSach WHERE TenDauSach = @Ten";
//            string insertSql = "INSERT INTO DauSach(TenDauSach, MaTheLoai, MaNXB) OUTPUT INSERTED.MaDauSach VALUES(@Ten, @TheLoai, @NXB)";

//            using (SqlCommand cmd = new SqlCommand(checkSql, conn))
//            {
//                cmd.Parameters.AddWithValue("@Ten", ten);
//                var result = cmd.ExecuteScalar();
//                if (result != null) return (int)result;
//            }

//            using (SqlCommand cmd = new SqlCommand(insertSql, conn))
//            {
//                cmd.Parameters.AddWithValue("@Ten", ten);
//                cmd.Parameters.AddWithValue("@TheLoai", maTheLoai);
//                cmd.Parameters.AddWithValue("@NXB", maNXB);
//                return (int)cmd.ExecuteScalar();
//            }
//        }

//        private void InsertCuonSach(SqlConnection conn, int maDauSach, string ten, string trangThai)
//        {
//            string insertSql = "INSERT INTO CuonSach(MaDauSach, TenCuonSach, TrangThaiSach) VALUES(@Ma, @Ten, @TrangThai)";
//            using (SqlCommand cmd = new SqlCommand(insertSql, conn))
//            {
//                cmd.Parameters.AddWithValue("@Ma", maDauSach);
//                cmd.Parameters.AddWithValue("@Ten", ten);
//                cmd.Parameters.AddWithValue("@TrangThai", trangThai);
//                cmd.ExecuteNonQuery();
//            }
//        }

//        private void InsertDauSachTacGia(SqlConnection conn, int maDauSach, int maTacGia)
//        {
//            string checkSql = "SELECT COUNT(*) FROM DauSach_TacGia WHERE MaDauSach = @DauSach AND MaTacGia = @TacGia";
//            using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
//            {
//                checkCmd.Parameters.AddWithValue("@DauSach", maDauSach);
//                checkCmd.Parameters.AddWithValue("@TacGia", maTacGia);
//                int count = (int)checkCmd.ExecuteScalar();
//                if (count > 0) return;
//            }

//            string insertSql = "INSERT INTO DauSach_TacGia(MaDauSach, MaTacGia) VALUES(@DauSach, @TacGia)";
//            using (SqlCommand cmd = new SqlCommand(insertSql, conn))
//            {
//                cmd.Parameters.AddWithValue("@DauSach", maDauSach);
//                cmd.Parameters.AddWithValue("@TacGia", maTacGia);
//                cmd.ExecuteNonQuery();
//            }
//        }
//    }
//}
