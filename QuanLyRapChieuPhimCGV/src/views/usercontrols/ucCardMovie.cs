using QuanLyRapChieuPhimCGV.src.DAO;
using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.views.forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyRapChieuPhimCGV.src.views.usercontrols
{
    public partial class ucCardMovie : UserControl
    {
        private fSelectSchedule preComponent;
        private Movie movie;
        private List<Schedule> schedules = new List<Schedule>();
        private DAO_Schedule dao_s = new DAO_Schedule();
        private DateTime date;
        private string time = "";
        public ucCardMovie()
        {
            InitializeComponent();
        }
        public ucCardMovie(fSelectSchedule f, Movie m, DateTime dt)
        {
            date = dt;
            preComponent = f;
            movie = m;
            InitializeComponent();
            if (movie != null)
            {
                lblTitle.Text = $"{movie.name}";
                lblInfoMovie.Text = $"Đạo diễn: {movie.producer}\nThể loại: {"Chưa xử lý"}\nKhởi chiếu: {movie.dateStart.ToString("dd-MM-yyyy")}\nThời lượng: {movie.length} phút\nNgôn ngữ: {movie.language}\nĐộ tuổi phù hợp: từ {movie.ageLimit} trở lên\n\n{movie.description}";
                pictureMovie.Image = Image.FromFile(movie.image);
                schedules = dao_s.getAllByMovieCanWatch(movie);
                schedules.ForEach(schedule =>
                {
                    if(DateTime.Compare(schedule.dateTime, date) >= 0)
                    {
                        Button btnTime = new Button();
                        btnTime.AutoSize = true;
                        btnTime.Text = $"{schedule.dateTime.ToString("dd/MM/yyyy HH:mm")}";
                        btnTime.Cursor = Cursors.Hand;
                        btnTime.BackColor = Color.FromArgb(187, 187, 251);
                        btnTime.FlatAppearance.BorderSize = 0;
                        btnTime.FlatStyle = FlatStyle.Flat;
                        btnTime.Click += new EventHandler(selectTime);
                        flowLayoutPanel1.Controls.Add(btnTime);
                    }
                });
            }
        }

        private void selectTime(object sender, EventArgs e)
        {
            time = (sender as Button).Text;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            preComponent.getTime(movie, time);
        }
    }
}
