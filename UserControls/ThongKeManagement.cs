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
        private bool columnsInitialized = false;

        public ThongKeManagement()
        {
            InitializeComponent();
            this.Load += ThongKeManagement_Load;

            // Khởi tạo columns ngay từ đầu
            KhoiTaoDataGridViewColumns();

            // Khởi tạo giá trị mặc định cho ComboBox
            InitializeComboBoxes();

            // Thiết lập style cho charts
            SetupChartStyles();
        }

        private void KhoiTaoDataGridViewColumns()
        {
            try
            {
                if (columnsInitialized) return;

                dgvThongKe.AutoGenerateColumns = false;
                dgvThongKe.Columns.Clear();

                // Tạo từng column một cách manual
                DataGridViewTextBoxColumn colSTT = new DataGridViewTextBoxColumn
                {
                    Name = "STT",
                    HeaderText = "STT",
                    DataPropertyName = "STT",
                    Width = 50,
                    ReadOnly = true
                };
                dgvThongKe.Columns.Add(colSTT);

                DataGridViewTextBoxColumn colMaDocGia = new DataGridViewTextBoxColumn
                {
                    Name = "MaDocGia",
                    HeaderText = "Mã ĐG",
                    DataPropertyName = "MaDocGia",
                    Width = 80,
                    ReadOnly = true
                };
                dgvThongKe.Columns.Add(colMaDocGia);

                DataGridViewTextBoxColumn colHoTen = new DataGridViewTextBoxColumn
                {
                    Name = "HoTen",
                    HeaderText = "Họ tên",
                    DataPropertyName = "HoTen",
                    Width = 150,
                    ReadOnly = true
                };
                dgvThongKe.Columns.Add(colHoTen);

                DataGridViewTextBoxColumn colTongTienMuon = new DataGridViewTextBoxColumn
                {
                    Name = "TongTienMuon",
                    HeaderText = "Tiền mượn",
                    DataPropertyName = "TongTienMuon",
                    Width = 120,
                    ReadOnly = true
                };
                dgvThongKe.Columns.Add(colTongTienMuon);

                DataGridViewTextBoxColumn colTongTienPhat = new DataGridViewTextBoxColumn
                {
                    Name = "TongTienPhat",
                    HeaderText = "Tiền phạt",
                    DataPropertyName = "TongTienPhat",
                    Width = 120,
                    ReadOnly = true
                };
                dgvThongKe.Columns.Add(colTongTienPhat);

                DataGridViewTextBoxColumn colTongCong = new DataGridViewTextBoxColumn
                {
                    Name = "TongCong",
                    HeaderText = "Tổng cộng",
                    DataPropertyName = "TongCong",
                    Width = 130,
                    ReadOnly = true,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        ForeColor = Color.Red
                    }
                };
                dgvThongKe.Columns.Add(colTongCong);

                DataGridViewTextBoxColumn colSoLanMuon = new DataGridViewTextBoxColumn
                {
                    Name = "SoLanMuon",
                    HeaderText = "Số lần mượn",
                    DataPropertyName = "SoLanMuon",
                    Width = 100,
                    ReadOnly = true
                };
                dgvThongKe.Columns.Add(colSoLanMuon);

                DataGridViewTextBoxColumn colLanMuonGanNhat = new DataGridViewTextBoxColumn
                {
                    Name = "LanMuonGanNhat",
                    HeaderText = "Lần mượn gần nhất",
                    DataPropertyName = "LanMuonGanNhat",
                    Width = 130,
                    ReadOnly = true
                };
                dgvThongKe.Columns.Add(colLanMuonGanNhat);

                // Áp dụng style ngay
                StyleDataGridView();

                columnsInitialized = true;
                System.Diagnostics.Debug.WriteLine("DataGridView columns đã được khởi tạo thành công!");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khởi tạo columns: {ex.Message}");
                MessageBox.Show($"Lỗi khởi tạo bảng: {ex.Message}", "Lỗi");
            }
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

            // Style cho chartTien (doanh thu)
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
                LoadThongKeDoanhThuChart(nam); // ✅ Đổi tên method và gọi doanh thu
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

        // ✅ NEW - Method thống kê doanh thu từ bảng BienLai
        private void LoadThongKeDoanhThuChart(int nam)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"🔍 Bắt đầu load doanh thu cho năm {nam}");

                var data = thongKeDAO.GetThongKeDoanhThuTheoThang(nam);

                System.Diagnostics.Debug.WriteLine($"📊 Nhận được {data?.Count ?? 0} records từ DAO");

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        System.Diagnostics.Debug.WriteLine($"📈 Data: Tháng {item.Thang}, Doanh thu: {item.TongDoanhThu:N0}, Giao dịch: {item.SoGiaoDich}");
                    }
                }

                chartTien.Series.Clear();
                chartTien.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;

                // Series cho doanh thu (cột) - CHÍNH
                Series seriesDoanhThu = new Series("Doanh thu");
                seriesDoanhThu.ChartType = SeriesChartType.Column;
                seriesDoanhThu.Color = Color.FromArgb(40, 167, 69);
                seriesDoanhThu.BorderWidth = 0;
                seriesDoanhThu.IsValueShownAsLabel = true;

                // Series cho số giao dịch (đường) - PHỤ
                Series seriesGiaoDich = new Series("Số giao dịch");
                seriesGiaoDich.ChartType = SeriesChartType.Line;
                seriesGiaoDich.Color = Color.FromArgb(255, 193, 7);
                seriesGiaoDich.BorderWidth = 3;
                seriesGiaoDich.MarkerStyle = MarkerStyle.Circle;
                seriesGiaoDich.MarkerSize = 8;
                seriesGiaoDich.YAxisType = AxisType.Secondary;
                seriesGiaoDich.IsValueShownAsLabel = true;

                decimal maxDoanhThu = 0;
                int maxGiaoDich = 0;

                // Add data for all 12 months
                for (int i = 1; i <= 12; i++)
                {
                    var monthData = data?.FirstOrDefault(d => d.Thang == i);
                    decimal doanhThu = monthData?.TongDoanhThu ?? 0;
                    int soGiaoDich = monthData?.SoGiaoDich ?? 0;

                    System.Diagnostics.Debug.WriteLine($"📊 Tháng {i}: Doanh thu = {doanhThu:N0}, Giao dịch = {soGiaoDich}");

                    // Track max values
                    if (doanhThu > maxDoanhThu) maxDoanhThu = doanhThu;
                    if (soGiaoDich > maxGiaoDich) maxGiaoDich = soGiaoDich;

                    // Thêm điểm doanh thu
                    seriesDoanhThu.Points.AddXY($"T{i}", (double)doanhThu);

                    // Thêm điểm số giao dịch
                    seriesGiaoDich.Points.AddXY($"T{i}", soGiaoDich);
                }

                System.Diagnostics.Debug.WriteLine($"📊 Max Doanh thu: {maxDoanhThu:N0}, Max Giao dịch: {maxGiaoDich}");

                chartTien.Series.Add(seriesDoanhThu);
                chartTien.Series.Add(seriesGiaoDich);

                // Thiết lập trục
                chartTien.ChartAreas[0].AxisX.Title = "Tháng";
                chartTien.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)";
                chartTien.ChartAreas[0].AxisY2.Title = "Số giao dịch";
                chartTien.ChartAreas[0].AxisX.TitleFont = new Font("Segoe UI", 10F, FontStyle.Bold);
                chartTien.ChartAreas[0].AxisY.TitleFont = new Font("Segoe UI", 10F, FontStyle.Bold);
                chartTien.ChartAreas[0].AxisY2.TitleFont = new Font("Segoe UI", 10F, FontStyle.Bold);

                // Format Y-axis
                chartTien.ChartAreas[0].AxisY.LabelStyle.Format = "N0";

                // Set axis ranges để đảm bảo hiển thị
                if (maxDoanhThu > 0)
                {
                    chartTien.ChartAreas[0].AxisY.Minimum = 0;
                    chartTien.ChartAreas[0].AxisY.Maximum = (double)(maxDoanhThu * 1.2m);
                }

                if (maxGiaoDich > 0)
                {
                    chartTien.ChartAreas[0].AxisY2.Minimum = 0;
                    chartTien.ChartAreas[0].AxisY2.Maximum = maxGiaoDich * 1.2;
                }

                // Thiết lập tiêu đề
                chartTien.Titles.Clear();
                var title = chartTien.Titles.Add($"Thống kê doanh thu năm {nam}");
                title.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                title.ForeColor = Color.FromArgb(115, 154, 79);

                // Thêm legend
                chartTien.Legends.Clear();
                Legend legend = new Legend();
                legend.Font = new Font("Segoe UI", 9F);
                legend.Docking = Docking.Top;
                chartTien.Legends.Add(legend);

                // Force refresh
                chartTien.Invalidate();
                chartTien.Update();

                System.Diagnostics.Debug.WriteLine("✅ Hoàn thành load chart doanh thu");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi LoadThongKeDoanhThuChart: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"❌ Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Lỗi khi tải biểu đồ doanh thu: {ex.Message}", "Lỗi");
            }
        }

        private void LoadThongKeTienMuonTable(int thang, int nam)
        {
            try
            {
                // Đảm bảo columns đã được khởi tạo
                if (!columnsInitialized)
                {
                    KhoiTaoDataGridViewColumns();
                }

                var data = thongKeDAO.GetThongKeTienMuonDocGia(thang, nam);

                // Tạo DataTable đơn giản
                DataTable dt = new DataTable();
                dt.Columns.Add("STT", typeof(int));
                dt.Columns.Add("MaDocGia", typeof(string));
                dt.Columns.Add("HoTen", typeof(string));
                dt.Columns.Add("TongTienMuon", typeof(string));
                dt.Columns.Add("TongTienPhat", typeof(string));
                dt.Columns.Add("TongCong", typeof(string));
                dt.Columns.Add("SoLanMuon", typeof(int));
                dt.Columns.Add("LanMuonGanNhat", typeof(string));

                if (data == null || data.Count == 0)
                {
                    // Thêm dòng dữ liệu rỗng
                    dt.Rows.Add(1, "N/A", "Không có dữ liệu cho tháng này", "0 VNĐ", "0 VNĐ", "0 VNĐ", 0, "N/A");
                }
                else
                {
                    // Thêm dữ liệu thực
                    int index = 1;
                    foreach (var x in data.Take(20))
                    {
                        dt.Rows.Add(
                            index++,
                            x.MaDocGia.ToString(),
                            x.HoTen,
                            x.TongTienMuon.ToString("N0") + " VNĐ",
                            x.TongTienPhat.ToString("N0") + " VNĐ",
                            x.TongCong.ToString("N0") + " VNĐ",
                            x.SoLanMuon,
                            x.LanMuonGanNhat?.ToString("dd/MM/yyyy") ?? "Chưa mượn"
                        );
                    }
                }

                // Binding dữ liệu
                dgvThongKe.DataSource = dt;

                // Cập nhật label
                lblThongKeThang.Text = $"🏆 Top 20 độc giả có chi phí cao nhất - Tháng {thang}/{nam}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải bảng thống kê: {ex.Message}", "Lỗi");
                dgvThongKe.DataSource = null;
            }
        }

        private void StyleDataGridView()
        {
            try
            {
                if (dgvThongKe == null) return;

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

                // Misc
                dgvThongKe.AllowUserToAddRows = false;
                dgvThongKe.AllowUserToDeleteRows = false;
                dgvThongKe.ReadOnly = true;
                dgvThongKe.MultiSelect = false;
                dgvThongKe.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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
