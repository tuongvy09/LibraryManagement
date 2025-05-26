using LibraryManagement.Repositories;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.UI
{
    public partial class FormAddTheLoai : Form
    {
        private Label lblTitle, lblQDSoTuoi, lblTenTheLoai;
        private TextBox txtQDSoTuoi, txtTenTheLoai;
        private Button btnSave, btnCancel;

        public FormAddTheLoai()
        {
            InitializeComponentForm();
            InitializeCustomStyle();
        }

        private void InitializeComponentForm()
        {
            this.Text = "Thêm Thể Loại";
            this.Size = new Size(400, 250);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeCustomStyle()
        {
            Color mainColor = ColorTranslator.FromHtml("#739a4f");

            lblTitle = new Label()
            {
                Text = "Thêm Thể Loại",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = mainColor,
                Location = new Point(120, 10),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            lblQDSoTuoi = new Label()
            {
                Text = "Quy định số tuổi:",
                Location = new Point(20, 60),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblQDSoTuoi);

            txtQDSoTuoi = new TextBox()
            {
                Location = new Point(160, 57),
                Width = 200,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtQDSoTuoi);

            lblTenTheLoai = new Label()
            {
                Text = "Tên thể loại:",
                Location = new Point(20, 100),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTenTheLoai);

            txtTenTheLoai = new TextBox()
            {
                Location = new Point(160, 97),
                Width = 200,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtTenTheLoai);

            btnSave = new Button()
            {
                Text = "Lưu",
                BackColor = mainColor,
                ForeColor = Color.White,
                Location = new Point(90, 150),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            btnCancel = new Button()
            {
                Text = "Hủy",
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Location = new Point(200, 150),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtQDSoTuoi.Text.Trim(), out int qdSoTuoi))
            {
                MessageBox.Show("Số tuổi không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenTheLoai = txtTenTheLoai.Text.Trim();
            if (string.IsNullOrEmpty(tenTheLoai))
            {
                MessageBox.Show("Vui lòng nhập tên thể loại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                TheLoaiRepository repo = new TheLoaiRepository();
                repo.AddTheLoai(qdSoTuoi, tenTheLoai);
                MessageBox.Show("Thêm thể loại thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi thêm thể loại.\nChi tiết: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
