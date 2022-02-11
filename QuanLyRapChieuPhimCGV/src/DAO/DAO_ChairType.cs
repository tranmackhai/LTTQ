using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_ChairType
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_ChairType()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<ChairType> getAll()//Lấy tất cả danh sách loại ghế
        {
            List<ChairType> chairTypes = new List<ChairType>();
            try
            {
                cnn.Open();
                string query = "select maloaighe, tenloai from loaighe";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    ChairType chairType = new ChairType();
                    chairType.id = reader.GetString(0);
                    chairType.name = reader.GetString(1);
                    chairTypes.Add(chairType);
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
            return chairTypes;
        }

        public List<ChairType> getAllInRoom(Room room)//Lấy tất cả loại ghế trong phòng
        {
            List<ChairType> chairTypes = new List<ChairType>();
            try
            {
                cnn.Open();
                string query = $@"SELECT L.MALOAIGHE, L.TENLOAI
                                FROM LOAIGHE L, GHE G, PHONGCHIEU P
                                WHERE L.MALOAIGHE = G.MALOAI AND
	                                G.MAPHONG = P.MAPHONG AND
	                                P.MAPHONG = '{room.id}'
                                GROUP BY L.MALOAIGHE, L.TENLOAI";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    ChairType chairType = new ChairType();
                    chairType.id = reader.GetString(0);
                    chairType.name = reader.GetString(1);
                    chairTypes.Add(chairType);
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
            return chairTypes;
        }

        public ChairType getById(string chairTypeId)//Lấy loại ghế theo mã loại ghế
        {
            ChairType chairType = null;
            try
            {
                cnn.Open();
                string query = $"select maloaighe, tenloai from loaighe where maloaighe = '{chairTypeId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    chairType = new ChairType();
                    chairType.id = reader.GetString(0);
                    chairType.name = reader.GetString(1);
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
            return chairType;
        }

        public void insertOne(ChairType chairType)//Thêm loại ghế
        {
            try
            {
                cnn.Open();
                string query = $@"insert into loaighe(maloaighe, tenloai)
                           values ('{chairType.id}',N'{chairType.name}')";
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

        public void updateOne(ChairType chairType)//Cập nhật loại ghế
        {
            try
            {
                cnn.Open();
                string query = $@"update loaighe set tenloai = N'{chairType.name}' where maloaighe = '{chairType.id}'";
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

        public void deleteById(string chairTypeId)//Xoá loại ghế theo mã loại ghế
        {
            try
            {
                cnn.Open();
                string query = $@"delete from loaighe where maloaighe = '{chairTypeId}'";
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
