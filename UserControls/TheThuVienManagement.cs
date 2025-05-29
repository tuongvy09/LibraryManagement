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
using LibraryManagement.Utilities;  // NEW: For QR Code

namespace LibraryManagement.UserControls
{
    public partial class TheThuVienManagement : UserControl
    {
        private readonly TheThuVienDAO _theThuVienDAO = new TheThuVienDAO();
        private List<TheThuVien> currentData;

        public TheThuVienManagement()
        {
            InitializeComponent();
            LoadTheThuVienData();
            txtSearch.Text = "Nhập mã thẻ, tên độc giả hoặc số điện thoại...";
            txtSearch.ForeColor = Color.Gray;

            // Gán sự kiện
            txtSearch.Enter += TxtSearch_Enter;
            txtSearch.Leave += TxtSearch_Leave;
            txtSearch.KeyDown += TxtSearch_KeyDown;
        }

        private void LoadTheThuVienData()
        {
            try
            {
                currentData = _theThuVienDAO.GetAllTheThuVien();

                var displayData = currentData.Select(x => new
                {
                    MaThe = x.MaThe,
                    TenDocGia = x.TenDocGia,
                    SoDT = x.SoDT,
                    NgayCap = x.NgayCap.ToString("dd/MM/yyyy"),
                    NgayHetHan = x.NgayHetHan.ToString("dd/MM/yyyy"),
                    TrangThai = x.TrangThaiThe ? "Còn hiệu lực" : "Hết hạn"
                }).ToList();

                dgvTheThuVien.DataSource = displayData;

                // Thiết lập header text giống như FormDocGiaManagement
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
            if (dgvTheThuVien.Columns["MaThe"] != null)
                dgvTheThuVien.Columns["MaThe"].HeaderText = "Mã thẻ";
            if (dgvTheThuVien.Columns["TenDocGia"] != null)
                dgvTheThuVien.Columns["TenDocGia"].HeaderText = "Tên độc giả";
            if (dgvTheThuVien.Columns["SoDT"] != null)
                dgvTheThuVien.Columns["SoDT"].HeaderText = "Số ĐT";
            if (dgvTheThuVien.Columns["NgayCap"] != null)
                dgvTheThuVien.Columns["NgayCap"].HeaderText = "Ngày cấp";
            if (dgvTheThuVien.Columns["NgayHetHan"] != null)
                dgvTheThuVien.Columns["NgayHetHan"].HeaderText = "Ngày hết hạn";
            if (dgvTheThuVien.Columns["TrangThai"] != null)
                dgvTheThuVien.Columns["TrangThai"].HeaderText = "Trạng thái";
        }

        private void TxtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Nhập mã thẻ, tên độc giả hoặc số điện thoại...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Nhập mã thẻ, tên độc giả hoặc số điện thoại...";
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
            SearchTheThuVien();
        }

        private void SearchTheThuVien()
        {
            try
            {
                string searchText = txtSearch.Text.Trim();
                if (searchText == "Nhập mã thẻ, tên độc giả hoặc số điện thoại..." || string.IsNullOrEmpty(searchText))
                {
                    LoadTheThuVienData();
                }
                else
                {
                    var searchResults = _theThuVienDAO.SearchTheThuVien(searchText);
                    var displayData = searchResults.Select(x => new
                    {
                        MaThe = x.MaThe,
                        TenDocGia = x.TenDocGia,
                        SoDT = x.SoDT,
                        NgayCap = x.NgayCap.ToString("dd/MM/yyyy"),
                        NgayHetHan = x.NgayHetHan.ToString("dd/MM/yyyy"),
                        TrangThai = x.TrangThaiThe ? "Còn hiệu lực" : "Hết hạn"
                    }).ToList();

                    dgvTheThuVien.DataSource = displayData;
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
            using (var formAdd = new FormAddTheThuVien())
            {
                if (formAdd.ShowDialog() == DialogResult.OK)
                {
                    LoadTheThuVienData();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvTheThuVien.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn thẻ thư viện cần sửa!", "Thông báo",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maThe = Convert.ToInt32(dgvTheThuVien.CurrentRow.Cells["MaThe"].Value);

            using (var formEdit = new FormEditTheThuVien(maThe))
            {
                if (formEdit.ShowDialog() == DialogResult.OK)
                {
                    LoadTheThuVienData();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTheThuVien.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn thẻ thư viện cần xóa!", "Thông báo",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa thẻ thư viện này?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int maThe = Convert.ToInt32(dgvTheThuVien.CurrentRow.Cells["MaThe"].Value);
                    if (_theThuVienDAO.DeleteTheThuVien(maThe))
                    {
                        MessageBox.Show("Xóa thẻ thư viện thành công!", "Thành công",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadTheThuVienData();
                    }
                    else
                    {
                        MessageBox.Show("Xóa thẻ thư viện thất bại!", "Lỗi",
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
            txtSearch.Text = "Nhập mã thẻ, tên độc giả hoặc số điện thoại...";
            txtSearch.ForeColor = Color.Gray;
            LoadTheThuVienData();
        }

        // NEW: QR Code Generation Button Event Handler
        private void BtnGenerateQR_Click(object sender, EventArgs e)
        {
            if (dgvTheThuVien.CurrentRow == null)
            {
                MessageBox.Show("❗ Vui lòng chọn thẻ thư viện cần tạo QR Code!", "Thông báo",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int maThe = Convert.ToInt32(dgvTheThuVien.CurrentRow.Cells["MaThe"].Value);
                var theThuVien = _theThuVienDAO.GetTheThuVienById(maThe);

                if (theThuVien != null)
                {
                    // Check if card is still valid
                    if (!theThuVien.TrangThaiThe)
                    {
                        var continueResult = MessageBox.Show(
                            "⚠️ Thẻ này đã hết hạn!\n\nBạn có muốn tiếp tục tạo QR Code không?",
                            "Thẻ hết hạn",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (continueResult == DialogResult.No)
                            return;
                    }

                    // Generate QR Code
                    string qrText = QRCodeManager.CreateLibraryCardQR(theThuVien);
                    var qrImage = QRCodeManager.GenerateQRCode(qrText);

                    // Show QR Code Dialog
                    using (var qrDialog = new FormQRDisplay(qrImage, theThuVien))
                    {
                        qrDialog.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("❌ Không tìm thấy thông tin thẻ thư viện!", "Lỗi",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi khi tạo QR Code: {ex.Message}", "Lỗi",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvTheThuVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Có thể thêm logic xử lý click vào cell nếu cần
        }

        private void DgvTheThuVien_DoubleClick(object sender, EventArgs e)
        {
            BtnEdit_Click(sender, e);
        }
    }
}
