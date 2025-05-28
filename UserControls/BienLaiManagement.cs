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
    public partial class BienLaiManagement : UserControl
    {
        private readonly BienLaiDAO _bienLaiDAO = new BienLaiDAO();
        private List<BienLai> currentData;

        public BienLaiManagement()
        {
            InitializeComponent();
            LoadBienLaiData();
            txtSearch.Text = "Nhập mã biên lai, tên độc giả hoặc hình thức thanh toán...";
            txtSearch.ForeColor = Color.Gray;

            // Gán sự kiện
            txtSearch.Enter += TxtSearch_Enter;
            txtSearch.Leave += TxtSearch_Leave;
            txtSearch.KeyDown += TxtSearch_KeyDown;
        }

        private void LoadBienLaiData()
        {
            try
            {
                currentData = _bienLaiDAO.GetAllBienLai();

                var displayData = currentData.Select(x => new
                {
                    MaBienLai = x.MaBienLai,
                    TenDocGia = x.TenDocGia,
                    SoDT = x.SoDT,
                    NgayTraTT = x.NgayTraTT.ToString("dd/MM/yyyy"),
                    HinhThucThanhToan = x.HinhThucThanhToan,
                    TienTra = string.Format("{0:N0} VNĐ", x.TienTra)
                }).ToList();

                dgvBienLai.DataSource = displayData;
                SetupColumnHeaders();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupColumnHeaders()
        {
            if (dgvBienLai.Columns["MaBienLai"] != null)
                dgvBienLai.Columns["MaBienLai"].HeaderText = "Mã biên lai";
            if (dgvBienLai.Columns["TenDocGia"] != null)
                dgvBienLai.Columns["TenDocGia"].HeaderText = "Tên độc giả";
            if (dgvBienLai.Columns["SoDT"] != null)
                dgvBienLai.Columns["SoDT"].HeaderText = "Số ĐT";
            if (dgvBienLai.Columns["NgayTraTT"] != null)
                dgvBienLai.Columns["NgayTraTT"].HeaderText = "Ngày thanh toán";
            if (dgvBienLai.Columns["HinhThucThanhToan"] != null)
                dgvBienLai.Columns["HinhThucThanhToan"].HeaderText = "Hình thức TT";
            if (dgvBienLai.Columns["TienTra"] != null)
                dgvBienLai.Columns["TienTra"].HeaderText = "Số tiền";
        }

        private void TxtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Nhập mã biên lai, tên độc giả hoặc hình thức thanh toán...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Nhập mã biên lai, tên độc giả hoặc hình thức thanh toán...";
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

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchBienLai();
        }

        private void SearchBienLai()
        {
            try
            {
                string searchText = txtSearch.Text.Trim();
                if (searchText == "Nhập mã biên lai, tên độc giả hoặc hình thức thanh toán..." || string.IsNullOrEmpty(searchText))
                {
                    LoadBienLaiData();
                }
                else
                {
                    var searchResults = _bienLaiDAO.SearchBienLai(searchText);
                    var displayData = searchResults.Select(x => new
                    {
                        MaBienLai = x.MaBienLai,
                        TenDocGia = x.TenDocGia,
                        SoDT = x.SoDT,
                        NgayTraTT = x.NgayTraTT.ToString("dd/MM/yyyy"),
                        HinhThucThanhToan = x.HinhThucThanhToan,
                        TienTra = string.Format("{0:N0} VNĐ", x.TienTra)
                    }).ToList();

                    dgvBienLai.DataSource = displayData;
                    currentData = searchResults;
                    SetupColumnHeaders();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var formAdd = new FormAddBienLai())
            {
                if (formAdd.ShowDialog() == DialogResult.OK)
                {
                    LoadBienLaiData();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvBienLai.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn biên lai cần sửa!", "Thông báo",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maBienLai = Convert.ToInt32(dgvBienLai.CurrentRow.Cells["MaBienLai"].Value);

            using (var formEdit = new FormEditBienLai(maBienLai))
            {
                if (formEdit.ShowDialog() == DialogResult.OK)
                {
                    LoadBienLaiData();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvBienLai.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn biên lai cần xóa!", "Thông báo",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa biên lai này?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int maBienLai = Convert.ToInt32(dgvBienLai.CurrentRow.Cells["MaBienLai"].Value);
                    if (_bienLaiDAO.DeleteBienLai(maBienLai))
                    {
                        MessageBox.Show("Xóa biên lai thành công!", "Thành công",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadBienLaiData();
                    }
                    else
                    {
                        MessageBox.Show("Xóa biên lai thất bại!", "Lỗi",
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
            txtSearch.Text = "Nhập mã biên lai, tên độc giả hoặc hình thức thanh toán...";
            txtSearch.ForeColor = Color.Gray;
            LoadBienLaiData();
        }

        private void DgvBienLai_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Có thể thêm logic xử lý click vào cell nếu cần
        }

        private void DgvBienLai_DoubleClick(object sender, EventArgs e)
        {
            BtnEdit_Click(sender, e);
        }
    }
}
