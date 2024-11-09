using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Movies.Model
{
    public class MovieRepository
    {
        private string _connectionString;
        private Movie movie;
        public ObservableCollection<Movie> movies = new ObservableCollection<Movie>();

        public MovieRepository()
        {
            // Directly access the connection string here
            _connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
        }

        public void Add(Movie movie)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    // Use an inline SQL query instead of a stored procedure
                    string query = "INSERT INTO MoviesRevisited (Title, Length, Genre) VALUES (@Title, @Length, @Genre)";
                    SqlCommand cmd = new SqlCommand(query, con);

                    // Add parameters with their values
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = movie.Title;
                    cmd.Parameters.Add("@Length", SqlDbType.Int).Value = movie.Length;
                    cmd.Parameters.Add("@Genre", SqlDbType.NVarChar).Value = movie.Genre;

                    cmd.ExecuteNonQuery();
                    movies.Add(movie); // Add the movie to the collection if you want to maintain it locally
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
            }
        }
        public ObservableCollection<Movie> GetAllMovies()
        {

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                // Use a normal SQL query string to select all movies
                SqlCommand cmd = new SqlCommand("SELECT Id, Title, Length, Genre FROM MoviesRevisited", con);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Movie movie = new Movie(
                            dr["Title"].ToString(),
                            int.Parse(dr["Length"].ToString()),
                            dr["Genre"].ToString()
                        );
                        movie.Id = int.Parse(dr["Id"].ToString());
                        movies.Add(movie);
                    }
                }
            }
            return movies;

        }
        public void Remove(Movie movie)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                // Use an inline SQL query instead of a stored procedure
                SqlCommand cmd = new SqlCommand("DELETE FROM MoviesRevisited WHERE id = @Id", con);

                // Add parameter for the MovieId
                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = movie.Id });

                cmd.ExecuteNonQuery();
                movies.Remove(movie); // Remove the movie from the local collection
            }
        }
        public void Update(Movie movie)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open(); // Open the connection

                // Use an inline SQL query instead of a stored procedure
                SqlCommand cmd = new SqlCommand(
                    "UPDATE MoviesRevisited SET Title = @Title, Genre = @Genre, Length = @Length WHERE id = @Id",
                    con
                );

                // Add parameters for the SQL query
                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = movie.Id });
                cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar) { Value = movie.Title });
                cmd.Parameters.Add(new SqlParameter("@Genre", SqlDbType.NVarChar) { Value = movie.Genre });
                cmd.Parameters.Add(new SqlParameter("@Length", SqlDbType.Int) { Value = movie.Length });

                cmd.ExecuteNonQuery(); 
            }
        }

    }
}
