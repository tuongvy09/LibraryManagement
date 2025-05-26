using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Models;
using LibraryManagement.Repositories;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.UI
{
    public partial class FormThuThuManagement : Form
    {
        private DataGridView dgvThuThu;
        private TextBox txtSearch;
        private Button btnSearch, btnAdd, btnEdit, btnDelete, btnRefresh;
        private Label lblSearch, lblTitle;

        private ThuThuDAO thuThuDAO = new ThuThuDAO();
        private List<ThuThu> currentData;

        public FormThuThuManagement()
        {
            InitializeComponentForm();
            InitializeCustomStyle();
            LoadData();
        }

        private void InitializeComponentForm()
        {
            this.Text = "Quản lý Thủ thư";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
        }

        private void InitializeCustomStyle()
        {
            Color mainColor = ColorTranslator.FromHtml("#739a4f");
            this.BackColor = Color.White;

            // Title
            lblTitle = new Label()
            {
                Text = "QUẢN LÝ THỦ THƯ",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = mainColor,
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Search controls
            lblSearch = new Label()
            {
                Text = "Tìm kiếm:",
                Location = new Point(20, 70),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblSearch);

            txtSearch = new TextBox()
            {
                Location = new Point(100, 67),
                Width = 250,
                Font = new Font("Segoe UI", 10)
            };
            txtSearch.KeyDown += TxtSearch_KeyDown;
            this.Controls.Add(txtSearch);

            btnSearch = new Button()
            {
                Text = "Tìm kiếm",
                BackColor = mainColor,
                ForeColor = Color.White,
                Location = new Point(360, 67),
                Size = new Size(80, 25),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;
            this.Controls.Add(btnSearch);

            // Action buttons
            btnAdd = new Button()
            {
                Text = "Thêm mới",
                BackColor = ColorTranslator.FromHtml("#28a745"),
                ForeColor = Color.White,
                Location = new Point(20, 110),
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;
            this.Controls.Add(btnAdd);

            btnEdit = new Button()
            {
                Text = "Sửa",
                BackColor = ColorTranslator.FromHtml("#ffc107"),
                ForeColor = Color.White,
                Location = new Point(130, 110),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += BtnEdit_Click;
            this.Controls.Add(btnEdit);

            btnDelete = new Button()
            {
                Text = "Xóa",
                BackColor = ColorTranslator.FromHtml("#dc3545"),
                ForeColor = Color.White,
                Location = new Point(220, 110),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;
            this.Controls.Add(btnDelete);

            btnRefresh = new Button()
            {
                Text = "Làm mới",
                BackColor = ColorTranslator.FromHtml("#6c757d"),
                ForeColor = Color.White,
                Location = new Point(310, 110),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;
            this.Controls.Add(btnRefresh);

            // DataGridView
            dgvThuThu = new DataGridView()
            {
                Location = new Point(20, 160),
                Size = new Size(950, 350),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            dgvThuThu.DoubleClick += DgvThuThu_DoubleClick;
            this.Controls.Add(dgvThuThu);
        }

        private void LoadData()
        {
            try
            {
                currentData = thuThuDAO.GetAllThuThu();
                var displayData = currentData.Select(tt => new
                {
                    MaThuThu = tt.MaThuThu,
                    TenThuThu = tt.TenThuThu,
                    Email = tt.Email,
                    SoDienThoai = tt.SoDienThoai,
                    DiaChi = tt.DiaChi,
                    NgayBatDauLam = tt.NgayBatDauLam.ToString("dd/MM/yyyy"),
                    TrangThai = tt.TrangThai ? "Hoạt động" : "Ngừng hoạt động",
                    Username = tt.Username
                }).ToList();

                dgvThuThu.DataSource = displayData;

                // Thiết lập header
                if (dgvThuThu.Columns["MaThuThu"] != null)
                    dgvThuThu.Columns["MaThuThu"].HeaderText = "Mã TT";
                if (dgvThuThu.Columns["TenThuThu"] != null)
                    dgvThuThu.Columns["TenThuThu"].HeaderText = "Tên thủ thư";
                if (dgvThuThu.Columns["Email"] != null)
                    dgvThuThu.Columns["Email"].HeaderText = "Email";
                if (dgvThuThu.Columns["SoDienThoai"] != null)
                    dgvThuThu.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                if (dgvThuThu.Columns["DiaChi"] != null)
                    dgvThuThu.Columns["DiaChi"].HeaderText = "Địa chỉ";
                if (dgvThuThu.Columns["NgayBatDauLam"] != null)
                    dgvThuThu.Columns["NgayBatDauLam"].HeaderText = "Ngày bắt đầu làm";
                if (dgvThuThu.Columns["TrangThai"] != null)
                    dgvThuThu.Columns["TrangThai"].HeaderText = "Trạng thái";
                if (dgvThuThu.Columns["Username"] != null)
                    dgvThuThu.Columns["Username"].HeaderText = "Tài khoản";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var formAdd = new FormAddEditThuThu())
            {
                if (formAdd.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvThuThu.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn thủ thư cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maThuThu = Convert.ToInt32(dgvThuThu.CurrentRow.Cells["MaThuThu"].Value);
            var thuThu = currentData.FirstOrDefault(tt => tt.MaThuThu == maThuThu);

            if (thuThu != null)
            {
                using (var formEdit = new FormAddEditThuThu(thuThu))
                {
                    if (formEdit.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvThuThu.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn thủ thư cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa thủ thư này?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int maThuThu = Convert.ToInt32(dgvThuThu.CurrentRow.Cells["MaThuThu"].Value);
                    if (thuThuDAO.DeleteThuThu(maThuThu))
                    {
                        MessageBox.Show("Xóa thủ thư thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Xóa thủ thư thất bại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadData();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchThuThu();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchThuThu();
            }
        }

        private void SearchThuThu()
        {
            try
            {
                string searchText = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchText))
                {
                    LoadData();
                }
                else
                {
                    var searchResults = thuThuDAO.SearchThuThu(searchText);
                    var displayData = searchResults.Select(tt => new
                    {
                        MaThuThu = tt.MaThuThu,
                        TenThuThu = tt.TenThuThu,
                        Email = tt.Email,
                        SoDienThoai = tt.SoDienThoai,
                        DiaChi = tt.DiaChi,
                        NgayBatDauLam = tt.NgayBatDauLam.ToString("dd/MM/yyyy"),
                        TrangThai = tt.TrangThai ? "Hoạt động" : "Ngừng hoạt động",
                        Username = tt.Username
                    }).ToList();

                    dgvThuThu.DataSource = displayData;
                    currentData = searchResults;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvThuThu_DoubleClick(object sender, EventArgs e)
        {
            BtnEdit_Click(sender, e);
        }
    }
}

