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
        private readonly ThuThuDAO thuThuDAO = new ThuThuDAO();
        private List<ThuThu> currentData;
        private string placeholderText = "Nhập tên, email hoặc số điện thoại...";
        private bool dataLoaded = false;
        private bool isPlaceholderActive = true; // Track placeholder state

        public ThuThuManagement()
        {
            InitializeComponent();

            // Đăng ký events
            dgvThuThu.DataBindingComplete += DgvThuThu_DataBindingComplete;
            this.Load += ThuThuManagement_Load;

            // Setup placeholder text
            InitializePlaceholder();
        }

        // Khởi tạo placeholder text
        private void InitializePlaceholder()
        {
            SetPlaceholder();

            // Gán sự kiện
            txtSearch.Enter += TxtSearch_Enter;
            txtSearch.Leave += TxtSearch_Leave;
            txtSearch.KeyDown += TxtSearch_KeyDown;
            txtSearch.TextChanged += TxtSearch_TextChanged;
        }

        // Thiết lập placeholder
        private void SetPlaceholder()
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text) || isPlaceholderActive)
            {
                txtSearch.Text = placeholderText;
                txtSearch.ForeColor = Color.Gray;
                txtSearch.Font = new Font(txtSearch.Font, FontStyle.Italic);
                isPlaceholderActive = true;
            }
        }

        // Xóa placeholder
        private void ClearPlaceholder()
        {
            if (isPlaceholderActive)
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
                txtSearch.Font = new Font(txtSearch.Font, FontStyle.Regular);
                isPlaceholderActive = false;
            }
        }

        // Event handler cho Load
        private void ThuThuManagement_Load(object sender, EventArgs e)
        {
            if (!dataLoaded)
            {
                LoadData();
            }
        }

        // Event handler cho DataBindingComplete
        private void DgvThuThu_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            SetupColumnHeaders();
        }

        // Override OnVisibleChanged để load data khi UserControl được hiển thị
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (this.Visible && this.IsHandleCreated && !dataLoaded)
            {
                LoadData();
            }
        }

        // Override SetVisibleCore để load data khi UserControl được hiển thị
        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(value);

            if (value && this.Created && !dataLoaded)
            {
                // Delay load để đảm bảo UserControl đã được khởi tạo hoàn toàn
                this.BeginInvoke(new Action(() => {
                    if (!dataLoaded)
                    {
                        LoadData();
                    }
                }));
            }
        }

        // Phương thức công khai để khởi tạo data từ Form cha
        public void InitializeData()
        {
            if (!dataLoaded)
            {
                LoadData();
            }
        }

        // Phương thức công khai để refresh dữ liệu từ bên ngoài
        public void RefreshData()
        {
            if (this.IsHandleCreated || this.Created)
            {
                LoadData();
            }
        }

        // Phương thức công khai để tìm kiếm từ bên ngoài
        public void SearchData(string searchText)
        {
            if (this.IsHandleCreated || this.Created)
            {
                ClearPlaceholder();
                txtSearch.Text = searchText;
                txtSearch.ForeColor = Color.Black;
                txtSearch.Font = new Font(txtSearch.Font, FontStyle.Regular);
                isPlaceholderActive = false;
                SearchThuThu();
            }
        }

        // Phương thức công khai để clear search và reload data
        public void ClearSearch()
        {
            if (this.IsHandleCreated || this.Created)
            {
                SetPlaceholder();
                LoadData();
            }
        }

        private void LoadData()
        {
            try
            {
                currentData = thuThuDAO.GetAllThuThu();

                // ✅ Sắp xếp: Hoạt động trước, Ngừng hoạt động sau, trong mỗi nhóm sắp xếp theo tên
                var displayData = currentData
                    .OrderByDescending(tt => tt.TrangThai) // True (Hoạt động) trước, False (Ngừng hoạt động) sau
                    .ThenBy(tt => tt.TenThuThu) // Sắp xếp theo tên trong mỗi nhóm
                    .Select(tt => new
                    {
                        MaThuThu = tt.MaThuThu,
                        TenThuThu = tt.TenThuThu,
                        Email = string.IsNullOrEmpty(tt.Email) ? "Chưa cập nhật" : tt.Email,
                        SoDienThoai = string.IsNullOrEmpty(tt.SoDienThoai) ? "Chưa cập nhật" : tt.SoDienThoai,
                        DiaChi = string.IsNullOrEmpty(tt.DiaChi) ? "Chưa cập nhật" : tt.DiaChi,
                        NgaySinh = tt.NgaySinh.ToString("dd/MM/yyyy"),
                        NgayBatDauLam = tt.NgayBatDauLam.ToString("dd/MM/yyyy"),
                        GioiTinh = tt.GioiTinh == "M" ? "Nam" :
                                  tt.GioiTinh == "F" ? "Nữ" :
                                  tt.GioiTinh == "Nam" ? "Nam" :
                                  tt.GioiTinh == "Nữ" ? "Nữ" : "Chưa xác định",
                        TrangThai = tt.TrangThai ? "Hoạt động" : "Ngừng hoạt động"
                    }).ToList();

                dgvThuThu.DataSource = displayData;
                dataLoaded = true;
                ResetTitle(); // ✅ Reset title khi load data
                // SetupColumnHeaders sẽ được gọi trong event DataBindingComplete
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupColumnHeaders()
        {
            try
            {
                if (dgvThuThu?.Columns == null) return;

                if (dgvThuThu.Columns["MaThuThu"] != null)
                {
                    dgvThuThu.Columns["MaThuThu"].HeaderText = "Mã TT";
                    dgvThuThu.Columns["MaThuThu"].Width = 70;
                    dgvThuThu.Columns["MaThuThu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvThuThu.Columns["TenThuThu"] != null)
                {
                    dgvThuThu.Columns["TenThuThu"].HeaderText = "Tên thủ thư";
                    dgvThuThu.Columns["TenThuThu"].Width = 150;
                }
                if (dgvThuThu.Columns["Email"] != null)
                {
                    dgvThuThu.Columns["Email"].HeaderText = "Email";
                    dgvThuThu.Columns["Email"].Width = 200;
                }
                if (dgvThuThu.Columns["SoDienThoai"] != null)
                {
                    dgvThuThu.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                    dgvThuThu.Columns["SoDienThoai"].Width = 120;
                    dgvThuThu.Columns["SoDienThoai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvThuThu.Columns["DiaChi"] != null)
                {
                    dgvThuThu.Columns["DiaChi"].HeaderText = "Địa chỉ";
                    dgvThuThu.Columns["DiaChi"].Width = 180;
                }
                if (dgvThuThu.Columns["NgaySinh"] != null)
                {
                    dgvThuThu.Columns["NgaySinh"].HeaderText = "Ngày sinh";
                    dgvThuThu.Columns["NgaySinh"].Width = 100;
                    dgvThuThu.Columns["NgaySinh"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvThuThu.Columns["NgayBatDauLam"] != null)
                {
                    dgvThuThu.Columns["NgayBatDauLam"].HeaderText = "Ngày bắt đầu làm";
                    dgvThuThu.Columns["NgayBatDauLam"].Width = 130;
                    dgvThuThu.Columns["NgayBatDauLam"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvThuThu.Columns["GioiTinh"] != null)
                {
                    dgvThuThu.Columns["GioiTinh"].HeaderText = "Giới tính";
                    dgvThuThu.Columns["GioiTinh"].Width = 80;
                    dgvThuThu.Columns["GioiTinh"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvThuThu.Columns["TrangThai"] != null)
                {
                    dgvThuThu.Columns["TrangThai"].HeaderText = "Trạng thái";
                    dgvThuThu.Columns["TrangThai"].Width = 100;
                    dgvThuThu.Columns["TrangThai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // ✅ Highlight các dòng "Ngừng hoạt động"
                foreach (DataGridViewRow row in dgvThuThu.Rows)
                {
                    if (row.Cells["TrangThai"]?.Value?.ToString() == "Ngừng hoạt động")
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250); // Màu xám nhạt
                        row.DefaultCellStyle.ForeColor = Color.Gray; // Chữ màu xám
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi setup column headers: {ex.Message}");
            }
        }

        // ===== XỬ LÝ PLACEHOLDER TEXT =====
        private void TxtSearch_Enter(object sender, EventArgs e)
        {
            ClearPlaceholder();
        }

        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                SetPlaceholder();
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            // Nếu user đang nhập và không phải placeholder
            if (!isPlaceholderActive && txtSearch.Focused)
            {
                txtSearch.ForeColor = Color.Black;
                txtSearch.Font = new Font(txtSearch.Font, FontStyle.Regular);
            }
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearch_Click(sender, e);
                e.Handled = true; // Ngăn tiếng beep
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                BtnRefresh_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        // ===== CÁC BUTTON EVENTS =====
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var formAdd = new FormAddEditThuThu())
            {
                if (formAdd.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                    MessageBox.Show("Thêm thủ thư thành công!", "Thông báo",
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
                        MessageBox.Show("Cập nhật thông tin thủ thư thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvThuThu.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn thủ thư cần vô hiệu hóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenThuThu = dgvThuThu.CurrentRow.Cells["TenThuThu"].Value.ToString();
            string trangThaiHienTai = dgvThuThu.CurrentRow.Cells["TrangThai"].Value.ToString();

            // ✅ Kiểm tra trạng thái hiện tại để hiển thị thông báo phù hợp
            if (trangThaiHienTai == "Ngừng hoạt động")
            {
                var result = MessageBox.Show($"Thủ thư '{tenThuThu}' đã được vô hiệu hóa trước đó.\n\nBạn có muốn kích hoạt lại tài khoản này không?",
                    "Kích hoạt lại tài khoản", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int maThuThu = Convert.ToInt32(dgvThuThu.CurrentRow.Cells["MaThuThu"].Value);
                        if (thuThuDAO.DeleteThuThu(maThuThu)) // Method này sẽ toggle trạng thái
                        {
                            MessageBox.Show($"Đã kích hoạt lại tài khoản thủ thư '{tenThuThu}' thành công!", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Kích hoạt lại tài khoản thất bại!", "Lỗi",
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
            else
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn vô hiệu hóa tài khoản thủ thư '{tenThuThu}'?\n\n" +
                    "Lưu ý: Thủ thư sẽ không thể truy cập hệ thống, nhưng dữ liệu sẽ được lưu trữ.",
                    "Xác nhận vô hiệu hóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int maThuThu = Convert.ToInt32(dgvThuThu.CurrentRow.Cells["MaThuThu"].Value);
                        if (thuThuDAO.DeleteThuThu(maThuThu))
                        {
                            MessageBox.Show($"Đã vô hiệu hóa tài khoản thủ thư '{tenThuThu}' thành công!", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Vô hiệu hóa tài khoản thất bại!", "Lỗi",
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
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            ClearSearch();
            MessageBox.Show("Đã làm mới dữ liệu!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchThuThu();
        }

        private void SearchThuThu()
        {
            try
            {
                // Kiểm tra nếu đang hiển thị placeholder
                if (isPlaceholderActive || string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    LoadData();
                    return;
                }

                string searchText = txtSearch.Text.Trim();
                var searchResults = thuThuDAO.SearchThuThu(searchText);

                // ✅ Sắp xếp kết quả tìm kiếm tương tự như LoadData
                var displayData = searchResults
                    .OrderByDescending(tt => tt.TrangThai) // True (Hoạt động) trước, False (Ngừng hoạt động) sau
                    .ThenBy(tt => tt.TenThuThu) // Sắp xếp theo tên trong mỗi nhóm
                    .Select(tt => new
                    {
                        MaThuThu = tt.MaThuThu,
                        TenThuThu = tt.TenThuThu,
                        Email = string.IsNullOrEmpty(tt.Email) ? "Chưa cập nhật" : tt.Email,
                        SoDienThoai = string.IsNullOrEmpty(tt.SoDienThoai) ? "Chưa cập nhật" : tt.SoDienThoai,
                        DiaChi = string.IsNullOrEmpty(tt.DiaChi) ? "Chưa cập nhật" : tt.DiaChi,
                        NgaySinh = tt.NgaySinh.ToString("dd/MM/yyyy"),
                        NgayBatDauLam = tt.NgayBatDauLam.ToString("dd/MM/yyyy"),
                        GioiTinh = tt.GioiTinh == "M" ? "Nam" :
                                  tt.GioiTinh == "F" ? "Nữ" :
                                  tt.GioiTinh == "Nam" ? "Nam" :
                                  tt.GioiTinh == "Nữ" ? "Nữ" : "Chưa xác định",
                        TrangThai = tt.TrangThai ? "Hoạt động" : "Ngừng hoạt động"
                    }).ToList();

                dgvThuThu.DataSource = displayData;
                currentData = searchResults;

                // Hiển thị kết quả tìm kiếm
                if (searchResults.Count == 0)
                {
                    MessageBox.Show($"Không tìm thấy thủ thư nào với từ khóa '{searchText}'!", "Kết quả tìm kiếm",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblTitle.Text = $"QUẢN LÝ THỦ THƯ - Tìm thấy {searchResults.Count} kết quả";
                }
                // SetupColumnHeaders sẽ được gọi trong event DataBindingComplete
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

        private void DgvThuThu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Có thể thêm logic xử lý click vào cell nếu cần
        }

        // Reset title khi clear search
        private void ResetTitle()
        {
            lblTitle.Text = "QUẢN LÝ THỦ THƯ";
        }
    }
}
