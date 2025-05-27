using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.Repositories.LibraryManagement.Repositories;
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
    public partial class FormAddDauSach : Form
    {
        private Label lblTitle;
        private Label lblTenDauSach, lblTheLoai, lblNXB;
        private TextBox txtTenDauSach;
        private ComboBox cboTheLoai, cboNXB;
        private Button btnSave, btnCancel;
        private CheckedListBox clbTacGia;
        private Label lblTacGia;

        private DauSachRepository repo = new DauSachRepository();
        private TheLoaiRepository theLoaiRepository = new TheLoaiRepository();
        private NXBRepository theXBRepository = new NXBRepository();
        private TacGiaRepository tacGiaRepository = new TacGiaRepository();
        private DauSachTacGiaRepository dstgRepository = new DauSachTacGiaRepository();
        public FormAddDauSach()
        {
            InitializeComponentForm();
            InitializeCustomStyle();
            LoadComboBoxData();
            LoadTacGia();
        }

        private void InitializeComponentForm()
        {
            this.Text = "Thêm Đầu Sách";
            this.Size = new Size(400, 350);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeCustomStyle()
        {
            Color mainColor = ColorTranslator.FromHtml("#739a4f");

            lblTitle = new Label()
            {
                Text = "Thêm Đầu Sách",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = mainColor,
                Location = new Point(140, 10),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            lblTenDauSach = new Label()
            {
                Text = "Tên đầu sách:",
                Location = new Point(20, 60),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTenDauSach);

            txtTenDauSach = new TextBox()
            {
                Location = new Point(130, 57),
                Width = 220,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtTenDauSach);

            lblTheLoai = new Label()
            {
                Text = "Thể loại:",
                Location = new Point(20, 100),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTheLoai);

            cboTheLoai = new ComboBox()
            {
                Location = new Point(130, 97),
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(cboTheLoai);

            // Nút thêm Thể loại
            Button btnAddTheLoai = new Button()
            {
                Text = "+",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(30, cboTheLoai.Height),
                Location = new Point(cboTheLoai.Right + 5, cboTheLoai.Top),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat,
            };
            btnAddTheLoai.FlatAppearance.BorderSize = 0;
            btnAddTheLoai.Click += (s, e) =>
            {
                FormAddTheLoai form = new FormAddTheLoai();
                form.ShowDialog();
                LoadTheLoaiList();
            };
            this.Controls.Add(btnAddTheLoai);


            lblNXB = new Label()
            {
                Text = "Nhà xuất bản:",
                Location = new Point(20, 140),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblNXB);

            cboNXB = new ComboBox()
            {
                Location = new Point(130, 137),
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(cboNXB);
            // Nút thêm nhà xuất bản
            Button btnAddNXB = new Button()
            {
                Text = "+",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(30, cboNXB.Height),
                Location = new Point(cboNXB.Right + 5, cboNXB.Top),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat,
            };
            btnAddNXB.FlatAppearance.BorderSize = 0;
            btnAddNXB.Click += BtnAddNXB_Click;
            this.Controls.Add(btnAddNXB);

            lblTacGia = new Label()
            {
                Text = "Tác giả:",
                Location = new Point(20, 180),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            this.Controls.Add(lblTacGia);

            clbTacGia = new CheckedListBox()
            {
                Location = new Point(130, 180),
                Size = new Size(220, 80),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(clbTacGia);

            // Nút thêm Tác giả
            Button btnAddTacGia = new Button()
            {
                Text = "+",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(30, clbTacGia.ItemHeight),
                Location = new Point(clbTacGia.Right + 5, clbTacGia.Top),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat,
            };
            btnAddTacGia.FlatAppearance.BorderSize = 0;
            btnAddTacGia.Click += (s, e) =>
            {
                FormAddTG form = new FormAddTG();
                form.ShowDialog();
                LoadTacGia();
            };
            this.Controls.Add(btnAddTacGia);

            btnSave = new Button()
            {
                Text = "Lưu",
                BackColor = mainColor,
                ForeColor = Color.White,
                Location = new Point(100, 250),
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
                Location = new Point(220, 250),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancel);
        }

        private void LoadTacGia()
        {
            List<TacGia> danhSachTacGia = tacGiaRepository.GetAllTacGia(); // <-- Lấy đúng danh sách tác giả
            clbTacGia.DataSource = danhSachTacGia;
            clbTacGia.DisplayMember = "TenTG";
            clbTacGia.ValueMember = "MaTacGia";
        }

        private void LoadTheLoaiList()
        {
            TheLoaiRepository repo = new TheLoaiRepository();
            List<TheLoai> theLoais = repo.GetAllTheLoai();

            cboTheLoai.DataSource = null;
            cboTheLoai.DataSource = theLoais;
            cboTheLoai.DisplayMember = "TenTheLoai"; 
            cboTheLoai.ValueMember = "MaTheLoai"; 

            if (cboTheLoai.Items.Count > 0)
                cboTheLoai.SelectedIndex = 0;
        }

        private void LoadComboBoxData()
        {
            var theLoaiList = theLoaiRepository.GetAllTheLoai();
            var nxbList = theXBRepository.GetAllNXB();

            cboTheLoai.DataSource = theLoaiList;
            cboTheLoai.DisplayMember = "TenTheLoai";
            cboTheLoai.ValueMember = "MaTheLoai";

            cboNXB.DataSource = nxbList;
            cboNXB.DisplayMember = "TenNXB";
            cboNXB.ValueMember = "MaNXB";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string tenDauSach = txtTenDauSach.Text.Trim();

            if (string.IsNullOrEmpty(tenDauSach))
            {
                MessageBox.Show("Vui lòng nhập tên đầu sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboTheLoai.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn thể loại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboNXB.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn nhà xuất bản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maTheLoai = (int)cboTheLoai.SelectedValue;
            int maNXB = (int)cboNXB.SelectedValue;

            try
            {
                int maDauSach = repo.AddDauSach(tenDauSach, maTheLoai, maNXB);

                foreach (var item in clbTacGia.CheckedItems)
                {
                    int maTacGia = ((TacGia)item).MaTacGia;
                    dstgRepository.AddTacGiaChoDauSach(maDauSach, maTacGia);
                }

                MessageBox.Show("Thêm đầu sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm đầu sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormAddDauSach_Load(object sender, EventArgs e)
        {

        }
        private void BtnAddNXB_Click(object sender, EventArgs e)
        {
            // Giả sử bạn đã có FormAddNXB để thêm nhà xuất bản
            using (var formAddNXB = new FormAddNXB())
            {
                if (formAddNXB.ShowDialog() == DialogResult.OK)
                {
                    // Tải lại dữ liệu nhà xuất bản sau khi thêm
                    var nxbList = theXBRepository.GetAllNXB();
                    cboNXB.DataSource = null;
                    cboNXB.DataSource = nxbList;
                    cboNXB.DisplayMember = "TenNXB";
                    cboNXB.ValueMember = "MaNXB";
                }
            }
        }

    }
}
