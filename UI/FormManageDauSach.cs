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
            this.Size = new Size(700, 500);

            lblTitle = new Label()
            {
                Text = "Quản lý Đầu Sách",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = mainColor,
                Location = new Point(250, 10),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Tên đầu sách
            Label lblTenDauSach = new Label()
            {
                Text = "Tên Đầu Sách:",
                Location = new Point(20, 60),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTenDauSach);

            txtTenDauSach = new TextBox()
            {
                Location = new Point(130, 57),
                Width = 300,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtTenDauSach);

            // Thể loại
            Label lblTheLoai = new Label()
            {
                Text = "Thể Loại:",
                Location = new Point(20, 95),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTheLoai);

            cbTheLoai = new ComboBox()
            {
                Location = new Point(130, 92),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(cbTheLoai);

            // Nhà xuất bản
            Label lblNXB = new Label()
            {
                Text = "Nhà Xuất Bản:",
                Location = new Point(20, 130),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblNXB);

            cbNXB = new ComboBox()
            {
                Location = new Point(130, 127),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(cbNXB);

            // Tác giả
            Label lblTacGia = new Label()
            {
                Text = "Tác Giả:",
                Location = new Point(460, 95),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTacGia);

            clbTacGia = new CheckedListBox()
            {
                Location = new Point(520, 92),
                Size = new Size(140, 95),
                Font = new Font("Segoe UI", 10),
                CheckOnClick = true
            };
            this.Controls.Add(clbTacGia);


            // Ô tìm kiếm
            txtSearch = new TextBox()
            {
                Location = new Point(130, 165),
                Width = 200,
                Font = new Font("Segoe UI", 10),
            };
            this.Controls.Add(txtSearch);

            // Nút Tìm kiếm
            btnSearch = new Button()
            {
                Text = "Tìm kiếm",
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Location = new Point(340, 163),
                Size = new Size(90, 30),
                FlatStyle = FlatStyle.Flat
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;
            this.Controls.Add(btnSearch);

            // Các nút Thêm, Sửa, Xóa
            btnThem = new Button()
            {
                Text = "Thêm",
                BackColor = mainColor,
                ForeColor = Color.White,
                Location = new Point(450, 55),
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
                Location = new Point(520, 55),
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
                Location = new Point(450, 130),
                Size = new Size(60, 30),
                FlatStyle = FlatStyle.Flat,
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.Click += BtnXoa_Click;
            this.Controls.Add(btnXoa);

            // Bảng danh sách đầu sách
            dgvDauSach = new DataGridView()
            {
                Location = new Point(20, 210),
                Size = new Size(640, 230),
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

                // Gán danh sách làm nguồn dữ liệu cho dgv
                dgvDauSach.DataSource = null;
                dgvDauSach.DataSource = list;

                // Tùy chỉnh hiển thị cột (nếu muốn)
                dgvDauSach.Columns["MaDauSach"].HeaderText = "Mã Đầu Sách";
                dgvDauSach.Columns["TenDauSach"].HeaderText = "Tên Đầu Sách";
                dgvDauSach.Columns["TenTheLoai"].HeaderText = "Thể Loại";
                dgvDauSach.Columns["TenNXB"].HeaderText = "Nhà Xuất Bản";
                dgvDauSach.Columns["TacGia"].HeaderText = "Tác Giả";

                // Ẩn hoặc sửa kích thước cột nếu cần
                dgvDauSach.Columns["MaDauSach"].Visible = false; // Nếu bạn không muốn hiển thị mã
                dgvDauSach.Columns["MaTheLoai"].Visible = false;
                dgvDauSach.Columns["MaNXB"].Visible = false;
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
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvDauSach.Rows[e.RowIndex];

            int maDauSach = Convert.ToInt32(row.Cells["MaDauSach"].Value);

            txtTenDauSach.Text = row.Cells["TenDauSach"].Value.ToString();

            // Lấy MaTheLoai và MaNXB dưới dạng int
            int maTheLoai = Convert.ToInt32(row.Cells["MaTheLoai"].Value);
            int maNXB = Convert.ToInt32(row.Cells["MaNXB"].Value);

            cbTheLoai.SelectedValue = maTheLoai;
            cbNXB.SelectedValue = maNXB;

            // Bỏ chọn tất cả tác giả trước
            for (int i = 0; i < clbTacGia.Items.Count; i++)
            {
                clbTacGia.SetItemChecked(i, false);
            }

            // Lấy danh sách mã tác giả của đầu sách này từ DB
            List<int> danhSachTacGia = dstgRepository.LayDanhSachMaTacGiaTheoDauSach(maDauSach);

            // Đánh dấu các tác giả tương ứng trong clbTacGia
            for (int i = 0; i < clbTacGia.Items.Count; i++)
            {
                TacGia tg = (TacGia)clbTacGia.Items[i];
                if (danhSachTacGia.Contains(tg.MaTacGia))
                {
                    clbTacGia.SetItemChecked(i, true);
                }
            }

            // Lưu lại MaDauSach để sử dụng khi bấm nút "Sửa"
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

            try
            {
                int maDauSach = dsRepository.AddDauSach(tenDauSach, maTheLoai, maNXB);

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

            try
            {
                // Cập nhật thông tin đầu sách
                dsRepository.UpdateDauSach(maDauSach, maTheLoai, maNXB);

                // Chuẩn bị danh sách tác giả được chọn
                List<int> danhSachMaTacGiaMoi = new List<int>();
                foreach (var item in clbTacGia.CheckedItems)
                {
                    int maTacGia = ((TacGia)item).MaTacGia;
                    danhSachMaTacGiaMoi.Add(maTacGia);
                }

                // Cập nhật danh sách tác giả (xóa cũ, thêm mới)
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
                dgvDauSach.Columns["MaDauSach"].Visible = false; // Nếu bạn không muốn hiển thị mã
                dgvDauSach.Columns["MaTheLoai"].Visible = false;
                dgvDauSach.Columns["MaNXB"].Visible = false;

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
