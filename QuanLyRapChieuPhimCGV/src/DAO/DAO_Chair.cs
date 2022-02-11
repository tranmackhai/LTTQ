using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_Chair
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_Chair()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<Chair> getAll()//Lấy tất cả danh sách ghế
        {
            List<Chair> chairs = new List<Chair>();
            try
            {
                DAO_ChairType dao_ct = new DAO_ChairType();
                DAO_Room dao_r = new DAO_Room();
                cnn.Open();
                string query = "select maghe, vitrihang, vitricot, maphong, maloai from ghe";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    Chair chair = new Chair();
                    chair.id = reader.GetString(0);
                    chair.row = reader.GetInt32(1);
                    chair.column = reader.GetInt32(2);
                    chair.room = dao_r.getById(reader.GetString(3));
                    chair.type = dao_ct.getById(reader.GetString(4));
                    chairs.Add(chair);
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
            return chairs;
        }

        public string getChairName(Chair chair)
        {
            List<Chair> a = getAllInRow(chair.row, chair.room);
            int i = 1;
            int index = a.FindIndex(ch => ch.id == chair.id);
            while (i<=index)
            {
                if(chair.column == i)
                {
                    break;
                }

                i++;
            }

            return "" + (char)(65 + chair.row - 1) + " - " + i;
        }
        public List<Chair> getAllInRow(int row, Room room)//Lấy tất cả danh sách ghế trong 1 hàng trong rạp
        {
            List<Chair> chairs = new List<Chair>();
            try
            {
                DAO_ChairType dao_ct = new DAO_ChairType();
                DAO_Room dao_r = new DAO_Room();
                cnn.Open();
                string query = $"select maghe, vitrihang, vitricot, maphong, maloai from ghe where maphong = '{room.id}' and vitrihang = {row} order by vitricot";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    Chair chair = new Chair();
                    chair.id = reader.GetString(0);
                    chair.row = reader.GetInt32(1);
                    chair.column = reader.GetInt32(2);
                    chair.room = dao_r.getById(reader.GetString(3));
                    chair.type = dao_ct.getById(reader.GetString(4));
                    chairs.Add(chair);
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
            return chairs;
        }

        public bool isBooked(Schedule schedule, Chair chair)
        {
            bool result = false;
            if (schedule != null)
            {
                try
                {
                    DAO_ChairType dao_ct = new DAO_ChairType();
                    DAO_Room dao_r = new DAO_Room();
                    cnn.Open();
                    string query = $@"SELECT *
                            FROM VE V, LICHCHIEU L, GHE G
                            WHERE V.MALICH = L.MALICH AND 
	                            G.MAPHONG = L.MAPHONG AND 
	                            L.MALICH = '{schedule.id}' AND 
	                            G.MAGHE = '{chair.id}' AND 
	                            V.MAGHE = G.MAGHE";
                    scm = new SqlCommand(query, cnn);
                    reader = scm.ExecuteReader();
                    if (reader.Read())
                    {
                        result = true;
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
            }
            return result;
        }
        public List<Chair> getAllByRoom(Room room)//Lấy tất cả danh sách ghế của 1 rạp
        {
            List<Chair> chairs = new List<Chair>();
            try
            {
                DAO_ChairType dao_ct = new DAO_ChairType();
                DAO_Room dao_r = new DAO_Room();
                cnn.Open();
                string query = $"select maghe, vitrihang, vitricot, maphong, maloai from ghe where maphong = '{room.id}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    Chair chair = new Chair();
                    chair.id = reader.GetString(0);
                    chair.row = reader.GetInt32(1);
                    chair.column = reader.GetInt32(2);
                    chair.room = dao_r.getById(reader.GetString(3));
                    chair.type = dao_ct.getById(reader.GetString(4));
                    chairs.Add(chair);
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
            return chairs;
        }

        public Chair getById(string chairId)//Lấy ghế theo mã ghế
        {
            Chair chair = null;
            try
            {
                DAO_ChairType dao_ct = new DAO_ChairType();
                DAO_Room dao_r = new DAO_Room();
                cnn.Open();
                string query = $"select maghe, vitrihang, vitricot, maphong, maloai from ghe where maghe = '{chairId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    chair = new Chair();
                    chair.id = reader.GetString(0);
                    chair.row = reader.GetInt32(1);
                    chair.column = reader.GetInt32(2);
                    chair.room = dao_r.getById(reader.GetString(3));
                    chair.type = dao_ct.getById(reader.GetString(4));
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
            return chair;
        }

        public void insertOne(Chair chair)//Thêm ghế
        {
            try
            {
                cnn.Open();
                string query = $@"insert into ghe(maghe, vitrihang, vitricot, maphong, maloai)
                           values ('{chair.id}',{chair.row}, {chair.column}, '{chair.room.id}', '{chair.type.id}')";
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

        public void updateOne(Chair chair)//Cập nhật loại ghế
        {
            try
            {
                cnn.Open();
                string query = $@"update ghe set vitrihang = {chair.row}, vitricot = {chair.column},
                    maphong = '{chair.room.id}', maloai = '{chair.type.id}' where maghe = '{chair.id}'";
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


        public void deleteById(string chairId)//Xoá loại ghế theo mã loại ghế
        {
            try
            {
                cnn.Open();
                string query = $@"delete from ghe where maloai = '{chairId}'";
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

        public string generateId()//Tạo mã loại ghế
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
