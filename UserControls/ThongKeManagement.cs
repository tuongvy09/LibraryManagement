using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using LibraryManagement.Repositories;


namespace LibraryManagement.UserControls
{
    public partial class ThongKeManagement : UserControl
    {
        private readonly ThongKeDAO thongKeDAO = new ThongKeDAO();
        private bool dataLoaded = false;

        public ThongKeManagement()
        {
            InitializeComponent();
            this.Load += ThongKeManagement_Load;

            // Khởi tạo năm mặc định
            cboNam.SelectedItem = DateTime.Now.Year.ToString();
            cboThang.SelectedItem = DateTime.Now.Month.ToString();
        }

        private void ThongKeManagement_Load(object sender, EventArgs e)
        {
            if (!dataLoaded)
            {
                LoadData();
            }
        }

        public void InitializeData()
        {
            if (!dataLoaded)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            try
            {
                int nam = int.Parse(cboNam.SelectedItem.ToString());
                int thang = int.Parse(cboThang.SelectedItem.ToString());

                LoadThongKeDocGiaChart(nam);
                LoadThongKeTienChart(nam);
                LoadThongKeTienMuonTable(thang, nam);

                dataLoaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu thống kê: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadThongKeDocGiaChart(int nam)
        {
            var data = thongKeDAO.GetThongKeDocGiaTheoThang(nam);

            chartDocGia.Series.Clear();
            Series series = new Series("Độc giả mới");
            series.ChartType = SeriesChartType.Column;
            series.Color = Color.FromArgb(115, 154, 79);

            foreach (var item in data)
            {
                series.Points.AddXY($"T{item.Thang}", item.SoLuongDocGiaMoi);
            }

            chartDocGia.Series.Add(series);
            chartDocGia.ChartAreas[0].AxisX.Title = "Tháng";
            chartDocGia.ChartAreas[0].AxisY.Title = "Số lượng độc giả";
            chartDocGia.Titles.Clear();
            chartDocGia.Titles.Add($"Thống kê độc giả mới năm {nam}");
        }

        private void LoadThongKeTienChart(int nam)
        {
            var data = thongKeDAO.GetThongKeTienTheoThang(nam);

            chartTien.Series.Clear();

            Series seriesMuon = new Series("Tiền mượn");
            seriesMuon.ChartType = SeriesChartType.Column;
            seriesMuon.Color = Color.FromArgb(40, 167, 69);

            Series seriesPhat = new Series("Tiền phạt");
            seriesPhat.ChartType = SeriesChartType.Column;
            seriesPhat.Color = Color.FromArgb(220, 53, 69);

            foreach (var item in data)
            {
                seriesMuon.Points.AddXY($"T{item.Thang}", (double)item.TongTienMuon);
                seriesPhat.Points.AddXY($"T{item.Thang}", (double)item.TongTienPhat);
            }

            chartTien.Series.Add(seriesMuon);
            chartTien.Series.Add(seriesPhat);
            chartTien.ChartAreas[0].AxisX.Title = "Tháng";
            chartTien.ChartAreas[0].AxisY.Title = "Số tiền (VNĐ)";
            chartTien.Titles.Clear();
            chartTien.Titles.Add($"Thống kê doanh thu năm {nam}");
        }

        private void LoadThongKeTienMuonTable(int thang, int nam)
        {
            var data = thongKeDAO.GetThongKeTienMuonDocGia(thang, nam);

            var displayData = data.Select(x => new
            {
                MaDocGia = x.MaDocGia,
                HoTen = x.HoTen,
                TongTienMuon = x.TongTienMuon.ToString("N0") + " VNĐ",
                TongTienPhat = x.TongTienPhat.ToString("N0") + " VNĐ",
                TongCong = x.TongCong.ToString("N0") + " VNĐ",
                SoLanMuon = x.SoLanMuon,
                LanMuonGanNhat = x.LanMuonGanNhat?.ToString("dd/MM/yyyy") ?? "Chưa mượn"
            }).ToList();

            dgvThongKe.DataSource = displayData;
            SetupTableHeaders();
        }

        private void SetupTableHeaders()
        {
            if (dgvThongKe.Columns["MaDocGia"] != null)
                dgvThongKe.Columns["MaDocGia"].HeaderText = "Mã ĐG";
            if (dgvThongKe.Columns["HoTen"] != null)
                dgvThongKe.Columns["HoTen"].HeaderText = "Họ tên";
            if (dgvThongKe.Columns["TongTienMuon"] != null)
                dgvThongKe.Columns["TongTienMuon"].HeaderText = "Tiền mượn";
            if (dgvThongKe.Columns["TongTienPhat"] != null)
                dgvThongKe.Columns["TongTienPhat"].HeaderText = "Tiền phạt";
            if (dgvThongKe.Columns["TongCong"] != null)
                dgvThongKe.Columns["TongCong"].HeaderText = "Tổng cộng";
            if (dgvThongKe.Columns["SoLanMuon"] != null)
                dgvThongKe.Columns["SoLanMuon"].HeaderText = "Số lần mượn";
            if (dgvThongKe.Columns["LanMuonGanNhat"] != null)
                dgvThongKe.Columns["LanMuonGanNhat"].HeaderText = "Lần mượn gần nhất";
        }

        private void CboNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void CboThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            // Xuất báo cáo (có thể implement sau)
            MessageBox.Show("Chức năng xuất báo cáo đang được phát triển!", "Thông báo");
        }
    }
}

