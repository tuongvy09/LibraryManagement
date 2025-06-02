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
    public partial class FormAddEditThuThu : Form
    {
        private Label lblTitle;
        private Label lblTenThuThu, lblEmail, lblSoDienThoai, lblDiaChi;
        private Label lblNgayBatDauLam, lblNgaySinh, lblGioiTinh;
        private TextBox txtTenThuThu, txtEmail, txtSoDienThoai, txtDiaChi;
        private DateTimePicker dtpNgayBatDauLam, dtpNgaySinh;
        private ComboBox cboGioiTinh;

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormAddEditThuThu
            // 
            this.ClientSize = new System.Drawing.Size(821, 383);
            this.Name = "FormAddEditThuThu";
            this.ResumeLayout(false);
        }

        private CheckBox chkTrangThai;
        private Button btnSave, btnCancel;

        private ThuThuDAO thuThuDAO = new ThuThuDAO();
        private ThuThu thuThuToEdit;
        private bool isEditMode = false;

        public FormAddEditThuThu()
        {
            InitializeComponentForm();
            InitializeCustomStyle();
            isEditMode = false;
        }

        public FormAddEditThuThu(ThuThu thuThu)
        {
            InitializeComponentForm();
            InitializeCustomStyle();
            isEditMode = true;
            thuThuToEdit = thuThu;
            LoadDataToForm();
        }

        private void InitializeComponentForm()
        {
            this.Text = "Thêm/Sửa Thủ thư";
            this.Size = new Size(500, 450); // Giảm chiều cao vì bớt 2 trường
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
                Text = isEditMode ? "Sửa Thủ thư" : "Thêm Thủ thư",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = mainColor,
                Location = new Point(180, 15),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Tên thủ thư
            lblTenThuThu = new Label()
            {
                Text = "Tên thủ thư: *",
                Location = new Point(20, 60),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTenThuThu);

            txtTenThuThu = new TextBox()
            {
                Location = new Point(150, 57),
                Width = 300,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtTenThuThu);

            // Email
            lblEmail = new Label()
            {
                Text = "Email:",
                Location = new Point(20, 100),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblEmail);

            txtEmail = new TextBox()
            {
                Location = new Point(150, 97),
                Width = 300,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtEmail);

            // Số điện thoại
            lblSoDienThoai = new Label()
            {
                Text = "Số điện thoại:",
                Location = new Point(20, 140),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblSoDienThoai);

            txtSoDienThoai = new TextBox()
            {
                Location = new Point(150, 137),
                Width = 300,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtSoDienThoai);

            // Địa chỉ
            lblDiaChi = new Label()
            {
                Text = "Địa chỉ:",
                Location = new Point(20, 180),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblDiaChi);

            txtDiaChi = new TextBox()
            {
                Location = new Point(150, 177),
                Width = 300,
                Height = 60,
                Multiline = true,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtDiaChi);

            // Ngày sinh
            lblNgaySinh = new Label()
            {
                Text = "Ngày sinh:",
                Location = new Point(20, 260),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblNgaySinh);

            dtpNgaySinh = new DateTimePicker()
            {
                Location = new Point(150, 257),
                Width = 150,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(dtpNgaySinh);

            // Giới tính
            lblGioiTinh = new Label()
            {
                Text = "Giới tính:",
                Location = new Point(320, 260),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblGioiTinh);

            cboGioiTinh = new ComboBox()
            {
                Location = new Point(390, 257),
                Width = 60,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ" });
            this.Controls.Add(cboGioiTinh);

            // Ngày bắt đầu làm
            lblNgayBatDauLam = new Label()
            {
                Text = "Ngày bắt đầu làm:",
                Location = new Point(20, 300),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblNgayBatDauLam);

            dtpNgayBatDauLam = new DateTimePicker()
            {
                Location = new Point(150, 297),
                Width = 150,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(dtpNgayBatDauLam);

            // Trạng thái
            chkTrangThai = new CheckBox()
            {
                Text = "Hoạt động",
                Location = new Point(150, 340),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor,
                Checked = true
            };
            this.Controls.Add(chkTrangThai);

            // Buttons
            btnSave = new Button()
            {
                Text = "Lưu",
                BackColor = mainColor,
                ForeColor = Color.White,
                Location = new Point(150, 380),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            btnCancel = new Button()
            {
                Text = "Hủy",
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Location = new Point(250, 380),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancel);
        }

        private void LoadDataToForm()
        {
            if (thuThuToEdit != null)
            {
                txtTenThuThu.Text = thuThuToEdit.TenThuThu;
                txtEmail.Text = thuThuToEdit.Email;
                txtSoDienThoai.Text = thuThuToEdit.SoDienThoai;
                txtDiaChi.Text = thuThuToEdit.DiaChi;
                dtpNgaySinh.Value = thuThuToEdit.NgaySinh;
                dtpNgayBatDauLam.Value = thuThuToEdit.NgayBatDauLam;
                cboGioiTinh.Text = thuThuToEdit.GioiTinh;
                chkTrangThai.Checked = thuThuToEdit.TrangThai;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                var thuThu = new ThuThu()
                {
                    TenThuThu = txtTenThuThu.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    SoDienThoai = txtSoDienThoai.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim(),
                    NgaySinh = dtpNgaySinh.Value,
                    NgayBatDauLam = dtpNgayBatDauLam.Value,
                    GioiTinh = cboGioiTinh.Text,
                    TrangThai = chkTrangThai.Checked
                };

                bool success = false;
                if (isEditMode)
                {
                    thuThu.MaThuThu = thuThuToEdit.MaThuThu;
                    success = thuThuDAO.UpdateThuThu(thuThu);
                }
                else
                {
                    success = thuThuDAO.InsertThuThu(thuThu);
                }

                if (success)
                {
                    MessageBox.Show(isEditMode ? "Cập nhật thủ thư thành công!" : "Thêm thủ thư thành công!",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(isEditMode ? "Cập nhật thủ thư thất bại!" : "Thêm thủ thư thất bại!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenThuThu.Text))
            {
                MessageBox.Show("Vui lòng nhập tên thủ thư!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenThuThu.Focus();
                return false;
            }

            // Validation cho email nếu có nhập
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                if (!IsValidEmail(txtEmail.Text))
                {
                    MessageBox.Show("Email không hợp lệ!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return false;
                }
            }

            // Validation cho số điện thoại nếu có nhập
            if (!string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                if (!IsValidPhoneNumber(txtSoDienThoai.Text))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoDienThoai.Focus();
                    return false;
                }
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Kiểm tra số điện thoại Việt Nam (10-11 số, bắt đầu bằng 0)
            return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^0\d{9,10}$");
        }
    }
}
