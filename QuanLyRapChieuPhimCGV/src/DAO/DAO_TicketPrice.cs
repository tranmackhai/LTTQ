using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_TicketPrice
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_TicketPrice()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<TicketPrice> getAll()//Lấy tất cả danh sách đồ ăn
        {
            List<TicketPrice> ticketPrices = new List<TicketPrice>();
            try
            {
                cnn.Open();
                string query = "select magia, tuthu, denthu, doituong, gia from giave";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    TicketPrice ticketPrice = new TicketPrice();
                    ticketPrice.id = reader.GetString(0);
                    ticketPrice.startDate = reader.GetInt32(1);
                    ticketPrice.endDate = reader.GetInt32(2);
                    ticketPrice.objectPerson = reader.GetString(3);
                    ticketPrice.price = reader.GetDecimal(4);
                    ticketPrices.Add(ticketPrice);
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
            return ticketPrices;
        }

        public List<TicketPrice> getAllByNow()//Lấy tất cả danh sách đồ ăn
        {
            List<TicketPrice> ticketPrices = new List<TicketPrice>();
            try
            {
                int now =(int) DateTime.Now.DayOfWeek == 0 ? 8 : (int)DateTime.Now.DayOfWeek + 1;
                cnn.Open();
                string query = $"select magia, tuthu, denthu, doituong, gia from giave where tuthu <= {now} and denthu >= {now}";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    TicketPrice ticketPrice = new TicketPrice();
                    ticketPrice.id = reader.GetString(0);
                    ticketPrice.startDate = reader.GetInt32(1);
                    ticketPrice.endDate = reader.GetInt32(2);
                    ticketPrice.objectPerson = reader.GetString(3);
                    ticketPrice.price = reader.GetDecimal(4);
                    ticketPrices.Add(ticketPrice);
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
            return ticketPrices;
        }

        public TicketPrice getById(string ticketPriceId)//Lấy đồ ăn theo mã đồ ăn
        {
            TicketPrice ticketPrice = null;
            try
            {
                cnn.Open();
                string query = $"select magia, tuthu, denthu, doituong, gia from giave where magia = '{ticketPriceId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    ticketPrice = new TicketPrice();
                    ticketPrice.id = reader.GetString(0);
                    ticketPrice.startDate = reader.GetInt32(1);
                    ticketPrice.endDate = reader.GetInt32(2);
                    ticketPrice.objectPerson = reader.GetString(3);
                    ticketPrice.price = reader.GetDecimal(4);
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
            return ticketPrice;
        }

        public void insertOne(TicketPrice ticketPrice)//Thêm đồ ăn
        {
            try
            {
                cnn.Open();
                string query = $@"insert into giave(magia, tuthu, denthu, doituong, gia)
                           values ('{ticketPrice.id}',{ticketPrice.startDate},{ticketPrice.endDate}, N'{ticketPrice.objectPerson}', {ticketPrice.price})";
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

        public void updateOne(TicketPrice ticketPrice)//Cập nhật đồ ăn
        {
            try
            {
                cnn.Open();
                string query = $@"update giave set tuthu = {ticketPrice.startDate}, denthu = {ticketPrice.endDate}, 
                    doituong = N'{ticketPrice.objectPerson}', gia = {ticketPrice.price} where magia = '{ticketPrice.id}'";
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

        public void deleteById(string ticketPriceId)//Xoá đồ ăn theo mã đồ ăn
        {
            try
            {
                cnn.Open();
                string query = $@"delete from giave where magia = '{ticketPriceId}'";
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
            string str_now = DateTime.Now.Year.ToString().Substring(2, 2);//2 chữ số đầu tiên là năm hiện tại

            int i = 1;

            string str = str_now + methods.addZero(3, i);
            while (true)
            {
                if (getById(str) == null)
                {
                    result += str;
                    break;
                }
                else
                {
                    str = str_now+ methods.addZero(3, ++i);
                }
            }

            return result;
        }
    }
}
