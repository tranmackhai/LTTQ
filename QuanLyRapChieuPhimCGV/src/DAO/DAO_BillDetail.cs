using QuanLyRapChieuPhimCGV.src.models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_BillDetail
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_BillDetail()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<BillDetail> getAll(Bill bill)//Lấy tất cả chi tiết hoá đơn của hoá đơn
        {
            List<BillDetail> billDetails = new List<BillDetail>();
            try
            {
                DAO_Food dao_f = new DAO_Food();
                cnn.Open();
                string query = $"select * from chitiethoadon where sohd = '{bill.id}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    BillDetail billDetail = new BillDetail()
                    {
                        food = dao_f.getById(reader.GetString(1)),
                        quantity = reader.GetInt32(2)
                    };

                    billDetails.Add(billDetail);
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
            return billDetails;
        }
        public void deleteAll(string billId)
        {
            try
            {
                cnn.Open();
                string query = $@"delete from chitiethoadon where sohd = '{billId}')";
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

        public void insertOne(string billId, BillDetail billDetail)
        {
            try
            {
                cnn.Open();
                string query = $@"insert into chitiethoadon(mada, sohd, soluong) values('{billDetail.food.id}','{billId}', {billDetail.quantity})";
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

        public void updateOne(string billId, BillDetail billDetail)
        {
            try
            {
                cnn.Open();
                string query = query = $@"update chitiethoadon set soluong = {billDetail.quantity}, 
                        where sohd = '{billId}' and mada = '{billDetail.food.id}'";
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

        public void deleteOne(string billId, string foodId)
        {
            try
            {
                cnn.Open();
                string query = $@"delete from chitiethoadon where sohd = '{billId}' and mada = '{foodId}'";
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
    }

    
}
