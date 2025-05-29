using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using LibraryManagement.Models;
using LibraryManagement.Utilities;

namespace LibraryManagement.UI
{
    public partial class FormQRDisplay : Form
    {
        private Bitmap qrImage;
        private TheThuVien cardInfo;

        private PictureBox picQR;
        private Label lblInfo;
        private Button btnSave;
        private Button btnPrint;
        private Button btnClose;

        public FormQRDisplay(Bitmap qr, TheThuVien card)
        {
            qrImage = qr;
            cardInfo = card;
            InitializeCustomComponents(); // CHANGED: Đổi tên method
            DisplayQRCode();
        }

        // CHANGED: Đổi tên từ InitializeComponent() thành InitializeCustomComponents()
        private void InitializeCustomComponents()
        {
            this.Text = "QR Code - Thẻ Thư Viện";
            this.Size = new Size(480, 520);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            Color mainColor = ColorTranslator.FromHtml("#739a4f");

            // Title
            Label lblTitle = new Label()
            {
                Text = "QR CODE THẺ THƯ VIỆN",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = mainColor,
                Location = new Point(120, 15),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // QR Code PictureBox
            picQR = new PictureBox()
            {
                Location = new Point(90, 50),
                Size = new Size(300, 300),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            this.Controls.Add(picQR);

            // Info label
            lblInfo = new Label()
            {
                Location = new Point(50, 360),
                Size = new Size(380, 80),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Black
            };
            this.Controls.Add(lblInfo);

            // Save button
            btnSave = new Button()
            {
                Text = "💾 Lưu QR",
                Location = new Point(60, 450),
                Size = new Size(100, 35),
                BackColor = mainColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            // Print button  
            btnPrint = new Button()
            {
                Text = "🖨️ In thẻ",
                Location = new Point(170, 450),
                Size = new Size(100, 35),
                BackColor = ColorTranslator.FromHtml("#2196F3"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.Click += BtnPrint_Click;
            this.Controls.Add(btnPrint);

            // Copy QR Data button
            Button btnCopy = new Button()
            {
                Text = "📋 Copy",
                Location = new Point(280, 450),
                Size = new Size(80, 35),
                BackColor = ColorTranslator.FromHtml("#FF9800"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            btnCopy.FlatAppearance.BorderSize = 0;
            btnCopy.Click += BtnCopy_Click;
            this.Controls.Add(btnCopy);

            // Close button
            btnClose = new Button()
            {
                Text = "❌ Đóng",
                Location = new Point(370, 450),
                Size = new Size(80, 35),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }

        private void DisplayQRCode()
        {
            picQR.Image = qrImage;
            lblInfo.Text =
                $"🆔 Mã thẻ: {cardInfo.MaThe}\n" +
                $"👤 Độc giả: {cardInfo.TenDocGia}\n" +
                $"📅 Ngày cấp: {cardInfo.NgayCap:dd/MM/yyyy}\n" +
                $"⏰ Hết hạn: {cardInfo.NgayHetHan:dd/MM/yyyy}";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "PNG Images|*.png|JPEG Images|*.jpg|All Files|*.*";
                    sfd.FileName = $"QR_The_{cardInfo.MaThe}_{cardInfo.TenDocGia}";
                    sfd.DefaultExt = "png";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        qrImage.Save(sfd.FileName);
                        MessageBox.Show("✅ Đã lưu QR Code thành công!", "Thành công",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi khi lưu file: {ex.Message}", "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                string qrData = QRCodeManager.CreateLibraryCardQR(cardInfo);
                Clipboard.SetText(qrData);
                MessageBox.Show("📋 Đã copy dữ liệu QR vào clipboard!", "Thông báo",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi khi copy: {ex.Message}", "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintLibraryCard();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi khi in: {ex.Message}", "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintLibraryCard()
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += (s, e) =>
            {
                Graphics g = e.Graphics;
                Font titleFont = new Font("Arial", 16, FontStyle.Bold);
                Font normalFont = new Font("Arial", 12);
                Font smallFont = new Font("Arial", 10);

                // Draw library card template
                Rectangle cardRect = new Rectangle(50, 50, 350, 220);
                g.DrawRectangle(Pens.Black, cardRect);

                // Header
                g.DrawString("THƯ VIỆN ABC", titleFont, Brushes.Black, new PointF(60, 70));
                g.DrawString("LIBRARY CARD", smallFont, Brushes.Gray, new PointF(60, 95));

                // Card info
                g.DrawString($"Mã thẻ: {cardInfo.MaThe}", normalFont, Brushes.Black, new PointF(60, 120));
                g.DrawString($"Độc giả: {cardInfo.TenDocGia}", normalFont, Brushes.Black, new PointF(60, 145));
                g.DrawString($"Ngày cấp: {cardInfo.NgayCap:dd/MM/yyyy}", normalFont, Brushes.Black, new PointF(60, 170));
                g.DrawString($"Hết hạn: {cardInfo.NgayHetHan:dd/MM/yyyy}", normalFont, Brushes.Black, new PointF(60, 195));

                // QR Code (smaller for card)
                var cardQR = QRCodeManager.GenerateQRCode(QRCodeManager.CreateLibraryCardQR(cardInfo), 3);
                g.DrawImage(cardQR, new Rectangle(280, 120, 100, 100));

                // Footer
                g.DrawString("Quét QR để kiểm tra thẻ", smallFont, Brushes.Gray, new PointF(60, 240));
            };

            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = pd;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
                MessageBox.Show("✅ Đã gửi lệnh in thẻ!", "Thành công",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
