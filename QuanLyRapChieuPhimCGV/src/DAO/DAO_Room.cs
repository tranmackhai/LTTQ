using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_Room
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_Room()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<Room> getAll()//Lấy tất cả danh sách phòng chiếu
        {
            List<Room> rooms = new List<Room>();
            try
            {
                DAO_Screen dao_s = new DAO_Screen();
                cnn.Open();
                string query = "select maphong, tenphong, tongsohang, tongsocot from phongchieu";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    Room room = new Room();
                    room.id = reader.GetString(0);
                    room.name = reader.GetString(1);
                    room.totalRows = reader.GetInt32(2);
                    room.totalColumns = reader.GetInt32(3);
                    rooms.Add(room);
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
            return rooms;
        }

        public Room getById(string roomId)//Lấy phòng chiếu theo mã phòng chiếu
        {
            Room room = null;
            try
            {
                DAO_Screen dao_s = new DAO_Screen();
                cnn.Open();
                string query = $"select maphong, tenphong, tongsohang, tongsocot from phongchieu where maphong = '{roomId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    room = new Room();
                    room.id = reader.GetString(0);
                    room.name = reader.GetString(1);
                    room.totalRows = reader.GetInt32(2);
                    room.totalColumns = reader.GetInt32(3);
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
            return room;
        }

        public void insertOne(Room room)//Thêm phòng chiếu
        {
            try
            {
                cnn.Open();
                string query = $@"insert into phongchieu(maphong, tenphong, tongsohang, tongsocot)
                           values ('{room.id}',N'{room.name}',{room.totalRows}, {room.totalColumns})";
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
        public bool isAllowedUpdate(Room room)
        {
            bool result = true;
            try
            {
                cnn.Open();
                string query = $"SELECT COUNT(MAGHE) FROM GHE WHERE (VITRICOT > {room.totalColumns} OR VITRIHANG > {room.totalRows}) AND MAPHONG = '{room.id}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                if (reader.Read())
                {
                   if(reader.GetInt32(0) > 0)
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
        public void updateOne(Room room)//Cập nhật phòng chiếu
        {
            try
            {
                cnn.Open();
                string query = $@"update phongchieu set tenphong = N'{room.name}', tongsohang = {room.totalRows}, 
                    tongsocot = {room.totalColumns} where maphong = '{room.id}'";
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

        public void deleteById(string roomId)//Xoá phòng chiếu theo mã phòng chiếu
        {
            try
            {
                cnn.Open();
                string query = $@"delete from phongchieu where maphong = '{roomId}'";
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

        public string generateId()//Tạo mã phòng chiếu
        {
            Methods methods = new Methods();
            string result = "";

            int i = 1;

            string str = methods.addZero(3, i);
            while (true)
            {
                if (getById(str) == null)
                {
                    result += str;
                    break;
                }
                else
                {
                    str = methods.addZero(3, ++i);
                }
            }

            return result;
        }
    }

    
}
