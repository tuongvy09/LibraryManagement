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
        private TextBox txtMaThe;
        private ComboBox cboDocGia;
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
            LoadComboBoxData();
            LoadTheThuVienData(maThe);
        }

        private void InitializeComponentForm()
        {
            this.Text = "Sửa Thẻ Thư Viện";
            this.Size = new Size(500, 400);
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
                Text = "Sửa Thẻ Thư Viện",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = mainColor,
                Location = new Point(170, 15),
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

            // Độc giả
            lblDocGia = new Label()
            {
                Text = "Độc giả: *",
                Location = new Point(20, 100),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblDocGia);

            cboDocGia = new ComboBox()
            {
                Location = new Point(150, 97),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(cboDocGia);

            // Ngày cấp
            lblNgayCap = new Label()
            {
                Text = "Ngày cấp: *",
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
            this.Controls.Add(dtpNgayCap);

            // Ngày hết hạn
            lblNgayHetHan = new Label()
            {
                Text = "Ngày hết hạn: *",
                Location = new Point(20, 180),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblNgayHetHan);

            dtpNgayHetHan = new DateTimePicker()
            {
                Location = new Point(150, 177),
                Width = 200,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 10)
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

            // Buttons
            btnSave = new Button()
            {
                Text = "Lưu",
                BackColor = mainColor,
                ForeColor = Color.White,
                Location = new Point(150, 280),
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
                Location = new Point(250, 280),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancel);

            // Thêm ghi chú
            Label lblNote = new Label()
            {
                Text = "* Trường bắt buộc",
                Location = new Point(20, 250),
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.Red
            };
            this.Controls.Add(lblNote);
        }

        private void LoadComboBoxData()
        {
            try
            {
                var docGiaList = docGiaDAO.GetAllDocGia().Where(d => d.TrangThai).ToList();

                // Thêm item mặc định
                var defaultItem = new DocGiaDTO { MaDocGia = 0, HoTen = "-- Chọn độc giả --" };
                docGiaList.Insert(0, defaultItem);

                cboDocGia.DataSource = docGiaList;
                cboDocGia.DisplayMember = "HoTen";
                cboDocGia.ValueMember = "MaDocGia";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách độc giả: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTheThuVienData(int maThe)
        {
            try
            {
                theThuVienToEdit = theThuVienDAO.GetTheThuVienById(maThe);
                if (theThuVienToEdit != null)
                {
                    txtMaThe.Text = theThuVienToEdit.MaThe.ToString();
                    cboDocGia.SelectedValue = theThuVienToEdit.MaDG ?? 0;
                    dtpNgayCap.Value = theThuVienToEdit.NgayCap;
                    dtpNgayHetHan.Value = theThuVienToEdit.NgayHetHan;
                    UpdateTrangThaiDisplay();
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

        private void DtpNgayHetHan_ValueChanged(object sender, EventArgs e)
        {
            UpdateTrangThaiDisplay();
        }

        private void UpdateTrangThaiDisplay()
        {
            bool conHieuLuc = DateTime.Now <= dtpNgayHetHan.Value;
            lblTrangThaiValue.Text = conHieuLuc ? "Còn hiệu lực" : "Hết hạn";
            lblTrangThaiValue.ForeColor = conHieuLuc ? Color.Green : Color.Red;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                theThuVienToEdit.MaDG = Convert.ToInt32(cboDocGia.SelectedValue);
                theThuVienToEdit.NgayCap = dtpNgayCap.Value.Date;
                theThuVienToEdit.NgayHetHan = dtpNgayHetHan.Value.Date;

                bool success = theThuVienDAO.UpdateTheThuVien(theThuVienToEdit);

                if (success)
                {
                    MessageBox.Show("Cập nhật thẻ thư viện thành công!",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cập nhật thẻ thư viện thất bại!",
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
            if (cboDocGia.SelectedValue == null || Convert.ToInt32(cboDocGia.SelectedValue) == 0)
            {
                MessageBox.Show("Vui lòng chọn độc giả!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboDocGia.Focus();
                return false;
            }

            // Kiểm tra độc giả đã có thẻ khác chưa (trừ thẻ hiện tại)
            int maDG = Convert.ToInt32(cboDocGia.SelectedValue);
            if (theThuVienDAO.CheckDocGiaHasCard(maDG, theThuVienToEdit.MaThe))
            {
                MessageBox.Show("Độc giả này đã có thẻ thư viện khác!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboDocGia.Focus();
                return false;
            }

            if (dtpNgayHetHan.Value <= dtpNgayCap.Value)
            {
                MessageBox.Show("Ngày hết hạn phải sau ngày cấp!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgayHetHan.Focus();
                return false;
            }

            return true;
        }
    }
}