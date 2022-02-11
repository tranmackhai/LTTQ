using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.views.forms;
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
    public partial class ucReportStatistic : UserControl
    {
        public DAO_Statistic dao_sta = new DAO_Statistic();
        public ucReportStatistic()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;

            revenueInDay();
            revenueInMonth();
            revenueInYear();

            cbFilterStatistic.SelectedIndex = 0;

            revenueOfMovies();
        }

        public void revenueInDay()
        {
            label1.Text = $"Ngày {dtDateStatistic.Value.Day}";
            label2.Text = $"{dao_sta.getRevenueInDay(dtDateStatistic.Value).ToString("#,##")}đ";
        }

        public void revenueInMonth()
        {
            label4.Text = $"Tháng {dtDateStatistic.Value.Month}";
            label3.Text = $"{dao_sta.getRevenueInMonth(dtDateStatistic.Value).ToString("#,##")}đ";
        }

        public void revenueInYear()
        {
            label6.Text = $"Năm {dtDateStatistic.Value.Year}";
            label5.Text = $"{dao_sta.getRevenueInYear(dtDateStatistic.Value).ToString("#,##")}đ";
        }

        public void statisticRevenueDaysOfWeek()
        {
            new fStatistics(dao_sta.getRevenueDaysOfWeek(dtDateStatistic.Value), $"DOANH THU CÁC NGÀY TRONG TUẦN ({dtDateStatistic.Value.ToString("dd/MM/yyyy")})").Visible = true;
        }

        public void statisticRevenueDaysOfMonth()
        {
            new fStatistics(dao_sta.getRevenueDaysOfMonth(dtDateStatistic.Value), $"DOANH THU CÁC NGÀY TRONG THÁNG {dtDateStatistic.Value.Month} NĂM {dtDateStatistic.Value.Year}").Visible = true;
        }

        public void statisticRevenueMonthsOfYear()
        {
            new fStatistics(dao_sta.getRevenueMonthsOfYear(dtDateStatistic.Value), $"DOANH THU CÁC THÁNG TRONG NĂM {dtDateStatistic.Value.Year}").Visible = true;
        }

        public void statisticRevenueQuartersOfYear()
        {
            new fStatistics(dao_sta.getRevenueQuartersOfYear(dtDateStatistic.Value), $"DOANH THU CÁC QUÝ TRONG NĂM {dtDateStatistic.Value.Year}").Visible = true;
        }

        public void statisticRevenueYears(int num)
        {
            new fStatistics(dao_sta.getRevenueYears(num, dtDateStatistic.Value), $"DOANH THU {num} NĂM GẦN ĐÂY").Visible = true;
        }

        public void statisticRevenue(int index)
        {
            if(index == 0)
            {
                statisticRevenueDaysOfWeek();
            }
            else if(index == 1)
            {
                statisticRevenueDaysOfMonth();
            }
            else if (index == 2)
            {
                statisticRevenueMonthsOfYear();
            }
            else if (index == 3)
            {
                statisticRevenueQuartersOfYear();
            }
            else if (index == 4)
            {
                statisticRevenueYears(3);
            }
        }

        private void dtDateStatistic_ValueChanged(object sender, EventArgs e)
        {
            revenueInDay();
            revenueInMonth();
            revenueInYear();
        }

        private void btnViewStatistic_Click(object sender, EventArgs e)
        {
            statisticRevenue(cbFilterStatistic.SelectedIndex);
        }

        public void revenueOfMovies()
        {
            try
            {
                DataTable table = dao_sta.getRevenueOfMovie("");
                dataGridView1.Rows.Clear();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    dataGridView1.Rows.Add(new object[]{
                    table.Rows[i].ItemArray[0], table.Rows[i].ItemArray[1], Convert.ToDecimal(table.Rows[i].ItemArray[2])
                });
                }
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void txtKeyword_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable table = dao_sta.getRevenueOfMovie(txtKeyword.Text);
                dataGridView1.Rows.Clear();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    dataGridView1.Rows.Add(new object[]{
                    table.Rows[i].ItemArray[0], table.Rows[i].ItemArray[1], Convert.ToDecimal(table.Rows[i].ItemArray[2])
                });
                }
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
