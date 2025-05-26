using LibraryManagement.Models;
using LibraryManagement.Repositories.LibraryManagement.Repositories;
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
    public partial class FormManageTG : Form
    {
        private Label lblTitle, lblTenTacGia;
        private TextBox txtTenTacGia, txtSearch;
        private Button btnThem, btnSua, btnXoa;
        private DataGridView dgvTacGia;
        private Button btnSearch;
        private string placeholderText = "Nhập tên tác giả...";
        private Color mainColor = ColorTranslator.FromHtml("#739a4f");

        private TacGiaRepository tacGiaRepository = new TacGiaRepository();
        public FormManageTG()
        {
            InitializeComponent();
            InitializeCustomStyle();
            LoadDanhSachTacGia();
            SetupPlaceholder();
        }

        private void InitializeCustomStyle()
        {
            this.Text = "Quản lý Tác Giả";
            this.Size = new Size(600, 400);

            lblTitle = new Label()
            {
                Text = "Quản lý Tác Giả",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = mainColor,
                Location = new Point(200, 10),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            lblTenTacGia = new Label()
            {
                Text = "Tên Tác Giả:",
                Location = new Point(20, 60),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTenTacGia);

            txtTenTacGia = new TextBox()
            {
                Location = new Point(120, 57),
                Width = 300,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtTenTacGia);
            txtSearch = new TextBox()
            {
                Location = new Point(120, 95),
                Width = 200,
                Font = new Font("Segoe UI", 10),
            };
            this.Controls.Add(txtSearch);

            // Nút tìm kiếm
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

            dgvTacGia = new DataGridView()
            {
                Location = new Point(20, 140),
                Size = new Size(540, 200),
                Font = new Font("Segoe UI", 10),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvTacGia.CellClick += DgvTacGia_CellClick;
            this.Controls.Add(dgvTacGia);
        }
        private void SetupPlaceholder()
        {
            txtSearch.ForeColor = Color.Gray;
            txtSearch.Text = placeholderText;

            txtSearch.Enter += (s, e) =>
            {
                if (txtSearch.Text == placeholderText)
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.Black;
                }
            };

            txtSearch.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = placeholderText;
                    txtSearch.ForeColor = Color.Gray;
                }
            };
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            LoadDanhSachTacGia(txtSearch.Text.Trim());
        }

        private void LoadDanhSachTacGia(string searchTerm = "")
        {
            List<TacGia> danhSach;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Nếu không có từ khóa tìm kiếm, lấy tất cả tác giả
                danhSach = tacGiaRepository.GetAllTacGia();
            }
            else
            {
                // Nếu có từ khóa tìm kiếm, lấy tác giả theo tên
                danhSach = tacGiaRepository.GetTacGias(searchTerm);
            }

            dgvTacGia.DataSource = danhSach;

            // Đặt tiêu đề cho các cột
            if (dgvTacGia.Columns.Contains("MaTacGia"))
                dgvTacGia.Columns["MaTacGia"].HeaderText = "Mã Tác Giả";

            if (dgvTacGia.Columns.Contains("TenTG"))
                dgvTacGia.Columns["TenTG"].HeaderText = "Tên Tác Giả";
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            string ten = txtTenTacGia.Text.Trim();
            if (string.IsNullOrEmpty(ten))
            {
                MessageBox.Show("Vui lòng nhập tên tác giả!");
                return;
            }

            tacGiaRepository.AddTacGia(ten);
            LoadDanhSachTacGia();
            txtTenTacGia.Clear();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (dgvTacGia.SelectedRows.Count == 0) return;

            int id = (int)dgvTacGia.SelectedRows[0].Cells["MaTacGia"].Value;
            string tenMoi = txtTenTacGia.Text.Trim();

            if (string.IsNullOrEmpty(tenMoi))
            {
                MessageBox.Show("Vui lòng nhập tên tác giả mới!");
                return;
            }

            tacGiaRepository.UpdateTacGia(id, tenMoi);
            LoadDanhSachTacGia();
            txtTenTacGia.Clear();
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvTacGia.SelectedRows.Count == 0) return;

            int id = (int)dgvTacGia.SelectedRows[0].Cells["MaTacGia"].Value;

            DialogResult confirm = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                tacGiaRepository.DeleteTacGia(id);
                LoadDanhSachTacGia();
                txtTenTacGia.Clear();
            }
        }

        private void DgvTacGia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtTenTacGia.Text = dgvTacGia.Rows[e.RowIndex].Cells["TenTG"].Value.ToString();
            }
        }
    }
}
