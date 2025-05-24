using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryManagement.Repositories;
using LibraryManagement.UI;

namespace LibraryManagement.UserControls
{
    public partial class BookControl : UserControl
    {
        private readonly CuonSachRepository _cuonSachRepo = new CuonSachRepository();
        public BookControl()
        {
            InitializeComponent();
            LoadCuonSachData();
            txtSearch.Text = "Tìm kiếm cuốn sách...";
            txtSearch.ForeColor = Color.Gray;

            // Gán sự kiện
            txtSearch.Enter += TxtSearch_Enter;
            txtSearch.Leave += TxtSearch_Leave;
        }

        private void LoadCuonSachData()
        {
            var list = _cuonSachRepo.GetAllCuonSachDetails();

            var bindingList = list.Select(x => new
            {
                x.MaCuonSach,
                x.TenDauSach, 
                x.TenCuonSach,
                x.TrangThaiSach,
                x.TenTheLoai,
                x.QDSoTuoi,
                x.TenNSB,
                TacGias = string.Join(", ", x.TacGias)
            }).ToList();

            dgvBooks.DataSource = bindingList;
        }

        private void TxtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Tìm kiếm cuốn sách...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Tìm kiếm cuốn sách...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            var list = _cuonSachRepo.SearchCuonSach(keyword);

            var bindingList = list.Select(x => new
            {
                x.TenCuonSach,
                x.TrangThaiSach,
                x.TenTheLoai,
                x.QDSoTuoi,
                x.TenNSB,
                TacGias = string.Join(", ", x.TacGias)
            }).ToList();

            dgvBooks.DataSource = bindingList;
        }


        private void BtnAdd_Click(object sender, EventArgs e)
        {
            FormAddCuonSach formAddCuonSach = new FormAddCuonSach();
            formAddCuonSach.ShowDialog();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count > 0)
            {
                int maCuonSach = Convert.ToInt32(dgvBooks.SelectedRows[0].Cells["MaCuonSach"].Value);

                FormUpdateCuonSach formUpdate = new FormUpdateCuonSach(maCuonSach);
                formUpdate.ShowDialog();

                LoadCuonSachData(); // Reload lại dữ liệu sau khi sửa
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một cuốn sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            // TODO: Xóa sách đã chọn
        }

        private void BtnManageAuthors_Click(object sender, EventArgs e)
        {
            // TODO: Mở form quản lý tác giả
        }

        private void BtnManagePublishers_Click(object sender, EventArgs e)
        {
            // TODO: Mở form quản lý nhà xuất bản
        }

        private void BtnManageCategories_Click(object sender, EventArgs e)
        {
            // TODO: Mở form quản lý thể loại
        }

        private void BtnManageTitles_Click(object sender, EventArgs e)
        {
            // TODO: Mở form quản lý đầu sách
        }

        private void dgvBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
