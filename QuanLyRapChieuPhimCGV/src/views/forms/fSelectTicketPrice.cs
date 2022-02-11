using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using QuanLyRapChieuPhimCGV.src.views.usercontrols;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV.src.views.forms
{
    public partial class fSelectTicketPrice : Form
    {
        private ucBookTicket preComponent;
        private DAO_TicketPrice dao_tp = new DAO_TicketPrice();
        private List<TicketPrice> ticketPrices = new List<TicketPrice>();
        private Methods myMethod = new Methods();
        public fSelectTicketPrice(ucBookTicket uc)
        {
            preComponent = uc;
            InitializeComponent();

            ticketPrices = dao_tp.getAllByNow();
            ticketPrices.ForEach(ticketPrice =>
            {
                dgvTicketPrice.Rows.Add(new object[]
                {
                    ticketPrice.id, ticketPrice.startDate, ticketPrice.endDate, ticketPrice.price.ToString("#,##"), ticketPrice.objectPerson
                });
            });
        }

        private void fSelectTicketPrice_Load(object sender, EventArgs e)
        {

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dgvTicketPrice.SelectedRows.Count > 0)
            {
                TicketPrice ticketPrice = ticketPrices.Find(tp => tp.id == dgvTicketPrice.Rows[dgvTicketPrice.SelectedRows[0].Index].Cells[0].Value.ToString());
                if (ticketPrice != null)
                {
                    preComponent.getTicketPrice(ticketPrice);
                }
            }
            Close();
        }
    }
}
