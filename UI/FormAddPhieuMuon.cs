using LibraryManagement.Models;
using LibraryManagement.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryManagement.UI
{
    public partial class FormAddPhieuMuon : Form
    {
        private Label lblTitle, lblDocGia, lblThuThu, lblNgayMuon, lblHanTra;
        private ComboBox cboDocGia, cboThuThu;
        private DateTimePicker dtpNgayMuon, dtpHanTra;
        private Button btnSave, btnCancel;

        private DocGiaDAO docGiaDAO = new DocGiaDAO();
        private ThuThuDAO thuThuDAO = new ThuThuDAO();
        private PhieuMuonDAO phieuMuonDAO = new PhieuMuonDAO();

        public FormAddPhieuMuon()
        {
            InitializeComponent();
            InitializeCustomStyle();
            LoadComboBoxData();
            SetDefaultValues();
        }

        private void InitializeComponentForm()
        {
            this.Text = "Thêm Phiếu Mượn";
            this.Size = new Size(500, 360);
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
                Text = "Thêm Phiếu Mượn",
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

            // Thủ thư
            lblThuThu = new Label()
            {
                Text = "Thủ thư: *",
                Location = new Point(20, 100),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblThuThu);

            cboThuThu = new ComboBox()
            {
                Location = new Point(150, 97),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(cboThuThu);

            // Ngày mượn
            lblNgayMuon = new Label()
            {
                Text = "Ngày mượn: *",
                Location = new Point(20, 140),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblNgayMuon);

            dtpNgayMuon = new DateTimePicker()
            {
                Location = new Point(150, 137),
                Width = 200,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(dtpNgayMuon);

            // Hạn trả
            lblHanTra = new Label()
            {
                Text = "Hạn trả: *",
                Location = new Point(20, 180),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblHanTra);

            dtpHanTra = new DateTimePicker()
            {
                Location = new Point(150, 177),
                Width = 200,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(dtpHanTra);

            // Buttons
            btnSave = new Button()
            {
                Text = "Lưu",
                BackColor = mainColor,
                ForeColor = Color.White,
                Location = new Point(150, 250),
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
                Location = new Point(250, 250),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancel);

            Label lblNote = new Label()
            {
                Text = "* Trường bắt buộc",
                Location = new Point(20, 220),
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
                var dgList = docGiaDAO.GetAllDocGia();
                dgList.Insert(0, new DocGiaDTO { MaDocGia = 0, HoTen = "-- Chọn độc giả --" });
                cboDocGia.DataSource = dgList;
                cboDocGia.DisplayMember = "HoTen";
                cboDocGia.ValueMember = "MaDocGia";

                var ttList = thuThuDAO.GetAllThuThu();
                ttList.Insert(0, new ThuThu { MaThuThu = 0, TenThuThu = "-- Chọn thủ thư --" }); // Sửa HoTen → TenThuThu
                cboThuThu.DataSource = ttList;
                cboThuThu.DisplayMember = "TenThuThu"; // Sửa HoTen → TenThuThu
                cboThuThu.ValueMember = "MaThuThu";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetDefaultValues()
        {
            dtpNgayMuon.Value = DateTime.Now;
            dtpHanTra.Value = DateTime.Now.AddDays(14); // Mặc định mượn 2 tuần

            dtpNgayMuon.ValueChanged += (s, e) =>
            {
                if (dtpHanTra.Value <= dtpNgayMuon.Value)
                    dtpHanTra.Value = dtpNgayMuon.Value.AddDays(14);
            };
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                var phieu = new PhieuMuon()
                {
                    MaDocGia = Convert.ToInt32(cboDocGia.SelectedValue),
                    MaThuThu = Convert.ToInt32(cboThuThu.SelectedValue),
                    NgayMuon = dtpNgayMuon.Value.Date,
                    HanTra = dtpHanTra.Value.Date
                };

                bool success = phieuMuonDAO.InsertPhieuMuon(phieu);

                if (success)
                {
                    MessageBox.Show("Thêm phiếu mượn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thêm phiếu mượn thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (Convert.ToInt32(cboDocGia.SelectedValue) == 0)
            {
                MessageBox.Show("Vui lòng chọn độc giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboDocGia.Focus();
                return false;
            }

            if (Convert.ToInt32(cboThuThu.SelectedValue) == 0)
            {
                MessageBox.Show("Vui lòng chọn thủ thư!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboThuThu.Focus();
                return false;
            }

            if (dtpHanTra.Value <= dtpNgayMuon.Value)
            {
                MessageBox.Show("Hạn trả phải sau ngày mượn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}
