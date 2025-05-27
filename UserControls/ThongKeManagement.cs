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
using LibraryManagement.Models;

namespace LibraryManagement.UserControls
{
    public partial class ThongKeManagement : UserControl
    {
        private readonly ThongKeDAO thongKeDAO = new ThongKeDAO();
        private bool dataLoaded = false;
        private bool isLoading = false;

        public ThongKeManagement()
        {
            InitializeComponent();
            this.Load += ThongKeManagement_Load;

            // ✅ Đăng ký DataBindingComplete event
            dgvThongKe.DataBindingComplete += DgvThongKe_DataBindingComplete;

            // Khởi tạo giá trị mặc định cho ComboBox
            InitializeComboBoxes();

            // Thiết lập style cho charts
            SetupChartStyles();
        }

        // ✅ Event handler cho DataBindingComplete
        private void DgvThongKe_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            SetupTableHeaders();
            StyleDataGridView();
        }

        private void InitializeComboBoxes()
        {
            try
            {
                // Set default values
                string currentYear = DateTime.Now.Year.ToString();
                string currentMonth = DateTime.Now.Month.ToString();

                // Kiểm tra và set giá trị cho cboNam
                if (cboNam.Items.Contains(currentYear))
                {
                    cboNam.SelectedItem = currentYear;
                }
                else
                {
                    cboNam.SelectedIndex = cboNam.Items.Count > 5 ? 5 : 0; // Default to 2025 or first item
                }

                // Kiểm tra và set giá trị cho cboThang
                if (cboThang.Items.Contains(currentMonth))
                {
                    cboThang.SelectedItem = currentMonth;
                }
                else
                {
                    cboThang.SelectedIndex = 0; // Default to January
                }
            }
            catch (Exception ex)
            {
                // Fallback values
                if (cboNam.Items.Count > 0) cboNam.SelectedIndex = 5; // 2025
                if (cboThang.Items.Count > 0) cboThang.SelectedIndex = 0; // January
            }
        }

        private void SetupChartStyles()
        {
            // Style cho chartDocGia
            chartDocGia.BackColor = Color.Transparent;
            chartDocGia.ChartAreas[0].BackColor = Color.White;
            chartDocGia.ChartAreas[0].BorderColor = Color.LightGray;
            chartDocGia.ChartAreas[0].BorderWidth = 1;
            chartDocGia.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chartDocGia.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            chartDocGia.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 9F);
            chartDocGia.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Segoe UI", 9F);

            // Style cho chartTien
            chartTien.BackColor = Color.Transparent;
            chartTien.ChartAreas[0].BackColor = Color.White;
            chartTien.ChartAreas[0].BorderColor = Color.LightGray;
            chartTien.ChartAreas[0].BorderWidth = 1;
            chartTien.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chartTien.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            chartTien.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 9F);
            chartTien.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Segoe UI", 9F);
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
            if (isLoading) return;

            try
            {
                isLoading = true;
                btnExport.Enabled = false;
                btnExport.Text = "Đang tải...";

                // Kiểm tra null và validate ComboBox values
                if (cboNam.SelectedItem == null || cboThang.SelectedItem == null)
                {
                    InitializeComboBoxes();
                    return;
                }

                if (!int.TryParse(cboNam.SelectedItem.ToString(), out int nam) ||
                    !int.TryParse(cboThang.SelectedItem.ToString(), out int thang))
                {
                    MessageBox.Show("Vui lòng chọn năm và tháng hợp lệ!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate year and month range
                if (nam < 2020 || nam > 2030 || thang < 1 || thang > 12)
                {
                    MessageBox.Show("Năm phải từ 2020-2030 và tháng từ 1-12!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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
            finally
            {
                isLoading = false;
                btnExport.Enabled = true;
                btnExport.Text = "📋 Xuất báo cáo";
            }
        }

        private void LoadThongKeDocGiaChart(int nam)
        {
            try
            {
                var data = thongKeDAO.GetThongKeDocGiaTheoThang(nam);

                chartDocGia.Series.Clear();
                Series series = new Series("Độc giả mới");
                series.ChartType = SeriesChartType.Column;
                series.Color = Color.FromArgb(115, 154, 79);
                series.BorderWidth = 2;
                series.ShadowOffset = 2;
                series.Font = new Font("Segoe UI", 9F);

                // Add data for all 12 months
                for (int i = 1; i <= 12; i++)
                {
                    var monthData = data.FirstOrDefault(d => d.Thang == i);
                    int count = monthData?.SoLuongDocGiaMoi ?? 0;
                    var point = series.Points.AddXY($"T{i}", count);

                    // Add data labels
                    series.Points[point].Label = count.ToString();
                    series.Points[point].LabelForeColor = Color.Black;
                }

                chartDocGia.Series.Add(series);
                chartDocGia.ChartAreas[0].AxisX.Title = "Tháng";
                chartDocGia.ChartAreas[0].AxisY.Title = "Số lượng độc giả";
                chartDocGia.ChartAreas[0].AxisX.TitleFont = new Font("Segoe UI", 10F, FontStyle.Bold);
                chartDocGia.ChartAreas[0].AxisY.TitleFont = new Font("Segoe UI", 10F, FontStyle.Bold);

                chartDocGia.Titles.Clear();
                var title = chartDocGia.Titles.Add($"Thống kê độc giả mới năm {nam}");
                title.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                title.ForeColor = Color.FromArgb(115, 154, 79);

                // Add legend
                chartDocGia.Legends.Clear();
                Legend legend = new Legend();
                legend.Font = new Font("Segoe UI", 9F);
                chartDocGia.Legends.Add(legend);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải biểu đồ độc giả: {ex.Message}", "Lỗi");
            }
        }

        private void LoadThongKeTienChart(int nam)
        {
            try
            {
                var data = thongKeDAO.GetThongKeTienTheoThang(nam);

                chartTien.Series.Clear();

                Series seriesMuon = new Series("Tiền mượn");
                seriesMuon.ChartType = SeriesChartType.Column;
                seriesMuon.Color = Color.FromArgb(40, 167, 69);
                seriesMuon.BorderWidth = 2;
                seriesMuon.ShadowOffset = 2;

                Series seriesPhat = new Series("Tiền phạt");
                seriesPhat.ChartType = SeriesChartType.Column;
                seriesPhat.Color = Color.FromArgb(220, 53, 69);
                seriesPhat.BorderWidth = 2;
                seriesPhat.ShadowOffset = 2;

                // Add data for all 12 months
                for (int i = 1; i <= 12; i++)
                {
                    var monthData = data.FirstOrDefault(d => d.Thang == i);
                    decimal tienMuon = monthData?.TongTienMuon ?? 0;
                    decimal tienPhat = monthData?.TongTienPhat ?? 0;

                    seriesMuon.Points.AddXY($"T{i}", (double)tienMuon);
                    seriesPhat.Points.AddXY($"T{i}", (double)tienPhat);
                }

                chartTien.Series.Add(seriesMuon);
                chartTien.Series.Add(seriesPhat);

                chartTien.ChartAreas[0].AxisX.Title = "Tháng";
                chartTien.ChartAreas[0].AxisY.Title = "Số tiền (VNĐ)";
                chartTien.ChartAreas[0].AxisX.TitleFont = new Font("Segoe UI", 10F, FontStyle.Bold);
                chartTien.ChartAreas[0].AxisY.TitleFont = new Font("Segoe UI", 10F, FontStyle.Bold);

                // Format Y-axis to show currency
                chartTien.ChartAreas[0].AxisY.LabelStyle.Format = "N0";

                chartTien.Titles.Clear();
                var title = chartTien.Titles.Add($"Thống kê doanh thu năm {nam}");
                title.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                title.ForeColor = Color.FromArgb(115, 154, 79);

                // Add legend
                chartTien.Legends.Clear();
                Legend legend = new Legend();
                legend.Font = new Font("Segoe UI", 9F);
                legend.Docking = Docking.Top;
                chartTien.Legends.Add(legend);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải biểu đồ doanh thu: {ex.Message}", "Lỗi");
            }
        }

        // ✅ Đã sửa: Xóa SetupTableHeaders và StyleDataGridView khỏi method này
        private void LoadThongKeTienMuonTable(int thang, int nam)
        {
            try
            {
                var data = thongKeDAO.GetThongKeTienMuonDocGia(thang, nam);

                if (data == null || data.Count == 0)
                {
                    // Show empty message without STT to avoid null reference
                    var emptyData = new List<object>
                    {
                        new {
                            MaDocGia = "N/A",
                            HoTen = "Không có dữ liệu cho tháng này",
                            TongTienMuon = "0 VNĐ",
                            TongTienPhat = "0 VNĐ",
                            TongCong = "0 VNĐ",
                            SoLanMuon = 0,
                            LanMuonGanNhat = "N/A"
                        }
                    };
                    dgvThongKe.DataSource = emptyData;
                }
                else
                {
                    var displayData = data.Take(20).Select((x, index) => new // Top 20
                    {
                        STT = index + 1,
                        MaDocGia = x.MaDocGia,
                        HoTen = x.HoTen,
                        TongTienMuon = x.TongTienMuon.ToString("N0") + " VNĐ",
                        TongTienPhat = x.TongTienPhat.ToString("N0") + " VNĐ",
                        TongCong = x.TongCong.ToString("N0") + " VNĐ",
                        SoLanMuon = x.SoLanMuon,
                        LanMuonGanNhat = x.LanMuonGanNhat?.ToString("dd/MM/yyyy") ?? "Chưa mượn"
                    }).ToList();

                    dgvThongKe.DataSource = displayData;
                }

                // ❌ Xóa 2 dòng này - sẽ được gọi trong DataBindingComplete
                // SetupTableHeaders();
                // StyleDataGridView();

                // Update label with current filter
                lblThongKeThang.Text = $"🏆 Top 20 độc giả có chi phí cao nhất - Tháng {thang}/{nam}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải bảng thống kê: {ex.Message}", "Lỗi");
            }
        }

        // ✅ Thêm try-catch để tránh lỗi null reference
        private void SetupTableHeaders()
        {
            try
            {
                // Kiểm tra DataGridView và Columns
                if (dgvThongKe?.Columns == null) return;

                if (dgvThongKe.Columns["STT"] != null)
                {
                    dgvThongKe.Columns["STT"].HeaderText = "STT";
                    dgvThongKe.Columns["STT"].Width = 50;
                }
                if (dgvThongKe.Columns["MaDocGia"] != null)
                {
                    dgvThongKe.Columns["MaDocGia"].HeaderText = "Mã ĐG";
                    dgvThongKe.Columns["MaDocGia"].Width = 80;
                }
                if (dgvThongKe.Columns["HoTen"] != null)
                {
                    dgvThongKe.Columns["HoTen"].HeaderText = "Họ tên";
                    dgvThongKe.Columns["HoTen"].Width = 150;
                }
                if (dgvThongKe.Columns["TongTienMuon"] != null)
                {
                    dgvThongKe.Columns["TongTienMuon"].HeaderText = "Tiền mượn";
                    dgvThongKe.Columns["TongTienMuon"].Width = 120;
                }
                if (dgvThongKe.Columns["TongTienPhat"] != null)
                {
                    dgvThongKe.Columns["TongTienPhat"].HeaderText = "Tiền phạt";
                    dgvThongKe.Columns["TongTienPhat"].Width = 120;
                }
                if (dgvThongKe.Columns["TongCong"] != null)
                {
                    dgvThongKe.Columns["TongCong"].HeaderText = "Tổng cộng";
                    dgvThongKe.Columns["TongCong"].Width = 130;
                    dgvThongKe.Columns["TongCong"].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    dgvThongKe.Columns["TongCong"].DefaultCellStyle.ForeColor = Color.Red;
                }
                if (dgvThongKe.Columns["SoLanMuon"] != null)
                {
                    dgvThongKe.Columns["SoLanMuon"].HeaderText = "Số lần mượn";
                    dgvThongKe.Columns["SoLanMuon"].Width = 100;
                }
                if (dgvThongKe.Columns["LanMuonGanNhat"] != null)
                {
                    dgvThongKe.Columns["LanMuonGanNhat"].HeaderText = "Lần mượn gần nhất";
                    dgvThongKe.Columns["LanMuonGanNhat"].Width = 130;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi setup table headers: {ex.Message}");
            }
        }

        private void StyleDataGridView()
        {
            try
            {
                // Header style
                dgvThongKe.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(115, 154, 79);
                dgvThongKe.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvThongKe.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                dgvThongKe.ColumnHeadersHeight = 35;
                dgvThongKe.EnableHeadersVisualStyles = false;

                // Row style
                dgvThongKe.RowTemplate.Height = 30;
                dgvThongKe.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
                dgvThongKe.DefaultCellStyle.SelectionBackColor = Color.FromArgb(115, 154, 79);
                dgvThongKe.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvThongKe.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);

                // Border
                dgvThongKe.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvThongKe.GridColor = Color.LightGray;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi style DataGridView: {ex.Message}");
            }
        }

        private void CboNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataLoaded && !isLoading)
            {
                LoadData();
            }
        }

        private void CboThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataLoaded && !isLoading)
            {
                LoadData();
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Chức năng xuất báo cáo Excel đang được phát triển!\n\nDữ liệu hiện tại có thể copy từ bảng thống kê.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
