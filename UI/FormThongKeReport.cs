using Microsoft.Reporting.WinForms;
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
    public partial class FormThongKeReport : Form
    {
        public FormThongKeReport(DataTable dtDocGiaMoiTheoThang, DataTable dtDoanhThuTheoThang, DataTable dtTop10, DataTable dtSachMuonTheoTheLoai, DataTable dtSachMuonTheoDocGia)
        {
            InitializeComponent();

            reportViewer1.LocalReport.ReportPath = "ThongKeReport.rdlc";
            reportViewer1.LocalReport.DataSources.Clear();

            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DocGiaMoiTheoThangDataSet", dtDocGiaMoiTheoThang));
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DoangThuTheoThangDataSet", dtDoanhThuTheoThang));
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Top10SachMuonDataSet", dtTop10));
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ThongKeSachMuonTheoTheLoaiDataSet", dtSachMuonTheoTheLoai));
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ThongKeSachMuonTheoDocGiaDataSet", dtSachMuonTheoDocGia));
            

            reportViewer1.RefreshReport();
        }

        private void FormThongKeReport_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }
    }
}
