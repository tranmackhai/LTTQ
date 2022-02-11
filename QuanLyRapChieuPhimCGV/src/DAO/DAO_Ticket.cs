using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_Ticket
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_Ticket()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<Ticket> getAll()//Lấy tất cả danh sách đồ ăn
        {
            List<Ticket> tickets = new List<Ticket>();
            try
            {
                DAO_Customer dao_cus = new DAO_Customer();
                DAO_Schedule dao_s = new DAO_Schedule();
                DAO_Chair dao_ch = new DAO_Chair();
                DAO_TicketPrice dao_tp = new DAO_TicketPrice();
                DAO_Employee dao_e = new DAO_Employee();
                cnn.Open();
                string query = "select sove, malich, makhach, manv, magia, maghe, tongtien, ngaylap, diem from ve";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    Ticket ticket = new Ticket();
                    ticket.id = reader.GetString(0);
                    ticket.schedule = dao_s.getById(reader.GetString(1));
                    ticket.customer = reader.IsDBNull(2) ? null :dao_cus.getById(reader.GetString(2));
                    ticket.employee = dao_e.getById(reader.GetString(3));
                    ticket.price = dao_tp.getById(reader.GetString(4));
                    ticket.chair = dao_ch.getById(reader.GetString(5));
                    ticket.totalPrice = reader.GetDecimal(6);
                    ticket.date = reader.GetDateTime(7);
                    ticket.point = reader.GetInt32(8);
                    tickets.Add(ticket);
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
            return tickets;
        }

        public Ticket getById(string ticketId)//Lấy đồ ăn theo mã đồ ăn
        {
            Ticket ticket = null;
            try
            {
                DAO_Customer dao_cus = new DAO_Customer();
                DAO_Schedule dao_s = new DAO_Schedule();
                DAO_Chair dao_ch = new DAO_Chair();
                DAO_TicketPrice dao_tp = new DAO_TicketPrice();
                DAO_Employee dao_e = new DAO_Employee();
                cnn.Open();
                string query = $"select sove, malich, makhach, manv, magia, maghe, tongtien, ngaylap, diem from ve where sove = '{ticketId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    ticket = new Ticket();
                    ticket.id = reader.GetString(0);
                    ticket.schedule = dao_s.getById(reader.GetString(1));
                    ticket.customer = reader.IsDBNull(2) ? null :dao_cus.getById(reader.GetString(2));
                    ticket.employee = dao_e.getById(reader.GetString(3));
                    ticket.price = dao_tp.getById(reader.GetString(4));
                    ticket.chair = dao_ch.getById(reader.GetString(5));
                    ticket.totalPrice = reader.GetDecimal(6);
                    ticket.date = reader.GetDateTime(7);
                    ticket.point = reader.GetInt32(8);
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
            return ticket;
        }

        public void insertOne(Ticket ticket)//Thêm đồ ăn
        {
            try
            {
                string query = "";
                cnn.Open();
                if (ticket.customer == null)
                {
                    query = $@"insert into ve(sove, malich, manv, maghe, magia, tongtien, ngaylap, diem)
                           values ('{ticket.id}','{ticket.schedule.id}','{ticket.employee.id}', 
                        '{ticket.chair.id}', '{ticket.price.id}', {ticket.totalPrice}, '{ticket.date}', {ticket.point})";
                }
                else
                {
                    query = $@"insert into ve(sove, malich, makhach, manv, maghe, magia, tongtien, ngaylap, diem)
                           values ('{ticket.id}','{ticket.schedule.id}','{ticket.customer.id}','{ticket.employee.id}', 
                        '{ticket.chair.id}', '{ticket.price.id}', {ticket.totalPrice}, '{ticket.date}', {ticket.point})";
                }
                
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

        public void updateOne(Ticket ticket)//Cập nhật đồ ăn
        {
            try
            {
                cnn.Open();
                string query = "";
                if(ticket.customer != null)
                {
                    query = $@"update ve set malich = '{ticket.schedule.id}', manv = '{ticket.employee.id}', makhach = '{ticket.customer.id}', ngaylap = '{ticket.date}',
                        maghe = '{ticket.chair.id}', magia = '{ticket.price.id}', diem = {ticket.point}, tongtien = {ticket.totalPrice} where sove = '{ticket.id}'";
                }
                else
                {
                    query = $@"update ve set malich = '{ticket.schedule.id}', manv = '{ticket.employee.id}', makhach = null,  ngaylap = '{ticket.date}',
                        maghe = '{ticket.chair.id}', magia = '{ticket.price.id}', diem = {ticket.point}, tongtien = {ticket.totalPrice} where sove = '{ticket.id}'";
                }
                
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

        public void deleteById(string ticketId)//Xoá đồ ăn theo mã đồ ăn
        {
            try
            {
                cnn.Open();
                string query = $@"delete from ve where sove = '{ticketId}'";
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

            string str = str_now+ methods.addZero(7, i);
            while (true)
            {
                if (getById(str) == null)
                {
                    result += str;
                    break;
                }
                else
                {
                    str = str_now+methods.addZero(7, ++i);
                }
            }

            return result;
        }
    }

    
}
