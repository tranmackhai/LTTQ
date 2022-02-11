using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_Screen
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_Screen()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<_Screen> getAll()//Lấy tất cả danh sách màn hình
        {
            List<_Screen> screens = new List<_Screen>();
            try
            {
                cnn.Open();
                string query = "select mamanhinh, tenmanhinh from manhinh";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    _Screen screen = new _Screen();
                    screen.id = reader.GetString(0);
                    screen.name = reader.GetString(1);
                    screens.Add(screen);
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
            return screens;
        }

        public _Screen getById(string screenId)//Lấy màn hình theo mã màn hình
        {
            _Screen screen = null;
            try
            {
                cnn.Open();
                string query = $"select mamanhinh, tenmanhinh from manhinh where mamanhinh = '{screenId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                if (reader.Read())
                {
                    screen = new _Screen();
                    screen.id = reader.GetString(0);
                    screen.name = reader.GetString(1);
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
            return screen;
        }
        public _Screen getByName(string screenName)//Lấy màn hình theo mã màn hình
        {
            _Screen screen = null;
            try
            {
                cnn.Open();
                string query = $"select mamanhinh, tenmanhinh from manhinh where tenmanhinh = N'{screenName}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                if (reader.Read())
                {
                    screen = new _Screen();
                    screen.id = reader.GetString(0);
                    screen.name = reader.GetString(1);
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
            return screen;
        }

        public void insertOne(_Screen screen)//Thêm màn hình
        {
            try
            {
                cnn.Open();
                string query = $@"insert into manhinh(mamanhinh, tenmanhinh)
                           values ('{screen.id}',N'{screen.name}')";
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

        public void updateOne(_Screen screen)//Cập nhật màn hình
        {
            try
            {
                cnn.Open();
                string query = $@"update manhinh set tenmanhinh = N'{screen.name}' where mamanhinh = '{screen.id}'";
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

        public void deleteById(string screenId)//Xoá màn hình theo mã màn hình
        {
            try
            {
                cnn.Open();
                string query = $@"delete from manhinh where mamanhinh = '{screenId}'";
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

        public string generateId()//Tạo mã màn hình
        {
            Methods methods = new Methods();
            string result = "";

            string str_now = DateTime.Now.Year.ToString().Substring(2, 2);//2 chữ số đầu tiên là năm hiện tại
            int i = 1;

            string str = str_now + methods.addZero(4, i);
            while (true)
            {
                if (getById(str) == null)
                {
                    result += str;
                    break;
                }
                else
                {
                    str = str_now + methods.addZero(4, ++i);
                }
            }

            return result;
        }
    }

    
}
