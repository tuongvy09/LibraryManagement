using LibraryManagement.BUS;
using LibraryManagement.Repositories;
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
    public partial class NguoiDungControl : UserControl
    {
        private int selectedUserId = -1;
        private UserRepository userRepo;

        public NguoiDungControl()
        {
            InitializeComponent();
            var dbConnection = new DBConnection();
            userRepo = new UserRepository(dbConnection);
            LoadUsers();
        }

        private void LoadUsers()
        {
            // Replace with your database helper
            var dt = userRepo.GetAllUsers();
            dgvUsers.DataSource = dt;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = cbRole.SelectedItem?.ToString();

            try
            {
                UserBLL userBLL = new UserBLL();
                bool result = userBLL.ThemNguoiDung(username, password, role);

                if (result)
                {
                    MessageBox.Show("Thêm người dùng thành công.");
                    LoadUsers();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Thêm thất bại. Có thể tên người dùng đã tồn tại.");
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Lỗi dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedUserId == -1)
            {
                MessageBox.Show("Vui lòng chọn người dùng để sửa.");
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = cbRole.SelectedItem?.ToString();

            if (userRepo.UpdateUser(selectedUserId, username, password, role))
            {
                MessageBox.Show("Cập nhật thành công.");
                LoadUsers();
                ClearInputs();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại.");
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedUserId == -1)
            {
                MessageBox.Show("Vui lòng chọn người dùng để xoá.");
                return;
            }

            if (MessageBox.Show("Xác nhận xoá?", "Xoá người dùng", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (userRepo.DeleteUser(selectedUserId))
                {
                    MessageBox.Show("Xoá thành công.");
                    LoadUsers();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Xoá thất bại.");
                }
            }
        }

        private void DgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvUsers.Rows.Count > e.RowIndex)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                selectedUserId = Convert.ToInt32(row.Cells["UserID"].Value);
                txtUsername.Text = row.Cells["Username"].Value.ToString();
                txtPassword.Text = row.Cells["Password"].Value.ToString();
                cbRole.SelectedItem = row.Cells["Role"].Value.ToString();
            }
        }

        private void ClearInputs()
        {
            selectedUserId = -1;
            txtUsername.Clear();
            txtPassword.Clear();
            cbRole.SelectedIndex = -1;
        }
    }
}
