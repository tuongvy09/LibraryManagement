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

        public ThuThuManagement()
        {
            InitializeComponent();

            // Đăng ký events
            dgvThuThu.DataBindingComplete += DgvThuThu_DataBindingComplete;
            this.Load += ThuThuManagement_Load;

            // Setup placeholder text
            txtSearch.Text = placeholderText;
            txtSearch.ForeColor = Color.Gray;

            // Gán sự kiện
            txtSearch.Enter += TxtSearch_Enter;
            txtSearch.Leave += TxtSearch_Leave;
            txtSearch.KeyDown += TxtSearch_KeyDown;
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
                txtSearch.Text = searchText;
                txtSearch.ForeColor = Color.Black;
                SearchThuThu();
            }
        }

        // Phương thức công khai để clear search và reload data
        public void ClearSearch()
        {
            if (this.IsHandleCreated || this.Created)
            {
                txtSearch.Text = placeholderText;
                txtSearch.ForeColor = Color.Gray;
                LoadData();
            }
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
                    Email = string.IsNullOrEmpty(tt.Email) ? "Chưa cập nhật" : tt.Email,
                    SoDienThoai = string.IsNullOrEmpty(tt.SoDienThoai) ? "Chưa cập nhật" : tt.SoDienThoai,
                    DiaChi = string.IsNullOrEmpty(tt.DiaChi) ? "Chưa cập nhật" : tt.DiaChi,
                    NgaySinh = tt.NgaySinh.ToString("dd/MM/yyyy"),
                    NgayBatDauLam = tt.NgayBatDauLam.ToString("dd/MM/yyyy"),
                    GioiTinh = tt.GioiTinh == "M" ? "Nam" : tt.GioiTinh == "F" ? "Nữ" : "Chưa xác định",
                    TrangThai = tt.TrangThai ? "Hoạt động" : "Ngừng hoạt động"
                }).ToList();

                dgvThuThu.DataSource = displayData;
                dataLoaded = true;
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
                }
                if (dgvThuThu.Columns["NgayBatDauLam"] != null)
                {
                    dgvThuThu.Columns["NgayBatDauLam"].HeaderText = "Ngày bắt đầu làm";
                    dgvThuThu.Columns["NgayBatDauLam"].Width = 130;
                }
                if (dgvThuThu.Columns["GioiTinh"] != null)
                {
                    dgvThuThu.Columns["GioiTinh"].HeaderText = "Giới tính";
                    dgvThuThu.Columns["GioiTinh"].Width = 80;
                }
                if (dgvThuThu.Columns["TrangThai"] != null)
                {
                    dgvThuThu.Columns["TrangThai"].HeaderText = "Trạng thái";
                    dgvThuThu.Columns["TrangThai"].Width = 100;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi setup column headers: {ex.Message}");
            }
        }

        // Xử lý placeholder text
        private void TxtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == placeholderText && txtSearch.ForeColor == Color.Gray)
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = placeholderText;
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearch_Click(sender, e);
            }
        }

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
                MessageBox.Show("Vui lòng chọn thủ thư cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenThuThu = dgvThuThu.CurrentRow.Cells["TenThuThu"].Value.ToString();
            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa thủ thư '{tenThuThu}'?\n\nLưu ý: Thao tác này không thể hoàn tác!",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
            ClearSearch();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchThuThu();
        }

        private void SearchThuThu()
        {
            try
            {
                string searchText = txtSearch.Text.Trim();

                if (searchText == placeholderText || string.IsNullOrEmpty(searchText))
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
                        Email = string.IsNullOrEmpty(tt.Email) ? "Chưa cập nhật" : tt.Email,
                        SoDienThoai = string.IsNullOrEmpty(tt.SoDienThoai) ? "Chưa cập nhật" : tt.SoDienThoai,
                        DiaChi = string.IsNullOrEmpty(tt.DiaChi) ? "Chưa cập nhật" : tt.DiaChi,
                        NgaySinh = tt.NgaySinh.ToString("dd/MM/yyyy"),
                        NgayBatDauLam = tt.NgayBatDauLam.ToString("dd/MM/yyyy"),
                        GioiTinh = tt.GioiTinh == "M" ? "Nam" : tt.GioiTinh == "F" ? "Nữ" : "Chưa xác định",
                        TrangThai = tt.TrangThai ? "Hoạt động" : "Ngừng hoạt động"
                    }).ToList();

                    dgvThuThu.DataSource = displayData;
                    currentData = searchResults;
                    // SetupColumnHeaders sẽ được gọi trong event DataBindingComplete
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

        private void DgvThuThu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Có thể thêm logic xử lý click vào cell nếu cần
        }
    }
}
