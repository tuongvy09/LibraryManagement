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
    public partial class FormDocGiaManagement : Form
    {
        private DataGridView dgvDocGia;
        private TextBox txtSearch;
        private Button btnSearch, btnAdd, btnEdit, btnDelete, btnRefresh;
        private Label lblSearch, lblTitle;

        private DocGiaDAO docGiaDAO = new DocGiaDAO();
        private List<DocGiaDTO> currentData;

        public FormDocGiaManagement()
        {
            InitializeComponentForm();
            InitializeCustomStyle();
            LoadData();
        }

        private void InitializeComponentForm()
        {
            this.Text = "Quản lý Độc giả";
            this.Size = new Size(1200, 600);
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
                Text = "QUẢN LÝ ĐỘC GIẢ",
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
            dgvDocGia = new DataGridView()
            {
                Location = new Point(20, 160),
                Size = new Size(1150, 350),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            dgvDocGia.DoubleClick += DgvDocGia_DoubleClick;
            this.Controls.Add(dgvDocGia);
        }

        private void LoadData()
        {
            try
            {
                currentData = docGiaDAO.GetAllDocGia();
                var displayData = currentData.Select(dg => new
                {
                    MaDocGia = dg.MaDocGia,
                    HoTen = dg.HoTen,
                    Tuoi = dg.Tuoi,
                    SoDT = dg.SoDT,
                    CCCD = dg.CCCD,
                    Email = dg.Email,
                    DiaChi = dg.DiaChi,
                    TenLoaiDG = dg.TenLoaiDG,
                    NgayDangKy = dg.NgayDangKy.ToString("dd/MM/yyyy"),
                    TienNo = dg.TienNo.ToString("N0") + " VNĐ",
                    TrangThai = dg.TrangThai ? "Hoạt động" : "Ngừng hoạt động"
                }).ToList();

                dgvDocGia.DataSource = displayData;

                // Thiết lập header
                if (dgvDocGia.Columns["MaDocGia"] != null)
                    dgvDocGia.Columns["MaDocGia"].HeaderText = "Mã ĐG";
                if (dgvDocGia.Columns["HoTen"] != null)
                    dgvDocGia.Columns["HoTen"].HeaderText = "Họ tên";
                if (dgvDocGia.Columns["Tuoi"] != null)
                    dgvDocGia.Columns["Tuoi"].HeaderText = "Tuổi";
                if (dgvDocGia.Columns["SoDT"] != null)
                    dgvDocGia.Columns["SoDT"].HeaderText = "Số ĐT";
                if (dgvDocGia.Columns["CCCD"] != null)
                    dgvDocGia.Columns["CCCD"].HeaderText = "CCCD";
                if (dgvDocGia.Columns["Email"] != null)
                    dgvDocGia.Columns["Email"].HeaderText = "Email";
                if (dgvDocGia.Columns["DiaChi"] != null)
                    dgvDocGia.Columns["DiaChi"].HeaderText = "Địa chỉ";
                if (dgvDocGia.Columns["TenLoaiDG"] != null)
                    dgvDocGia.Columns["TenLoaiDG"].HeaderText = "Loại ĐG";
                if (dgvDocGia.Columns["NgayDangKy"] != null)
                    dgvDocGia.Columns["NgayDangKy"].HeaderText = "Ngày đăng ký";
                if (dgvDocGia.Columns["TienNo"] != null)
                    dgvDocGia.Columns["TienNo"].HeaderText = "Tiền nợ";
                if (dgvDocGia.Columns["TrangThai"] != null)
                    dgvDocGia.Columns["TrangThai"].HeaderText = "Trạng thái";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var formAdd = new FormAddEditDocGia())
            {
                if (formAdd.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDocGia.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maDocGia = Convert.ToInt32(dgvDocGia.CurrentRow.Cells["MaDocGia"].Value);
            var docGia = currentData.FirstOrDefault(dg => dg.MaDocGia == maDocGia);

            if (docGia != null)
            {
                using (var formEdit = new FormAddEditDocGia(docGia))
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
            if (dgvDocGia.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa độc giả này?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int maDocGia = Convert.ToInt32(dgvDocGia.CurrentRow.Cells["MaDocGia"].Value);
                    if (docGiaDAO.DeleteDocGia(maDocGia))
                    {
                        MessageBox.Show("Xóa độc giả thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Xóa độc giả thất bại!", "Lỗi",
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormDocGiaManagement
            // 
            this.ClientSize = new System.Drawing.Size(933, 531);
            this.Name = "FormDocGiaManagement";
            this.ResumeLayout(false);

        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadData();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchDocGia();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchDocGia();
            }
        }

        private void SearchDocGia()
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
                    var searchResults = docGiaDAO.SearchDocGia(searchText);
                    var displayData = searchResults.Select(dg => new
                    {
                        MaDocGia = dg.MaDocGia,
                        HoTen = dg.HoTen,
                        Tuoi = dg.Tuoi,
                        SoDT = dg.SoDT,
                        CCCD = dg.CCCD,
                        Email = dg.Email,
                        DiaChi = dg.DiaChi,
                        TenLoaiDG = dg.TenLoaiDG,
                        NgayDangKy = dg.NgayDangKy.ToString("dd/MM/yyyy"),
                        TienNo = dg.TienNo.ToString("N0") + " VNĐ",
                        TrangThai = dg.TrangThai ? "Hoạt động" : "Ngừng hoạt động"
                    }).ToList();

                    dgvDocGia.DataSource = displayData;
                    currentData = searchResults;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvDocGia_DoubleClick(object sender, EventArgs e)
        {
            BtnEdit_Click(sender, e);
        }
    }
}

