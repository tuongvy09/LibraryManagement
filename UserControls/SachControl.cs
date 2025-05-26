using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryManagement.Repositories;
using LibraryManagement.UI;

namespace LibraryManagement.UserControls
{
    public partial class SachControl : UserControl
    {
        private readonly CuonSachRepository _cuonSachRepo = new CuonSachRepository();
        public SachControl()
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
            if (dgvBooks.SelectedRows.Count > 0)
            {
                // Lấy mã cuốn sách từ dòng được chọn
                int maCuonSach = Convert.ToInt32(dgvBooks.SelectedRows[0].Cells["MaCuonSach"].Value);

                // Xác nhận người dùng
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa cuốn sách này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        _cuonSachRepo.DeleteCuonSach(maCuonSach);
                        MessageBox.Show("Đã xóa cuốn sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadCuonSachData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa cuốn sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn cuốn sách cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnManageAuthors_Click(object sender, EventArgs e)
        {
            FormManageTG formManageTG = new FormManageTG();
            formManageTG.ShowDialog();
        }

        private void BtnManagePublishers_Click(object sender, EventArgs e)
        {
            FormManageNXB formManageNXB = new FormManageNXB();
            formManageNXB.ShowDialog();
        }

        private void BtnManageCategories_Click(object sender, EventArgs e)
        {
            FormManageTheLoai formManageTheLoai = new FormManageTheLoai();
            formManageTheLoai.ShowDialog();
        }

        private void BtnManageTitles_Click(object sender, EventArgs e)
        {
            FormManageDauSach formManageDauSach = new FormManageDauSach();
            formManageDauSach.ShowDialog();
        }


        private void BtnDownloadTemplate_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "Chọn nơi lưu file mẫu";
                saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveFileDialog.FileName = "mau_import.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Đọc file mẫu từ resource (dưới dạng byte[])
                        byte[] templateBytes = Properties.Resources.mau_import;

                        // Ghi file ra đĩa theo đường dẫn người dùng chọn
                        File.WriteAllBytes(saveFileDialog.FileName, templateBytes);

                        MessageBox.Show("Tải file mẫu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi tải file mẫu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnUploadFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                openFileDialog.Title = "Chọn file Excel để nhập dữ liệu";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    try
                    {
                        var importer = new LibraryManagement.Services.ExcelImporter();
                        importer.ImportExcelToDatabase(filePath);

                        MessageBox.Show("Nhập dữ liệu từ Excel thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCuonSachData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi nhập Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void dgvBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
