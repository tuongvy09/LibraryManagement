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
        private Label lblTitle;
        private Label lblTenDocGia, lblTenDauSach, lblNgayMuon, lblNgayTra, lblTrangThai;
        private ComboBox cbTenDocGia, cbTenDauSach;
        private DateTimePicker dtpNgayMuon, dtpNgayTra;
        private Label lblTrangThaiValue;
        private Button btnSave, btnCancel;

        private PhieuMuonDAO phieuMuonDAO = new PhieuMuonDAO();
        private DocGiaDAO docGiaDAO = new DocGiaDAO();
        private CuonSachRepository cuonSachDAO = new CuonSachRepository();

        private Dictionary<string, int> docGiaDict = new Dictionary<string, int>();
        private Dictionary<string, int> cuonSachDict = new Dictionary<string, int>();

        public FormAddPhieuMuon()
        {
            InitializeComponentForm();
            InitializeCustomStyle();
            LoadComboBoxData();
            UpdateTrangThaiDisplay();
        }

        private void InitializeComponentForm()
        {
            this.Text = "Thêm Phiếu Mượn";
            this.Size = new Size(550, 400);
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
                Location = new Point(180, 15),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            lblTenDocGia = CreateLabel("Tên độc giả: *", 20, 60, mainColor);
            this.Controls.Add(lblTenDocGia);

            cbTenDocGia = CreateComboBox(160, 57);
            this.Controls.Add(cbTenDocGia);

            lblTenDauSach = CreateLabel("Tên sách: *", 20, 100, mainColor);
            this.Controls.Add(lblTenDauSach);

            cbTenDauSach = CreateComboBox(160, 97);
            this.Controls.Add(cbTenDauSach);

            lblNgayMuon = CreateLabel("Ngày mượn: *", 20, 140, mainColor);
            this.Controls.Add(lblNgayMuon);

            dtpNgayMuon = CreateDateTimePicker(160, 137);
            dtpNgayMuon.Value = DateTime.Now.Date;
            this.Controls.Add(dtpNgayMuon);

            lblNgayTra = CreateLabel("Ngày trả: *", 20, 180, mainColor);
            this.Controls.Add(lblNgayTra);

            dtpNgayTra = CreateDateTimePicker(160, 177);
            dtpNgayTra.Value = DateTime.Now.Date.AddDays(7);
            dtpNgayTra.ValueChanged += (s, e) => UpdateTrangThaiDisplay();
            this.Controls.Add(dtpNgayTra);

            lblTrangThai = CreateLabel("Trạng thái:", 20, 220, mainColor);
            this.Controls.Add(lblTrangThai);

            lblTrangThaiValue = new Label()
            {
                Text = "Chưa trả",
                Location = new Point(160, 220),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.Red
            };
            this.Controls.Add(lblTrangThaiValue);

            btnSave = new Button()
            {
                Text = "Lưu",
                BackColor = mainColor,
                ForeColor = Color.White,
                Location = new Point(160, 270),
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
                Location = new Point(270, 270),
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
                Location = new Point(20, 320),
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.Red
            };
            this.Controls.Add(lblNote);
        }

        private Label CreateLabel(string text, int x, int y, Color foreColor)
        {
            return new Label()
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = foreColor
            };
        }

        private ComboBox CreateComboBox(int x, int y)
        {
            return new ComboBox()
            {
                Location = new Point(x, y),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
        }

        private DateTimePicker CreateDateTimePicker(int x, int y)
        {
            return new DateTimePicker()
            {
                Location = new Point(x, y),
                Width = 200,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 10)
            };
        }

        private void LoadComboBoxData()
        {
            var docGias = docGiaDAO.GetAllDocGia();
            foreach (var dg in docGias)
            {
                cbTenDocGia.Items.Add(dg.HoTen);
                docGiaDict[dg.HoTen] = dg.MaDocGia;
            }

            var cuonSachs = cuonSachDAO.GetAllCuonSachDetails();
            foreach (var cs in cuonSachs)
            {
                cbTenDauSach.Items.Add(cs.TenCuonSach);
                cuonSachDict[cs.TenCuonSach] = cs.MaDauSach;
            }

            if (cbTenDocGia.Items.Count > 0) cbTenDocGia.SelectedIndex = 0;
            if (cbTenDauSach.Items.Count > 0) cbTenDauSach.SelectedIndex = 0;
        }

        private void UpdateTrangThaiDisplay()
        {
            bool daTra = dtpNgayTra.Value.Date <= DateTime.Now.Date && dtpNgayTra.Value.Date >= dtpNgayMuon.Value.Date;
            lblTrangThaiValue.Text = daTra ? "Đã trả" : "Chưa trả";
            lblTrangThaiValue.ForeColor = daTra ? Color.Green : Color.Red;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var phieuMuonToAdd = new PhieuMuon()
                {
                    MaDocGia = GetMaDocGiaFromName(cbTenDocGia.SelectedItem.ToString()),
                    NgayMuon = dtpNgayMuon.Value.Date,
                    NgayTra = dtpNgayTra.Value.Date,
                    TrangThaiM = lblTrangThaiValue.Text,
                    GiaMuon = 0,
                    SoNgayMuon = (dtpNgayTra.Value.Date - dtpNgayMuon.Value.Date).Days,
                    TienCoc = 0
                };

                List<int> danhSachMaCuonSach = GetDanhSachMaCuonSach();

                bool success = phieuMuonDAO.AddPhieuMuon(phieuMuonToAdd, danhSachMaCuonSach);

                if (success)
                {
                    MessageBox.Show("Thêm phiếu mượn thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thêm phiếu mượn thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetMaDocGiaFromName(string tenDocGia)
        {
            return docGiaDict.TryGetValue(tenDocGia, out int ma) ? ma : -1;
        }

        private List<int> GetDanhSachMaCuonSach()
        {
            string tenCuonSach = cbTenDauSach.SelectedItem.ToString();
            if (cuonSachDict.TryGetValue(tenCuonSach, out int maCuonSach))
            {
                return new List<int> { maCuonSach };
            }
            return new List<int>();
        }

        private bool ValidateInput()
        {
            if (cbTenDocGia.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn tên độc giả!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbTenDocGia.Focus();
                return false;
            }

            if (cbTenDauSach.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn tên sách!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbTenDauSach.Focus();
                return false;
            }

            if (dtpNgayTra.Value.Date < dtpNgayMuon.Value.Date)
            {
                MessageBox.Show("Ngày trả không được nhỏ hơn ngày mượn!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgayTra.Focus();
                return false;
            }

            return true;
        }
    }
}
