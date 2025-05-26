using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.UserControls
{
    partial class NguoiDungControl
    {
        private Color mainColor = ColorTranslator.FromHtml("#739a4f");

        private DataGridView dgvUsers;
        private TextBox txtUsername, txtPassword;
        private ComboBox cbRole;
        private Button btnAdd, btnUpdate, btnDelete;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.White;

            Label lblUsername = new Label { Text = "Tên đăng nhập", Location = new Point(20, 20) };
            txtUsername = new TextBox { Location = new Point(130, 20), Width = 200 };

            Label lblPassword = new Label { Text = "Mật khẩu", Location = new Point(20, 60) };
            txtPassword = new TextBox { Location = new Point(130, 60), Width = 200 };

            Label lblRole = new Label { Text = "Vai trò", Location = new Point(20, 100) };
            cbRole = new ComboBox { Location = new Point(130, 100), Width = 200 };
            cbRole.Items.AddRange(new string[] { "admin", "user" });
            cbRole.DropDownStyle = ComboBoxStyle.DropDownList;

            btnAdd = new Button { Text = "Thêm", Location = new Point(20, 150), BackColor = mainColor, ForeColor = Color.White };
            btnUpdate = new Button { Text = "Sửa", Location = new Point(110, 150), BackColor = mainColor, ForeColor = Color.White };
            btnDelete = new Button { Text = "Xoá", Location = new Point(200, 150), BackColor = mainColor, ForeColor = Color.White };

            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;

            dgvUsers = new DataGridView
            {
                Location = new Point(20, 200),
                Width = 500,
                Height = 200,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvUsers.CellClick += DgvUsers_CellClick;

            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblRole);
            this.Controls.Add(cbRole);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnDelete);
            this.Controls.Add(dgvUsers);
        }
    }
}
