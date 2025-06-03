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
        private readonly DocGiaDAO docGiaDAO = new DocGiaDAO();
        private readonly ThongKeDAO thongKeDAO = new ThongKeDAO();
        private List<DocGiaDTO> currentData;
        private string placeholderText = "Nhập tên, số điện thoại hoặc CCCD...";
        private bool dataLoaded = false;
        private bool isPlaceholderActive = true; // Track placeholder state

        public DocGiaManagement()
        {
            InitializeComponent();
            InitializePlaceholder();

            // Đăng ký events
            dgvDocGia.DataBindingComplete += DgvDocGia_DataBindingComplete;
            this.Load += DocGiaManagement_Load;
        }

        // Khởi tạo placeholder text
        private void InitializePlaceholder()
        {
            // Setup placeholder text
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
        private void DocGiaManagement_Load(object sender, EventArgs e)
        {
            if (!dataLoaded)
            {
                LoadData();
            }
        }

        // Event handler cho DataBindingComplete
        private void DgvDocGia_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
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
                SearchDocGia();
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
                currentData = docGiaDAO.GetAllDocGia();

                // ✅ Sắp xếp: Hoạt động trước, Ngừng hoạt động sau, trong mỗi nhóm sắp xếp theo tên
                var displayData = currentData
                    .OrderByDescending(dg => dg.TrangThai) // True (Hoạt động) trước, False (Ngừng hoạt động) sau
                    .ThenBy(dg => dg.HoTen) // Sắp xếp theo tên trong mỗi nhóm
                    .Select(dg => new
                    {
                        MaDocGia = dg.MaDocGia,
                        HoTen = dg.HoTen,
                        Tuoi = dg.Tuoi,
                        GioiTinh = dg.GioiTinh,
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
                dataLoaded = true;
                ResetTitle();
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
                if (dgvDocGia?.Columns == null) return;

                if (dgvDocGia.Columns["MaDocGia"] != null)
                    dgvDocGia.Columns["MaDocGia"].HeaderText = "Mã ĐG";
                if (dgvDocGia.Columns["HoTen"] != null)
                    dgvDocGia.Columns["HoTen"].HeaderText = "Họ tên";
                if (dgvDocGia.Columns["Tuoi"] != null)
                    dgvDocGia.Columns["Tuoi"].HeaderText = "Tuổi";
                if (dgvDocGia.Columns["GioiTinh"] != null)
                    dgvDocGia.Columns["GioiTinh"].HeaderText = "Giới tính";
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

                // Căn giữa cho một số cột
                if (dgvDocGia.Columns["MaDocGia"] != null)
                    dgvDocGia.Columns["MaDocGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (dgvDocGia.Columns["Tuoi"] != null)
                    dgvDocGia.Columns["Tuoi"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (dgvDocGia.Columns["GioiTinh"] != null)
                    dgvDocGia.Columns["GioiTinh"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (dgvDocGia.Columns["SoDT"] != null)
                    dgvDocGia.Columns["SoDT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (dgvDocGia.Columns["CCCD"] != null)
                    dgvDocGia.Columns["CCCD"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (dgvDocGia.Columns["NgayDangKy"] != null)
                    dgvDocGia.Columns["NgayDangKy"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (dgvDocGia.Columns["TienNo"] != null)
                    dgvDocGia.Columns["TienNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                if (dgvDocGia.Columns["TrangThai"] != null)
                    dgvDocGia.Columns["TrangThai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Thiết lập độ rộng cột phù hợp
                if (dgvDocGia.Columns["MaDocGia"] != null)
                    dgvDocGia.Columns["MaDocGia"].FillWeight = 60;
                if (dgvDocGia.Columns["HoTen"] != null)
                    dgvDocGia.Columns["HoTen"].FillWeight = 120;
                if (dgvDocGia.Columns["Tuoi"] != null)
                    dgvDocGia.Columns["Tuoi"].FillWeight = 50;
                if (dgvDocGia.Columns["GioiTinh"] != null)
                    dgvDocGia.Columns["GioiTinh"].FillWeight = 70;
                if (dgvDocGia.Columns["SoDT"] != null)
                    dgvDocGia.Columns["SoDT"].FillWeight = 90;
                if (dgvDocGia.Columns["CCCD"] != null)
                    dgvDocGia.Columns["CCCD"].FillWeight = 100;
                if (dgvDocGia.Columns["Email"] != null)
                    dgvDocGia.Columns["Email"].FillWeight = 130;
                if (dgvDocGia.Columns["DiaChi"] != null)
                    dgvDocGia.Columns["DiaChi"].FillWeight = 150;
                if (dgvDocGia.Columns["TenLoaiDG"] != null)
                    dgvDocGia.Columns["TenLoaiDG"].FillWeight = 80;
                if (dgvDocGia.Columns["NgayDangKy"] != null)
                    dgvDocGia.Columns["NgayDangKy"].FillWeight = 90;
                if (dgvDocGia.Columns["TienNo"] != null)
                    dgvDocGia.Columns["TienNo"].FillWeight = 80;
                if (dgvDocGia.Columns["TrangThai"] != null)
                    dgvDocGia.Columns["TrangThai"].FillWeight = 90;

                // ✅ Highlight các dòng "Ngừng hoạt động"
                foreach (DataGridViewRow row in dgvDocGia.Rows)
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

        // ===== PHẦN THỐNG KÊ =====
        private void BtnViewStats_Click(object sender, EventArgs e)
        {
            if (dgvDocGia.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một độc giả để xem thống kê chi tiết,\nhoặc vào mục 'Thống kê báo cáo' để xem tổng quan!",
                    "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                int maDocGia = Convert.ToInt32(dgvDocGia.CurrentRow.Cells["MaDocGia"].Value);
                string hoTen = dgvDocGia.CurrentRow.Cells["HoTen"].Value.ToString();
                ShowDocGiaStats(maDocGia, hoTen);
            }
        }

        private void ShowDocGiaStats(int maDocGia, string hoTen)
        {
            try
            {
                var stats = thongKeDAO.GetChiTietTienMuonDocGia(maDocGia);

                if (stats == null)
                {
                    MessageBox.Show($"Không có dữ liệu thống kê cho độc giả {hoTen}!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Tạo form hiển thị thống kê
                using (var formStats = new FormDocGiaStatsDetail(stats))
                {
                    formStats.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy thống kê: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            using (var formAdd = new FormAddEditDocGia())
            {
                if (formAdd.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                    MessageBox.Show("Thêm độc giả thành công!", "Thông báo",
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
                        MessageBox.Show("Cập nhật thông tin độc giả thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDocGia.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả cần vô hiệu hóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hoTen = dgvDocGia.CurrentRow.Cells["HoTen"].Value.ToString();
            string trangThaiHienTai = dgvDocGia.CurrentRow.Cells["TrangThai"].Value.ToString();

            // ✅ Kiểm tra trạng thái hiện tại để hiển thị thông báo phù hợp
            if (trangThaiHienTai == "Ngừng hoạt động")
            {
                var result = MessageBox.Show($"Độc giả '{hoTen}' đã được vô hiệu hóa trước đó.\n\nBạn có muốn kích hoạt lại tài khoản này không?",
                    "Kích hoạt lại tài khoản", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int maDocGia = Convert.ToInt32(dgvDocGia.CurrentRow.Cells["MaDocGia"].Value);
                        if (docGiaDAO.DeleteDocGia(maDocGia)) // Method này sẽ toggle trạng thái
                        {
                            MessageBox.Show($"Đã kích hoạt lại tài khoản độc giả '{hoTen}' thành công!", "Thành công",
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
                var result = MessageBox.Show($"Bạn có chắc chắn muốn vô hiệu hóa tài khoản độc giả '{hoTen}'?\n\n" +
                    "Lưu ý: Độc giả sẽ không thể mượn sách mới, nhưng dữ liệu sẽ được lưu trữ.",
                    "Xác nhận vô hiệu hóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int maDocGia = Convert.ToInt32(dgvDocGia.CurrentRow.Cells["MaDocGia"].Value);
                        if (docGiaDAO.DeleteDocGia(maDocGia))
                        {
                            MessageBox.Show($"Đã vô hiệu hóa tài khoản độc giả '{hoTen}' thành công!", "Thành công",
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
            SearchDocGia();
        }

        private void SearchDocGia()
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
                var searchResults = docGiaDAO.SearchDocGia(searchText);

                // ✅ Sắp xếp kết quả tìm kiếm tương tự như LoadData
                var displayData = searchResults
                    .OrderByDescending(dg => dg.TrangThai) // True (Hoạt động) trước, False (Ngừng hoạt động) sau
                    .ThenBy(dg => dg.HoTen) // Sắp xếp theo tên trong mỗi nhóm
                    .Select(dg => new
                    {
                        MaDocGia = dg.MaDocGia,
                        HoTen = dg.HoTen,
                        Tuoi = dg.Tuoi,
                        GioiTinh = dg.GioiTinh,
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

                // Hiển thị kết quả tìm kiếm
                if (searchResults.Count == 0)
                {
                    MessageBox.Show($"Không tìm thấy độc giả nào với từ khóa '{searchText}'!", "Kết quả tìm kiếm",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblTitle.Text = $"QUẢN LÝ ĐỘC GIẢ - Tìm thấy {searchResults.Count} kết quả";
                }
                // SetupColumnHeaders sẽ được gọi trong event DataBindingComplete
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

        private void DgvDocGia_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Có thể thêm logic xử lý click vào cell nếu cần
        }

        // Reset title khi clear search
        private void ResetTitle()
        {
            lblTitle.Text = "QUẢN LÝ ĐỘC GIẢ";
        }
    }
}