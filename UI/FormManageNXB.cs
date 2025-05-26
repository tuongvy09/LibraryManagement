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
    public partial class FormManageNXB : Form
    {
        private Label lblTitle;
        private Label lblTenNXB;
        private TextBox txtTenNXB;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private DataGridView dgvNXB;

        private Color mainColor = ColorTranslator.FromHtml("#739a4f");
        private NXBRepository nxbRepository = new NXBRepository();

        public FormManageNXB()
        {
            InitializeComponent();
            InitializeCustomStyle();
            LoadDanhSachNXB();
        }

        private void InitializeCustomStyle()
        {
            this.Text = "Quản lý Nhà Xuất Bản";
            this.Size = new Size(600, 400);

            lblTitle = new Label()
            {
                Text = "Quản lý Nhà Xuất Bản",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = mainColor,
                Location = new Point(200, 10),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            lblTenNXB = new Label()
            {
                Text = "Tên NXB:",
                Location = new Point(20, 60),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTenNXB);

            txtTenNXB = new TextBox()
            {
                Location = new Point(120, 57),
                Width = 300,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtTenNXB);

            txtSearch = new TextBox()
            {
                Location = new Point(120, 95),
                Width = 200,
                Font = new Font("Segoe UI", 10),
            };
            this.Controls.Add(txtSearch);

            btnSearch = new Button()
            {
                Text = "Tìm kiếm",
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Location = new Point(330, 93),
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
                Location = new Point(440, 95),
                Size = new Size(130, 30),
                FlatStyle = FlatStyle.Flat,
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.Click += BtnXoa_Click;
            this.Controls.Add(btnXoa);

            dgvNXB = new DataGridView()
            {
                Location = new Point(20, 140),
                Size = new Size(540, 200),
                Font = new Font("Segoe UI", 10),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvNXB.CellClick += DgvNXB_CellClick;
            this.Controls.Add(dgvNXB);
        }

        // Load danh sách NXB
        private void LoadDanhSachNXB()
        {
            dgvNXB.DataSource = nxbRepository.GetAllNXB();
            dgvNXB.Columns["MaNXB"].HeaderText = "Mã NXB";
            dgvNXB.Columns["TenNXB"].HeaderText = "Tên Nhà Xuất Bản";
        }

        // Tìm kiếm NXB theo tên
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            dgvNXB.DataSource = nxbRepository.SearchNXB(keyword);
        }
        // Xử lý khi click dòng trong DataGridView
        private void DgvNXB_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtTenNXB.Text = dgvNXB.Rows[e.RowIndex].Cells["TenNXB"].Value.ToString();
            }
        }

        // Thêm mới NXB
        private void BtnThem_Click(object sender, EventArgs e)
        {
            string tenNXB = txtTenNXB.Text.Trim();
            if (string.IsNullOrEmpty(tenNXB))
            {
                MessageBox.Show("Vui lòng nhập tên Nhà Xuất Bản!");
                return;
            }

            try
            {
                nxbRepository.AddNXB(tenNXB); 
                LoadDanhSachNXB();
                MessageBox.Show("Thêm NXB thành công.");
                txtTenNXB.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm NXB thất bại. Lỗi: " + ex.Message);
            }
        }

        // Sửa NXB
        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvNXB.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn Nhà Xuất Bản cần sửa!");
                return;
            }

            int maNXB = (int)dgvNXB.SelectedRows[0].Cells["MaNXB"].Value;
            string tenNXB = txtTenNXB.Text.Trim();

            if (string.IsNullOrEmpty(tenNXB))
            {
                MessageBox.Show("Vui lòng nhập tên Nhà Xuất Bản!");
                return;
            }

            try
            {
                nxbRepository.UpdateNXB(maNXB, tenNXB);
                MessageBox.Show("Cập nhật thành công!");
                LoadDanhSachNXB();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }
        }

        // Xóa NXB
        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNXB.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn Nhà Xuất Bản cần xóa!");
                return;
            }

            int maNXB = (int)dgvNXB.SelectedRows[0].Cells["MaNXB"].Value;

            var confirm = MessageBox.Show("Bạn có chắc muốn xóa Nhà Xuất Bản này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    nxbRepository.DeleteNXB(maNXB);
                    MessageBox.Show("Xóa thành công!");
                    LoadDanhSachNXB();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa: " + ex.Message);
                }
            }
        }
    }
}
