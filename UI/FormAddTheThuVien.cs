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
    public partial class FormAddTheThuVien : Form
    {
        private Label lblTitle;
        private Label lblDocGia, lblNgayCap, lblNgayHetHan;
        private ComboBox cboDocGia;
        private DateTimePicker dtpNgayCap, dtpNgayHetHan;
        private Button btnSave, btnCancel;

        private TheThuVienDAO theThuVienDAO = new TheThuVienDAO();
        private DocGiaDAO docGiaDAO = new DocGiaDAO();

        public FormAddTheThuVien()
        {
            InitializeComponentForm();
            InitializeCustomStyle();
            LoadComboBoxData();
            SetDefaultValues();
        }

        private void InitializeComponentForm()
        {
            this.Text = "Thêm Thẻ Thư Viện";
            this.Size = new Size(500, 350);
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
                Text = "Thêm Thẻ Thư Viện",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = mainColor,
                Location = new Point(150, 15),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Độc giả
            lblDocGia = new Label()
            {
                Text = "Độc giả: *",
                Location = new Point(20, 60),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblDocGia);

            cboDocGia = new ComboBox()
            {
                Location = new Point(150, 57),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(cboDocGia);

            // Ngày cấp
            lblNgayCap = new Label()
            {
                Text = "Ngày cấp: *",
                Location = new Point(20, 100),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblNgayCap);

            dtpNgayCap = new DateTimePicker()
            {
                Location = new Point(150, 97),
                Width = 200,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(dtpNgayCap);

            // Ngày hết hạn
            lblNgayHetHan = new Label()
            {
                Text = "Ngày hết hạn: *",
                Location = new Point(20, 140),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblNgayHetHan);

            dtpNgayHetHan = new DateTimePicker()
            {
                Location = new Point(150, 137),
                Width = 200,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(dtpNgayHetHan);

            // Buttons
            btnSave = new Button()
            {
                Text = "Lưu",
                BackColor = mainColor,
                ForeColor = Color.White,
                Location = new Point(150, 220),
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
                Location = new Point(250, 220),
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
                Location = new Point(20, 180),
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
                // Lấy danh sách độc giả chưa có thẻ thư viện
                var docGiaList = docGiaDAO.GetDocGiaChuaCoThe();

                // Thêm item mặc định
                var defaultItem = new DocGiaDTO { MaDocGia = 0, HoTen = "-- Chọn độc giả --" };
                docGiaList.Insert(0, defaultItem);

                cboDocGia.DataSource = docGiaList;
                cboDocGia.DisplayMember = "HoTen";
                cboDocGia.ValueMember = "MaDocGia";
                cboDocGia.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách độc giả chưa có thẻ: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetDefaultValues()
        {
            dtpNgayCap.Value = DateTime.Now;
            dtpNgayHetHan.Value = DateTime.Now.AddYears(1); // Mặc định thẻ có hiệu lực 1 năm

            // Event để tự động cập nhật ngày hết hạn khi thay đổi ngày cấp
            dtpNgayCap.ValueChanged += (s, e) =>
            {
                if (dtpNgayHetHan.Value <= dtpNgayCap.Value)
                {
                    dtpNgayHetHan.Value = dtpNgayCap.Value.AddYears(1);
                }
            };
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                var theThuVien = new TheThuVien()
                {
                    MaDG = Convert.ToInt32(cboDocGia.SelectedValue),
                    NgayCap = dtpNgayCap.Value.Date,
                    NgayHetHan = dtpNgayHetHan.Value.Date
                };

                bool success = theThuVienDAO.InsertTheThuVien(theThuVien);

                if (success)
                {
                    MessageBox.Show("Thêm thẻ thư viện thành công!",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thêm thẻ thư viện thất bại!",
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

            // Kiểm tra độc giả đã có thẻ chưa
            int maDG = Convert.ToInt32(cboDocGia.SelectedValue);
            if (theThuVienDAO.CheckDocGiaHasCard(maDG))
            {
                MessageBox.Show("Độc giả này đã có thẻ thư viện!", "Thông báo",
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

            if (dtpNgayCap.Value > DateTime.Now)
            {
                MessageBox.Show("Ngày cấp không thể trong tương lai!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgayCap.Focus();
                return false;
            }

            return true;
        }
    }
}
