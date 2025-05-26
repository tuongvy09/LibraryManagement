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
    public partial class FormManageTheLoai : Form
    {
        private Label lblTitle, lblTenTheLoai, lblQDSoTuoi;
        private TextBox txtTenTheLoai, txtSearch, txtQDSoTuoi;
        private Button btnThem, btnSua, btnXoa, btnSearch;
        private DataGridView dgvTheLoai;
        private Color mainColor = ColorTranslator.FromHtml("#739a4f");

        private TheLoaiRepository theLoaiRepository = new TheLoaiRepository();
        public FormManageTheLoai()
        {
            InitializeComponent();
            InitializeCustomStyle();
            LoadDanhSachTheLoai();
        }

        private void InitializeCustomStyle()
        {
            this.Text = "Quản lý Thể Loại";
            this.Size = new Size(600, 450);

            lblTitle = new Label()
            {
                Text = "Quản lý Thể Loại",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = mainColor,
                Location = new Point(200, 10),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            lblTenTheLoai = new Label()
            {
                Text = "Tên Thể Loại:",
                Location = new Point(20, 60),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTenTheLoai);

            txtTenTheLoai = new TextBox()
            {
                Location = new Point(120, 57),
                Width = 300,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtTenTheLoai);

            txtSearch = new TextBox()
            {
                Location = new Point(120, 130),
                Width = 200,
                Font = new Font("Segoe UI", 10),
            };
            this.Controls.Add(txtSearch);
            lblQDSoTuoi = new Label()
            {
                Text = "QĐ Số Tuổi:",
                Location = new Point(20, 95),   // Đặt ngay dưới lblTenTheLoai
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblQDSoTuoi);

            txtQDSoTuoi = new TextBox()
            {
                Location = new Point(120, 92),  // Cùng vị trí ngang với txtTenTheLoai, xuống dưới một chút
                Width = 300,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtQDSoTuoi);

            btnSearch = new Button()
            {
                Text = "Tìm kiếm",
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Location = new Point(330, 128),
                Size = new Size(90, 30),
                FlatStyle = FlatStyle.Flat
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;
            this.Controls.Add(btnSearch);

            btnThem = new Button()
            {
                Text = "Thêm",
                BackColor = mainColor,
                ForeColor = Color.White,
                Location = new Point(440, 55),
                Size = new Size(60, 30),
                FlatStyle = FlatStyle.Flat,
            };
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.Click += BtnThem_Click;
            this.Controls.Add(btnThem);

            btnSua = new Button()
            {
                Text = "Sửa",
                BackColor = Color.Orange,
                ForeColor = Color.White,
                Location = new Point(510, 55),
                Size = new Size(60, 30),
                FlatStyle = FlatStyle.Flat,
            };
            btnSua.FlatAppearance.BorderSize = 0;
            btnSua.Click += BtnSua_Click;
            this.Controls.Add(btnSua);

            btnXoa = new Button()
            {
                Text = "Xóa",
                BackColor = Color.Red,
                ForeColor = Color.White,
                Location = new Point(440, 130),
                Size = new Size(130, 30),
                FlatStyle = FlatStyle.Flat,
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.Click += BtnXoa_Click;
            this.Controls.Add(btnXoa);

            dgvTheLoai = new DataGridView()
            {
                Location = new Point(20, 170), 
                Size = new Size(540, 230),
                Font = new Font("Segoe UI", 10),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvTheLoai.CellClick += DgvTheLoai_CellClick;
            this.Controls.Add(dgvTheLoai);
        }

        // Load danh sách thể loại lên dgvTheLoai
        private void LoadDanhSachTheLoai()
        {
            var list = theLoaiRepository.GetAllTheLoai();
            dgvTheLoai.DataSource = list;
            dgvTheLoai.Columns["MaTheLoai"].HeaderText = "Mã Thể Loại";
            dgvTheLoai.Columns["QDSoTuoi"].HeaderText = "Quy Định Số Tuổi";
            dgvTheLoai.Columns["TenTheLoai"].HeaderText = "Tên Thể Loại";
        }

        // Xử lý khi click dòng trong DataGridView
        private void DgvTheLoai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvTheLoai.Rows[e.RowIndex];
                txtQDSoTuoi.Text = row.Cells["QDSoTuoi"].Value.ToString();
                txtTenTheLoai.Text = row.Cells["TenTheLoai"].Value.ToString();
            }
        }

        // Thêm thể loại
        private void BtnThem_Click(object sender, EventArgs e)
        {
            string tenTheLoai = txtTenTheLoai.Text.Trim();
            if (!int.TryParse(txtQDSoTuoi.Text.Trim(), out int qdSoTuoi))
            {
                MessageBox.Show("Quy định số tuổi phải là số nguyên!");
                return;
            }
            if (string.IsNullOrEmpty(tenTheLoai))
            {
                MessageBox.Show("Vui lòng nhập tên thể loại!");
                return;
            }

            try
            {
                theLoaiRepository.AddTheLoai(qdSoTuoi, tenTheLoai);
                LoadDanhSachTheLoai();
                MessageBox.Show("Thêm thể loại thành công.");
                txtTenTheLoai.Clear();
                txtQDSoTuoi.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm thể loại thất bại: " + ex.Message);
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvTheLoai.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn thể loại cần sửa!");
                return;
            }

            int maTheLoai = Convert.ToInt32(dgvTheLoai.SelectedRows[0].Cells["MaTheLoai"].Value);

            if (!int.TryParse(txtQDSoTuoi.Text.Trim(), out int qdSoTuoi))
            {
                MessageBox.Show("Quy định số tuổi phải là số nguyên!");
                return;
            }

            try
            {
                theLoaiRepository.UpdateTheLoai(maTheLoai, qdSoTuoi);
                MessageBox.Show("Cập nhật thành công!");
                LoadDanhSachTheLoai();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvTheLoai.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn thể loại cần xóa!");
                return;
            }

            int maTheLoai = Convert.ToInt32(dgvTheLoai.SelectedRows[0].Cells["MaTheLoai"].Value);

            var confirm = MessageBox.Show("Bạn có chắc muốn xóa thể loại này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    theLoaiRepository.DeleteTheLoai(maTheLoai);
                    MessageBox.Show("Xóa thành công!");
                    LoadDanhSachTheLoai();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa: " + ex.Message);
                }
            }
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            var result = theLoaiRepository.SearchTheLoai(keyword);
            dgvTheLoai.DataSource = result;
        }
    }
}