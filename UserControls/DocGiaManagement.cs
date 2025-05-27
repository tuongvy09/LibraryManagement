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
        private List<DocGiaDTO> currentData;
        private string placeholderText = "Nhập tên, số điện thoại hoặc CCCD...";
        private bool dataLoaded = false;

        public DocGiaManagement()
        {
            InitializeComponent();

            // Đăng ký events
            dgvDocGia.DataBindingComplete += DgvDocGia_DataBindingComplete;
            this.Load += DocGiaManagement_Load;

            // Setup placeholder text
            txtSearch.Text = placeholderText;
            txtSearch.ForeColor = Color.Gray;

            // Gán sự kiện
            txtSearch.Enter += TxtSearch_Enter;
            txtSearch.Leave += TxtSearch_Leave;
            txtSearch.KeyDown += TxtSearch_KeyDown;
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
                txtSearch.Text = searchText;
                txtSearch.ForeColor = Color.Black;
                SearchDocGia();
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
                if (dgvDocGia?.Columns == null) return;

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
                MessageBox.Show("Vui lòng chọn độc giả cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hoTen = dgvDocGia.CurrentRow.Cells["HoTen"].Value.ToString();
            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa độc giả '{hoTen}'?\n\nLưu ý: Thao tác này không thể hoàn tác!",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            ClearSearch();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchDocGia();
        }

        private void SearchDocGia()
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
                    // SetupColumnHeaders sẽ được gọi trong event DataBindingComplete
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

        private void DgvDocGia_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Có thể thêm logic xử lý click vào cell nếu cần
        }
    }
}