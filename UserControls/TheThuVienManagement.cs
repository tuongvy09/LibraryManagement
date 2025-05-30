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
using LibraryManagement.Utilities;

namespace LibraryManagement.UserControls
{
    public partial class TheThuVienManagement : UserControl
    {
        private readonly TheThuVienDAO _theThuVienDAO = new TheThuVienDAO();

        public TheThuVienManagement()
        {
            InitializeComponent();
            InitializeDataGridViews();
        }

        private void InitializeDataGridViews()
        {
            try
            {
                // Khởi tạo DataTable rỗng để tránh null reference
                DataTable emptyTable = new DataTable();
                emptyTable.Columns.Add("Mã thẻ");
                emptyTable.Columns.Add("Tên độc giả");
                emptyTable.Columns.Add("Số ĐT");
                emptyTable.Columns.Add("Ngày cấp");
                emptyTable.Columns.Add("Ngày hết hạn");

                dgvConHieuLuc.DataSource = emptyTable.Copy();

                // Cho tab hết hạn thêm cột "Hết hạn từ"
                DataTable emptyTableHetHan = emptyTable.Copy();
                emptyTableHetHan.Columns.Add("Hết hạn từ");
                dgvHetHan.DataSource = emptyTableHetHan;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error initializing DataGridViews: " + ex.Message);
            }
        }

        private void TheThuVienManagement_Load(object sender, EventArgs e)
        {
            LoadData();
            SetupSearchPlaceholder();
        }

        private void SetupSearchPlaceholder()
        {
            txtSearch.Text = "Nhập mã thẻ, tên độc giả hoặc số điện thoại...";
            txtSearch.ForeColor = Color.Gray;

            txtSearch.Enter += TxtSearch_Enter;
            txtSearch.Leave += TxtSearch_Leave;
            txtSearch.KeyDown += TxtSearch_KeyDown;
        }

        void LoadData()
        {
            try
            {
                // Load data cho 2 tab
                dgvConHieuLuc.DataSource = _theThuVienDAO.LayTheThuVienConHieuLuc();
                dgvHetHan.DataSource = _theThuVienDAO.LayTheThuVienHetHan();

                dgvConHieuLuc.AutoResizeColumns();
                dgvHetHan.AutoResizeColumns();

                UpdateTabTitles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không lấy được dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateTabTitles()
        {
            try
            {
                // Cập nhật title với số lượng
                int conHieuLuc = dgvConHieuLuc?.Rows?.Count ?? 0;
                int hetHan = dgvHetHan?.Rows?.Count ?? 0;

                if (tabConHieuLuc != null)
                    tabConHieuLuc.Text = $"Còn hiệu lực ({conHieuLuc})";

                if (tabHetHan != null)
                    tabHetHan.Text = $"Hết hạn ({hetHan})";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating tab titles: " + ex.Message);
            }
        }

        private DataGridView GetActiveDataGridView()
        {
            return tabControl.SelectedTab == tabConHieuLuc ? dgvConHieuLuc : dgvHetHan;
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
            if (string.IsNullOrWhiteSpace(txtSearch.Text) ||
                txtSearch.Text == "Nhập mã thẻ, tên độc giả hoặc số điện thoại...")
            {
                LoadData();
                return;
            }

            try
            {
                // Tìm kiếm trên cả 2 tab
                var ketQuaTimKiem = _theThuVienDAO.TimKiemTheThuVien(txtSearch.Text.Trim());

                dgvConHieuLuc.DataSource = LayKetQuaTheoTrangThai(ketQuaTimKiem, true);
                dgvHetHan.DataSource = LayKetQuaTheoTrangThai(ketQuaTimKiem, false);

                dgvConHieuLuc.AutoResizeColumns();
                dgvHetHan.AutoResizeColumns();

                UpdateTabTitles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable LayKetQuaTheoTrangThai(DataTable ketQua, bool conHieuLuc)
        {
            if (ketQua == null) return new DataTable();

            DataTable result = ketQua.Clone();

            try
            {
                foreach (DataRow row in ketQua.Rows)
                {
                    if (row == null) continue;

                    string trangThai = row["Trạng thái"]?.ToString() ?? "";
                    bool isConHieuLuc = trangThai == "Còn hiệu lực";

                    if (isConHieuLuc == conHieuLuc)
                    {
                        result.ImportRow(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error filtering results: " + ex.Message);
            }

            return result;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var formAdd = new FormAddTheThuVien())
                {
                    if (formAdd.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form thêm thẻ thư viện: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            var activeGrid = GetActiveDataGridView();
            if (activeGrid.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn thẻ thư viện cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int maThe = Convert.ToInt32(activeGrid.CurrentRow.Cells[0].Value);
                using (var formEdit = new FormEditTheThuVien(maThe))
                {
                    if (formEdit.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form sửa thẻ thư viện: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "Nhập mã thẻ, tên độc giả hoặc số điện thoại...";
            txtSearch.ForeColor = Color.Gray;
            LoadData();
        }

        // QR Code Generation - Giữ nguyên như Version 1 cũ
        private void BtnGenerateQR_Click(object sender, EventArgs e)
        {
            var activeGrid = GetActiveDataGridView();
            if (activeGrid.CurrentRow == null)
            {
                MessageBox.Show("❗ Vui lòng chọn thẻ thư viện cần tạo QR Code!", "Thông báo",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int maThe = Convert.ToInt32(activeGrid.CurrentRow.Cells[0].Value);
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

        private void DgvConHieuLuc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle cell click for valid cards if needed
        }

        private void DgvHetHan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle cell click for expired cards if needed
        }

        private void DgvConHieuLuc_DoubleClick(object sender, EventArgs e)
        {
            BtnEdit_Click(sender, e);
        }

        private void DgvHetHan_DoubleClick(object sender, EventArgs e)
        {
            BtnEdit_Click(sender, e);
        }

        public void RefreshData()
        {
            LoadData();
        }
    }
}
