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
    public partial class FormEditBienLai : Form
    {
        private Label lblTitle;
        private Label lblMaBienLai, lblDocGia, lblNgayTraTT, lblHinhThucTT, lblTienTra;
        private TextBox txtMaBienLai, txtTienTra;
        private ComboBox cboDocGia, cboHinhThucTT;
        private DateTimePicker dtpNgayTraTT;
        private Button btnSave, btnCancel;

        private BienLaiDAO bienLaiDAO = new BienLaiDAO();
        private DocGiaDAO docGiaDAO = new DocGiaDAO();
        private BienLai bienLaiToEdit;

        public FormEditBienLai(int maBienLai)
        {
            InitializeComponentForm();
            InitializeCustomStyle();
            LoadComboBoxData();
            LoadBienLaiData(maBienLai);
        }

        private void InitializeComponentForm()
        {
            this.Text = "Sửa Biên Lai";
            this.Size = new Size(500, 450);
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
                Text = "Sửa Biên Lai",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = mainColor,
                Location = new Point(190, 15),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Mã biên lai
            lblMaBienLai = new Label()
            {
                Text = "Mã biên lai:",
                Location = new Point(20, 60),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblMaBienLai);

            txtMaBienLai = new TextBox()
            {
                Location = new Point(150, 57),
                Width = 100,
                ReadOnly = true,
                BackColor = Color.LightGray,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtMaBienLai);

            // Độc giả
            lblDocGia = new Label()
            {
                Text = "Độc giả:",
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

            // Ngày thanh toán
            lblNgayTraTT = new Label()
            {
                Text = "Ngày thanh toán: *",
                Location = new Point(20, 140),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblNgayTraTT);

            dtpNgayTraTT = new DateTimePicker()
            {
                Location = new Point(150, 137),
                Width = 200,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(dtpNgayTraTT);

            // Hình thức thanh toán
            lblHinhThucTT = new Label()
            {
                Text = "Hình thức TT: *",
                Location = new Point(20, 180),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblHinhThucTT);

            cboHinhThucTT = new ComboBox()
            {
                Location = new Point(150, 177),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(cboHinhThucTT);

            // Tiền trả
            lblTienTra = new Label()
            {
                Text = "Tiền trả: *",
                Location = new Point(20, 220),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTienTra);

            txtTienTra = new TextBox()
            {
                Location = new Point(150, 217),
                Width = 200,
                Font = new Font("Segoe UI", 10)
            };
            txtTienTra.KeyPress += TxtTienTra_KeyPress;
            this.Controls.Add(txtTienTra);

            // Buttons
            btnSave = new Button()
            {
                Text = "Lưu",
                BackColor = mainColor,
                ForeColor = Color.White,
                Location = new Point(150, 320),
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
                Location = new Point(250, 320),
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
                Location = new Point(20, 270),
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
                // Load độc giả
                var docGiaList = docGiaDAO.GetAllDocGia().Where(d => d.TrangThai).ToList();
                var defaultDocGia = new DocGiaDTO { MaDocGia = 0, HoTen = "-- Chọn độc giả --" };
                docGiaList.Insert(0, defaultDocGia);

                cboDocGia.DataSource = docGiaList;
                cboDocGia.DisplayMember = "HoTen";
                cboDocGia.ValueMember = "MaDocGia";

                // Load hình thức thanh toán
                var hinhThucList = bienLaiDAO.GetHinhThucThanhToan();
                cboHinhThucTT.DataSource = hinhThucList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBienLaiData(int maBienLai)
        {
            try
            {
                bienLaiToEdit = bienLaiDAO.GetBienLaiById(maBienLai);
                if (bienLaiToEdit != null)
                {
                    txtMaBienLai.Text = bienLaiToEdit.MaBienLai.ToString();
                    cboDocGia.SelectedValue = bienLaiToEdit.MaDocGia ?? 0;
                    dtpNgayTraTT.Value = bienLaiToEdit.NgayTraTT;
                    cboHinhThucTT.SelectedItem = bienLaiToEdit.HinhThucThanhToan;
                    txtTienTra.Text = bienLaiToEdit.TienTra.ToString();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin biên lai!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin biên lai: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void TxtTienTra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                bienLaiToEdit.MaDocGia = Convert.ToInt32(cboDocGia.SelectedValue) == 0 ? null : (int?)Convert.ToInt32(cboDocGia.SelectedValue);
                bienLaiToEdit.NgayTraTT = dtpNgayTraTT.Value.Date;
                bienLaiToEdit.HinhThucThanhToan = cboHinhThucTT.SelectedItem.ToString();
                bienLaiToEdit.TienTra = Convert.ToDecimal(txtTienTra.Text);

                bool success = bienLaiDAO.UpdateBienLai(bienLaiToEdit);

                if (success)
                {
                    MessageBox.Show("Cập nhật biên lai thành công!",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cập nhật biên lai thất bại!",
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
            if (cboHinhThucTT.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn hình thức thanh toán!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboHinhThucTT.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTienTra.Text) || !decimal.TryParse(txtTienTra.Text, out decimal tienTra) || tienTra <= 0)
            {
                MessageBox.Show("Vui lòng nhập số tiền hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTienTra.Focus();
                return false;
            }

            return true;
        }
    }
}
