using Microsoft.Reporting.WinForms;
using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;
using System;
using System.Data;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV.src.views.forms
{
    public partial class fReport : Form
    {
        public fReport()
        {
            InitializeComponent();
        }
        public fReport(Ticket ticket)
        {
            InitializeComponent();
            Text = "Vé xem phim";
            DAO_Chair dao_ch = new DAO_Chair();
            rpv.LocalReport.ReportPath = "../../src/views/report/Ticket.rdlc";
            ReportParameter rpt1 = new ReportParameter("typeCustomer", "Liên 2: Khách Hàng");
            ReportParameter rpt2 = new ReportParameter("ticketId", $"Số: {ticket.id}");
            ReportParameter rpt3 = new ReportParameter("ticketDate", $"{DateTime.Now}   {ticket.employee.name}");
            ReportParameter rpt4 = new ReportParameter("roomName", $"{ticket.schedule.room.name}    ");
            ReportParameter rpt5 = new ReportParameter("chairName", $"      {dao_ch.getChairName(ticket.chair)}");
            ReportParameter rpt6 = new ReportParameter("dateStart", $"{ticket.schedule.dateTime.ToString("dd/MM/yyyy HH:mm:ss")}");
            ReportParameter rpt7 = new ReportParameter("movieName", $"{ticket.schedule.movie.name}");
            ReportParameter rpt8 = new ReportParameter("ticketPrice", $"{ticket.price.price.ToString("#,##")}");
            ReportParameter rpt9 = new ReportParameter("point", $"{ticket.point}");
            ReportParameter rpt10 = new ReportParameter("totalPrice", $"{ticket.totalPrice.ToString("#,##")}");
            ReportParameter rpt11 = new ReportParameter("address", "Vincom Thủ Đức, 216 Đ. Võ Văn Ngân, Bình Thọ, Thủ Đức, Thành phố Hồ Chí Minh");
            rpv.LocalReport.SetParameters(new ReportParameter[]
                { rpt1, rpt2 , rpt3, rpt4,rpt5,rpt6, rpt7, rpt8, rpt9, rpt10, rpt11}
            );
            rpv.RefreshReport();
        }
        public fReport(Bill bill)
        {
            InitializeComponent();
            Text = "Hoá đơn thanh toán";
            rpv.LocalReport.ReportPath = "../../src/views/report/Bill.rdlc";
            DataTable table = new DataTable();
            table.Columns.Add("food");
            table.Columns.Add("quantity");
            table.Columns.Add("price");
            table.Columns.Add("totalPrice");

            DAO_BillDetail dao_bd = new DAO_BillDetail();
            dao_bd.getAll(bill).ForEach(detail =>
            {
                table.Rows.Add(detail.food.name, detail.quantity, detail.food.price.ToString("#,##"), (detail.food.price * detail.quantity).ToString("#,##"));
            });
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "BillDataSet";
            rds.Value = table;
            rpv.LocalReport.DataSources.Clear();
            rpv.LocalReport.DataSources.Add(rds);
            ReportParameter rpt1 = new ReportParameter("billId", $"Số: {bill.id}");
            ReportParameter rpt2 = new ReportParameter("date", $"{bill.date.ToString("dd/MM/yyyy HH:mm:ss")}");
            ReportParameter rpt3 = new ReportParameter("employee", $"{bill.employee.name}");
            ReportParameter rpt4 = new ReportParameter("price", $"{bill.totalPrice.ToString("#,##")}");
            ReportParameter rpt5 = new ReportParameter("point", $"{bill.point}");
            ReportParameter rpt6 = new ReportParameter("totalPrice", $"{bill.totalPrice.ToString("#,##")}");
            ReportParameter rpt7 = new ReportParameter("address", "Vincom Thủ Đức, 216 Đ. Võ Văn Ngân, Bình Thọ, Thủ Đức, Thành phố Hồ Chí Minh");
            rpv.LocalReport.SetParameters(new ReportParameter[]
                { rpt1, rpt2 , rpt3, rpt4,rpt5, rpt6, rpt7}
            );
            rpv.RefreshReport();
        }

        private void fReport_Load(object sender, EventArgs e)
        {
            this.rpv.RefreshReport();
        }
    }
}
