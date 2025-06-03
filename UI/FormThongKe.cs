using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Models;
using LibraryManagement.Repositories;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace LibraryManagement.UI
{
    public partial class FormThongKe : Form
    {
        private Label lblTitle;
        private TabControl tabControl;
        private TabPage tabTienMuon, tabTienMuonVaPhat, tabDocGiaMoi, tabTop10BestSeller;
        private TabPage tabTheoTheLoai;

        private NumericUpDown numNamThongKe;
        private NumericUpDown numThangThongKe;
        private ComboBox cboThongKeTheo;
        private DataGridView dgvThongKeSachMuon;

        private DocGiaDAO docGiaDAO = new DocGiaDAO();
        private CuonSachRepository CuonSachRepository = new CuonSachRepository();
        private List<ThongKeSachMuonTheoTheLoaiDTO> thongKeSachMuonTheoTheLoai;
        private List<ThongKeSachMuonTheoDocGiaDTO> thongKeSachMuonTheoDocGia;

        public FormThongKe()
        {
            InitializeComponentForm();
            InitializeCustomStyle();
        }

        private void InitializeComponentForm()
        {
            this.Text = "Thống kê - Báo cáo";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
        }

        private void InitializeCustomStyle()
        {
            Color mainColor = ColorTranslator.FromHtml("#739a4f");
            this.BackColor = Color.White;

            lblTitle = new Label()
            {
                Text = "THỐNG KÊ - BÁO CÁO",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = mainColor,
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // TabControl
            tabControl = new TabControl()
            {
                Location = new Point(20, 70),
                Size = new Size(950, 800),
                Font = new Font("Segoe UI", 10)
            };

            // Tab 1: Tổng tiền mượn theo tháng
            tabTienMuon = new TabPage("Tổng tiền mượn theo tháng");
            InitializeTabTienMuon();
            tabControl.TabPages.Add(tabTienMuon);

            // Tab 2: Tổng tiền mượn + phạt
            tabTienMuonVaPhat = new TabPage("Tổng tiền mượn + phạt");
            InitializeTabTienMuonVaPhat();
            tabControl.TabPages.Add(tabTienMuonVaPhat);

            // Tab 3: Độc giả mới theo tháng
            tabDocGiaMoi = new TabPage("Độc giả mới theo tháng");
            InitializeTabDocGiaMoi();
            tabControl.TabPages.Add(tabDocGiaMoi);

            // Tab 4: Top 10 Best Seller
            tabTop10BestSeller = new TabPage("Top 10 sách mượn nhiều nhất");
            CreateTop10BestSellerTab();
            tabControl.TabPages.Add(tabTop10BestSeller);

            // Tab 5: Sách mượn theo thể loại, theo tháng, theo độc giả
            tabTheoTheLoai = new TabPage("Tổng Sách Được Mượn");
            InitializeTabSachMuon();
            tabControl.TabPages.Add(tabTheoTheLoai);

            this.Controls.Add(tabControl);
        }

        private TabPage CreateTop10BestSellerTab()
        {
            Color mainColor = ColorTranslator.FromHtml("#739a4f");

            TabPage tabTop10BetSeller = new TabPage("Top 10 Best Seller")
            {
                BackColor = mainColor
            };

            DataGridView dgvBestSeller = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                BackgroundColor = Color.White,
                EnableHeadersVisualStyles = false,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = mainColor,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                }
            };

            dgvBestSeller.Columns.Add("STT", "STT");
            dgvBestSeller.Columns.Add("TenSach", "Tên Sách");
            dgvBestSeller.Columns.Add("SoLuong", "Số Lượng Đã Mượn");

            // Gọi hàm lấy dữ liệu top 10
            List<SachHot> danhSachHot = CuonSachRepository.GetTop10CuonSachHot();

            int stt = 1;
            foreach (var sach in danhSachHot)
            {
                dgvBestSeller.Rows.Add(stt++, $"{sach.TenCuonSach} ({sach.TenDauSach})", sach.SoLuongMuon);
            }

            tabTop10BetSeller.Controls.Add(dgvBestSeller);
            return tabTop10BetSeller;
        }


        private void InitializeTabTienMuon()
        {
            Color mainColor = ColorTranslator.FromHtml("#739a4f");

            // Controls for filtering
            Label lblThang = new Label()
            {
                Text = "Tháng:",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            tabTienMuon.Controls.Add(lblThang);

            ComboBox cboThang = new ComboBox()
            {
                Location = new Point(80, 17),
                Width = 80,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10),
                Name = "cboThangTienMuon"
            };
            for (int i = 1; i <= 12; i++)
            {
                cboThang.Items.Add(i);
            }
            cboThang.SelectedIndex = DateTime.Now.Month - 1;
            tabTienMuon.Controls.Add(cboThang);

            Label lblNam = new Label()
            {
                Text = "Năm:",
                Location = new Point(180, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            tabTienMuon.Controls.Add(lblNam);

            NumericUpDown numNam = new NumericUpDown()
            {
                Location = new Point(220, 17),
                Width = 80,
                Minimum = 2020,
                Maximum = 2030,
                Value = DateTime.Now.Year,
                Font = new Font("Segoe UI", 10),
                Name = "numNamTienMuon"
            };
            tabTienMuon.Controls.Add(numNam);

            Button btnThongKe = new Button()
            {
                Text = "Thống kê",
                BackColor = mainColor,
                ForeColor = Color.White,
                Location = new Point(320, 17),
                Size = new Size(100, 25),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            btnThongKe.FlatAppearance.BorderSize = 0;
            btnThongKe.Click += (s, e) => LoadThongKeTienMuon();
            tabTienMuon.Controls.Add(btnThongKe);

            // DataGridView
            DataGridView dgvTienMuon = new DataGridView()
            {
                Location = new Point(20, 60),
                Size = new Size(900, 400),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D,
                Name = "dgvTienMuon"
            };
            tabTienMuon.Controls.Add(dgvTienMuon);
        }

        private void InitializeTabTienMuonVaPhat()
        {
            Color mainColor = ColorTranslator.FromHtml("#739a4f");

            // Controls for filtering
            Label lblThang = new Label()
            {
                Text = "Tháng:",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            tabTienMuonVaPhat.Controls.Add(lblThang);

            ComboBox cboThang = new ComboBox()
            {
                Location = new Point(80, 17),
                Width = 80,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10),
                Name = "cboThangTienMuonVaPhat"
            };
            for (int i = 1; i <= 12; i++)
            {
                cboThang.Items.Add(i);
            }
            cboThang.SelectedIndex = DateTime.Now.Month - 1;
            tabTienMuonVaPhat.Controls.Add(cboThang);

            Label lblNam = new Label()
            {
                Text = "Năm:",
                Location = new Point(180, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            tabTienMuonVaPhat.Controls.Add(lblNam);

            NumericUpDown numNam = new NumericUpDown()
            {
                Location = new Point(220, 17),
                Width = 80,
                Minimum = 2020,
                Maximum = 2030,
                Value = DateTime.Now.Year,
                Font = new Font("Segoe UI", 10),
                Name = "numNamTienMuonVaPhat"
            };
            tabTienMuonVaPhat.Controls.Add(numNam);

            Button btnThongKe = new Button()
            {
                Text = "Thống kê",
                BackColor = mainColor,
                ForeColor = Color.White,
                Location = new Point(320, 17),
                Size = new Size(100, 25),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            btnThongKe.FlatAppearance.BorderSize = 0;
            btnThongKe.Click += (s, e) => LoadThongKeTienMuonVaPhat();
            tabTienMuonVaPhat.Controls.Add(btnThongKe);

            // DataGridView
            DataGridView dgvTienMuonVaPhat = new DataGridView()
            {
                Location = new Point(20, 60),
                Size = new Size(900, 400),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D,
                Name = "dgvTienMuonVaPhat"
            };
            tabTienMuonVaPhat.Controls.Add(dgvTienMuonVaPhat);
        }

        private void InitializeTabDocGiaMoi()
        {
            Color mainColor = ColorTranslator.FromHtml("#739a4f");

            // Controls for filtering
            Label lblNam = new Label()
            {
                Text = "Năm:",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = mainColor
            };
            tabDocGiaMoi.Controls.Add(lblNam);

            NumericUpDown numNam = new NumericUpDown()
            {
                Location = new Point(60, 17),
                Width = 80,
                Minimum = 2020,
                Maximum = 2030,
                Value = DateTime.Now.Year,
                Font = new Font("Segoe UI", 10),
                Name = "numNamDocGiaMoi"
            };
            tabDocGiaMoi.Controls.Add(numNam);

            Button btnThongKe = new Button()
            {
                Text = "Thống kê",
                BackColor = mainColor,
                ForeColor = Color.White,
                Location = new Point(160, 17),
                Size = new Size(100, 25),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            btnThongKe.FlatAppearance.BorderSize = 0;
            btnThongKe.Click += (s, e) => LoadThongKeDocGiaMoi();
            tabDocGiaMoi.Controls.Add(btnThongKe);

            // DataGridView
            DataGridView dgvDocGiaMoi = new DataGridView()
            {
                Location = new Point(20, 60),
                Size = new Size(900, 400),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D,
                Name = "dgvDocGiaMoi"
            };
            tabDocGiaMoi.Controls.Add(dgvDocGiaMoi);
        }

        private void InitializeTabSachMuon()
        {
            tabTheoTheLoai.BackColor = Color.White;

            // Label chọn năm
            Label labelNam = new Label()
            {
                Text = "Năm:",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };

            numNamThongKe = new NumericUpDown()
            {
                Minimum = 2000,
                Maximum = 2100,
                Value = DateTime.Now.Year,
                Location = new Point(70, 18),
                Width = 80,
                Font = new Font("Segoe UI", 10),
                Enabled = false
            };

            // Label chọn tháng
            Label labelThang = new Label()
            {
                Text = "Tháng:",
                Location = new Point(170, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };

            numThangThongKe = new NumericUpDown()
            {
                Minimum = 1,
                Maximum = 12,
                Value = DateTime.Now.Month,
                Location = new Point(230, 18),
                Width = 60,
                Font = new Font("Segoe UI", 10),
                Enabled = false
            };

            // Label thống kê theo
            Label labelLuaChon = new Label()
            {
                Text = "Thống kê theo:",
                Location = new Point(320, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };

            cboThongKeTheo = new ComboBox()
            {
                Location = new Point(430, 17),
                Width = 160,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cboThongKeTheo.Items.AddRange(new string[] { "Thể loại", "Tháng", "Độc giả" });

            // Nút thống kê
            Button btnThongKe = new Button()
            {
                Text = "Thống kê",
                Location = new Point(610, 15),
                Height = 30,
                Width = 100,
                BackColor = ColorTranslator.FromHtml("#739a4f"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnThongKe.FlatAppearance.BorderSize = 0;

            // DataGridView hiển thị kết quả
            dgvThongKeSachMuon = new DataGridView()
            {
                Location = new Point(20, 60),
                Size = new Size(900, 400),
                Font = new Font("Segoe UI", 10),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Sự kiện khi thay đổi ComboBox
            cboThongKeTheo.SelectedIndexChanged += (s, e) =>
            {
                string luaChon = cboThongKeTheo.SelectedItem?.ToString();
                bool enableThangNam = luaChon == "Tháng";

                numNamThongKe.Enabled = enableThangNam;
                numThangThongKe.Enabled = enableThangNam;
            };

            btnThongKe.Click += (s, e) =>
            {
                string luaChon = cboThongKeTheo.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(luaChon))
                {
                    LoadTatCaSachDangMuon();
                    return;
                }

                var thongKeDAO = new ThongKeDAO();

                if (luaChon == "Thể loại")
                {
                    thongKeSachMuonTheoTheLoai = thongKeDAO.GetThongKeSachMuonTheoTheLoai();
                    dgvThongKeSachMuon.DataSource = thongKeSachMuonTheoTheLoai;
                }
                else if (luaChon == "Độc giả")
                {
                    thongKeSachMuonTheoDocGia = thongKeDAO.GetThongKeSachMuonTheoDocGia();
                    dgvThongKeSachMuon.DataSource = thongKeSachMuonTheoDocGia;
                }
            };

            // Khi tab được chọn, reset các control và load dữ liệu mặc định
            tabTheoTheLoai.Enter += (s, e) =>
            {
                cboThongKeTheo.SelectedIndex = -1;
                numNamThongKe.Enabled = false;
                numThangThongKe.Enabled = false;
                LoadTatCaSachDangMuon();
            };

            // Thêm controls vào tab
            tabTheoTheLoai.Controls.Add(labelNam);
            tabTheoTheLoai.Controls.Add(numNamThongKe);
            tabTheoTheLoai.Controls.Add(labelThang);
            tabTheoTheLoai.Controls.Add(numThangThongKe);
            tabTheoTheLoai.Controls.Add(labelLuaChon);
            tabTheoTheLoai.Controls.Add(cboThongKeTheo);
            tabTheoTheLoai.Controls.Add(btnThongKe);
            tabTheoTheLoai.Controls.Add(dgvThongKeSachMuon);
        }

        private void LoadTatCaSachDangMuon()
        {
            var thongKeDAO = new ThongKeDAO();
            var data = thongKeDAO.GetTatCaSachDangMuon(); 
            dgvThongKeSachMuon.DataSource = data;
        }

        private void LoadThongKeTienMuon()
        {
            try
            {
                var cboThang = tabTienMuon.Controls.Find("cboThangTienMuon", false)[0] as ComboBox;
                var numNam = tabTienMuon.Controls.Find("numNamTienMuon", false)[0] as NumericUpDown;
                var dgv = tabTienMuon.Controls.Find("dgvTienMuon", false)[0] as DataGridView;

                int thang = (int)cboThang.SelectedItem;
                int nam = (int)numNam.Value;

                var results = docGiaDAO.GetTongTienMuonTheoThang(thang, nam);

                var displayData = results.Select(r => new
                {
                    MaDocGia = r.MaDocGia,
                    HoTen = r.HoTen,
                    TongTienMuon = r.TongTienMuon.ToString("N0") + " VNĐ",
                    SoLanMuon = r.SoLanMuon
                }).ToList();

                dgv.DataSource = displayData;

                // Thiết lập header
                if (dgv.Columns["MaDocGia"] != null)
                    dgv.Columns["MaDocGia"].HeaderText = "Mã ĐG";
                if (dgv.Columns["HoTen"] != null)
                    dgv.Columns["HoTen"].HeaderText = "Họ tên";
                if (dgv.Columns["TongTienMuon"] != null)
                    dgv.Columns["TongTienMuon"].HeaderText = "Tổng tiền mượn";
                if (dgv.Columns["SoLanMuon"] != null)
                    dgv.Columns["SoLanMuon"].HeaderText = "Số lần mượn";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thống kê: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadThongKeTienMuonVaPhat()
        {
            try
            {
                var cboThang = tabTienMuonVaPhat.Controls.Find("cboThangTienMuonVaPhat", false)[0] as ComboBox;
                var numNam = tabTienMuonVaPhat.Controls.Find("numNamTienMuonVaPhat", false)[0] as NumericUpDown;
                var dgv = tabTienMuonVaPhat.Controls.Find("dgvTienMuonVaPhat", false)[0] as DataGridView;

                int thang = (int)cboThang.SelectedItem;
                int nam = (int)numNam.Value;

                var results = docGiaDAO.GetTongTienMuonVaPhat(thang, nam);

                var displayData = results.Select(r => new
                {
                    MaDocGia = r.MaDocGia,
                    HoTen = r.HoTen,
                    TongTienMuon = r.TongTienMuon.ToString("N0") + " VNĐ",
                    TongTienPhat = r.TongTienPhat.ToString("N0") + " VNĐ",
                    TongCong = r.TongCong.ToString("N0") + " VNĐ"
                }).ToList();

                dgv.DataSource = displayData;

                // Thiết lập header
                if (dgv.Columns["MaDocGia"] != null)
                    dgv.Columns["MaDocGia"].HeaderText = "Mã ĐG";
                if (dgv.Columns["HoTen"] != null)
                    dgv.Columns["HoTen"].HeaderText = "Họ tên";
                if (dgv.Columns["TongTienMuon"] != null)
                    dgv.Columns["TongTienMuon"].HeaderText = "Tổng tiền mượn";
                if (dgv.Columns["TongTienPhat"] != null)
                    dgv.Columns["TongTienPhat"].HeaderText = "Tổng tiền phạt";
                if (dgv.Columns["TongCong"] != null)
                    dgv.Columns["TongCong"].HeaderText = "Tổng cộng";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thống kê: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadThongKeDocGiaMoi()
        {
            try
            {
                var numNam = tabDocGiaMoi.Controls.Find("numNamDocGiaMoi", false)[0] as NumericUpDown;
                var dgv = tabDocGiaMoi.Controls.Find("dgvDocGiaMoi", false)[0] as DataGridView;

                int nam = (int)numNam.Value;

                var results = docGiaDAO.GetThongKeDocGiaMoiTheoThang(nam);

                dgv.DataSource = results;

                // Thiết lập header
                if (dgv.Columns["Thang"] != null)
                    dgv.Columns["Thang"].HeaderText = "Tháng";
                if (dgv.Columns["TenThang"] != null)
                    dgv.Columns["TenThang"].HeaderText = "Tên tháng";
                if (dgv.Columns["SoDocGiaMoi"] != null)
                    dgv.Columns["SoDocGiaMoi"].HeaderText = "Số độc giả mới";
                if (dgv.Columns["Nam"] != null)
                    dgv.Columns["Nam"].HeaderText = "Năm";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thống kê: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}

