using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_Food
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_Food()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<Food> getAll()//Lấy tất cả danh sách đồ ăn
        {
            List<Food> foods = new List<Food>();
            try
            {
                DAO_GroupFood dao_g = new DAO_GroupFood();
                cnn.Open();
                string query = "select mada, ten, giaban, manhom from doan";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    Food food = new Food();
                    food.id = reader.GetString(0);
                    food.name = reader.GetString(1);
                    food.price = reader.GetDecimal(2);
                    food.groupFood = dao_g.getById(reader.GetString(3));
                    foods.Add(food);
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
            return foods;
        }
        public List<Food> getAllByGroup(GroupFood groupFood)//Lấy tất cả danh sách đồ ăn theo nhóm
        {
            List<Food> foods = new List<Food>();
            try
            {
                DAO_GroupFood dao_g = new DAO_GroupFood();
                cnn.Open();
                string query = $"select mada, ten, giaban, manhom from doan where manhom = '{groupFood.id}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    Food food = new Food();
                    food.id = reader.GetString(0);
                    food.name = reader.GetString(1);
                    food.price = reader.GetDecimal(2);
                    food.groupFood = dao_g.getById(reader.GetString(3));
                    foods.Add(food);
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
            return foods;
        }
        public Food getById(string foodId)//Lấy đồ ăn theo mã đồ ăn
        {
            Food food = null;
            try
            {
                DAO_GroupFood dao_g = new DAO_GroupFood();
                cnn.Open();
                string query = $"select mada, ten, giaban, manhom from doan where mada = '{foodId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    food = new Food();
                    food.id = reader.GetString(0);
                    food.name = reader.GetString(1);
                    food.price = reader.GetDecimal(2);
                    food.groupFood = dao_g.getById(reader.GetString(3));
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
            return food;
        }

        public void insertOne(Food food)//Thêm đồ ăn
        {
            try
            {
                cnn.Open();
                string query = $@"insert into doan(mada, ten, giaban, manhom)
                           values ('{food.id}',N'{food.name}',{food.price}, '{food.groupFood.id}')";
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

        public void updateOne(Food food)//Cập nhật đồ ăn
        {
            try
            {
                cnn.Open();
                string query = $@"update doan set ten = N'{food.name}', giaban = {food.price}, 
                    manhom = '{food.groupFood.id}' where mada = '{food.id}'";
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

        public void deleteById(string foodId)//Xoá đồ ăn theo mã đồ ăn
        {
            try
            {
                cnn.Open();
                string query = $@"delete from doan where mada = '{foodId}'";
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

        public string generateId()//Tạo mã đồ ăn
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
