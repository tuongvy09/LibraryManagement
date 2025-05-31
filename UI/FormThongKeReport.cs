using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.IO;

namespace LibraryManagement.UI
{
    public partial class FormThongKeReport : Form
    {
        public FormThongKeReport(DataTable dtChiPhi, DataTable dtSachMuon, DataTable dtTop10)
        {
            InitializeComponent();

            reportViewer1.LocalReport.ReportPath = "ThongKeReport.rdlc";
            reportViewer1.LocalReport.DataSources.Clear();

            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ThongKeChiPhiDataSet", dtChiPhi));
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ThongKeSachMuonDataSet", dtSachMuon));
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Top10SachMuonDataSet", dtTop10));

            reportViewer1.RefreshReport();
        }

        private void FormThongKeReport_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }
    }
}
