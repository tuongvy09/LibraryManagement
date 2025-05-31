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
using System.Xml.Linq;

namespace LibraryManagement.UI
{
    public partial class FormAddNXB : Form
    {
        public string NxbName { get; private set; }

        public FormAddNXB()
        {
            InitializeComponent();
            InitializeCustomStyle();
        }

        private void InitializeCustomStyle()
        {
            this.Text = "Thêm nhà xuất bản";
            this.Size = new Size(400, 200);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = ColorTranslator.FromHtml("#739a4f");

            Label lblName = new Label()
            {
                Text = "Tên NXB:",
                Location = new Point(30, 30),
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            TextBox txtName = new TextBox()
            {
                Name = "txtNxbName",
                Location = new Point(120, 26),
                Width = 220,
                Font = new Font("Segoe UI", 10)
            };

            Button btnSave = new Button()
            {
                Text = "Lưu",
                Location = new Point(120, 80),
                Width = 80,
                BackColor = Color.White,
                ForeColor = ColorTranslator.FromHtml("#739a4f"),
                FlatStyle = FlatStyle.Flat
            };
            btnSave.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên NXB!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    var nxbBLL = new NXBBLL();
                    nxbBLL.ThemNXB(txtName.Text.Trim());

                    MessageBox.Show("Thêm nhà xuất bản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm NXB: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            Button btnCancel = new Button()
            {
                Text = "Hủy",
                Location = new Point(220, 80),
                Width = 80,
                BackColor = Color.White,
                ForeColor = ColorTranslator.FromHtml("#739a4f"),
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void FormAddNXB_Load(object sender, EventArgs e)
        {

        }
    }
}
