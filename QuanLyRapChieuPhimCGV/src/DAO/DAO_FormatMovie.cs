using QuanLyRapChieuPhimCGV.src.models;
using QuanLyRapChieuPhimCGV.src.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyRapChieuPhimCGV.src.DAO
{
    public class DAO_FormatMovie
    {
        private SqlConnection cnn;
        private SqlCommand scm;
        private SqlDataReader reader;
        public DAO_FormatMovie()
        {
            cnn = new ConnectDatabase().cnn;
        }

        public List<FormatMovie> getAllByMovie(string movieId)
        {
            List<FormatMovie> formatMovies = new List<FormatMovie>();
            try
            {
                DAO_Movie dao_m = new DAO_Movie();
                DAO_CategoryMovie dao_cm = new DAO_CategoryMovie();
                cnn.Open();
                string query = $"select maphim, maloai from phanloaiphim where maphim = '{movieId}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    FormatMovie formatMovie = new FormatMovie();
                    formatMovie.movie = dao_m.getById(reader.GetString(0));
                    formatMovie.category = dao_cm.getById(reader.GetString(1));
                    formatMovies.Add(formatMovie);
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
            return formatMovies;
        }

        public string getCategories(Movie movie)
        {
            string result = "";
            List<string> categories = new List<string>();
            try
            {
                DAO_Movie dao_m = new DAO_Movie();
                DAO_CategoryMovie dao_cm = new DAO_CategoryMovie();
                cnn.Open();
                string query = $"select t.tenloai from phanloaiphim p, theloai t where p.maloai = t.maloai and p.maphim = '{movie.id}'";
                scm = new SqlCommand(query, cnn);
                reader = scm.ExecuteReader();
                while (reader.Read())
                {
                    categories.Add(reader.GetString(0));
                }
                for(int i = 0; i < categories.Count; i++)
                {
                    if (i != categories.Count - 1)
                    {
                        result += categories[i] + ", ";
                    }
                    else
                    {
                        result += categories[i];
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
        public void insertOne(FormatMovie formatMovie)
        {
            try
            {
                cnn.Open();
                string query = $@"insert into phanloaiphim(maphim, maloai)
                           values ('{formatMovie.movie.id}','{formatMovie.category.id}')";
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

        public void deleteOne(string movieId, string categoryMovieId)
        {
            try
            {
                cnn.Open();
                string query = $@"delete from phanloaiphim where maphim = '{movieId}' and maloai = '{categoryMovieId}'";
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
