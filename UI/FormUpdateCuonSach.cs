using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.Repositories.LibraryManagement.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryManagement.UI
{
    public partial class FormUpdateCuonSach : Form
    {
        private Label lblTitle, lblTenCuonSach, lblTrangThai, lblDauSach, lblTheLoai, lblNXB, lblTacGia;
        private TextBox txtTenCuonSach;
        private ComboBox cboTrangThai, cboDauSach, cboTheLoai, cboNXB;
        private ListBox lstTacGia;
        private Button btnSave, btnCancel;

        private DauSachRepository repo = new DauSachRepository();
        private TheLoaiRepository theLoaiRepository = new TheLoaiRepository();
        private NXBRepository theXBRepository = new NXBRepository();
        private TacGiaRepository tacGiaRepository = new TacGiaRepository();
        private CuonSachRepository cuonSachRepository = new CuonSachRepository();
        private int _maCuonSach;

        public FormUpdateCuonSach(int maCuonSach)
        {
            InitializeUpdateCuonSachForm();
            InitializeComponent();
            _maCuonSach = maCuonSach;
            LoadCuonSachDetail();
            LoadDataToControls();
        }
        private void LoadCuonSachDetail()
        {
            var cuonSach = cuonSachRepository.GetCuonSachById(_maCuonSach);

            txtTenCuonSach.Text = cuonSach.TenCuonSach;
            cboDauSach.SelectedValue = cuonSach.TenDauSach;
            cboNXB.SelectedValue = cuonSach.TenNSB;
            cboTheLoai.SelectedValue = cuonSach.TenNSB;
            cboTrangThai.SelectedItem = cuonSach.TrangThaiSach;

            // Chọn lại các tác giả
            for (int i = 0; i < lstTacGia.Items.Count; i++)
            {
                var item = (TacGiaItem)lstTacGia.Items[i];
                if (cuonSach.TacGias.Contains(item.TenTG))
                {
                    lstTacGia.SetSelected(i, true);
                }
            }
        }
        private void InitializeUpdateCuonSachForm()
        {
            Color mainColor = ColorTranslator.FromHtml("#739a4f");

            lblTitle = new Label()
            {
                Text = "Cập Nhật Cuốn Sách",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = mainColor,
                Location = new Point(140, 10),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            lblTenCuonSach = new Label()
            {
                Text = "Tên cuốn sách:",
                Location = new Point(20, 50),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTenCuonSach);

            txtTenCuonSach = new TextBox()
            {
                Location = new Point(140, 47),
                Width = 220,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtTenCuonSach);

            lblTrangThai = new Label()
            {
                Text = "Trạng thái sách:",
                Location = new Point(20, 90),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTrangThai);

            cboTrangThai = new ComboBox()
            {
                Location = new Point(140, 87),
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            // Ví dụ trạng thái
            cboTrangThai.Items.AddRange(new string[] { "Tốt", "Đang mượn", "Hỏng", "Mất" });
            this.Controls.Add(cboTrangThai);

            lblDauSach = new Label()
            {
                Text = "Đầu sách:",
                Location = new Point(20, 130),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblDauSach);

            cboDauSach = new ComboBox()
            {
                Location = new Point(140, 127),
                Width = 210,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(cboDauSach);

            lblTheLoai = new Label()
            {
                Text = "Thể loại:",
                Location = new Point(20, 170),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTheLoai);

            cboTheLoai = new ComboBox()
            {
                Location = new Point(140, 167),
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(cboTheLoai);

            lblNXB = new Label()
            {
                Text = "Nhà xuất bản:",
                Location = new Point(20, 210),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblNXB);

            cboNXB = new ComboBox()
            {
                Location = new Point(140, 207),
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(cboNXB);

            lblTacGia = new Label()
            {
                Text = "Tác giả:",
                Location = new Point(20, 250),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTacGia);

            lstTacGia = new ListBox()
            {
                Location = new Point(140, 247),
                Width = 220,
                Height = 80,
                SelectionMode = SelectionMode.MultiExtended,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lstTacGia);

            btnSave = new Button()
            {
                Text = "Lưu",
                BackColor = mainColor,
                ForeColor = Color.White,
                Location = new Point(100, 340),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click; // Bạn tự định nghĩa xử lý lưu
            this.Controls.Add(btnSave);

            btnCancel = new Button()
            {
                Text = "Hủy",
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Location = new Point(220, 340),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancel);

            // TODO: Load dữ liệu cho các combobox/listbox (ví dụ từ DB)
        }
        private void LoadDataToControls()
        {
            // Load DauSach
            List<DauSach> dsList = repo.GetAllMaDauSach();
            cboDauSach.DataSource = dsList;
            cboDauSach.DisplayMember = "TenDauSach";
            cboDauSach.ValueMember = "MaDauSach";

            // Load NhaXuatBan
            List<NhaXuatBan> nxbList = theXBRepository.GetAllNXB();
            cboNXB.DataSource = nxbList;
            cboNXB.DisplayMember = "TenNXB";
            cboNXB.ValueMember = "MaNXb";

            // Load TheLoai
            List<TheLoai> tlList = theLoaiRepository.GetAllTheLoai();
            cboTheLoai.DataSource = tlList;
            cboTheLoai.DisplayMember = "TenTheLoai";
            cboTheLoai.ValueMember = "MaTheLoai";

            // Load TacGia
            List<(int MaTacGia, string TenTG)> tgList = tacGiaRepository.GetAllTacGia();
            lstTacGia.Items.Clear();
            foreach (var tg in tgList)
            {
                // Tạo đối tượng item hiển thị tên nhưng lưu mã tác giả
                lstTacGia.Items.Add(new TacGiaItem(tg.MaTacGia, tg.TenTG));
            }
        }

        // Lớp tiện ích để lưu thông tin tác giả vào ListBox
        private class TacGiaItem
        {
            public int MaTacGia { get; }
            public string TenTG { get; }
            public TacGiaItem(int ma, string ten)
            {
                MaTacGia = ma;
                TenTG = ten;
            }
            public override string ToString() => TenTG;
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            string tenCuonSach = txtTenCuonSach.Text.Trim();
            int maDauSach = (int)cboDauSach.SelectedValue;
            int maNXB = (int)cboNXB.SelectedValue;
            int maTheLoai = (int)cboTheLoai.SelectedValue;
            string trangThai = cboTrangThai.SelectedItem?.ToString();

            // Lấy danh sách mã tác giả được chọn
            List<int> dsTacGiaChon = new List<int>();
            foreach (TacGiaItem item in lstTacGia.SelectedItems)
            {
                dsTacGiaChon.Add(item.MaTacGia);
            }

            try
            {
                cuonSachRepository.UpdateCuonSach(
                    _maCuonSach,
                    tenCuonSach,
                    maDauSach,
                    maNXB,
                    maTheLoai,
                    trangThai,
                    dsTacGiaChon
                );

                MessageBox.Show("Đã cập nhật cuốn sách thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật cuốn sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
