using QRCoder;
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using LibraryManagement.Models;

namespace LibraryManagement.Utilities
{
    public static class QRCodeManager
    {
        public static Bitmap GenerateQRCode(string text, int pixelsPerModule = 20)
        {
            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(pixelsPerModule);
                return qrCodeImage;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo QR Code: {ex.Message}");
            }
        }

        public static string CreateLibraryCardQR(TheThuVien theThuVien)
        {
            try
            {
                string qrContent =
                    $"=== THẺ THƯ VIỆN ===" + "\n" +
                    $"🆔 Mã thẻ: {theThuVien.MaThe}" + "\n" +
                    $"👤 Độc giả: {theThuVien.TenDocGia}" + "\n" +
                    $"📞 SĐT: {theThuVien.SoDT}" + "\n" +
                    $"📅 Ngày cấp: {theThuVien.NgayCap:dd/MM/yyyy}" + "\n" +
                    $"⏰ Hết hạn: {theThuVien.NgayHetHan:dd/MM/yyyy}" + "\n" +
                    $"📍 Trạng thái: {(theThuVien.TrangThaiThe ? "Còn hiệu lực" : "Hết hạn")}" + "\n" +
                    $"🏛️ Thư viện A" + "\n" +
                    $"📱 Quét lúc: {DateTime.Now:dd/MM/yyyy HH:mm}";

                return qrContent;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo dữ liệu QR: {ex.Message}");
            }
        }

        // ALTERNATIVE: Tạo QR với URL redirect (Nâng cao hơn)
        public static string CreateLibraryCardQRWithURL(TheThuVien theThuVien)
        {
            try
            {
                // Tạo URL với parameters
                string baseUrl = "https://thuvien-abc.com/card"; // Thay bằng domain thật
                string checksum = CalculateChecksum($"{theThuVien.MaThe}|{theThuVien.MaDG}");

                string url = $"{baseUrl}?id={theThuVien.MaThe}&token={checksum}";
                return url;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo URL QR: {ex.Message}");
            }
        }

        // Giữ nguyên format cũ cho validation
        public static string CreateLibraryCardQRData(TheThuVien theThuVien)
        {
            try
            {
                string baseData = $"{theThuVien.MaThe}|{theThuVien.MaDG}|{theThuVien.NgayHetHan:yyyyMMdd}";
                string checksum = CalculateChecksum(baseData);

                return $"LIBCARD|{baseData}|{checksum}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo dữ liệu QR: {ex.Message}");
            }
        }

        private static string CalculateChecksum(string data)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hash).Substring(0, 8);
            }
        }

        public static bool ValidateQRCode(string qrData)
        {
            try
            {
                string[] parts = qrData.Split('|');
                if (parts.Length != 5 || parts[0] != "LIBCARD")
                    return false;

                string baseData = $"{parts[1]}|{parts[2]}|{parts[3]}";
                string expectedChecksum = CalculateChecksum(baseData);

                return parts[4] == expectedChecksum;
            }
            catch
            {
                return false;
            }
        }
    }
}
