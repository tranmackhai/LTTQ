using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV.src.views.forms
{
    public partial class fStatistics : Form
    {
        public fStatistics(DataTable table, string title)
        {
            InitializeComponent();
            viewStatistic(table, title);
        }

        private void fStatistics_Load(object sender, EventArgs e)
        {

            rpv.RefreshReport();
        }

        public void viewStatistic(DataTable table, string title)
        {
            rpv.LocalReport.ReportPath = @"..\..\src\views\report\Statistics.rdlc";
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "RevenueDataSet";
            rds.Value = table;
            ReportParameter rptTitle = new ReportParameter("title", title);
            rpv.LocalReport.SetParameters(new ReportParameter[] { rptTitle });
            rpv.LocalReport.DataSources.Clear();
            rpv.LocalReport.DataSources.Add(rds);
            rpv.RefreshReport();
        }
    }
}
