using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Models;
using LibraryManagement.Repositories;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.UI
{
    public partial class FormEditTheThuVien : Form
    {
        private Label lblTitle;
        private Label lblMaThe, lblDocGia, lblNgayCap, lblNgayHetHan, lblTrangThai;
        private TextBox txtMaThe, txtDocGia;
        private DateTimePicker dtpNgayCap, dtpNgayHetHan;
        private Label lblTrangThaiValue;
        private Button btnSave, btnCancel;

        private TheThuVienDAO theThuVienDAO = new TheThuVienDAO();
        private DocGiaDAO docGiaDAO = new DocGiaDAO();
        private TheThuVien theThuVienToEdit;

        public FormEditTheThuVien(int maThe)
        {
            InitializeComponentForm();
            InitializeCustomStyle();
            LoadTheThuVienData(maThe);
        }

        private void InitializeComponentForm()
        {
            this.Text = "Gia hạn Thẻ Thư Viện";
            this.Size = new Size(500, 420);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeCustomStyle()
        {
            Color mainColor = ColorTranslator.FromHtml("#739a4f");
            this.BackColor = Color.White;

            lblTitle = new Label()
            {
                Text = "Gia hạn Thẻ Thư Viện",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = mainColor,
                Location = new Point(150, 15),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Mã thẻ
            lblMaThe = new Label()
            {
                Text = "Mã thẻ:",
                Location = new Point(20, 60),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblMaThe);

            txtMaThe = new TextBox()
            {
                Location = new Point(150, 57),
                Width = 100,
                ReadOnly = true,
                BackColor = Color.LightGray,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtMaThe);

            // Độc giả (readonly)
            lblDocGia = new Label()
            {
                Text = "Độc giả:",
                Location = new Point(20, 100),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblDocGia);

            txtDocGia = new TextBox()
            {
                Location = new Point(150, 97),
                Width = 300,
                ReadOnly = true,
                BackColor = Color.LightGray,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtDocGia);

            // Ngày cấp
            lblNgayCap = new Label()
            {
                Text = "Ngày cấp:",
                Location = new Point(20, 140),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblNgayCap);

            dtpNgayCap = new DateTimePicker()
            {
                Location = new Point(150, 137),
                Width = 200,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 10)
            };
            dtpNgayCap.ValueChanged += DtpNgayCap_ValueChanged;
            this.Controls.Add(dtpNgayCap);

            // Ngày hết hạn
            lblNgayHetHan = new Label()
            {
                Text = "Ngày hết hạn: *",
                Location = new Point(20, 180),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.Red
            };
            this.Controls.Add(lblNgayHetHan);

            dtpNgayHetHan = new DateTimePicker()
            {
                Location = new Point(150, 177),
                Width = 200,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.LightYellow // Highlight field chính
            };
            dtpNgayHetHan.ValueChanged += DtpNgayHetHan_ValueChanged;
            this.Controls.Add(dtpNgayHetHan);

            // Trạng thái
            lblTrangThai = new Label()
            {
                Text = "Trạng thái:",
                Location = new Point(20, 220),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTrangThai);

            lblTrangThaiValue = new Label()
            {
                Text = "Còn hiệu lực",
                Location = new Point(150, 220),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.Green
            };
            this.Controls.Add(lblTrangThaiValue);

            // Thông tin gia hạn
            Label lblGiaHanInfo = new Label()
            {
                Text = "💡 Thay đổi ngày hết hạn để gia hạn thẻ",
                Location = new Point(20, 250),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.Blue
            };
            this.Controls.Add(lblGiaHanInfo);

            // Buttons
            btnSave = new Button()
            {
                Text = "✅ Gia hạn",
                BackColor = mainColor,
                ForeColor = Color.White,
                Location = new Point(150, 300),
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            btnCancel = new Button()
            {
                Text = "❌ Hủy",
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Location = new Point(270, 300),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancel);

            // Thêm ghi chú
            //Label lblNote = new Label()
            //{
            //    Text = "* Trường bắt buộc - Focus vào ngày hết hạn để gia hạn",
            //    Location = new Point(20, 275),
            //    AutoSize = true,
            //    Font = new Font("Segoe UI", 8, FontStyle.Italic),
            //    ForeColor = Color.Red
            //};
            //this.Controls.Add(lblNote);

            // Thêm button tự động gia hạn
            Button btnAutoExtend = new Button()
            {
                Text = "📅 +1 năm",
                BackColor = Color.Orange,
                ForeColor = Color.White,
                Location = new Point(360, 177),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8)
            };
            btnAutoExtend.FlatAppearance.BorderSize = 0;
            btnAutoExtend.Click += BtnAutoExtend_Click;
            this.Controls.Add(btnAutoExtend);
        }

        private void LoadTheThuVienData(int maThe)
        {
            try
            {
                theThuVienToEdit = theThuVienDAO.GetTheThuVienById(maThe);
                if (theThuVienToEdit != null)
                {
                    txtMaThe.Text = theThuVienToEdit.MaThe.ToString();

                    // Load thông tin độc giả - CHỈ HIỂN THỊ TÊN
                    if (theThuVienToEdit.MaDG.HasValue)
                    {
                        var docGia = docGiaDAO.GetDocGiaById(theThuVienToEdit.MaDG.Value);
                        txtDocGia.Text = docGia != null ? docGia.HoTen : "Không tìm thấy thông tin độc giả";
                    }
                    else
                    {
                        txtDocGia.Text = "Chưa gán độc giả";
                    }

                    dtpNgayCap.Value = theThuVienToEdit.NgayCap;
                    dtpNgayHetHan.Value = theThuVienToEdit.NgayHetHan;
                    UpdateTrangThaiDisplay();

                    // Focus vào ngày hết hạn (trường chính cần sửa)
                    dtpNgayHetHan.Focus();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin thẻ thư viện!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin thẻ thư viện: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void DtpNgayCap_ValueChanged(object sender, EventArgs e)
        {
            UpdateTrangThaiDisplay();

            // Tự động điều chỉnh ngày hết hạn nếu nhỏ hơn ngày cấp
            if (dtpNgayHetHan.Value <= dtpNgayCap.Value)
            {
                dtpNgayHetHan.Value = dtpNgayCap.Value.AddYears(1);
            }
        }

        private void DtpNgayHetHan_ValueChanged(object sender, EventArgs e)
        {
            UpdateTrangThaiDisplay();
        }

        private void UpdateTrangThaiDisplay()
        {
            bool conHieuLuc = DateTime.Now <= dtpNgayHetHan.Value;
            lblTrangThaiValue.Text = conHieuLuc ? "Còn hiệu lực" : "Hết hạn";
            lblTrangThaiValue.ForeColor = conHieuLuc ? Color.Green : Color.Red;

            // Hiển thị số ngày còn lại hoặc đã hết hạn
            TimeSpan timeSpan = dtpNgayHetHan.Value - DateTime.Now;
            if (conHieuLuc)
            {
                lblTrangThaiValue.Text += $" ({timeSpan.Days} ngày)";
            }
            else
            {
                lblTrangThaiValue.Text += $" ({Math.Abs(timeSpan.Days)} ngày)";
            }
        }

        private void BtnAutoExtend_Click(object sender, EventArgs e)
        {
            // Tự động gia hạn thêm 1 năm từ ngày hiện tại hoặc ngày hết hạn cũ
            DateTime newExpiryDate = dtpNgayHetHan.Value > DateTime.Now ?
                dtpNgayHetHan.Value.AddYears(1) :
                DateTime.Now.AddYears(1);

            dtpNgayHetHan.Value = newExpiryDate;

            MessageBox.Show($"Đã tự động gia hạn đến {newExpiryDate:dd/MM/yyyy}", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                DateTime oldExpiryDate = theThuVienToEdit.NgayHetHan;

                theThuVienToEdit.NgayCap = dtpNgayCap.Value.Date;
                theThuVienToEdit.NgayHetHan = dtpNgayHetHan.Value.Date;

                bool success = theThuVienDAO.UpdateTheThuVien(theThuVienToEdit);

                if (success)
                {
                    string message = "Gia hạn thẻ thư viện thành công!\n\n";
                    message += $"Ngày hết hạn cũ: {oldExpiryDate:dd/MM/yyyy}\n";
                    message += $"Ngày hết hạn mới: {theThuVienToEdit.NgayHetHan:dd/MM/yyyy}";

                    MessageBox.Show(message, "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Gia hạn thẻ thư viện thất bại!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (dtpNgayHetHan.Value <= dtpNgayCap.Value)
            {
                MessageBox.Show("Ngày hết hạn phải sau ngày cấp!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgayHetHan.Focus();
                return false;
            }

            // Cảnh báo nếu gia hạn quá xa trong tương lai
            if (dtpNgayHetHan.Value > DateTime.Now.AddYears(5))
            {
                var result = MessageBox.Show(
                    "Ngày hết hạn xa quá trong tương lai (>5 năm).\nBạn có chắc chắn muốn tiếp tục?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    dtpNgayHetHan.Focus();
                    return false;
                }
            }

            return true;
        }

        // Thêm keyboard shortcuts
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    BtnSave_Click(null, null);
                    return true;
                case Keys.Escape:
                    this.Close();
                    return true;
                case Keys.F1: // Quick extend 1 year
                    BtnAutoExtend_Click(null, null);
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // Auto focus vào ngày hết hạn khi form load xong
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            dtpNgayHetHan.Focus();
        }
    }
}
