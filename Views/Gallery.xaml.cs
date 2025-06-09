using PhotoStudioApp.Model;
using PhotoStudioApp.Service;
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

namespace PhotoStudioApp.Views
{
    /// <summary>
    /// Логика взаимодействия для Gallery.xaml
    /// </summary>
    public partial class Gallery : UserControl
    {
        public Gallery()
        {
            InitializeComponent();
            rbHall.IsChecked = true;
        }

        private async void rbHall_Checked(object sender, RoutedEventArgs e)
        {
            if(rbHall.IsChecked == true)
            {
                hallFiltrationPanel.Visibility = Visibility.Visible;
                workerFiltrationPanel.Visibility = Visibility.Collapsed;
                await InitHalls();
            }
            else
            {
                workerFiltrationPanel.Visibility= Visibility.Visible;
                hallFiltrationPanel.Visibility = Visibility.Collapsed;
                await InitWorks();
            }
        }

        private async Task InitHalls()
        {
            HallApiService hallApiService = new();
            var halls = await hallApiService.GetAll();
            InitComboBox<Hall>(cbHalls, halls, "Description");
        }

        private async Task InitWorks()
        {
            WorkerApiService workerApiService = new();
            var workers = await workerApiService.GetAll();
            InitComboBox<Worker>(cbWorker, workers, "FullName");
        }

        private void InitComboBox<T>(ComboBox comboBox, List<T> lists, string DisplayMember)
        {
            photoPanel.Children.Clear();
            comboBox.ItemsSource = lists;
            comboBox.SelectedIndex = 0;
            comboBox.DisplayMemberPath = DisplayMember;
        }

        private async void cbHalls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            photoPanel.Children.Clear();
            var selectedItem = cbHalls.SelectedItem as Hall;
            if (selectedItem != null)
            {
                HallPhotoService hallPhotoService =new();
                var hallPhotos = await hallPhotoService.GetByHallId(selectedItem.ID);
                if (hallPhotos == null) return;
                foreach(var hp in hallPhotos)
                {
                    HallsPhoto hallphoto = new(selectedItem, hp, true);
                    hallphoto.Margin = new Thickness(0,0,10,0);
                    photoPanel.Children.Add(hallphoto);
                }
            }
        }

        private async void cbWorker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            photoPanel.Children.Clear();
            var selectedItem = (Worker)cbWorker.SelectedItem;
            if(selectedItem != null)
            {
                WorkerPhotoWorks workerPhotoWorks = new();
                var workerPhoto = await workerPhotoWorks.GetByWorkerId(selectedItem.ID);
                foreach(var photo in workerPhoto)
                {
                    MyWorksCardView myWorksCardView = new(photo, true);
                    photoPanel.Children.Add(myWorksCardView);
                }
            }
        }
    }
}
