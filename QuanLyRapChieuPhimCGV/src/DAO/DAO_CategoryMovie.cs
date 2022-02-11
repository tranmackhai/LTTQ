using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_CategoryMovie
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_CategoryMovie()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<CategoryMovie> getAll()//Lấy tất cả danh sách thể loại
        {
            List<CategoryMovie> categoryMovies = new List<CategoryMovie>();
            try
            {
                cnn.Open();
                string query = "select maloai, tenloai from theloai";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    CategoryMovie categoryMovie = new CategoryMovie();
                    categoryMovie.id = reader.GetString(0);
                    categoryMovie.name = reader.GetString(1);
                    categoryMovies.Add(categoryMovie);
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
            return categoryMovies;
        }

        public CategoryMovie getById(string categoryMovieId)//Lấy thể loại theo mã thể loại
        {
            CategoryMovie categoryMovie = null;
            try
            {
                cnn.Open();
                string query = $"select maloai, tenloai from theloai where maloai = '{categoryMovieId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                if (reader.Read())
                {
                    categoryMovie = new CategoryMovie();
                    categoryMovie.id = reader.GetString(0);
                    categoryMovie.name = reader.GetString(1);
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
            return categoryMovie;
        }

        public void insertOne(CategoryMovie categoryMovie)//Thêm thể loại
        {
            try
            {
                cnn.Open();
                string query = $@"insert into theloai(maloai, tenloai)
                           values ('{categoryMovie.id}',N'{categoryMovie.name}')";
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

        public void updateOne(CategoryMovie categoryMovie)//Cập nhật thể loại
        {
            try
            {
                cnn.Open();
                string query = $@"update theloai set tenloai = N'{categoryMovie.name}' where maloai = '{categoryMovie.id}'";
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

        public void deleteById(string categoryMovieId)//Xoá thể loại theo mã thể loại
        {
            try
            {
                cnn.Open();
                string query = $@"delete from theloai where maloai = '{categoryMovieId}'";
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

        public string generateId()//Tạo mã thể loại
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
