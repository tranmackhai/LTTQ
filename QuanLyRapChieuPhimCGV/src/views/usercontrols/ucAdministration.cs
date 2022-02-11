using QuanLyRapChieuPhimCGV.src.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV.src.views.usercontrols
{
    public partial class ucAdministration : UserControl
    {
        private Employee employee;
        public ucAdministration(Employee e)
        {
            employee = e;
            InitializeComponent();
            Dock = DockStyle.Fill;
        }
        private void btnBillItem_Click(object sender, EventArgs e)
        {
            Parent.Controls.Add(new ucBill(this, employee));
            Parent.Controls.Remove(this);
        }

        private void btnEmployeeItem_Click(object sender, EventArgs e)
        {
            Parent.Controls.Add(new ucEmployee(this, employee));
            Parent.Controls.Remove(this);
        }

        private void btnFoodItem_Click(object sender, EventArgs e)
        {
            Parent.Controls.Add(new ucFood(this, employee));
            Parent.Controls.Remove(this);
        }

        private void btnPriceTicketItem_Click(object sender, EventArgs e)
        {
            Parent.Controls.Add(new ucTicketPrice(this, employee));
            Parent.Controls.Remove(this);
        }

        private void btnTicketItem_Click(object sender, EventArgs e)
        {
            Parent.Controls.Add(new ucBookTicket(this, employee));
            Parent.Controls.Remove(this);
        }

        private void btnScheduleItem_Click(object sender, EventArgs e)
        {
            Parent.Controls.Add(new ucSchedule(this, employee));
            Parent.Controls.Remove(this);
        }

        private void btnGroupFoodItem_Click(object sender, EventArgs e)
        {
            Parent.Controls.Add(new ucGroupFood(this, employee));
            Parent.Controls.Remove(this);
        }

        private void btnChairTypeItem_Click(object sender, EventArgs e)
        {
            Parent.Controls.Add(new ucChairType(this, employee));
            Parent.Controls.Remove(this);
        }

        private void btnCardItem_Click(object sender, EventArgs e)
        {
            Parent.Controls.Add(new ucCustomerCard(this, employee));
            Parent.Controls.Remove(this);
        }

        private void btnCustomerItem_Click(object sender, EventArgs e)
        {
            Parent.Controls.Add(new ucCustomer(this, employee));
            Parent.Controls.Remove(this);
        }

        private void btnChairItem_Click(object sender, EventArgs e)
        {
            Parent.Controls.Add(new ucChair(this, employee));
            Parent.Controls.Remove(this);
        }

        private void btnScreenItem_Click(object sender, EventArgs e)
        {
            Parent.Controls.Add(new ucScreen(this, employee));
            Parent.Controls.Remove(this);
        }

        private void btnTheaterItem_Click(object sender, EventArgs e)
        {
            Parent.Controls.Add(new ucRoom(this, employee));
            Parent.Controls.Remove(this);
        }

        private void btnMovieItem_Click(object sender, EventArgs e)
        {
            Parent.Controls.Add(new ucMovie(this, employee));
            Parent.Controls.Remove(this);
        }

        private void btnCategoryMovieItem_Click(object sender, EventArgs e)
        {
            Parent.Controls.Add(new ucCategoryMovie(this, employee));
            Parent.Controls.Remove(this);
        }

        private void btnDistributorItem_Click(object sender, EventArgs e)
        {
            Parent.Controls.Add(new ucReportStatistic());
            Parent.Controls.Remove(this);
        }
    }
}
