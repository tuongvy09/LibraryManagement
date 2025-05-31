using LibraryManagement.BUS;
using LibraryManagement.Models;
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

namespace LibraryManagement.UI
{
    public partial class FormAddCuonSach : Form
    {
        private Label lblTitle, lblTenCuonSach, lblTrangThai, lblDauSach;
        private TextBox txtTenCuonSach, txtTrangThai;
        private ComboBox cboDauSach;
        private Button btnSave, btnCancel;
        private DauSachRepository repo = new DauSachRepository();

        public FormAddCuonSach()
        {
            InitializeComponent();
            InitializeCustomStyle();
            InitializeComponentForm();
        }

        private void InitializeComponentForm()
        {
            this.Text = "Thêm Cuốn sách";
            this.Size = new Size(400, 280);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeCustomStyle()
        {
            Color mainColor = ColorTranslator.FromHtml("#739a4f");

            lblTitle = new Label()
            {
                Text = "Thêm Cuốn Sách",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = mainColor,
                Location = new Point(130, 10),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            lblTenCuonSach = new Label()
            {
                Text = "Tên cuốn sách:",
                Location = new Point(20, 60),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTenCuonSach);

            txtTenCuonSach = new TextBox()
            {
                Location = new Point(140, 57),
                Width = 220,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtTenCuonSach);

            lblTrangThai = new Label()
            {
                Text = "Trạng thái:",
                Location = new Point(20, 100),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTrangThai);

            txtTrangThai = new TextBox()
            {
                Location = new Point(140, 97),
                Width = 220,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtTrangThai);

            lblDauSach = new Label()
            {
                Text = "Đầu sách:",
                Location = new Point(20, 140),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblDauSach);

            cboDauSach = new ComboBox()
            {
                Location = new Point(140, 137),
                Width = 190,
                DropDownStyle = ComboBoxStyle.DropDown, // ← CHỈNH TỪ DropDownList SANG DropDown
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(cboDauSach);

            // Tích hợp tìm kiếm trong ComboBox
            cboDauSach.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboDauSach.AutoCompleteSource = AutoCompleteSource.ListItems;

            Button btnAddDauSach = new Button()
            {
                Text = "+",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Size = new Size(30, 28),
                Location = new Point(cboDauSach.Right + 5, 135),
                BackColor = mainColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
            };
            btnAddDauSach.FlatAppearance.BorderSize = 0;
            btnAddDauSach.Click += BtnAddDauSach_Click;  // Sự kiện mở form thêm đầu sách
            this.Controls.Add(btnAddDauSach);

            btnSave = new Button()
            {
                Text = "Lưu",
                BackColor = mainColor,
                ForeColor = Color.White,
                Location = new Point(90, 190),
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
                Location = new Point(210, 190),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancel);
        }

        private void FormAddCuonSach_Load(object sender, EventArgs e)
        {
            var dauSachRepo = new DauSachRepository();
            var list = dauSachRepo.GetAllMaDauSach(); // List<DauSach>

            cboDauSach.DataSource = list;
            Console.WriteLine(list);
            cboDauSach.DisplayMember = "TenDauSach";
            cboDauSach.ValueMember = "MaDauSach";
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            string tenCuonSach = txtTenCuonSach.Text.Trim();
            string trangThai = txtTrangThai.Text.Trim();

            if (string.IsNullOrEmpty(tenCuonSach) || string.IsNullOrEmpty(trangThai) || cboDauSach.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maDauSach = (int)cboDauSach.SelectedValue;

            try
            {
                // Gọi tầng BLL, không gọi repo trực tiếp nữa
                var bll = new CuonSachBLL();
                bll.ThemCuonSach(maDauSach, trangThai, tenCuonSach);

                MessageBox.Show("Thêm cuốn sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm cuốn sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnAddDauSach_Click(object sender, EventArgs e)
        {
            // Ví dụ mở form thêm Đầu Sách mới
            FormAddDauSach formAdd = new FormAddDauSach();
            if (formAdd.ShowDialog() == DialogResult.OK)
            {
                // Sau khi thêm xong, load lại dữ liệu cho cboDauSach
                LoadDauSachToComboBox();
            }
        }
        private void LoadDauSachToComboBox()
        {
            List<DauSach> dauSachList = repo.GetAllMaDauSach(); // lấy dữ liệu từ repo

            cboDauSach.DataSource = dauSachList;
            cboDauSach.DisplayMember = "TenDauSach";  // tên thuộc tính hiển thị
            cboDauSach.ValueMember = "MaDauSach";     // tên thuộc tính giá trị

            if (dauSachList.Count > 0)
                cboDauSach.SelectedIndex = 0;
        }

    }
}
