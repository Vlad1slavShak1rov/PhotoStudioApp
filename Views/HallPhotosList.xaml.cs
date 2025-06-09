using PhotoStudioApp.Model;
using PhotoStudioApp.Service;
using PhotoStudioApp.Windows;
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
    /// Логика взаимодействия для HallPhotosList.xaml
    /// </summary>
    public partial class HallPhotosList : UserControl
    {
        public HallPhotosList()
        {
            InitializeComponent();
            InitData();
        }

        private async void InitData()
        {
            HallApiService hallApiService = new();
            cbHalls.ItemsSource = await hallApiService.GetAll();
            cbHalls.SelectedIndex = 0;
            cbHalls.DisplayMemberPath = "Description";
        }

        private async Task LoadView(Hall hall)
        {
            imagePanel.Children.Clear();
            HallPhotoService hallPhotoApi = new();
            var hallsPhoto = await hallPhotoApi.GetByHallId(hall.ID);
            if (hallsPhoto == null) return;
            foreach (var hallphoto in hallsPhoto)
            {
                HallsPhoto hallsPhotoView = new(hall, hallphoto);
                hallsPhotoView.Update += HallsPhotoView_Update;
                hallsPhotoView.Margin = new Thickness(0, 0, 10, 0);
                imagePanel.Children.Add(hallsPhotoView);
            }
        }

        private void HallsPhotoView_Update(object? sender, EventArgs e)
        {
            InitData();
        }

        private async void cbHalls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedHall = cbHalls.SelectedItem as Hall;
            if (selectedHall != null)
            {
                await LoadView(selectedHall);
            }
        }

        private void btAddHall_Click(object sender, RoutedEventArgs e)
        {
            var selectedHall = cbHalls.SelectedItem as Hall;
      
            ChooseActionWindow chooseActionWindow  =  new ChooseActionWindow();
            AddUpdateHallWindow addHallPhotoWindow;

            if (chooseActionWindow.ShowDialog() == true)
            {
                if (chooseActionWindow.IsCreateNew)
                    addHallPhotoWindow = new AddUpdateHallWindow();
                else
                    addHallPhotoWindow = new AddUpdateHallWindow(selectedHall);
            }
            else
            {
                return;
            }

            var result = addHallPhotoWindow.ShowDialog();
            if (result == true)
            {
                InitData();
            }
        }

        private void btAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            var selectedHall = cbHalls.SelectedItem as Hall;
            AddPhotoHallWindow add = new(selectedHall.ID);
            if(add.ShowDialog() == true)
            {
                InitData();
            }
        }
    }
}
