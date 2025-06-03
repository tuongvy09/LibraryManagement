using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LibraryManagement.UserControls
{
    public partial class PhieuMuonManagement : UserControl
    {
        private readonly PhieuMuonDAO _muonSachDAO = new PhieuMuonDAO();
        private List<Models.PhieuMuon> currentData;

        public PhieuMuonManagement()
        {
            InitializeComponent();
            LoadPhieuMuonData();
            txtSearch.Text = "Nhập mã phiếu, tên độc giả hoặc tên cuốn sách...";
            txtSearch.ForeColor = Color.Gray;

            dgvPhieuMuon.DoubleClick += dgvPhieuMuon_DoubleClick;
            txtSearch.Enter += TxtSearch_Enter;
            txtSearch.Leave += TxtSearch_Leave;
            txtSearch.KeyDown += TxtSearch_KeyDown;

            btnSearch.Click += BtnSearch_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            dgvPhieuMuon.CellFormatting += dgvPhieuMuon_CellFormatting;
        }

        private void LoadPhieuMuonData()
        {
            try
            {
                currentData = _muonSachDAO.GetAllPhieuMuon();

                var displayData = currentData.Select(x => new
                {
                    x.MaMuonSach,
                    x.TenDocGia,
                    x.TenCuonSach,
                    NgayMuon = x.NgayMuon.ToString("dd/MM/yyyy"),
                    NgayTra = x.NgayTra.ToString("dd/MM/yyyy"),
                    x.TrangThaiM,
                    x.GiaMuon,
                    x.SoNgayMuon,
                    x.TienCoc
                }).ToList();

                dgvPhieuMuon.DataSource = displayData;
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
            dgvPhieuMuon.Columns["MaMuonSach"].HeaderText = "Mã phiếu mượn";
            dgvPhieuMuon.Columns["TenDocGia"].HeaderText = "Tên độc giả";
            dgvPhieuMuon.Columns["TenCuonSach"].HeaderText = "Tên cuốn sách";
            dgvPhieuMuon.Columns["NgayMuon"].HeaderText = "Ngày mượn";
            dgvPhieuMuon.Columns["NgayTra"].HeaderText = "Ngày trả";
            dgvPhieuMuon.Columns["TrangThaiM"].HeaderText = "Trạng thái";
            dgvPhieuMuon.Columns["GiaMuon"].HeaderText = "Giá mượn";
            dgvPhieuMuon.Columns["SoNgayMuon"].HeaderText = "Số ngày mượn";
            dgvPhieuMuon.Columns["TienCoc"].HeaderText = "Tiền cọc";
        }

        private void TxtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Nhập mã phiếu, tên độc giả hoặc tên cuốn sách...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Nhập mã phiếu, tên độc giả hoặc tên cuốn sách...";
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
            string searchText = txtSearch.Text.Trim().ToLowerInvariant();
            if (string.IsNullOrEmpty(searchText) || searchText == "nhập mã phiếu, tên độc giả hoặc tên cuốn sách...")
            {
                LoadPhieuMuonData();
                return;
            }

            var filtered = currentData.Where(p =>
                p.MaMuonSach.ToString().Contains(searchText) ||
                (p.TenDocGia != null && p.TenDocGia.ToLowerInvariant().Contains(searchText)) ||
                (p.TenCuonSach != null && p.TenCuonSach.ToLowerInvariant().Contains(searchText))
            ).ToList();

            dgvPhieuMuon.DataSource = filtered.Select(x => new
            {
                x.MaMuonSach,
                x.TenDocGia,
                x.TenCuonSach,
                NgayMuon = x.NgayMuon.ToString("dd/MM/yyyy"),
                NgayTra = x.NgayTra.ToString("dd/MM/yyyy"),
                x.TrangThaiM,
                x.GiaMuon,
                x.SoNgayMuon,
                x.TienCoc
            }).ToList();

            SetupColumnHeaders();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "Nhập mã phiếu, tên độc giả hoặc tên cuốn sách...";
            txtSearch.ForeColor = Color.Gray;
            LoadPhieuMuonData();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var formAdd = new FormAddPhieuMuon())
            {
                if (formAdd.ShowDialog() == DialogResult.OK)
                {
                    LoadPhieuMuonData();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvPhieuMuon.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu mượn cần sửa!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maMuonSach = Convert.ToInt32(dgvPhieuMuon.CurrentRow.Cells["MaMuonSach"].Value);

            try
            {
                using (var formEdit = new FormEditPhieuMuon(maMuonSach))
                {
                    var result = formEdit.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        LoadPhieuMuonData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở form sửa phiếu mượn do: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPhieuMuon.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu mượn cần xóa!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa phiếu mượn này?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int maMuonSach = Convert.ToInt32(dgvPhieuMuon.CurrentRow.Cells["MaMuonSach"].Value);
                    if (_muonSachDAO.DeletePhieuMuon(maMuonSach)) // Giả sử hàm xóa có tên này
                    {
                        MessageBox.Show("Xóa phiếu mượn thành công!", "Thành công",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadPhieuMuonData();
                    }
                    else
                    {
                        MessageBox.Show("Xóa phiếu mượn thất bại!", "Lỗi",
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

        private void dgvPhieuMuon_DoubleClick(object sender, EventArgs e)
        {
            if (dgvPhieuMuon.CurrentRow != null)
            {
                var row = dgvPhieuMuon.CurrentRow;

                var chiTiet = new PhieuMuon
                {
                    MaMuonSach = Convert.ToInt32(row.Cells["MaMuonSach"].Value),
                    TenDocGia = row.Cells["TenDocGia"].Value?.ToString(),
                    TenCuonSach = row.Cells["TenCuonSach"].Value?.ToString(),
                    NgayMuon = DateTime.ParseExact(row.Cells["NgayMuon"].Value.ToString(), "dd/MM/yyyy", null),
                    NgayTra = DateTime.ParseExact(row.Cells["NgayTra"].Value.ToString(), "dd/MM/yyyy", null),
                    TrangThaiM = row.Cells["TrangThaiM"].Value?.ToString(),
                    GiaMuon = Convert.ToDecimal(row.Cells["GiaMuon"].Value),
                    SoNgayMuon = Convert.ToInt32(row.Cells["SoNgayMuon"].Value),
                    TienCoc = Convert.ToDecimal(row.Cells["TienCoc"].Value)
                };

                FormChiTietPhieuMuon form = new FormChiTietPhieuMuon(chiTiet);
                form.ShowDialog();
            }
        }

        private void dgvPhieuMuon_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvPhieuMuon.Columns[e.ColumnIndex].Name == "GiaMuon" ||
                dgvPhieuMuon.Columns[e.ColumnIndex].Name == "TienCoc")
            {
                if (e.Value != null && decimal.TryParse(e.Value.ToString(), out decimal amount))
                {
                    // Định dạng tiền tệ Việt Nam (VND)
                    e.Value = string.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:c0}", amount);
                    e.FormattingApplied = true;
                }
            }
        }
    }
}
