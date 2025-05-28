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
    public partial class PhieuPhatManagement : UserControl
    {
        private readonly PhieuPhatDAO _phieuPhatDAO = new PhieuPhatDAO();
        private List<PhieuPhat> currentData;

        public PhieuPhatManagement()
        {
            InitializeComponent();
            LoadPhieuPhatData();
            txtSearch.Text = "Nhập mã phiếu hoặc tên độc giả...";
            txtSearch.ForeColor = Color.Gray;

            dgvPhieuPhat.DoubleClick += dgvPhieuPhat_DoubleClick;
            txtSearch.Enter += TxtSearch_Enter;
            txtSearch.Leave += TxtSearch_Leave;
            txtSearch.KeyDown += TxtSearch_KeyDown;

            btnSearch.Click += BtnSearch_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
        }

        private void LoadPhieuPhatData()
        {
            try
            {
                currentData = _phieuPhatDAO.GetAllPhieuPhat();

                var displayData = currentData.Select(p => new
                {
                    p.MaPhieuPhat,
                    TenDocGia = p.HoTen,  // đổi tên cột ở đây để khớp với cài đặt header
                    TongTien = p.TongTien.ToString("N0") + " đ",
                    LoiViPham = string.Join(", ", p.LoiViPhams.Select(l => l.LyDo))
                }).ToList();

                dgvPhieuPhat.DataSource = displayData;
                SetupColumnHeaders();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupColumnHeaders()
        {
            dgvPhieuPhat.Columns["MaPhieuPhat"].HeaderText = "Mã phiếu phạt";
            dgvPhieuPhat.Columns["TenDocGia"].HeaderText = "Tên độc giả";
            dgvPhieuPhat.Columns["TongTien"].HeaderText = "Tổng tiền phạt";
            dgvPhieuPhat.Columns["LoiViPham"].HeaderText = "Lỗi vi phạm";
        }

        private void TxtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Nhập mã phiếu hoặc tên độc giả...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Nhập mã phiếu hoặc tên độc giả...";
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
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword) || keyword == "Nhập mã phiếu hoặc tên độc giả...")
            {
                LoadPhieuPhatData();
                return;
            }

            var filtered = _phieuPhatDAO.SearchPhieuPhat(keyword);

            var displayData = filtered.Select(p => new
            {
                p.MaPhieuPhat,
                TenDocGia = p.HoTen,
                TongTien = p.TongTien.ToString("N0") + " đ",
                LoiViPham = string.Join(", ", p.LoiViPhams.Select(l => l.LyDo))
            }).ToList();

            dgvPhieuPhat.DataSource = displayData;
            SetupColumnHeaders();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "Nhập mã phiếu hoặc tên độc giả...";
            txtSearch.ForeColor = Color.Gray;
            LoadPhieuPhatData();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new FormAddPhieuPhat())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadPhieuPhatData();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvPhieuPhat.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu phạt cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maPhieuPhat = Convert.ToInt32(dgvPhieuPhat.CurrentRow.Cells["MaPhieuPhat"].Value);
            using (var form = new FormEditPhieuPhat(maPhieuPhat))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadPhieuPhatData(); 
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPhieuPhat.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu phạt cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa phiếu phạt này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                int maPhieuPhat = Convert.ToInt32(dgvPhieuPhat.CurrentRow.Cells["MaPhieuPhat"].Value);
                if (_phieuPhatDAO.DeletePhieuPhat(maPhieuPhat))
                {
                    MessageBox.Show("Xóa phiếu phạt thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPhieuPhatData();
                }
                else
                {
                    MessageBox.Show("Xóa phiếu phạt thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvPhieuPhat_DoubleClick(object sender, EventArgs e)
        {
            if (dgvPhieuPhat.CurrentRow != null)
            {
                var row = dgvPhieuPhat.CurrentRow;

                var chiTiet = new PhieuPhat
                {
                    MaPhieuPhat = Convert.ToInt32(row.Cells["MaPhieuPhat"].Value),
                    HoTen = row.Cells["TenDocGia"].Value?.ToString(),
                    TongTien = decimal.Parse(row.Cells["TongTien"].Value.ToString().Replace(" đ", "")),
                    LoiViPhams = new List<QDP>() // Nếu cần chi tiết hơn thì lấy từ DAO
                };

                var form = new FormChiTietPhieuPhat(chiTiet);
                form.ShowDialog();
            }
        }
    }
}
