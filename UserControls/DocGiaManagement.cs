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
    public partial class DocGiaManagement : UserControl
    {
        private DataGridView dgvDocGia;
        private TextBox txtSearch;
        private Button btnSearch, btnAdd, btnEdit, btnDelete, btnRefresh, btnThongKe;
        private Label lblSearch, lblTitle;
        private Panel headerPanel, searchPanel, buttonPanel, gridPanel;

        private DocGiaDAO docGiaDAO = new DocGiaDAO();
        private List<DocGiaDTO> currentData;

        public DocGiaManagement()
        {
            InitializeControls(); // Đổi tên để tránh trùng với Designer
        }

        private void InitializeControls() // Đổi tên từ InitializeComponent
        {
            this.SuspendLayout();

            // 
            // DocGiaManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Name = "DocGiaManagement";
            this.Size = new System.Drawing.Size(1000, 600);
            this.Load += new System.EventHandler(this.DocGiaManagement_Load);

            this.ResumeLayout(false);
        }

        private void DocGiaManagement_Load(object sender, EventArgs e)
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
                Text = "QUẢN LÝ ĐỘC GIẢ",
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

            btnThongKe = new Button()
            {
                Text = "Thống kê",
                BackColor = ColorTranslator.FromHtml("#6f42c1"),
                ForeColor = Color.White,
                Location = new Point(290, 10),
                Size = new Size(90, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnThongKe.FlatAppearance.BorderSize = 0;
            btnThongKe.Click += BtnThongKe_Click;
            buttonPanel.Controls.Add(btnThongKe);

            btnRefresh = new Button()
            {
                Text = "Làm mới",
                BackColor = ColorTranslator.FromHtml("#6c757d"),
                ForeColor = Color.White,
                Location = new Point(390, 10),
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
            dgvDocGia = new DataGridView()
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
            dgvDocGia.DoubleClick += DgvDocGia_DoubleClick;
            gridPanel.Controls.Add(dgvDocGia);
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
                    CCCD = dg.CCCD ?? "",
                    Email = dg.Email ?? "",
                    TenLoaiDG = dg.TenLoaiDG ?? "",
                    NgayDangKy = dg.NgayDangKy.ToString("dd/MM/yyyy"),
                    TienNo = dg.TienNo.ToString("N0") + " VNĐ",
                    TrangThai = dg.TrangThai ? "Hoạt động" : "Ngừng hoạt động"
                }).ToList();

                dgvDocGia.DataSource = displayData;

                // Thiết lập header và width
                SetupDataGridColumns();

                // Highlight các dòng không hoạt động
                HighlightInactiveRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridColumns()
        {
            if (dgvDocGia.Columns["MaDocGia"] != null)
            {
                dgvDocGia.Columns["MaDocGia"].HeaderText = "Mã ĐG";
                dgvDocGia.Columns["MaDocGia"].Width = 70;
            }
            if (dgvDocGia.Columns["HoTen"] != null)
                dgvDocGia.Columns["HoTen"].HeaderText = "Họ tên";
            if (dgvDocGia.Columns["Tuoi"] != null)
            {
                dgvDocGia.Columns["Tuoi"].HeaderText = "Tuổi";
                dgvDocGia.Columns["Tuoi"].Width = 60;
            }
            if (dgvDocGia.Columns["SoDT"] != null)
            {
                dgvDocGia.Columns["SoDT"].HeaderText = "Số ĐT";
                dgvDocGia.Columns["SoDT"].Width = 100;
            }
            if (dgvDocGia.Columns["CCCD"] != null)
            {
                dgvDocGia.Columns["CCCD"].HeaderText = "CCCD";
                dgvDocGia.Columns["CCCD"].Width = 120;
            }
            if (dgvDocGia.Columns["Email"] != null)
                dgvDocGia.Columns["Email"].HeaderText = "Email";
            if (dgvDocGia.Columns["TenLoaiDG"] != null)
            {
                dgvDocGia.Columns["TenLoaiDG"].HeaderText = "Loại ĐG";
                dgvDocGia.Columns["TenLoaiDG"].Width = 100;
            }
            if (dgvDocGia.Columns["NgayDangKy"] != null)
            {
                dgvDocGia.Columns["NgayDangKy"].HeaderText = "Ngày đăng ký";
                dgvDocGia.Columns["NgayDangKy"].Width = 110;
            }
            if (dgvDocGia.Columns["TienNo"] != null)
            {
                dgvDocGia.Columns["TienNo"].HeaderText = "Tiền nợ";
                dgvDocGia.Columns["TienNo"].Width = 100;
            }
            if (dgvDocGia.Columns["TrangThai"] != null)
            {
                dgvDocGia.Columns["TrangThai"].HeaderText = "Trạng thái";
                dgvDocGia.Columns["TrangThai"].Width = 100;
            }
        }

        private void HighlightInactiveRows()
        {
            foreach (DataGridViewRow row in dgvDocGia.Rows)
            {
                if (row.Cells["TrangThai"].Value != null)
                {
                    string trangThai = row.Cells["TrangThai"].Value.ToString();
                    if (trangThai == "Ngừng hoạt động")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGray;
                        row.DefaultCellStyle.ForeColor = Color.DarkGray;
                    }

                    // Highlight tiền nợ > 0
                    if (row.Cells["TienNo"].Value != null)
                    {
                        string tienNoStr = row.Cells["TienNo"].Value.ToString().Replace(" VNĐ", "").Replace(",", "");
                        if (decimal.TryParse(tienNoStr, out decimal tienNo) && tienNo > 0)
                        {
                            row.Cells["TienNo"].Style.BackColor = Color.LightYellow;
                            row.Cells["TienNo"].Style.ForeColor = Color.Red;
                        }
                    }
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var formAdd = new FormAddEditDocGia())
            {
                if (formAdd.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                    MessageBox.Show("Thêm độc giả thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        MessageBox.Show("Cập nhật độc giả thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa độc giả này?\n(Độc giả sẽ bị đánh dấu là ngừng hoạt động)",
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

        private void BtnThongKe_Click(object sender, EventArgs e)
        {
            var formThongKe = new FormThongKe();
            formThongKe.Show();
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
                        CCCD = dg.CCCD ?? "",
                        Email = dg.Email ?? "",
                        TenLoaiDG = dg.TenLoaiDG ?? "",
                        NgayDangKy = dg.NgayDangKy.ToString("dd/MM/yyyy"),
                        TienNo = dg.TienNo.ToString("N0") + " VNĐ",
                        TrangThai = dg.TrangThai ? "Hoạt động" : "Ngừng hoạt động"
                    }).ToList();

                    dgvDocGia.DataSource = displayData;
                    currentData = searchResults;

                    SetupDataGridColumns();
                    HighlightInactiveRows();

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

        private void DgvDocGia_DoubleClick(object sender, EventArgs e)
        {
            BtnEdit_Click(sender, e);
        }

        // Public methods
        public void RefreshData()
        {
            LoadData();
        }

        public void SearchData(string searchText)
        {
            txtSearch.Text = searchText;
            SearchDocGia();
        }
    }
}
