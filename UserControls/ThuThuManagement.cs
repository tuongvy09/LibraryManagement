using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.UI;

namespace LibraryManagement.UserControls
{
    public partial class ThuThuManagement : UserControl
    {
        private DataGridView dgvThuThu;
        private TextBox txtSearch;
        private Button btnSearch, btnAdd, btnEdit, btnDelete, btnRefresh;
        private Label lblSearch, lblTitle;
        private Panel headerPanel, searchPanel, buttonPanel, gridPanel;

        private ThuThuDAO thuThuDAO = new ThuThuDAO();
        private List<ThuThu> currentData;

        public ThuThuManagement()
        {
            InitializeControls(); // Đổi tên để tránh trùng với Designer
        }

        private void InitializeControls() // Đổi tên từ InitializeComponent
        {
            this.SuspendLayout();

            // 
            // ThuThuManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Name = "ThuThuManagement";
            this.Size = new System.Drawing.Size(1000, 600);
            this.Load += new System.EventHandler(this.ThuThuManagement_Load);

            this.ResumeLayout(false);
        }

        private void ThuThuManagement_Load(object sender, EventArgs e)
        {
            InitializeCustomControls();
            LoadData();
        }

        private void InitializeCustomControls()
        {
            Color mainColor = ColorTranslator.FromHtml("#739a4f");

            // Header Panel
            headerPanel = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White
            };
            this.Controls.Add(headerPanel);

            lblTitle = new Label()
            {
                Text = "QUẢN LÝ THỦ THƯ",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = mainColor,
                Location = new Point(20, 15),
                AutoSize = true
            };
            headerPanel.Controls.Add(lblTitle);

            // Search Panel
            searchPanel = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(20, 10, 20, 10)
            };
            this.Controls.Add(searchPanel);

            lblSearch = new Label()
            {
                Text = "Tìm kiếm:",
                Location = new Point(0, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            searchPanel.Controls.Add(lblSearch);

            txtSearch = new TextBox()
            {
                Location = new Point(80, 12),
                Width = 250,
                Font = new Font("Segoe UI", 10)
            };
            txtSearch.KeyDown += TxtSearch_KeyDown;
            searchPanel.Controls.Add(txtSearch);

            btnSearch = new Button()
            {
                Text = "Tìm kiếm",
                BackColor = mainColor,
                ForeColor = Color.White,
                Location = new Point(340, 12),
                Size = new Size(80, 25),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;
            searchPanel.Controls.Add(btnSearch);

            // Button Panel
            buttonPanel = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 10)
            };
            this.Controls.Add(buttonPanel);

            btnAdd = new Button()
            {
                Text = "Thêm mới",
                BackColor = ColorTranslator.FromHtml("#28a745"),
                ForeColor = Color.White,
                Location = new Point(0, 10),
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;
            buttonPanel.Controls.Add(btnAdd);

            btnEdit = new Button()
            {
                Text = "Sửa",
                BackColor = ColorTranslator.FromHtml("#ffc107"),
                ForeColor = Color.White,
                Location = new Point(110, 10),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += BtnEdit_Click;
            buttonPanel.Controls.Add(btnEdit);

            btnDelete = new Button()
            {
                Text = "Xóa",
                BackColor = ColorTranslator.FromHtml("#dc3545"),
                ForeColor = Color.White,
                Location = new Point(200, 10),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;
            buttonPanel.Controls.Add(btnDelete);

            btnRefresh = new Button()
            {
                Text = "Làm mới",
                BackColor = ColorTranslator.FromHtml("#6c757d"),
                ForeColor = Color.White,
                Location = new Point(290, 10),
                Size = new Size(80, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;
            buttonPanel.Controls.Add(btnRefresh);

            // Grid Panel
            gridPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 20)
            };
            this.Controls.Add(gridPanel);

            // DataGridView
            dgvThuThu = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle()
                {
                    BackColor = ColorTranslator.FromHtml("#739a4f"),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                },
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Font = new Font("Segoe UI", 9),
                    SelectionBackColor = ColorTranslator.FromHtml("#a8cc7a"),
                    SelectionForeColor = Color.Black
                }
            };
            dgvThuThu.DoubleClick += DgvThuThu_DoubleClick;
            gridPanel.Controls.Add(dgvThuThu);
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
                    Email = tt.Email ?? "",
                    SoDienThoai = tt.SoDienThoai ?? "",
                    DiaChi = tt.DiaChi ?? "",
                    NgayBatDauLam = tt.NgayBatDauLam.ToString("dd/MM/yyyy"),
                    TrangThai = tt.TrangThai ? "Hoạt động" : "Ngừng hoạt động",
                    Username = tt.Username ?? ""
                }).ToList();

                dgvThuThu.DataSource = displayData;

                // Thiết lập header
                if (dgvThuThu.Columns["MaThuThu"] != null)
                {
                    dgvThuThu.Columns["MaThuThu"].HeaderText = "Mã TT";
                    dgvThuThu.Columns["MaThuThu"].Width = 80;
                }
                if (dgvThuThu.Columns["TenThuThu"] != null)
                    dgvThuThu.Columns["TenThuThu"].HeaderText = "Tên thủ thư";
                if (dgvThuThu.Columns["Email"] != null)
                    dgvThuThu.Columns["Email"].HeaderText = "Email";
                if (dgvThuThu.Columns["SoDienThoai"] != null)
                {
                    dgvThuThu.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                    dgvThuThu.Columns["SoDienThoai"].Width = 120;
                }
                if (dgvThuThu.Columns["DiaChi"] != null)
                    dgvThuThu.Columns["DiaChi"].HeaderText = "Địa chỉ";
                if (dgvThuThu.Columns["NgayBatDauLam"] != null)
                {
                    dgvThuThu.Columns["NgayBatDauLam"].HeaderText = "Ngày bắt đầu làm";
                    dgvThuThu.Columns["NgayBatDauLam"].Width = 130;
                }
                if (dgvThuThu.Columns["TrangThai"] != null)
                {
                    dgvThuThu.Columns["TrangThai"].HeaderText = "Trạng thái";
                    dgvThuThu.Columns["TrangThai"].Width = 120;
                }
                if (dgvThuThu.Columns["Username"] != null)
                {
                    dgvThuThu.Columns["Username"].HeaderText = "Tài khoản";
                    dgvThuThu.Columns["Username"].Width = 100;
                }

                // Highlight các dòng không hoạt động
                foreach (DataGridViewRow row in dgvThuThu.Rows)
                {
                    if (row.Cells["TrangThai"].Value != null &&
                        row.Cells["TrangThai"].Value.ToString() == "Ngừng hoạt động")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGray;
                        row.DefaultCellStyle.ForeColor = Color.DarkGray;
                    }
                }
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
                    MessageBox.Show("Thêm thủ thư thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        MessageBox.Show("Cập nhật thủ thư thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa thủ thư này?\n(Thủ thư sẽ bị đánh dấu là ngừng hoạt động)",
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
                        Email = tt.Email ?? "",
                        SoDienThoai = tt.SoDienThoai ?? "",
                        DiaChi = tt.DiaChi ?? "",
                        NgayBatDauLam = tt.NgayBatDauLam.ToString("dd/MM/yyyy"),
                        TrangThai = tt.TrangThai ? "Hoạt động" : "Ngừng hoạt động",
                        Username = tt.Username ?? ""
                    }).ToList();

                    dgvThuThu.DataSource = displayData;
                    currentData = searchResults;

                    if (searchResults.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy kết quả nào!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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

        // Public method để refresh data từ bên ngoài
        public void RefreshData()
        {
            LoadData();
        }

        // Public method để tìm kiếm từ bên ngoài
        public void SearchData(string searchText)
        {
            txtSearch.Text = searchText;
            SearchThuThu();
        }
    }
}
