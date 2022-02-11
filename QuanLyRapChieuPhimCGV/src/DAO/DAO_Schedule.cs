using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_Schedule
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_Schedule()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<Schedule> getAll()//Lấy tất cả danh sách phim
        {
            List<Schedule> schedules = new List<Schedule>();
            try
            {
                DAO_Movie dao_m = new DAO_Movie();
                DAO_Room dao_r = new DAO_Room();
                cnn.Open();
                string query = "select malich, maphim, ngaychieu, maphong from lichchieu";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    Schedule schedule = new Schedule()
                    {
                        id = reader.GetString(0),
                        movie = dao_m.getById(reader.GetString(1)),
                        dateTime = reader.GetDateTime(2),
                        room = dao_r.getById(reader.GetString(3))
                    };
                    schedules.Add(schedule);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return schedules;
        }

        public List<Schedule> getAllByMovieCanWatch(Movie movie)//Lấy tất cả lịch chiếu chưa chiếu của phim
        {
            List<Schedule> schedules = new List<Schedule>();
            try
            {
                DAO_Movie dao_m = new DAO_Movie();
                DAO_Room dao_r = new DAO_Room();
                cnn.Open();
                string query = $@"SELECT * FROM LICHCHIEU
                        WHERE NGAYCHIEU >= DATEADD(minute, 15, GETDATE()) AND MAPHIM = '{movie.id}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    Schedule schedule = new Schedule()
                    {
                        id = reader.GetString(0),
                        movie = dao_m.getById(reader.GetString(1)),
                        dateTime = reader.GetDateTime(2),
                        room = dao_r.getById(reader.GetString(3))
                    };
                    schedules.Add(schedule);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return schedules;
        }
        public Schedule getById(string scheduleId)//Lấy phim theo mã phim
        {
            Schedule schedule = null;
            try
            {
                DAO_Movie dao_m = new DAO_Movie();
                DAO_Room dao_r = new DAO_Room();
                cnn.Open();
                string query = $"select malich, maphim, ngaychieu, maphong from lichchieu where malich = '{scheduleId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                if (reader.Read())
                {
                    schedule = new Schedule()
                    {
                        id = reader.GetString(0),
                        movie = dao_m.getById(reader.GetString(1)),
                        dateTime = reader.GetDateTime(2),
                        room = dao_r.getById(reader.GetString(3))
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return schedule;
        }
        public Schedule getByDateAndMovie(Movie movie, DateTime date)//Lấy phim theo mã phim
        {
            Schedule schedule = null;
            try
            {
                DAO_Movie dao_m = new DAO_Movie();
                DAO_Room dao_r = new DAO_Room();
                cnn.Open();
                string query = $"select malich, maphim, ngaychieu, maphong from lichchieu where maphim = '{movie.id}' and ngaychieu = '{date}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                if (reader.Read())
                {
                    schedule = new Schedule()
                    {
                        id = reader.GetString(0),
                        movie = dao_m.getById(reader.GetString(1)),
                        dateTime = reader.GetDateTime(2),
                        room = dao_r.getById(reader.GetString(3))
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
            return schedule;
        }

        public bool canInsertSchedule(Schedule schedule)//Kiểm tra lịch này đã có chưa
        {
            bool result = true;
            try
            {
                cnn.Open();
                string query = $@"SELECT *
                                    FROM LICHCHIEU WHERE MALICH != '{schedule.id}' AND MAPHONG = '{schedule.room.id}' AND
                                    datediff(minute, '{schedule.dateTime}', NGAYCHIEU) < {schedule.movie.length + 30}  AND 
                                    datediff(minute, '{schedule.dateTime}', NGAYCHIEU) > -{schedule.movie.length + 30}";
                scm = new SqlCommand(query, cnn);
                Console.WriteLine(query);
                reader = scm.ExecuteReader();
                if (reader.Read())
                {
                    result = false;
                }
                else
                {
                    result = true;
                    DateTime now = DateTime.Now;
                    DateTime d1 = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
                    DateTime d2 = new DateTime(schedule.dateTime.Year, schedule.dateTime.Month, schedule.dateTime.Day, schedule.dateTime.Hour, schedule.dateTime.Minute, 0);
                    Console.WriteLine(DateTime.Compare(d1, d2));
                    if (DateTime.Compare(d1, d2) > 0)
                    {
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }

            return result;
        }
        public void insertOne(Schedule schedule)//Thêm phim
        {
            try
            {
                cnn.Open();
                string query = $@"insert into lichchieu(malich, maphim, ngaychieu, maphong)
                           values ('{schedule.id}','{schedule.movie.id}','{schedule.dateTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{schedule.room.id}')";
                scm = new SqlCommand(query, cnn);
                scm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
        }

        public void updateOne(Schedule schedule)//Cập nhật phim
        {
            try
            {
                cnn.Open();
                string query = $@"update lichchieu set maphim = '{schedule.movie.id}', maphong = '{schedule.room.id}', 
                    ngaychieu = '{schedule.dateTime.ToString("yyyy-MM-dd HH:mm:ss")}' where malich = '{schedule.id}'";
                scm = new SqlCommand(query, cnn);
                scm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
        }

        public void deleteById(string scheduleId)//Xoá lịch chiếu theo mã lịch
        {
            try
            {
                cnn.Open();
                string query = $@"DELETE FROM VE WHERE MALICH = '{scheduleId}';delete from lichchieu where malich = '{scheduleId}'";
                scm = new SqlCommand(query, cnn);
                scm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cnn.Close();
            }
        }

        public string generateId()//Tạo mã phim
        {
            Methods methods = new Methods();
            string result = "";
            string str_now = DateTime.Now.Year.ToString().Substring(2, 2);//2 chữ số đầu tiên là năm hiện tại
            int i = 1;

            string str = str_now + methods.addZero(7, i);
            while (true)
            {
                if (getById(str) == null)
                {
                    result += str;
                    break;
                }
                else
                {
                    str = str_now + methods.addZero(7, ++i);
                }
            }

            return result;
        }
    }


}