using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.views.usercontrols;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV.src.views.forms
{
    public partial class fSelectSchedule : Form
    {
        private ucBookTicket preComponent;
        private List<Movie> movies = new List<Movie>();
        private List<ucCardMovie> cards = new List<ucCardMovie>();
        private DAO_Movie dao_m = new DAO_Movie();
        public fSelectSchedule(ucBookTicket uc)
        {
            preComponent = uc;
            InitializeComponent();
            DateTime now = DateTime.Now;
            dao_m.getAllCanWatch(DateTime.Now).ForEach(movie =>
            {
                if(movies.FindIndex(m=>m.id == movie.id) == -1)
                {
                    movies.Add(movie);
                    cards.Add(new ucCardMovie(this, movie, now));
                    flowLayoutPanel1.Controls.Add(cards[cards.Count - 1]);
                }
            });
        }

        public void getTime(Movie movie, string time)
        {
            try
            {
                string[] splitDate = time.Split(' ')[0].Split('/');
                string[] splitTime = time.Split(' ')[1].Split(':');
                int day = Convert.ToInt32(splitDate[0]);
                int month = Convert.ToInt32(splitDate[1]);
                int year = Convert.ToInt32(splitDate[2]);
                int hour = Convert.ToInt32(splitTime[0]);
                int minute = Convert.ToInt32(splitTime[1]);
                DateTime dt = new DateTime(year, month, day, hour, minute, 0);
                preComponent.getSchedule(movie, dt);
                Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Chưa chọn thời gian chiếu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
