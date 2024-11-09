using Movies.Model;
using Movies.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Movies
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel mvm;

        public MainWindow()
        {
            mvm = new MainViewModel();
            DataContext = mvm;
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {            
            string titel = tb_Title.Text;
            int length = int.Parse(tb_Length.Text);
            string genre = tb_Genre.Text;
            mvm.AddMovie(titel, length, genre);
            ClearTextBoxes();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lv_Movie.SelectedItem != null)
            {
                mvm.SelectedMovie = (Movie)lv_Movie.SelectedItem;
            }
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (mvm.SelectedMovie != null)
            {
                MessageBoxResult result = MessageBox.Show("Denne handling kan ikke fortrydes. Er du sikker på du vil slette filmen?", "Slet Filmen", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    mvm.DeleteMovie();
                    ClearTextBoxes();
                }
                return;
            }
            else
            {
                MessageBox.Show("Vælg først en film du vil slette", "Vælg film", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btn_Update_Click(object sender, RoutedEventArgs e)
        {
            if (mvm.SelectedMovie != null)
            {
                MessageBoxResult result = MessageBox.Show("Er du sikker på du vil opdatere filmen", "Opdater Kunde", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes) mvm.UpdateMovie();
                ClearTextBoxes();
            }
            else
            {
                MessageBox.Show("Vælg først den film du vil opdatere", "Vælg film", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ClearTextBoxes()
        {
            tb_Title.Clear();
            tb_Length.Clear();
            tb_Genre.Clear();
        }

        
    }
}
