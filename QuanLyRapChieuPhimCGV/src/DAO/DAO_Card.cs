using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_Card
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_Card()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<Card> getAll()//Lấy tất cả danh sách thẻ
        {
            List<Card> cards = new List<Card>();
            try
            {
                cnn.Open();
                string query = "select mathe, tenthe, phantram_ve, phantram_doan from the";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    Card card = new Card();
                    card.id = reader.GetString(0);
                    card.name = reader.GetString(1);
                    card.percentForTicket = (float) reader.GetDouble(2);//dùng reader.GetFloat(2) bị lỗi nên dùng cách này
                    card.percentForFood = (float) reader.GetDouble(3);
                    cards.Add(card);
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
            return cards;
        }

        public Card getById(string cardId)//Lấy thẻ theo mã thẻ
        {
            Card card = null;
            try
            {
                cnn.Open();
                string query = $"select mathe, tenthe, phantram_ve, phantram_doan from the where mathe = '{cardId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    card = new Card();
                    card.id = reader.GetString(0);
                    card.name = reader.GetString(1);
                    card.percentForTicket = (float)reader.GetDouble(2);//dùng reader.GetFloat(2) bị lỗi nên dùng cách này
                    card.percentForFood = (float)reader.GetDouble(3);
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
            return card;
        }

        public void insertOne(Card card)//Thêm thẻ
        {
            try
            {
                cnn.Open();
                string query = $@"insert into the(mathe, tenthe, phantram_ve, phantram_doan)
                           values ('{card.id}',N'{card.name}',{card.percentForTicket}, {card.percentForFood})";
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

        public void updateOne(Card card)//Cập nhật thẻ
        {
            try
            {
                cnn.Open();
                string query = $@"update the set tenthe = N'{card.name}', phantram_ve = {card.percentForTicket}, 
                            phantram_doan = {card.percentForFood} where mathe = '{card.id}'";
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

        public void deleteById(string cardId)//Xoá loại ghế theo mã loại ghế
        {
            try
            {
                cnn.Open();
                string query = $@"delete from the where mathe = '{cardId}'";
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

        public string generateId()//Tạo mã thẻ
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
