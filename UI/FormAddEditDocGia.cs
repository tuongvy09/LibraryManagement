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
    public partial class FormAddEditDocGia : Form
    {
        private Label lblTitle;
        private Label lblHoTen, lblTuoi, lblSoDT, lblCCCD, lblEmail, lblDiaChi;
        private Label lblGioiTinh, lblLoaiDocGia;
        private TextBox txtHoTen, txtTuoi, txtSoDT, txtCCCD, txtEmail, txtDiaChi;
        private ComboBox cboGioiTinh, cboLoaiDocGia;
        private CheckBox chkTrangThai;
        private Button btnSave, btnCancel;

        private DocGiaDAO docGiaDAO = new DocGiaDAO();
        private LoaiDocGiaDAO loaiDocGiaDAO = new LoaiDocGiaDAO();
        private DocGiaDTO docGiaToEdit;
        private bool isEditMode = false;

        public FormAddEditDocGia()
        {
            InitializeComponentForm();
            InitializeCustomStyle();
            LoadComboBoxData();
            isEditMode = false;
        }

        public FormAddEditDocGia(DocGiaDTO docGia)
        {
            InitializeComponentForm();
            InitializeCustomStyle();
            LoadComboBoxData();
            isEditMode = true;
            docGiaToEdit = docGia;
            LoadDataToForm();
        }

        private void InitializeComponentForm()
        {
            this.Text = "Thêm/Sửa Độc giả";
            this.Size = new Size(500, 550);
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
                Text = isEditMode ? "Sửa Độc giả" : "Thêm Độc giả",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = mainColor,
                Location = new Point(180, 15),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Họ tên
            lblHoTen = new Label()
            {
                Text = "Họ tên: *",
                Location = new Point(20, 60),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblHoTen);

            txtHoTen = new TextBox()
            {
                Location = new Point(150, 57),
                Width = 300,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtHoTen);

            // Tuổi
            lblTuoi = new Label()
            {
                Text = "Tuổi: *",
                Location = new Point(20, 100),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTuoi);

            txtTuoi = new TextBox()
            {
                Location = new Point(150, 97),
                Width = 100,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtTuoi);

            // Giới tính
            lblGioiTinh = new Label()
            {
                Text = "Giới tính:",
                Location = new Point(270, 100),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblGioiTinh);

            cboGioiTinh = new ComboBox()
            {
                Location = new Point(350, 97),
                Width = 100,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cboGioiTinh.Items.AddRange(new object[] { "M", "F" });
            this.Controls.Add(cboGioiTinh);

            // Số điện thoại
            lblSoDT = new Label()
            {
                Text = "Số điện thoại: *",
                Location = new Point(20, 140),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblSoDT);

            txtSoDT = new TextBox()
            {
                Location = new Point(150, 137),
                Width = 150,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtSoDT);

            // CCCD
            lblCCCD = new Label()
            {
                Text = "CCCD:",
                Location = new Point(20, 180),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblCCCD);

            txtCCCD = new TextBox()
            {
                Location = new Point(150, 177),
                Width = 200,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtCCCD);

            // Email
            lblEmail = new Label()
            {
                Text = "Email:",
                Location = new Point(20, 220),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblEmail);

            txtEmail = new TextBox()
            {
                Location = new Point(150, 217),
                Width = 300,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtEmail);

            // Địa chỉ
            lblDiaChi = new Label()
            {
                Text = "Địa chỉ:",
                Location = new Point(20, 260),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblDiaChi);

            txtDiaChi = new TextBox()
            {
                Location = new Point(150, 257),
                Width = 300,
                Height = 60,
                Multiline = true,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtDiaChi);

            // Loại độc giả
            lblLoaiDocGia = new Label()
            {
                Text = "Loại độc giả:",
                Location = new Point(20, 340),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblLoaiDocGia);

            cboLoaiDocGia = new ComboBox()
            {
                Location = new Point(150, 337),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(cboLoaiDocGia);

            // Trạng thái
            chkTrangThai = new CheckBox()
            {
                Text = "Hoạt động",
                Location = new Point(150, 380),
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
                Location = new Point(150, 430),
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
                Location = new Point(250, 430),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancel);
        }

        private void LoadComboBoxData()
        {
            var loaiDocGiaList = loaiDocGiaDAO.GetAllLoaiDocGia();

            cboLoaiDocGia.DataSource = loaiDocGiaList;
            cboLoaiDocGia.DisplayMember = "TenLoaiDG";
            cboLoaiDocGia.ValueMember = "MaLoaiDG";
        }

        private void LoadDataToForm()
        {
            if (docGiaToEdit != null)
            {
                txtHoTen.Text = docGiaToEdit.HoTen;
                txtTuoi.Text = docGiaToEdit.Tuoi.ToString();
                txtSoDT.Text = docGiaToEdit.SoDT;
                txtCCCD.Text = docGiaToEdit.CCCD;
                txtEmail.Text = docGiaToEdit.Email;
                txtDiaChi.Text = docGiaToEdit.DiaChi;
                cboGioiTinh.Text = docGiaToEdit.GioiTinh;
                cboLoaiDocGia.SelectedValue = docGiaToEdit.MaLoaiDG;
                chkTrangThai.Checked = docGiaToEdit.TrangThai;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                var docGia = new DocGiaDTO()
                {
                    HoTen = txtHoTen.Text.Trim(),
                    Tuoi = Convert.ToInt32(txtTuoi.Text),
                    SoDT = txtSoDT.Text.Trim(),
                    CCCD = txtCCCD.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    DiaChi = txtDiaChi.Text.Trim(),
                    GioiTinh = cboGioiTinh.Text,
                    MaLoaiDG = cboLoaiDocGia.SelectedValue != null ? (int?)cboLoaiDocGia.SelectedValue : null,
                    TrangThai = chkTrangThai.Checked
                };

                bool success = false;
                if (isEditMode)
                {
                    docGia.MaDocGia = docGiaToEdit.MaDocGia;
                    success = docGiaDAO.UpdateDocGia(docGia);
                }
                else
                {
                    success = docGiaDAO.InsertDocGia(docGia);
                }

                if (success)
                {
                    MessageBox.Show(isEditMode ? "Cập nhật độc giả thành công!" : "Thêm độc giả thành công!",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(isEditMode ? "Cập nhật độc giả thất bại!" : "Thêm độc giả thất bại!",
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
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTuoi.Text))
            {
                MessageBox.Show("Vui lòng nhập tuổi!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTuoi.Focus();
                return false;
            }

            if (!int.TryParse(txtTuoi.Text, out int tuoi) || tuoi <= 0)
            {
                MessageBox.Show("Tuổi phải là số nguyên dương!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTuoi.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSoDT.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDT.Focus();
                return false;
            }

            return true;
        }
    }
}

