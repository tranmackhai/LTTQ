using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_Bill
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_Bill()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<Bill> getAll()//Lấy tất cả danh sách đồ ăn
        {
            List<Bill> bills = new List<Bill>();
            try
            {
                DAO_Customer dao_cus = new DAO_Customer();
                DAO_Employee dao_e = new DAO_Employee();
                cnn.Open();
                string query = "select sohd, ngayhd, makhach, manv, tongtien, diem from hoadon";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    Bill bill = new Bill();
                    bill.id = reader.GetString(0);
                    bill.date = reader.GetDateTime(1);
                    bill.customer = reader.IsDBNull(2) ? null :dao_cus.getById(reader.GetString(2));
                    bill.employee = dao_e.getById(reader.GetString(3));
                    bill.totalPrice = reader.GetDecimal(4);
                    bill.point = reader.GetInt32(5);
                    bills.Add(bill);
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
            return bills;
        }

        public Bill getById(string billId)//Lấy đồ ăn theo mã đồ ăn
        {
            Bill bill = null;
            try
            {
                DAO_Customer dao_cus = new DAO_Customer();
                DAO_Employee dao_e = new DAO_Employee();
                cnn.Open();
                string query = $"select sohd, ngayhd, makhach, manv, tongtien, diem from hoadon where sohd = '{billId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    bill = new Bill();
                    bill.id = reader.GetString(0);
                    bill.date = reader.GetDateTime(1);
                    bill.customer = reader.IsDBNull(2) ? null : dao_cus.getById(reader.GetString(2));
                    bill.employee = dao_e.getById(reader.GetString(3));
                    bill.totalPrice = reader.GetDecimal(4);
                    bill.point = reader.GetInt32(5);
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
            return bill;
        }

        public void insertOne(Bill bill)//Thêm đồ ăn
        {
            try
            {

                string query = "";
                if (bill.customer == null)
                {
                    query = $@"insert into hoadon(sohd, ngayhd, manv, tongtien, diem) values 
                           ('{bill.id}','{bill.date.ToString("yyyy-MM-dd")}','{bill.employee.id}', {bill.totalPrice}, {bill.point})";
                }
                else
                {
                    query = $@"insert into hoadon(sohd, ngayhd, manv, makhach, tongtien, diem) values 
                          ('{bill.id}','{bill.date.ToString("yyyy-MM-dd")}','{bill.employee.id}', '{bill.customer.id}', {bill.totalPrice}, {bill.point})";
                    Console.WriteLine(query);
                }
                cnn.Open();

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

        public void updateOne(Bill bill)//Cập nhật đồ ăn
        {
            try
            {
                string query = "";
                cnn.Open();
                if (bill.customer == null)
                {
                    query = $@"update hoadon set ngayhd = '{bill.date.ToString("yyyy-MM-dd")}', manv = '{bill.employee.id}', 
                        tongtien = {bill.totalPrice},diem = {bill.point} where sohd = '{bill.id}'";
                }
                else
                {
                    query = $@"update hoadon set ngayhd = '{bill.date.ToString("yyyy-MM-dd")}', manv = '{bill.employee.id}', 
                        makhach = '{bill.customer.id}',diem = {bill.point},  tongtien = {bill.totalPrice} where sohd = '{bill.id}'";
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

        public void deleteById(string billId)//Xoá đồ ăn theo mã đồ ăn
        {
            try
            {
                cnn.Open();
                string query = $@"delete from hoadon where sohd = '{billId}'";
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
