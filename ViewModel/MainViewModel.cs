using Movies.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Movies.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        //SETUP

        public ObservableCollection<Movie> movies { get; set; }       
        private MovieRepository repository;
        private Movie selectedmovie;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged( [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<Movie> Movies
        {
            get { return movies; }
            set
            {
                movies = value;
                OnPropertyChanged();
            }
        }

        public Movie SelectedMovie
        {
            get { return selectedmovie; }
            set
            {
                selectedmovie = value;
                OnPropertyChanged();
            }
        }

        //CODE FROM REPOSITORY BEING CALLED BASED ON USER ACTIONS

        public MainViewModel()
        {
            repository = new MovieRepository();
            repository.GetAllMovies();
            Movies = repository.movies;

        }

        public void AddMovie(string titel, int length, string genre)
        {
            var mov = new Movie(titel, length, genre);  
            repository.Add(mov);             
        }

        public void DeleteMovie()
        {
            repository.Remove(SelectedMovie);            
        }
        public void UpdateMovie()
        {
            repository.Update(SelectedMovie);
        }



    }
}
