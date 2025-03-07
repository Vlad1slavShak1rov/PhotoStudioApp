using PhotoStudioApp.Model;
using PhotoStudioApp.Views;
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
using System.Windows.Shapes;

namespace PhotoStudioApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User _user;
        private SettingsView settingsView;
        private CreateBooking createBooking;
        public MainWindow(User user)
        {
            InitializeComponent();
            _user = user;

            if ((Enums.Role)_user.Role == Enums.Role.Customer) CustomerGrid.Visibility = Visibility.Visible;
            else if ((Enums.Role)_user.Role == Enums.Role.Worker) WorkerGrid.Visibility = Visibility.Visible;
            else AdminGrid.Visibility = Visibility.Visible;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new();
            loginWindow.Show();
            this.Close();
        }

        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();
            createBooking = new(_user);
            createBooking.CloseButton += CreateBooking_CloseButton;

            createBooking.VerticalAlignment = VerticalAlignment.Top;
            createBooking.HorizontalAlignment = HorizontalAlignment.Center;
            createBooking.Width = 800;
            createBooking.Height = 450;
            createBooking.Margin = new Thickness(0,50,0,0);

            MainGrid.Children.Add(createBooking);
        }

        //Освобождаем память
        private void CreateBooking_CloseButton(object? sender, EventArgs e)
        {
            createBooking.CloseButton -= CreateBooking_CloseButton;
            createBooking = null;
            MainGrid.Children.Clear();
        }
        private void ListBookingButton_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();
            MyBooking myBooking = new(_user);
            MainGrid.Children.Add(myBooking);
        }

        private void ListServicesButton_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();
            ListServices ListServices = new(false);
            MainGrid.Children.Add(ListServices);
        }

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Clear();
            ViewsList viewsList = new(_user);
            MainGrid.Children.Add(viewsList);
        }

        private void WorkerListBookingButton_Click(object sender, RoutedEventArgs e)
        {
            WorkerMainPanel.Children.Clear();
            MyBooking myBooking = new(_user);
            WorkerMainPanel.Children.Add(myBooking);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            WorkerMainPanel.Children.Clear();
            settingsView = new(_user);
            settingsView.HorizontalAlignment = HorizontalAlignment.Center;
            settingsView.VerticalAlignment = VerticalAlignment.Center;
            settingsView.Width = 350;
            settingsView.Height = 350;
            settingsView.Margin = new Thickness(0,10,0,0);
            settingsView.CloseClick += SettingsView_CloseClick;
            WorkerMainPanel.Children.Add(settingsView);
        }

        //Освобождаем память
        private void SettingsView_CloseClick(object? sender, EventArgs e)
        {
            settingsView.CloseClick -= SettingsView_CloseClick;
            settingsView = null;
            WorkerMainPanel.Children.Clear();
        }

        private void AllBooking_Click(object sender, RoutedEventArgs e)
        {
            AdminMainPanel.Children.Clear();
            MyBooking myBooking = new(_user);
            AdminMainPanel.Children.Add(myBooking);
        }

        private void AllWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            AdminMainPanel.Children.Clear();
            WorkerListView workerListView = new();
            AdminMainPanel.Children.Add(workerListView);
        }

        private void AllServiceButton_Click(object sender, RoutedEventArgs e)
        {
            AdminMainPanel.Children.Clear();
            ListServices ListServices = new(true);
            AdminMainPanel.Children.Add(ListServices);
        }
    }
}
