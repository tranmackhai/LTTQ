using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_GroupFood
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_GroupFood()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<GroupFood> getAll()//Lấy tất cả danh sách nhóm đồ ăn
        {
            List<GroupFood> groupFoods = new List<GroupFood>();
            try
            {
                cnn.Open();
                string query = "select manhom, tennhom from nhomdoan";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    GroupFood groupFood = new GroupFood();
                    groupFood.id = reader.GetString(0);
                    groupFood.name = reader.GetString(1);
                    groupFoods.Add(groupFood);
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
            return groupFoods;
        }

        public GroupFood getById(string groupFoodId)//Lấy nhóm đồ ăn theo mã nhóm đồ ăn
        {
            GroupFood groupFood = null;
            try
            {
                cnn.Open();
                string query = $"select manhom, tennhom from nhomdoan where manhom = '{groupFoodId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                if (reader.Read())
                {
                    groupFood = new GroupFood();
                    groupFood.id = reader.GetString(0);
                    groupFood.name = reader.GetString(1);
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
            return groupFood;
        }
        public GroupFood getByName(string groupFoodName)//Lấy nhóm đồ ăn theo mã nhóm đồ ăn
        {
            GroupFood groupFood = null;
            try
            {
                cnn.Open();
                string query = $"select manhom, tennhom from nhomdoan where tennhom = N'{groupFoodName}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                if (reader.Read())
                {
                    groupFood = new GroupFood();
                    groupFood.id = reader.GetString(0);
                    groupFood.name = reader.GetString(1);
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
            return groupFood;
        }

        public void insertOne(GroupFood groupFood)//Thêm nhóm đồ ăn
        {
            try
            {
                cnn.Open();
                string query = $@"insert into nhomdoan(manhom, tennhom)
                           values ('{groupFood.id}',N'{groupFood.name}')";
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

        public void updateOne(GroupFood groupFood)//Cập nhật nhóm đồ ăn
        {
            try
            {
                cnn.Open();
                string query = $@"update nhomdoan set tennhom = N'{groupFood.name}' where manhom = '{groupFood.id}'";
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

        public void deleteById(string groupFoodId)//Xoá nhóm đồ ăn theo mã nhóm đồ ăn
        {
            try
            {
                cnn.Open();
                string query = $@"delete from nhomdoan where manhom = '{groupFoodId}'";
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

        public string generateId()//Tạo mã nhóm đồ ăn
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
