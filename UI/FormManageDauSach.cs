using LibraryManagement.Models;
using LibraryManagement.Repositories.LibraryManagement.Repositories;
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
    public partial class FormManageDauSach : Form
    {
        Label lblTitle;
        TextBox txtTenDauSach, txtSearch;
        private TextBox txtNamXB, txtNgonNgu, txtSoTrang, txtGiaTien, txtMoTa;
        ComboBox cbTheLoai, cbNXB;
        Button btnThem, btnSua, btnXoa, btnSearch;
        DataGridView dgvDauSach;
        private CheckedListBox clbTacGia;
        private Color mainColor = ColorTranslator.FromHtml("#739a4f");

        TacGiaRepository tacGiaRepository = new TacGiaRepository();
        TheLoaiRepository theLoaiRepository = new TheLoaiRepository();
        NXBRepository NXBRepository = new NXBRepository();
        DauSachRepository dsRepository = new DauSachRepository();
        DauSachTacGiaRepository dstgRepository = new DauSachTacGiaRepository();
        private int maDauSach = -1;
        public FormManageDauSach()
        {
            InitializeComponent();
            InitializeCustomStyle();
            LoadDauSachToDgv();
            LoadTacGia();
            LoadTheLoaiList();
            LoadComboBoxData();
        }

        private void InitializeCustomStyle()
        {
            this.Text = "Quản lý Đầu Sách";
            this.Size = new Size(750, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblTitle = new Label()
            {
                Text = "Quản lý Đầu Sách",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = mainColor,
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitle);

            // Panel chứa các controls
            Panel inputPanel = new Panel()
            {
                Location = new Point(10, 50),
                Size = new Size(720, 240),
                AutoScroll = true
            };
            this.Controls.Add(inputPanel);

            // Sử dụng TableLayoutPanel
            TableLayoutPanel layout = new TableLayoutPanel()
            {
                ColumnCount = 4,
                RowCount = 6,
                Dock = DockStyle.Fill,
                AutoSize = true,
                Padding = new Padding(5),
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            inputPanel.Controls.Add(layout);

            // Dòng 1
            layout.Controls.Add(new Label() { Text = "Tên Đầu Sách:", Font = new Font("Segoe UI", 10), ForeColor = mainColor, Anchor = AnchorStyles.Right }, 0, 0);
            txtTenDauSach = new TextBox() { Font = new Font("Segoe UI", 10), Dock = DockStyle.Fill };
            layout.Controls.Add(txtTenDauSach, 1, 0);

            layout.Controls.Add(new Label() { Text = "Thể Loại:", Font = new Font("Segoe UI", 10), ForeColor = mainColor, Anchor = AnchorStyles.Right }, 2, 0);
            cbTheLoai = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10), Dock = DockStyle.Fill };
            layout.Controls.Add(cbTheLoai, 3, 0);

            // Dòng 2
            layout.Controls.Add(new Label() { Text = "Nhà Xuất Bản:", Font = new Font("Segoe UI", 10), ForeColor = mainColor, Anchor = AnchorStyles.Right }, 0, 1);
            cbNXB = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10), Dock = DockStyle.Fill };
            layout.Controls.Add(cbNXB, 1, 1);

            layout.Controls.Add(new Label() { Text = "Tác Giả:", Font = new Font("Segoe UI", 10), ForeColor = mainColor, Anchor = AnchorStyles.Right }, 2, 1);
            clbTacGia = new CheckedListBox() { Font = new Font("Segoe UI", 10), Height = 60, Dock = DockStyle.Fill };
            layout.Controls.Add(clbTacGia, 3, 1);

            // Dòng 3
            layout.Controls.Add(new Label() { Text = "Năm XB:", Font = new Font("Segoe UI", 10), ForeColor = mainColor, Anchor = AnchorStyles.Right }, 0, 2);
            txtNamXB = new TextBox() { Font = new Font("Segoe UI", 10), Dock = DockStyle.Fill };
            layout.Controls.Add(txtNamXB, 1, 2);

            layout.Controls.Add(new Label() { Text = "Ngôn ngữ:", Font = new Font("Segoe UI", 10), ForeColor = mainColor, Anchor = AnchorStyles.Right }, 2, 2);
            txtNgonNgu = new TextBox() { Font = new Font("Segoe UI", 10), Dock = DockStyle.Fill };
            layout.Controls.Add(txtNgonNgu, 3, 2);

            // Dòng 4
            layout.Controls.Add(new Label() { Text = "Số lượng:", Font = new Font("Segoe UI", 10), ForeColor = mainColor, Anchor = AnchorStyles.Right }, 0, 3);
            txtSoTrang = new TextBox() { Font = new Font("Segoe UI", 10), Dock = DockStyle.Fill };
            layout.Controls.Add(txtSoTrang, 1, 3);

            layout.Controls.Add(new Label() { Text = "ISBN:", Font = new Font("Segoe UI", 10), ForeColor = mainColor, Anchor = AnchorStyles.Right }, 2, 3);
            txtGiaTien = new TextBox() { Font = new Font("Segoe UI", 10), Dock = DockStyle.Fill };
            layout.Controls.Add(txtGiaTien, 3, 3);

            // Dòng 5: Mô tả
            layout.Controls.Add(new Label() { Text = "Mô tả:", Font = new Font("Segoe UI", 10), ForeColor = mainColor, Anchor = AnchorStyles.Right }, 0, 4);
            txtMoTa = new TextBox() { Font = new Font("Segoe UI", 10), Multiline = true, Height = 50, Dock = DockStyle.Fill };
            layout.SetColumnSpan(txtMoTa, 3);
            layout.Controls.Add(txtMoTa, 1, 4);

            // Dòng 6: Tìm kiếm
            layout.Controls.Add(new Label() { Text = "Tìm kiếm:", Font = new Font("Segoe UI", 10), ForeColor = mainColor, Anchor = AnchorStyles.Right }, 0, 5);
            txtSearch = new TextBox() { Font = new Font("Segoe UI", 10), Dock = DockStyle.Fill };
            layout.Controls.Add(txtSearch, 1, 5);

            btnSearch = new Button()
            {
                Text = "Tìm kiếm",
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                Width = 90,
                FlatStyle = FlatStyle.Flat
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            layout.Controls.Add(btnSearch, 3, 5);

            // Các nút thêm, sửa, xóa bên ngoài
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel()
            {
                Location = new Point(10, 300),
                Size = new Size(700, 40),
                FlowDirection = FlowDirection.LeftToRight
            };
            this.Controls.Add(buttonPanel);

            btnThem = new Button() { Text = "Thêm", BackColor = mainColor, ForeColor = Color.White, Size = new Size(70, 30), FlatStyle = FlatStyle.Flat };
            btnSua = new Button() { Text = "Sửa", BackColor = Color.Orange, ForeColor = Color.White, Size = new Size(70, 30), FlatStyle = FlatStyle.Flat };
            btnXoa = new Button() { Text = "Xóa", BackColor = Color.Red, ForeColor = Color.White, Size = new Size(70, 30), FlatStyle = FlatStyle.Flat };
            btnThem.FlatAppearance.BorderSize = 0;
            btnSua.FlatAppearance.BorderSize = 0;
            btnXoa.FlatAppearance.BorderSize = 0;

            buttonPanel.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa });

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;

            // DataGridView
            dgvDauSach = new DataGridView()
            {
                Location = new Point(10, 350),
                Size = new Size(710, 200),
                Font = new Font("Segoe UI", 10),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvDauSach.CellClick += DgvDauSach_CellClick;
            this.Controls.Add(dgvDauSach);
        }

        private void LoadDauSachToDgv()
        {
            try
            {
                List<DauSachDTO> list = dsRepository.GetAllDauSachFullInfo();

                dgvDauSach.DataSource = null;
                dgvDauSach.DataSource = list;

                // Đặt tiêu đề cột cho đẹp
                dgvDauSach.Columns["MaDauSach"].HeaderText = "Mã Đầu Sách";
                dgvDauSach.Columns["TenDauSach"].HeaderText = "Tên Đầu Sách";
                dgvDauSach.Columns["TenTheLoai"].HeaderText = "Thể Loại";
                if (dgvDauSach.Columns.Contains("TenNSB"))
                {
                    dgvDauSach.Columns["TenNSB"].HeaderText = "Nhà Xuất Bản";
                }
                dgvDauSach.Columns["NamXuatBan"].HeaderText = "Năm Xuất Bản";
                dgvDauSach.Columns["GiaTien"].HeaderText = "Giá Tiền";
                dgvDauSach.Columns["SoTrang"].HeaderText = "Số Trang";
                dgvDauSach.Columns["NgonNgu"].HeaderText = "Ngôn Ngữ";
                if (dgvDauSach.Columns.Contains("TacGia"))
                {
                    dgvDauSach.Columns["TacGia"].HeaderText = "Tác Giả";
                }
                if (dgvDauSach.Columns.Contains("MoTa"))
                {
                    dgvDauSach.Columns["MoTa"].HeaderText = "Mô tả";
                    dgvDauSach.Columns["MoTa"].Visible = true; // nếu muốn hiện
                }

                // Ẩn các cột ID
                dgvDauSach.Columns["MaDauSach"].Visible = false;
                dgvDauSach.Columns["MaTheLoai"].Visible = false;
                dgvDauSach.Columns["MaNXB"].Visible = false;

                // Tự động co dãn cột
                dgvDauSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách đầu sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTacGia()
        {
            List<TacGia> danhSachTacGia = tacGiaRepository.GetAllTacGia(); // <-- Lấy đúng danh sách tác giả
            clbTacGia.DataSource = danhSachTacGia;
            clbTacGia.DisplayMember = "TenTG";
            clbTacGia.ValueMember = "MaTacGia";
        }

        private void LoadTheLoaiList()
        {
            TheLoaiRepository repo = new TheLoaiRepository();
            List<TheLoai> theLoais = repo.GetAllTheLoai();

            cbTheLoai.DataSource = null;
            cbTheLoai.DataSource = theLoais;
            cbTheLoai.DisplayMember = "TenTheLoai";
            cbTheLoai.ValueMember = "MaTheLoai";

            if (cbTheLoai.Items.Count > 0)
                cbTheLoai.SelectedIndex = 0;
        }

        private void LoadComboBoxData()
        {
            var theLoaiList = theLoaiRepository.GetAllTheLoai();
            var nxbList = NXBRepository.GetAllNXB();

            cbTheLoai.DataSource = theLoaiList;
            cbTheLoai.DisplayMember = "TenTheLoai";
            cbTheLoai.ValueMember = "MaTheLoai";

            cbNXB.DataSource = nxbList;
            cbNXB.DisplayMember = "TenNXB";
            cbNXB.ValueMember = "MaNXB";
        }

        private void DgvDauSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvDauSach.Rows.Count) return;

            DataGridViewRow row = dgvDauSach.Rows[e.RowIndex];

            // Kiểm tra các cell trước khi lấy Value
            object maDauSachObj = row.Cells["MaDauSach"]?.Value;
            if (maDauSachObj == null || maDauSachObj == DBNull.Value) return;  // Hoặc xử lý khác

            int maDauSach = Convert.ToInt32(maDauSachObj);

            txtTenDauSach.Text = row.Cells["TenDauSach"]?.Value?.ToString() ?? "";

            object maTheLoaiObj = row.Cells["MaTheLoai"]?.Value;
            int maTheLoai = (maTheLoaiObj != null && maTheLoaiObj != DBNull.Value) ? Convert.ToInt32(maTheLoaiObj) : 0;

            object maNXBObj = row.Cells["MaNXB"]?.Value;
            int maNXB = (maNXBObj != null && maNXBObj != DBNull.Value) ? Convert.ToInt32(maNXBObj) : 0;

            object namXBObj = row.Cells["NamXuatBan"]?.Value;
            int? namXB = (namXBObj != null && namXBObj != DBNull.Value) ? (int?)Convert.ToInt32(namXBObj) : null;

            string ngonNgu = row.Cells["NgonNgu"]?.Value?.ToString() ?? "";

            object soTrangObj = row.Cells["SoTrang"]?.Value;
            int? soTrang = (soTrangObj != null && soTrangObj != DBNull.Value) ? (int?)Convert.ToInt32(soTrangObj) : null;

            object giaTienObj = row.Cells["GiaTien"]?.Value;
            decimal? giaTien = (giaTienObj != null && giaTienObj != DBNull.Value) ? (decimal?)Convert.ToDecimal(giaTienObj) : null;

            string moTa = "";
            if (dgvDauSach.Columns.Contains("MoTa"))
            {
                var moTaCell = row.Cells["MoTa"];
                if (moTaCell != null && moTaCell.Value != null && moTaCell.Value != DBNull.Value)
                    moTa = moTaCell.Value.ToString();
            }

            // Gán lên textbox
            txtNamXB.Text = namXB?.ToString() ?? "";
            txtNgonNgu.Text = ngonNgu;
            txtSoTrang.Text = soTrang?.ToString() ?? "";
            txtGiaTien.Text = giaTien?.ToString("N0") ?? "";
            txtMoTa.Text = moTa;

            // Gán comboBox
            cbTheLoai.SelectedValue = maTheLoai;
            cbNXB.SelectedValue = maNXB;

            // CheckedListBox xử lý tác giả
            for (int i = 0; i < clbTacGia.Items.Count; i++)
                clbTacGia.SetItemChecked(i, false);

            List<int> danhSachTacGia = dstgRepository.LayDanhSachMaTacGiaTheoDauSach(maDauSach);
            for (int i = 0; i < clbTacGia.Items.Count; i++)
            {
                TacGia tg = (TacGia)clbTacGia.Items[i];
                if (danhSachTacGia.Contains(tg.MaTacGia))
                    clbTacGia.SetItemChecked(i, true);
            }

            this.maDauSach = maDauSach;
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            string tenDauSach = txtTenDauSach.Text.Trim();

            if (string.IsNullOrEmpty(tenDauSach))
            {
                MessageBox.Show("Vui lòng nhập tên đầu sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbTheLoai.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn thể loại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbNXB.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn nhà xuất bản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maTheLoai = (int)cbTheLoai.SelectedValue;
            int maNXB = (int)cbNXB.SelectedValue;

            // Lấy dữ liệu mới từ các textbox và parse sang kiểu phù hợp
            int? namXuatBan = null;
            if (int.TryParse(txtNamXB.Text.Trim(), out int tempNam))
            {
                namXuatBan = tempNam;
            }

            decimal? giaTien = null;
            if (decimal.TryParse(txtGiaTien.Text.Trim(), out decimal tempGia))
            {
                giaTien = tempGia;
            }

            int? soTrang = null;
            if (int.TryParse(txtSoTrang.Text.Trim(), out int tempSoTrang))
            {
                soTrang = tempSoTrang;
            }

            string ngonNgu = txtNgonNgu.Text.Trim();
            string mota = txtMoTa.Text.Trim();

            Console.WriteLine(mota);
            try
            {
                // Gọi phương thức AddDauSach với đủ tham số
                int maDauSach = dsRepository.AddDauSach(tenDauSach, maTheLoai, maNXB, namXuatBan, giaTien, soTrang, ngonNgu, mota);

                foreach (var item in clbTacGia.CheckedItems)
                {
                    int maTacGia = ((TacGia)item).MaTacGia;
                    dstgRepository.AddTacGiaChoDauSach(maDauSach, maTacGia);
                }

                MessageBox.Show("Thêm đầu sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm đầu sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            string tenDauSach = txtTenDauSach.Text.Trim();

            if (string.IsNullOrEmpty(tenDauSach))
            {
                MessageBox.Show("Vui lòng nhập tên đầu sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbTheLoai.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn thể loại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbNXB.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn nhà xuất bản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maTheLoai = (int)cbTheLoai.SelectedValue;
            int maNXB = (int)cbNXB.SelectedValue;

            // Lấy dữ liệu từ các textbox mới và chuyển đổi
            int? namXuatBan = null;
            if (int.TryParse(txtNamXB.Text.Trim(), out int tempNam))
            {
                namXuatBan = tempNam;
            }

            decimal? giaTien = null;
            if (decimal.TryParse(txtGiaTien.Text.Trim(), out decimal tempGia))
            {
                giaTien = tempGia;
            }

            int? soTrang = null;
            if (int.TryParse(txtSoTrang.Text.Trim(), out int tempSoTrang))
            {
                soTrang = tempSoTrang;
            }

            string ngonNgu = txtNgonNgu.Text.Trim();
            string mota = txtMoTa.Text.Trim();

            try
            {
                // Gọi hàm update với đầy đủ tham số
                dsRepository.UpdateDauSach(maDauSach, maTheLoai, maNXB, namXuatBan, giaTien, soTrang, ngonNgu, mota);

                // Cập nhật danh sách tác giả (xóa cũ, thêm mới)
                List<int> danhSachMaTacGiaMoi = new List<int>();
                foreach (var item in clbTacGia.CheckedItems)
                {
                    int maTacGia = ((TacGia)item).MaTacGia;
                    danhSachMaTacGiaMoi.Add(maTacGia);
                }
                dstgRepository.UpdateTacGiaChoDauSach(maDauSach, danhSachMaTacGiaMoi);

                MessageBox.Show("Cập nhật đầu sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật đầu sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng có muốn xóa hay không
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa đầu sách này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    // Giả sử bạn lấy mã đầu sách từ ô đang chọn trên DataGridView
                    int maDauSach = Convert.ToInt32(dgvDauSach.CurrentRow.Cells["MaDauSach"].Value);

                    // Gọi hàm xóa trong repository
                    dsRepository.DeleteDauSach(maDauSach);

                    MessageBox.Show("Xóa đầu sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Load lại dữ liệu lên DataGridView sau khi xóa
                    LoadDauSachToDgv(); // Hàm này là hàm bạn dùng để load lại danh sách đầu sách
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa đầu sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            try
            {
                // Gọi hàm tìm kiếm đầu sách theo từ khóa
                var list = dsRepository.SearchDauSach(keyword);

                // Gán dữ liệu lên DataGridView
                dgvDauSach.DataSource = null;
                dgvDauSach.DataSource = list;

                // Ẩn các cột ID không cần hiển thị
                if (dgvDauSach.Columns.Contains("MaDauSach"))
                    dgvDauSach.Columns["MaDauSach"].Visible = false;
                if (dgvDauSach.Columns.Contains("MaTheLoai"))
                    dgvDauSach.Columns["MaTheLoai"].Visible = false;
                if (dgvDauSach.Columns.Contains("MaNXB"))
                    dgvDauSach.Columns["MaNXB"].Visible = false;

                // Tùy chỉnh header nếu muốn
                if (dgvDauSach.Columns.Contains("TenDauSach"))
                    dgvDauSach.Columns["TenDauSach"].HeaderText = "Tên Đầu Sách";
                if (dgvDauSach.Columns.Contains("TenTheLoai"))
                    dgvDauSach.Columns["TenTheLoai"].HeaderText = "Thể Loại";
                if (dgvDauSach.Columns.Contains("TenNXB"))
                    dgvDauSach.Columns["TenNXB"].HeaderText = "Nhà Xuất Bản";
                if (dgvDauSach.Columns.Contains("TacGia"))
                    dgvDauSach.Columns["TacGia"].HeaderText = "Tác Giả";
                if (dgvDauSach.Columns.Contains("NamXuatBan"))
                    dgvDauSach.Columns["NamXuatBan"].HeaderText = "Năm Xuất Bản";
                if (dgvDauSach.Columns.Contains("NgonNgu"))
                    dgvDauSach.Columns["NgonNgu"].HeaderText = "Ngôn Ngữ";
                if (dgvDauSach.Columns.Contains("SoTrang"))
                    dgvDauSach.Columns["SoTrang"].HeaderText = "Số Trang";
                if (dgvDauSach.Columns.Contains("GiaTien"))
                    dgvDauSach.Columns["GiaTien"].HeaderText = "Giá Tiền";
                if (dgvDauSach.Columns.Contains("MoTa"))
                    dgvDauSach.Columns["MoTa"].HeaderText = "Mô Tả";

                dgvDauSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                if (list.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy đầu sách phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm đầu sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
